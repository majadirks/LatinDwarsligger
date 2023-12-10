using System.Collections;
using System.Drawing;

namespace LatinDwarsliggerLogic;

public record ChunkSize(double WidthInInches, double HeightInInches);
public class Paragraph : IEnumerable<string>, IEquatable<Paragraph>
{
    private readonly string[] lines;
    public Paragraph(IEnumerable<string> lines)
    {
        this.lines = lines.ToArray();
    }
    public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)lines).GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => lines.GetEnumerator(); 
    public override string ToString() => string.Join(Environment.NewLine, lines);

    public bool Equals(Paragraph? other)
    {
        if (other == null) return false;
        if (this.lines.Length != other.lines.Length) 
            return false;
        for (int i = 0; i < this.lines.Length; i++)
        {
            string myLine = this.lines[i];
            string otherLine = other.lines[i];
            if (myLine != otherLine) 
                return false;
        }
        return true;
    }

    public override bool Equals(object? obj) 
        => obj != null && obj is Paragraph other && Equals(other);

    public override int GetHashCode()
    {
        int hashCode = 39;
        unchecked
        {
            for (int i = 0; i < lines.Length; i++)
            {
                hashCode = hashCode + 17 * lines[i].GetHashCode();
            }
        }
        return hashCode;
    }

    public ChunkSize GetSize(Font font)
    {
        float width = lines.Width(font);
        float height = lines.Height(font);
        return new(WidthInInches: width, HeightInInches: height);
    }

}
