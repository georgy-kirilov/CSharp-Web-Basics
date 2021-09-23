namespace CoolWeb.Sandbox
{
    using Http;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main()
        {
            var code = HttpStatusCode.InternalServerError;
            System.Console.WriteLine(code.Message());
            var server = new HttpServer();
            await server.StartAsync();
        }
    }
}
