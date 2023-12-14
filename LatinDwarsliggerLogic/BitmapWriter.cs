using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
namespace LatinDwarsliggerLogic;

#pragma warning disable CA1416 // Validate platform compatibility
public class BitmapWriter
{

    // https://stackoverflow.com/questions/63704594/render-very-high-quality-text-to-bitmap
    public static Bitmap FromColumn(Column column)
    {
        Font font = column.Font;
        int pixelsPerInch = column.PixelsPerInch;
        float padding = 0.2f * font.Size * pixelsPerInch;

        Bitmap bitmap = new(
            width: Convert.ToInt32((pixelsPerInch * column.WidthInInches()) + 4 * padding) , 
            height: Convert.ToInt32((pixelsPerInch * column.HeightInInches()) + 2 * padding));
        bitmap.SetResolution(pixelsPerInch, pixelsPerInch);
        Graphics graphics = Graphics.FromImage(bitmap);
        
        string text = string.Join(Environment.NewLine, column.Contents);
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        graphics.Clear(Color.White);
        graphics.DrawString(
            s: text, 
            font: font, 
            brush: new SolidBrush(Color.Black), 
            x: padding, 
            y: padding, 
            format: StringFormat.GenericTypographic);

        string filename = column.Contents.First().Replace(" ", "").Substring(0, 10) + ".bmp";
        bitmap.Save(filename);
        return bitmap;
    }
}
#pragma warning restore CA1416 // Validate platform compatibility