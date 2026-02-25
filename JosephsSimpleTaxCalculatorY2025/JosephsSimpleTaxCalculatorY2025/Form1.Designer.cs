namespace JosephsSimpleTaxCalculatorY2025
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label2 = new Label();
            label3 = new Label();
            incomeNumericUpDown = new NumericUpDown();
            statesComboBox = new ComboBox();
            filingStatusComboBox = new ComboBox();
            label4 = new Label();
            OldOrBlindcheckBox = new CheckBox();
            CalculateButton = new Button();
            ((System.ComponentModel.ISupportInitialize)incomeNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 9);
            label2.Name = "label2";
            label2.Size = new Size(99, 15);
            label2.TabIndex = 1;
            label2.Text = "Yearly Income:   $";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 68);
            label3.Name = "label3";
            label3.Size = new Size(74, 15);
            label3.TabIndex = 2;
            label3.Text = "Filing Status:";
            // 
            // incomeNumericUpDown
            // 
            incomeNumericUpDown.DecimalPlaces = 2;
            incomeNumericUpDown.Location = new Point(107, 7);
            incomeNumericUpDown.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
            incomeNumericUpDown.Name = "incomeNumericUpDown";
            incomeNumericUpDown.Size = new Size(157, 23);
            incomeNumericUpDown.TabIndex = 3;
            incomeNumericUpDown.ValueChanged += incomeNumericUpDown_ValueChanged;
            // 
            // statesComboBox
            // 
            statesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statesComboBox.FormattingEnabled = true;
            statesComboBox.Items.AddRange(new object[] { "Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut", "Delaware", "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey", "New Mexico", "New York", "North Carolina", "North Dakota", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming" });
            statesComboBox.Location = new Point(107, 36);
            statesComboBox.Name = "statesComboBox";
            statesComboBox.Size = new Size(157, 23);
            statesComboBox.TabIndex = 4;
            statesComboBox.SelectedIndexChanged += statesComboBox_SelectedIndexChanged;
            // 
            // filingStatusComboBox
            // 
            filingStatusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            filingStatusComboBox.FormattingEnabled = true;
            filingStatusComboBox.Items.AddRange(new object[] { "Single", "Married Filing Jointly", "Head of Household", "Married Filing Separately", "Widowed" });
            filingStatusComboBox.Location = new Point(107, 65);
            filingStatusComboBox.Name = "filingStatusComboBox";
            filingStatusComboBox.Size = new Size(157, 23);
            filingStatusComboBox.TabIndex = 5;
            filingStatusComboBox.SelectedIndexChanged += filingStatusComboBox_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 39);
            label4.Name = "label4";
            label4.Size = new Size(36, 15);
            label4.TabIndex = 6;
            label4.Text = "State:";
            // 
            // OldOrBlindcheckBox
            // 
            OldOrBlindcheckBox.AutoSize = true;
            OldOrBlindcheckBox.Location = new Point(107, 94);
            OldOrBlindcheckBox.Name = "OldOrBlindcheckBox";
            OldOrBlindcheckBox.Size = new Size(157, 19);
            OldOrBlindcheckBox.TabIndex = 7;
            OldOrBlindcheckBox.Text = "Additional Age(65)/Blind";
            OldOrBlindcheckBox.UseVisualStyleBackColor = true;
            OldOrBlindcheckBox.CheckedChanged += OldOrBlindcheckBox_CheckedChanged;
            // 
            // CalculateButton
            // 
            CalculateButton.Location = new Point(189, 119);
            CalculateButton.Name = "CalculateButton";
            CalculateButton.Size = new Size(75, 23);
            CalculateButton.TabIndex = 8;
            CalculateButton.Text = "Calculate";
            CalculateButton.UseVisualStyleBackColor = true;
            CalculateButton.Click += CalculateButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(CalculateButton);
            Controls.Add(OldOrBlindcheckBox);
            Controls.Add(label4);
            Controls.Add(filingStatusComboBox);
            Controls.Add(statesComboBox);
            Controls.Add(incomeNumericUpDown);
            Controls.Add(label3);
            Controls.Add(label2);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Joseph's Simple Tax Calculator Y2025";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)incomeNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label3;
        private NumericUpDown incomeNumericUpDown;
        private ComboBox statesComboBox;
        private ComboBox filingStatusComboBox;
        private Label label4;
        private CheckBox OldOrBlindcheckBox;
        private Button CalculateButton;
    }
}
