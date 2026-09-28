using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograVJ.Engine.Manager
{
    public class LoopStream : WaveStream
    {
        private WaveStream sourceStream;
        public bool EnableLoop { get; set; }

        public LoopStream(WaveStream ss)
        {
            sourceStream = ss;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;

        public override long Length => sourceStream.Length;

        public override long Position { get => sourceStream.Position; set => sourceStream.Position = value; }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalRead, count - totalRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLoop) break;
                    sourceStream.Position = 0;
                    continue;
                }
                totalRead += bytesRead;
            }
            return totalRead;
        }
    }
}
