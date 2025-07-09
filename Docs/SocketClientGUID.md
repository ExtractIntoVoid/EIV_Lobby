### Socket/Client/{GUID} Actions:

# MatchmakeCheck / GameStart:
When user click play, we send a "MatchmakeCheck" if any match Queue has this map.\
First we check if map actually available. If not we just dont care about the request. (If 3 non existing map request we disconnect the user)\
If we have a Queue for the map then we join into it, if not create a new one.\
Then if minimum player is done, we start a server with the map. \
Then we respond with "GameStart" with IP and Port of the GameServer

# Groups.
We can invite friends to our group to play with us.\
Only the "Owner" can start a Matchmake. Others can't.\
Anyone in the group can invite others but the owner can change it.\
You can kick any players in your lobby.\
Invitee can decline the invite or accept it, or block the inviter.

# Friends.
We ask the client DRM to send a list for friends. (If exists)\
If exists inside our system we automaticly make both friends.\
Also you can search for friends by its name, or GUID.\
Sent request can be denied or accepted.\
User also can block anyone who sends request.

# Inventory actions.
TODO.