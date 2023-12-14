﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
namespace LatinDwarsliggerLogic;

#pragma warning disable CA1416 // Validate platform compatibility
public class BitmapWriter
{
    private const int PIXELS_PER_INCH = 320;

    // https://stackoverflow.com/questions/63704594/render-very-high-quality-text-to-bitmap
    public static Bitmap FromColumn(Column column)
    {
        Font font = column.Font;
        float padding = 0.2f * font.Size * PIXELS_PER_INCH;

        Bitmap bitmap = new(
            width: Convert.ToInt32((PIXELS_PER_INCH * column.WidthInInches()) + 4 * padding) , 
            height: Convert.ToInt32((PIXELS_PER_INCH * column.HeightInInches()) + 2 * padding));
        bitmap.SetResolution(PIXELS_PER_INCH, PIXELS_PER_INCH);
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