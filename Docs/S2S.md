### Server to Server
The server to connect will be the server who provides the data to the clients.\
It adds our local synced maps, mods to the connected server.

# Configuration
S2S must configurated to able to connect to both WS and WSS.\
Need a connection key for connecting to the other server.
Can also configurate the "sublobby" option.

# Connection
Connect websocket to `/Socket/S2S` with a special header `S2SKey: MyKeyHere`\

# Communication
When connected it sync maps, mods that found in the local server.\
When sending queue information it also processes there too.

# Sub-lobby
Useful if want to run more lobby server for the better resource control.