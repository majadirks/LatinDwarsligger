using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinDwarsliggerLogic;

public class BitmapWriter
{
    // https://stackoverflow.com/questions/63704594/render-very-high-quality-text-to-bitmap
    //public static Bitmap ConvertTextToImage(string text, Font font)
    //{
        //Bitmap bitmap = new(1, 1);
        //Graphics graphics = Graphics.FromImage(bitmap);
        //StringFormat stringFormat = new();
        //Color backgroundColor = Color.Transparent;
        //Color foregroundColor = Color.Black;
        //SizeF stringSize = graphics.MeasureString(text, font, int.MaxValue, stringFormat);
        //
        //bitmap = new Bitmap((int)stringSize.Width, (int)stringSize.Height);
        //graphics = Graphics.FromImage(bitmap);
        //graphics.CompositingQuality = CompositingQuality.HighQuality;
        //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //graphics.SmoothingMode = SmoothingMode.HighQuality;
        //graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        //graphics.Clear(backgroundColor);
        //
        //int x = 0;
        //if (stringFormat.FormatFlags == StringFormatFlags.DirectionRightToLeft && stringFormat.Alignment == StringAlignment.Center)
        //    x = (int)stringSize.Width / 2;
        //else if (stringFormat.FormatFlags == StringFormatFlags.DirectionRightToLeft) x = (int)stringSize.Width;
        //else if (stringFormat.Alignment == StringAlignment.Center) x += (int)stringSize.Width / 2;
        //
        //graphics.DrawString(text, font, new SolidBrush(foregroundColor), x, 0, stringFormat);
        //return bitmap;
    //}
}
