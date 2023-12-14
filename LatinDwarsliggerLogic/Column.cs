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
        this.Bitmap = new(width: Convert.ToInt32(maxWidthInches * pixelsPerInch), height: Convert.ToInt32(maxHeightInches * pixelsPerInch));
        this.graphics = Graphics.FromImage(Bitmap);
        graphics.PageUnit = GraphicsUnit.Inch;
        this.stringFormat = new();
        disposed = false;
    }

    public List<string> Contents { get; private set; }
    public Font Font { get; init; }
    public Bitmap Bitmap { get; init; }

    private readonly Graphics graphics;
    private readonly StringFormat stringFormat;
    private bool disposed;
    
    public float Width()
    {
        string? longestLine = Contents.MaxBy(line => line.Length);
        Debug.Assert(longestLine != null);
        SizeF stringSize = graphics.MeasureString(text: longestLine, font: Font);
        return stringSize.Width; // I expect a line of dactylic hexameter in 11pt font to be >1000 pixels, or 3.5 inches 
    }
    public float Height()
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
        Bitmap?.Dispose();
        stringFormat?.Dispose();
        GC.SuppressFinalize(this);
        disposed = true;
    }

    ~Column() => Dispose();
}
#pragma warning restore CA1416 // Validate platform compatibility