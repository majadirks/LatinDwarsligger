using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinDwarsliggerLogic
{

    public class ChunkOfText : IEnumerable<string>
    {
        private string[] lines;
        public ChunkOfText(IEnumerable<string> lines)
        {
            this.lines = lines.ToArray();
        }
        public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)lines).GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => lines.GetEnumerator(); 
        public override string ToString() => string.Join(Environment.NewLine, lines);
    }
}
