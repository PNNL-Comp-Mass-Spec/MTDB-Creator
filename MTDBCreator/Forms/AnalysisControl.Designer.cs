namespace MTDBCreator.Forms
{
    partial class AnalysisControl
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
            m_processor.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            PNNL.Controls.PenProvider penProvider1 = new PNNL.Controls.PenProvider();
            PNNL.Controls.PenProvider penProvider2 = new PNNL.Controls.PenProvider();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalysisControl));
            this.ctlChartScanVsNET = new PNNL.Controls.ctlScatterChart();
            this.listViewDatasets = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDataset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNumUniqueMassTags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_proteins = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.mbutton_process = new System.Windows.Forms.Button();
            this.mbutton_showDatabase = new System.Windows.Forms.Button();
            this.mbutton_saveDatabase = new System.Windows.Forms.Button();
            this.mbutton_options = new System.Windows.Forms.Button();
            this.mcheckBox_displayRegression = new System.Windows.Forms.CheckBox();
            this.radioButtonAverageNET = new System.Windows.Forms.RadioButton();
            this.radioButtonPredictedNET = new System.Windows.Forms.RadioButton();
            this.mcheckBox_useResiduals = new System.Windows.Forms.CheckBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.statusBarStrip = new System.Windows.Forms.StatusStrip();
            this.statusHover = new System.Windows.Forms.ToolStripStatusLabel();
            this.mprogress_analysis = new System.Windows.Forms.ToolStripProgressBar();
            this.databaseControl1 = new MTDBCreator.Forms.DatabaseControl();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ctlChartScanVsNET)).BeginInit();
            this.panel1.SuspendLayout();
            this.statusBarStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctlChartScanVsNET
            // 
            this.ctlChartScanVsNET.AutoViewPortXBase = 0F;
            this.ctlChartScanVsNET.AutoViewPortYBase = 0F;
            this.ctlChartScanVsNET.AxisAndLabelFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.ctlChartScanVsNET.AxisAndLabelMaxFontSize = 15;
            this.ctlChartScanVsNET.AxisAndLabelMinFontSize = 8;
            this.ctlChartScanVsNET.BackColor = System.Drawing.Color.White;
            this.ctlChartScanVsNET.ChartBackgroundColor = System.Drawing.Color.White;
            this.ctlChartScanVsNET.ChartLayout.LegendFraction = 0.2F;
            this.ctlChartScanVsNET.ChartLayout.LegendLocation = PNNL.Controls.ChartLegendLocation.Right;
            this.ctlChartScanVsNET.ChartLayout.MaxLegendHeight = 150;
            this.ctlChartScanVsNET.ChartLayout.MaxLegendWidth = 250;
            this.ctlChartScanVsNET.ChartLayout.MaxTitleHeight = 50;
            this.ctlChartScanVsNET.ChartLayout.MinLegendHeight = 50;
            this.ctlChartScanVsNET.ChartLayout.MinLegendWidth = 75;
            this.ctlChartScanVsNET.ChartLayout.MinTitleHeight = 15;
            this.ctlChartScanVsNET.ChartLayout.TitleFraction = 0.1F;
            this.ctlChartScanVsNET.DefaultZoomHandler.Active = true;
            this.ctlChartScanVsNET.DefaultZoomHandler.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(119)))), ((int)(((byte)(136)))), ((int)(((byte)(153)))));
            this.ctlChartScanVsNET.DefaultZoomHandler.LineColor = System.Drawing.Color.Black;
            this.ctlChartScanVsNET.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlChartScanVsNET.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            penProvider1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            penProvider1.Width = 1F;
            this.ctlChartScanVsNET.GridLinePen = penProvider1;
            this.ctlChartScanVsNET.HilightColor = System.Drawing.Color.Magenta;
            this.ctlChartScanVsNET.Legend.BackColor = System.Drawing.Color.Transparent;
            penProvider2.Color = System.Drawing.Color.Black;
            penProvider2.Width = 1F;
            this.ctlChartScanVsNET.Legend.BorderPen = penProvider2;
            this.ctlChartScanVsNET.Legend.Bounds = new System.Drawing.Rectangle(547, 66, 130, 270);
            this.ctlChartScanVsNET.Legend.ColumnWidth = 125;
            this.ctlChartScanVsNET.Legend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ctlChartScanVsNET.Legend.MaxFontSize = 12F;
            this.ctlChartScanVsNET.Legend.MinFontSize = 6F;
            this.ctlChartScanVsNET.Location = new System.Drawing.Point(192, 0);
            this.ctlChartScanVsNET.Margins.BottomMarginFraction = 0.1F;
            this.ctlChartScanVsNET.Margins.BottomMarginMax = 72;
            this.ctlChartScanVsNET.Margins.BottomMarginMin = 30;
            this.ctlChartScanVsNET.Margins.DefaultMarginFraction = 0.05F;
            this.ctlChartScanVsNET.Margins.DefaultMarginMax = 15;
            this.ctlChartScanVsNET.Margins.DefaultMarginMin = 5;
            this.ctlChartScanVsNET.Margins.LeftMarginFraction = 0.2F;
            this.ctlChartScanVsNET.Margins.LeftMarginMax = 150;
            this.ctlChartScanVsNET.Margins.LeftMarginMin = 72;
            this.ctlChartScanVsNET.Name = "ctlChartScanVsNET";
            this.ctlChartScanVsNET.Size = new System.Drawing.Size(683, 367);
            this.ctlChartScanVsNET.TabIndex = 15;
            this.ctlChartScanVsNET.Title = "";
            this.ctlChartScanVsNET.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ctlChartScanVsNET.TitleMaxFontSize = 50F;
            this.ctlChartScanVsNET.TitleMinFontSize = 6F;
            this.ctlChartScanVsNET.VerticalExpansion = 1F;
            this.ctlChartScanVsNET.ViewPort = ((System.Drawing.RectangleF)(resources.GetObject("ctlChartScanVsNET.ViewPort")));
            this.ctlChartScanVsNET.XAxisLabel = "Scan";
            this.ctlChartScanVsNET.YAxisLabel = "Predicted NET ";
            // 
            // listViewDatasets
            // 
            this.listViewDatasets.BackColor = System.Drawing.Color.White;
            this.listViewDatasets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeaderDataset,
            this.columnHeaderNumUniqueMassTags,
            this.columnHeader_proteins});
            this.listViewDatasets.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listViewDatasets.FullRowSelect = true;
            this.listViewDatasets.GridLines = true;
            this.listViewDatasets.HideSelection = false;
            this.listViewDatasets.Location = new System.Drawing.Point(192, 400);
            this.listViewDatasets.MultiSelect = false;
            this.listViewDatasets.Name = "listViewDatasets";
            this.listViewDatasets.Size = new System.Drawing.Size(683, 208);
            this.listViewDatasets.TabIndex = 14;
            this.listViewDatasets.UseCompatibleStateImageBehavior = false;
            this.listViewDatasets.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 18;
            // 
            // columnHeaderDataset
            // 
            this.columnHeaderDataset.Text = "Dataset Name";
            this.columnHeaderDataset.Width = 400;
            // 
            // columnHeaderNumUniqueMassTags
            // 
            this.columnHeaderNumUniqueMassTags.Text = "Unique Mass Tags";
            this.columnHeaderNumUniqueMassTags.Width = 140;
            // 
            // columnHeader_proteins
            // 
            this.columnHeader_proteins.Text = "Protein Count";
            this.columnHeader_proteins.Width = 91;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.mbutton_process);
            this.panel1.Controls.Add(this.mbutton_showDatabase);
            this.panel1.Controls.Add(this.mbutton_saveDatabase);
            this.panel1.Controls.Add(this.mbutton_options);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(186, 608);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(689, 41);
            this.panel1.TabIndex = 16;
            // 
            // mbutton_process
            // 
            this.mbutton_process.BackColor = System.Drawing.Color.White;
            this.mbutton_process.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.mbutton_process.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbutton_process.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mbutton_process.ForeColor = System.Drawing.Color.Black;
            this.mbutton_process.Location = new System.Drawing.Point(261, 6);
            this.mbutton_process.Name = "mbutton_process";
            this.mbutton_process.Size = new System.Drawing.Size(138, 27);
            this.mbutton_process.TabIndex = 4;
            this.mbutton_process.Text = "Create Database";
            this.mbutton_process.UseVisualStyleBackColor = false;
            this.mbutton_process.Click += new System.EventHandler(this.mbutton_process_Click);
            // 
            // mbutton_showDatabase
            // 
            this.mbutton_showDatabase.BackColor = System.Drawing.Color.White;
            this.mbutton_showDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbutton_showDatabase.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mbutton_showDatabase.ForeColor = System.Drawing.Color.Black;
            this.mbutton_showDatabase.Location = new System.Drawing.Point(9, 6);
            this.mbutton_showDatabase.Name = "mbutton_showDatabase";
            this.mbutton_showDatabase.Size = new System.Drawing.Size(136, 26);
            this.mbutton_showDatabase.TabIndex = 3;
            this.mbutton_showDatabase.Text = "Display All";
            this.mbutton_showDatabase.UseVisualStyleBackColor = false;
            this.mbutton_showDatabase.Click += new System.EventHandler(this.mbutton_showDatabase_Click);
            // 
            // mbutton_saveDatabase
            // 
            this.mbutton_saveDatabase.BackColor = System.Drawing.Color.Red;
            this.mbutton_saveDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbutton_saveDatabase.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mbutton_saveDatabase.ForeColor = System.Drawing.Color.White;
            this.mbutton_saveDatabase.Location = new System.Drawing.Point(405, 6);
            this.mbutton_saveDatabase.Name = "mbutton_saveDatabase";
            this.mbutton_saveDatabase.Size = new System.Drawing.Size(138, 27);
            this.mbutton_saveDatabase.TabIndex = 3;
            this.mbutton_saveDatabase.Text = "Save Database";
            this.mbutton_saveDatabase.UseVisualStyleBackColor = false;
            this.mbutton_saveDatabase.Visible = false;
            this.mbutton_saveDatabase.Click += new System.EventHandler(this.mbutton_saveDatabase_Click);
            // 
            // mbutton_options
            // 
            this.mbutton_options.BackColor = System.Drawing.Color.White;
            this.mbutton_options.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbutton_options.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mbutton_options.ForeColor = System.Drawing.Color.Black;
            this.mbutton_options.Location = new System.Drawing.Point(151, 6);
            this.mbutton_options.Name = "mbutton_options";
            this.mbutton_options.Size = new System.Drawing.Size(104, 27);
            this.mbutton_options.TabIndex = 1;
            this.mbutton_options.Text = "Options";
            this.mbutton_options.UseVisualStyleBackColor = false;
            this.mbutton_options.Click += new System.EventHandler(this.mbutton_options_Click);
            // 
            // mcheckBox_displayRegression
            // 
            this.mcheckBox_displayRegression.Location = new System.Drawing.Point(399, 2);
            this.mcheckBox_displayRegression.Name = "mcheckBox_displayRegression";
            this.mcheckBox_displayRegression.Size = new System.Drawing.Size(176, 17);
            this.mcheckBox_displayRegression.TabIndex = 5;
            this.mcheckBox_displayRegression.Text = "Display Regression";
            this.mcheckBox_displayRegression.UseVisualStyleBackColor = true;
            this.mcheckBox_displayRegression.CheckedChanged += new System.EventHandler(this.mcheckBox_displayRegression_CheckedChanged);
            // 
            // radioButtonAverageNET
            // 
            this.radioButtonAverageNET.Location = new System.Drawing.Point(116, 3);
            this.radioButtonAverageNET.Name = "radioButtonAverageNET";
            this.radioButtonAverageNET.Size = new System.Drawing.Size(153, 18);
            this.radioButtonAverageNET.TabIndex = 1;
            this.radioButtonAverageNET.Text = "Average Observed NET";
            this.radioButtonAverageNET.CheckedChanged += new System.EventHandler(this.radioButtonAverageNET_CheckedChanged);
            // 
            // radioButtonPredictedNET
            // 
            this.radioButtonPredictedNET.Checked = true;
            this.radioButtonPredictedNET.Location = new System.Drawing.Point(3, 2);
            this.radioButtonPredictedNET.Name = "radioButtonPredictedNET";
            this.radioButtonPredictedNET.Size = new System.Drawing.Size(107, 18);
            this.radioButtonPredictedNET.TabIndex = 0;
            this.radioButtonPredictedNET.TabStop = true;
            this.radioButtonPredictedNET.Text = "Predicted NET";
            this.radioButtonPredictedNET.CheckedChanged += new System.EventHandler(this.radioButtonPredictedNET_CheckedChanged);
            // 
            // mcheckBox_useResiduals
            // 
            this.mcheckBox_useResiduals.Location = new System.Drawing.Point(275, 3);
            this.mcheckBox_useResiduals.Name = "mcheckBox_useResiduals";
            this.mcheckBox_useResiduals.Size = new System.Drawing.Size(118, 16);
            this.mcheckBox_useResiduals.TabIndex = 4;
            this.mcheckBox_useResiduals.Text = "Display Residuals";
            this.mcheckBox_useResiduals.UseVisualStyleBackColor = true;
            this.mcheckBox_useResiduals.CheckedChanged += new System.EventHandler(this.mcheckBox_useResiduals_CheckedChanged);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.Silver;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(192, 394);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(683, 6);
            this.splitter1.TabIndex = 18;
            this.splitter1.TabStop = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "arrow-58-16.png");
            this.imageList1.Images.SetKeyName(1, "check-mark-7-16.png");
            this.imageList1.Images.SetKeyName(2, "square-rounded-16.png");
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.Color.Silver;
            this.splitter2.Location = new System.Drawing.Point(186, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(6, 608);
            this.splitter2.TabIndex = 19;
            this.splitter2.TabStop = false;
            // 
            // statusBarStrip
            // 
            this.statusBarStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusBarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusHover,
            this.mprogress_analysis});
            this.statusBarStrip.Location = new System.Drawing.Point(0, 649);
            this.statusBarStrip.Name = "statusBarStrip";
            this.statusBarStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusBarStrip.Size = new System.Drawing.Size(875, 22);
            this.statusBarStrip.TabIndex = 20;
            this.statusBarStrip.Text = "statusStrip1";
            // 
            // statusHover
            // 
            this.statusHover.Name = "statusHover";
            this.statusHover.Size = new System.Drawing.Size(42, 17);
            this.statusHover.Text = "Ready.";
            // 
            // mprogress_analysis
            // 
            this.mprogress_analysis.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mprogress_analysis.ForeColor = System.Drawing.Color.Green;
            this.mprogress_analysis.Name = "mprogress_analysis";
            this.mprogress_analysis.Size = new System.Drawing.Size(200, 16);
            this.mprogress_analysis.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.mprogress_analysis.Visible = false;
            // 
            // databaseControl1
            // 
            this.databaseControl1.BackColor = System.Drawing.Color.White;
            this.databaseControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.databaseControl1.Location = new System.Drawing.Point(0, 0);
            this.databaseControl1.Name = "databaseControl1";
            this.databaseControl1.Size = new System.Drawing.Size(186, 649);
            this.databaseControl1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.mcheckBox_displayRegression);
            this.panel2.Controls.Add(this.radioButtonAverageNET);
            this.panel2.Controls.Add(this.mcheckBox_useResiduals);
            this.panel2.Controls.Add(this.radioButtonPredictedNET);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(192, 367);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(683, 27);
            this.panel2.TabIndex = 17;
            // 
            // AnalysisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctlChartScanVsNET);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.listViewDatasets);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.databaseControl1);
            this.Controls.Add(this.statusBarStrip);
            this.Name = "AnalysisControl";
            this.Size = new System.Drawing.Size(875, 671);
            ((System.ComponentModel.ISupportInitialize)(this.ctlChartScanVsNET)).EndInit();
            this.panel1.ResumeLayout(false);
            this.statusBarStrip.ResumeLayout(false);
            this.statusBarStrip.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PNNL.Controls.ctlScatterChart ctlChartScanVsNET;
        private System.Windows.Forms.ListView listViewDatasets;
        private System.Windows.Forms.ColumnHeader columnHeaderDataset;
        private System.Windows.Forms.ColumnHeader columnHeaderNumUniqueMassTags;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button mbutton_options;
        private System.Windows.Forms.RadioButton radioButtonAverageNET;
        private System.Windows.Forms.RadioButton radioButtonPredictedNET;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button mbutton_showDatabase;
        private System.Windows.Forms.Button mbutton_saveDatabase;
        private System.Windows.Forms.ColumnHeader columnHeader_proteins;
        private System.Windows.Forms.CheckBox mcheckBox_useResiduals;
        private System.Windows.Forms.Button mbutton_process;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private DatabaseControl databaseControl1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.StatusStrip statusBarStrip;
        private System.Windows.Forms.ToolStripProgressBar mprogress_analysis;
        private System.Windows.Forms.ToolStripStatusLabel statusHover;
        private System.Windows.Forms.CheckBox mcheckBox_displayRegression;
        private System.Windows.Forms.Panel panel2;
    }
}
