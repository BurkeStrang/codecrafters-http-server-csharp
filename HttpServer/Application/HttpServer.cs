#pragma warning disable IDE1006 // Naming Styles
namespace codecrafters_http_server.HttpServer.Application;
#pragma warning restore IDE1006 // Naming Styles

public class HttpServer
{
    public HttpServer() => _tcpListenerServer = new TcpListenerServer(IPAddress.Any, 4221);

    private readonly TcpListenerServer _tcpListenerServer;

    public async Task Start()
    {
        _tcpListenerServer.Start();
        while (true)
        {
            Socket client = await _tcpListenerServer.AcceptClient();
            Console.WriteLine("Connection received");
            _ = Task.Run(() => SocketHandler.HandleClient(client));
        }
    }
}
