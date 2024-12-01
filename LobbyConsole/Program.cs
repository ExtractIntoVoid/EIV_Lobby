using LobbyLib;

namespace LobbyConsole;

internal class Program
{
    static void Main(string[] args)
    {
        bool result = MainControl.InitAll();
        if (!result)
        {
            Console.WriteLine("Creating Lobby Server failed!");
            return;
        }
        string? req = Console.ReadLine();
        while (req != "quit" && req != "q" && req != "exit")
            req = Console.ReadLine();
        MainControl.Stop();
    }
}