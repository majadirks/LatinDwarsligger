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
    public class ChunkArranger
    {
        public ChunkArranger(Graphics graphics, Font font, decimal pageDoubleHeightInches, decimal pageWidthInches, decimal leftRightMarginInches, decimal topBottomMarginInches)
        {
            this.graphics = graphics;
            this.font = font;
            PageDoubleHeightInches = pageDoubleHeightInches;
            PageWidthInches = pageWidthInches;
            LeftRightMarginInches = leftRightMarginInches;
            TopBottomMarginInches = topBottomMarginInches;
        }

        private Font font;
        private Graphics graphics;
        public decimal PageDoubleHeightInches { get; init; }
        public decimal PageWidthInches { get; init; }
        public decimal LeftRightMarginInches { get; init; }
        public decimal TopBottomMarginInches { get; init; }

        public IEnumerable<PaperSheet> ArrangeChunks(IEnumerable<ChunkOfText> chunks)
        {
            throw new NotImplementedException();
        }

    }
}
