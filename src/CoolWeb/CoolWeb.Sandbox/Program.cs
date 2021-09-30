namespace CoolWeb.Sandbox
{
    using Http;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main()
        {
            var server = new HttpServer();

            server.AddRoute(HttpMethod.Get, "/", Home);
            server.AddRoute(HttpMethod.Get, "/Home", Home);
            server.AddRoute(HttpMethod.Get, "/About", About);
            server.AddRoute(HttpMethod.Get, "/Privacy", Privacy);

            await server.StartAsync();
        }

        public const string Navbar = "<div><a href=\"/Home\">Home</a> " +
                                     "<a href=\"/About\">About</a> <a href=\"/Privacy\">Privacy</a></div>";

        public static void Home(HttpRequest request, HttpResponse response)
        {
            string html = Navbar + "<h1>Welcome to the Home Page</h1>";
            response.Body = Encoding.UTF8.GetBytes(html);
            response.ContentType = ContentTypes.Html;
        }

        public static void About(HttpRequest request, HttpResponse response)
        {
            string html = Navbar + "<h1>This is the About page</h1>";
            response.Body = Encoding.UTF8.GetBytes(html);
            response.ContentType = ContentTypes.Html;
        }

        public static void Privacy(HttpRequest request, HttpResponse response)
        {
            string html = Navbar + "<h1>Privacy and policy</h1>";
            response.Body = Encoding.UTF8.GetBytes(html);
            response.ContentType = ContentTypes.Html;
        }
    }
}
