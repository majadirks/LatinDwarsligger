using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LatinDwarsliggerLogic
{
    /// <summary>
    /// Given some chunks of text,
    /// decide how to lay them out on a page
    ///     /// Consecutive pages A, B, C, D should come out looking like

    /// -------  -------    
    /// |  B  |	 | ∀  |
    /// |-----|	 |-----|
    /// |  C  |	 | (|  | (upside-down D)
    /// ------	 -------
    ///
    /// Two-column format where possible 
	///   - If two chunks fit side-by-side on one HalfSide, make it so
	///   - Similarly, if the font size allows two columns of dactylic hexameter
    ///   - to fit on a HalfSide, make it so.
    /// </summary>
    public class Arranger
    {
        public Arranger(Font font, decimal pageDoubleHeightInches, decimal pageWidthInches, decimal leftRightMarginInches, decimal topBottomMarginInches)
        {
            this.font = font;
            PageDoubleHeightInches = pageDoubleHeightInches;
            PageWidthInches = pageWidthInches;
            LeftRightMarginInches = leftRightMarginInches;
            TopBottomMarginInches = topBottomMarginInches;         
        }

        private Font font;
        public decimal PageDoubleHeightInches { get; init; }
        public decimal HalfSideHeightInches => (PageDoubleHeightInches / 2) - (TopBottomMarginInches * 2);
        public decimal PageWidthInches { get; init; }
        public decimal LeftRightMarginInches { get; init; }
        public decimal TopBottomMarginInches { get; init; }

        public IEnumerable<Column> ArrangeParagraphsIntoColumns(IEnumerable<Paragraph> paragraphs)
        {
            var columns = new List<Column>();
            // Figure out what will fit in column A1.
            // Then figure out what will go in "the next column".
            // If the two columns can fit side-by-side, call both HalfPageA.
            // Otherwise, add the first column to Half Page A
            // and assign thesecond column to column B1.
            // Proceed until we have filled up through D1 (or D2 if possible).

            // Or: Find the first eight columns. Assign them to spots A1-D2 as possible.


            float halfSideHeightInches = Convert.ToSingle(HalfSideHeightInches);
            float lineHeight = Utils.LineHeight(font);

            string[] lines = paragraphs
                .SelectMany(p => p.Lines.Append(" ")) // Add paragraph break after each paragraph
                .SkipLast(1) // ignore paragraph break
                .ToArray();

            int i = 0;
            for (int colIdx = 0; i < lines.Length; colIdx++)
            {
                Column col = new(font: font, leftRightMarginInches: LeftRightMarginInches, topBottomMarginInches: TopBottomMarginInches);
                // In new column, skip any opening breaks
                string line = lines[i];
                while (string.IsNullOrWhiteSpace(line) && i < lines.Length)
                {
                    i++;
                    line = lines[i];
                }

                

                // Add lines until the next line would push the column above the max height
                for (; col.Height + lineHeight < halfSideHeightInches && i < lines.Length; i++)
                {
                    line = lines[i];
                    col.Contents.Add(line);
                }

                // If final two lines are a break and a line, move the line to the next column
                if (string.IsNullOrWhiteSpace(col.Contents.SkipLast(1).Last()))
                {
                    i--;
                    col.RemoveFinalTwoLines();
                }

                columns.Add(col);
            }
            return columns;
        }

        public IEnumerable<HalfSide> ArrangeColumnsIntoHalfSides(IEnumerable<Column> columns)
        {
            var columnsQ = new Queue<Column>(columns);
            
            throw new NotImplementedException();
        }
    }
}
