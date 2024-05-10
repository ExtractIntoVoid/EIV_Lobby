using System.IO.Compression;
using System.Text;

namespace EIV_DataPack
{
    public class DataPackWriter : IDataPackManipulator
    {
        BinaryWriter Writer;
        public DataPack Pack { get; set; }

        public DataPackWriter(BinaryWriter writer, DataPack dataPack)
        {
            Pack = dataPack;
            Writer = writer;
        }

        public void Open()
        {

        }

        public void Close()
        {
            Writer.Close();
            Writer.Dispose();
        }

        public void Save()
        {
            Console.WriteLine(Pack.FileNames.Count);
            Writer.Write(Pack.FileNames.Count);
            foreach (var filename in Pack.FileNames)
            {
                var name = Encoding.UTF8.GetBytes(filename);
                Writer.Write(BitConverter.GetBytes(name.Length));
                Writer.Write(name);
                var pos_bef = Writer.BaseStream.Position;
                Writer.Write(BitConverter.GetBytes((long)0));
                Pack.FileNameToData.Add(filename, pos_bef);
            }
            Writer.Write((ushort)0);
            foreach (var item in Pack.FileNameToData)
            {
                var data = Pack.FileNameToMetadata[item.Key];
                var pos_start = Writer.BaseStream.Position;
                Writer.Write(BitConverter.GetBytes(data.Length));
                Writer.Write(data);
                var pos_end = Writer.BaseStream.Position;
                Writer.BaseStream.Seek(item.Value, SeekOrigin.Begin);
                Writer.Write(BitConverter.GetBytes(pos_start));
                Writer.BaseStream.Seek(pos_end, SeekOrigin.Begin);
            }
            Writer.Flush();
            Pack.FileNameToMetadata.Clear();
            Pack.FileNameToData.Clear();
            Pack.FileNames.Clear();
        }

        public void AddFile(string path)
        {
            Pack.FileNames.Add(path);
            var data = File.OpenRead(path);
            MemoryStream mem = new();
            DeflateStream deflateStream = new(mem, CompressionMode.Compress);
            data.CopyTo(deflateStream);
            deflateStream.Dispose();
            Pack.FileNameToMetadata.Add(path, mem.ToArray());
        }

        public void AddDirectory(string path, bool Recursive = false)
        {
            var files = Directory.GetFiles(path,"*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var item in files)
            {
                AddFile(item);
            }
        }

        public void AddData(string dataname, string data)
        {
            AddData(dataname, Encoding.UTF8.GetBytes(data));
        }

        public void AddData(string dataname, byte[] data)
        {
            Pack.FileNames.Add(dataname);
            using MemoryStream memdata = new(data);
            MemoryStream mem = new();
            DeflateStream deflateStream = new(mem, CompressionMode.Compress);
            memdata.CopyTo(deflateStream);
            deflateStream.Dispose();
            Pack.FileNameToMetadata.Add(dataname, mem.ToArray());
        }
    }
}
