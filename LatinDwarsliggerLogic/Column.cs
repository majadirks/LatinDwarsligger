using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace LatinDwarsliggerLogic;
#pragma warning disable CA1416 // Validate platform compatibility
public sealed class Column : IEnumerable<string>, IDisposable
{
    public Column(Font font, decimal maxHeightInches, decimal maxWidthInches, decimal leftRightMarginInches, decimal topBottomMarginInches, int pixelsPerInch)
    {
        this.Contents = [];
        this.Font = font;
        this.TopBottomMarginInches = topBottomMarginInches;
        this.LeftRightMarginInches = leftRightMarginInches;
        this.bitmap = new(width: Convert.ToInt32(maxWidthInches * pixelsPerInch), height: Convert.ToInt32(maxHeightInches * pixelsPerInch));
        this.graphics = Graphics.FromImage(bitmap);
        this.PixelsPerInch = pixelsPerInch;
        graphics.PageUnit = GraphicsUnit.Inch;
        this.stringFormat = new();
        disposed = false;
    }

    public List<string> Contents { get; private set; }
    public Font Font { get; init; }
    public int PixelsPerInch { get; init; }
    private readonly Bitmap bitmap;

    private readonly Graphics graphics;
    private readonly StringFormat stringFormat;
    private bool disposed;
    
    public float WidthInInches()
    {
        IEnumerable<float> widths = Contents.Select(line => graphics.MeasureString(text: line, font: Font).Width);
        float maxWidth = widths.Max(); // I expect a line of dactylic hexameter in 11pt font to be >1000 pixels, or 3.5 inches
        return maxWidth; 
    }
    public float HeightInInches()
    {
        string contentsStr = string.Join(Environment.NewLine, Contents);
        Debug.Assert(contentsStr != null);
        SizeF size = graphics.MeasureString(text: contentsStr, font: Font);
        return size.Height;
    }
    public decimal LeftRightMarginInches { get; init; }
    public decimal TopBottomMarginInches { get; init; }
    
    public IEnumerator<string> GetEnumerator() => Contents.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Contents).GetEnumerator();
    public override string ToString() => string.Join(Environment.NewLine, Contents);
    public void RemoveFinalTwoLines() => Contents = Contents.SkipLast(2).ToList();

    public void Dispose()
    {
        if (disposed) return;
        graphics?.Dispose();
        bitmap?.Dispose();
        stringFormat?.Dispose();
        // Don't dispose of Font, since it's created outside this class and passed in as a parameter
        GC.SuppressFinalize(this);
        disposed = true;
    }

    ~Column() => Dispose();
}
#pragma warning restore CA1416 // Validate platform compatibility