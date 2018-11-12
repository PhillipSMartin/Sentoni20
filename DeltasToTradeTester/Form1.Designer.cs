namespace DeltasToTradeTester
{
    partial class Form1
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
            this.labelCurrentDelta = new System.Windows.Forms.Label();
            this.textBoxCurrentEquity = new System.Windows.Forms.TextBox();
            this.textBoxDeltaGoal = new System.Windows.Forms.TextBox();
            this.textBoxNumIndices = new System.Windows.Forms.TextBox();
            this.textBoxIndex1Weight = new System.Windows.Forms.TextBox();
            this.textBoxIndex1Price = new System.Windows.Forms.TextBox();
            this.textBoxIndex1Delta = new System.Windows.Forms.TextBox();
            this.labelIndex1DeltasToTrade = new System.Windows.Forms.Label();
            this.labelIndex1ResultingDelta = new System.Windows.Forms.Label();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.labelIndex2ResultingDelta = new System.Windows.Forms.Label();
            this.labelIndex2DeltasToTrade = new System.Windows.Forms.Label();
            this.textBoxIndex2Delta = new System.Windows.Forms.TextBox();
            this.textBoxIndex2Price = new System.Windows.Forms.TextBox();
            this.textBoxIndex2Weight = new System.Windows.Forms.TextBox();
            this.labelIndex3ResultingDelta = new System.Windows.Forms.Label();
            this.labelIndex3DeltasToTrade = new System.Windows.Forms.Label();
            this.textBoxIndex3Delta = new System.Windows.Forms.TextBox();
            this.textBoxIndex3Price = new System.Windows.Forms.TextBox();
            this.textBoxIndex3Weight = new System.Windows.Forms.TextBox();
            this.labelErrorMsg = new System.Windows.Forms.Label();
            this.buttonVerify = new System.Windows.Forms.Button();
            this.textBoxIndex3NumPuts = new System.Windows.Forms.TextBox();
            this.textBoxIndex2NumPuts = new System.Windows.Forms.TextBox();
            this.textBoxIndex1NumPuts = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelIndex1PutsToRebalance = new System.Windows.Forms.Label();
            this.labelIndex2PutsToRebalance = new System.Windows.Forms.Label();
            this.labelIndex3PutsToRebalance = new System.Windows.Forms.Label();
            this.labelTotalPutsToTrade = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.labelIndex1FaceValuePutsPct = new System.Windows.Forms.Label();
            this.labelIndex2FaceValuePutsPct = new System.Windows.Forms.Label();
            this.labelIndex3FaceValuePutsPct = new System.Windows.Forms.Label();
            this.labelTotalFaceValuePutsPct = new System.Windows.Forms.Label();
            this.labelIndex3PutsToTrade = new System.Windows.Forms.Label();
            this.labelIndex2PutsToTrade = new System.Windows.Forms.Label();
            this.labelIndex1PutsToTrade = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Equity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Indices";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Weight";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 267);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Price";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 299);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Current Delta";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 400);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Deltas to trade";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(39, 431);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Resulting Delta";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Current Delta";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 97);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Delta Goal";
            // 
            // labelCurrentDelta
            // 
            this.labelCurrentDelta.AutoSize = true;
            this.labelCurrentDelta.Location = new System.Drawing.Point(170, 66);
            this.labelCurrentDelta.Name = "labelCurrentDelta";
            this.labelCurrentDelta.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentDelta.TabIndex = 9;
            // 
            // textBoxCurrentEquity
            // 
            this.textBoxCurrentEquity.Location = new System.Drawing.Point(170, 32);
            this.textBoxCurrentEquity.Name = "textBoxCurrentEquity";
            this.textBoxCurrentEquity.Size = new System.Drawing.Size(100, 20);
            this.textBoxCurrentEquity.TabIndex = 0;
            this.textBoxCurrentEquity.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxDeltaGoal
            // 
            this.textBoxDeltaGoal.Location = new System.Drawing.Point(170, 94);
            this.textBoxDeltaGoal.Name = "textBoxDeltaGoal";
            this.textBoxDeltaGoal.Size = new System.Drawing.Size(100, 20);
            this.textBoxDeltaGoal.TabIndex = 1;
            this.textBoxDeltaGoal.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxNumIndices
            // 
            this.textBoxNumIndices.Location = new System.Drawing.Point(170, 125);
            this.textBoxNumIndices.Name = "textBoxNumIndices";
            this.textBoxNumIndices.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumIndices.TabIndex = 2;
            this.textBoxNumIndices.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxIndex1Weight
            // 
            this.textBoxIndex1Weight.Location = new System.Drawing.Point(167, 232);
            this.textBoxIndex1Weight.Name = "textBoxIndex1Weight";
            this.textBoxIndex1Weight.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex1Weight.TabIndex = 3;
            this.textBoxIndex1Weight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxIndex1Price
            // 
            this.textBoxIndex1Price.Location = new System.Drawing.Point(167, 264);
            this.textBoxIndex1Price.Name = "textBoxIndex1Price";
            this.textBoxIndex1Price.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex1Price.TabIndex = 4;
            this.textBoxIndex1Price.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxIndex1Delta
            // 
            this.textBoxIndex1Delta.Location = new System.Drawing.Point(167, 296);
            this.textBoxIndex1Delta.Name = "textBoxIndex1Delta";
            this.textBoxIndex1Delta.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex1Delta.TabIndex = 5;
            this.textBoxIndex1Delta.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelIndex1DeltasToTrade
            // 
            this.labelIndex1DeltasToTrade.AutoSize = true;
            this.labelIndex1DeltasToTrade.Location = new System.Drawing.Point(167, 400);
            this.labelIndex1DeltasToTrade.Name = "labelIndex1DeltasToTrade";
            this.labelIndex1DeltasToTrade.Size = new System.Drawing.Size(0, 13);
            this.labelIndex1DeltasToTrade.TabIndex = 16;
            // 
            // labelIndex1ResultingDelta
            // 
            this.labelIndex1ResultingDelta.AutoSize = true;
            this.labelIndex1ResultingDelta.Location = new System.Drawing.Point(167, 431);
            this.labelIndex1ResultingDelta.Name = "labelIndex1ResultingDelta";
            this.labelIndex1ResultingDelta.Size = new System.Drawing.Size(0, 13);
            this.labelIndex1ResultingDelta.TabIndex = 17;
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Enabled = false;
            this.buttonCalculate.Location = new System.Drawing.Point(590, 53);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(142, 23);
            this.buttonCalculate.TabIndex = 16;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(167, 198);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Index 1";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(324, 198);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(42, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "Index 2";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(484, 198);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(42, 13);
            this.label14.TabIndex = 21;
            this.label14.Text = "Index 3";
            // 
            // labelIndex2ResultingDelta
            // 
            this.labelIndex2ResultingDelta.AutoSize = true;
            this.labelIndex2ResultingDelta.Location = new System.Drawing.Point(324, 431);
            this.labelIndex2ResultingDelta.Name = "labelIndex2ResultingDelta";
            this.labelIndex2ResultingDelta.Size = new System.Drawing.Size(0, 13);
            this.labelIndex2ResultingDelta.TabIndex = 26;
            // 
            // labelIndex2DeltasToTrade
            // 
            this.labelIndex2DeltasToTrade.AutoSize = true;
            this.labelIndex2DeltasToTrade.Location = new System.Drawing.Point(324, 400);
            this.labelIndex2DeltasToTrade.Name = "labelIndex2DeltasToTrade";
            this.labelIndex2DeltasToTrade.Size = new System.Drawing.Size(0, 13);
            this.labelIndex2DeltasToTrade.TabIndex = 25;
            // 
            // textBoxIndex2Delta
            // 
            this.textBoxIndex2Delta.Location = new System.Drawing.Point(324, 296);
            this.textBoxIndex2Delta.Name = "textBoxIndex2Delta";
            this.textBoxIndex2Delta.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex2Delta.TabIndex = 9;
            this.textBoxIndex2Delta.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxIndex2Price
            // 
            this.textBoxIndex2Price.Location = new System.Drawing.Point(324, 264);
            this.textBoxIndex2Price.Name = "textBoxIndex2Price";
            this.textBoxIndex2Price.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex2Price.TabIndex = 8;
            this.textBoxIndex2Price.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxIndex2Weight
            // 
            this.textBoxIndex2Weight.Location = new System.Drawing.Point(324, 232);
            this.textBoxIndex2Weight.Name = "textBoxIndex2Weight";
            this.textBoxIndex2Weight.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex2Weight.TabIndex = 7;
            this.textBoxIndex2Weight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelIndex3ResultingDelta
            // 
            this.labelIndex3ResultingDelta.AutoSize = true;
            this.labelIndex3ResultingDelta.Location = new System.Drawing.Point(484, 431);
            this.labelIndex3ResultingDelta.Name = "labelIndex3ResultingDelta";
            this.labelIndex3ResultingDelta.Size = new System.Drawing.Size(0, 13);
            this.labelIndex3ResultingDelta.TabIndex = 31;
            // 
            // labelIndex3DeltasToTrade
            // 
            this.labelIndex3DeltasToTrade.AutoSize = true;
            this.labelIndex3DeltasToTrade.Location = new System.Drawing.Point(484, 400);
            this.labelIndex3DeltasToTrade.Name = "labelIndex3DeltasToTrade";
            this.labelIndex3DeltasToTrade.Size = new System.Drawing.Size(0, 13);
            this.labelIndex3DeltasToTrade.TabIndex = 30;
            // 
            // textBoxIndex3Delta
            // 
            this.textBoxIndex3Delta.Location = new System.Drawing.Point(484, 296);
            this.textBoxIndex3Delta.Name = "textBoxIndex3Delta";
            this.textBoxIndex3Delta.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex3Delta.TabIndex = 13;
            this.textBoxIndex3Delta.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxIndex3Price
            // 
            this.textBoxIndex3Price.Location = new System.Drawing.Point(484, 264);
            this.textBoxIndex3Price.Name = "textBoxIndex3Price";
            this.textBoxIndex3Price.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex3Price.TabIndex = 12;
            this.textBoxIndex3Price.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxIndex3Weight
            // 
            this.textBoxIndex3Weight.Location = new System.Drawing.Point(484, 232);
            this.textBoxIndex3Weight.Name = "textBoxIndex3Weight";
            this.textBoxIndex3Weight.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex3Weight.TabIndex = 11;
            this.textBoxIndex3Weight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelErrorMsg
            // 
            this.labelErrorMsg.AutoSize = true;
            this.labelErrorMsg.BackColor = System.Drawing.SystemColors.Control;
            this.labelErrorMsg.ForeColor = System.Drawing.Color.Red;
            this.labelErrorMsg.Location = new System.Drawing.Point(489, 100);
            this.labelErrorMsg.Name = "labelErrorMsg";
            this.labelErrorMsg.Size = new System.Drawing.Size(0, 13);
            this.labelErrorMsg.TabIndex = 32;
            // 
            // buttonVerify
            // 
            this.buttonVerify.Location = new System.Drawing.Point(588, 25);
            this.buttonVerify.Name = "buttonVerify";
            this.buttonVerify.Size = new System.Drawing.Size(144, 23);
            this.buttonVerify.TabIndex = 15;
            this.buttonVerify.Text = "Verify";
            this.buttonVerify.UseVisualStyleBackColor = true;
            this.buttonVerify.Click += new System.EventHandler(this.buttonVerify_Click);
            // 
            // textBoxIndex3NumPuts
            // 
            this.textBoxIndex3NumPuts.Location = new System.Drawing.Point(484, 334);
            this.textBoxIndex3NumPuts.Name = "textBoxIndex3NumPuts";
            this.textBoxIndex3NumPuts.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex3NumPuts.TabIndex = 14;
            // 
            // textBoxIndex2NumPuts
            // 
            this.textBoxIndex2NumPuts.Location = new System.Drawing.Point(324, 334);
            this.textBoxIndex2NumPuts.Name = "textBoxIndex2NumPuts";
            this.textBoxIndex2NumPuts.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex2NumPuts.TabIndex = 10;
            // 
            // textBoxIndex1NumPuts
            // 
            this.textBoxIndex1NumPuts.Location = new System.Drawing.Point(167, 334);
            this.textBoxIndex1NumPuts.Name = "textBoxIndex1NumPuts";
            this.textBoxIndex1NumPuts.Size = new System.Drawing.Size(100, 20);
            this.textBoxIndex1NumPuts.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(37, 334);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 13);
            this.label11.TabIndex = 36;
            this.label11.Text = "Number of puts";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(39, 459);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 13);
            this.label12.TabIndex = 37;
            this.label12.Text = "Puts to Rebalance";
            // 
            // labelIndex1PutsToRebalance
            // 
            this.labelIndex1PutsToRebalance.AutoSize = true;
            this.labelIndex1PutsToRebalance.Location = new System.Drawing.Point(167, 459);
            this.labelIndex1PutsToRebalance.Name = "labelIndex1PutsToRebalance";
            this.labelIndex1PutsToRebalance.Size = new System.Drawing.Size(0, 13);
            this.labelIndex1PutsToRebalance.TabIndex = 38;
            // 
            // labelIndex2PutsToRebalance
            // 
            this.labelIndex2PutsToRebalance.AutoSize = true;
            this.labelIndex2PutsToRebalance.Location = new System.Drawing.Point(324, 458);
            this.labelIndex2PutsToRebalance.Name = "labelIndex2PutsToRebalance";
            this.labelIndex2PutsToRebalance.Size = new System.Drawing.Size(0, 13);
            this.labelIndex2PutsToRebalance.TabIndex = 39;
            // 
            // labelIndex3PutsToRebalance
            // 
            this.labelIndex3PutsToRebalance.AutoSize = true;
            this.labelIndex3PutsToRebalance.Location = new System.Drawing.Point(494, 458);
            this.labelIndex3PutsToRebalance.Name = "labelIndex3PutsToRebalance";
            this.labelIndex3PutsToRebalance.Size = new System.Drawing.Size(0, 13);
            this.labelIndex3PutsToRebalance.TabIndex = 40;
            // 
            // labelTotalPutsToTrade
            // 
            this.labelTotalPutsToTrade.AutoSize = true;
            this.labelTotalPutsToTrade.Location = new System.Drawing.Point(636, 486);
            this.labelTotalPutsToTrade.Name = "labelTotalPutsToTrade";
            this.labelTotalPutsToTrade.Size = new System.Drawing.Size(0, 13);
            this.labelTotalPutsToTrade.TabIndex = 41;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(39, 369);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(95, 13);
            this.label15.TabIndex = 42;
            this.label15.Text = "FaceValuePutsPct";
            // 
            // labelIndex1FaceValuePutsPct
            // 
            this.labelIndex1FaceValuePutsPct.AutoSize = true;
            this.labelIndex1FaceValuePutsPct.Location = new System.Drawing.Point(167, 369);
            this.labelIndex1FaceValuePutsPct.Name = "labelIndex1FaceValuePutsPct";
            this.labelIndex1FaceValuePutsPct.Size = new System.Drawing.Size(0, 13);
            this.labelIndex1FaceValuePutsPct.TabIndex = 43;
            // 
            // labelIndex2FaceValuePutsPct
            // 
            this.labelIndex2FaceValuePutsPct.AutoSize = true;
            this.labelIndex2FaceValuePutsPct.Location = new System.Drawing.Point(324, 369);
            this.labelIndex2FaceValuePutsPct.Name = "labelIndex2FaceValuePutsPct";
            this.labelIndex2FaceValuePutsPct.Size = new System.Drawing.Size(0, 13);
            this.labelIndex2FaceValuePutsPct.TabIndex = 44;
            // 
            // labelIndex3FaceValuePutsPct
            // 
            this.labelIndex3FaceValuePutsPct.AutoSize = true;
            this.labelIndex3FaceValuePutsPct.Location = new System.Drawing.Point(484, 369);
            this.labelIndex3FaceValuePutsPct.Name = "labelIndex3FaceValuePutsPct";
            this.labelIndex3FaceValuePutsPct.Size = new System.Drawing.Size(0, 13);
            this.labelIndex3FaceValuePutsPct.TabIndex = 45;
            // 
            // labelTotalFaceValuePutsPct
            // 
            this.labelTotalFaceValuePutsPct.AutoSize = true;
            this.labelTotalFaceValuePutsPct.Location = new System.Drawing.Point(636, 369);
            this.labelTotalFaceValuePutsPct.Name = "labelTotalFaceValuePutsPct";
            this.labelTotalFaceValuePutsPct.Size = new System.Drawing.Size(0, 13);
            this.labelTotalFaceValuePutsPct.TabIndex = 46;
            // 
            // labelIndex3PutsToTrade
            // 
            this.labelIndex3PutsToTrade.AutoSize = true;
            this.labelIndex3PutsToTrade.Location = new System.Drawing.Point(494, 486);
            this.labelIndex3PutsToTrade.Name = "labelIndex3PutsToTrade";
            this.labelIndex3PutsToTrade.Size = new System.Drawing.Size(0, 13);
            this.labelIndex3PutsToTrade.TabIndex = 50;
            // 
            // labelIndex2PutsToTrade
            // 
            this.labelIndex2PutsToTrade.AutoSize = true;
            this.labelIndex2PutsToTrade.Location = new System.Drawing.Point(324, 486);
            this.labelIndex2PutsToTrade.Name = "labelIndex2PutsToTrade";
            this.labelIndex2PutsToTrade.Size = new System.Drawing.Size(0, 13);
            this.labelIndex2PutsToTrade.TabIndex = 49;
            // 
            // labelIndex1PutsToTrade
            // 
            this.labelIndex1PutsToTrade.AutoSize = true;
            this.labelIndex1PutsToTrade.Location = new System.Drawing.Point(167, 487);
            this.labelIndex1PutsToTrade.Name = "labelIndex1PutsToTrade";
            this.labelIndex1PutsToTrade.Size = new System.Drawing.Size(0, 13);
            this.labelIndex1PutsToTrade.TabIndex = 48;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(39, 487);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(71, 13);
            this.label19.TabIndex = 47;
            this.label19.Text = "Puts to Trade";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 533);
            this.Controls.Add(this.labelIndex3PutsToTrade);
            this.Controls.Add(this.labelIndex2PutsToTrade);
            this.Controls.Add(this.labelIndex1PutsToTrade);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.labelTotalFaceValuePutsPct);
            this.Controls.Add(this.labelIndex3FaceValuePutsPct);
            this.Controls.Add(this.labelIndex2FaceValuePutsPct);
            this.Controls.Add(this.labelIndex1FaceValuePutsPct);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.labelTotalPutsToTrade);
            this.Controls.Add(this.labelIndex3PutsToRebalance);
            this.Controls.Add(this.labelIndex2PutsToRebalance);
            this.Controls.Add(this.labelIndex1PutsToRebalance);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxIndex3NumPuts);
            this.Controls.Add(this.textBoxIndex2NumPuts);
            this.Controls.Add(this.textBoxIndex1NumPuts);
            this.Controls.Add(this.buttonVerify);
            this.Controls.Add(this.labelErrorMsg);
            this.Controls.Add(this.labelIndex3ResultingDelta);
            this.Controls.Add(this.labelIndex3DeltasToTrade);
            this.Controls.Add(this.textBoxIndex3Delta);
            this.Controls.Add(this.textBoxIndex3Price);
            this.Controls.Add(this.textBoxIndex3Weight);
            this.Controls.Add(this.labelIndex2ResultingDelta);
            this.Controls.Add(this.labelIndex2DeltasToTrade);
            this.Controls.Add(this.textBoxIndex2Delta);
            this.Controls.Add(this.textBoxIndex2Price);
            this.Controls.Add(this.textBoxIndex2Weight);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.labelIndex1ResultingDelta);
            this.Controls.Add(this.labelIndex1DeltasToTrade);
            this.Controls.Add(this.textBoxIndex1Delta);
            this.Controls.Add(this.textBoxIndex1Price);
            this.Controls.Add(this.textBoxIndex1Weight);
            this.Controls.Add(this.textBoxNumIndices);
            this.Controls.Add(this.textBoxDeltaGoal);
            this.Controls.Add(this.textBoxCurrentEquity);
            this.Controls.Add(this.labelCurrentDelta);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.InfoText;
            this.Name = "Form1";
            this.Text = "Deltas to Trade Tester";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.Label labelCurrentDelta;
        private System.Windows.Forms.TextBox textBoxCurrentEquity;
        private System.Windows.Forms.TextBox textBoxDeltaGoal;
        private System.Windows.Forms.TextBox textBoxNumIndices;
        private System.Windows.Forms.TextBox textBoxIndex1Weight;
        private System.Windows.Forms.TextBox textBoxIndex1Price;
        private System.Windows.Forms.TextBox textBoxIndex1Delta;
        private System.Windows.Forms.Label labelIndex1DeltasToTrade;
        private System.Windows.Forms.Label labelIndex1ResultingDelta;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelIndex2ResultingDelta;
        private System.Windows.Forms.Label labelIndex2DeltasToTrade;
        private System.Windows.Forms.TextBox textBoxIndex2Delta;
        private System.Windows.Forms.TextBox textBoxIndex2Price;
        private System.Windows.Forms.TextBox textBoxIndex2Weight;
        private System.Windows.Forms.Label labelIndex3ResultingDelta;
        private System.Windows.Forms.Label labelIndex3DeltasToTrade;
        private System.Windows.Forms.TextBox textBoxIndex3Delta;
        private System.Windows.Forms.TextBox textBoxIndex3Price;
        private System.Windows.Forms.TextBox textBoxIndex3Weight;
        private System.Windows.Forms.Label labelErrorMsg;
        private System.Windows.Forms.Button buttonVerify;
        private System.Windows.Forms.TextBox textBoxIndex3NumPuts;
        private System.Windows.Forms.TextBox textBoxIndex2NumPuts;
        private System.Windows.Forms.TextBox textBoxIndex1NumPuts;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelIndex1PutsToRebalance;
        private System.Windows.Forms.Label labelIndex2PutsToRebalance;
        private System.Windows.Forms.Label labelIndex3PutsToRebalance;
        private System.Windows.Forms.Label labelTotalPutsToTrade;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelIndex1FaceValuePutsPct;
        private System.Windows.Forms.Label labelIndex2FaceValuePutsPct;
        private System.Windows.Forms.Label labelIndex3FaceValuePutsPct;
        private System.Windows.Forms.Label labelTotalFaceValuePutsPct;
        private System.Windows.Forms.Label labelIndex3PutsToTrade;
        private System.Windows.Forms.Label labelIndex2PutsToTrade;
        private System.Windows.Forms.Label labelIndex1PutsToTrade;
        private System.Windows.Forms.Label label19;
    }
}

