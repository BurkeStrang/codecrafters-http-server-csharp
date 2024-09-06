namespace codecrafters_http_server.HttpServer.Domain.Models;

public sealed record HttpResponse(string ResponseString)
{
    public static implicit operator string(HttpResponse httpResponse)
    {
        return httpResponse.ResponseString;
    }

    public static implicit operator byte[](HttpResponse httpResponse)
    {
        return Encoding.UTF8.GetBytes(httpResponse.ResponseString);
    }

    public HttpResponse AddHeader(string httpHeader, string value)
    {
        StringBuilder sb = new(ResponseString);
        sb.Append($"{httpHeader}: {value}\r\n");
        return new(sb.ToString());
    }

    public HttpResponse AddBody(string body)
    {
        StringBuilder sb = new(ResponseString);
        sb.Append(body);
        return new(sb.ToString());
    }

    public HttpResponse AddCrlf()
    {
        StringBuilder sb = new(ResponseString);
        sb.Append("\r\n");
        return new(sb.ToString());
    }

    public static HttpResponse Ok()
    {
        return new HttpResponse(HttpStatusLine.Ok).AddCrlf();
    }

    public static HttpResponse Ok(string body)
    {
        return new HttpResponse(HttpStatusLine.Ok)
            .AddHeader(HttpHeader.ContentType, "text/plain")
            .AddHeader(HttpHeader.ContentLength, body.Length.ToString())
            .AddCrlf()
            .AddBody(body);
    }

    public static HttpResponse File(string fileName)
    {
        byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
        string fileContent = System.IO.File.ReadAllText(fileName);
        return new HttpResponse(HttpStatusLine.Ok)
            .AddHeader(HttpHeader.ContentType, "application/octet-stream")
            .AddHeader(HttpHeader.ContentLength, fileBytes.Length.ToString())
            .AddCrlf()
            .AddBody(fileContent);
    }

    public static HttpResponse NotFound()
    {
        return new HttpResponse(HttpStatusLine.NotFound).AddCrlf();
    }
}


