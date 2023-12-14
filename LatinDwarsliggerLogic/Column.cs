using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace LatinDwarsliggerLogic;
#pragma warning disable CA1416 // Validate platform compatibility
public class Column : IEnumerable<string>
{
    public Column(Font font, decimal maxHeightInches, decimal maxWidthInches, decimal leftRightMarginInches, decimal topBottomMarginInches, int pixelsPerInch)
    {
        this.Contents = [];
        this.font = font;
        this.TopBottomMarginInches = topBottomMarginInches;
        this.LeftRightMarginInches = leftRightMarginInches;
        this.bitmap = new(width: Convert.ToInt32(maxWidthInches * pixelsPerInch), height: Convert.ToInt32(maxHeightInches * pixelsPerInch));
        this.graphics = Graphics.FromImage(bitmap);
        graphics.PageUnit = GraphicsUnit.Inch;
        this.stringFormat = new();
    }

    public List<string> Contents { get; private set; }
    private readonly Bitmap bitmap;
    private readonly Graphics graphics;
    private readonly Font font;
    private readonly StringFormat stringFormat;
    
    public float Width()
    {
        string? longestLine = Contents.MaxBy(line => line.Length);
        Debug.Assert(longestLine != null);
        SizeF stringSize = graphics.MeasureString(text: longestLine, font: font);
        return stringSize.Width; // I expect a line of dactylic hex. to be >1000 pixels, or 3.5 inches
    }
    public float Height()
    {
        string contentsStr = string.Join(Environment.NewLine, Contents);
        Debug.Assert(contentsStr != null);
        SizeF size = graphics.MeasureString(text: contentsStr, font: font);
        return size.Height;
    }
    public decimal LeftRightMarginInches { get; init; }
    public decimal TopBottomMarginInches { get; init; }
    
    public IEnumerator<string> GetEnumerator() => Contents.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Contents).GetEnumerator();
    public override string ToString() => string.Join(Environment.NewLine, Contents);
    public void RemoveFinalTwoLines() => Contents = Contents.SkipLast(2).ToList(); 
}
#pragma warning restore CA1416 // Validate platform compatibility