using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
namespace LatinDwarsliggerLogic;

#pragma warning disable CA1416 // Validate platform compatibility
public static class BitmapWriter
{
    private static float GetPadding(Font font, int pixelsPerInch) => 0.2f * font.Size * pixelsPerInch;

    // https://stackoverflow.com/questions/63704594/render-very-high-quality-text-to-bitmap
    public static Bitmap ToBitmap(this Column column)
    {
        Font font = column.Font;
        int pixelsPerInch = column.PixelsPerInch;
        float padding = GetPadding(font, pixelsPerInch);
        string text = string.Join(Environment.NewLine, column.Contents);

        Bitmap bitmap = new(
            width: Convert.ToInt32(pixelsPerInch * column.WidthInInches()) + 1 , 
            height: Convert.ToInt32((pixelsPerInch * column.HeightInInches()) + padding));
        bitmap.SetResolution(pixelsPerInch, pixelsPerInch);
        Graphics graphics = FromBitmap(bitmap);
        graphics.DrawString(
            s: text, 
            font: font, 
            brush: new SolidBrush(Color.Black), 
            x: padding, 
            y: padding, 
            format: StringFormat.GenericTypographic);

        // debug code
        //string filename = column.Contents.First().Replace(" ", "").Substring(0, 10) + ".bmp";
        //bitmap.Save(filename);
        return bitmap;
    }

    public static Bitmap ToBitmap(this HalfSide halfSide, Arranger arranger)
    {
        Font font = arranger.Font;
        int pixelsPerInch = arranger.PixelsPerInch;
        float padding = GetPadding(font, pixelsPerInch);
        Bitmap leftCol = halfSide.LeftColumn.ToBitmap();
        Bitmap? rightCol = halfSide.RightColumn?.ToBitmap();

        Bitmap bitmap = new(
            width: Convert.ToInt32(pixelsPerInch * (arranger.PageWidthInches + arranger.LeftRightMarginInches * 2)),
            height: Convert.ToInt32(pixelsPerInch * (arranger.HalfSideHeightInches + arranger.TopBottomMarginInches * 2)));
        bitmap.SetResolution(pixelsPerInch, pixelsPerInch);

        Graphics graphics = FromBitmap(bitmap);

        float leftColX = arranger.LeftRightMarginInches * pixelsPerInch;
        float y = arranger.TopBottomMarginInches * pixelsPerInch;
        graphics.DrawImage(image: leftCol, x: leftColX, y: y);

        if (rightCol != null)
        {
            int rightColX = Convert.ToInt32((arranger.PageWidthInches * pixelsPerInch / 2.0) + (arranger.LeftRightMarginInches * pixelsPerInch / 2.0));
            graphics.DrawImage(
                image: rightCol, 
                x: rightColX, 
                y: y);
        }

        // debug code
        //string path = halfSide.LeftColumn.Contents.First().Substring(0, 10) + "_halfSide.bmp";
        //bitmap.Save(path);

        return bitmap;
    }

    private static Graphics FromBitmap(Bitmap bitmap)
    {
        Graphics graphics = Graphics.FromImage(bitmap);
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        graphics.Clear(Color.White);
        return graphics;
    }
}

#pragma warning restore CA1416 // Validate platform compatibility