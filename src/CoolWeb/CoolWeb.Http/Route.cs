namespace CoolWeb.Http
{
    using System;

    public class Route
    {
        public Route(HttpMethod method, string path)
        {
            this.Method = method;

            if (string.IsNullOrEmpty(path?.Trim()))
            {
                string message = $"{nameof(Route)} {nameof(Path)} cannot be null, empty or white space";
                throw new ArgumentException(message);
            }

            this.Path = path;
        }

        public HttpMethod Method { get; }

        public string Path { get; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Route other)
            {
                return this.Method == other.Method && this.Path.Equals(other.Path, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Method, this.Path?.ToLower());
        }
    }
}
