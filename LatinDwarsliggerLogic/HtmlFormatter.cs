namespace LatinDwarsliggerLogic
{
    public static class HtmlFormatter
    {
        public static IEnumerable<string> StripLineNumbers(string[] verses)
        {
            return verses
                .Select(verse => verse.Replace("&nbsp;", ""))
                .Select(DeleteSpanTags);
        }

        /// <summary>
        /// Find "<span", delete everything through subsequent </span>
        /// </summary>
        private static string DeleteSpanTags(string line)
        {
            int start = line.IndexOf("<span");
            if (start == -1) return line; // no span to delete
            int end = line.IndexOf("</span>");
            if (end == -1) return line; // badly formed line; ignore
            return line.Substring(0, start) + line.Substring(end + 7);
        }
    }
}
