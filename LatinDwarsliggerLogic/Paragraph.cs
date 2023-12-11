using System.Collections;
using System.Drawing;

namespace LatinDwarsliggerLogic;

public record ParagraphSize(double WidthInInches, double HeightInInches);
public class Paragraph : IEnumerable<string>, IEquatable<Paragraph>
{
    public string[] Lines { get; init; }
    public Paragraph(IEnumerable<string> lines)
    {
        this.Lines = lines.ToArray();
    }
    public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)Lines).GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => Lines.GetEnumerator(); 
    public override string ToString() => string.Join(Environment.NewLine, Lines);

    public bool Equals(Paragraph? other)
    {
        if (other == null) return false;
        if (this.Lines.Length != other.Lines.Length) 
            return false;
        for (int i = 0; i < this.Lines.Length; i++)
        {
            string myLine = this.Lines[i];
            string otherLine = other.Lines[i];
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
            for (int i = 0; i < Lines.Length; i++)
            {
                hashCode = hashCode + 17 * Lines[i].GetHashCode();
            }
        }
        return hashCode;
    }

    public ParagraphSize GetSize(Font font)
    {
        float width = Lines.Width(font);
        float height = Lines.Height(font);
        return new(WidthInInches: width, HeightInInches: height);
    }

}
