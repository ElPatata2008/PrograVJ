using NAudio.Dmo.Effect;
using NAudio.Gui;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrograVJ.Engine.Manager
{
    public static class AudioManager
    {
        private class SFXCache
        {
            public byte[] AudioData;
            public WaveFormat waveFormat;

            public SFXCache(string FilePath)
            {
                using (var reader = new AudioFileReader(FilePath))
                {
                    waveFormat = reader.WaveFormat;

                    var pcmData = new List<byte>((int)reader.Length);
                    var buffer = new byte[reader.WaveFormat.AverageBytesPerSecond];
                    int bytesRead;

                    while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        pcmData.AddRange(buffer.Take(bytesRead));
                    }

                    AudioData = pcmData.ToArray();
                }
            }
        }


        private static Dictionary<string, string> music = new Dictionary<string, string>();

        // CAMBIO: WaveOut -> WaveOutEvent (no usa ventana oculta, se puede liberar desde cualquier hilo)
        private static WaveOutEvent musicOutput;
        private static MixingSampleProvider musicMixer;
        private static Dictionary<int, (AudioFileReader reader, FadeInOutSampleProvider fader, bool loops)> musicTracks = new Dictionary<int, (AudioFileReader reader, FadeInOutSampleProvider fader, bool loops)>();

        private static Dictionary<string, (AudioFileReader reader, FadeInOutSampleProvider fader)> musicLayers = new Dictionary<string, (AudioFileReader reader, FadeInOutSampleProvider fader)>();

        private static Dictionary<string, SFXCache> sfxMap = new Dictionary<string, SFXCache>();

        private static Dictionary<int, (WaveOutEvent player, IDisposable reader)> PlayingAudioMap = new Dictionary<int, (WaveOutEvent player, IDisposable reader)>();
        private static int? PlayingMusic = null;
        private static int PlayingIndex = 0;
        private static string audioPath = "Assets/Audios/";
        private static string musicPath = "Assets/Music/";

        public static void LoadMusic(string filename, string id)
        {
            string file = Path.Combine(musicPath, filename);
            if (!File.Exists(file)) throw new FileNotFoundException(file);
            if (music.ContainsKey(id)) throw new ArgumentException(id);
            music.Add(id, file);
        }

        public static int PlayMusic(string id, bool loop, int fadeTime)
        {
            if (!music.TryGetValue(id, out var filePath)) throw new ArgumentException(id);
            if (musicLayers.Count > 0) StopLayeredMusic(fadeTime);

            int? previousIndex = PlayingMusic;

            var reader = new AudioFileReader(filePath);
            var _loop = new LoopStream(reader) { EnableLoop = loop };
            var sampleProvider = _loop.ToSampleProvider();
            var fader = new FadeInOutSampleProvider(sampleProvider, initiallySilent: true);

            musicMixer.AddMixerInput(fader);

            int index = Interlocked.Increment(ref PlayingIndex);
            PlayingMusic = index;
            lock (musicTracks) musicTracks.Add(index, (reader, fader, loop));

            fader.BeginFadeIn(fadeTime);

            if (previousIndex.HasValue) FadeOutAndRemove(previousIndex.Value, fadeTime);

            return index;
        }

        public static void StopFinishedMusic()
        {
            // CAMBIO: se toma un snapshot dentro del lock, porque FadeOutAndRemove
            // modifica musicTracks desde otro hilo y podía romper el foreach
            List<KeyValuePair<int, (AudioFileReader reader, FadeInOutSampleProvider fader, bool loops)>> snapshot;
            lock (musicTracks)
            {
                if (musicTracks.Count == 0) return;
                snapshot = musicTracks.ToList();
            }

            var finishedTracks = new List<int>();
            foreach (var track in snapshot)
            {
                bool finished = track.Value.reader.CurrentTime >= track.Value.reader.TotalTime;
                if (!track.Value.loops && finished) finishedTracks.Add(track.Key);
            }

            foreach (var index in finishedTracks)
            {
                (AudioFileReader reader, FadeInOutSampleProvider fader, bool loops) track;
                lock (musicTracks)
                {
                    if (!musicTracks.TryGetValue(index, out track)) continue;
                    musicTracks.Remove(index);
                }

                musicMixer.RemoveMixerInput(track.fader);
                track.reader.Dispose();
            }
        }

        private static void FadeOutAndRemove(int index, int time)
        {
            (AudioFileReader reader, FadeInOutSampleProvider fader, bool loops) track;
            lock (musicTracks)
            {
                if (!musicTracks.TryGetValue(index, out track)) return;
            }

            track.fader.BeginFadeOut(time);

            Task.Delay(time).ContinueWith(_ =>
            {
                musicMixer.RemoveMixerInput(track.fader);
                track.reader.Dispose();
                lock (musicTracks) musicTracks.Remove(index);
            });
        }

        public static void InitMixer()
        {
            var mixerFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            musicMixer = new MixingSampleProvider(mixerFormat) { ReadFully = true };
            musicOutput = new WaveOutEvent(); // CAMBIO: antes WaveOut
            musicOutput.Init(musicMixer);
            musicOutput.Play();
        }


        public static void StartLayeredMusic(Dictionary<string, string> layerFilenames, string initialActiveLayer, bool loop, int fadeTime)
        {
            foreach (var musicFile in layerFilenames)
            {
                var layerName = musicFile.Key; var filename = musicFile.Value;
                if (!File.Exists(Path.Combine(musicPath, filename))) throw new FileNotFoundException(filename);
            }

            if (PlayingMusic.HasValue)
            {
                FadeOutAndRemove(PlayingMusic.Value, fadeTime);
                PlayingMusic = null;
            }

            foreach (var musicFile in layerFilenames)
            {
                var layerName = musicFile.Key; var filename = musicFile.Value;

                string file = Path.Combine(musicPath, filename);
                var reader = new AudioFileReader(file);
                var _loop = new LoopStream(reader) { EnableLoop = loop };
                var sampleProvider = _loop.ToSampleProvider();

                bool startSilent = layerName != initialActiveLayer;
                var fader = new FadeInOutSampleProvider(sampleProvider, initiallySilent: startSilent);

                musicMixer.AddMixerInput(fader);
                musicLayers.Add(layerName, (reader, fader));
            }
        }

        public static void CrossfadeToLayer(string layerName, int fadeTime)
        {
            foreach (var musicFile in musicLayers)
            {
                if (musicFile.Key == layerName) musicFile.Value.fader.BeginFadeIn(fadeTime);
                else musicFile.Value.fader.BeginFadeOut(fadeTime);
            }
        }


        public static void StopLayeredMusic(int fadeTime)
        {
            foreach (var musicFile in musicLayers)
            {
                var fader = musicFile.Value.fader;
                var reader = musicFile.Value.reader;

                fader.BeginFadeOut(fadeTime);

                Task.Delay(fadeTime).ContinueWith(_ =>
                {
                    musicMixer.RemoveMixerInput(fader);
                    reader.Dispose();
                });
            }
            musicLayers.Clear();
        }

        public static void LoadSFX(string filename, string id)
        {
            string file = Path.Combine(audioPath, filename);
            if (!File.Exists(file)) throw new FileNotFoundException(file);
            if (sfxMap.ContainsKey(id)) throw new ArgumentException(id);
            sfxMap.Add(id, new SFXCache(file));
        }

        public static int PlaySFX(string AudioID)
        {
            if (!sfxMap.TryGetValue(AudioID, out var sound)) throw new ArgumentException(AudioID);

            var ms = new MemoryStream(sound.AudioData);
            var stream = new RawSourceWaveStream(ms, sound.waveFormat);

            // CAMBIO PRINCIPAL: WaveOut -> WaveOutEvent.
            // WaveOut crea una ventana oculta (control sin nombre) ligada al hilo que lo creó;
            // hacerle Dispose desde otro hilo causaba la InvalidOperationException.
            var player = new WaveOutEvent();
            player.Init(stream);

            int index = Interlocked.Increment(ref PlayingIndex);
            lock (PlayingAudioMap) PlayingAudioMap.Add(index, (player, stream));

            player.PlaybackStopped += (s, e) =>
            {
                // stream.Dispose() (dentro de Clean) ya libera también el MemoryStream
                Task.Run(() => Clean(index));
            };

            player.Play();
            return index;
        }

        // Helper: lectura segura del diccionario desde cualquier hilo
        private static bool TryGetPlaying(int id, out (WaveOutEvent player, IDisposable reader) data)
        {
            lock (PlayingAudioMap) return PlayingAudioMap.TryGetValue(id, out data);
        }

        public static void Pause(int id)
        {
            if (TryGetPlaying(id, out var AudioData)) AudioData.player.Pause();
        }

        public static void Resume(int id)
        {
            if (TryGetPlaying(id, out var AudioData)) AudioData.player.Play();
        }

        public static void Stop(int id)
        {
            if (TryGetPlaying(id, out var AudioData))
            {
                AudioData.player.Stop();
            }
        }

        public static void StopMusic() => musicOutput.Stop();

        public static void ResumeMusic() => musicOutput.Play();
        public static void PauseMusic() => musicOutput.Pause();

        private static void Clean(int id)
        {
            (WaveOutEvent player, IDisposable reader) data;

            // Remover del diccionario de forma atómica: evita doble Dispose si dos hilos llegan a la vez
            lock (PlayingAudioMap)
            {
                if (!PlayingAudioMap.TryGetValue(id, out data)) return;
                PlayingAudioMap.Remove(id);
            }

            // Dispose fuera del lock para evitar bloqueos
            data.player.Dispose();
            data.reader.Dispose();
        }
    }
}