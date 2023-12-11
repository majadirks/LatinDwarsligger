using System.Collections;
using System.Drawing;

namespace LatinDwarsliggerLogic;
public class HalfSide
{
    public Column leftColumn { get; init; }
    public Column? rightColumn { get; init; }
    public int ColumnCount => rightColumn == null ? 2 : 1;
    public decimal LeftRightMarginInches { get; init; }
    public decimal TopBottomMarginInches { get; init; }
    public float Width =>
        rightColumn == null ?
        leftColumn.Width + Convert.ToSingle(2 * LeftRightMarginInches) :
        leftColumn.Width + rightColumn.Width + Convert.ToSingle(3 * LeftRightMarginInches); // extra margin in the middle
    public HalfSide(Column leftColumn, Column? rightColumn = null)
    {
        this.leftColumn = leftColumn;
        this.rightColumn = rightColumn;
    }

    public override string ToString()
    {
        if (rightColumn == null)
            return string.Join(Environment.NewLine, leftColumn);

        var zipped = leftColumn.Zip(rightColumn, resultSelector: (str1, str2) => $"{str1 ?? ""}\t| {str2 ?? ""}");
        return string.Join(Environment.NewLine, zipped);

    }
}

public class Column : IEnumerable<string>
{
    public List<string> Contents { get; init; }
    private readonly Font font;
    public float Width => Contents.Width(font);
    public float Height => Contents.Height(font);
    public decimal LeftRightMarginInches { get; init; }
    public decimal TopBottomMarginInches { get; init; }
    public Column(Font font, decimal leftRightMarginInches, decimal topBottomMarginInches)
    {
        this.Contents = [];
        this.font = font;
        this.TopBottomMarginInches = topBottomMarginInches;
        this.LeftRightMarginInches = leftRightMarginInches;
    }
    public IEnumerator<string> GetEnumerator() => Contents.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Contents).GetEnumerator();
    public override string ToString() => string.Join(Environment.NewLine, Contents);
    
}