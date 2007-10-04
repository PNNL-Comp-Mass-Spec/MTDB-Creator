using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for frmOptions.
	/// </summary>
	public class frmOptions : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panelOKCancel;
		private System.Windows.Forms.Button mbuttonOK;
		private System.Windows.Forms.Button mbuttonCancel;
		private System.Windows.Forms.Panel mpanelMain;
		private System.Windows.Forms.StatusBar mstatusBar;
		private System.Windows.Forms.TabControl mtabControlOptions;
		private System.Windows.Forms.TabPage mtabPageAlignmentOptions;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panelMaxLogEValForAlignment;
		private System.Windows.Forms.Label labelMinObservations;
		private System.Windows.Forms.Panel panelMaxRankForExport;
		private System.Windows.Forms.Label labelMaxRank;
		private System.Windows.Forms.Panel panelModification;
		private System.Windows.Forms.TextBox mtextBoxModifications;
		private System.Windows.Forms.Label labelModifications;
		private System.Windows.Forms.Panel panelMinObservations;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panelAlignmentOptions;
		private System.Windows.Forms.GroupBox groupBoxAlignmentType;
		private System.Windows.Forms.Panel panelOrderRegression;
		private System.Windows.Forms.TextBox mtextBoxRegressionOrder;
		private System.Windows.Forms.GroupBox groupBoxPredictionAlgo;
		private System.Windows.Forms.TabPage mtabPageExportOptions;
		private System.Windows.Forms.GroupBox groupBoxSequest;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox mgroupBoxDelCN;
		private System.Windows.Forms.Label mlabelDelCN;
		private System.Windows.Forms.CheckBox mcheckBoxDelCN;
		private System.Windows.Forms.GroupBox mgroupBoxXCorrThresholds;
		private System.Windows.Forms.Panel panelTrypticCS3;
		private System.Windows.Forms.Label labelXCorrForExportCS3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label labelXCorrForExportCS2;
		private System.Windows.Forms.Panel mpanelXCorrExport;
		private System.Windows.Forms.Label labelXCorrForExportCS1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Panel panelSequest;
		private System.Windows.Forms.Label labelMinXCorr;
		private System.Windows.Forms.GroupBox groupBoxXTandem;
		private System.Windows.Forms.Panel panelXTandemForExport;
		private System.Windows.Forms.TextBox mtextBoxMaxLogEvalForExport;
		private System.Windows.Forms.Label labelMaxLogEValForExport;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForExportPartiallyTrypticsCS3;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForExportPartiallyTrypticsCS2;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForExportPartiallyTrypticsCS1;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForExportNonTrypticsCS3;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForExportNonTrypticsCS2;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForExportNonTrypticsCS1;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForAlignment;
		private System.Windows.Forms.TextBox mtextBoxDelCN;
		private System.Windows.Forms.CheckBox mcheckBoxNonTryptic;
		private System.Windows.Forms.CheckBox mcheckBoxPartially;
		private System.Windows.Forms.CheckBox mcheckBoxTryptic;
		private System.Windows.Forms.TextBox mtextBoxMaxLogEValForAlignment;
		private System.Windows.Forms.TextBox mtextBoxMaxRank;
		private System.Windows.Forms.TextBox mtextBoxMinObservations;
		private System.Windows.Forms.RadioButton mradioButtonKrokhin;
		private System.Windows.Forms.RadioButton mradioButtonKangas;
		private System.Windows.Forms.RadioButton mradioButtonMixtureRegression;
		private System.Windows.Forms.RadioButton mradioButtonLinearEM;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForExportTrypticsCS3;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForExportTrypticsCS2;
		private System.Windows.Forms.TextBox mtextBoxMinXCorrForExportTrypticsCS1;
		private System.Windows.Forms.Label labelXCorrForExportPartiallyTrytpicsCS3;
		private System.Windows.Forms.Label labelXCorrForExportPartiallyTrytpicsCS2;
		private System.Windows.Forms.Label labelXCorrForExportPartiallyTrytpicsCS1;
		private System.Windows.Forms.Label labelXCorrForExportNonTrytpicsCS3;
		private System.Windows.Forms.Label labelXCorrForExportNonTrytpicsCS2;
		private System.Windows.Forms.Label labelXCorrForExportNonTrytpicsCS1;
		private System.Windows.Forms.Label mlabelRegressionOrder;
		private System.Windows.Forms.GroupBox mgroupBoxXCorrThresholdsNonTryptic;
		private System.Windows.Forms.GroupBox mgroupBoxXCorrThresholdsPartiallyTryptic;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmOptions()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			Init() ; 
		}

		public void Init()
		{
			mtextBoxMinXCorrForExportTrypticsCS1.DataBindings.Add("Enabled", mcheckBoxTryptic, "Checked") ; 
			labelXCorrForExportCS1.DataBindings.Add("Enabled", mcheckBoxTryptic, "Checked") ; 
			mtextBoxMinXCorrForExportTrypticsCS2.DataBindings.Add("Enabled", mcheckBoxTryptic, "Checked") ; 
			labelXCorrForExportCS2.DataBindings.Add("Enabled", mcheckBoxTryptic, "Checked") ; 
			mtextBoxMinXCorrForExportTrypticsCS3.DataBindings.Add("Enabled", mcheckBoxTryptic, "Checked") ; 
			labelXCorrForExportCS3.DataBindings.Add("Enabled", mcheckBoxTryptic, "Checked") ; 

			mtextBoxMinXCorrForExportPartiallyTrypticsCS1.DataBindings.Add("Enabled", mcheckBoxPartially, "Checked") ; 
			labelXCorrForExportPartiallyTrytpicsCS1.DataBindings.Add("Enabled", mcheckBoxPartially, "Checked") ; 
			mtextBoxMinXCorrForExportPartiallyTrypticsCS2.DataBindings.Add("Enabled", mcheckBoxPartially, "Checked") ; 
			labelXCorrForExportPartiallyTrytpicsCS2.DataBindings.Add("Enabled", mcheckBoxPartially, "Checked") ; 
			mtextBoxMinXCorrForExportPartiallyTrypticsCS3.DataBindings.Add("Enabled", mcheckBoxPartially, "Checked") ; 
			labelXCorrForExportPartiallyTrytpicsCS3.DataBindings.Add("Enabled", mcheckBoxPartially, "Checked") ; 

			mtextBoxMinXCorrForExportNonTrypticsCS1.DataBindings.Add("Enabled", mcheckBoxNonTryptic, "Checked") ; 
			labelXCorrForExportNonTrytpicsCS1.DataBindings.Add("Enabled", mcheckBoxNonTryptic, "Checked") ; 
			mtextBoxMinXCorrForExportNonTrypticsCS2.DataBindings.Add("Enabled", mcheckBoxNonTryptic, "Checked") ; 
			labelXCorrForExportNonTrytpicsCS2.DataBindings.Add("Enabled", mcheckBoxNonTryptic, "Checked") ; 
			mtextBoxMinXCorrForExportNonTrypticsCS3.DataBindings.Add("Enabled", mcheckBoxNonTryptic, "Checked") ; 
			labelXCorrForExportNonTrytpicsCS3.DataBindings.Add("Enabled", mcheckBoxNonTryptic, "Checked") ; 

			mlabelDelCN.DataBindings.Add("Enabled", mcheckBoxDelCN, "Checked") ; 
			mtextBoxDelCN.DataBindings.Add("Enabled", mcheckBoxDelCN, "Checked") ; 

			mlabelRegressionOrder.DataBindings.Add("Enabled", mradioButtonMixtureRegression, "Checked") ; 
			mtextBoxRegressionOrder.DataBindings.Add("Enabled", mradioButtonMixtureRegression, "Checked") ; 

#if BASIC
			mradioButtonKangas.Checked = false ; 
			mradioButtonKrokhin.Checked = true ; 
			mradioButtonKangas.Enabled = false ; 
#else 
			mradioButtonKangas.Enabled = true ; 
#endif 
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
			this.panelOKCancel = new System.Windows.Forms.Panel();
			this.mstatusBar = new System.Windows.Forms.StatusBar();
			this.mbuttonCancel = new System.Windows.Forms.Button();
			this.mbuttonOK = new System.Windows.Forms.Button();
			this.mpanelMain = new System.Windows.Forms.Panel();
			this.mtabControlOptions = new System.Windows.Forms.TabControl();
			this.mtabPageAlignmentOptions = new System.Windows.Forms.TabPage();
			this.panelAlignmentOptions = new System.Windows.Forms.Panel();
			this.groupBoxAlignmentType = new System.Windows.Forms.GroupBox();
			this.panelOrderRegression = new System.Windows.Forms.Panel();
			this.mtextBoxRegressionOrder = new System.Windows.Forms.TextBox();
			this.mlabelRegressionOrder = new System.Windows.Forms.Label();
			this.mradioButtonMixtureRegression = new System.Windows.Forms.RadioButton();
			this.mradioButtonLinearEM = new System.Windows.Forms.RadioButton();
			this.groupBoxPredictionAlgo = new System.Windows.Forms.GroupBox();
			this.mradioButtonKrokhin = new System.Windows.Forms.RadioButton();
			this.mradioButtonKangas = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panelSequest = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForAlignment = new System.Windows.Forms.TextBox();
			this.labelMinXCorr = new System.Windows.Forms.Label();
			this.panelMaxLogEValForAlignment = new System.Windows.Forms.Panel();
			this.mtextBoxMinObservations = new System.Windows.Forms.TextBox();
			this.labelMinObservations = new System.Windows.Forms.Label();
			this.panelMaxRankForExport = new System.Windows.Forms.Panel();
			this.mtextBoxMaxRank = new System.Windows.Forms.TextBox();
			this.labelMaxRank = new System.Windows.Forms.Label();
			this.panelModification = new System.Windows.Forms.Panel();
			this.mtextBoxModifications = new System.Windows.Forms.TextBox();
			this.labelModifications = new System.Windows.Forms.Label();
			this.panelMinObservations = new System.Windows.Forms.Panel();
			this.mtextBoxMaxLogEValForAlignment = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.mtabPageExportOptions = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.groupBoxXTandem = new System.Windows.Forms.GroupBox();
			this.panelXTandemForExport = new System.Windows.Forms.Panel();
			this.mtextBoxMaxLogEvalForExport = new System.Windows.Forms.TextBox();
			this.labelMaxLogEValForExport = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.mcheckBoxNonTryptic = new System.Windows.Forms.CheckBox();
			this.mcheckBoxPartially = new System.Windows.Forms.CheckBox();
			this.mcheckBoxTryptic = new System.Windows.Forms.CheckBox();
			this.groupBoxSequest = new System.Windows.Forms.GroupBox();
			this.mgroupBoxXCorrThresholdsNonTryptic = new System.Windows.Forms.GroupBox();
			this.panel5 = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForExportNonTrypticsCS3 = new System.Windows.Forms.TextBox();
			this.labelXCorrForExportNonTrytpicsCS3 = new System.Windows.Forms.Label();
			this.panel6 = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForExportNonTrypticsCS2 = new System.Windows.Forms.TextBox();
			this.labelXCorrForExportNonTrytpicsCS2 = new System.Windows.Forms.Label();
			this.panel7 = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForExportNonTrypticsCS1 = new System.Windows.Forms.TextBox();
			this.labelXCorrForExportNonTrytpicsCS1 = new System.Windows.Forms.Label();
			this.mgroupBoxXCorrThresholdsPartiallyTryptic = new System.Windows.Forms.GroupBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3 = new System.Windows.Forms.TextBox();
			this.labelXCorrForExportPartiallyTrytpicsCS3 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2 = new System.Windows.Forms.TextBox();
			this.labelXCorrForExportPartiallyTrytpicsCS2 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1 = new System.Windows.Forms.TextBox();
			this.labelXCorrForExportPartiallyTrytpicsCS1 = new System.Windows.Forms.Label();
			this.mgroupBoxXCorrThresholds = new System.Windows.Forms.GroupBox();
			this.panelTrypticCS3 = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForExportTrypticsCS3 = new System.Windows.Forms.TextBox();
			this.labelXCorrForExportCS3 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForExportTrypticsCS2 = new System.Windows.Forms.TextBox();
			this.labelXCorrForExportCS2 = new System.Windows.Forms.Label();
			this.mpanelXCorrExport = new System.Windows.Forms.Panel();
			this.mtextBoxMinXCorrForExportTrypticsCS1 = new System.Windows.Forms.TextBox();
			this.labelXCorrForExportCS1 = new System.Windows.Forms.Label();
			this.mgroupBoxDelCN = new System.Windows.Forms.GroupBox();
			this.mtextBoxDelCN = new System.Windows.Forms.TextBox();
			this.mlabelDelCN = new System.Windows.Forms.Label();
			this.mcheckBoxDelCN = new System.Windows.Forms.CheckBox();
			this.panelOKCancel.SuspendLayout();
			this.mpanelMain.SuspendLayout();
			this.mtabControlOptions.SuspendLayout();
			this.mtabPageAlignmentOptions.SuspendLayout();
			this.panelAlignmentOptions.SuspendLayout();
			this.groupBoxAlignmentType.SuspendLayout();
			this.panelOrderRegression.SuspendLayout();
			this.groupBoxPredictionAlgo.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panelSequest.SuspendLayout();
			this.panelMaxLogEValForAlignment.SuspendLayout();
			this.panelMaxRankForExport.SuspendLayout();
			this.panelModification.SuspendLayout();
			this.panelMinObservations.SuspendLayout();
			this.mtabPageExportOptions.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBoxXTandem.SuspendLayout();
			this.panelXTandemForExport.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBoxSequest.SuspendLayout();
			this.mgroupBoxXCorrThresholdsNonTryptic.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel7.SuspendLayout();
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.mgroupBoxXCorrThresholds.SuspendLayout();
			this.panelTrypticCS3.SuspendLayout();
			this.panel1.SuspendLayout();
			this.mpanelXCorrExport.SuspendLayout();
			this.mgroupBoxDelCN.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelOKCancel
			// 
			this.panelOKCancel.Controls.Add(this.mstatusBar);
			this.panelOKCancel.Controls.Add(this.mbuttonCancel);
			this.panelOKCancel.Controls.Add(this.mbuttonOK);
			this.panelOKCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelOKCancel.Location = new System.Drawing.Point(0, 207);
			this.panelOKCancel.Name = "panelOKCancel";
			this.panelOKCancel.Size = new System.Drawing.Size(562, 64);
			this.panelOKCancel.TabIndex = 0;
			// 
			// mstatusBar
			// 
			this.mstatusBar.Location = new System.Drawing.Point(0, 48);
			this.mstatusBar.Name = "mstatusBar";
			this.mstatusBar.Size = new System.Drawing.Size(562, 16);
			this.mstatusBar.SizingGrip = false;
			this.mstatusBar.TabIndex = 2;
			this.mstatusBar.Text = "Set Options";
			// 
			// mbuttonCancel
			// 
			this.mbuttonCancel.Location = new System.Drawing.Point(312, 8);
			this.mbuttonCancel.Name = "mbuttonCancel";
			this.mbuttonCancel.Size = new System.Drawing.Size(86, 27);
			this.mbuttonCancel.TabIndex = 1;
			this.mbuttonCancel.Text = "Cancel";
			this.mbuttonCancel.Click += new System.EventHandler(this.mbuttonCancel_Click);
			// 
			// mbuttonOK
			// 
			this.mbuttonOK.Location = new System.Drawing.Point(176, 9);
			this.mbuttonOK.Name = "mbuttonOK";
			this.mbuttonOK.Size = new System.Drawing.Size(86, 27);
			this.mbuttonOK.TabIndex = 0;
			this.mbuttonOK.Text = "OK";
			this.mbuttonOK.Click += new System.EventHandler(this.mbuttonOK_Click);
			// 
			// mpanelMain
			// 
			this.mpanelMain.Controls.Add(this.mtabControlOptions);
			this.mpanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mpanelMain.Location = new System.Drawing.Point(0, 0);
			this.mpanelMain.Name = "mpanelMain";
			this.mpanelMain.Size = new System.Drawing.Size(562, 207);
			this.mpanelMain.TabIndex = 4;
			// 
			// mtabControlOptions
			// 
			this.mtabControlOptions.Controls.Add(this.mtabPageAlignmentOptions);
			this.mtabControlOptions.Controls.Add(this.mtabPageExportOptions);
			this.mtabControlOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtabControlOptions.Location = new System.Drawing.Point(0, 0);
			this.mtabControlOptions.Name = "mtabControlOptions";
			this.mtabControlOptions.SelectedIndex = 0;
			this.mtabControlOptions.Size = new System.Drawing.Size(562, 207);
			this.mtabControlOptions.TabIndex = 8;
			// 
			// mtabPageAlignmentOptions
			// 
			this.mtabPageAlignmentOptions.Controls.Add(this.panelAlignmentOptions);
			this.mtabPageAlignmentOptions.Controls.Add(this.groupBox1);
			this.mtabPageAlignmentOptions.Location = new System.Drawing.Point(4, 22);
			this.mtabPageAlignmentOptions.Name = "mtabPageAlignmentOptions";
			this.mtabPageAlignmentOptions.Size = new System.Drawing.Size(554, 181);
			this.mtabPageAlignmentOptions.TabIndex = 0;
			this.mtabPageAlignmentOptions.Text = "Alignment Options";
			// 
			// panelAlignmentOptions
			// 
			this.panelAlignmentOptions.Controls.Add(this.groupBoxAlignmentType);
			this.panelAlignmentOptions.Controls.Add(this.groupBoxPredictionAlgo);
			this.panelAlignmentOptions.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelAlignmentOptions.Location = new System.Drawing.Point(224, 0);
			this.panelAlignmentOptions.Name = "panelAlignmentOptions";
			this.panelAlignmentOptions.Size = new System.Drawing.Size(144, 181);
			this.panelAlignmentOptions.TabIndex = 8;
			// 
			// groupBoxAlignmentType
			// 
			this.groupBoxAlignmentType.Controls.Add(this.panelOrderRegression);
			this.groupBoxAlignmentType.Controls.Add(this.mradioButtonMixtureRegression);
			this.groupBoxAlignmentType.Controls.Add(this.mradioButtonLinearEM);
			this.groupBoxAlignmentType.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBoxAlignmentType.Location = new System.Drawing.Point(0, 88);
			this.groupBoxAlignmentType.Name = "groupBoxAlignmentType";
			this.groupBoxAlignmentType.Size = new System.Drawing.Size(144, 80);
			this.groupBoxAlignmentType.TabIndex = 7;
			this.groupBoxAlignmentType.TabStop = false;
			this.groupBoxAlignmentType.Text = "Alignment Type";
			// 
			// panelOrderRegression
			// 
			this.panelOrderRegression.Controls.Add(this.mtextBoxRegressionOrder);
			this.panelOrderRegression.Controls.Add(this.mlabelRegressionOrder);
			this.panelOrderRegression.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelOrderRegression.Location = new System.Drawing.Point(3, 48);
			this.panelOrderRegression.Name = "panelOrderRegression";
			this.panelOrderRegression.Size = new System.Drawing.Size(138, 24);
			this.panelOrderRegression.TabIndex = 3;
			// 
			// mtextBoxRegressionOrder
			// 
			this.mtextBoxRegressionOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxRegressionOrder.Location = new System.Drawing.Point(80, 0);
			this.mtextBoxRegressionOrder.Name = "mtextBoxRegressionOrder";
			this.mtextBoxRegressionOrder.Size = new System.Drawing.Size(58, 20);
			this.mtextBoxRegressionOrder.TabIndex = 2;
			this.mtextBoxRegressionOrder.Text = "1";
			this.mtextBoxRegressionOrder.MouseHover += new System.EventHandler(this.mtextBoxRegressionOrder_MouseHover);
			// 
			// mlabelRegressionOrder
			// 
			this.mlabelRegressionOrder.Dock = System.Windows.Forms.DockStyle.Left;
			this.mlabelRegressionOrder.Location = new System.Drawing.Point(0, 0);
			this.mlabelRegressionOrder.Name = "mlabelRegressionOrder";
			this.mlabelRegressionOrder.Size = new System.Drawing.Size(80, 24);
			this.mlabelRegressionOrder.TabIndex = 0;
			this.mlabelRegressionOrder.Text = "Order:";
			this.mlabelRegressionOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mradioButtonMixtureRegression
			// 
			this.mradioButtonMixtureRegression.Checked = true;
			this.mradioButtonMixtureRegression.Dock = System.Windows.Forms.DockStyle.Top;
			this.mradioButtonMixtureRegression.Location = new System.Drawing.Point(3, 32);
			this.mradioButtonMixtureRegression.Name = "mradioButtonMixtureRegression";
			this.mradioButtonMixtureRegression.Size = new System.Drawing.Size(138, 16);
			this.mradioButtonMixtureRegression.TabIndex = 2;
			this.mradioButtonMixtureRegression.TabStop = true;
			this.mradioButtonMixtureRegression.Text = "Mixture Regression";
			this.mradioButtonMixtureRegression.MouseHover += new System.EventHandler(this.mradioButtonMixtureRegression_MouseHover);
			// 
			// mradioButtonLinearEM
			// 
			this.mradioButtonLinearEM.Dock = System.Windows.Forms.DockStyle.Top;
			this.mradioButtonLinearEM.Location = new System.Drawing.Point(3, 16);
			this.mradioButtonLinearEM.Name = "mradioButtonLinearEM";
			this.mradioButtonLinearEM.Size = new System.Drawing.Size(138, 16);
			this.mradioButtonLinearEM.TabIndex = 1;
			this.mradioButtonLinearEM.Text = "Linear ";
			this.mradioButtonLinearEM.MouseHover += new System.EventHandler(this.mradioButtonLinearEM_MouseHover);
			// 
			// groupBoxPredictionAlgo
			// 
			this.groupBoxPredictionAlgo.Controls.Add(this.mradioButtonKrokhin);
			this.groupBoxPredictionAlgo.Controls.Add(this.mradioButtonKangas);
			this.groupBoxPredictionAlgo.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBoxPredictionAlgo.Location = new System.Drawing.Point(0, 0);
			this.groupBoxPredictionAlgo.Name = "groupBoxPredictionAlgo";
			this.groupBoxPredictionAlgo.Size = new System.Drawing.Size(144, 88);
			this.groupBoxPredictionAlgo.TabIndex = 8;
			this.groupBoxPredictionAlgo.TabStop = false;
			this.groupBoxPredictionAlgo.Text = "Prediction Algorithm";
			// 
			// mradioButtonKrokhin
			// 
			this.mradioButtonKrokhin.Dock = System.Windows.Forms.DockStyle.Top;
			this.mradioButtonKrokhin.Location = new System.Drawing.Point(3, 32);
			this.mradioButtonKrokhin.Name = "mradioButtonKrokhin";
			this.mradioButtonKrokhin.Size = new System.Drawing.Size(138, 16);
			this.mradioButtonKrokhin.TabIndex = 2;
			this.mradioButtonKrokhin.Text = "Krokhin";
			this.mradioButtonKrokhin.MouseHover += new System.EventHandler(this.mradioButtonKrokhin_MouseHover);
			// 
			// mradioButtonKangas
			// 
			this.mradioButtonKangas.Checked = true;
			this.mradioButtonKangas.Dock = System.Windows.Forms.DockStyle.Top;
			this.mradioButtonKangas.Location = new System.Drawing.Point(3, 16);
			this.mradioButtonKangas.Name = "mradioButtonKangas";
			this.mradioButtonKangas.Size = new System.Drawing.Size(138, 16);
			this.mradioButtonKangas.TabIndex = 1;
			this.mradioButtonKangas.TabStop = true;
			this.mradioButtonKangas.Text = "Kangas ANN";
			this.mradioButtonKangas.MouseHover += new System.EventHandler(this.mradioButtonKangas_MouseHover);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.panelSequest);
			this.groupBox1.Controls.Add(this.panelMaxLogEValForAlignment);
			this.groupBox1.Controls.Add(this.panelMaxRankForExport);
			this.groupBox1.Controls.Add(this.panelModification);
			this.groupBox1.Controls.Add(this.panelMinObservations);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(224, 181);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "NET Values";
			// 
			// panelSequest
			// 
			this.panelSequest.Controls.Add(this.mtextBoxMinXCorrForAlignment);
			this.panelSequest.Controls.Add(this.labelMinXCorr);
			this.panelSequest.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelSequest.Location = new System.Drawing.Point(3, 112);
			this.panelSequest.Name = "panelSequest";
			this.panelSequest.Size = new System.Drawing.Size(218, 24);
			this.panelSequest.TabIndex = 11;
			// 
			// mtextBoxMinXCorrForAlignment
			// 
			this.mtextBoxMinXCorrForAlignment.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForAlignment.Location = new System.Drawing.Point(152, 0);
			this.mtextBoxMinXCorrForAlignment.Name = "mtextBoxMinXCorrForAlignment";
			this.mtextBoxMinXCorrForAlignment.Size = new System.Drawing.Size(66, 20);
			this.mtextBoxMinXCorrForAlignment.TabIndex = 1;
			this.mtextBoxMinXCorrForAlignment.Text = "3";
			this.mtextBoxMinXCorrForAlignment.MouseHover += new System.EventHandler(this.mtextBoxMinXCorrForAlignment_MouseHover);
			// 
			// labelMinXCorr
			// 
			this.labelMinXCorr.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelMinXCorr.Location = new System.Drawing.Point(0, 0);
			this.labelMinXCorr.Name = "labelMinXCorr";
			this.labelMinXCorr.Size = new System.Drawing.Size(152, 24);
			this.labelMinXCorr.TabIndex = 0;
			this.labelMinXCorr.Text = "Min XCorr for alignment";
			this.labelMinXCorr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelMaxLogEValForAlignment
			// 
			this.panelMaxLogEValForAlignment.Controls.Add(this.mtextBoxMinObservations);
			this.panelMaxLogEValForAlignment.Controls.Add(this.labelMinObservations);
			this.panelMaxLogEValForAlignment.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelMaxLogEValForAlignment.Location = new System.Drawing.Point(3, 88);
			this.panelMaxLogEValForAlignment.Name = "panelMaxLogEValForAlignment";
			this.panelMaxLogEValForAlignment.Size = new System.Drawing.Size(218, 24);
			this.panelMaxLogEValForAlignment.TabIndex = 4;
			// 
			// mtextBoxMinObservations
			// 
			this.mtextBoxMinObservations.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinObservations.Location = new System.Drawing.Point(152, 0);
			this.mtextBoxMinObservations.Name = "mtextBoxMinObservations";
			this.mtextBoxMinObservations.Size = new System.Drawing.Size(66, 20);
			this.mtextBoxMinObservations.TabIndex = 1;
			this.mtextBoxMinObservations.Text = "3";
			this.mtextBoxMinObservations.MouseHover += new System.EventHandler(this.mtextBoxMinObservations_MouseHover);
			// 
			// labelMinObservations
			// 
			this.labelMinObservations.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelMinObservations.Location = new System.Drawing.Point(0, 0);
			this.labelMinObservations.Name = "labelMinObservations";
			this.labelMinObservations.Size = new System.Drawing.Size(152, 24);
			this.labelMinObservations.TabIndex = 0;
			this.labelMinObservations.Text = "Min Observations:";
			this.labelMinObservations.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelMaxRankForExport
			// 
			this.panelMaxRankForExport.Controls.Add(this.mtextBoxMaxRank);
			this.panelMaxRankForExport.Controls.Add(this.labelMaxRank);
			this.panelMaxRankForExport.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelMaxRankForExport.Location = new System.Drawing.Point(3, 64);
			this.panelMaxRankForExport.Name = "panelMaxRankForExport";
			this.panelMaxRankForExport.Size = new System.Drawing.Size(218, 24);
			this.panelMaxRankForExport.TabIndex = 10;
			// 
			// mtextBoxMaxRank
			// 
			this.mtextBoxMaxRank.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMaxRank.Location = new System.Drawing.Point(152, 0);
			this.mtextBoxMaxRank.Name = "mtextBoxMaxRank";
			this.mtextBoxMaxRank.Size = new System.Drawing.Size(66, 20);
			this.mtextBoxMaxRank.TabIndex = 1;
			this.mtextBoxMaxRank.Text = "3";
			this.mtextBoxMaxRank.MouseHover += new System.EventHandler(this.mtextBoxMaxRank_MouseHover);
			// 
			// labelMaxRank
			// 
			this.labelMaxRank.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelMaxRank.Location = new System.Drawing.Point(0, 0);
			this.labelMaxRank.Name = "labelMaxRank";
			this.labelMaxRank.Size = new System.Drawing.Size(152, 24);
			this.labelMaxRank.TabIndex = 0;
			this.labelMaxRank.Text = "Maximum Rank for Export:";
			this.labelMaxRank.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelModification
			// 
			this.panelModification.Controls.Add(this.mtextBoxModifications);
			this.panelModification.Controls.Add(this.labelModifications);
			this.panelModification.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelModification.Location = new System.Drawing.Point(3, 40);
			this.panelModification.Name = "panelModification";
			this.panelModification.Size = new System.Drawing.Size(218, 24);
			this.panelModification.TabIndex = 7;
			// 
			// mtextBoxModifications
			// 
			this.mtextBoxModifications.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxModifications.Location = new System.Drawing.Point(152, 0);
			this.mtextBoxModifications.Name = "mtextBoxModifications";
			this.mtextBoxModifications.Size = new System.Drawing.Size(66, 20);
			this.mtextBoxModifications.TabIndex = 1;
			this.mtextBoxModifications.Text = "2";
			this.mtextBoxModifications.MouseHover += new System.EventHandler(this.mtextBoxModifications_MouseHover);
			// 
			// labelModifications
			// 
			this.labelModifications.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelModifications.Location = new System.Drawing.Point(0, 0);
			this.labelModifications.Name = "labelModifications";
			this.labelModifications.Size = new System.Drawing.Size(152, 24);
			this.labelModifications.TabIndex = 0;
			this.labelModifications.Text = "Max # mods for alignment:";
			this.labelModifications.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelMinObservations
			// 
			this.panelMinObservations.Controls.Add(this.mtextBoxMaxLogEValForAlignment);
			this.panelMinObservations.Controls.Add(this.label2);
			this.panelMinObservations.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelMinObservations.Location = new System.Drawing.Point(3, 16);
			this.panelMinObservations.Name = "panelMinObservations";
			this.panelMinObservations.Size = new System.Drawing.Size(218, 24);
			this.panelMinObservations.TabIndex = 5;
			// 
			// mtextBoxMaxLogEValForAlignment
			// 
			this.mtextBoxMaxLogEValForAlignment.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMaxLogEValForAlignment.Location = new System.Drawing.Point(152, 0);
			this.mtextBoxMaxLogEValForAlignment.Name = "mtextBoxMaxLogEValForAlignment";
			this.mtextBoxMaxLogEValForAlignment.Size = new System.Drawing.Size(66, 20);
			this.mtextBoxMaxLogEValForAlignment.TabIndex = 1;
			this.mtextBoxMaxLogEValForAlignment.Text = "-2";
			this.mtextBoxMaxLogEValForAlignment.MouseHover += new System.EventHandler(this.mtextBoxMaxLogEValForAlignment_MouseHover);
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Left;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 24);
			this.label2.TabIndex = 0;
			this.label2.Text = "Max log(EVal) for alignment:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mtabPageExportOptions
			// 
			this.mtabPageExportOptions.Controls.Add(this.groupBox5);
			this.mtabPageExportOptions.Controls.Add(this.groupBoxSequest);
			this.mtabPageExportOptions.Location = new System.Drawing.Point(4, 22);
			this.mtabPageExportOptions.Name = "mtabPageExportOptions";
			this.mtabPageExportOptions.Size = new System.Drawing.Size(554, 181);
			this.mtabPageExportOptions.TabIndex = 1;
			this.mtabPageExportOptions.Text = "Export Options";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.groupBoxXTandem);
			this.groupBox5.Controls.Add(this.groupBox2);
			this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox5.Location = new System.Drawing.Point(0, 0);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(234, 181);
			this.groupBox5.TabIndex = 7;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "groupBoxRules";
			// 
			// groupBoxXTandem
			// 
			this.groupBoxXTandem.Controls.Add(this.panelXTandemForExport);
			this.groupBoxXTandem.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBoxXTandem.Location = new System.Drawing.Point(3, 104);
			this.groupBoxXTandem.Name = "groupBoxXTandem";
			this.groupBoxXTandem.Size = new System.Drawing.Size(228, 72);
			this.groupBoxXTandem.TabIndex = 14;
			this.groupBoxXTandem.TabStop = false;
			this.groupBoxXTandem.Text = "X!Tandem";
			// 
			// panelXTandemForExport
			// 
			this.panelXTandemForExport.Controls.Add(this.mtextBoxMaxLogEvalForExport);
			this.panelXTandemForExport.Controls.Add(this.labelMaxLogEValForExport);
			this.panelXTandemForExport.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelXTandemForExport.Location = new System.Drawing.Point(3, 16);
			this.panelXTandemForExport.Name = "panelXTandemForExport";
			this.panelXTandemForExport.Size = new System.Drawing.Size(222, 24);
			this.panelXTandemForExport.TabIndex = 10;
			// 
			// mtextBoxMaxLogEvalForExport
			// 
			this.mtextBoxMaxLogEvalForExport.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMaxLogEvalForExport.Location = new System.Drawing.Point(136, 0);
			this.mtextBoxMaxLogEvalForExport.Name = "mtextBoxMaxLogEvalForExport";
			this.mtextBoxMaxLogEvalForExport.Size = new System.Drawing.Size(86, 20);
			this.mtextBoxMaxLogEvalForExport.TabIndex = 1;
			this.mtextBoxMaxLogEvalForExport.Text = "-2";
			// 
			// labelMaxLogEValForExport
			// 
			this.labelMaxLogEValForExport.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelMaxLogEValForExport.Location = new System.Drawing.Point(0, 0);
			this.labelMaxLogEValForExport.Name = "labelMaxLogEValForExport";
			this.labelMaxLogEValForExport.Size = new System.Drawing.Size(136, 24);
			this.labelMaxLogEValForExport.TabIndex = 0;
			this.labelMaxLogEValForExport.Text = "Max log(EVal) for export:";
			this.labelMaxLogEValForExport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.mcheckBoxNonTryptic);
			this.groupBox2.Controls.Add(this.mcheckBoxPartially);
			this.groupBox2.Controls.Add(this.mcheckBoxTryptic);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(3, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(228, 88);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Select Tryptic State You Want to Export";
			// 
			// mcheckBoxNonTryptic
			// 
			this.mcheckBoxNonTryptic.Dock = System.Windows.Forms.DockStyle.Top;
			this.mcheckBoxNonTryptic.Location = new System.Drawing.Point(3, 58);
			this.mcheckBoxNonTryptic.Name = "mcheckBoxNonTryptic";
			this.mcheckBoxNonTryptic.Size = new System.Drawing.Size(222, 21);
			this.mcheckBoxNonTryptic.TabIndex = 3;
			this.mcheckBoxNonTryptic.Text = "NonTryptic Peptides";
			// 
			// mcheckBoxPartially
			// 
			this.mcheckBoxPartially.Dock = System.Windows.Forms.DockStyle.Top;
			this.mcheckBoxPartially.Location = new System.Drawing.Point(3, 37);
			this.mcheckBoxPartially.Name = "mcheckBoxPartially";
			this.mcheckBoxPartially.Size = new System.Drawing.Size(222, 21);
			this.mcheckBoxPartially.TabIndex = 2;
			this.mcheckBoxPartially.Text = "Partially Tryptic Peptides";
			// 
			// mcheckBoxTryptic
			// 
			this.mcheckBoxTryptic.Checked = true;
			this.mcheckBoxTryptic.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mcheckBoxTryptic.Dock = System.Windows.Forms.DockStyle.Top;
			this.mcheckBoxTryptic.Location = new System.Drawing.Point(3, 16);
			this.mcheckBoxTryptic.Name = "mcheckBoxTryptic";
			this.mcheckBoxTryptic.Size = new System.Drawing.Size(222, 21);
			this.mcheckBoxTryptic.TabIndex = 1;
			this.mcheckBoxTryptic.Text = "Tryptic Peptides";
			// 
			// groupBoxSequest
			// 
			this.groupBoxSequest.Controls.Add(this.mgroupBoxXCorrThresholdsNonTryptic);
			this.groupBoxSequest.Controls.Add(this.mgroupBoxXCorrThresholdsPartiallyTryptic);
			this.groupBoxSequest.Controls.Add(this.mgroupBoxXCorrThresholds);
			this.groupBoxSequest.Controls.Add(this.mgroupBoxDelCN);
			this.groupBoxSequest.Dock = System.Windows.Forms.DockStyle.Right;
			this.groupBoxSequest.Location = new System.Drawing.Point(234, 0);
			this.groupBoxSequest.Name = "groupBoxSequest";
			this.groupBoxSequest.Size = new System.Drawing.Size(320, 181);
			this.groupBoxSequest.TabIndex = 6;
			this.groupBoxSequest.TabStop = false;
			this.groupBoxSequest.Text = "Sequest";
			// 
			// mgroupBoxXCorrThresholdsNonTryptic
			// 
			this.mgroupBoxXCorrThresholdsNonTryptic.Controls.Add(this.panel5);
			this.mgroupBoxXCorrThresholdsNonTryptic.Controls.Add(this.panel6);
			this.mgroupBoxXCorrThresholdsNonTryptic.Controls.Add(this.panel7);
			this.mgroupBoxXCorrThresholdsNonTryptic.Dock = System.Windows.Forms.DockStyle.Top;
			this.mgroupBoxXCorrThresholdsNonTryptic.Location = new System.Drawing.Point(3, 136);
			this.mgroupBoxXCorrThresholdsNonTryptic.Name = "mgroupBoxXCorrThresholdsNonTryptic";
			this.mgroupBoxXCorrThresholdsNonTryptic.Size = new System.Drawing.Size(314, 40);
			this.mgroupBoxXCorrThresholdsNonTryptic.TabIndex = 17;
			this.mgroupBoxXCorrThresholdsNonTryptic.TabStop = false;
			this.mgroupBoxXCorrThresholdsNonTryptic.Text = "NonTryptic XCorr Thresholds:";
			this.mgroupBoxXCorrThresholdsNonTryptic.MouseHover += new System.EventHandler(this.mgroupBoxXCorrThresholdsNonTryptic_MouseHover);
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.mtextBoxMinXCorrForExportNonTrypticsCS3);
			this.panel5.Controls.Add(this.labelXCorrForExportNonTrytpicsCS3);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel5.Location = new System.Drawing.Point(189, 16);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(99, 21);
			this.panel5.TabIndex = 13;
			// 
			// mtextBoxMinXCorrForExportNonTrypticsCS3
			// 
			this.mtextBoxMinXCorrForExportNonTrypticsCS3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForExportNonTrypticsCS3.Location = new System.Drawing.Point(72, 0);
			this.mtextBoxMinXCorrForExportNonTrypticsCS3.Name = "mtextBoxMinXCorrForExportNonTrypticsCS3";
			this.mtextBoxMinXCorrForExportNonTrypticsCS3.Size = new System.Drawing.Size(27, 20);
			this.mtextBoxMinXCorrForExportNonTrypticsCS3.TabIndex = 1;
			this.mtextBoxMinXCorrForExportNonTrypticsCS3.Text = "3";
			// 
			// labelXCorrForExportNonTrytpicsCS3
			// 
			this.labelXCorrForExportNonTrytpicsCS3.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelXCorrForExportNonTrytpicsCS3.Location = new System.Drawing.Point(0, 0);
			this.labelXCorrForExportNonTrytpicsCS3.Name = "labelXCorrForExportNonTrytpicsCS3";
			this.labelXCorrForExportNonTrytpicsCS3.Size = new System.Drawing.Size(72, 21);
			this.labelXCorrForExportNonTrytpicsCS3.TabIndex = 0;
			this.labelXCorrForExportNonTrytpicsCS3.Text = "Charge >=3:";
			this.labelXCorrForExportNonTrytpicsCS3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.mtextBoxMinXCorrForExportNonTrypticsCS2);
			this.panel6.Controls.Add(this.labelXCorrForExportNonTrytpicsCS2);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel6.Location = new System.Drawing.Point(96, 16);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(93, 21);
			this.panel6.TabIndex = 12;
			// 
			// mtextBoxMinXCorrForExportNonTrypticsCS2
			// 
			this.mtextBoxMinXCorrForExportNonTrypticsCS2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForExportNonTrypticsCS2.Location = new System.Drawing.Point(64, 0);
			this.mtextBoxMinXCorrForExportNonTrypticsCS2.Name = "mtextBoxMinXCorrForExportNonTrypticsCS2";
			this.mtextBoxMinXCorrForExportNonTrypticsCS2.Size = new System.Drawing.Size(29, 20);
			this.mtextBoxMinXCorrForExportNonTrypticsCS2.TabIndex = 1;
			this.mtextBoxMinXCorrForExportNonTrypticsCS2.Text = "3";
			// 
			// labelXCorrForExportNonTrytpicsCS2
			// 
			this.labelXCorrForExportNonTrytpicsCS2.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelXCorrForExportNonTrytpicsCS2.Location = new System.Drawing.Point(0, 0);
			this.labelXCorrForExportNonTrytpicsCS2.Name = "labelXCorrForExportNonTrytpicsCS2";
			this.labelXCorrForExportNonTrytpicsCS2.Size = new System.Drawing.Size(64, 21);
			this.labelXCorrForExportNonTrytpicsCS2.TabIndex = 0;
			this.labelXCorrForExportNonTrytpicsCS2.Text = "Charge 2:";
			this.labelXCorrForExportNonTrytpicsCS2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.mtextBoxMinXCorrForExportNonTrypticsCS1);
			this.panel7.Controls.Add(this.labelXCorrForExportNonTrytpicsCS1);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel7.Location = new System.Drawing.Point(3, 16);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(93, 21);
			this.panel7.TabIndex = 11;
			// 
			// mtextBoxMinXCorrForExportNonTrypticsCS1
			// 
			this.mtextBoxMinXCorrForExportNonTrypticsCS1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForExportNonTrypticsCS1.Location = new System.Drawing.Point(64, 0);
			this.mtextBoxMinXCorrForExportNonTrypticsCS1.Name = "mtextBoxMinXCorrForExportNonTrypticsCS1";
			this.mtextBoxMinXCorrForExportNonTrypticsCS1.Size = new System.Drawing.Size(29, 20);
			this.mtextBoxMinXCorrForExportNonTrypticsCS1.TabIndex = 1;
			this.mtextBoxMinXCorrForExportNonTrypticsCS1.Text = "3";
			// 
			// labelXCorrForExportNonTrytpicsCS1
			// 
			this.labelXCorrForExportNonTrytpicsCS1.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelXCorrForExportNonTrytpicsCS1.Location = new System.Drawing.Point(0, 0);
			this.labelXCorrForExportNonTrytpicsCS1.Name = "labelXCorrForExportNonTrytpicsCS1";
			this.labelXCorrForExportNonTrytpicsCS1.Size = new System.Drawing.Size(64, 21);
			this.labelXCorrForExportNonTrytpicsCS1.TabIndex = 0;
			this.labelXCorrForExportNonTrytpicsCS1.Text = "Charge 1:";
			this.labelXCorrForExportNonTrytpicsCS1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mgroupBoxXCorrThresholdsPartiallyTryptic
			// 
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.Controls.Add(this.panel2);
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.Controls.Add(this.panel3);
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.Controls.Add(this.panel4);
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.Dock = System.Windows.Forms.DockStyle.Top;
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.Location = new System.Drawing.Point(3, 96);
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.Name = "mgroupBoxXCorrThresholdsPartiallyTryptic";
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.Size = new System.Drawing.Size(314, 40);
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.TabIndex = 16;
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.TabStop = false;
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.Text = "Partially Tryptic XCorr Thresholds:";
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.MouseHover += new System.EventHandler(this.groupBox4_MouseHover);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3);
			this.panel2.Controls.Add(this.labelXCorrForExportPartiallyTrytpicsCS3);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel2.Location = new System.Drawing.Point(189, 16);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(99, 21);
			this.panel2.TabIndex = 13;
			// 
			// mtextBoxMinXCorrForExportPartiallyTrypticsCS3
			// 
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Location = new System.Drawing.Point(72, 0);
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Name = "mtextBoxMinXCorrForExportPartiallyTrypticsCS3";
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Size = new System.Drawing.Size(27, 20);
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.TabIndex = 1;
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Text = "3";
			// 
			// labelXCorrForExportPartiallyTrytpicsCS3
			// 
			this.labelXCorrForExportPartiallyTrytpicsCS3.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelXCorrForExportPartiallyTrytpicsCS3.Location = new System.Drawing.Point(0, 0);
			this.labelXCorrForExportPartiallyTrytpicsCS3.Name = "labelXCorrForExportPartiallyTrytpicsCS3";
			this.labelXCorrForExportPartiallyTrytpicsCS3.Size = new System.Drawing.Size(72, 21);
			this.labelXCorrForExportPartiallyTrytpicsCS3.TabIndex = 0;
			this.labelXCorrForExportPartiallyTrytpicsCS3.Text = "Charge >=3:";
			this.labelXCorrForExportPartiallyTrytpicsCS3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2);
			this.panel3.Controls.Add(this.labelXCorrForExportPartiallyTrytpicsCS2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel3.Location = new System.Drawing.Point(96, 16);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(93, 21);
			this.panel3.TabIndex = 12;
			// 
			// mtextBoxMinXCorrForExportPartiallyTrypticsCS2
			// 
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Location = new System.Drawing.Point(64, 0);
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Name = "mtextBoxMinXCorrForExportPartiallyTrypticsCS2";
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Size = new System.Drawing.Size(29, 20);
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.TabIndex = 1;
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Text = "3";
			// 
			// labelXCorrForExportPartiallyTrytpicsCS2
			// 
			this.labelXCorrForExportPartiallyTrytpicsCS2.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelXCorrForExportPartiallyTrytpicsCS2.Location = new System.Drawing.Point(0, 0);
			this.labelXCorrForExportPartiallyTrytpicsCS2.Name = "labelXCorrForExportPartiallyTrytpicsCS2";
			this.labelXCorrForExportPartiallyTrytpicsCS2.Size = new System.Drawing.Size(64, 21);
			this.labelXCorrForExportPartiallyTrytpicsCS2.TabIndex = 0;
			this.labelXCorrForExportPartiallyTrytpicsCS2.Text = "Charge 2:";
			this.labelXCorrForExportPartiallyTrytpicsCS2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1);
			this.panel4.Controls.Add(this.labelXCorrForExportPartiallyTrytpicsCS1);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel4.Location = new System.Drawing.Point(3, 16);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(93, 21);
			this.panel4.TabIndex = 11;
			// 
			// mtextBoxMinXCorrForExportPartiallyTrypticsCS1
			// 
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Location = new System.Drawing.Point(64, 0);
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Name = "mtextBoxMinXCorrForExportPartiallyTrypticsCS1";
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Size = new System.Drawing.Size(29, 20);
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.TabIndex = 1;
			this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Text = "3";
			// 
			// labelXCorrForExportPartiallyTrytpicsCS1
			// 
			this.labelXCorrForExportPartiallyTrytpicsCS1.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelXCorrForExportPartiallyTrytpicsCS1.Location = new System.Drawing.Point(0, 0);
			this.labelXCorrForExportPartiallyTrytpicsCS1.Name = "labelXCorrForExportPartiallyTrytpicsCS1";
			this.labelXCorrForExportPartiallyTrytpicsCS1.Size = new System.Drawing.Size(64, 21);
			this.labelXCorrForExportPartiallyTrytpicsCS1.TabIndex = 0;
			this.labelXCorrForExportPartiallyTrytpicsCS1.Text = "Charge 1:";
			this.labelXCorrForExportPartiallyTrytpicsCS1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mgroupBoxXCorrThresholds
			// 
			this.mgroupBoxXCorrThresholds.Controls.Add(this.panelTrypticCS3);
			this.mgroupBoxXCorrThresholds.Controls.Add(this.panel1);
			this.mgroupBoxXCorrThresholds.Controls.Add(this.mpanelXCorrExport);
			this.mgroupBoxXCorrThresholds.Dock = System.Windows.Forms.DockStyle.Top;
			this.mgroupBoxXCorrThresholds.Enabled = ((bool)(configurationAppSettings.GetValue("checkBoxTryptic.Checked", typeof(bool))));
			this.mgroupBoxXCorrThresholds.Location = new System.Drawing.Point(3, 56);
			this.mgroupBoxXCorrThresholds.Name = "mgroupBoxXCorrThresholds";
			this.mgroupBoxXCorrThresholds.Size = new System.Drawing.Size(314, 40);
			this.mgroupBoxXCorrThresholds.TabIndex = 15;
			this.mgroupBoxXCorrThresholds.TabStop = false;
			this.mgroupBoxXCorrThresholds.Text = "Tyrptic XCorr Thresholds:";
			this.mgroupBoxXCorrThresholds.MouseHover += new System.EventHandler(this.mgroupBoxXCorrThresholds_MouseHover);
			// 
			// panelTrypticCS3
			// 
			this.panelTrypticCS3.Controls.Add(this.mtextBoxMinXCorrForExportTrypticsCS3);
			this.panelTrypticCS3.Controls.Add(this.labelXCorrForExportCS3);
			this.panelTrypticCS3.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelTrypticCS3.Location = new System.Drawing.Point(189, 16);
			this.panelTrypticCS3.Name = "panelTrypticCS3";
			this.panelTrypticCS3.Size = new System.Drawing.Size(99, 21);
			this.panelTrypticCS3.TabIndex = 13;
			// 
			// mtextBoxMinXCorrForExportTrypticsCS3
			// 
			this.mtextBoxMinXCorrForExportTrypticsCS3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForExportTrypticsCS3.Enabled = ((bool)(configurationAppSettings.GetValue("mcheckBoxTryptic.Checked", typeof(bool))));
			this.mtextBoxMinXCorrForExportTrypticsCS3.Location = new System.Drawing.Point(72, 0);
			this.mtextBoxMinXCorrForExportTrypticsCS3.Name = "mtextBoxMinXCorrForExportTrypticsCS3";
			this.mtextBoxMinXCorrForExportTrypticsCS3.Size = new System.Drawing.Size(27, 20);
			this.mtextBoxMinXCorrForExportTrypticsCS3.TabIndex = 1;
			this.mtextBoxMinXCorrForExportTrypticsCS3.Text = "3";
			// 
			// labelXCorrForExportCS3
			// 
			this.labelXCorrForExportCS3.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelXCorrForExportCS3.Location = new System.Drawing.Point(0, 0);
			this.labelXCorrForExportCS3.Name = "labelXCorrForExportCS3";
			this.labelXCorrForExportCS3.Size = new System.Drawing.Size(72, 21);
			this.labelXCorrForExportCS3.TabIndex = 0;
			this.labelXCorrForExportCS3.Text = "Charge >=3:";
			this.labelXCorrForExportCS3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.mtextBoxMinXCorrForExportTrypticsCS2);
			this.panel1.Controls.Add(this.labelXCorrForExportCS2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(96, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(93, 21);
			this.panel1.TabIndex = 12;
			// 
			// mtextBoxMinXCorrForExportTrypticsCS2
			// 
			this.mtextBoxMinXCorrForExportTrypticsCS2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForExportTrypticsCS2.Location = new System.Drawing.Point(64, 0);
			this.mtextBoxMinXCorrForExportTrypticsCS2.Name = "mtextBoxMinXCorrForExportTrypticsCS2";
			this.mtextBoxMinXCorrForExportTrypticsCS2.Size = new System.Drawing.Size(29, 20);
			this.mtextBoxMinXCorrForExportTrypticsCS2.TabIndex = 1;
			this.mtextBoxMinXCorrForExportTrypticsCS2.Text = "3";
			// 
			// labelXCorrForExportCS2
			// 
			this.labelXCorrForExportCS2.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelXCorrForExportCS2.Enabled = ((bool)(configurationAppSettings.GetValue("checkBoxTryptic.Checked", typeof(bool))));
			this.labelXCorrForExportCS2.Location = new System.Drawing.Point(0, 0);
			this.labelXCorrForExportCS2.Name = "labelXCorrForExportCS2";
			this.labelXCorrForExportCS2.Size = new System.Drawing.Size(64, 21);
			this.labelXCorrForExportCS2.TabIndex = 0;
			this.labelXCorrForExportCS2.Text = "Charge 2:";
			this.labelXCorrForExportCS2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mpanelXCorrExport
			// 
			this.mpanelXCorrExport.Controls.Add(this.mtextBoxMinXCorrForExportTrypticsCS1);
			this.mpanelXCorrExport.Controls.Add(this.labelXCorrForExportCS1);
			this.mpanelXCorrExport.Dock = System.Windows.Forms.DockStyle.Left;
			this.mpanelXCorrExport.Location = new System.Drawing.Point(3, 16);
			this.mpanelXCorrExport.Name = "mpanelXCorrExport";
			this.mpanelXCorrExport.Size = new System.Drawing.Size(93, 21);
			this.mpanelXCorrExport.TabIndex = 11;
			// 
			// mtextBoxMinXCorrForExportTrypticsCS1
			// 
			this.mtextBoxMinXCorrForExportTrypticsCS1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mtextBoxMinXCorrForExportTrypticsCS1.Location = new System.Drawing.Point(64, 0);
			this.mtextBoxMinXCorrForExportTrypticsCS1.Name = "mtextBoxMinXCorrForExportTrypticsCS1";
			this.mtextBoxMinXCorrForExportTrypticsCS1.Size = new System.Drawing.Size(29, 20);
			this.mtextBoxMinXCorrForExportTrypticsCS1.TabIndex = 1;
			this.mtextBoxMinXCorrForExportTrypticsCS1.Text = "3";
			// 
			// labelXCorrForExportCS1
			// 
			this.labelXCorrForExportCS1.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelXCorrForExportCS1.Location = new System.Drawing.Point(0, 0);
			this.labelXCorrForExportCS1.Name = "labelXCorrForExportCS1";
			this.labelXCorrForExportCS1.Size = new System.Drawing.Size(64, 21);
			this.labelXCorrForExportCS1.TabIndex = 0;
			this.labelXCorrForExportCS1.Text = "Charge 1:";
			this.labelXCorrForExportCS1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mgroupBoxDelCN
			// 
			this.mgroupBoxDelCN.Controls.Add(this.mtextBoxDelCN);
			this.mgroupBoxDelCN.Controls.Add(this.mlabelDelCN);
			this.mgroupBoxDelCN.Controls.Add(this.mcheckBoxDelCN);
			this.mgroupBoxDelCN.Dock = System.Windows.Forms.DockStyle.Top;
			this.mgroupBoxDelCN.Location = new System.Drawing.Point(3, 16);
			this.mgroupBoxDelCN.Name = "mgroupBoxDelCN";
			this.mgroupBoxDelCN.Size = new System.Drawing.Size(314, 40);
			this.mgroupBoxDelCN.TabIndex = 13;
			this.mgroupBoxDelCN.TabStop = false;
			this.mgroupBoxDelCN.Text = "DelCN";
			// 
			// mtextBoxDelCN
			// 
			this.mtextBoxDelCN.Dock = System.Windows.Forms.DockStyle.Left;
			this.mtextBoxDelCN.Location = new System.Drawing.Point(216, 16);
			this.mtextBoxDelCN.Name = "mtextBoxDelCN";
			this.mtextBoxDelCN.Size = new System.Drawing.Size(27, 20);
			this.mtextBoxDelCN.TabIndex = 2;
			this.mtextBoxDelCN.Text = "0.1";
			// 
			// mlabelDelCN
			// 
			this.mlabelDelCN.Dock = System.Windows.Forms.DockStyle.Left;
			this.mlabelDelCN.Location = new System.Drawing.Point(147, 16);
			this.mlabelDelCN.Name = "mlabelDelCN";
			this.mlabelDelCN.Size = new System.Drawing.Size(69, 21);
			this.mlabelDelCN.TabIndex = 1;
			this.mlabelDelCN.Text = "Min DelCN:";
			this.mlabelDelCN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// mcheckBoxDelCN
			// 
			this.mcheckBoxDelCN.Dock = System.Windows.Forms.DockStyle.Left;
			this.mcheckBoxDelCN.Location = new System.Drawing.Point(3, 16);
			this.mcheckBoxDelCN.Name = "mcheckBoxDelCN";
			this.mcheckBoxDelCN.Size = new System.Drawing.Size(144, 21);
			this.mcheckBoxDelCN.TabIndex = 0;
			this.mcheckBoxDelCN.Text = "Use DelCN Threshold";
			// 
			// frmOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(562, 271);
			this.Controls.Add(this.mpanelMain);
			this.Controls.Add(this.panelOKCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmOptions";
			this.Text = "Options";
			this.panelOKCancel.ResumeLayout(false);
			this.mpanelMain.ResumeLayout(false);
			this.mtabControlOptions.ResumeLayout(false);
			this.mtabPageAlignmentOptions.ResumeLayout(false);
			this.panelAlignmentOptions.ResumeLayout(false);
			this.groupBoxAlignmentType.ResumeLayout(false);
			this.panelOrderRegression.ResumeLayout(false);
			this.groupBoxPredictionAlgo.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panelSequest.ResumeLayout(false);
			this.panelMaxLogEValForAlignment.ResumeLayout(false);
			this.panelMaxRankForExport.ResumeLayout(false);
			this.panelModification.ResumeLayout(false);
			this.panelMinObservations.ResumeLayout(false);
			this.mtabPageExportOptions.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBoxXTandem.ResumeLayout(false);
			this.panelXTandemForExport.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBoxSequest.ResumeLayout(false);
			this.mgroupBoxXCorrThresholdsNonTryptic.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.mgroupBoxXCorrThresholdsPartiallyTryptic.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.mgroupBoxXCorrThresholds.ResumeLayout(false);
			this.panelTrypticCS3.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.mpanelXCorrExport.ResumeLayout(false);
			this.mgroupBoxDelCN.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region "Properties"
		public double  [] MinXCorrForExportTrytpic
		{
			get
			{
				double xcorrCS1 = Convert.ToDouble(mtextBoxMinXCorrForExportTrypticsCS1.Text) ; 
				double xcorrCS2 = Convert.ToDouble(mtextBoxMinXCorrForExportTrypticsCS2.Text) ; 
				double xcorrCS3 = Convert.ToDouble(mtextBoxMinXCorrForExportTrypticsCS3.Text) ; 
				return new double [] {xcorrCS1, xcorrCS2, xcorrCS3 } ; 
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException() ; 
				if (value.Length != 3)
					throw new System.ArgumentException("Incorrect number of arguments to MinXCorrForExportTrytpic") ; 

				mtextBoxMinXCorrForExportTrypticsCS1.Text = Convert.ToString(value[0]) ; 
				mtextBoxMinXCorrForExportTrypticsCS2.Text = Convert.ToString(value[1]) ; 
				mtextBoxMinXCorrForExportTrypticsCS3.Text = Convert.ToString(value[2]) ; 
			}
		}
		public double  [] MinXCorrForExportPartiallyTrytpic
		{
			get
			{
				double xcorrCS1 = Convert.ToDouble(mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Text) ; 
				double xcorrCS2 = Convert.ToDouble(mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Text) ; 
				double xcorrCS3 = Convert.ToDouble(mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Text) ; 
				return new double [] {xcorrCS1, xcorrCS2, xcorrCS3 } ; 
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException() ; 
				if (value.Length != 3)
					throw new System.ArgumentException("Incorrect number of arguments to MinXCorrForExportPartiallyTrytpic") ; 

				mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Text = Convert.ToString(value[0]) ; 
				mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Text = Convert.ToString(value[1]) ; 
				mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Text = Convert.ToString(value[2]) ; 
			}
		}
		public double  [] MinXCorrForExportNonTrytpic
		{
			get
			{
				double xcorrCS1 = Convert.ToDouble(mtextBoxMinXCorrForExportNonTrypticsCS1.Text) ; 
				double xcorrCS2 = Convert.ToDouble(mtextBoxMinXCorrForExportNonTrypticsCS2.Text) ; 
				double xcorrCS3 = Convert.ToDouble(mtextBoxMinXCorrForExportNonTrypticsCS3.Text) ; 
				return new double [] {xcorrCS1, xcorrCS2, xcorrCS3 } ; 
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException() ; 
				if (value.Length != 3)
					throw new System.ArgumentException("Incorrect number of arguments to MinXCorrForExportNonTrytpic") ; 

				mtextBoxMinXCorrForExportNonTrypticsCS1.Text = Convert.ToString(value[0]) ; 
				mtextBoxMinXCorrForExportNonTrypticsCS2.Text = Convert.ToString(value[1]) ; 
				mtextBoxMinXCorrForExportNonTrypticsCS3.Text = Convert.ToString(value[2]) ; 
			}
		}


		public double MinDelCNForExport
		{
			get
			{
				return Convert.ToDouble(mtextBoxDelCN.Text); 
			}
			set
			{
				mtextBoxDelCN.Text = Convert.ToString(value) ; 
			}
		}

		public double MinXCorrForAlignment
		{
			get
			{
				return Convert.ToDouble(mtextBoxMinXCorrForAlignment.Text) ; 
			}
			set
			{
				mtextBoxMinXCorrForAlignment.Text = Convert.ToString(value) ; 
			}
		}

		public bool UseDelCN
		{
			get
			{
				return mcheckBoxDelCN.Checked ; 
			}
			set
			{
				mcheckBoxDelCN.Checked = value ; 
			}
		}

		public bool ExportTryptic
		{
			get
			{
				return mcheckBoxTryptic.Checked ; 
			}
			set
			{
				mcheckBoxTryptic.Checked = value ; 
			}
		}

		public bool ExportPartiallyTryptic
		{
			get
			{
				return mcheckBoxPartially.Checked ; 
			}
			set
			{
				mcheckBoxPartially.Checked = value ; 
			}
		}

		public bool ExportNonTryptic
		{
			get
			{
				return mcheckBoxNonTryptic.Checked ; 
			}
			set
			{
				mcheckBoxNonTryptic.Checked = value ; 
			}
		}

		public double MaxLogEValForXTandemExport
		{
			get
			{
				return Convert.ToDouble(mtextBoxMaxLogEvalForExport.Text) ; 
			}
			set
			{
				mtextBoxMaxLogEvalForExport.Text = Convert.ToString(value) ; 
			}
		}

		public double MaxLogEValForXTandemAlignment
		{
			get
			{
				return Convert.ToDouble(mtextBoxMaxLogEValForAlignment.Text) ; 
			}
			set
			{
				mtextBoxMaxLogEValForAlignment.Text = Convert.ToString(value) ; 
			}
		}

		public short MaxModificationsForAlignment
		{
			get
			{
				return Convert.ToInt16(mtextBoxModifications.Text) ; 
			}
			set
			{
				mtextBoxModifications.Text = Convert.ToString(value) ; 
			}
		}

		public short MaxRankForExport
		{
			get
			{
				return Convert.ToInt16(mtextBoxMaxRank.Text) ; 
			}
			set
			{
				mtextBoxMaxRank.Text = Convert.ToString(value) ; 
			}
		}

		public short MinObservationsForExport
		{
			get
			{
				return Convert.ToInt16(mtextBoxMinObservations.Text) ; 
			}
			set
			{
				mtextBoxMinObservations.Text = Convert.ToString(value); 
			}
		}

		public bool UseKrokhinNET
		{
			get
			{
				return mradioButtonKrokhin.Checked ; 
			}
			set
			{
				mradioButtonKrokhin.Checked = value ; 
			}
		}

		public short RegressionOrder
		{
			get
			{
				return Convert.ToInt16(mtextBoxRegressionOrder.Text) ;
			}
			set
			{
				mtextBoxRegressionOrder.Text = Convert.ToString(value) ;
			}
		}
		public Regressor.clsRegressor.RegressionType RegressionType
		{
			get
			{
				if (mradioButtonLinearEM.Checked)
					return Regressor.clsRegressor.RegressionType.LINEAR_EM ; 
				else
					return Regressor.clsRegressor.RegressionType.MIXTURE_REGRESSION ; 
			}
			set
			{
				if (value == Regressor.clsRegressor.RegressionType.LINEAR_EM)
				{
					mradioButtonLinearEM.Checked = true ; 
				}
				else
				{
					mradioButtonMixtureRegression.Checked = true ; 
				}
			}
		}

		public clsOptions Options
		{
			set
			{
				MinXCorrForExportTrytpic = value.MinXCorrForExportTrytpic ; 
				MinXCorrForExportPartiallyTrytpic = value.MinXCorrForExportPartiallyTrytpic ; 
				MinXCorrForExportNonTrytpic = value.MinXCorrForExportNonTrytpic ; 
				MinDelCNForExport = value.MinDelCNForExport ; 
				MinXCorrForAlignment = value.MinXCorrForAlignment ; 
				UseDelCN = value.UseDelCN ; 
				ExportTryptic = value.ExportTryptic ; 
				ExportPartiallyTryptic = value.ExportPartiallyTryptic ; 
				ExportNonTryptic = value.ExportNonTryptic ; 
				MaxLogEValForXTandemExport = value.MaxLogEValForXTandemExport ; 
				MaxLogEValForXTandemAlignment = value.MaxLogEValForXTandemAlignment ; 
				MaxModificationsForAlignment = value.MaxModificationsForAlignment ; 
				MaxRankForExport = value.MaxRankForExport ; 
				MinObservationsForExport = value.MinObservationsForExport ; 
				UseKrokhinNET = value.UseKrokhinNET ; 
				RegressionType = value.RegressionType ; 
				RegressionOrder = value.RegressionOrder ; 
			}
			get
			{
				clsOptions options = new clsOptions() ; 

				options.MinXCorrForExportTrytpic = MinXCorrForExportTrytpic ; 
				options.MinXCorrForExportPartiallyTrytpic = MinXCorrForExportPartiallyTrytpic ; 
				options.MinXCorrForExportNonTrytpic = MinXCorrForExportNonTrytpic ; 
				options.MinDelCNForExport = MinDelCNForExport ; 
				options.MinXCorrForAlignment = MinXCorrForAlignment ; 
				options.UseDelCN = UseDelCN ; 
				options.ExportTryptic = ExportTryptic ; 
				options.ExportPartiallyTryptic = ExportPartiallyTryptic ; 
				options.ExportNonTryptic = ExportNonTryptic ; 
				options.MaxLogEValForXTandemExport = MaxLogEValForXTandemExport ; 
				options.MaxLogEValForXTandemAlignment = MaxLogEValForXTandemAlignment ; 
				options.MaxModificationsForAlignment = MaxModificationsForAlignment ; 
				options.MaxRankForExport = MaxRankForExport ; 
				options.MinObservationsForExport = MinObservationsForExport ; 
				options.UseKrokhinNET = UseKrokhinNET ; 
				options.RegressionType = RegressionType ; 
				options.RegressionOrder = RegressionOrder ; 
				return options ; 
			}
		}
		#endregion

		#region "Event Handling"
		private void mbuttonOK_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK ;
			this.Hide() ; 
		}

		private void mbuttonCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel ; 
			this.Hide() ; 
		}
		#endregion
		private void mtextBoxMaxLogEValForAlignment_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Specifies the maximum log(eval) that a match from X!Tandem can have for it to be used in the alignment" ;		
		}

		private void mtextBoxModifications_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "For datasets, this specifies the maximum number of modifications a peptide can have for it to be used in the alignment" ;						
		}

		private void mtextBoxMaxRank_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Maximum Rank of a Match for Loading" ;										
		}

		private void mtextBoxMinObservations_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Specifies the minimum number of observations needed for a mass tag to be exported into the database" ;
		}

		private void mtextBoxMinXCorrForAlignment_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "For SEQUEST results, this specifies the minimum XCorr a match can have for it to be used in the alignment" ;
		}

		private void mradioButtonKangas_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Use Prediction by Kangas to compute NET for a sequence. (Petritis, K., Kangas, L.J., et. al. Anal Chem. 2003 Mar 1; 75(5):1039-48)" ; 								
		}

		private void mradioButtonKrokhin_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Use Prediction by Krokhin to compute NET for a sequence. (Krokhin, O.V. et. al.  Mol. Cell. Proteomics 2004, 3, 908-919)" ; 								
		}

		private void mradioButtonLinearEM_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Check to use Least Squares Linear Fit between Scan and NET" ; 		
		}

		private void mradioButtonMixtureRegression_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Check to use Mixture Model Regression Fit between Scan and NET" ; 				
		}

		private void mtextBoxRegressionOrder_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Specify order of Mixture Model Regression Fit between Scan and NET" ; 				
		}

		private void mgroupBoxXCorrThresholds_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Specify threshold XCorr scores for different charges, for matches to be exported to database" ; 
		}

		private void groupBox4_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Specify threshold XCorr scores for different charges of partially tryptic peptides, for matches to be exported to database" ; 		
		}

		private void mgroupBoxXCorrThresholdsNonTryptic_MouseHover(object sender, System.EventArgs e)
		{
			mstatusBar.Text = "Specify threshold XCorr scores for different charges of non tryptic peptides, for matches to be exported to database" ; 				
		}

	}
}
