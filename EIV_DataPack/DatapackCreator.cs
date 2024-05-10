
namespace EIV_DataPack
{
    public class DatapackCreator
    {
        public const ushort DATAPACK_VERSION = 1;
        public const string MAGIC = "eivp"; //65 69 76 70
        public const int MagicInt = 1886808421;
        public IDataPackManipulator Manipulator;
        internal DatapackCreator(Stream stream, bool IsRead)
        {
            if (IsRead)
            {
                BinaryReader reader = new BinaryReader(stream);
                if (reader.ReadInt32() != MagicInt)
                    throw new Exception("Wrong file readed!");
                if (reader.ReadUInt16() != DATAPACK_VERSION)
                    throw new Exception("Wrong version readed!");
                Console.WriteLine(reader.BaseStream.Position);
                Manipulator = new DataPackReader(reader, new());
            }
            else
            {
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(MagicInt);
                writer.Write(DATAPACK_VERSION);
                Manipulator = new DataPackWriter(writer, new());
            }
            Manipulator.Open();
        }

        public static DatapackCreator Create(string Filename)
        {
            return Create(File.OpenWrite(Filename));
        }

        public static DatapackCreator Create(FileStream fileStream)
        {
            return new DatapackCreator(fileStream, false);
        }

        public static DatapackCreator Read(string Filename)
        {
            return Read(File.OpenRead(Filename));
        }

        public static DatapackCreator Read(FileStream fileStream)
        {
            return new DatapackCreator(fileStream, true);
        }

        public void Close()
        {
            Manipulator.Close();
        }
    }
}
