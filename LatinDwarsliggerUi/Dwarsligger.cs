using LatinDwarsliggerLogic;

namespace LatinDwarsliggerUi;

public partial class Dwarsligger : Form
{
    public Dwarsligger()
    {
        InitializeComponent();
        using Arranger defaultArranger = Arranger.Default;
        fontCb.Text = defaultArranger.Font.Name;
        fontSizeTextbox.Text = defaultArranger.FontSizePoints.ToString();
        pageWidthTextbox.Text = defaultArranger.PageWidthInches.ToString();
        pageHeightTextbox.Text = defaultArranger.PageDoubleHeightInches.ToString();
        leftRightMarginTextbox.Text = defaultArranger.LeftRightMarginInches.ToString();
        topBottomMarginTextbox.Text = defaultArranger.TopBottomMarginInches.ToString();
        ppiTextbox.Text = defaultArranger.PixelsPerInch.ToString();
        urlTextbox.Text = @"https://www.thelatinlibrary.com/vergil/aen1.shtml";
    }

    private async void goButton_Click(object sender, EventArgs e)
    {
        goButton.Enabled = false;
        using Arranger? arr = await ArrangerFromInputs();
        if (arr == null)
        {
            goButton.Enabled = true;
            return;
        }
        var result = saveFileDialog.ShowDialog();
        string filename = saveFileDialog.FileName;
        if (string.IsNullOrWhiteSpace(filename))
        {
            goButton.Enabled = true;
            return;
        }
        string url = urlTextbox.Text.Trim();

        Log("Parsing HTML...");
        var paragraphs = await HtmlCleaner.FormatHtmlFromUrl(url);
        Log($"Arranging {paragraphs.Count()} paragraphs into columns ...");
        var columns = await Task.Run(() => arr.ArrangeParagraphsIntoColumns(paragraphs));
        Log($"Arranging {columns.Count()} columns into half-sides...");
        var halfSides = await Task.Run(() => arr.ArrangeColumnsIntoHalfSides(columns));
        Log($"Arranging {halfSides.Count()} half-sides into sheets...");
        var pages = await Task.Run(() => arr.ArrangeHalfSidesIntoPaperSheets(halfSides).ToArray());

        Log("Generating images...");

        List<PaperSheetImages> psis = [];
        for (int i = 0; i < pages.Length; i++)
        {
            Log($"\tGenerating image {i + 1} of {pages.Length}...");
            PaperSheet page = pages[i];
            var bitmap = await Task.Run(() => page.ToBitmaps(arr));
            psis.Add(bitmap);
        }

        Log($"Generating PDF '{filename}'...");
        IProgress<string> logger = new Progress<string>(str => Log($"\t{str}"));

        try
        {
            await Task.Run(() => DwarsliggerPdf.GeneratePdf(filename, psis, logger));
            Log("Done. Please print PDF double-sided (flip on short edge).");
        }
        catch (Exception ex)
        {
            Log(ex.Message);
        }
        finally
        {
            goButton.Enabled = true;
        }

    }

    private void Log(string msg)
    {
        logTextbox.AppendText(msg + Environment.NewLine); // first line, no newline nee
        logTextbox.Update();
    }

    private async Task<Arranger?> ArrangerFromInputs()
    {
        string font = fontCb.Text;
        bool validFontSize = float.TryParse(fontSizeTextbox.Text, out float fontSize);
        bool validPageWidth = float.TryParse(pageWidthTextbox.Text, out float pageWidth);
        bool validPageHeight = float.TryParse(pageHeightTextbox.Text, out float pageHeight);
        bool validLeftRightMargin = float.TryParse(leftRightMarginTextbox.Text, out float leftRightMargin);
        bool validTopBottomMargin = float.TryParse(topBottomMarginTextbox.Text, out float topBottomMargin);
        bool validPpi = int.TryParse(ppiTextbox.Text, out int ppi);


        if (!validFontSize || !validPageWidth || !validPageHeight || !validLeftRightMargin || !validTopBottomMargin || !validPpi)
        {
            MessageBox.Show("At least one of the numerical inputs was invalid.");
            return null;
        }

        bool validUrl = await ValidUrlAsync(urlTextbox.Text.Trim());
        if (!validUrl)
        {
            MessageBox.Show("Failed to fetch data from the url.");
            return null;
        }

        return new Arranger(
            fontFamilyName: font,
            emSizePoints: fontSize,
            pageDoubleHeightInches: pageHeight,
            pageWidthInches: pageWidth,
            leftRightMarginInches: leftRightMargin,
            topBottomMarginInches: topBottomMargin,
            pixelsPerInch: ppi);
        
    }

    private static async Task<bool> ValidUrlAsync(string url)
    {
        try
        {
            HttpClient client = new();
            string html = await client.GetStringAsync(url);
            return !string.IsNullOrWhiteSpace(html);
        }
        catch
        {
            return false;
        }

    }

}
