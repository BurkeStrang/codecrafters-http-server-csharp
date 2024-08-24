
namespace codecrafters_http_server.HttpServer.Infrastructure;

public class TcpListenerServer
{
    private readonly TcpListener _tcpListener;

    public TcpListenerServer(IPAddress ipAddress, int port)
    {
        _tcpListener = new TcpListener(ipAddress, port);
    }

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
