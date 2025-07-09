### Socket/Chat Actions:

User sents a "ChatMessage" json.\
We check if user blocked, if so dont send anything.\
Lobby can make it check for any bad word, if exists we dont send it.\
We do not provide any bad word for filtering itself.\
Filter stats with "ex: " will consider regex.\
Otherwise we check with Message.Contains(filteredWord)