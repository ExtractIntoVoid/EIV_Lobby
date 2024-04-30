using JsonLib;
using JsonLib.Convert;
using JsonLib.Interfaces;
using LobbyLib.Modding;
using Newtonsoft.Json;

namespace LobbyConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            ModLoader.LoadMods();

        }
    }
}
