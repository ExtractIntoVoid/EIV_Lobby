# EIV_Lobby

This is a separate part of the server.\
Lobby as sending and receiving stuff to Game Server.\
Lobby should contain:
- general settings for the Game Server. (Arg, player count, etc)
- mods for the Game Server, Client, itself.

### Server
Game Server (GS) should be started by Lobby. (Or with specific arguments)\
GS should only use Lobby to get the Players Profile (Inventory, Stash, Profile, etc)\
GS can reply with specific request (Current players, stats)
GS started as On-Demand, so no server will run if no player want to start a game.

### Client
Client using Lobby to:
- Get GS connection information.
- Queue with players to join random server.
- Interact with friends (Chat, Send Invite, Send Message/Gifts)
- Interact with inventory/stash to modify the loadout to join the match.