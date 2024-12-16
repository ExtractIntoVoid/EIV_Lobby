using LobbyLib;

namespace LobbyConsole;

internal class Program
{
    static void Main(string[] args)
    {
        if (!MainControl.InitAll())
        {
            Console.WriteLine("Creating Lobby Server failed!");
            return;
        }
        Console.WriteLine("Type 'quit', 'q' or 'exit' to quit shutdown the server.");
        string? req = "";
        while (req != "quit" && req != "q" && req != "exit")
            req = Console.ReadLine();
        MainControl.Stop();
    }
}