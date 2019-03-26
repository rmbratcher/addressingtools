namespace AddressingTools
{
    partial class AddressingWindow
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
            this.AddressingWindowTabControl = new System.Windows.Forms.TabControl();
            this.searchTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.high_range_tbx = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.low_range_tbx = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.calc_high_range_tbx = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.centerlineValuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.centerlinerangedataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.centerline_range_data = new AddressingTools.centerline_range_data();
            this.errorTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.errorCountLabel = new System.Windows.Forms.Label();
            this.CheckFeaturesProgressBar = new System.Windows.Forms.ProgressBar();
            this.label10 = new System.Windows.Forms.Label();
            this.check_errors_btn = new System.Windows.Forms.Button();
            this.error_treeview = new System.Windows.Forms.TreeView();
            this.settingsTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LayersgroupBox = new System.Windows.Forms.GroupBox();
            this.matchCenterlineFieldsButton = new System.Windows.Forms.Button();
            this.matchAddressPointFieldsButton = new System.Windows.Forms.Button();
            this.addressPointcomboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.centerlineLayerComboBox = new System.Windows.Forms.ComboBox();
            this.DisplaygroupBox = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.flashDelaycomboBox = new System.Windows.Forms.ComboBox();
            this.ChangeflashColorButton = new System.Windows.Forms.Button();
            this.flashColorSwatch = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.linefeedbackColorSwatch = new System.Windows.Forms.Panel();
            this.ChangeLineFeedBackColorButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.setAddressesPerUnit = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.zoomToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.AddressingWindowTabControl.SuspendLayout();
            this.searchTab.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerlineValuesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerlinerangedataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerline_range_data)).BeginInit();
            this.errorTab.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.settingsTab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.LayersgroupBox.SuspendLayout();
            this.DisplaygroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddressingWindowTabControl
            // 
            this.AddressingWindowTabControl.Controls.Add(this.searchTab);
            this.AddressingWindowTabControl.Controls.Add(this.errorTab);
            this.AddressingWindowTabControl.Controls.Add(this.settingsTab);
            this.AddressingWindowTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddressingWindowTabControl.Location = new System.Drawing.Point(0, 0);
            this.AddressingWindowTabControl.Name = "AddressingWindowTabControl";
            this.AddressingWindowTabControl.SelectedIndex = 0;
            this.AddressingWindowTabControl.Size = new System.Drawing.Size(325, 575);
            this.AddressingWindowTabControl.TabIndex = 0;
            // 
            // searchTab
            // 
            this.searchTab.Controls.Add(this.tableLayoutPanel2);
            this.searchTab.Location = new System.Drawing.Point(4, 22);
            this.searchTab.Name = "searchTab";
            this.searchTab.Padding = new System.Windows.Forms.Padding(3);
            this.searchTab.Size = new System.Drawing.Size(317, 549);
            this.searchTab.TabIndex = 0;
            this.searchTab.Text = "Range";
            this.searchTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(311, 543);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.high_range_tbx);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.low_range_tbx);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.calc_high_range_tbx);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 184);
            this.panel1.TabIndex = 0;
            // 
            // high_range_tbx
            // 
            this.high_range_tbx.Location = new System.Drawing.Point(173, 30);
            this.high_range_tbx.Name = "high_range_tbx";
            this.high_range_tbx.Size = new System.Drawing.Size(78, 20);
            this.high_range_tbx.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(170, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "High Range";
            // 
            // low_range_tbx
            // 
            this.low_range_tbx.Location = new System.Drawing.Point(6, 30);
            this.low_range_tbx.Name = "low_range_tbx";
            this.low_range_tbx.Size = new System.Drawing.Size(100, 20);
            this.low_range_tbx.TabIndex = 3;
            this.low_range_tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.low_range_tbx_KeyPress);
            this.low_range_tbx.KeyUp += new System.Windows.Forms.KeyEventHandler(this.low_range_tbx_KeyUp);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Low Range";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Calculated High Range";
            // 
            // calc_high_range_tbx
            // 
            this.calc_high_range_tbx.Enabled = false;
            this.calc_high_range_tbx.Location = new System.Drawing.Point(6, 83);
            this.calc_high_range_tbx.Name = "calc_high_range_tbx";
            this.calc_high_range_tbx.Size = new System.Drawing.Size(100, 20);
            this.calc_high_range_tbx.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 193);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(305, 347);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn});
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView1.DataSource = this.centerlineValuesBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(305, 347);
            this.dataGridView1.TabIndex = 0;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 110;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            this.valueDataGridViewTextBoxColumn.Width = 150;
            // 
            // centerlineValuesBindingSource
            // 
            this.centerlineValuesBindingSource.DataMember = "CenterlineValues";
            this.centerlineValuesBindingSource.DataSource = this.centerlinerangedataBindingSource;
            // 
            // centerlinerangedataBindingSource
            // 
            this.centerlinerangedataBindingSource.DataSource = this.centerline_range_data;
            this.centerlinerangedataBindingSource.Position = 0;
            // 
            // centerline_range_data
            // 
            this.centerline_range_data.DataSetName = "centerline_range_data";
            this.centerline_range_data.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // errorTab
            // 
            this.errorTab.Controls.Add(this.tableLayoutPanel3);
            this.errorTab.Location = new System.Drawing.Point(4, 22);
            this.errorTab.Name = "errorTab";
            this.errorTab.Size = new System.Drawing.Size(317, 549);
            this.errorTab.TabIndex = 2;
            this.errorTab.Text = "Error Explorer";
            this.errorTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.error_treeview, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.85064F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82.14936F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(317, 549);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(311, 92);
            this.panel3.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.errorCountLabel);
            this.panel4.Controls.Add(this.CheckFeaturesProgressBar);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.check_errors_btn);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(311, 92);
            this.panel4.TabIndex = 1;
            // 
            // errorCountLabel
            // 
            this.errorCountLabel.AutoSize = true;
            this.errorCountLabel.Location = new System.Drawing.Point(63, 15);
            this.errorCountLabel.Name = "errorCountLabel";
            this.errorCountLabel.Size = new System.Drawing.Size(13, 13);
            this.errorCountLabel.TabIndex = 4;
            this.errorCountLabel.Text = "0";
            // 
            // CheckFeaturesProgressBar
            // 
            this.CheckFeaturesProgressBar.Location = new System.Drawing.Point(146, 53);
            this.CheckFeaturesProgressBar.Name = "CheckFeaturesProgressBar";
            this.CheckFeaturesProgressBar.Size = new System.Drawing.Size(140, 23);
            this.CheckFeaturesProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.CheckFeaturesProgressBar.TabIndex = 3;
            this.CheckFeaturesProgressBar.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 15);
            this.label10.TabIndex = 2;
            this.label10.Text = "Errors: ";
            // 
            // check_errors_btn
            // 
            this.check_errors_btn.Location = new System.Drawing.Point(3, 53);
            this.check_errors_btn.Name = "check_errors_btn";
            this.check_errors_btn.Size = new System.Drawing.Size(124, 23);
            this.check_errors_btn.TabIndex = 1;
            this.check_errors_btn.Text = "Check Centerlines";
            this.check_errors_btn.UseVisualStyleBackColor = true;
            this.check_errors_btn.Click += new System.EventHandler(this.check_errors_btn_Click);
            // 
            // error_treeview
            // 
            this.error_treeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.error_treeview.Location = new System.Drawing.Point(3, 101);
            this.error_treeview.Name = "error_treeview";
            this.error_treeview.Size = new System.Drawing.Size(311, 445);
            this.error_treeview.TabIndex = 1;
            // 
            // settingsTab
            // 
            this.settingsTab.Controls.Add(this.tableLayoutPanel1);
            this.settingsTab.Location = new System.Drawing.Point(4, 22);
            this.settingsTab.Name = "settingsTab";
            this.settingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.settingsTab.Size = new System.Drawing.Size(317, 549);
            this.settingsTab.TabIndex = 1;
            this.settingsTab.Text = "Settings";
            this.settingsTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.LayersgroupBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.DisplaygroupBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(311, 543);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // LayersgroupBox
            // 
            this.LayersgroupBox.Controls.Add(this.matchCenterlineFieldsButton);
            this.LayersgroupBox.Controls.Add(this.matchAddressPointFieldsButton);
            this.LayersgroupBox.Controls.Add(this.addressPointcomboBox);
            this.LayersgroupBox.Controls.Add(this.label2);
            this.LayersgroupBox.Controls.Add(this.label1);
            this.LayersgroupBox.Controls.Add(this.centerlineLayerComboBox);
            this.LayersgroupBox.Location = new System.Drawing.Point(3, 3);
            this.LayersgroupBox.Name = "LayersgroupBox";
            this.LayersgroupBox.Size = new System.Drawing.Size(296, 174);
            this.LayersgroupBox.TabIndex = 0;
            this.LayersgroupBox.TabStop = false;
            this.LayersgroupBox.Text = "Layers";
            // 
            // matchCenterlineFieldsButton
            // 
            this.matchCenterlineFieldsButton.Location = new System.Drawing.Point(155, 64);
            this.matchCenterlineFieldsButton.Name = "matchCenterlineFieldsButton";
            this.matchCenterlineFieldsButton.Size = new System.Drawing.Size(75, 23);
            this.matchCenterlineFieldsButton.TabIndex = 5;
            this.matchCenterlineFieldsButton.Text = "Match Fields";
            this.matchCenterlineFieldsButton.UseVisualStyleBackColor = true;
            this.matchCenterlineFieldsButton.Click += new System.EventHandler(this.matchCenterlineFieldsButton_Click);
            // 
            // matchAddressPointFieldsButton
            // 
            this.matchAddressPointFieldsButton.Location = new System.Drawing.Point(155, 131);
            this.matchAddressPointFieldsButton.Name = "matchAddressPointFieldsButton";
            this.matchAddressPointFieldsButton.Size = new System.Drawing.Size(75, 23);
            this.matchAddressPointFieldsButton.TabIndex = 4;
            this.matchAddressPointFieldsButton.Text = "Match Fields";
            this.matchAddressPointFieldsButton.UseVisualStyleBackColor = true;
            this.matchAddressPointFieldsButton.Click += new System.EventHandler(this.matchAddressPointFieldsButton_Click);
            // 
            // addressPointcomboBox
            // 
            this.addressPointcomboBox.FormattingEnabled = true;
            this.addressPointcomboBox.Location = new System.Drawing.Point(16, 104);
            this.addressPointcomboBox.Name = "addressPointcomboBox";
            this.addressPointcomboBox.Size = new System.Drawing.Size(215, 21);
            this.addressPointcomboBox.TabIndex = 3;
            this.addressPointcomboBox.SelectedIndexChanged += new System.EventHandler(this.addressPointcomboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "AddressPoint Layer";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Centerline Layer";
            // 
            // centerlineLayerComboBox
            // 
            this.centerlineLayerComboBox.FormattingEnabled = true;
            this.centerlineLayerComboBox.Location = new System.Drawing.Point(18, 37);
            this.centerlineLayerComboBox.Name = "centerlineLayerComboBox";
            this.centerlineLayerComboBox.Size = new System.Drawing.Size(215, 21);
            this.centerlineLayerComboBox.TabIndex = 0;
            this.centerlineLayerComboBox.SelectedIndexChanged += new System.EventHandler(this.centerlineLayerComboBox_SelectedIndexChanged);
            // 
            // DisplaygroupBox
            // 
            this.DisplaygroupBox.Controls.Add(this.label8);
            this.DisplaygroupBox.Controls.Add(this.flashDelaycomboBox);
            this.DisplaygroupBox.Controls.Add(this.ChangeflashColorButton);
            this.DisplaygroupBox.Controls.Add(this.flashColorSwatch);
            this.DisplaygroupBox.Controls.Add(this.label6);
            this.DisplaygroupBox.Controls.Add(this.label5);
            this.DisplaygroupBox.Controls.Add(this.linefeedbackColorSwatch);
            this.DisplaygroupBox.Controls.Add(this.ChangeLineFeedBackColorButton);
            this.DisplaygroupBox.Location = new System.Drawing.Point(3, 280);
            this.DisplaygroupBox.Name = "DisplaygroupBox";
            this.DisplaygroupBox.Size = new System.Drawing.Size(296, 149);
            this.DisplaygroupBox.TabIndex = 2;
            this.DisplaygroupBox.TabStop = false;
            this.DisplaygroupBox.Text = "FeedBack";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(194, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Delay";
            // 
            // flashDelaycomboBox
            // 
            this.flashDelaycomboBox.FormattingEnabled = true;
            this.flashDelaycomboBox.Items.AddRange(new object[] {
            "0.6",
            "0.7",
            "0.8",
            "0.9",
            "1.0",
            "1.5",
            "2.0",
            "2.5"});
            this.flashDelaycomboBox.Location = new System.Drawing.Point(194, 88);
            this.flashDelaycomboBox.Name = "flashDelaycomboBox";
            this.flashDelaycomboBox.Size = new System.Drawing.Size(59, 21);
            this.flashDelaycomboBox.TabIndex = 7;
            this.flashDelaycomboBox.SelectionChangeCommitted += new System.EventHandler(this.flashDelaycomboBox_SelectionChangeCommitted);
            // 
            // ChangeflashColorButton
            // 
            this.ChangeflashColorButton.Location = new System.Drawing.Point(74, 86);
            this.ChangeflashColorButton.Name = "ChangeflashColorButton";
            this.ChangeflashColorButton.Size = new System.Drawing.Size(100, 23);
            this.ChangeflashColorButton.TabIndex = 5;
            this.ChangeflashColorButton.Text = "Change Color";
            this.ChangeflashColorButton.UseVisualStyleBackColor = true;
            this.ChangeflashColorButton.Click += new System.EventHandler(this.ChangeflashColorButton_Click);
            // 
            // flashColorSwatch
            // 
            this.flashColorSwatch.Location = new System.Drawing.Point(15, 86);
            this.flashColorSwatch.Name = "flashColorSwatch";
            this.flashColorSwatch.Size = new System.Drawing.Size(35, 23);
            this.flashColorSwatch.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Highlight Color";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Sketch Color";
            // 
            // linefeedbackColorSwatch
            // 
            this.linefeedbackColorSwatch.Location = new System.Drawing.Point(15, 33);
            this.linefeedbackColorSwatch.Name = "linefeedbackColorSwatch";
            this.linefeedbackColorSwatch.Size = new System.Drawing.Size(35, 23);
            this.linefeedbackColorSwatch.TabIndex = 2;
            // 
            // ChangeLineFeedBackColorButton
            // 
            this.ChangeLineFeedBackColorButton.Location = new System.Drawing.Point(74, 33);
            this.ChangeLineFeedBackColorButton.Name = "ChangeLineFeedBackColorButton";
            this.ChangeLineFeedBackColorButton.Size = new System.Drawing.Size(100, 23);
            this.ChangeLineFeedBackColorButton.TabIndex = 1;
            this.ChangeLineFeedBackColorButton.Text = "Change Color";
            this.ChangeLineFeedBackColorButton.UseVisualStyleBackColor = true;
            this.ChangeLineFeedBackColorButton.Click += new System.EventHandler(this.ChangeLineFeedBackColorButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.setAddressesPerUnit);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(3, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 91);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Misc";
            // 
            // setAddressesPerUnit
            // 
            this.setAddressesPerUnit.Location = new System.Drawing.Point(142, 41);
            this.setAddressesPerUnit.Name = "setAddressesPerUnit";
            this.setAddressesPerUnit.Size = new System.Drawing.Size(75, 23);
            this.setAddressesPerUnit.TabIndex = 2;
            this.setAddressesPerUnit.Text = "Set Value";
            this.setAddressesPerUnit.UseVisualStyleBackColor = true;
            this.setAddressesPerUnit.Click += new System.EventHandler(this.setAddressesPerUnit_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(35, 45);
            this.textBox1.MaxLength = 6;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Addresses Per Unit";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToToolStripMenuItem,
            this.flashToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 48);
            // 
            // zoomToToolStripMenuItem
            // 
            this.zoomToToolStripMenuItem.Name = "zoomToToolStripMenuItem";
            this.zoomToToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.zoomToToolStripMenuItem.Text = "Zoom To";
            this.zoomToToolStripMenuItem.Click += new System.EventHandler(this.zoomToToolStripMenuItem_Click);
            // 
            // flashToolStripMenuItem
            // 
            this.flashToolStripMenuItem.Name = "flashToolStripMenuItem";
            this.flashToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.flashToolStripMenuItem.Text = "Flash";
            this.flashToolStripMenuItem.Click += new System.EventHandler(this.flashToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(123, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            this.toolStripMenuItem1.Text = "Zoom To";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // AddressingWindow
            // 
            this.Controls.Add(this.AddressingWindowTabControl);
            this.Name = "AddressingWindow";
            this.Size = new System.Drawing.Size(325, 575);
            this.AddressingWindowTabControl.ResumeLayout(false);
            this.searchTab.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerlineValuesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerlinerangedataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerline_range_data)).EndInit();
            this.errorTab.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.settingsTab.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.LayersgroupBox.ResumeLayout(false);
            this.LayersgroupBox.PerformLayout();
            this.DisplaygroupBox.ResumeLayout(false);
            this.DisplaygroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl AddressingWindowTabControl;
        private System.Windows.Forms.TabPage settingsTab;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox LayersgroupBox;
        private System.Windows.Forms.Button matchCenterlineFieldsButton;
        private System.Windows.Forms.Button matchAddressPointFieldsButton;
        private System.Windows.Forms.ComboBox addressPointcomboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox centerlineLayerComboBox;
        private System.Windows.Forms.GroupBox DisplaygroupBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox flashDelaycomboBox;
        private System.Windows.Forms.Button ChangeflashColorButton;
        private System.Windows.Forms.Panel flashColorSwatch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel linefeedbackColorSwatch;
        private System.Windows.Forms.Button ChangeLineFeedBackColorButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button setAddressesPerUnit;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage searchTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox high_range_tbx;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox low_range_tbx;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox calc_high_range_tbx;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource centerlinerangedataBindingSource;
        private centerline_range_data centerline_range_data;
        private System.Windows.Forms.BindingSource centerlineValuesBindingSource;
        private System.Windows.Forms.TabPage errorTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button check_errors_btn;
        private System.Windows.Forms.TreeView error_treeview;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem zoomToToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripMenuItem flashToolStripMenuItem;
        private System.Windows.Forms.Label errorCountLabel;
        private System.Windows.Forms.ProgressBar CheckFeaturesProgressBar;
        private System.Windows.Forms.Label label10;

    }
}
