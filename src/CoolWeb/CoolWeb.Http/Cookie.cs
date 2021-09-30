namespace CoolWeb.Http
{
    public class Cookie
    {
        public Cookie(string rawCookie)
        {
            var arguments = rawCookie.Split("=");
            this.Name = arguments[0];
            this.Value = arguments[1];
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
