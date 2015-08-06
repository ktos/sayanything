/*
 * SayAnything
 *
 * Copyright (C) Marcin Badurowicz 2012-2015
 *
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files
 * (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
 * BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE. 
 */
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Ktos.SayAnything.Models
{
    class Microphone
    {
        private Microsoft.Xna.Framework.Audio.Microphone micro;
        private MemoryStream stream;

        public Microphone()
        {
            micro = Microsoft.Xna.Framework.Audio.Microphone.Default;

            stream = new MemoryStream();
            micro.BufferDuration = TimeSpan.FromSeconds(1);
            byte[] buffer = new byte[micro.GetSampleSizeInBytes(micro.BufferDuration)];

            micro.BufferReady += (s, f) =>
                {
                    micro.GetData(buffer);
                    stream.Write(buffer, 0, buffer.Length);
                };
        }

        public async Task SaveToIsolatedStorageAsync(string fileName)
        {
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;            
            StorageFile f = await local.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var s = await f.OpenStreamForWriteAsync();

            s.Write(stream.ToArray(), 0, stream.ToArray().Length);
            s.Close();
        }

        public void SaveToMediaLibrary(string fileName, string artist, string name)
        {
            MediaLibrary ml = new MediaLibrary();
            ml.SaveSong(new Uri(fileName, UriKind.Relative), new SongMetadata() { ArtistName = artist, Name = name }, SaveSongOperation.CopyToLibrary);
        }

        public async Task PlayFileAsync(string fileName)
        {
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            var f = await local.GetFileAsync(fileName);

            await Windows.System.Launcher.LaunchFileAsync(f);
        }

        private void updateWavHeader()
        {
            if (!stream.CanSeek) throw new Exception("Can't seek stream to update wav header");

            var oldPos = stream.Position;

            // ChunkSize 36 + SubChunk2Size
            stream.Seek(4, SeekOrigin.Begin);
            stream.Write(BitConverter.GetBytes((int)stream.Length - 8), 0, 4);

            // Subchunk2Size == NumSamples * NumChannels * BitsPerSample/8 This is the number of bytes in the data.
            stream.Seek(40, SeekOrigin.Begin);
            stream.Write(BitConverter.GetBytes((int)stream.Length - 44), 0, 4);

            stream.Seek(oldPos, SeekOrigin.Begin);
        }

        private void writeWavHeader()
        {
            const int bitsPerSample = 16;
            const int bytesPerSample = bitsPerSample / 8;
            var encoding = System.Text.Encoding.UTF8;
            int sampleRate = this.micro.SampleRate;

            // ChunkID Contains the letters "RIFF" in ASCII form (0x52494646 big-endian form).
            stream.Write(encoding.GetBytes("RIFF"), 0, 4);

            // NOTE this will be filled in later
            stream.Write(BitConverter.GetBytes(0), 0, 4);

            // Format Contains the letters "WAVE"(0x57415645 big-endian form).
            stream.Write(encoding.GetBytes("WAVE"), 0, 4);

            // Subchunk1ID Contains the letters "fmt " (0x666d7420 big-endian form).
            stream.Write(encoding.GetBytes("fmt "), 0, 4);

            // Subchunk1Size 16 for PCM. This is the size of therest of the Subchunk which follows this number.
            stream.Write(BitConverter.GetBytes(16), 0, 4);

            // AudioFormat PCM = 1 (i.e. Linear quantization) Values other than 1 indicate some form of compression.
            stream.Write(BitConverter.GetBytes((short)1), 0, 2);

            // NumChannels Mono = 1, Stereo = 2, etc.
            stream.Write(BitConverter.GetBytes((short)1), 0, 2);

            // SampleRate 8000, 44100, etc.
            stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);

            // ByteRate = SampleRate * NumChannels * BitsPerSample/8
            stream.Write(BitConverter.GetBytes(sampleRate * bytesPerSample), 0, 4);

            // BlockAlign NumChannels * BitsPerSample/8 The number of bytes for one sample including all channels.
            stream.Write(BitConverter.GetBytes((short)(bytesPerSample)), 0, 2);

            // BitsPerSample 8 bits = 8, 16 bits = 16, etc.
            stream.Write(BitConverter.GetBytes((short)(bitsPerSample)), 0, 2);

            // Subchunk2ID Contains the letters "data" (0x64617461 big-endian form).
            stream.Write(encoding.GetBytes("data"), 0, 4);

            // NOTE to be filled in later
            stream.Write(BitConverter.GetBytes(0), 0, 4);
        }

        public void Start()
        {
            stream.SetLength(0);
            this.writeWavHeader();
            micro.Start();
        }

        public void Stop()
        {
            micro.Stop();
            this.updateWavHeader();
        }
    }
}
