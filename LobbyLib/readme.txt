/Connect - Connecting to Lobby Server. (Getting custom related IDs, hashes, etc.)
/Disconnect - Disconnecting from Lobby Server.

/About - About this Lobby Server.

/Socket/Chat - Websocket for chat
/Socket/Client/{GUID} - Websocket for mostly that doesnt need HTTP (Matchmake, friend system?, groups, Item manipulation. (Mainly Stash Moving, and onto a character)

/Profile/Character - Get our character data.
/Profile/Inventory - Get our character inventory.
/Profile/Stash - Get our stash inventory

/Files/Client/Mods/{Modpath}
/Files/Server/Mods/{Modpath}

- 
Game connect to this Lobby with Socket Server "Mod".

Socket Naming:
LobbySocket_{GameServerPort}_{MapToPlay}.sock

- Group

An owner of group can invite a player. (Need to be friend)
Said player can accept or deny, which will displayed on owner.
Owner also can set others as manager (so set others to owner basically)
Owner can kick user from group.

A group is only exists inside the lobby. List of it shared to game server to set players to a group.
