using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinDwarsliggerLogic;

#pragma warning disable CA1416 // Validate platform compatibility
public static class Utils
{
    private const float POINTS_PER_INCH = 72;  // Use float instead of int to avoid integer division
    public static float Width(this IEnumerable<string> lines, Font font)
        => lines.Select(line => line.Length * font.SizeInPoints / POINTS_PER_INCH).Max();
    public static float Height(this IEnumerable<string> lines, Font font)
        => lines.Count() * font.SizeInPoints / POINTS_PER_INCH;

}
#pragma warning restore CA1416 // Validate platform compatibility

