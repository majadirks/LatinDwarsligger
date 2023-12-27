using System.ComponentModel;
using System.Text.RegularExpressions;

namespace LatinDwarsliggerLogic
{

    /// <summary>
    /// Responsible for taking an HTML file from The Latin Library
    /// And converting it into an enumerable of ChunkOfText objects
    /// With no line numbers, no HTML tags, etc.
    /// </summary>
    public static class HtmlCleaner
    {
        public static IEnumerable<Paragraph> FormatHtmlFile(string path)
            => File.ReadAllLines(path).FormatHtmlCode();

        public static async Task<IEnumerable<Paragraph>> FormatHtmlFromUrl(string url)
        {
            HttpClient client = new();
            string html = await client.GetStringAsync(url);
            string[] lines = html.Split('\n');
            return FormatHtmlCode(lines);

        }

        public static IEnumerable<Paragraph> FormatHtmlCode(this string[] lines)
        {
            var formatted = lines.StripTagAttributes();
            formatted = formatted.MoveParagraphBeginTagsToOwnLine();
            formatted = formatted.SplitOnBrTags();
            formatted = formatted.DeleteTags("a")
                .DeleteTags("link")
                .ToList();
            formatted = formatted.RemovePairedTags();
            formatted = formatted.StripLineNumbers();
            formatted = formatted.RemoveParagraphCloseTags();
            formatted = formatted.FormatAngleBrackets();
            formatted = formatted.FormatEmDashes();
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
                if (c == '<') // found a tag
                {
                    bool isClosingTag = i < line.Length - 1 && line[i + 1] == '/';
                    if (isClosingTag)
                    {
                        copy.Add(c);
                        continue;
                    }
                    while (c != ' ' && c != '>' && i < line.Length - 1)
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
                .Select(verse => verse.Replace("&nbsp;", "", StringComparison.InvariantCultureIgnoreCase))
                .Select(line => DeleteTags(line, "span"));
        }

        /// <summary>
        /// Find opening of given tag and delete everything through its close
        /// </summary>
        private static string DeleteTags(this string line, string tag)
        {
            while (line.Contains($"<{tag}", StringComparison.InvariantCultureIgnoreCase))
            {
                int start = line.IndexOf($"<{tag}", StringComparison.InvariantCultureIgnoreCase);
                int end = line.IndexOf($"</{tag}>", StringComparison.InvariantCultureIgnoreCase);
                if (end == -1) return line; // no end of tag; just give up here.
                int endTagCharCount = tag.Length + 3;
                line = string.Concat(line.AsSpan(0, start), line.AsSpan(end + endTagCharCount));
            }
            return line;
        }

        /// <summary>
        /// Remove everything within the given tag. Assumes the start and end of the tag are on the same line.
        /// </summary>
        private static IEnumerable<string> DeleteTags(this IEnumerable<string> lines, string tag)
        {
            return lines.Select(
                line => 
                    line.DeleteTags(tag)
                    .Replace($"<{tag}>", "", StringComparison.InvariantCultureIgnoreCase) //clean up any tags (eg <link>) that don't have a closing tag
                    .Replace("[]",""));
        }

        private static IEnumerable<string> FormatEmDashes(this IEnumerable<string> lines)
        {
            // &#151; is not an em dash, but Latin Library treats it as one.
            return lines.Select(line => line
            .Replace("&#151;", "—")
            .Replace("&#151", "—")
            .Replace("&mdash;", "—", StringComparison.InvariantCultureIgnoreCase)
            .Replace("&mdash", "—", StringComparison.InvariantCultureIgnoreCase));
        }

        public static IEnumerable<string> RemoveParagraphCloseTags(this IEnumerable<string> lines)
        => lines.Select(line => line.Replace("</p>", "", StringComparison.InvariantCultureIgnoreCase))
                .Where(line => !string.IsNullOrEmpty(line));

        public static IEnumerable<string> MoveParagraphBeginTagsToOwnLine(this IEnumerable<string> lines)
        {
            var newLines = new List<string>(capacity: lines.Count());
            foreach (string line in lines)
            {
                string copy = new(line);
                while (copy.Contains("<p>", StringComparison.InvariantCultureIgnoreCase) && copy.Length > 3)
                {
                    int startIndex = copy.IndexOf("<p>", StringComparison.InvariantCultureIgnoreCase);
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
                .Replace("<BR>", "<br>", StringComparison.InvariantCultureIgnoreCase)
                .Replace("</BR>", "</br>", StringComparison.InvariantCultureIgnoreCase)
                .Split("<br>")
                .SelectMany(str => str.Split("</br>"));
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

                if (!line.Equals("<p>", StringComparison.InvariantCultureIgnoreCase)) 
                    continue; 

                // At this point, we've just added a <p> tag,
                // so skip any subsequent <p> tags
                while (!line.Equals("<p>", StringComparison.InvariantCultureIgnoreCase) && i < inputArray.Length - 1)
                {
                    i++;
                    line = inputArray[i];
                }
                // We've reached the next non-<p> line, so add it
                copy.Add(line);
            }
            // Don't need a <p> at the end
            if (copy.Count > 0 && copy.Last().Equals("<p>", StringComparison.InvariantCultureIgnoreCase)) 
                copy = copy[0..(copy.Count - 1)];
            return copy;
        }

        public static IEnumerable<string> FormatAngleBrackets(this IEnumerable<string> text)
        {
            return text.Select(line => line
            .Replace("&lt;", "<", StringComparison.InvariantCultureIgnoreCase)
            .Replace("&lt", "<", StringComparison.InvariantCultureIgnoreCase)
            .Replace("&gt;", ">", StringComparison.InvariantCultureIgnoreCase)
            .Replace("&gt", ">", StringComparison.InvariantCultureIgnoreCase)
            .Replace("&laquo;", "«", StringComparison.InvariantCultureIgnoreCase)
            .Replace("&laquo", "«", StringComparison.InvariantCultureIgnoreCase)
            .Replace("&raquo;", "»", StringComparison.InvariantCultureIgnoreCase)
            .Replace("&raquo", "»", StringComparison.InvariantCultureIgnoreCase));
        }

        private static IEnumerable<string> RemovePairedTags(this IEnumerable<string> lines)
        {
            // use regexp, fine everything matching <stuff>; if </stuff> also exists, remove both
            Regex rx = new Regex(@"\<\w+\>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var tags = lines.SelectMany(line => rx.Matches(line))
                .Select(match => match.Value)
                .Distinct()
                // ignore <p> and <br> tags, which we will actually pay attention to later
                .Where(val => 
                    !val.Equals("<p>", StringComparison.InvariantCultureIgnoreCase) &&
                    !val.Equals("<br>", StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            List<string> strippedLines = new(capacity: lines.Count());
            foreach (string line in lines)
            {
                string copy = line;
                foreach (string tag in tags)
                {
                    string bareTag = tag.Replace("<", "").Replace(">", "");
                    copy = copy
                        .Replace(tag, "", StringComparison.InvariantCultureIgnoreCase)
                        .Replace($"</{bareTag}>", "", StringComparison.InvariantCultureIgnoreCase);
                }
                strippedLines.Add(copy);
            }
            return strippedLines;
        }

        public static IEnumerable<Paragraph> ParseTextIntoChunks(this IEnumerable<string> lines)
        {
            string[] lineArray = lines.ToArray();
            var chunks = new List<Paragraph>();
            for(int i = 0; i < lineArray.Length; i++)
            {
                string line = lineArray[i];
                if (line.Equals("<p>", StringComparison.InvariantCultureIgnoreCase) && i < lineArray.Length - 1)
                {
                    i++;
                    line = lineArray[i];
                    var linesInChunk = new List<string>();
                    while (i < lineArray.Length - 1 && !lineArray[i].Equals("<p>", StringComparison.InvariantCultureIgnoreCase))
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
                if (i < lineArray.Length && lineArray[i].Equals("<p>", StringComparison.InvariantCultureIgnoreCase))
                    i--;
            }
            return chunks.Where(chunk => chunk.Any(line => !string.IsNullOrWhiteSpace(line)));
        }
    }
}
