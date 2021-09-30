namespace CoolWeb.Http
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class HttpServer
    {
        private readonly Dictionary<Route, Action<HttpRequest, HttpResponse>> routingTable = new();

        private static readonly IPAddress Localhost = IPAddress.Loopback;
        private const int DefaultPort = 9999;
        private const int BufferSize = 1024;

        private readonly TcpListener listener;

        public HttpServer() : this(Localhost, DefaultPort)
        {
        }

        public HttpServer(IPAddress address, int port)
        {
            this.Port = port;
            this.listener = new(address, Port);
        }

        public int Port { get; }

        public void AddRoute(HttpMethod method, string path, Action<HttpRequest, HttpResponse> action = null)
        {
            var route = new Route(method, path);

            if (this.routingTable.ContainsKey(route))
            {
                throw new InvalidOperationException($"{nameof(Route)} with such value already exists");
            }

            this.routingTable.Add(route, action);
        }

        public async Task StartAsync()
        {
            this.listener.Start();

            Console.WriteLine($"Server started on port {this.Port}...");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                TcpClient client = await this.listener.AcceptTcpClientAsync();
                using NetworkStream stream = client.GetStream();

                var bytesRead = new List<byte>();
                var buffer = new byte[BufferSize];
                int offset = 0;

                while (true)
                {
                    int bytesReadCount = await stream.ReadAsync(buffer, offset, buffer.Length);
                    offset += bytesReadCount;

                    if (bytesReadCount >= buffer.Length)
                    {
                        bytesRead.AddRange(buffer);
                    }
                    else
                    {
                        var trimmmedCopy = new byte[bytesReadCount];
                        Array.Copy(buffer, trimmmedCopy, trimmmedCopy.Length);
                        bytesRead.AddRange(trimmmedCopy);
                        break;
                    }
                }

                string requestAsString = Encoding.UTF8.GetString(bytesRead.ToArray());

                var request = new HttpRequest(requestAsString);
                var response = new HttpResponse();

                var route = new Route(request.Method, request.Path);

                var sessionCookie = request.SessionCookie ?? Guid.NewGuid().ToString();

                var responseSessionCookie = 
                    new ResponseCookie(HttpConstants.SessionCookieName, sessionCookie)
                    {
                        MaxAge = 9000
                    }
                    .CreateHeader();

                response.Headers.Add(responseSessionCookie.Key, responseSessionCookie.Value);

                if (this.routingTable.ContainsKey(route))
                {
                    try
                    {
                        this.routingTable[route].Invoke(request, response);
                    }
                    catch (Exception)
                    {
                        response.StatusCode = HttpStatusCode.InternalServerError;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                await stream.WriteAsync(response.ToByteArray());
                Console.WriteLine(response.ToString());
            }
        }
    }
}
