namespace SentoniClient
{
    partial class FormPutCalculator
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.underlyingSymbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expirationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.strikePriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deltaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumberToSell = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.multiplierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NettingAdjustment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._100DeltaUSD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accountDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.accountDataSet = new SentoniClient.AccountDataSet();
            this.label6 = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.textBoxCurrentDelta = new System.Windows.Forms.TextBox();
            this.textBoxDesiredDelta = new System.Windows.Forms.TextBox();
            this.textBoxDeltaOfPutToBuy = new System.Windows.Forms.TextBox();
            this.textBoxCurrentNotionalValue = new System.Windows.Forms.TextBox();
            this.textBoxMaxNotionalValue = new System.Windows.Forms.TextBox();
            this.labelTradeVolume = new System.Windows.Forms.Label();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.UnderlyingSymbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpirationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StrikePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxStrikePrice = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxNewDelta = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxNewNotionalValue = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxMultiplier = new System.Windows.Forms.TextBox();
            this.textBoxNumberToBuy = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonVerify = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountDataSet)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Max";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Current";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Goal";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(73, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Delta";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.underlyingSymbolDataGridViewTextBoxColumn,
            this.expirationDateDataGridViewTextBoxColumn,
            this.strikePriceDataGridViewTextBoxColumn,
            this.deltaDataGridViewTextBoxColumn,
            this.CurrentPosition,
            this.NumberToSell,
            this.multiplierDataGridViewTextBoxColumn,
            this.NettingAdjustment,
            this._100DeltaUSD});
            this.dataGridView1.DataMember = "Portfolio";
            this.dataGridView1.DataSource = this.accountDataSetBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(68, 156);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(462, 285);
            this.dataGridView1.TabIndex = 5;
            // 
            // underlyingSymbolDataGridViewTextBoxColumn
            // 
            this.underlyingSymbolDataGridViewTextBoxColumn.DataPropertyName = "UnderlyingSymbol";
            this.underlyingSymbolDataGridViewTextBoxColumn.HeaderText = "Underlying Symbol";
            this.underlyingSymbolDataGridViewTextBoxColumn.Name = "underlyingSymbolDataGridViewTextBoxColumn";
            this.underlyingSymbolDataGridViewTextBoxColumn.Width = 65;
            // 
            // expirationDateDataGridViewTextBoxColumn
            // 
            this.expirationDateDataGridViewTextBoxColumn.DataPropertyName = "ExpirationDate";
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.expirationDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle31;
            this.expirationDateDataGridViewTextBoxColumn.HeaderText = "Expiration Date";
            this.expirationDateDataGridViewTextBoxColumn.Name = "expirationDateDataGridViewTextBoxColumn";
            this.expirationDateDataGridViewTextBoxColumn.Width = 75;
            // 
            // strikePriceDataGridViewTextBoxColumn
            // 
            this.strikePriceDataGridViewTextBoxColumn.DataPropertyName = "StrikePrice";
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle32.Format = "f0";
            this.strikePriceDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle32;
            this.strikePriceDataGridViewTextBoxColumn.HeaderText = "Strike Price";
            this.strikePriceDataGridViewTextBoxColumn.Name = "strikePriceDataGridViewTextBoxColumn";
            this.strikePriceDataGridViewTextBoxColumn.Width = 60;
            // 
            // deltaDataGridViewTextBoxColumn
            // 
            this.deltaDataGridViewTextBoxColumn.DataPropertyName = "Delta";
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle33.Format = "f2";
            this.deltaDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle33;
            this.deltaDataGridViewTextBoxColumn.HeaderText = "Delta";
            this.deltaDataGridViewTextBoxColumn.Name = "deltaDataGridViewTextBoxColumn";
            this.deltaDataGridViewTextBoxColumn.Width = 60;
            // 
            // CurrentPosition
            // 
            this.CurrentPosition.DataPropertyName = "Current_Position";
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.CurrentPosition.DefaultCellStyle = dataGridViewCellStyle34;
            this.CurrentPosition.HeaderText = "Current Position";
            this.CurrentPosition.Name = "CurrentPosition";
            this.CurrentPosition.Width = 65;
            // 
            // NumberToSell
            // 
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.NumberToSell.DefaultCellStyle = dataGridViewCellStyle35;
            this.NumberToSell.HeaderText = "Number To Sell";
            this.NumberToSell.Name = "NumberToSell";
            this.NumberToSell.Width = 65;
            // 
            // multiplierDataGridViewTextBoxColumn
            // 
            this.multiplierDataGridViewTextBoxColumn.DataPropertyName = "Multiplier";
            this.multiplierDataGridViewTextBoxColumn.HeaderText = "Multiplier";
            this.multiplierDataGridViewTextBoxColumn.Name = "multiplierDataGridViewTextBoxColumn";
            this.multiplierDataGridViewTextBoxColumn.Visible = false;
            // 
            // NettingAdjustment
            // 
            this.NettingAdjustment.DataPropertyName = "NettingAdjustment";
            this.NettingAdjustment.HeaderText = "NettingAdjustment";
            this.NettingAdjustment.Name = "NettingAdjustment";
            this.NettingAdjustment.Visible = false;
            // 
            // _100DeltaUSD
            // 
            this._100DeltaUSD.DataPropertyName = "_100DeltaUSD";
            this._100DeltaUSD.HeaderText = "100 Delta USD";
            this._100DeltaUSD.Name = "_100DeltaUSD";
            this._100DeltaUSD.Visible = false;
            // 
            // accountDataSetBindingSource
            // 
            this.accountDataSetBindingSource.DataSource = this.accountDataSet;
            this.accountDataSetBindingSource.Position = 0;
            // 
            // accountDataSet
            // 
            this.accountDataSet.DataSetName = "AccountDataSet";
            this.accountDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(65, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Current Positions";
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(12, 448);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 13);
            this.labelStatus.TabIndex = 7;
            // 
            // textBoxCurrentDelta
            // 
            this.textBoxCurrentDelta.Location = new System.Drawing.Point(66, 43);
            this.textBoxCurrentDelta.Name = "textBoxCurrentDelta";
            this.textBoxCurrentDelta.Size = new System.Drawing.Size(68, 20);
            this.textBoxCurrentDelta.TabIndex = 8;
            this.textBoxCurrentDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxDesiredDelta
            // 
            this.textBoxDesiredDelta.Location = new System.Drawing.Point(66, 17);
            this.textBoxDesiredDelta.Name = "textBoxDesiredDelta";
            this.textBoxDesiredDelta.Size = new System.Drawing.Size(68, 20);
            this.textBoxDesiredDelta.TabIndex = 0;
            this.textBoxDesiredDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxDeltaOfPutToBuy
            // 
            this.textBoxDeltaOfPutToBuy.Location = new System.Drawing.Point(119, 17);
            this.textBoxDeltaOfPutToBuy.Name = "textBoxDeltaOfPutToBuy";
            this.textBoxDeltaOfPutToBuy.Size = new System.Drawing.Size(68, 20);
            this.textBoxDeltaOfPutToBuy.TabIndex = 0;
            this.textBoxDeltaOfPutToBuy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxCurrentNotionalValue
            // 
            this.textBoxCurrentNotionalValue.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxCurrentNotionalValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCurrentNotionalValue.Location = new System.Drawing.Point(71, 47);
            this.textBoxCurrentNotionalValue.Name = "textBoxCurrentNotionalValue";
            this.textBoxCurrentNotionalValue.ReadOnly = true;
            this.textBoxCurrentNotionalValue.Size = new System.Drawing.Size(130, 13);
            this.textBoxCurrentNotionalValue.TabIndex = 11;
            this.textBoxCurrentNotionalValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxMaxNotionalValue
            // 
            this.textBoxMaxNotionalValue.Location = new System.Drawing.Point(71, 17);
            this.textBoxMaxNotionalValue.Name = "textBoxMaxNotionalValue";
            this.textBoxMaxNotionalValue.Size = new System.Drawing.Size(134, 20);
            this.textBoxMaxNotionalValue.TabIndex = 12;
            this.textBoxMaxNotionalValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelTradeVolume
            // 
            this.labelTradeVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTradeVolume.AutoSize = true;
            this.labelTradeVolume.Location = new System.Drawing.Point(30, 99);
            this.labelTradeVolume.Name = "labelTradeVolume";
            this.labelTradeVolume.Size = new System.Drawing.Size(77, 13);
            this.labelTradeVolume.TabIndex = 13;
            this.labelTradeVolume.Text = "Number to Buy";
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCalculate.Enabled = false;
            this.buttonCalculate.Location = new System.Drawing.Point(552, 188);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(112, 23);
            this.buttonCalculate.TabIndex = 2;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // UnderlyingSymbol
            // 
            this.UnderlyingSymbol.DataPropertyName = "UnderlyingSymbol";
            this.UnderlyingSymbol.HeaderText = "Underlying Symbol";
            this.UnderlyingSymbol.Name = "UnderlyingSymbol";
            // 
            // ExpirationDate
            // 
            this.ExpirationDate.DataPropertyName = "ExpirationDate";
            this.ExpirationDate.HeaderText = "Expiration Date";
            this.ExpirationDate.Name = "ExpirationDate";
            // 
            // StrikePrice
            // 
            this.StrikePrice.DataPropertyName = "StrikePrice";
            this.StrikePrice.HeaderText = "Strike Price";
            this.StrikePrice.Name = "StrikePrice";
            // 
            // Delta
            // 
            this.Delta.DataPropertyName = "Delta";
            this.Delta.HeaderText = "Delta";
            this.Delta.Name = "Delta";
            // 
            // textBoxStrikePrice
            // 
            this.textBoxStrikePrice.Location = new System.Drawing.Point(119, 43);
            this.textBoxStrikePrice.Name = "textBoxStrikePrice";
            this.textBoxStrikePrice.Size = new System.Drawing.Size(68, 20);
            this.textBoxStrikePrice.TabIndex = 1;
            this.textBoxStrikePrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "New";
            // 
            // textBoxNewDelta
            // 
            this.textBoxNewDelta.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxNewDelta.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxNewDelta.Location = new System.Drawing.Point(66, 73);
            this.textBoxNewDelta.Name = "textBoxNewDelta";
            this.textBoxNewDelta.ReadOnly = true;
            this.textBoxNewDelta.Size = new System.Drawing.Size(64, 13);
            this.textBoxNewDelta.TabIndex = 18;
            this.textBoxNewDelta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxNewDelta);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxCurrentDelta);
            this.groupBox1.Controls.Add(this.textBoxDesiredDelta);
            this.groupBox1.Location = new System.Drawing.Point(281, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(162, 100);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Delta %";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxNewNotionalValue);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBoxCurrentNotionalValue);
            this.groupBox2.Controls.Add(this.textBoxMaxNotionalValue);
            this.groupBox2.Location = new System.Drawing.Point(15, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(238, 100);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notional Value";
            // 
            // textBoxNewNotionalValue
            // 
            this.textBoxNewNotionalValue.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxNewNotionalValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxNewNotionalValue.Location = new System.Drawing.Point(71, 73);
            this.textBoxNewNotionalValue.Name = "textBoxNewNotionalValue";
            this.textBoxNewNotionalValue.ReadOnly = true;
            this.textBoxNewNotionalValue.Size = new System.Drawing.Size(130, 13);
            this.textBoxNewNotionalValue.TabIndex = 14;
            this.textBoxNewNotionalValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(36, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "New";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.textBoxMultiplier);
            this.groupBox3.Controls.Add(this.textBoxNumberToBuy);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.textBoxDeltaOfPutToBuy);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textBoxStrikePrice);
            this.groupBox3.Controls.Add(this.labelTradeVolume);
            this.groupBox3.Location = new System.Drawing.Point(464, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(202, 128);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Put to Trade";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 73);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Shares per Contract";
            // 
            // textBoxMultiplier
            // 
            this.textBoxMultiplier.Location = new System.Drawing.Point(119, 69);
            this.textBoxMultiplier.Name = "textBoxMultiplier";
            this.textBoxMultiplier.Size = new System.Drawing.Size(68, 20);
            this.textBoxMultiplier.TabIndex = 19;
            this.textBoxMultiplier.Text = "100";
            this.textBoxMultiplier.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxNumberToBuy
            // 
            this.textBoxNumberToBuy.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxNumberToBuy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxNumberToBuy.Location = new System.Drawing.Point(119, 99);
            this.textBoxNumberToBuy.Name = "textBoxNumberToBuy";
            this.textBoxNumberToBuy.ReadOnly = true;
            this.textBoxNumberToBuy.Size = new System.Drawing.Size(64, 13);
            this.textBoxNumberToBuy.TabIndex = 18;
            this.textBoxNumberToBuy.Text = "0";
            this.textBoxNumberToBuy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(44, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Strike Price";
            // 
            // buttonVerify
            // 
            this.buttonVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonVerify.Location = new System.Drawing.Point(552, 156);
            this.buttonVerify.Name = "buttonVerify";
            this.buttonVerify.Size = new System.Drawing.Size(112, 23);
            this.buttonVerify.TabIndex = 1;
            this.buttonVerify.Text = "Verify Input";
            this.buttonVerify.UseVisualStyleBackColor = true;
            this.buttonVerify.Click += new System.EventHandler(this.buttonVerify_Click);
            // 
            // FormPutCalculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 467);
            this.Controls.Add(this.buttonVerify);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormPutCalculator";
            this.Text = "Put Calculator";
            this.Load += new System.EventHandler(this.FormPutCalculator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountDataSet)).EndInit();
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
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TextBox textBoxCurrentDelta;
        private System.Windows.Forms.TextBox textBoxDesiredDelta;
        private System.Windows.Forms.TextBox textBoxDeltaOfPutToBuy;
        private System.Windows.Forms.TextBox textBoxCurrentNotionalValue;
        private System.Windows.Forms.TextBox textBoxMaxNotionalValue;
        private System.Windows.Forms.Label labelTradeVolume;
        private System.Windows.Forms.Button buttonCalculate;
         private System.Windows.Forms.DataGridViewTextBoxColumn UnderlyingSymbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExpirationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn StrikePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Delta;
        private System.Windows.Forms.BindingSource accountDataSetBindingSource;
        private AccountDataSet accountDataSet;
        private System.Windows.Forms.TextBox textBoxStrikePrice;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxNewDelta;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxNewNotionalValue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxNumberToBuy;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonVerify;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxMultiplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn underlyingSymbolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expirationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn strikePriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deltaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumberToSell;
        private System.Windows.Forms.DataGridViewTextBoxColumn multiplierDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NettingAdjustment;
        private System.Windows.Forms.DataGridViewTextBoxColumn _100DeltaUSD;
    }
}