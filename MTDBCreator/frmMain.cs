using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb ; 
using PNNL.Controls ; 
namespace MTDBCreator
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private PNNL.Controls.ExpandPanel expandPanelLists;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private PNNL.Controls.ExpandPanel expandPanelOptions;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.MenuItem menuItemOpenList;
		private System.Windows.Forms.ListView listViewDatasets;
		private System.Windows.Forms.ColumnHeader columnHeaderId;
		private System.Windows.Forms.ColumnHeader columnHeaderDataset;
		private System.Windows.Forms.ColumnHeader columnHeaderSlope;
		private System.Windows.Forms.ColumnHeader columnHeaderIntercept;
		private System.Windows.Forms.ColumnHeader columnHeaderRSquared;
		private System.Windows.Forms.ColumnHeader columnHeaderNumUniqueMassTags;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButtonPredictedNET;
		private System.Windows.Forms.RadioButton radioButtonAverageNET;
		private System.Windows.Forms.CheckBox checkBoxShowResiduals;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private clsAnalysisDescription [] marrAnalyses ; 
		private const int mintAnalysisIdColumn = 0 ; 
		private const int mintDatasetNameColumn = 1 ; 
		private const int mintSlopeColumn = 2 ; 
		private const int mintInterceptColumn = 3 ; 
		private const int mintRSquaredColumn = 4 ; 
		private const int mintNumMassTagsColumn = 5 ; 
		
		private string mstrAccessDbPath ;
		private clsMTDB mobjMTDB ; 
		private float [] marrPeptideScans ; 
		private float [] marrPeptidePredictedNETs ; 
		private double mdbl_slope ; 
		private double mdbl_intercept ; 

		private clsXTandemAnalysisReader mobjXTandemPHRPResultsReader ; 
		private PNNL.Controls.clsPlotParams mobjPlotParams ;
		private PNNL.Controls.ctlScatterChart ctlChartScanVsNET; 
		private PNNL.Controls.clsSeries mobjSeries ;
		private System.Windows.Forms.GroupBox groupBoxAlignmentType;
		private System.Windows.Forms.RadioButton radioButtonLinearEM;
		private System.Windows.Forms.RadioButton radioButtonWARP;
		private System.Windows.Forms.RadioButton radioButtonCubicSpline;
		private System.Windows.Forms.RadioButton radioButtonHybrid;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemCreateDB; 
		private PNNL.Controls.ChartPostRenderingProcessor mobjTransformPlotter ;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxMinObservations;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox textBoxMaxLogE;
		private System.Windows.Forms.Label label2;
		frmStatus mfrmStatus = new frmStatus() ; 
		private string 	mstrErrors = "" ; 

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
//			Microsoft.Office.Interop.Access.ApplicationClass oAccess = new Microsoft.Office.Interop.Access.ApplicationClass() ; 
//			oAccess.OpenCurrentDatabase("c:\\projects\\mtdbcreator\\data\\Model_AMT_Database.mdb", false, "") ; 
//			dao.Relation relation = oAccess.CurrentDb().Relations[0] ; 

			Init() ;
			menuItemCreateDB.Enabled = false ; 
		}


		private void Init() 
		{
			PNNL.Controls.BubbleShape shape = new PNNL.Controls.BubbleShape(1, false) ; 
			mobjPlotParams = new PNNL.Controls.clsPlotParams(shape, Color.Blue, false, true, true) ; 
			mobjTransformPlotter = new ChartPostRenderingProcessor(this.DrawNetRegressionLine) ; 
			ctlChartScanVsNET.AddPostProcessor(mobjTransformPlotter, PostProcessPriority.Mid) ;
			mdbl_slope = 0 ; 
			mdbl_intercept = 0 ; 
			marrPeptidePredictedNETs = new float[1] ; 
			marrPeptideScans = new float[1] ; 
			mobjMTDB = new clsMTDB(mfrmStatus) ; 
		}


//		private void DebugOnly()
//		{
//
//			mstrAccessDbPath = "c:\\projects\\mtdbcreator\\data\\test1.mdb" ; 
//			try
//			{
//				System.IO.File.Delete(mstrAccessDbPath) ; 
//			}
//			catch (Exception ex)
//			{
//			}
//
//			string datasetFile = "c:\\projects\\mtdbcreator\\data\\DatasetsDescription.txt" ; 
//			clsAnalysisDescriptionReader reader = new clsAnalysisDescriptionReader(datasetFile) ; 
//			marrAnalyses = reader.Analyses ; 
//			AddAnalysesTolist(marrAnalyses) ; 			
//			mobjMTDB.AccessDBPath = mstrAccessDbPath ; 
//			System.Threading.ThreadStart start = new System.Threading.ThreadStart(this.ProcessAllDatasets) ; 
//			System.Threading.Thread thrd = new System.Threading.Thread(start) ; 
//			thrd.Start() ; 
//
//		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			PNNL.Controls.PenProvider penProvider1 = new PNNL.Controls.PenProvider();
			PNNL.Controls.PenProvider penProvider2 = new PNNL.Controls.PenProvider();
			this.expandPanelLists = new PNNL.Controls.ExpandPanel(300);
			this.listViewDatasets = new System.Windows.Forms.ListView();
			this.columnHeaderId = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderDataset = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderSlope = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderIntercept = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderRSquared = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderNumUniqueMassTags = new System.Windows.Forms.ColumnHeader();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.expandPanelOptions = new PNNL.Controls.ExpandPanel(196);
			this.groupBoxAlignmentType = new System.Windows.Forms.GroupBox();
			this.radioButtonHybrid = new System.Windows.Forms.RadioButton();
			this.radioButtonCubicSpline = new System.Windows.Forms.RadioButton();
			this.radioButtonWARP = new System.Windows.Forms.RadioButton();
			this.radioButtonLinearEM = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.textBoxMinObservations = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkBoxShowResiduals = new System.Windows.Forms.CheckBox();
			this.radioButtonAverageNET = new System.Windows.Forms.RadioButton();
			this.radioButtonPredictedNET = new System.Windows.Forms.RadioButton();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemOpenList = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemCreateDB = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.ctlChartScanVsNET = new PNNL.Controls.ctlScatterChart();
			this.panel2 = new System.Windows.Forms.Panel();
			this.textBoxMaxLogE = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.expandPanelLists)).BeginInit();
			this.expandPanelLists.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.expandPanelOptions)).BeginInit();
			this.expandPanelOptions.SuspendLayout();
			this.groupBoxAlignmentType.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ctlChartScanVsNET)).BeginInit();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// expandPanelLists
			// 
			this.expandPanelLists.Controls.Add(this.listViewDatasets);
			this.expandPanelLists.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.expandPanelLists.ExpandImage = ((System.Drawing.Image)(resources.GetObject("expandPanelLists.ExpandImage")));
			this.expandPanelLists.HeaderRightToLeft = System.Windows.Forms.RightToLeft.No;
			this.expandPanelLists.HeaderTextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.expandPanelLists.Location = new System.Drawing.Point(0, 473);
			this.expandPanelLists.Name = "expandPanelLists";
			this.expandPanelLists.Size = new System.Drawing.Size(1136, 320);
			this.expandPanelLists.TabIndex = 5;
			// 
			// listViewDatasets
			// 
			this.listViewDatasets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.columnHeaderId,
																							   this.columnHeaderDataset,
																							   this.columnHeaderSlope,
																							   this.columnHeaderIntercept,
																							   this.columnHeaderRSquared,
																							   this.columnHeaderNumUniqueMassTags});
			this.listViewDatasets.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewDatasets.FullRowSelect = true;
			this.listViewDatasets.GridLines = true;
			this.listViewDatasets.Location = new System.Drawing.Point(1, 20);
			this.listViewDatasets.MultiSelect = false;
			this.listViewDatasets.Name = "listViewDatasets";
			this.listViewDatasets.Size = new System.Drawing.Size(1134, 299);
			this.listViewDatasets.TabIndex = 2;
			this.listViewDatasets.View = System.Windows.Forms.View.Details;
			this.listViewDatasets.SelectedIndexChanged += new System.EventHandler(this.listViewDatasets_SelectedIndexChanged);
			// 
			// columnHeaderId
			// 
			this.columnHeaderId.Text = "Analysis Id";
			this.columnHeaderId.Width = 80;
			// 
			// columnHeaderDataset
			// 
			this.columnHeaderDataset.Text = "Dataset Name";
			this.columnHeaderDataset.Width = 400;
			// 
			// columnHeaderSlope
			// 
			this.columnHeaderSlope.Text = "Slope";
			// 
			// columnHeaderIntercept
			// 
			this.columnHeaderIntercept.Text = "Intercept";
			// 
			// columnHeaderRSquared
			// 
			this.columnHeaderRSquared.Text = "R Squared";
			this.columnHeaderRSquared.Width = 80;
			// 
			// columnHeaderNumUniqueMassTags
			// 
			this.columnHeaderNumUniqueMassTags.Text = "# of Unique Mass Tags";
			this.columnHeaderNumUniqueMassTags.Width = 140;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 467);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(1136, 6);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// expandPanelOptions
			// 
			this.expandPanelOptions.Controls.Add(this.groupBoxAlignmentType);
			this.expandPanelOptions.Controls.Add(this.groupBox1);
			this.expandPanelOptions.Dock = System.Windows.Forms.DockStyle.Right;
			this.expandPanelOptions.DockPadding.All = 10;
			this.expandPanelOptions.ExpandDirection = PNNL.Controls.ExpandDirection.Left;
			this.expandPanelOptions.ExpandImage = ((System.Drawing.Image)(resources.GetObject("expandPanelOptions.ExpandImage")));
			this.expandPanelOptions.HeaderContractText = "Hide Options";
			this.expandPanelOptions.HeaderExpandText = "Show Options";
			this.expandPanelOptions.HeaderLocation = PNNL.Controls.HeaderLocation.Left;
			this.expandPanelOptions.HeaderRightToLeft = System.Windows.Forms.RightToLeft.No;
			this.expandPanelOptions.HeaderText = "Hide Options";
			this.expandPanelOptions.HeaderTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.expandPanelOptions.HeaderTextDirection = PNNL.Controls.TextDirection.Vertical;
			this.expandPanelOptions.HeaderTextMode = PNNL.Controls.HeaderTextSwitchingMode.ExpandContractText;
			this.expandPanelOptions.Location = new System.Drawing.Point(920, 0);
			this.expandPanelOptions.Name = "expandPanelOptions";
			this.expandPanelOptions.Size = new System.Drawing.Size(216, 467);
			this.expandPanelOptions.TabIndex = 4;
			// 
			// groupBoxAlignmentType
			// 
			this.groupBoxAlignmentType.Controls.Add(this.radioButtonHybrid);
			this.groupBoxAlignmentType.Controls.Add(this.radioButtonCubicSpline);
			this.groupBoxAlignmentType.Controls.Add(this.radioButtonWARP);
			this.groupBoxAlignmentType.Controls.Add(this.radioButtonLinearEM);
			this.groupBoxAlignmentType.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBoxAlignmentType.Location = new System.Drawing.Point(20, 168);
			this.groupBoxAlignmentType.Name = "groupBoxAlignmentType";
			this.groupBoxAlignmentType.Size = new System.Drawing.Size(194, 87);
			this.groupBoxAlignmentType.TabIndex = 5;
			this.groupBoxAlignmentType.TabStop = false;
			this.groupBoxAlignmentType.Text = "Alignment Type";
			this.groupBoxAlignmentType.Visible = false;
			// 
			// radioButtonHybrid
			// 
			this.radioButtonHybrid.Dock = System.Windows.Forms.DockStyle.Top;
			this.radioButtonHybrid.Location = new System.Drawing.Point(3, 64);
			this.radioButtonHybrid.Name = "radioButtonHybrid";
			this.radioButtonHybrid.Size = new System.Drawing.Size(188, 16);
			this.radioButtonHybrid.TabIndex = 4;
			this.radioButtonHybrid.Text = "Hybrid";
			// 
			// radioButtonCubicSpline
			// 
			this.radioButtonCubicSpline.Dock = System.Windows.Forms.DockStyle.Top;
			this.radioButtonCubicSpline.Location = new System.Drawing.Point(3, 48);
			this.radioButtonCubicSpline.Name = "radioButtonCubicSpline";
			this.radioButtonCubicSpline.Size = new System.Drawing.Size(188, 16);
			this.radioButtonCubicSpline.TabIndex = 3;
			this.radioButtonCubicSpline.Text = "Cubic Spline";
			// 
			// radioButtonWARP
			// 
			this.radioButtonWARP.Dock = System.Windows.Forms.DockStyle.Top;
			this.radioButtonWARP.Location = new System.Drawing.Point(3, 32);
			this.radioButtonWARP.Name = "radioButtonWARP";
			this.radioButtonWARP.Size = new System.Drawing.Size(188, 16);
			this.radioButtonWARP.TabIndex = 2;
			this.radioButtonWARP.Text = "Warp";
			// 
			// radioButtonLinearEM
			// 
			this.radioButtonLinearEM.Checked = true;
			this.radioButtonLinearEM.Dock = System.Windows.Forms.DockStyle.Top;
			this.radioButtonLinearEM.Location = new System.Drawing.Point(3, 16);
			this.radioButtonLinearEM.Name = "radioButtonLinearEM";
			this.radioButtonLinearEM.Size = new System.Drawing.Size(188, 16);
			this.radioButtonLinearEM.TabIndex = 1;
			this.radioButtonLinearEM.TabStop = true;
			this.radioButtonLinearEM.Text = "Linear ";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.panel2);
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Controls.Add(this.checkBoxShowResiduals);
			this.groupBox1.Controls.Add(this.radioButtonAverageNET);
			this.groupBox1.Controls.Add(this.radioButtonPredictedNET);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(20, 1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(194, 167);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "NET Values";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.textBoxMinObservations);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 104);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(188, 24);
			this.panel1.TabIndex = 4;
			// 
			// textBoxMinObservations
			// 
			this.textBoxMinObservations.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxMinObservations.Enabled = false;
			this.textBoxMinObservations.Location = new System.Drawing.Point(104, 0);
			this.textBoxMinObservations.Name = "textBoxMinObservations";
			this.textBoxMinObservations.Size = new System.Drawing.Size(84, 20);
			this.textBoxMinObservations.TabIndex = 1;
			this.textBoxMinObservations.Text = "3";
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Min Observations:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkBoxShowResiduals
			// 
			this.checkBoxShowResiduals.Dock = System.Windows.Forms.DockStyle.Top;
			this.checkBoxShowResiduals.Location = new System.Drawing.Point(3, 80);
			this.checkBoxShowResiduals.Name = "checkBoxShowResiduals";
			this.checkBoxShowResiduals.Size = new System.Drawing.Size(188, 24);
			this.checkBoxShowResiduals.TabIndex = 3;
			this.checkBoxShowResiduals.Text = "Show Residuals";
			this.checkBoxShowResiduals.CheckedChanged += new System.EventHandler(this.checkBoxShowResiduals_CheckedChanged);
			// 
			// radioButtonAverageNET
			// 
			this.radioButtonAverageNET.Dock = System.Windows.Forms.DockStyle.Top;
			this.radioButtonAverageNET.Location = new System.Drawing.Point(3, 48);
			this.radioButtonAverageNET.Name = "radioButtonAverageNET";
			this.radioButtonAverageNET.Size = new System.Drawing.Size(188, 32);
			this.radioButtonAverageNET.TabIndex = 1;
			this.radioButtonAverageNET.Text = "Average Observed NET";
			this.radioButtonAverageNET.CheckedChanged += new System.EventHandler(this.radioButtonAverageNET_CheckedChanged);
			// 
			// radioButtonPredictedNET
			// 
			this.radioButtonPredictedNET.Checked = true;
			this.radioButtonPredictedNET.Dock = System.Windows.Forms.DockStyle.Top;
			this.radioButtonPredictedNET.Location = new System.Drawing.Point(3, 16);
			this.radioButtonPredictedNET.Name = "radioButtonPredictedNET";
			this.radioButtonPredictedNET.Size = new System.Drawing.Size(188, 32);
			this.radioButtonPredictedNET.TabIndex = 0;
			this.radioButtonPredictedNET.TabStop = true;
			this.radioButtonPredictedNET.Text = "Predicted NET";
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem2,
																					  this.menuItem3});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemOpenList});
			this.menuItem1.Text = "File";
			// 
			// menuItemOpenList
			// 
			this.menuItemOpenList.Index = 0;
			this.menuItemOpenList.Text = "Open Dataset List";
			this.menuItemOpenList.Click += new System.EventHandler(this.menuItemOpenList_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemCreateDB});
			this.menuItem2.Text = "Tools";
			// 
			// menuItemCreateDB
			// 
			this.menuItemCreateDB.Index = 0;
			this.menuItemCreateDB.Text = "Create Mass Tag Database";
			this.menuItemCreateDB.Click += new System.EventHandler(this.menuItemCreateDB_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "About";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter2.Location = new System.Drawing.Point(914, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(6, 467);
			this.splitter2.TabIndex = 3;
			this.splitter2.TabStop = false;
			// 
			// ctlChartScanVsNET
			// 
			this.ctlChartScanVsNET.AutoViewPortXBase = 0F;
			this.ctlChartScanVsNET.AutoViewPortYBase = 0F;
			this.ctlChartScanVsNET.AxisAndLabelFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			this.ctlChartScanVsNET.AxisAndLabelMaxFontSize = 15;
			this.ctlChartScanVsNET.AxisAndLabelMinFontSize = 8;
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
			this.ctlChartScanVsNET.DefaultZoomHandler.FillColor = System.Drawing.Color.FromArgb(((System.Byte)(60)), ((System.Byte)(119)), ((System.Byte)(136)), ((System.Byte)(153)));
			this.ctlChartScanVsNET.DefaultZoomHandler.LineColor = System.Drawing.Color.Black;
			this.ctlChartScanVsNET.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctlChartScanVsNET.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			penProvider1.Color = System.Drawing.Color.FromArgb(((System.Byte)(211)), ((System.Byte)(211)), ((System.Byte)(211)));
			penProvider1.Width = 1F;
			this.ctlChartScanVsNET.GridLinePen = penProvider1;
			this.ctlChartScanVsNET.HilightColor = System.Drawing.Color.Magenta;
			this.ctlChartScanVsNET.Legend.BackColor = System.Drawing.Color.Transparent;
			penProvider2.Color = System.Drawing.Color.Black;
			penProvider2.Width = 1F;
			this.ctlChartScanVsNET.Legend.BorderPen = penProvider2;
			this.ctlChartScanVsNET.Legend.Bounds = new System.Drawing.Rectangle(732, 76, 173, 351);
			this.ctlChartScanVsNET.Legend.ColumnWidth = 125;
			this.ctlChartScanVsNET.Legend.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.ctlChartScanVsNET.Legend.MaxFontSize = 12F;
			this.ctlChartScanVsNET.Legend.MinFontSize = 6F;
			this.ctlChartScanVsNET.Location = new System.Drawing.Point(0, 0);
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
			this.ctlChartScanVsNET.Size = new System.Drawing.Size(914, 467);
			this.ctlChartScanVsNET.TabIndex = 6;
			this.ctlChartScanVsNET.Title = "Predicted NET vs Scan for peptides";
			this.ctlChartScanVsNET.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 27F);
			this.ctlChartScanVsNET.TitleMaxFontSize = 50F;
			this.ctlChartScanVsNET.TitleMinFontSize = 6F;
			this.ctlChartScanVsNET.VerticalExpansion = 1F;
			this.ctlChartScanVsNET.ViewPort = ((System.Drawing.RectangleF)(resources.GetObject("ctlChartScanVsNET.ViewPort")));
			this.ctlChartScanVsNET.XAxisLabel = "peptide scan";
			this.ctlChartScanVsNET.YAxisLabel = "Predicted NET (Krokhin et.al.)";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.textBoxMaxLogE);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 128);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(188, 24);
			this.panel2.TabIndex = 5;
			// 
			// textBoxMaxLogE
			// 
			this.textBoxMaxLogE.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxMaxLogE.Location = new System.Drawing.Point(104, 0);
			this.textBoxMaxLogE.Name = "textBoxMaxLogE";
			this.textBoxMaxLogE.Size = new System.Drawing.Size(84, 20);
			this.textBoxMaxLogE.TabIndex = 1;
			this.textBoxMaxLogE.Text = "-2";
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Left;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 24);
			this.label2.TabIndex = 0;
			this.label2.Text = "Max log(E-Value):";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1136, 793);
			this.Controls.Add(this.ctlChartScanVsNET);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.expandPanelOptions);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.expandPanelLists);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "MTDBCreator";
			((System.ComponentModel.ISupportInitialize)(this.expandPanelLists)).EndInit();
			this.expandPanelLists.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.expandPanelOptions)).EndInit();
			this.expandPanelOptions.ResumeLayout(false);
			this.groupBoxAlignmentType.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ctlChartScanVsNET)).EndInit();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		#region "Helpers"
		private void AddAnalysesTolist(clsAnalysisDescription [] analyses)
		{
			for (int i = 0 ; i < analyses.Length ; i++)
			{
				AddAnalysisToList(analyses[i]) ;
			}
		}
		private void AddAnalysisToList(clsAnalysisDescription analysis) 
		{
			ListViewItem listItem = listViewDatasets.Items.Add((analysis.mintDatasetId+1).ToString()) ; 
			listItem.Tag = analysis ; 
			listItem.SubItems.Add(analysis.mstrDataset) ; 
		}

		#endregion 

		#region "Loading functions"
		private void LoadDisplayXTandemDataset(clsAnalysisDescription analysis)
		{
			DateTime now = DateTime.Now ; 
			mfrmStatus.Text = analysis.mstrDataset ; 
			mfrmStatus.SetStatusMessage("Reading PHRP files") ; 
			mobjXTandemPHRPResultsReader = new clsXTandemAnalysisReader(analysis.mstrArchivePath, 
				analysis.mstrDataset, mfrmStatus) ; 
			DateTime read = DateTime.Now; 
			Regressor.RegressionType regressionType = Regressor.RegressionType.LINEAR_EM ; 
			if (radioButtonCubicSpline.Checked)
				regressionType = Regressor.RegressionType.CUBIC_SPLINE ; 
			else if (radioButtonHybrid.Checked)
				regressionType = Regressor.RegressionType.HYBRID ; 
			else if (radioButtonLinearEM.Checked)
				regressionType = Regressor.RegressionType.LINEAR_EM ; 
			else if (radioButtonWARP.Checked)
				regressionType = Regressor.RegressionType.CENTRAL ; 

			double rsquared = 0 ; 
			int numScans = 0 ; 
			if (!radioButtonAverageNET.Checked)
				mobjMTDB.AlignXTandemDatasetToTheoreticalNETs(mobjXTandemPHRPResultsReader, regressionType, ref marrPeptideScans, ref marrPeptidePredictedNETs, ref mdbl_slope, ref mdbl_intercept, ref numScans, ref rsquared) ; 
			else
				mobjMTDB.AlignXTandemDatasetToAverageNETs(mobjXTandemPHRPResultsReader, regressionType, ref marrPeptideScans, ref marrPeptidePredictedNETs, ref mdbl_slope, ref mdbl_intercept,Convert.ToInt16(textBoxMinObservations.Text)) ; 
			DisplayScansVsNet(analysis.mstrDataset) ; 
		}

		private void LoadDisplaySequestDataset(clsAnalysisDescription analysis)
		{
			DateTime now = DateTime.Now ; 
			mfrmStatus.Text = analysis.mstrDataset ; 
			mfrmStatus.SetStatusMessage("Reading PHRP files") ; 
			mobjXTandemPHRPResultsReader = new clsXTandemAnalysisReader(analysis.mstrArchivePath, 
				analysis.mstrDataset, mfrmStatus) ; 
			DateTime read = DateTime.Now; 
			Regressor.RegressionType regressionType = Regressor.RegressionType.LINEAR_EM ; 
			if (radioButtonCubicSpline.Checked)
				regressionType = Regressor.RegressionType.CUBIC_SPLINE ; 
			else if (radioButtonHybrid.Checked)
				regressionType = Regressor.RegressionType.HYBRID ; 
			else if (radioButtonLinearEM.Checked)
				regressionType = Regressor.RegressionType.LINEAR_EM ; 
			else if (radioButtonWARP.Checked)
				regressionType = Regressor.RegressionType.CENTRAL ; 

			double rsquared = 0 ; 
			int numScans = 0 ; 
			if (!radioButtonAverageNET.Checked)
				mobjMTDB.AlignXTandemDatasetToTheoreticalNETs(mobjXTandemPHRPResultsReader, regressionType, ref marrPeptideScans, ref marrPeptidePredictedNETs, ref mdbl_slope, ref mdbl_intercept, ref numScans, ref rsquared) ; 
			else
				mobjMTDB.AlignXTandemDatasetToAverageNETs(mobjXTandemPHRPResultsReader, regressionType, ref marrPeptideScans, ref marrPeptidePredictedNETs, ref mdbl_slope, ref mdbl_intercept,Convert.ToInt16(textBoxMinObservations.Text)) ; 
			DisplayScansVsNet(analysis.mstrDataset) ; 
		}

		#endregion

		#region "Processing"
		private void ProcessAllDatasets()
		{

			mobjMTDB.mdbl_max_log_eval = Convert.ToDouble(textBoxMaxLogE.Text) ; 
			mstrErrors = "" ; 
			for (int i = 0 ; i < listViewDatasets.Items.Count ; i++)
			{
				try
				{
					ProcessItem(i) ; 
				}
				catch (Exception ex)
				{
					ListViewItem item = listViewDatasets.Items[i] ; 
					clsAnalysisDescription analysis = (clsAnalysisDescription) item.Tag ; 
					mstrErrors += "Ignoring " + analysis.mstrDataset + " because of errors\n" ; 
				}
			}
			mobjMTDB.CalculateMassTagNETs() ; 
			mobjMTDB.CalculateProteinsPassingFilters() ; 
			mobjMTDB.LoadResultsIntoDB() ; 
			mfrmStatus.Hide() ; 
			GC.Collect();

		}

		private void ProcessCurrentDataset()
		{
			try
			{
				if (listViewDatasets.SelectedItems.Count == 0)
					return ; 
				int selectedIndex = listViewDatasets.SelectedIndices[0] ; 
				ProcessItem(selectedIndex) ; 
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message) ; 
			}
		}

		private void ProcessItem(int itemNum)
		{
			ListViewItem item = listViewDatasets.Items[itemNum] ; 
			try
			{
				clsAnalysisDescription analysis = (clsAnalysisDescription) item.Tag ; 
				if (analysis.mstrAnalysisTool == "XTandem")
				{
					DateTime now = DateTime.Now ; 
					mfrmStatus.Text = analysis.mstrDataset ; 
					mfrmStatus.SetStatusMessage("Reading PHRP files") ; 
					mobjXTandemPHRPResultsReader = new clsXTandemAnalysisReader(analysis.mstrArchivePath, 
						analysis.mstrDataset, mfrmStatus) ; 
					DateTime read = DateTime.Now; 
					Regressor.RegressionType regressionType = Regressor.RegressionType.LINEAR_EM ; 
					if (radioButtonCubicSpline.Checked)
						regressionType = Regressor.RegressionType.CUBIC_SPLINE ; 
					else if (radioButtonHybrid.Checked)
						regressionType = Regressor.RegressionType.HYBRID ; 
					else if (radioButtonLinearEM.Checked)
						regressionType = Regressor.RegressionType.LINEAR_EM ; 
					else if (radioButtonWARP.Checked)
						regressionType = Regressor.RegressionType.CENTRAL ; 

					double rsquared = 0 ; 
					int numScans = 0 ; 
					mfrmStatus.SetStatusMessage("Performing Alignment") ; 
					mobjMTDB.AlignXTandemDatasetToTheoreticalNETs(mobjXTandemPHRPResultsReader, regressionType, ref marrPeptideScans, ref marrPeptidePredictedNETs, ref mdbl_slope, ref mdbl_intercept, ref numScans, ref rsquared) ; 
					DisplayScansVsNet(analysis.mstrDataset) ; 
					analysis.mdbl_scan_net_intercept = mdbl_intercept ; 
					analysis.mdbl_scan_net_slope = mdbl_slope ;
					analysis.mint_num_scans = numScans ; 
					analysis.mdbl_scan_net_rsquared = rsquared ; 
					mobjMTDB.AddResults(mobjXTandemPHRPResultsReader, regressionType, analysis) ; 

					item.SubItems.Add(Convert.ToString(analysis.mdbl_scan_net_slope)) ; 
					item.SubItems.Add(Convert.ToString(analysis.mdbl_scan_net_intercept)) ; 
					item.SubItems.Add(Convert.ToString(analysis.mdbl_scan_net_rsquared)) ; 
					DateTime add = DateTime.Now ; 
					Console.WriteLine("# of items = " + mobjXTandemPHRPResultsReader.marrXTandemResults.Length +  " Read Time = " + Convert.ToString(read.Subtract(now)) + " Insert time = " + Convert.ToString(add.Subtract(read))) ; 

				}
			}
			catch (Exception ex)
			{
				throw ex ;
			}
		}

		#endregion 
		#region "Display functions"
		private void ShowSelectedDataset()
		{
			if (listViewDatasets.SelectedItems.Count == 0)
				return ; 
			int selectedIndex = listViewDatasets.SelectedIndices[0] ; 

			ListViewItem item = listViewDatasets.Items[selectedIndex] ; 
			try
			{
				clsAnalysisDescription analysis = (clsAnalysisDescription) item.Tag ; 
				switch(analysis.mstrAnalysisTool.ToUpper())
				{
					case "XTANDEM":
						LoadDisplayXTandemDataset(analysis) ; 
						break ; 
					case "SEQUEST":
						LoadDisplaySequestDataset(analysis) ; 
						break ; 
					default:
						break ; 
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace) ; 
			}
		}

		private void DisplayScansVsNet(string dataset)
		{
				
			mobjPlotParams.Name = dataset ; 

			if (!checkBoxShowResiduals.Checked)
			{
				mobjSeries = new clsSeries(new PNNL.Controls.ArrayChartDataProvider(marrPeptideScans, marrPeptidePredictedNETs), 
					mobjPlotParams) ; 
			}
			else
			{
				// remove predicted component for each part. 
				float [] residuals = new float[marrPeptideScans.Length] ; 
				for (int i = 0 ; i < marrPeptideScans.Length ; i++)
				{
					residuals[i] = marrPeptidePredictedNETs[i] - mobjMTDB.GetTransformedNET(marrPeptideScans[i]) ; 
				}
				mobjSeries = new clsSeries(new PNNL.Controls.ArrayChartDataProvider(marrPeptideScans, residuals), 
					mobjPlotParams) ; 

			}

			ctlChartScanVsNET.SeriesCollection.Clear() ; 
			ctlChartScanVsNET.SeriesCollection.Add(mobjSeries) ; 
			ctlChartScanVsNET.AutoViewPort() ; 
		}
		
		private void DrawNetRegressionLine(ctlChartBase sender, PostRenderEventArgs args) 
		{
			try
			{
				float xFrom = sender.ViewPort.X  ; 
				float xFinal = sender.ViewPort.Right  ; 
				Pen mpen_line = new Pen(Brushes.Black,3) ; 
				if (!checkBoxShowResiduals.Checked)
				{
					int numDivisions = 10 ; 


					float step = (xFinal - xFrom) / numDivisions ; 


					for ( ; xFrom < xFinal ; xFrom += step)
					{
						float yFrom = mobjMTDB.GetTransformedNET(xFrom) ;
						float xTo = xFrom + step ; 
						float yTo =  mobjMTDB.GetTransformedNET(xTo) ;
						args.Graphics.DrawLine(mpen_line, sender.GetScreenPixelX(xFrom), sender.GetScreenPixelY(yFrom),
							sender.GetScreenPixelX(xTo), sender.GetScreenPixelY(yTo)) ; 
					}
				}
				else
				{
					args.Graphics.DrawLine(mpen_line, sender.GetScreenPixelX(xFrom), sender.GetScreenPixelY(0),
						sender.GetScreenPixelX(xFinal), sender.GetScreenPixelY(0)) ; 
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + " " + ex.StackTrace) ; 
			}
		}

		#endregion

		# region "UI Responses"
		private void menuItemOpenList_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.listViewDatasets.Items.Clear() ;

				radioButtonPredictedNET.Checked = true ; 
				radioButtonAverageNET.Enabled = false ; 

				OpenFileDialog openFileDialog1 = new OpenFileDialog();

				openFileDialog1.InitialDirectory = "c:\\projects\\MTDBCreator\\data\\" ;
				openFileDialog1.Filter = "txt files (*.txt)|*.txt" ;
				openFileDialog1.FilterIndex = 1 ;
				openFileDialog1.RestoreDirectory = true ;

				if(openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					clsAnalysisDescriptionReader reader = new clsAnalysisDescriptionReader(openFileDialog1.FileName) ; 
					marrAnalyses = reader.Analyses ; 
					AddAnalysesTolist(marrAnalyses) ; 
				}
				menuItemCreateDB.Enabled = true ; 
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message) ; 
			}
		}

		private void menuItemCreateDB_Click(object sender, System.EventArgs e)
		{
			mobjMTDB = null ; 
			mobjMTDB = new clsMTDB(mfrmStatus) ; 

			SaveFileDialog saveFileDialog = new SaveFileDialog() ; 
			saveFileDialog.Title = "Select output database" ; 
			saveFileDialog.Filter =  "Microsoft Access Files (*.mdb)|*.mdb" ; 
			saveFileDialog.FilterIndex = 1 ; 
			saveFileDialog.RestoreDirectory = true ; 

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				mstrAccessDbPath = saveFileDialog.FileName ; 
			}
			else
			{
				MessageBox.Show("No database output selected. Defaulting to untitled.mdb") ; 
				mstrAccessDbPath = "untitled.mdb" ; 
			}
			mobjMTDB.AccessDBPath = mstrAccessDbPath ; 
			try
			{
				System.IO.File.Delete(mstrAccessDbPath) ; 
			}
			catch (Exception ex)
			{
			}

			radioButtonPredictedNET.Checked = true ; 
			radioButtonAverageNET.Enabled = false ; 

			System.Threading.ThreadStart start = new System.Threading.ThreadStart(this.ProcessAllDatasets) ; 
			System.Threading.Thread thrd = new System.Threading.Thread(start) ; 
			thrd.Start() ; 

			mfrmStatus.ShowStatusBox(this, "Creating Mass Tag Database") ; 
			if (thrd.IsAlive)
			{
				thrd.Abort() ; 
			}
			if (mstrErrors != "")
			{
				MessageBox.Show(this, mstrErrors, "Errors in processing") ; 
			}
			radioButtonAverageNET.Enabled = true ; 
		}

		private void menuItemOpen_Click(object sender, System.EventArgs e)
		{
			this.ShowSelectedDataset() ;
			//			System.Threading.ThreadStart start = new System.Threading.ThreadStart(this.ShowSelectedDataset) ; 
			//			System.Threading.Thread thrd = new System.Threading.Thread(start) ; 
			//			thrd.Start() ; 
		}

		private void listViewDatasets_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			menuItemOpen_Click(sender, e) ; 
		}

		private void checkBoxShowResiduals_CheckedChanged(object sender, System.EventArgs e)
		{
			if (listViewDatasets.SelectedItems.Count == 0)
				return ; 
			int selectedIndex = listViewDatasets.SelectedIndices[0] ; 
			clsAnalysisDescription analysis = (clsAnalysisDescription) listViewDatasets.Items[selectedIndex].Tag ; 
			DisplayScansVsNet(analysis.mstrDataset) ; 
		}

		private void radioButtonAverageNET_CheckedChanged(object sender, System.EventArgs e)
		{
			textBoxMinObservations.Enabled = radioButtonAverageNET.Checked ; 
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			frmAbout aboutForm = new frmAbout() ;
			aboutForm.ShowDialog() ; 
		}
		#endregion 
	}
}
