namespace LatinDwarsliggerLogic;
public sealed class HalfSide : IDisposable
{
    public Column LeftColumn { get; init; }
    public Column? RightColumn { get; init; }
    public int ColumnCount => RightColumn == null ? 2 : 1;
    public decimal LeftRightMarginInches { get; init; }
    public decimal TopBottomMarginInches { get; init; }
    private bool disposed;
    public float Width =>
        RightColumn == null ?
        LeftColumn.Width() + Convert.ToSingle(2 * LeftRightMarginInches) :
        LeftColumn.Width() + RightColumn.Width() + Convert.ToSingle(3 * LeftRightMarginInches); // extra margin in the middle
    public HalfSide(Column leftColumn, Column? rightColumn = null)
    {
        this.LeftColumn = leftColumn;
        this.RightColumn = rightColumn;
        disposed = false;
    }

    public override string ToString()
    {
        if (RightColumn == null)
            return string.Join(Environment.NewLine, LeftColumn);

        var zipped = LeftColumn.Zip(RightColumn, resultSelector: (str1, str2) => $"{str1 ?? ""}\t| {str2 ?? ""}");
        return string.Join(Environment.NewLine, zipped);

    }

    public void Dispose()
    {
        if (disposed) return;
        LeftColumn?.Dispose();
        RightColumn?.Dispose();
        GC.SuppressFinalize(this);
        disposed = true;
    }

    ~HalfSide() => Dispose();
}
