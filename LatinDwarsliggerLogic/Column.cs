using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace LatinDwarsliggerLogic;
#pragma warning disable CA1416 // Validate platform compatibility
public class Column : IEnumerable<string>
{
    private const int PIXELS_PER_INCH = 320;

    public Column(Font font, decimal maxHeightInches, decimal maxWidthInches, decimal leftRightMarginInches, decimal topBottomMarginInches)
    {
        this.Contents = [];
        this.font = font;
        this.TopBottomMarginInches = topBottomMarginInches;
        this.LeftRightMarginInches = leftRightMarginInches;
        this.bitmap = new(width: Convert.ToInt32(maxWidthInches * PIXELS_PER_INCH), height: Convert.ToInt32(maxHeightInches * PIXELS_PER_INCH));
        this.graphics = Graphics.FromImage(bitmap);
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
        SizeF stringSize = graphics.MeasureString(longestLine, font, int.MaxValue, stringFormat);
        return stringSize.Width / PIXELS_PER_INCH; // Maybe?     
    }
    public float Height()
    {
        return Contents.Count * font.SizeInPoints / Constants.PICA_POINTS_PER_INCH;
    }
    public decimal LeftRightMarginInches { get; init; }
    public decimal TopBottomMarginInches { get; init; }
    
    public IEnumerator<string> GetEnumerator() => Contents.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Contents).GetEnumerator();
    public override string ToString() => string.Join(Environment.NewLine, Contents);
    public void RemoveFinalTwoLines() => Contents = Contents.SkipLast(2).ToList(); 
}
#pragma warning restore CA1416 // Validate platform compatibility