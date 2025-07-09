### Main

Game first calls /About.\
If version is not compatible, we skip it showing to users.
It checks again with the data from the previous /About that the lobby is full or not.\
then join/connect to the server , game calls /Connect\
It checks again with /About that the lobby is full or not.\
Also checking if has any mods, if so, download them all, verify with hash. \
Loading them, if any needs a full game restart, it will do so. -- NOTE: Many mods does NOT need a full game restart. If required you probably see a screen to do so.\
It gets a json with our ID, a custom formatted Ticket. (and maybe a permission?)\
We connect to Websocket Chat, Websocket Client. (With ID, and Ticket)\
When user clicks to View Stash we load from /Profile/Stash.\
When user clicks to View Inventory we load from /Profile/Inventory.\
Actions such as moving items, throwing out, de/euqip will handled with Socket/Client/{GUID}\