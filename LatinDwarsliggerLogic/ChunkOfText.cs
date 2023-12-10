using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LatinDwarsliggerLogic;

public record ChunkSize(double WidthInInches, double HeightInInches);
public class ChunkOfText : IEnumerable<string>, IEquatable<ChunkOfText>
{
    private const float POINTS_PER_INCH = 72;  // Use float instead of int to avoid integer division
    private readonly string[] lines;
    public ChunkOfText(IEnumerable<string> lines)
    {
        this.lines = lines.ToArray();
    }
    public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)lines).GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => lines.GetEnumerator(); 
    public override string ToString() => string.Join(Environment.NewLine, lines);

    public bool Equals(ChunkOfText? other)
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
        => obj != null && obj is ChunkOfText other && Equals(other);

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

#pragma warning disable CA1416 // Validate platform compatibility
    public ChunkSize GetSize(Font font)
    {
        float width = lines.Select(line => line.Length * font.SizeInPoints / POINTS_PER_INCH).Max();
        float height = lines.Length * font.SizeInPoints / POINTS_PER_INCH;
        return new(WidthInInches: width, HeightInInches: height);
    }
#pragma warning restore CA1416 // Validate platform compatibility

}
