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
    /// </summary>
    public class ChunkArranger(Font font, decimal pageDoubleHeightInches, decimal pageWidthInches, decimal leftRightMarginInches, decimal topBottomMarginInches)
    {
        public Font Font { get; init; } = font;
        public decimal PageDoubleHeightInches { get; init; } = pageDoubleHeightInches;
        public decimal PageWidthInches { get; init; } = pageWidthInches;
        public decimal LeftRightMarginInches { get; init; } = leftRightMarginInches;
        public decimal TopBottomMarginInches { get; init; } = topBottomMarginInches;

    }
}
