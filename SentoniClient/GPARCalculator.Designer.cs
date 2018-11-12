namespace SentoniClient
{
    partial class GPARCalculator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.textBoxEquity = new System.Windows.Forms.TextBox();
            this.textBoxATMPutDelta = new System.Windows.Forms.TextBox();
            this.textBoxOTMPutDelta = new System.Windows.Forms.TextBox();
            this.textBoxATMCallDelta = new System.Windows.Forms.TextBox();
            this.textBoxIndexPrice = new System.Windows.Forms.TextBox();
            this.textBoxMultiplier = new System.Windows.Forms.TextBox();
            this.labelCallsToSell = new System.Windows.Forms.Label();
            this.labelPutsToSell = new System.Windows.Forms.Label();
            this.labelPutsToBuy = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxTargetDelta = new System.Windows.Forms.TextBox();
            this.labelMessage = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelOTMStrike = new System.Windows.Forms.Label();
            this.textBoxNumVerticals = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonFirstTrade = new System.Windows.Forms.RadioButton();
            this.radioButtonRoll = new System.Windows.Forms.RadioButton();
            this.radioButtonSell = new System.Windows.Forms.RadioButton();
            this.radioButtonBuy = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelCall1 = new System.Windows.Forms.Label();
            this.labelCall2 = new System.Windows.Forms.Label();
            this.labelCall3 = new System.Windows.Forms.Label();
            this.labelCall4 = new System.Windows.Forms.Label();
            this.labelCall5 = new System.Windows.Forms.Label();
            this.labelCall5Trade = new System.Windows.Forms.Label();
            this.labelCall4Trade = new System.Windows.Forms.Label();
            this.labelCall3Trade = new System.Windows.Forms.Label();
            this.labelCall2Trade = new System.Windows.Forms.Label();
            this.labelCall1Trade = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.textBoxCall1Position = new System.Windows.Forms.TextBox();
            this.textBoxCall2Position = new System.Windows.Forms.TextBox();
            this.textBoxCall3Position = new System.Windows.Forms.TextBox();
            this.textBoxCall4Position = new System.Windows.Forms.TextBox();
            this.textBoxCall5Position = new System.Windows.Forms.TextBox();
            this.textBoxCall5Delta = new System.Windows.Forms.TextBox();
            this.textBoxCall4Delta = new System.Windows.Forms.TextBox();
            this.textBoxCall3Delta = new System.Windows.Forms.TextBox();
            this.textBoxCall2Delta = new System.Windows.Forms.TextBox();
            this.textBoxCall1Delta = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxVerticalDelta = new System.Windows.Forms.TextBox();
            this.textBoxDeltasToTrade = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Equity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Multiplier";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(82, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Index Price";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(78, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "At-the-money Call Delta";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(79, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "At-the-money Put Delta";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(60, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Out-of-the-money Put Delta";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(269, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Calls to Sell";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(270, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Puts to Sell";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(269, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Puts to Buy";
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCalculate.Location = new System.Drawing.Point(498, 187);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(127, 23);
            this.buttonCalculate.TabIndex = 9;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // textBoxEquity
            // 
            this.textBoxEquity.Location = new System.Drawing.Point(148, 17);
            this.textBoxEquity.Name = "textBoxEquity";
            this.textBoxEquity.Size = new System.Drawing.Size(147, 20);
            this.textBoxEquity.TabIndex = 10;
            this.textBoxEquity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxEquity.TextChanged += new System.EventHandler(this.textBoxEquity_TextChanged);
            // 
            // textBoxATMPutDelta
            // 
            this.textBoxATMPutDelta.Location = new System.Drawing.Point(201, 49);
            this.textBoxATMPutDelta.Name = "textBoxATMPutDelta";
            this.textBoxATMPutDelta.Size = new System.Drawing.Size(54, 20);
            this.textBoxATMPutDelta.TabIndex = 11;
            this.textBoxATMPutDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxOTMPutDelta
            // 
            this.textBoxOTMPutDelta.Location = new System.Drawing.Point(201, 79);
            this.textBoxOTMPutDelta.Name = "textBoxOTMPutDelta";
            this.textBoxOTMPutDelta.Size = new System.Drawing.Size(54, 20);
            this.textBoxOTMPutDelta.TabIndex = 12;
            this.textBoxOTMPutDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxATMCallDelta
            // 
            this.textBoxATMCallDelta.Location = new System.Drawing.Point(201, 19);
            this.textBoxATMCallDelta.Name = "textBoxATMCallDelta";
            this.textBoxATMCallDelta.Size = new System.Drawing.Size(54, 20);
            this.textBoxATMCallDelta.TabIndex = 13;
            this.textBoxATMCallDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxIndexPrice
            // 
            this.textBoxIndexPrice.Location = new System.Drawing.Point(148, 107);
            this.textBoxIndexPrice.Name = "textBoxIndexPrice";
            this.textBoxIndexPrice.Size = new System.Drawing.Size(147, 20);
            this.textBoxIndexPrice.TabIndex = 14;
            this.textBoxIndexPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxIndexPrice.TextChanged += new System.EventHandler(this.textBoxIndexPrice_TextChanged);
            // 
            // textBoxMultiplier
            // 
            this.textBoxMultiplier.Location = new System.Drawing.Point(148, 47);
            this.textBoxMultiplier.Name = "textBoxMultiplier";
            this.textBoxMultiplier.Size = new System.Drawing.Size(147, 20);
            this.textBoxMultiplier.TabIndex = 15;
            this.textBoxMultiplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelCallsToSell
            // 
            this.labelCallsToSell.AutoSize = true;
            this.labelCallsToSell.Location = new System.Drawing.Point(332, 22);
            this.labelCallsToSell.Name = "labelCallsToSell";
            this.labelCallsToSell.Size = new System.Drawing.Size(0, 13);
            this.labelCallsToSell.TabIndex = 16;
            // 
            // labelPutsToSell
            // 
            this.labelPutsToSell.AutoSize = true;
            this.labelPutsToSell.Location = new System.Drawing.Point(333, 52);
            this.labelPutsToSell.Name = "labelPutsToSell";
            this.labelPutsToSell.Size = new System.Drawing.Size(0, 13);
            this.labelPutsToSell.TabIndex = 17;
            // 
            // labelPutsToBuy
            // 
            this.labelPutsToBuy.AutoSize = true;
            this.labelPutsToBuy.Location = new System.Drawing.Point(334, 82);
            this.labelPutsToBuy.Name = "labelPutsToBuy";
            this.labelPutsToBuy.Size = new System.Drawing.Size(0, 13);
            this.labelPutsToBuy.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(36, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Target Delta Percent";
            // 
            // textBoxTargetDelta
            // 
            this.textBoxTargetDelta.Location = new System.Drawing.Point(148, 77);
            this.textBoxTargetDelta.Name = "textBoxTargetDelta";
            this.textBoxTargetDelta.Size = new System.Drawing.Size(147, 20);
            this.textBoxTargetDelta.TabIndex = 20;
            this.textBoxTargetDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxTargetDelta.TextChanged += new System.EventHandler(this.textBoxTargetDelta_TextChanged);
            // 
            // labelMessage
            // 
            this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMessage.AutoSize = true;
            this.labelMessage.ForeColor = System.Drawing.Color.Red;
            this.labelMessage.Location = new System.Drawing.Point(12, 484);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(0, 13);
            this.labelMessage.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(127, 110);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "2% Put strike";
            // 
            // labelOTMStrike
            // 
            this.labelOTMStrike.AutoSize = true;
            this.labelOTMStrike.Location = new System.Drawing.Point(206, 110);
            this.labelOTMStrike.Name = "labelOTMStrike";
            this.labelOTMStrike.Size = new System.Drawing.Size(0, 13);
            this.labelOTMStrike.TabIndex = 23;
            // 
            // textBoxNumVerticals
            // 
            this.textBoxNumVerticals.Enabled = false;
            this.textBoxNumVerticals.Location = new System.Drawing.Point(231, 299);
            this.textBoxNumVerticals.Name = "textBoxNumVerticals";
            this.textBoxNumVerticals.Size = new System.Drawing.Size(54, 20);
            this.textBoxNumVerticals.TabIndex = 25;
            this.textBoxNumVerticals.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioButtonBuy);
            this.groupBox1.Controls.Add(this.radioButtonSell);
            this.groupBox1.Controls.Add(this.radioButtonRoll);
            this.groupBox1.Controls.Add(this.radioButtonFirstTrade);
            this.groupBox1.Location = new System.Drawing.Point(473, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 124);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trade Type";
            // 
            // radioButtonFirstTrade
            // 
            this.radioButtonFirstTrade.AutoSize = true;
            this.radioButtonFirstTrade.Location = new System.Drawing.Point(31, 20);
            this.radioButtonFirstTrade.Name = "radioButtonFirstTrade";
            this.radioButtonFirstTrade.Size = new System.Drawing.Size(115, 17);
            this.radioButtonFirstTrade.TabIndex = 0;
            this.radioButtonFirstTrade.TabStop = true;
            this.radioButtonFirstTrade.Text = "First trade of month";
            this.radioButtonFirstTrade.UseVisualStyleBackColor = true;
            this.radioButtonFirstTrade.CheckedChanged += new System.EventHandler(this.radioButtonTradeType_CheckedChanged);
            // 
            // radioButtonRoll
            // 
            this.radioButtonRoll.AutoSize = true;
            this.radioButtonRoll.Location = new System.Drawing.Point(31, 44);
            this.radioButtonRoll.Name = "radioButtonRoll";
            this.radioButtonRoll.Size = new System.Drawing.Size(77, 17);
            this.radioButtonRoll.TabIndex = 1;
            this.radioButtonRoll.TabStop = true;
            this.radioButtonRoll.Text = "Weekly roll";
            this.radioButtonRoll.UseVisualStyleBackColor = true;
            this.radioButtonRoll.CheckedChanged += new System.EventHandler(this.radioButtonTradeType_CheckedChanged);
            // 
            // radioButtonSell
            // 
            this.radioButtonSell.AutoSize = true;
            this.radioButtonSell.Location = new System.Drawing.Point(31, 68);
            this.radioButtonSell.Name = "radioButtonSell";
            this.radioButtonSell.Size = new System.Drawing.Size(75, 17);
            this.radioButtonSell.TabIndex = 2;
            this.radioButtonSell.TabStop = true;
            this.radioButtonSell.Text = "Sell Deltas";
            this.radioButtonSell.UseVisualStyleBackColor = true;
            this.radioButtonSell.CheckedChanged += new System.EventHandler(this.radioButtonTradeType_CheckedChanged);
            // 
            // radioButtonBuy
            // 
            this.radioButtonBuy.AutoSize = true;
            this.radioButtonBuy.Location = new System.Drawing.Point(31, 92);
            this.radioButtonBuy.Name = "radioButtonBuy";
            this.radioButtonBuy.Size = new System.Drawing.Size(76, 17);
            this.radioButtonBuy.TabIndex = 3;
            this.radioButtonBuy.TabStop = true;
            this.radioButtonBuy.Text = "Buy Deltas";
            this.radioButtonBuy.UseVisualStyleBackColor = true;
            this.radioButtonBuy.CheckedChanged += new System.EventHandler(this.radioButtonTradeType_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxCall5Delta);
            this.groupBox2.Controls.Add(this.textBoxCall4Delta);
            this.groupBox2.Controls.Add(this.textBoxCall3Delta);
            this.groupBox2.Controls.Add(this.textBoxCall2Delta);
            this.groupBox2.Controls.Add(this.textBoxCall1Delta);
            this.groupBox2.Controls.Add(this.textBoxCall5Position);
            this.groupBox2.Controls.Add(this.textBoxCall4Position);
            this.groupBox2.Controls.Add(this.textBoxCall3Position);
            this.groupBox2.Controls.Add(this.textBoxCall2Position);
            this.groupBox2.Controls.Add(this.textBoxCall1Position);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.labelCall5Trade);
            this.groupBox2.Controls.Add(this.labelCall4Trade);
            this.groupBox2.Controls.Add(this.labelCall3Trade);
            this.groupBox2.Controls.Add(this.labelCall2Trade);
            this.groupBox2.Controls.Add(this.labelCall1Trade);
            this.groupBox2.Controls.Add(this.labelCall5);
            this.groupBox2.Controls.Add(this.labelCall4);
            this.groupBox2.Controls.Add(this.labelCall3);
            this.groupBox2.Controls.Add(this.labelCall2);
            this.groupBox2.Controls.Add(this.labelCall1);
            this.groupBox2.Location = new System.Drawing.Point(30, 152);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(418, 141);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current Call Positions";
            // 
            // labelCall1
            // 
            this.labelCall1.AutoSize = true;
            this.labelCall1.Location = new System.Drawing.Point(22, 29);
            this.labelCall1.Name = "labelCall1";
            this.labelCall1.Size = new System.Drawing.Size(0, 13);
            this.labelCall1.TabIndex = 0;
            // 
            // labelCall2
            // 
            this.labelCall2.AutoSize = true;
            this.labelCall2.Location = new System.Drawing.Point(22, 50);
            this.labelCall2.Name = "labelCall2";
            this.labelCall2.Size = new System.Drawing.Size(0, 13);
            this.labelCall2.TabIndex = 2;
            // 
            // labelCall3
            // 
            this.labelCall3.AutoSize = true;
            this.labelCall3.Location = new System.Drawing.Point(22, 71);
            this.labelCall3.Name = "labelCall3";
            this.labelCall3.Size = new System.Drawing.Size(0, 13);
            this.labelCall3.TabIndex = 3;
            // 
            // labelCall4
            // 
            this.labelCall4.AutoSize = true;
            this.labelCall4.Location = new System.Drawing.Point(22, 92);
            this.labelCall4.Name = "labelCall4";
            this.labelCall4.Size = new System.Drawing.Size(0, 13);
            this.labelCall4.TabIndex = 4;
            // 
            // labelCall5
            // 
            this.labelCall5.AutoSize = true;
            this.labelCall5.Location = new System.Drawing.Point(22, 113);
            this.labelCall5.Name = "labelCall5";
            this.labelCall5.Size = new System.Drawing.Size(0, 13);
            this.labelCall5.TabIndex = 5;
            // 
            // labelCall5Trade
            // 
            this.labelCall5Trade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCall5Trade.AutoSize = true;
            this.labelCall5Trade.Location = new System.Drawing.Point(345, 113);
            this.labelCall5Trade.Name = "labelCall5Trade";
            this.labelCall5Trade.Size = new System.Drawing.Size(0, 13);
            this.labelCall5Trade.TabIndex = 15;
            // 
            // labelCall4Trade
            // 
            this.labelCall4Trade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCall4Trade.AutoSize = true;
            this.labelCall4Trade.Location = new System.Drawing.Point(345, 92);
            this.labelCall4Trade.Name = "labelCall4Trade";
            this.labelCall4Trade.Size = new System.Drawing.Size(0, 13);
            this.labelCall4Trade.TabIndex = 14;
            // 
            // labelCall3Trade
            // 
            this.labelCall3Trade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCall3Trade.AutoSize = true;
            this.labelCall3Trade.Location = new System.Drawing.Point(345, 71);
            this.labelCall3Trade.Name = "labelCall3Trade";
            this.labelCall3Trade.Size = new System.Drawing.Size(0, 13);
            this.labelCall3Trade.TabIndex = 13;
            // 
            // labelCall2Trade
            // 
            this.labelCall2Trade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCall2Trade.AutoSize = true;
            this.labelCall2Trade.Location = new System.Drawing.Point(345, 50);
            this.labelCall2Trade.Name = "labelCall2Trade";
            this.labelCall2Trade.Size = new System.Drawing.Size(0, 13);
            this.labelCall2Trade.TabIndex = 12;
            // 
            // labelCall1Trade
            // 
            this.labelCall1Trade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCall1Trade.AutoSize = true;
            this.labelCall1Trade.Location = new System.Drawing.Point(345, 29);
            this.labelCall1Trade.Name = "labelCall1Trade";
            this.labelCall1Trade.Size = new System.Drawing.Size(0, 13);
            this.labelCall1Trade.TabIndex = 11;
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(335, 10);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(51, 13);
            this.label27.TabIndex = 16;
            this.label27.Text = "To Trade";
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(190, 10);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(81, 13);
            this.label28.TabIndex = 17;
            this.label28.Text = "Current Position";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(106, 302);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(97, 13);
            this.label29.TabIndex = 28;
            this.label29.Text = "Current Put Spread";
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(279, 10);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(32, 13);
            this.label30.TabIndex = 18;
            this.label30.Text = "Delta";
            // 
            // textBoxCall1Position
            // 
            this.textBoxCall1Position.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall1Position.Enabled = false;
            this.textBoxCall1Position.Location = new System.Drawing.Point(201, 26);
            this.textBoxCall1Position.Name = "textBoxCall1Position";
            this.textBoxCall1Position.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall1Position.TabIndex = 29;
            this.textBoxCall1Position.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCall2Position
            // 
            this.textBoxCall2Position.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall2Position.Enabled = false;
            this.textBoxCall2Position.Location = new System.Drawing.Point(201, 47);
            this.textBoxCall2Position.Name = "textBoxCall2Position";
            this.textBoxCall2Position.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall2Position.TabIndex = 30;
            this.textBoxCall2Position.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCall3Position
            // 
            this.textBoxCall3Position.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall3Position.Enabled = false;
            this.textBoxCall3Position.Location = new System.Drawing.Point(201, 68);
            this.textBoxCall3Position.Name = "textBoxCall3Position";
            this.textBoxCall3Position.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall3Position.TabIndex = 31;
            this.textBoxCall3Position.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCall4Position
            // 
            this.textBoxCall4Position.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall4Position.Enabled = false;
            this.textBoxCall4Position.Location = new System.Drawing.Point(201, 89);
            this.textBoxCall4Position.Name = "textBoxCall4Position";
            this.textBoxCall4Position.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall4Position.TabIndex = 32;
            this.textBoxCall4Position.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCall5Position
            // 
            this.textBoxCall5Position.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall5Position.Enabled = false;
            this.textBoxCall5Position.Location = new System.Drawing.Point(201, 110);
            this.textBoxCall5Position.Name = "textBoxCall5Position";
            this.textBoxCall5Position.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall5Position.TabIndex = 33;
            this.textBoxCall5Position.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCall5Delta
            // 
            this.textBoxCall5Delta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall5Delta.Enabled = false;
            this.textBoxCall5Delta.Location = new System.Drawing.Point(270, 110);
            this.textBoxCall5Delta.Name = "textBoxCall5Delta";
            this.textBoxCall5Delta.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall5Delta.TabIndex = 38;
            this.textBoxCall5Delta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCall4Delta
            // 
            this.textBoxCall4Delta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall4Delta.Enabled = false;
            this.textBoxCall4Delta.Location = new System.Drawing.Point(270, 89);
            this.textBoxCall4Delta.Name = "textBoxCall4Delta";
            this.textBoxCall4Delta.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall4Delta.TabIndex = 37;
            this.textBoxCall4Delta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCall3Delta
            // 
            this.textBoxCall3Delta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall3Delta.Enabled = false;
            this.textBoxCall3Delta.Location = new System.Drawing.Point(270, 68);
            this.textBoxCall3Delta.Name = "textBoxCall3Delta";
            this.textBoxCall3Delta.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall3Delta.TabIndex = 36;
            this.textBoxCall3Delta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCall2Delta
            // 
            this.textBoxCall2Delta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall2Delta.Enabled = false;
            this.textBoxCall2Delta.Location = new System.Drawing.Point(270, 47);
            this.textBoxCall2Delta.Name = "textBoxCall2Delta";
            this.textBoxCall2Delta.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall2Delta.TabIndex = 35;
            this.textBoxCall2Delta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCall1Delta
            // 
            this.textBoxCall1Delta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCall1Delta.Enabled = false;
            this.textBoxCall1Delta.Location = new System.Drawing.Point(270, 26);
            this.textBoxCall1Delta.Name = "textBoxCall1Delta";
            this.textBoxCall1Delta.Size = new System.Drawing.Size(54, 20);
            this.textBoxCall1Delta.TabIndex = 34;
            this.textBoxCall1Delta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.textBoxATMCallDelta);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.labelOTMStrike);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.textBoxATMPutDelta);
            this.groupBox3.Controls.Add(this.textBoxOTMPutDelta);
            this.groupBox3.Controls.Add(this.labelCallsToSell);
            this.groupBox3.Controls.Add(this.labelPutsToBuy);
            this.groupBox3.Controls.Add(this.labelPutsToSell);
            this.groupBox3.Location = new System.Drawing.Point(30, 338);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(427, 137);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "New Positions";
            // 
            // textBoxVerticalDelta
            // 
            this.textBoxVerticalDelta.Enabled = false;
            this.textBoxVerticalDelta.Location = new System.Drawing.Point(300, 299);
            this.textBoxVerticalDelta.Name = "textBoxVerticalDelta";
            this.textBoxVerticalDelta.Size = new System.Drawing.Size(54, 20);
            this.textBoxVerticalDelta.TabIndex = 39;
            this.textBoxVerticalDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxDeltasToTrade
            // 
            this.textBoxDeltasToTrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDeltasToTrade.Enabled = false;
            this.textBoxDeltasToTrade.Location = new System.Drawing.Point(544, 156);
            this.textBoxDeltasToTrade.Name = "textBoxDeltasToTrade";
            this.textBoxDeltasToTrade.Size = new System.Drawing.Size(81, 20);
            this.textBoxDeltasToTrade.TabIndex = 40;
            this.textBoxDeltasToTrade.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(462, 159);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 13);
            this.label12.TabIndex = 41;
            this.label12.Text = "Deltas to trade";
            // 
            // GPARCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 518);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxDeltasToTrade);
            this.Controls.Add(this.textBoxVerticalDelta);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxNumVerticals);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.textBoxTargetDelta);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxMultiplier);
            this.Controls.Add(this.textBoxIndexPrice);
            this.Controls.Add(this.textBoxEquity);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "GPARCalculator";
            this.Text = "GPAR Calculator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.TextBox textBoxEquity;
        private System.Windows.Forms.TextBox textBoxATMPutDelta;
        private System.Windows.Forms.TextBox textBoxOTMPutDelta;
        private System.Windows.Forms.TextBox textBoxATMCallDelta;
        private System.Windows.Forms.TextBox textBoxIndexPrice;
        private System.Windows.Forms.TextBox textBoxMultiplier;
        private System.Windows.Forms.Label labelCallsToSell;
        private System.Windows.Forms.Label labelPutsToSell;
        private System.Windows.Forms.Label labelPutsToBuy;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxTargetDelta;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelOTMStrike;
        private System.Windows.Forms.TextBox textBoxNumVerticals;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonBuy;
        private System.Windows.Forms.RadioButton radioButtonSell;
        private System.Windows.Forms.RadioButton radioButtonRoll;
        private System.Windows.Forms.RadioButton radioButtonFirstTrade;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxCall5Delta;
        private System.Windows.Forms.TextBox textBoxCall4Delta;
        private System.Windows.Forms.TextBox textBoxCall3Delta;
        private System.Windows.Forms.TextBox textBoxCall2Delta;
        private System.Windows.Forms.TextBox textBoxCall1Delta;
        private System.Windows.Forms.TextBox textBoxCall5Position;
        private System.Windows.Forms.TextBox textBoxCall4Position;
        private System.Windows.Forms.TextBox textBoxCall3Position;
        private System.Windows.Forms.TextBox textBoxCall2Position;
        private System.Windows.Forms.TextBox textBoxCall1Position;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label labelCall5Trade;
        private System.Windows.Forms.Label labelCall4Trade;
        private System.Windows.Forms.Label labelCall3Trade;
        private System.Windows.Forms.Label labelCall2Trade;
        private System.Windows.Forms.Label labelCall1Trade;
        private System.Windows.Forms.Label labelCall5;
        private System.Windows.Forms.Label labelCall4;
        private System.Windows.Forms.Label labelCall3;
        private System.Windows.Forms.Label labelCall2;
        private System.Windows.Forms.Label labelCall1;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxVerticalDelta;
        private System.Windows.Forms.TextBox textBoxDeltasToTrade;
        private System.Windows.Forms.Label label12;
    }
}