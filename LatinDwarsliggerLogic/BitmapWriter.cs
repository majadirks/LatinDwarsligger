using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
namespace LatinDwarsliggerLogic;

#pragma warning disable CA1416 // Validate platform compatibility
public record PaperSheetImages(Bitmap SideASideD, Bitmap? SideBSideC);
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

    public static PaperSheetImages ToBitmaps(this PaperSheet paperSheet, Arranger arranger)
    {
        Font font = arranger.Font;
        int pixelsPerInch = arranger.PixelsPerInch;
        float padding = GetPadding(font, pixelsPerInch);

        Bitmap sideASideD = paperSheet.GetSideASideDBitmap(arranger);
        Bitmap? sideBSideC = paperSheet.GetSideBSideCBitmap(arranger);   
        return new PaperSheetImages(sideASideD, sideBSideC);
    }

    private static Bitmap DrawSmallDotInLowerRightHandCorner(this Bitmap sideA, int pixelsPerInch)
    {
        float x = sideA.Width - 0.2f * pixelsPerInch;
        float y = sideA.Height - 0.2f * pixelsPerInch;
        float diameter = 0.03f * pixelsPerInch;
        using Graphics g = Graphics.FromImage(sideA);
        using Brush brush = new SolidBrush(Color.Black);
        g.FillEllipse(brush, x: x, y: y, width: diameter, height: diameter);
        return sideA;
    }

    private static Bitmap DrawLineInMiddleOfPage(this Bitmap pageSide)
    {
        using Graphics g = Graphics.FromImage(pageSide);
        using Pen pen = new(Color.Black);
        Point start = new(x: 0, y: pageSide.Height / 2);
        Point end = new Point(x: pageSide.Width, y: pageSide.Height / 2);
        g.DrawLine(pen, start, end);
        return pageSide;
    }

    private static Bitmap GetSideASideDBitmap(this PaperSheet paperSheet, Arranger arranger)
    {
        int pixelsPerInch = arranger.PixelsPerInch;
        Bitmap sideA = paperSheet.SideA.ToBitmap(arranger);
        sideA = sideA.DrawSmallDotInLowerRightHandCorner(pixelsPerInch);
        
        sideA.RotateFlip(RotateFlipType.Rotate180FlipNone);
        Bitmap? sideD = paperSheet.SideD?.ToBitmap(arranger);
        sideD?.RotateFlip(RotateFlipType.Rotate180FlipNone);
        
        // A above D, both upside-down
        Bitmap sideASideD;
        if (sideD != null)
        {
            sideASideD = new(
            width: Math.Max(Convert.ToInt32(arranger.PageWidthInches * pixelsPerInch), Math.Max(sideA.Width, sideD.Width)),
            height: Math.Max(Convert.ToInt32(arranger.PageDoubleHeightInches * pixelsPerInch), sideA.Height + sideD.Height));
            sideASideD.SetResolution(pixelsPerInch, pixelsPerInch);
            Graphics g = FromBitmap(sideASideD);
            g.DrawImage(sideA, x: 0, y: 0);
            g.DrawImage(sideD, x: 0, y: (arranger.HalfSideHeightInches + 2 * arranger.TopBottomMarginInches) * pixelsPerInch);
        }
        else // side A not null, side D is null
        {
            sideASideD = new(
            width: Math.Max(Convert.ToInt32(arranger.PageWidthInches * pixelsPerInch), sideA.Width),
            height: Math.Max(Convert.ToInt32(arranger.PageDoubleHeightInches * pixelsPerInch), sideA.Height));
            sideASideD.SetResolution(pixelsPerInch, pixelsPerInch);
            Graphics g = FromBitmap(sideASideD);
            g.DrawImage(sideA, x: 0, y: 0);
        }
        sideASideD.DrawLineInMiddleOfPage();
        return sideASideD;
    }


    private static Bitmap? GetSideBSideCBitmap(this PaperSheet paperSheet, Arranger arranger)
    {
        Bitmap? sideB = paperSheet.SideB?.ToBitmap(arranger);
        Bitmap? sideC = paperSheet.SideC?.ToBitmap(arranger);
        int pixelsPerInch = arranger.PixelsPerInch;
        // B above C, right-side-up
        Bitmap? sideBSideC = null;
        if (sideB != null)
        {
            if (sideC != null)
            {
                sideBSideC = new(
                width: Math.Max(Convert.ToInt32(arranger.PageWidthInches * pixelsPerInch), Math.Max(sideB.Width, sideC.Width)),
                height: Math.Max(Convert.ToInt32(arranger.PageDoubleHeightInches * pixelsPerInch), sideB.Height + sideC.Height));
                sideBSideC.SetResolution(pixelsPerInch, pixelsPerInch);

                Graphics g = FromBitmap(sideBSideC);
                g.DrawImage(sideB, x: 0, y: 0);
                g.DrawImage(sideC, x: 0, y: (arranger.HalfSideHeightInches + 2 * arranger.TopBottomMarginInches) * pixelsPerInch);
            }
            else // side B not null, side C is nulll
            {
                sideBSideC = new(
                width: Math.Max(Convert.ToInt32(arranger.PageWidthInches * pixelsPerInch), sideB.Width),
                height: Math.Max(Convert.ToInt32(arranger.PageDoubleHeightInches * pixelsPerInch), sideB.Height));
                sideBSideC.SetResolution(pixelsPerInch, pixelsPerInch);

                Graphics g = FromBitmap(sideBSideC);
                g.DrawImage(sideB, x: 0, y: 0);
            }
        }
        return sideBSideC;
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