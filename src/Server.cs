using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = new(IPAddress.Any, 4221);
server.Start();
while (true)
{
    Socket client = server.AcceptSocket();
    Console.WriteLine("Connection reveived");
    byte[] bufferRequest = new byte[1024];
    client.Receive(bufferRequest);
    string request = Encoding.ASCII.GetString(bufferRequest);
    string startLine = request.Split("\r\n")[0];
    string path = startLine.Split(" ")[1];
    string response = "HTTP/1.1 404 Not Found\r\n\r\n";
    if (path.Equals("/"))
        response = "HTTP/1.1 200 OK\r\n\r\n";
    byte[] msg = Encoding.ASCII.GetBytes(response);
    Console.WriteLine(response);
    client.Send(msg);
}
