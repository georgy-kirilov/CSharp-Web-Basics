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

                string request = Encoding.UTF8.GetString(bytesRead.ToArray());

                string content = "<h1>Hello World</h1><p>This is Cool Web Server speaking</p>";
                var response = new HttpResponse(content, ContentTypes.Html);

                await stream.WriteAsync(response.ToByteArray());
            }
        }
    }
}
