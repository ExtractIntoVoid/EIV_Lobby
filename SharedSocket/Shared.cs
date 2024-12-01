namespace SharedSocket;

public delegate void ReceivedSession(SocketUdsSession session, ReadOnlySpan<byte> data);

public delegate void ReceivedClient(SocketUdsClient client, ReadOnlySpan<byte> data);

public delegate void SessionAction(SocketUdsSession session, bool IsDisconnected);