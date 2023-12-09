using System.ComponentModel;

namespace LatinDwarsliggerLogic
{
    public static class HtmlFormatter
    {
        public static IEnumerable<string> FormatHtmlFile(string path)
        {
            var lines = File.ReadAllLines(path);

            var formatted = lines.StripTagAttributes()
                .MoveParagraphBeginTagsToOwnLine()
                .SplitOnBrTags()
                .StripLineNumbers()
                .RemoveParagraphCloseTags()
                .RemoveDivTags()
                .Skip(1) // remove header stuff
                .RemoveRedundantParagraphTags()
                .ToArray();
            return formatted;

        }
        public static IEnumerable<string> StripTagAttributes(this IEnumerable<string> lines)
        => lines.Select(StripTagAttributes);

        private static string StripTagAttributes(string line)
        {
            var copy = new List<char>(line.Length);
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '<')
                {
                    bool isEnd = i < line.Length - 1 && line[i + 1] == '/';
                    if (isEnd)
                    {
                        copy.Add(c);
                        continue;
                    }
                    while (c != ' ' && i < line.Length - 1)
                    {
                        copy.Add(c);
                        i++;
                        c = line[i];
                    }

                    while (c != '>' && i < line.Length - 1)
                    {
                        i++;
                        c = line[i];
                    }
                }
                copy.Add(c);
            }
            return new(copy.ToArray());
        }

        public static IEnumerable<string> StripLineNumbers(this IEnumerable<string> verses)
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

        public static IEnumerable<string> RemoveParagraphCloseTags(this IEnumerable<string> lines)
        => lines.Select(line => line.Replace("</p>", ""))
                .Where(line => !string.IsNullOrEmpty(line));

        public static IEnumerable<string> MoveParagraphBeginTagsToOwnLine(this IEnumerable<string> lines)
        {
            var newLines = new List<string>(capacity: lines.Count());
            foreach (string line in lines)
            {
                string copy = new(line);
                while (copy.Contains("<p>") && copy.Length > 3)
                {
                    int startIndex = copy.IndexOf("<p>");
                    if (startIndex > 0)
                        newLines.Add(copy[..startIndex]);
                    newLines.Add("<p>");
                    copy = copy[(startIndex + 3)..];
                }
                newLines.Add(copy);
            }
            return newLines.Select(line => line.Trim()).Where(line => !string.IsNullOrWhiteSpace(line));
        }


        /// <summary>
        /// Create an IEnumerable of strings based on the input,
        /// but rearrange so that new strings are based on br tags
        /// and p tags
        /// rather than line breaks in the original file.
        /// (Possibly retain opening p as its own line for later use?)
        /// </summary>
        public static IEnumerable<string> SplitOnBrTags(this IEnumerable<string> text)
        {
            var x = string.
                Join(" ", text)
                .Split("<br>");
            var y = x
                .MoveParagraphBeginTagsToOwnLine()
                .RemoveParagraphCloseTags()
                .Select(line => line.Trim());
            return y;
        }

        public static IEnumerable<string> RemoveRedundantParagraphTags(this IEnumerable<string> lines)
        {
            var inputArray = lines
                .SkipWhile(line => line == "<p>") // skip any starting <p> tags
                .ToArray();
            int lineCount = inputArray.Length;
            var copy = new List<string>(lineCount);
            for (int i = 0; i < lineCount; i++)
            {
                string line = inputArray[i];
                copy.Add(line);
                if (line == "<p>")
                {
                    i++;
                    if (i < inputArray.Length)
                        line = inputArray[i];
                    if (line == "<p>") continue;
                }
            }
            if (copy.Last() == "<p>")
                copy = copy[0..(copy.Count - 1)];
            return copy;
        }

        public static IEnumerable<string> RemoveDivTags(this IEnumerable<string> text)
        {
            return text.Select(line => line.Replace("<div>", ""));
        }
    }
}
