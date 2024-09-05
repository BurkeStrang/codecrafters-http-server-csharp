namespace codecrafters_http_server.HttpServer.Infrastructure;

public class TcpListenerServer(IPAddress ipAddress, int port)
{
    private readonly TcpListener _tcpListener = new(ipAddress, port);

    public void Start()
    {
        _tcpListener.Start();
        Console.WriteLine("Server started and listening...");
    }

    public Socket AcceptClient()
    {
        return _tcpListener.AcceptSocket();
    }
}
