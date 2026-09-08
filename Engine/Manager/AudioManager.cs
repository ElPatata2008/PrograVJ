using NAudio.Dmo.Effect;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
                //using var reader = new AudioFileReader(FilePath);
                var reader = new AudioFileReader(FilePath);
                waveFormat = reader.WaveFormat;

                var pcmData = new List<byte>((int)reader.Length);
                var buffer = new byte[reader.WaveFormat.AverageBytesPerSecond];
                int bytesRead;

                while ((bytesRead = reader.Read(buffer, 0 , buffer.Length)) > 0)
                {
                    pcmData.AddRange(buffer.Take(bytesRead));
                }

                AudioData = pcmData.ToArray();
            }
        }


        private static Dictionary<string, string> music = new Dictionary<string, string>();
        private static WaveOut musicOutput;
        private static AudioFileReader musicStream;

        private static Dictionary<string, SFXCache> sfxMap = new Dictionary<string, SFXCache>();

        private static Dictionary<string, MemoryStream> audios = new Dictionary<string, MemoryStream>();
        private static Dictionary<int, (WaveOut player, StreamMediaFoundationReader reader)> PlayingAudioMap = new Dictionary<int, (WaveOut player, StreamMediaFoundationReader reader)>();
        private static int PlayingIndex = 0;
        private static string audioPath = "Assets/Audios/";
        private static string musicPath = "Assets/Music/";

        public static void LoadMusic(string filename, string id)
        {
            string file = Path.Combine(musicPath, filename);
            if (!File.Exists(file)) throw new FileNotFoundException(file);
            if (audios.ContainsKey(id)) throw new ArgumentException(id);
            music.Add(id, file);
        }

        public static void PlayMusic(string id)
        {
            if (!music.TryGetValue(id, out var filePath)) throw new ArgumentException(id);
            //StopMusic();
            musicOutput = new WaveOut();
            musicStream = new AudioFileReader(filePath);
            musicOutput.Init(musicStream);
            musicOutput.Play();
        }

        public static void LoadSFX(string filename, string id)
        {
            string file = Path.Combine(audioPath, filename);
            if (!File.Exists(file)) throw new FileNotFoundException(file);
            if (audios.ContainsKey(id)) throw new ArgumentException(id);
            sfxMap.Add(id, new SFXCache(file));
        }

        public static void PlaySFX(string AudioID)
        {
            if (!sfxMap.TryGetValue(AudioID, out var sound)) throw new ArgumentException(AudioID);

            var ms = new MemoryStream(sound.AudioData);
            var stream = new RawSourceWaveStream(ms, sound.waveFormat);
            var player = new WaveOut();
            player.Init(stream);
            player.PlaybackStopped += (s, e) =>
            {
                player.Dispose();
                stream.Dispose();
                ms.Dispose();
            };
            player.Play();
        }

        public static void Load(string filename, string id)
        {
            string file = Path.Combine(audioPath, filename);
            if (!File.Exists(file)) throw new FileNotFoundException(file);
            if (audios.ContainsKey(id)) throw new ArgumentException(id);
            audios.Add(id, GetMemorysStream(file));
        }

        private static MemoryStream GetMemorysStream(string filename)
        {
            byte[] memory = File.ReadAllBytes(filename);
            return new MemoryStream(memory);
        }

        public static int Play(string id)
        {
            if (!audios.ContainsKey(id)) throw new ArgumentException(id);

            var memStream = new MemoryStream(audios[id].ToArray());

            StreamMediaFoundationReader reader = new StreamMediaFoundationReader(memStream);
            WaveOut player = new WaveOut();

            player.Init(reader); player.Play();
            int index = PlayingIndex++;
            PlayingAudioMap.Add(index, (player, reader));
            return index;
        }

        public static void Pause(int id)
        {
            if (PlayingAudioMap.TryGetValue(id, out var AudioData)) AudioData.player.Pause();
        }

        public static void Resume(int id)
        {
            if (PlayingAudioMap.TryGetValue(id, out var AudioData)) AudioData.player.Play();
        }

        public static void Stop(int id)
        {
            if (PlayingAudioMap.TryGetValue(id, out var AudioData))
            {
                AudioData.player.Stop();
                AudioData.player.Dispose(); AudioData.reader.Dispose();
                lock(PlayingAudioMap) PlayingAudioMap.Remove(id);
            }
        }
    }
}
