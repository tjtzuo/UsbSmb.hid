namespace RegTool
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Checkbox_Keep_Scanning = new System.Windows.Forms.CheckBox();
            this.btn_Stop_Logging = new System.Windows.Forms.Button();
            this.btn_Start_log = new System.Windows.Forms.Button();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Addr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RegName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Access = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.hexBox1 = new Be.Windows.Forms.HexBox();
            this.btn_ReadAll = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label_connect = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 21);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(558, 655);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Checkbox_Keep_Scanning);
            this.tabPage1.Controls.Add(this.btn_Stop_Logging);
            this.tabPage1.Controls.Add(this.btn_Start_log);
            this.tabPage1.Controls.Add(this.btn_Refresh);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(550, 629);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Registers Table";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Checkbox_Keep_Scanning
            // 
            this.Checkbox_Keep_Scanning.AutoSize = true;
            this.Checkbox_Keep_Scanning.Location = new System.Drawing.Point(22, 25);
            this.Checkbox_Keep_Scanning.Name = "Checkbox_Keep_Scanning";
            this.Checkbox_Keep_Scanning.Size = new System.Drawing.Size(94, 16);
            this.Checkbox_Keep_Scanning.TabIndex = 21;
            this.Checkbox_Keep_Scanning.Text = "Keep Scanning";
            this.Checkbox_Keep_Scanning.UseVisualStyleBackColor = true;
            this.Checkbox_Keep_Scanning.CheckedChanged += new System.EventHandler(this.Checkbox_Keep_Scanning_CheckedChanged);
            // 
            // btn_Stop_Logging
            // 
            this.btn_Stop_Logging.Enabled = false;
            this.btn_Stop_Logging.Location = new System.Drawing.Point(230, 18);
            this.btn_Stop_Logging.Name = "btn_Stop_Logging";
            this.btn_Stop_Logging.Size = new System.Drawing.Size(70, 28);
            this.btn_Stop_Logging.TabIndex = 20;
            this.btn_Stop_Logging.Text = "Stop Log";
            this.btn_Stop_Logging.UseVisualStyleBackColor = true;
            this.btn_Stop_Logging.Click += new System.EventHandler(this.btn_Stop_Logging_Click);
            // 
            // btn_Start_log
            // 
            this.btn_Start_log.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Start_log.Location = new System.Drawing.Point(131, 18);
            this.btn_Start_log.Name = "btn_Start_log";
            this.btn_Start_log.Size = new System.Drawing.Size(82, 28);
            this.btn_Start_log.TabIndex = 19;
            this.btn_Start_log.Text = "Start Log";
            this.btn_Start_log.UseVisualStyleBackColor = true;
            this.btn_Start_log.Click += new System.EventHandler(this.btn_Start_log_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(422, 18);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(70, 28);
            this.btn_Refresh.TabIndex = 16;
            this.btn_Refresh.Text = "Refresh";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Addr,
            this.RegName,
            this.Access,
            this.Reset,
            this.Value});
            this.dataGridView1.Location = new System.Drawing.Point(3, 61);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(538, 552);
            this.dataGridView1.TabIndex = 0;
            // 
            // Addr
            // 
            this.Addr.HeaderText = "Address";
            this.Addr.Name = "Addr";
            this.Addr.ReadOnly = true;
            this.Addr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RegName
            // 
            this.RegName.HeaderText = "Register Name";
            this.RegName.Name = "RegName";
            this.RegName.ReadOnly = true;
            this.RegName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Access
            // 
            this.Access.HeaderText = "Access";
            this.Access.Name = "Access";
            this.Access.ReadOnly = true;
            this.Access.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Reset
            // 
            this.Reset.HeaderText = "Reset Value";
            this.Reset.Name = "Reset";
            this.Reset.ReadOnly = true;
            this.Reset.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Value
            // 
            this.Value.HeaderText = "Current Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.hexBox1);
            this.tabPage2.Controls.Add(this.btn_ReadAll);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(550, 629);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Data EEPROM";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // hexBox1
            // 
            this.hexBox1.ColumnInfoVisible = true;
            this.hexBox1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.hexBox1.GroupSeparatorVisible = true;
            this.hexBox1.LineInfoVisible = true;
            this.hexBox1.Location = new System.Drawing.Point(3, 60);
            this.hexBox1.Name = "hexBox1";
            this.hexBox1.ReadOnly = true;
            this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox1.Size = new System.Drawing.Size(541, 354);
            this.hexBox1.TabIndex = 18;
            this.hexBox1.UseFixedBytesPerLine = true;
            // 
            // btn_ReadAll
            // 
            this.btn_ReadAll.Location = new System.Drawing.Point(459, 15);
            this.btn_ReadAll.Name = "btn_ReadAll";
            this.btn_ReadAll.Size = new System.Drawing.Size(70, 28);
            this.btn_ReadAll.TabIndex = 17;
            this.btn_ReadAll.Text = "Read All";
            this.btn_ReadAll.UseVisualStyleBackColor = true;
            this.btn_ReadAll.Click += new System.EventHandler(this.btn_ReadAll_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(550, 629);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Read/Write Register";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label_connect
            // 
            this.label_connect.AutoSize = true;
            this.label_connect.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.label_connect.ForeColor = System.Drawing.Color.Red;
            this.label_connect.Location = new System.Drawing.Point(335, 9);
            this.label_connect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_connect.Name = "label_connect";
            this.label_connect.Size = new System.Drawing.Size(159, 19);
            this.label_connect.TabIndex = 13;
            this.label_connect.Text = "No Device connecting ";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 686);
            this.Controls.Add(this.label_connect);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Sensor Registers Access Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label_connect;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn Addr;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Access;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reset;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.Button btn_ReadAll;
        private System.Windows.Forms.TabPage tabPage3;
        private Be.Windows.Forms.HexBox hexBox1;
        private System.Windows.Forms.CheckBox Checkbox_Keep_Scanning;
        private System.Windows.Forms.Button btn_Stop_Logging;
        private System.Windows.Forms.Button btn_Start_log;
        private System.Windows.Forms.Timer timer1;
    }
}

