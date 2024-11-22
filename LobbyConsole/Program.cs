using EIV_Common.JsonStuff;
using EIV_JsonLib.Classes;
using LobbyLib;
using LobbyLib.GameSocket;
using NetCoreServer;

namespace LobbyConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MainControl.InitAll());
            string ret = "ret";
            while (ret != "q")
            {
                ret = Console.ReadLine().ToLower();
                if (ret == "test")
                {
                    ServerUds.LobbyUdsClient chatClient = new(Path.Combine(Path.GetTempPath(), "chat.sock"));
                    chatClient.Connect();
                    Console.WriteLine("Press Enter to stop the client or '!' to reconnect the client...");

                    // Perform text input
                    for (; ; )
                    {
                        string line = Console.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            break;

                        // Disconnect the client
                        if (line == "!")
                        {
                            Console.Write("Client disconnecting...");
                            chatClient.DisconnectAsync();
                            Console.WriteLine("Done!");
                            continue;
                        }

                        // Send the entered text to the chat server
                        chatClient.SendAsync(line);
                    }

                    // Disconnect the client
                    Console.Write("Client disconnecting...");
                    chatClient.Disconnect();
                    Console.WriteLine("Done!");
                }
            }
            MainControl.Stop();
        }

        static void x()
        {
            var itemRecreators = new List<ItemRecreator>
            {
                new("Backpack_32"),
                new("Armor_Medium"),
                new("ArmoredRig_Medium", 1, [new("ArmorPlate_Lightweight")]),
                new("Melee_Knife"),
                new("Throwable_Flashbang", 2),
                new("Consumable_FoodCan", 4)
            };

            itemRecreators[2].Contained.AddToSlot([("Medkit",2)], AcceptedSlots.ItemsSlot);

            var mag = new ItemRecreator("Magazine_919");
            mag.Contained.AddToSlot([("Ammo_919", 34)], AcceptedSlots.AmmoSlot);

            itemRecreators[2].Contained.Add(mag);
            itemRecreators[2].Contained.Add(mag);


        }
    }
}