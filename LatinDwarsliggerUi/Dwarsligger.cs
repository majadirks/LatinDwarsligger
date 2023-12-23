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

    private void goButton_Click(object sender, EventArgs e)
    {
        // todo: validate inputs
        var result = saveFileDialog.ShowDialog();
        ;
    }
}

/*
*   public static Arranger Default = new (
        fontFamilyName:"Arial", 
        emSizePoints: 11, 
        pageDoubleHeightInches: 8.5f, 
        pageWidthInches: 8.5f, 
        leftRightMarginInches: 0.2f, 
        topBottomMarginInches: 0.2f,
        pixelsPerInch: 320);
*/