using System.Collections;
using System.Diagnostics;
using System.Drawing;
namespace LatinDwarsliggerLogic;

public sealed class Column : IEnumerable<string>
{
    public Column(Font font, int pixelsPerInch, StringMeasurer measureString)
    {
        this.Contents = [];
        this.Font = font;
        this.PixelsPerInch = pixelsPerInch;
        this.measureString = measureString;
    }

    public List<string> Contents { get; private set; }
    public Font Font { get; init; }
    public int PixelsPerInch { get; init; }
    private readonly StringMeasurer measureString;
    
    public float WidthInInches()
    {
        IEnumerable<float> widths = Contents.Select(line => measureString(line).Width);
        float maxWidth = widths.Max(); // I expect a line of dactylic hexameter in 11pt font to be >1000 pixels, or 3.5 inches
        return maxWidth; 
    }
    public float HeightInInches()
    {
        string contentsStr = string.Join(Environment.NewLine, Contents);
        Debug.Assert(contentsStr != null);
        SizeF size = measureString(text: contentsStr);
        return size.Height;
    }
    
    public IEnumerator<string> GetEnumerator() => Contents.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Contents).GetEnumerator();
    public override string ToString() => string.Join(Environment.NewLine, Contents);
    public void RemoveFinalTwoLines() => Contents = Contents.SkipLast(2).ToList();
}