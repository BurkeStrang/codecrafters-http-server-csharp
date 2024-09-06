class Program
{
    static async Task Main(string[] args) => await new HttpServer().Start(args);
}
