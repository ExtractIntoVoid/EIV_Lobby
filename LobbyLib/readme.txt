

/EIV_Lobby/

Connect - via Master
DirectConnect - via Direct Connect

About - About this Lobby Server.

Socket/Chat - Websocket for chat
Socket/Client/{GUID} - Websocket for mostly that doesnt need HTTP (Matchmake, friend system?, groups)

Profile/Inventory/{Action} - Item manipulation. (Mainly Stash Moving, and onto a character)
Profile/Inventory - Get our charcter inventory
Profile/Stash - Get our stash inventory

Files/Client/Mods/{Modpath}
Files/Server/Mods/{Modpath}

- 
Game connect to this Lobby with Socket Server "Mod".

Socket Naming:
LobbySocket_{SHA1(LobbbyIPPort_GameServerPort)}.sock

- Group

An owner of group can invite a player. (Need to be friend)
Said player can accept or deny, which will displayed on owner.
Owner also can set others as manager (so set others to owner basically)
Owner can kick user from group.

A group is only exists inside the lobby. List of it shared to game server to set players to a group.