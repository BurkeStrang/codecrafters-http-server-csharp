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
        // if (httpHeader.Contains("Accept-Encoding") && !value.Contains("gzip"))
        // {
        //     return null;
        // }
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

    public static HttpResponse Ok(bool? isGzip = false, string? body = null)
    {
        if (isGzip == true)
        {
            return new HttpResponse(HttpStatusLine.Ok)
                .AddHeader(HttpHeader.ContentType, "text/plain")
                .AddHeader(HttpHeader.ContentEncoding, "gzip")
                .AddCrlf();
        }
        else if (body != null)
        {
            return new HttpResponse(HttpStatusLine.Ok)
                .AddHeader(HttpHeader.ContentType, "text/plain")
                .AddHeader(HttpHeader.ContentLength, body.Length.ToString())
                .AddCrlf()
                .AddBody(body);
        }
        return new HttpResponse(HttpStatusLine.Ok).AddCrlf();
    }

    public static HttpResponse File(string filePath)
    {
        string fileContent = System.IO.File.ReadAllText(filePath);
        return new HttpResponse(HttpStatusLine.Ok)
            .AddHeader(HttpHeader.ContentType, "application/octet-stream")
            .AddHeader(HttpHeader.ContentLength, fileContent.Length.ToString())
            .AddCrlf()
            .AddBody(fileContent);
    }

    public static HttpResponse NotFound()
    {
        return new HttpResponse(HttpStatusLine.NotFound).AddCrlf();
    }

    public static HttpResponse Created()
    {
        return new HttpResponse(HttpStatusLine.Created).AddCrlf();
    }

    public static HttpResponse BadRequest()
    {
        return new HttpResponse(HttpStatusLine.BadRequest).AddCrlf();
    }
}
