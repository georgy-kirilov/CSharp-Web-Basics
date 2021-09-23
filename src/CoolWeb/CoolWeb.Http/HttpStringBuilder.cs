namespace CoolWeb.Http
{
    using System.Text;

    public class HttpStringBuilder
    {
        private readonly StringBuilder builder = new();

        public void AppendLine(object value = null)
        {
            builder.Append($"{value}{HttpConstants.NewLine}");
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}
