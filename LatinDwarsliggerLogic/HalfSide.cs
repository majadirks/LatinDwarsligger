using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LatinDwarsliggerLogic;
public class HalfSide
{
    private readonly List<string> leftColumn;
    private readonly List<string>? rightColumn;
    public HalfSide(IEnumerable<string> leftColumn, IEnumerable<string>? rightColumn = null)
    {
        this.leftColumn = leftColumn.ToList();
        this.rightColumn = rightColumn?.ToList(); 
    }
}
