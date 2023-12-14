namespace LatinDwarsliggerLogic;

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
public sealed class PaperSheet
{
    public PaperSheet(HalfSide sideA, HalfSide? sideB, HalfSide? sideC, HalfSide? sideD)
    {
        SideA = sideA;
        SideB = sideB;
        SideC = sideC;
        SideD = sideD;
    }

    public HalfSide SideA { get; init; }
    public HalfSide? SideB { get; init; }
    public HalfSide? SideC { get; init; }
    public HalfSide? SideD { get; init; }
}
