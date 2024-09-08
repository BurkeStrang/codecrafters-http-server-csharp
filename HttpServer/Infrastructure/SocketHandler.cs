namespace codecrafters_http_server.HttpServer.Infrastructure;

public static class SocketHandler
{
    public static void HandleClient(Socket client, string[] args)
    {
        byte[] bufferRequest = new byte[1_024];
        client.Receive(bufferRequest);
        HttpResponse response = HttpRequest.CreateResponse(bufferRequest, args);
        client.Send(response.ToBytes());
    }
}
