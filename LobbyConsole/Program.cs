using LobbyLib;

namespace LobbyConsole;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(MainControl.InitAll());
        string? req = Console.ReadLine();
        while (req != "quit" && req != "q" && req != "exit")
            req = Console.ReadLine();
        MainControl.Stop();
    }
}