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
    internal class ChunkArranger
    {
        public ChunkArranger(Font font, decimal pageDoubleHeightInches, decimal pageWidthInches, decimal leftRightMarginInches, decimal topBottomMarginInches)
        {
            this.font = font;
            PageDoubleHeightInches = pageDoubleHeightInches;
            PageWidthInches = pageWidthInches;
            LeftRightMarginInches = leftRightMarginInches;
            TopBottomMarginInches = topBottomMarginInches;
        }

        public Font font {get; init;}
        public decimal PageDoubleHeightInches {get; init;}
        public decimal PageWidthInches { get; init; }
        public decimal LeftRightMarginInches {get; init; }
        public decimal TopBottomMarginInches { get; init; }

    }
}
