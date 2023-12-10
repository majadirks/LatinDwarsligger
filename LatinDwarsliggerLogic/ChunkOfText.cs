using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LatinDwarsliggerLogic
{

    public class ChunkOfText : IEnumerable<string>, IEquatable<ChunkOfText>
    {
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
        public (float width, float height) GetSize(Graphics graphics, Font font)
        {
            IEnumerable<SizeF> widths = lines.Select(line => graphics.MeasureString(line, font, line.Length, StringFormat.GenericTypographic));
            float maxWidth = widths.Select(w => w.Width).Max();
                float height = widths.Select(w => w.Height).Sum();
                return (maxWidth, height);
        }
#pragma warning restore CA1416 // Validate platform compatibility

    }
}
