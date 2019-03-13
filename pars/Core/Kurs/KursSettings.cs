
namespace Parser.Core.Kurs
{
    class KursSettings : IParserSettings
    {
        public KursSettings(int start, int end)
        {
            StartPoint = start;
            EndPoint = end;
        }

        public string BaseUrl { get; set; } = "https://kurs.kz";

        public string Prefix { get; set; } = "{CurrentId}";

        public int StartPoint { get; set; }

        public int EndPoint { get; set; }
    }
}
