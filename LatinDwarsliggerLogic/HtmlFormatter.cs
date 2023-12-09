using System.ComponentModel;

namespace LatinDwarsliggerLogic
{
    public static class HtmlFormatter
    {
        public static IEnumerable<ChunkOfText> FormatHtmlFile(string path)
            => File.ReadAllLines(path).FormatHtmlCode();
        
        public static IEnumerable<ChunkOfText> FormatHtmlCode(this string[] lines)
        {
            var formatted = lines.StripTagAttributes();
            formatted = formatted.MoveParagraphBeginTagsToOwnLine();
            formatted = formatted.SplitOnBrTags();
            formatted = formatted.StripLineNumbers();
            formatted = formatted.RemoveParagraphCloseTags();
            formatted = formatted.RemoveDivTags();
            formatted = formatted.Skip(1); // remove header stuff
            formatted = formatted.RemoveRedundantParagraphTags();
            formatted = formatted.Select(line => line.Trim());
            var chunks = formatted.ParseTextIntoChunks();
            return chunks;
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
            var inputArray = lines.ToArray();
            int lineCount = inputArray.Length;
            var copy = new List<string>(lineCount);
            for (int i = 0; i < lineCount; i++)
            {
                string line = inputArray[i];
                copy.Add(line);

                if (line != "<p>") continue; 

                // At this point, we've just added a <p> tag,
                // so skip any subsequent <p> tags
                while (line == "<p>" && i < inputArray.Length - 1)
                {
                    i++;
                    line = inputArray[i];
                }
                // We've reached the next non-<p> line, so add it
                copy.Add(line);
            }
            if (copy.Last() == "<p>") // Don't need a <p> at the end
                copy = copy[0..(copy.Count - 1)];
            return copy;
        }

        public static IEnumerable<string> RemoveDivTags(this IEnumerable<string> text)
        {
            return text.Select(line => line.Replace("<div>", ""));
        }

        public static IEnumerable<ChunkOfText> ParseTextIntoChunks(this IEnumerable<string> lines)
        {
            string[] lineArray = lines.ToArray();
            var chunks = new List<ChunkOfText>();
            for(int i = 0; i < lineArray.Length; i++)
            {
                string line = lineArray[i];
                if (line == "<p>" && i < lineArray.Length - 1)
                {
                    i++;
                    line = lineArray[i];
                    var linesInChunk = new List<string>();
                    while (i < lineArray.Length - 1 && lineArray[i] != "<p>")
                    {
                        linesInChunk.Add(line);
                        i++;
                        line = lineArray[i];
                    }
                    chunks.Add(new(linesInChunk));
                }
                else
                {
                    chunks.Add(new([line]));
                }
                if (i < lineArray.Length && lineArray[i] == "<p>")
                    i--;
            }
            return chunks.Where(chunk => chunk.Any(line => !string.IsNullOrWhiteSpace(line)));
        }
    }
}
