namespace codecrafters_http_server.HttpServer.Infrastructure;

public class SocketHandler
{
    public async static Task HandleClient(Socket client)
    {
        byte[] bufferRequest = new byte[1024];
        await client.ReceiveAsync(bufferRequest);

        HttpRequest request = HttpRequest.Parse(bufferRequest);
        HttpResponse response = HttpResponse.CreateResponse(request);

        byte[] msg = Encoding.ASCII.GetBytes(response.ToString());
        await client.SendAsync(msg);

        // Console.WriteLine(response.ToString());
    }
}
