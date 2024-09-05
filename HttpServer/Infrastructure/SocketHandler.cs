namespace codecrafters_http_server.HttpServer.Infrastructure;

public class SocketHandler
{
    public static void HandleClient(Socket client)
    {
        byte[] bufferRequest = new byte[1024];
        client.Receive(bufferRequest);

        HttpRequest request = HttpRequest.Parse(bufferRequest);
        HttpResponse response = HttpResponse.CreateResponse(request);

        byte[] msg = Encoding.ASCII.GetBytes(response.ToString());
        client.Send(msg);

        Console.WriteLine(response.ToString());
    }
}
