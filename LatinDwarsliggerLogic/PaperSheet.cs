using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinDwarsliggerLogic
{
    /// <summary>
    /// Represents a two-sided piece of paper to be printed and folded:
    /// 
    /// Consecutive pages A, B, C, D should come out looking like

    /// -------  -------    
    /// |  B  |	 | ∀  |
    /// |-----|	 |-----|
    /// |  C  |	 | (|  | (upside-down D)
    /// ------	 -------

    /// </summary>
    public sealed class PaperSheet : IDisposable
    {
        public PaperSheet(HalfSide sideA, HalfSide? sideB, HalfSide? sideC, HalfSide? sideD)
        {
            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
            SideD = sideD;
            disposed = false;
        }

        public HalfSide SideA { get; init; }
        public HalfSide? SideB { get; init; }
        public HalfSide? SideC { get; init; }
        public HalfSide? SideD { get; init; }
        private bool disposed;

        public void Dispose()
        {
            if (disposed) return;
            SideA?.Dispose();
            SideB?.Dispose();
            SideC?.Dispose();
            SideD?.Dispose();
            GC.SuppressFinalize(this))
            disposed = true;
        }

        ~PaperSheet() => Dispose();
    }
}
