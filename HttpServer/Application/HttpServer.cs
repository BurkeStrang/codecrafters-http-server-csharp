namespace codecrafters_http_server.HttpServer.Application;

public class HttpServer
{
    private readonly TcpListenerServer _tcpListenerServer;
    private readonly SocketHandler _socketHandler;

    public HttpServer()
    {
        _tcpListenerServer = new TcpListenerServer(IPAddress.Any, 4221);
        _socketHandler = new SocketHandler();
    }

    public void Start()
    {
        _tcpListenerServer.Start();
        while (true)
        {
            var client = _tcpListenerServer.AcceptClient();
            Console.WriteLine("Connection received");
            SocketHandler.HandleClient(client);
        }
    }
}
