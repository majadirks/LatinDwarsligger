namespace LatinDwarsliggerLogic
{
    public class HtmlFormatter
    {
        public static IEnumerable<string> ReadFile(string filename)
        {
            using StreamReader reader = new(filename);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
