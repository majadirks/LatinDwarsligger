namespace LatinDwarsliggerLogic
{
    public static class HtmlFormatter
    {
        public static IEnumerable<string> StripLineNumbers(IEnumerable<string> verses)
        {
            return verses
                .Select(verse => verse.Replace("&nbsp;", ""))
                .Select(DeleteSpanTags);
        }

        /// <summary>
        /// Find opening "span" tag and delete everything through its close
        /// </summary>
        private static string DeleteSpanTags(string line)
        {
            int start = line.IndexOf("<span");
            if (start == -1) return line; // no span to delete
            int end = line.IndexOf("</span>");
            if (end == -1) return line; // badly formed line; ignore
            return string.Concat(line.AsSpan(0, start), line.AsSpan(end + 7));
        }

        public static IEnumerable<string> RemoveParagraphCloseTags(IEnumerable<string> lines)
        => lines.Select(line => line.Replace("</p>", ""))
                .Where(line => !string.IsNullOrEmpty(line));

        public static IEnumerable<string> ReplaceBrWithSpace(IEnumerable<string> lines)
            => lines.Select(line => line.Replace("<br>", " "));

        /// <summary>
        /// Create an IEnumerable of strings based on the input,
        /// but rearrange so that new strings are based on br tags
        /// and p tags
        /// rather than line breaks in the original file.
        /// (Possibly retain opening p as its own line for later use?)
        /// </summary>
        public static IEnumerable<string> SplitOnBrTags(IEnumerable<string> text)
        {
            // replace <br> with space
            // call RemoveParagraphCloseTags
            throw new NotImplementedException();
        }

        public static IEnumerable<ChunkOfText> DivideTextIntoChunks(string[] text)
        {
            throw new NotImplementedException();
        }
    }
}
