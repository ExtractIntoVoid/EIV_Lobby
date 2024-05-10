using System.IO.Compression;
using System.Text;

namespace EIV_DataPack
{
    public class DataPackReader : IDataPackManipulator
    {
        BinaryReader Reader;
        public DataPack Pack { get; set; }

        public int FileNameCount { get; internal set; } = 0;
        internal long ReadedFilesPos = -1;

        public DataPackReader(BinaryReader reader, DataPack dataPack) 
        {
            Pack = dataPack;
            Reader = reader;
        }

        public void Open()
        {
            FileNameCount = BitConverter.ToInt32(Reader.ReadBytes(4));
            Reader.BaseStream.Position = 6;
        }

        public void Close()
        {
            Reader.Close();
            Reader.Dispose();
        }

        public void ReadFileNames()
        {
            Pack.FileNameToData.Clear();
            Pack.FileNames.Clear();
            Reader.BaseStream.Position = 6;
            FileNameCount = BitConverter.ToInt32(Reader.ReadBytes(4));
            for (int i = 0; i < FileNameCount; i++)
            {
                var filename_len = Reader.ReadInt32();
                var filename = Encoding.UTF8.GetString(Reader.ReadBytes(filename_len));
                Pack.FileNames.Add(filename);
                var start_len = Reader.ReadInt64();
                Pack.FileNameToData.Add(filename, start_len);
            }
            ReadedFilesPos = Reader.BaseStream.Position;
        }

        public byte[] GetFileData(string filename)
        {
            Reader.BaseStream.Position = ReadedFilesPos;
            if (!Pack.FileNameToData.TryGetValue(filename, out var data))
            {
                throw new Exception("file not found inside eivp");
            }
            Reader.BaseStream.Seek(data, SeekOrigin.Begin);
            var datalen = Reader.ReadInt32();
            var databytes = Reader.ReadBytes(datalen);
            var mem = new MemoryStream();
            using var decompressor = new DeflateStream(new MemoryStream(databytes), CompressionMode.Decompress);
            decompressor.CopyTo(mem);
            var arr = mem.ToArray();
            mem.Dispose();
            decompressor.Dispose();
            return arr;
        }

        public byte[] GetFileData(int FileIndex)
        {
            Reader.BaseStream.Position = 10;
            long len_to_Start = -1;
            if (FileIndex > FileNameCount)
                throw new Exception("FileIndex > FileNameCount !!!");
            for (int i = 0; i <= FileIndex; i++)
            {
                var filename_len = Reader.ReadInt32();
                Reader.ReadBytes(filename_len);
                len_to_Start = Reader.ReadInt64();
            }
            if (len_to_Start == -1)
            {
                Console.WriteLine("something wrong");
            }
            Reader.BaseStream.Seek(len_to_Start, SeekOrigin.Begin);
            var datalen = Reader.ReadInt32();
            var databytes = Reader.ReadBytes(datalen);
            var mem = new MemoryStream();
            using var decompressor = new DeflateStream(new MemoryStream(databytes), CompressionMode.Decompress);
            decompressor.CopyTo(mem);
            var arr = mem.ToArray();
            mem.Dispose();
            decompressor.Dispose();
            return arr;
        }
    }
}
