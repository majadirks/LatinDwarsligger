using System.Collections;

namespace LatinDwarsliggerLogic;
public class HalfSide
{
    private readonly Column leftColumn;
    private readonly Column? rightColumn;
    public int ColumnCount => rightColumn == null ? 2 : 1;
    public HalfSide(Column leftColumn, Column? rightColumn = null)
    {
        this.leftColumn = leftColumn;
        this.rightColumn = rightColumn;
    }
}

public class Column : IEnumerable<string>
{
    public IEnumerable<string> Contents { get; init; }
    public Column(IEnumerable<string> contents)
    {
        this.Contents = contents.ToList();
    }
    public IEnumerator<string> GetEnumerator() => Contents.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Contents).GetEnumerator();
    
}