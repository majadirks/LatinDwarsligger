namespace LatinDwarsliggerUi
{
    partial class Dwarsligger
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            fontCb = new ComboBox();
            Font = new Label();
            fontSizeTextbox = new TextBox();
            fontSizeLabel = new Label();
            pageWidthTextbox = new TextBox();
            pageWidthLabel = new Label();
            pageHeightLabel = new Label();
            pageHeightTextbox = new TextBox();
            leftRightMarginLabel = new Label();
            topBottomMarginLabel = new Label();
            leftRightMarginTextbox = new TextBox();
            topBottomMarginTextbox = new TextBox();
            label1 = new Label();
            ppiTextbox = new TextBox();
            urlLabel = new Label();
            urlTextbox = new TextBox();
            saveFileDialog = new SaveFileDialog();
            goButton = new Button();
            SuspendLayout();
            // 
            // fontCb
            // 
            fontCb.FormattingEnabled = true;
            fontCb.Items.AddRange(new object[] { "Arial" });
            fontCb.Location = new Point(160, 47);
            fontCb.Name = "fontCb";
            fontCb.Size = new Size(273, 49);
            fontCb.TabIndex = 0;
            // 
            // Font
            // 
            Font.AutoSize = true;
            Font.Location = new Point(48, 41);
            Font.Name = "Font";
            Font.Size = new Size(78, 41);
            Font.TabIndex = 1;
            Font.Text = "Font";
            // 
            // fontSizeTextbox
            // 
            fontSizeTextbox.Location = new Point(555, 49);
            fontSizeTextbox.Name = "fontSizeTextbox";
            fontSizeTextbox.Size = new Size(86, 47);
            fontSizeTextbox.TabIndex = 2;
            // 
            // fontSizeLabel
            // 
            fontSizeLabel.AutoSize = true;
            fontSizeLabel.Location = new Point(466, 41);
            fontSizeLabel.Name = "fontSizeLabel";
            fontSizeLabel.Size = new Size(71, 41);
            fontSizeLabel.TabIndex = 3;
            fontSizeLabel.Text = "Size";
            // 
            // pageWidthTextbox
            // 
            pageWidthTextbox.Location = new Point(466, 159);
            pageWidthTextbox.Name = "pageWidthTextbox";
            pageWidthTextbox.Size = new Size(119, 47);
            pageWidthTextbox.TabIndex = 4;
            // 
            // pageWidthLabel
            // 
            pageWidthLabel.AutoSize = true;
            pageWidthLabel.Location = new Point(61, 159);
            pageWidthLabel.Name = "pageWidthLabel";
            pageWidthLabel.Size = new Size(281, 41);
            pageWidthLabel.TabIndex = 5;
            pageWidthLabel.Text = "Page Width (inches)";
            // 
            // pageHeightLabel
            // 
            pageHeightLabel.AutoSize = true;
            pageHeightLabel.Location = new Point(61, 243);
            pageHeightLabel.Name = "pageHeightLabel";
            pageHeightLabel.Size = new Size(290, 41);
            pageHeightLabel.TabIndex = 6;
            pageHeightLabel.Text = "Page Height (inches)";
            // 
            // pageHeightTextbox
            // 
            pageHeightTextbox.Location = new Point(466, 243);
            pageHeightTextbox.Name = "pageHeightTextbox";
            pageHeightTextbox.Size = new Size(119, 47);
            pageHeightTextbox.TabIndex = 7;
            // 
            // leftRightMarginLabel
            // 
            leftRightMarginLabel.AutoSize = true;
            leftRightMarginLabel.Location = new Point(57, 323);
            leftRightMarginLabel.Name = "leftRightMarginLabel";
            leftRightMarginLabel.Size = new Size(361, 41);
            leftRightMarginLabel.TabIndex = 8;
            leftRightMarginLabel.Text = "Left/Right Margin (inches)";
            // 
            // topBottomMarginLabel
            // 
            topBottomMarginLabel.AutoSize = true;
            topBottomMarginLabel.Location = new Point(57, 404);
            topBottomMarginLabel.Name = "topBottomMarginLabel";
            topBottomMarginLabel.Size = new Size(390, 41);
            topBottomMarginLabel.TabIndex = 9;
            topBottomMarginLabel.Text = "Top/Bottom Margin (inches)";
            // 
            // leftRightMarginTextbox
            // 
            leftRightMarginTextbox.Location = new Point(466, 323);
            leftRightMarginTextbox.Name = "leftRightMarginTextbox";
            leftRightMarginTextbox.Size = new Size(119, 47);
            leftRightMarginTextbox.TabIndex = 10;
            // 
            // topBottomMarginTextbox
            // 
            topBottomMarginTextbox.Location = new Point(466, 404);
            topBottomMarginTextbox.Name = "topBottomMarginTextbox";
            topBottomMarginTextbox.Size = new Size(119, 47);
            topBottomMarginTextbox.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(61, 501);
            label1.Name = "label1";
            label1.Size = new Size(206, 41);
            label1.TabIndex = 12;
            label1.Text = "Pixels Per Inch";
            // 
            // ppiTextbox
            // 
            ppiTextbox.Location = new Point(273, 495);
            ppiTextbox.Name = "ppiTextbox";
            ppiTextbox.Size = new Size(119, 47);
            ppiTextbox.TabIndex = 13;
            // 
            // urlLabel
            // 
            urlLabel.AutoSize = true;
            urlLabel.Location = new Point(818, 54);
            urlLabel.Name = "urlLabel";
            urlLabel.Size = new Size(472, 41);
            urlLabel.TabIndex = 14;
            urlLabel.Text = "Enter URL from thelatinlibrary.com";
            // 
            // urlTextbox
            // 
            urlTextbox.Location = new Point(819, 105);
            urlTextbox.Name = "urlTextbox";
            urlTextbox.Size = new Size(861, 47);
            urlTextbox.TabIndex = 15;
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "PDF files|*.pdf";
            // 
            // goButton
            // 
            goButton.Location = new Point(835, 217);
            goButton.Name = "goButton";
            goButton.Size = new Size(246, 110);
            goButton.TabIndex = 16;
            goButton.Text = "Go!";
            goButton.UseVisualStyleBackColor = true;
            goButton.Click += goButton_Click;
            // 
            // Dwarsligger
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1881, 647);
            Controls.Add(goButton);
            Controls.Add(urlTextbox);
            Controls.Add(urlLabel);
            Controls.Add(ppiTextbox);
            Controls.Add(label1);
            Controls.Add(topBottomMarginTextbox);
            Controls.Add(leftRightMarginTextbox);
            Controls.Add(topBottomMarginLabel);
            Controls.Add(leftRightMarginLabel);
            Controls.Add(pageHeightTextbox);
            Controls.Add(pageHeightLabel);
            Controls.Add(pageWidthLabel);
            Controls.Add(pageWidthTextbox);
            Controls.Add(fontSizeLabel);
            Controls.Add(fontSizeTextbox);
            Controls.Add(Font);
            Controls.Add(fontCb);
            Name = "Dwarsligger";
            Text = "Dwarsligger";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox fontCb;
        private Label Font;
        private TextBox fontSizeTextbox;
        private Label fontSizeLabel;
        private TextBox pageWidthTextbox;
        private Label pageWidthLabel;
        private Label pageHeightLabel;
        private TextBox pageHeightTextbox;
        private Label leftRightMarginLabel;
        private Label topBottomMarginLabel;
        private TextBox leftRightMarginTextbox;
        private TextBox topBottomMarginTextbox;
        private Label label1;
        private TextBox ppiTextbox;
        private Label urlLabel;
        private TextBox urlTextbox;
        private SaveFileDialog saveFileDialog;
        private Button goButton;
    }
}
