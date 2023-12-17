using System.ComponentModel;

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
        
        public static IEnumerable<Paragraph> FormatHtmlCode(this string[] lines)
        {
            var formatted = lines.StripTagAttributes();

            formatted = formatted.MoveParagraphBeginTagsToOwnLine();
            formatted = formatted.SplitOnBrTags();
            formatted = formatted.DeleteTags("a").DeleteTags("title").DeleteTags("link");
            formatted = formatted.DeleteBoldTags();
            formatted = formatted.StripLineNumbers();
            formatted = formatted.RemoveParagraphCloseTags();
            formatted = formatted.RemoveDivTags();
            formatted = formatted.FormatAngleBrackets();
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
                .Select(verse => verse.Replace("&nbsp;", ""))
                .Select(line => DeleteTags(line, "span"));
        }

        /// <summary>
        /// Find opening of given tag and delete everything through its close
        /// </summary>
        private static string DeleteTags(string line, string tag)
        {
            while (line.Contains($"<{tag}"))
            {
                int start = line.IndexOf($"<{tag}");
                int end = line.IndexOf($"</{tag}>");
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
                    DeleteTags(line, tag)
                    .Replace($"<{tag}>", "") //clean up any tags (eg <link>) that don't have a closing tag
                    .Replace("[]",""));
        }

        private static IEnumerable<string> DeleteBoldTags(this IEnumerable<string> lines)
        {
            // Don't delete the bolded content, just the tags
            return lines.Select(line => line.Replace("<b>", "").Replace("</b>", ""));
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

        public static IEnumerable<string> FormatAngleBrackets(this IEnumerable<string> text)
        {
            return text.Select(line => line.Replace("&lt;", "<").Replace("&gt;", ">"));
        }

        public static IEnumerable<Paragraph> ParseTextIntoChunks(this IEnumerable<string> lines)
        {
            string[] lineArray = lines.ToArray();
            var chunks = new List<Paragraph>();
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
