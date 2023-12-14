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
public record PaperSheet(HalfSide SideA, HalfSide? SideB, HalfSide? SideC, HalfSide? SideD);
