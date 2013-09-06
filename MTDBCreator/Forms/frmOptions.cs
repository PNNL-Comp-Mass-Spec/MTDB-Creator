using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MTDBCreator.Algorithms;
using MTDBCreator.Data;

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
		private System.Windows.Forms.TabControl mtabControlOptions;
		private System.Windows.Forms.TabPage mtabPageAlignmentOptions;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelMinObservations;
		private System.Windows.Forms.TextBox mtextBoxModifications;
        private System.Windows.Forms.Label labelModifications;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxAlignmentType;
		private System.Windows.Forms.TextBox mtextBoxRegressionOrder;
		private System.Windows.Forms.GroupBox groupBoxPredictionAlgo;
        private System.Windows.Forms.TabPage mtabPageExportOptions;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox mgroupBoxDelCN;
		private System.Windows.Forms.Label mlabelDelCN;
		private System.Windows.Forms.CheckBox mcheckBoxDelCN;
        private System.Windows.Forms.GroupBox mgroupBoxXCorrThresholds;
		private System.Windows.Forms.Label labelMinXCorr;
        private System.Windows.Forms.GroupBox groupBoxXTandem;
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
		private System.Windows.Forms.TextBox mtextBoxMinObservations;
		private System.Windows.Forms.RadioButton mradioButtonKrokhin;
		private System.Windows.Forms.RadioButton mradioButtonKangas;
		private System.Windows.Forms.RadioButton mradioButtonMixtureRegression;
        private System.Windows.Forms.RadioButton mradioButtonLinearEM;
		private System.Windows.Forms.Label labelXCorrForExportPartiallyTrytpicsCS3;
		private System.Windows.Forms.Label labelXCorrForExportPartiallyTrytpicsCS2;
		private System.Windows.Forms.Label labelXCorrForExportPartiallyTrytpicsCS1;
		private System.Windows.Forms.Label labelXCorrForExportNonTrytpicsCS3;
		private System.Windows.Forms.Label labelXCorrForExportNonTrytpicsCS2;
		private System.Windows.Forms.Label labelXCorrForExportNonTrytpicsCS1;
		private System.Windows.Forms.Label mlabelRegressionOrder;
		private System.Windows.Forms.GroupBox mgroupBoxXCorrThresholdsNonTryptic;
		private System.Windows.Forms.GroupBox mgroupBoxXCorrThresholdsPartiallyTryptic;
        private System.Windows.Forms.TextBox txtPredictionAlgorithmReference;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private TextBox mtextBoxMinXCorrForExportTrypticsCS3;
        private Label labelXCorrForExportCS3;
        private TextBox mtextBoxMinXCorrForExportTrypticsCS2;
        private Label labelXCorrForExportCS2;
        private TextBox mtextBoxMinXCorrForExportTrypticsCS1;
        private Label labelXCorrForExportCS1;
        private Label description;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmOptions()
		{
			InitializeComponent();
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


			mradioButtonKangas.Checked = true ;
			mradioButtonKangas.Enabled = true ;  
			UpdatePredictionReference();
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
            this.mbuttonCancel = new System.Windows.Forms.Button();
            this.mbuttonOK = new System.Windows.Forms.Button();
            this.mpanelMain = new System.Windows.Forms.Panel();
            this.mtabControlOptions = new System.Windows.Forms.TabControl();
            this.mtabPageAlignmentOptions = new System.Windows.Forms.TabPage();
            this.description = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.mtextBoxMaxLogEValForAlignment = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.mtextBoxMinXCorrForAlignment = new System.Windows.Forms.TextBox();
            this.labelMinXCorr = new System.Windows.Forms.Label();
            this.groupBoxAlignmentType = new System.Windows.Forms.GroupBox();
            this.mtextBoxRegressionOrder = new System.Windows.Forms.TextBox();
            this.mlabelRegressionOrder = new System.Windows.Forms.Label();
            this.mradioButtonMixtureRegression = new System.Windows.Forms.RadioButton();
            this.mradioButtonLinearEM = new System.Windows.Forms.RadioButton();
            this.groupBoxPredictionAlgo = new System.Windows.Forms.GroupBox();
            this.mradioButtonKrokhin = new System.Windows.Forms.RadioButton();
            this.txtPredictionAlgorithmReference = new System.Windows.Forms.TextBox();
            this.mradioButtonKangas = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelMinObservations = new System.Windows.Forms.Label();
            this.mtextBoxMinObservations = new System.Windows.Forms.TextBox();
            this.mtextBoxModifications = new System.Windows.Forms.TextBox();
            this.labelModifications = new System.Windows.Forms.Label();
            this.mtabPageExportOptions = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBoxXTandem = new System.Windows.Forms.GroupBox();
            this.labelMaxLogEValForExport = new System.Windows.Forms.Label();
            this.mtextBoxMaxLogEvalForExport = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mgroupBoxXCorrThresholdsNonTryptic = new System.Windows.Forms.GroupBox();
            this.mtextBoxMinXCorrForExportNonTrypticsCS3 = new System.Windows.Forms.TextBox();
            this.labelXCorrForExportNonTrytpicsCS3 = new System.Windows.Forms.Label();
            this.mtextBoxMinXCorrForExportNonTrypticsCS2 = new System.Windows.Forms.TextBox();
            this.labelXCorrForExportNonTrytpicsCS2 = new System.Windows.Forms.Label();
            this.mtextBoxMinXCorrForExportNonTrypticsCS1 = new System.Windows.Forms.TextBox();
            this.labelXCorrForExportNonTrytpicsCS1 = new System.Windows.Forms.Label();
            this.mcheckBoxNonTryptic = new System.Windows.Forms.CheckBox();
            this.mcheckBoxPartially = new System.Windows.Forms.CheckBox();
            this.mgroupBoxXCorrThresholds = new System.Windows.Forms.GroupBox();
            this.mtextBoxMinXCorrForExportTrypticsCS3 = new System.Windows.Forms.TextBox();
            this.mtextBoxMinXCorrForExportTrypticsCS2 = new System.Windows.Forms.TextBox();
            this.labelXCorrForExportCS3 = new System.Windows.Forms.Label();
            this.mtextBoxMinXCorrForExportTrypticsCS1 = new System.Windows.Forms.TextBox();
            this.labelXCorrForExportCS2 = new System.Windows.Forms.Label();
            this.labelXCorrForExportCS1 = new System.Windows.Forms.Label();
            this.mcheckBoxTryptic = new System.Windows.Forms.CheckBox();
            this.mgroupBoxXCorrThresholdsPartiallyTryptic = new System.Windows.Forms.GroupBox();
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3 = new System.Windows.Forms.TextBox();
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2 = new System.Windows.Forms.TextBox();
            this.labelXCorrForExportPartiallyTrytpicsCS3 = new System.Windows.Forms.Label();
            this.labelXCorrForExportPartiallyTrytpicsCS2 = new System.Windows.Forms.Label();
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1 = new System.Windows.Forms.TextBox();
            this.labelXCorrForExportPartiallyTrytpicsCS1 = new System.Windows.Forms.Label();
            this.mgroupBoxDelCN = new System.Windows.Forms.GroupBox();
            this.mtextBoxDelCN = new System.Windows.Forms.TextBox();
            this.mlabelDelCN = new System.Windows.Forms.Label();
            this.mcheckBoxDelCN = new System.Windows.Forms.CheckBox();
            this.panelOKCancel.SuspendLayout();
            this.mpanelMain.SuspendLayout();
            this.mtabControlOptions.SuspendLayout();
            this.mtabPageAlignmentOptions.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxAlignmentType.SuspendLayout();
            this.groupBoxPredictionAlgo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.mtabPageExportOptions.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBoxXTandem.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.mgroupBoxXCorrThresholdsNonTryptic.SuspendLayout();
            this.mgroupBoxXCorrThresholds.SuspendLayout();
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.SuspendLayout();
            this.mgroupBoxDelCN.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelOKCancel
            // 
            this.panelOKCancel.Controls.Add(this.mbuttonCancel);
            this.panelOKCancel.Controls.Add(this.mbuttonOK);
            this.panelOKCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOKCancel.Location = new System.Drawing.Point(0, 655);
            this.panelOKCancel.Name = "panelOKCancel";
            this.panelOKCancel.Size = new System.Drawing.Size(488, 39);
            this.panelOKCancel.TabIndex = 0;
            // 
            // mbuttonCancel
            // 
            this.mbuttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbuttonCancel.BackColor = System.Drawing.Color.White;
            this.mbuttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbuttonCancel.Location = new System.Drawing.Point(398, 6);
            this.mbuttonCancel.Name = "mbuttonCancel";
            this.mbuttonCancel.Size = new System.Drawing.Size(86, 27);
            this.mbuttonCancel.TabIndex = 1;
            this.mbuttonCancel.Text = "Cancel";
            this.mbuttonCancel.UseVisualStyleBackColor = false;
            this.mbuttonCancel.Click += new System.EventHandler(this.mbuttonCancel_Click);
            // 
            // mbuttonOK
            // 
            this.mbuttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mbuttonOK.BackColor = System.Drawing.Color.White;
            this.mbuttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbuttonOK.Location = new System.Drawing.Point(306, 6);
            this.mbuttonOK.Name = "mbuttonOK";
            this.mbuttonOK.Size = new System.Drawing.Size(86, 27);
            this.mbuttonOK.TabIndex = 0;
            this.mbuttonOK.Text = "OK";
            this.mbuttonOK.UseVisualStyleBackColor = false;
            this.mbuttonOK.Click += new System.EventHandler(this.mbuttonOK_Click);
            // 
            // mpanelMain
            // 
            this.mpanelMain.Controls.Add(this.mtabControlOptions);
            this.mpanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mpanelMain.Location = new System.Drawing.Point(0, 0);
            this.mpanelMain.Name = "mpanelMain";
            this.mpanelMain.Size = new System.Drawing.Size(488, 655);
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
            this.mtabControlOptions.Size = new System.Drawing.Size(488, 655);
            this.mtabControlOptions.TabIndex = 8;
            // 
            // mtabPageAlignmentOptions
            // 
            this.mtabPageAlignmentOptions.BackColor = System.Drawing.Color.White;
            this.mtabPageAlignmentOptions.Controls.Add(this.description);
            this.mtabPageAlignmentOptions.Controls.Add(this.groupBox4);
            this.mtabPageAlignmentOptions.Controls.Add(this.groupBox3);
            this.mtabPageAlignmentOptions.Controls.Add(this.groupBoxAlignmentType);
            this.mtabPageAlignmentOptions.Controls.Add(this.groupBoxPredictionAlgo);
            this.mtabPageAlignmentOptions.Controls.Add(this.groupBox1);
            this.mtabPageAlignmentOptions.Location = new System.Drawing.Point(4, 22);
            this.mtabPageAlignmentOptions.Name = "mtabPageAlignmentOptions";
            this.mtabPageAlignmentOptions.Size = new System.Drawing.Size(480, 629);
            this.mtabPageAlignmentOptions.TabIndex = 0;
            this.mtabPageAlignmentOptions.Text = "Alignment Options";
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.BackColor = System.Drawing.Color.White;
            this.description.Location = new System.Drawing.Point(10, 505);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(466, 123);
            this.description.TabIndex = 11;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.BackColor = System.Drawing.Color.White;
            this.groupBox4.Controls.Add(this.mtextBoxMaxLogEValForAlignment);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(8, 157);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(469, 50);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "X!Tandem Filters";
            // 
            // mtextBoxMaxLogEValForAlignment
            // 
            this.mtextBoxMaxLogEValForAlignment.Location = new System.Drawing.Point(231, 19);
            this.mtextBoxMaxLogEValForAlignment.Name = "mtextBoxMaxLogEValForAlignment";
            this.mtextBoxMaxLogEValForAlignment.Size = new System.Drawing.Size(66, 20);
            this.mtextBoxMaxLogEValForAlignment.TabIndex = 1;
            this.mtextBoxMaxLogEValForAlignment.Text = "-2";
            this.mtextBoxMaxLogEValForAlignment.MouseHover += new System.EventHandler(this.mtextBoxMaxLogEValForAlignment_MouseHover);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "Max log(EVal) for alignment:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.Controls.Add(this.mtextBoxMinXCorrForAlignment);
            this.groupBox3.Controls.Add(this.labelMinXCorr);
            this.groupBox3.Location = new System.Drawing.Point(8, 101);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(469, 50);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SEQUEST Filters";
            // 
            // mtextBoxMinXCorrForAlignment
            // 
            this.mtextBoxMinXCorrForAlignment.Location = new System.Drawing.Point(231, 16);
            this.mtextBoxMinXCorrForAlignment.Name = "mtextBoxMinXCorrForAlignment";
            this.mtextBoxMinXCorrForAlignment.Size = new System.Drawing.Size(66, 20);
            this.mtextBoxMinXCorrForAlignment.TabIndex = 1;
            this.mtextBoxMinXCorrForAlignment.Text = "3";
            this.mtextBoxMinXCorrForAlignment.MouseHover += new System.EventHandler(this.mtextBoxMinXCorrForAlignment_MouseHover);
            // 
            // labelMinXCorr
            // 
            this.labelMinXCorr.Location = new System.Drawing.Point(8, 16);
            this.labelMinXCorr.Name = "labelMinXCorr";
            this.labelMinXCorr.Size = new System.Drawing.Size(212, 24);
            this.labelMinXCorr.TabIndex = 0;
            this.labelMinXCorr.Text = "Minimum SEQUEST XCorr for alignment";
            this.labelMinXCorr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxAlignmentType
            // 
            this.groupBoxAlignmentType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAlignmentType.BackColor = System.Drawing.Color.White;
            this.groupBoxAlignmentType.Controls.Add(this.mtextBoxRegressionOrder);
            this.groupBoxAlignmentType.Controls.Add(this.mlabelRegressionOrder);
            this.groupBoxAlignmentType.Controls.Add(this.mradioButtonMixtureRegression);
            this.groupBoxAlignmentType.Controls.Add(this.mradioButtonLinearEM);
            this.groupBoxAlignmentType.Location = new System.Drawing.Point(8, 213);
            this.groupBoxAlignmentType.Name = "groupBoxAlignmentType";
            this.groupBoxAlignmentType.Size = new System.Drawing.Size(469, 88);
            this.groupBoxAlignmentType.TabIndex = 7;
            this.groupBoxAlignmentType.TabStop = false;
            this.groupBoxAlignmentType.Text = "Alignment Type";
            // 
            // mtextBoxRegressionOrder
            // 
            this.mtextBoxRegressionOrder.Location = new System.Drawing.Point(231, 62);
            this.mtextBoxRegressionOrder.Name = "mtextBoxRegressionOrder";
            this.mtextBoxRegressionOrder.Size = new System.Drawing.Size(66, 20);
            this.mtextBoxRegressionOrder.TabIndex = 2;
            this.mtextBoxRegressionOrder.Text = "1";
            this.mtextBoxRegressionOrder.MouseHover += new System.EventHandler(this.mtextBoxRegressionOrder_MouseHover);
            // 
            // mlabelRegressionOrder
            // 
            this.mlabelRegressionOrder.Location = new System.Drawing.Point(26, 59);
            this.mlabelRegressionOrder.Name = "mlabelRegressionOrder";
            this.mlabelRegressionOrder.Size = new System.Drawing.Size(80, 24);
            this.mlabelRegressionOrder.TabIndex = 0;
            this.mlabelRegressionOrder.Text = "Order:";
            this.mlabelRegressionOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mradioButtonMixtureRegression
            // 
            this.mradioButtonMixtureRegression.Checked = true;
            this.mradioButtonMixtureRegression.Location = new System.Drawing.Point(6, 43);
            this.mradioButtonMixtureRegression.Name = "mradioButtonMixtureRegression";
            this.mradioButtonMixtureRegression.Size = new System.Drawing.Size(138, 16);
            this.mradioButtonMixtureRegression.TabIndex = 2;
            this.mradioButtonMixtureRegression.TabStop = true;
            this.mradioButtonMixtureRegression.Text = "Mixture Regression";
            this.mradioButtonMixtureRegression.MouseHover += new System.EventHandler(this.mradioButtonMixtureRegression_MouseHover);
            // 
            // mradioButtonLinearEM
            // 
            this.mradioButtonLinearEM.Location = new System.Drawing.Point(6, 23);
            this.mradioButtonLinearEM.Name = "mradioButtonLinearEM";
            this.mradioButtonLinearEM.Size = new System.Drawing.Size(138, 16);
            this.mradioButtonLinearEM.TabIndex = 1;
            this.mradioButtonLinearEM.Text = "Linear ";
            this.mradioButtonLinearEM.MouseHover += new System.EventHandler(this.mradioButtonLinearEM_MouseHover);
            // 
            // groupBoxPredictionAlgo
            // 
            this.groupBoxPredictionAlgo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPredictionAlgo.BackColor = System.Drawing.Color.White;
            this.groupBoxPredictionAlgo.Controls.Add(this.mradioButtonKrokhin);
            this.groupBoxPredictionAlgo.Controls.Add(this.txtPredictionAlgorithmReference);
            this.groupBoxPredictionAlgo.Controls.Add(this.mradioButtonKangas);
            this.groupBoxPredictionAlgo.Location = new System.Drawing.Point(8, 307);
            this.groupBoxPredictionAlgo.Name = "groupBoxPredictionAlgo";
            this.groupBoxPredictionAlgo.Size = new System.Drawing.Size(469, 191);
            this.groupBoxPredictionAlgo.TabIndex = 8;
            this.groupBoxPredictionAlgo.TabStop = false;
            this.groupBoxPredictionAlgo.Text = "Prediction Algorithm";
            // 
            // mradioButtonKrokhin
            // 
            this.mradioButtonKrokhin.Location = new System.Drawing.Point(6, 48);
            this.mradioButtonKrokhin.Name = "mradioButtonKrokhin";
            this.mradioButtonKrokhin.Size = new System.Drawing.Size(138, 16);
            this.mradioButtonKrokhin.TabIndex = 2;
            this.mradioButtonKrokhin.Text = "Krokhin";
            this.mradioButtonKrokhin.CheckedChanged += new System.EventHandler(this.mradioButtonKrokhin_CheckedChanged);
            this.mradioButtonKrokhin.MouseHover += new System.EventHandler(this.mradioButtonKrokhin_MouseHover);
            // 
            // txtPredictionAlgorithmReference
            // 
            this.txtPredictionAlgorithmReference.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPredictionAlgorithmReference.BackColor = System.Drawing.Color.White;
            this.txtPredictionAlgorithmReference.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPredictionAlgorithmReference.Location = new System.Drawing.Point(10, 80);
            this.txtPredictionAlgorithmReference.Multiline = true;
            this.txtPredictionAlgorithmReference.Name = "txtPredictionAlgorithmReference";
            this.txtPredictionAlgorithmReference.ReadOnly = true;
            this.txtPredictionAlgorithmReference.Size = new System.Drawing.Size(443, 105);
            this.txtPredictionAlgorithmReference.TabIndex = 9;
            this.txtPredictionAlgorithmReference.Text = "Prediction Algorithm Reference";
            // 
            // mradioButtonKangas
            // 
            this.mradioButtonKangas.Checked = true;
            this.mradioButtonKangas.Location = new System.Drawing.Point(6, 23);
            this.mradioButtonKangas.Name = "mradioButtonKangas";
            this.mradioButtonKangas.Size = new System.Drawing.Size(138, 16);
            this.mradioButtonKangas.TabIndex = 1;
            this.mradioButtonKangas.TabStop = true;
            this.mradioButtonKangas.Text = "Kangas ANN";
            this.mradioButtonKangas.CheckedChanged += new System.EventHandler(this.mradioButtonKangas_CheckedChanged);
            this.mradioButtonKangas.MouseHover += new System.EventHandler(this.mradioButtonKangas_MouseHover);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.labelMinObservations);
            this.groupBox1.Controls.Add(this.mtextBoxMinObservations);
            this.groupBox1.Controls.Add(this.mtextBoxModifications);
            this.groupBox1.Controls.Add(this.labelModifications);
            this.groupBox1.Location = new System.Drawing.Point(8, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(469, 79);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Peptides";
            // 
            // labelMinObservations
            // 
            this.labelMinObservations.Location = new System.Drawing.Point(9, 42);
            this.labelMinObservations.Name = "labelMinObservations";
            this.labelMinObservations.Size = new System.Drawing.Size(152, 24);
            this.labelMinObservations.TabIndex = 0;
            this.labelMinObservations.Text = "Minimum Observations:";
            this.labelMinObservations.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mtextBoxMinObservations
            // 
            this.mtextBoxMinObservations.Location = new System.Drawing.Point(231, 45);
            this.mtextBoxMinObservations.Name = "mtextBoxMinObservations";
            this.mtextBoxMinObservations.Size = new System.Drawing.Size(66, 20);
            this.mtextBoxMinObservations.TabIndex = 1;
            this.mtextBoxMinObservations.Text = "3";
            this.mtextBoxMinObservations.MouseHover += new System.EventHandler(this.mtextBoxMinObservations_MouseHover);
            // 
            // mtextBoxModifications
            // 
            this.mtextBoxModifications.Location = new System.Drawing.Point(231, 19);
            this.mtextBoxModifications.Name = "mtextBoxModifications";
            this.mtextBoxModifications.Size = new System.Drawing.Size(66, 20);
            this.mtextBoxModifications.TabIndex = 1;
            this.mtextBoxModifications.Text = "2";
            this.mtextBoxModifications.MouseHover += new System.EventHandler(this.mtextBoxModifications_MouseHover);
            // 
            // labelModifications
            // 
            this.labelModifications.Location = new System.Drawing.Point(9, 16);
            this.labelModifications.Name = "labelModifications";
            this.labelModifications.Size = new System.Drawing.Size(216, 24);
            this.labelModifications.TabIndex = 0;
            this.labelModifications.Text = "Maximum Modifications For Alignment";
            this.labelModifications.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mtabPageExportOptions
            // 
            this.mtabPageExportOptions.Controls.Add(this.groupBox5);
            this.mtabPageExportOptions.Location = new System.Drawing.Point(4, 22);
            this.mtabPageExportOptions.Name = "mtabPageExportOptions";
            this.mtabPageExportOptions.Size = new System.Drawing.Size(480, 629);
            this.mtabPageExportOptions.TabIndex = 1;
            this.mtabPageExportOptions.Text = "Export Options";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.White;
            this.groupBox5.Controls.Add(this.groupBoxXTandem);
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Controls.Add(this.mgroupBoxDelCN);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(480, 629);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Rules";
            // 
            // groupBoxXTandem
            // 
            this.groupBoxXTandem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxXTandem.BackColor = System.Drawing.Color.White;
            this.groupBoxXTandem.Controls.Add(this.labelMaxLogEValForExport);
            this.groupBoxXTandem.Controls.Add(this.mtextBoxMaxLogEvalForExport);
            this.groupBoxXTandem.Location = new System.Drawing.Point(14, 395);
            this.groupBoxXTandem.Name = "groupBoxXTandem";
            this.groupBoxXTandem.Size = new System.Drawing.Size(458, 194);
            this.groupBoxXTandem.TabIndex = 14;
            this.groupBoxXTandem.TabStop = false;
            this.groupBoxXTandem.Text = "X!Tandem";
            // 
            // labelMaxLogEValForExport
            // 
            this.labelMaxLogEValForExport.Location = new System.Drawing.Point(39, 20);
            this.labelMaxLogEValForExport.Name = "labelMaxLogEValForExport";
            this.labelMaxLogEValForExport.Size = new System.Drawing.Size(136, 24);
            this.labelMaxLogEValForExport.TabIndex = 0;
            this.labelMaxLogEValForExport.Text = "Max log(EVal) for export:";
            this.labelMaxLogEValForExport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mtextBoxMaxLogEvalForExport
            // 
            this.mtextBoxMaxLogEvalForExport.Location = new System.Drawing.Point(176, 23);
            this.mtextBoxMaxLogEvalForExport.Name = "mtextBoxMaxLogEvalForExport";
            this.mtextBoxMaxLogEvalForExport.Size = new System.Drawing.Size(38, 20);
            this.mtextBoxMaxLogEvalForExport.TabIndex = 1;
            this.mtextBoxMaxLogEvalForExport.Text = "-2";
            this.mtextBoxMaxLogEvalForExport.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.mgroupBoxXCorrThresholdsNonTryptic);
            this.groupBox2.Controls.Add(this.mcheckBoxNonTryptic);
            this.groupBox2.Controls.Add(this.mcheckBoxPartially);
            this.groupBox2.Controls.Add(this.mgroupBoxXCorrThresholds);
            this.groupBox2.Controls.Add(this.mcheckBoxTryptic);
            this.groupBox2.Controls.Add(this.mgroupBoxXCorrThresholdsPartiallyTryptic);
            this.groupBox2.Location = new System.Drawing.Point(14, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(458, 285);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tryptic State ";
            // 
            // mgroupBoxXCorrThresholdsNonTryptic
            // 
            this.mgroupBoxXCorrThresholdsNonTryptic.Controls.Add(this.mtextBoxMinXCorrForExportNonTrypticsCS3);
            this.mgroupBoxXCorrThresholdsNonTryptic.Controls.Add(this.labelXCorrForExportNonTrytpicsCS3);
            this.mgroupBoxXCorrThresholdsNonTryptic.Controls.Add(this.mtextBoxMinXCorrForExportNonTrypticsCS2);
            this.mgroupBoxXCorrThresholdsNonTryptic.Controls.Add(this.labelXCorrForExportNonTrytpicsCS2);
            this.mgroupBoxXCorrThresholdsNonTryptic.Controls.Add(this.mtextBoxMinXCorrForExportNonTrypticsCS1);
            this.mgroupBoxXCorrThresholdsNonTryptic.Controls.Add(this.labelXCorrForExportNonTrytpicsCS1);
            this.mgroupBoxXCorrThresholdsNonTryptic.Location = new System.Drawing.Point(24, 214);
            this.mgroupBoxXCorrThresholdsNonTryptic.Name = "mgroupBoxXCorrThresholdsNonTryptic";
            this.mgroupBoxXCorrThresholdsNonTryptic.Size = new System.Drawing.Size(268, 60);
            this.mgroupBoxXCorrThresholdsNonTryptic.TabIndex = 17;
            this.mgroupBoxXCorrThresholdsNonTryptic.TabStop = false;
            this.mgroupBoxXCorrThresholdsNonTryptic.MouseHover += new System.EventHandler(this.mgroupBoxXCorrThresholdsNonTryptic_MouseHover);
            // 
            // mtextBoxMinXCorrForExportNonTrypticsCS3
            // 
            this.mtextBoxMinXCorrForExportNonTrypticsCS3.Location = new System.Drawing.Point(223, 11);
            this.mtextBoxMinXCorrForExportNonTrypticsCS3.Name = "mtextBoxMinXCorrForExportNonTrypticsCS3";
            this.mtextBoxMinXCorrForExportNonTrypticsCS3.Size = new System.Drawing.Size(38, 20);
            this.mtextBoxMinXCorrForExportNonTrypticsCS3.TabIndex = 1;
            this.mtextBoxMinXCorrForExportNonTrypticsCS3.Text = "3";
            this.mtextBoxMinXCorrForExportNonTrypticsCS3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelXCorrForExportNonTrytpicsCS3
            // 
            this.labelXCorrForExportNonTrytpicsCS3.Location = new System.Drawing.Point(151, 11);
            this.labelXCorrForExportNonTrytpicsCS3.Name = "labelXCorrForExportNonTrytpicsCS3";
            this.labelXCorrForExportNonTrytpicsCS3.Size = new System.Drawing.Size(72, 21);
            this.labelXCorrForExportNonTrytpicsCS3.TabIndex = 0;
            this.labelXCorrForExportNonTrytpicsCS3.Text = "Charge >=3:";
            this.labelXCorrForExportNonTrytpicsCS3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mtextBoxMinXCorrForExportNonTrypticsCS2
            // 
            this.mtextBoxMinXCorrForExportNonTrypticsCS2.Location = new System.Drawing.Point(78, 34);
            this.mtextBoxMinXCorrForExportNonTrypticsCS2.Name = "mtextBoxMinXCorrForExportNonTrypticsCS2";
            this.mtextBoxMinXCorrForExportNonTrypticsCS2.Size = new System.Drawing.Size(40, 20);
            this.mtextBoxMinXCorrForExportNonTrypticsCS2.TabIndex = 1;
            this.mtextBoxMinXCorrForExportNonTrypticsCS2.Text = "3";
            this.mtextBoxMinXCorrForExportNonTrypticsCS2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelXCorrForExportNonTrytpicsCS2
            // 
            this.labelXCorrForExportNonTrytpicsCS2.Location = new System.Drawing.Point(14, 34);
            this.labelXCorrForExportNonTrytpicsCS2.Name = "labelXCorrForExportNonTrytpicsCS2";
            this.labelXCorrForExportNonTrytpicsCS2.Size = new System.Drawing.Size(64, 21);
            this.labelXCorrForExportNonTrytpicsCS2.TabIndex = 0;
            this.labelXCorrForExportNonTrytpicsCS2.Text = "Charge 2:";
            this.labelXCorrForExportNonTrytpicsCS2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mtextBoxMinXCorrForExportNonTrypticsCS1
            // 
            this.mtextBoxMinXCorrForExportNonTrypticsCS1.Location = new System.Drawing.Point(78, 11);
            this.mtextBoxMinXCorrForExportNonTrypticsCS1.Name = "mtextBoxMinXCorrForExportNonTrypticsCS1";
            this.mtextBoxMinXCorrForExportNonTrypticsCS1.Size = new System.Drawing.Size(40, 20);
            this.mtextBoxMinXCorrForExportNonTrypticsCS1.TabIndex = 1;
            this.mtextBoxMinXCorrForExportNonTrypticsCS1.Text = "3";
            this.mtextBoxMinXCorrForExportNonTrypticsCS1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelXCorrForExportNonTrytpicsCS1
            // 
            this.labelXCorrForExportNonTrytpicsCS1.Location = new System.Drawing.Point(14, 10);
            this.labelXCorrForExportNonTrytpicsCS1.Name = "labelXCorrForExportNonTrytpicsCS1";
            this.labelXCorrForExportNonTrytpicsCS1.Size = new System.Drawing.Size(64, 21);
            this.labelXCorrForExportNonTrytpicsCS1.TabIndex = 0;
            this.labelXCorrForExportNonTrytpicsCS1.Text = "Charge 1:";
            this.labelXCorrForExportNonTrytpicsCS1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mcheckBoxNonTryptic
            // 
            this.mcheckBoxNonTryptic.Location = new System.Drawing.Point(2, 197);
            this.mcheckBoxNonTryptic.Name = "mcheckBoxNonTryptic";
            this.mcheckBoxNonTryptic.Size = new System.Drawing.Size(180, 21);
            this.mcheckBoxNonTryptic.TabIndex = 3;
            this.mcheckBoxNonTryptic.Text = "NonTryptic Peptides";
            // 
            // mcheckBoxPartially
            // 
            this.mcheckBoxPartially.Location = new System.Drawing.Point(3, 112);
            this.mcheckBoxPartially.Name = "mcheckBoxPartially";
            this.mcheckBoxPartially.Size = new System.Drawing.Size(289, 21);
            this.mcheckBoxPartially.TabIndex = 2;
            this.mcheckBoxPartially.Text = "Partially Tryptic Peptides";
            // 
            // mgroupBoxXCorrThresholds
            // 
            this.mgroupBoxXCorrThresholds.Controls.Add(this.mtextBoxMinXCorrForExportTrypticsCS3);
            this.mgroupBoxXCorrThresholds.Controls.Add(this.mtextBoxMinXCorrForExportTrypticsCS2);
            this.mgroupBoxXCorrThresholds.Controls.Add(this.labelXCorrForExportCS3);
            this.mgroupBoxXCorrThresholds.Controls.Add(this.mtextBoxMinXCorrForExportTrypticsCS1);
            this.mgroupBoxXCorrThresholds.Controls.Add(this.labelXCorrForExportCS2);
            this.mgroupBoxXCorrThresholds.Controls.Add(this.labelXCorrForExportCS1);
            this.mgroupBoxXCorrThresholds.Location = new System.Drawing.Point(24, 36);
            this.mgroupBoxXCorrThresholds.Name = "mgroupBoxXCorrThresholds";
            this.mgroupBoxXCorrThresholds.Size = new System.Drawing.Size(268, 74);
            this.mgroupBoxXCorrThresholds.TabIndex = 15;
            this.mgroupBoxXCorrThresholds.TabStop = false;
            this.mgroupBoxXCorrThresholds.MouseHover += new System.EventHandler(this.mgroupBoxXCorrThresholds_MouseHover);
            // 
            // mtextBoxMinXCorrForExportTrypticsCS3
            // 
            this.mtextBoxMinXCorrForExportTrypticsCS3.Enabled = ((bool)(configurationAppSettings.GetValue("mcheckBoxTryptic.Checked", typeof(bool))));
            this.mtextBoxMinXCorrForExportTrypticsCS3.Location = new System.Drawing.Point(223, 17);
            this.mtextBoxMinXCorrForExportTrypticsCS3.Name = "mtextBoxMinXCorrForExportTrypticsCS3";
            this.mtextBoxMinXCorrForExportTrypticsCS3.Size = new System.Drawing.Size(38, 20);
            this.mtextBoxMinXCorrForExportTrypticsCS3.TabIndex = 1;
            this.mtextBoxMinXCorrForExportTrypticsCS3.Text = "3";
            this.mtextBoxMinXCorrForExportTrypticsCS3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mtextBoxMinXCorrForExportTrypticsCS2
            // 
            this.mtextBoxMinXCorrForExportTrypticsCS2.Location = new System.Drawing.Point(78, 45);
            this.mtextBoxMinXCorrForExportTrypticsCS2.Name = "mtextBoxMinXCorrForExportTrypticsCS2";
            this.mtextBoxMinXCorrForExportTrypticsCS2.Size = new System.Drawing.Size(40, 20);
            this.mtextBoxMinXCorrForExportTrypticsCS2.TabIndex = 1;
            this.mtextBoxMinXCorrForExportTrypticsCS2.Text = "3";
            this.mtextBoxMinXCorrForExportTrypticsCS2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelXCorrForExportCS3
            // 
            this.labelXCorrForExportCS3.Location = new System.Drawing.Point(151, 18);
            this.labelXCorrForExportCS3.Name = "labelXCorrForExportCS3";
            this.labelXCorrForExportCS3.Size = new System.Drawing.Size(72, 20);
            this.labelXCorrForExportCS3.TabIndex = 0;
            this.labelXCorrForExportCS3.Text = "Charge >=3:";
            this.labelXCorrForExportCS3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mtextBoxMinXCorrForExportTrypticsCS1
            // 
            this.mtextBoxMinXCorrForExportTrypticsCS1.Location = new System.Drawing.Point(78, 18);
            this.mtextBoxMinXCorrForExportTrypticsCS1.Name = "mtextBoxMinXCorrForExportTrypticsCS1";
            this.mtextBoxMinXCorrForExportTrypticsCS1.Size = new System.Drawing.Size(40, 20);
            this.mtextBoxMinXCorrForExportTrypticsCS1.TabIndex = 1;
            this.mtextBoxMinXCorrForExportTrypticsCS1.Text = "3";
            this.mtextBoxMinXCorrForExportTrypticsCS1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelXCorrForExportCS2
            // 
            this.labelXCorrForExportCS2.Location = new System.Drawing.Point(14, 37);
            this.labelXCorrForExportCS2.Name = "labelXCorrForExportCS2";
            this.labelXCorrForExportCS2.Size = new System.Drawing.Size(59, 34);
            this.labelXCorrForExportCS2.TabIndex = 0;
            this.labelXCorrForExportCS2.Text = "Charge 2:";
            this.labelXCorrForExportCS2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelXCorrForExportCS1
            // 
            this.labelXCorrForExportCS1.Location = new System.Drawing.Point(14, 16);
            this.labelXCorrForExportCS1.Name = "labelXCorrForExportCS1";
            this.labelXCorrForExportCS1.Size = new System.Drawing.Size(64, 21);
            this.labelXCorrForExportCS1.TabIndex = 0;
            this.labelXCorrForExportCS1.Text = "Charge 1:";
            this.labelXCorrForExportCS1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mcheckBoxTryptic
            // 
            this.mcheckBoxTryptic.Checked = true;
            this.mcheckBoxTryptic.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mcheckBoxTryptic.Location = new System.Drawing.Point(12, 19);
            this.mcheckBoxTryptic.Name = "mcheckBoxTryptic";
            this.mcheckBoxTryptic.Size = new System.Drawing.Size(108, 21);
            this.mcheckBoxTryptic.TabIndex = 1;
            this.mcheckBoxTryptic.Text = "Tryptic Peptides";
            // 
            // mgroupBoxXCorrThresholdsPartiallyTryptic
            // 
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.Controls.Add(this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3);
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.Controls.Add(this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2);
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.Controls.Add(this.labelXCorrForExportPartiallyTrytpicsCS3);
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.Controls.Add(this.labelXCorrForExportPartiallyTrytpicsCS2);
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.Controls.Add(this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1);
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.Controls.Add(this.labelXCorrForExportPartiallyTrytpicsCS1);
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.Location = new System.Drawing.Point(24, 129);
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.Name = "mgroupBoxXCorrThresholdsPartiallyTryptic";
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.Size = new System.Drawing.Size(268, 62);
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.TabIndex = 16;
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.TabStop = false;
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.MouseHover += new System.EventHandler(this.groupBox4_MouseHover);
            // 
            // mtextBoxMinXCorrForExportPartiallyTrypticsCS3
            // 
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Location = new System.Drawing.Point(223, 16);
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Name = "mtextBoxMinXCorrForExportPartiallyTrypticsCS3";
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Size = new System.Drawing.Size(38, 20);
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.TabIndex = 1;
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.Text = "3";
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mtextBoxMinXCorrForExportPartiallyTrypticsCS2
            // 
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Location = new System.Drawing.Point(78, 36);
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Name = "mtextBoxMinXCorrForExportPartiallyTrypticsCS2";
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Size = new System.Drawing.Size(40, 20);
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.TabIndex = 1;
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.Text = "3";
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelXCorrForExportPartiallyTrytpicsCS3
            // 
            this.labelXCorrForExportPartiallyTrytpicsCS3.Location = new System.Drawing.Point(151, 14);
            this.labelXCorrForExportPartiallyTrytpicsCS3.Name = "labelXCorrForExportPartiallyTrytpicsCS3";
            this.labelXCorrForExportPartiallyTrytpicsCS3.Size = new System.Drawing.Size(72, 23);
            this.labelXCorrForExportPartiallyTrytpicsCS3.TabIndex = 0;
            this.labelXCorrForExportPartiallyTrytpicsCS3.Text = "Charge >=3:";
            this.labelXCorrForExportPartiallyTrytpicsCS3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelXCorrForExportPartiallyTrytpicsCS2
            // 
            this.labelXCorrForExportPartiallyTrytpicsCS2.Location = new System.Drawing.Point(14, 36);
            this.labelXCorrForExportPartiallyTrytpicsCS2.Name = "labelXCorrForExportPartiallyTrytpicsCS2";
            this.labelXCorrForExportPartiallyTrytpicsCS2.Size = new System.Drawing.Size(64, 23);
            this.labelXCorrForExportPartiallyTrytpicsCS2.TabIndex = 0;
            this.labelXCorrForExportPartiallyTrytpicsCS2.Text = "Charge 2:";
            this.labelXCorrForExportPartiallyTrytpicsCS2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mtextBoxMinXCorrForExportPartiallyTrypticsCS1
            // 
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Location = new System.Drawing.Point(78, 10);
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Name = "mtextBoxMinXCorrForExportPartiallyTrypticsCS1";
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Size = new System.Drawing.Size(40, 20);
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.TabIndex = 1;
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.Text = "3";
            this.mtextBoxMinXCorrForExportPartiallyTrypticsCS1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelXCorrForExportPartiallyTrytpicsCS1
            // 
            this.labelXCorrForExportPartiallyTrytpicsCS1.Location = new System.Drawing.Point(14, 10);
            this.labelXCorrForExportPartiallyTrytpicsCS1.Name = "labelXCorrForExportPartiallyTrytpicsCS1";
            this.labelXCorrForExportPartiallyTrytpicsCS1.Size = new System.Drawing.Size(64, 23);
            this.labelXCorrForExportPartiallyTrytpicsCS1.TabIndex = 0;
            this.labelXCorrForExportPartiallyTrytpicsCS1.Text = "Charge 1:";
            this.labelXCorrForExportPartiallyTrytpicsCS1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mgroupBoxDelCN
            // 
            this.mgroupBoxDelCN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mgroupBoxDelCN.BackColor = System.Drawing.Color.White;
            this.mgroupBoxDelCN.Controls.Add(this.mtextBoxDelCN);
            this.mgroupBoxDelCN.Controls.Add(this.mlabelDelCN);
            this.mgroupBoxDelCN.Controls.Add(this.mcheckBoxDelCN);
            this.mgroupBoxDelCN.Location = new System.Drawing.Point(14, 319);
            this.mgroupBoxDelCN.Name = "mgroupBoxDelCN";
            this.mgroupBoxDelCN.Size = new System.Drawing.Size(458, 69);
            this.mgroupBoxDelCN.TabIndex = 13;
            this.mgroupBoxDelCN.TabStop = false;
            this.mgroupBoxDelCN.Text = "SEQUEST DelCN";
            // 
            // mtextBoxDelCN
            // 
            this.mtextBoxDelCN.Location = new System.Drawing.Point(176, 43);
            this.mtextBoxDelCN.Name = "mtextBoxDelCN";
            this.mtextBoxDelCN.Size = new System.Drawing.Size(38, 20);
            this.mtextBoxDelCN.TabIndex = 2;
            this.mtextBoxDelCN.Text = "0.1";
            this.mtextBoxDelCN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mlabelDelCN
            // 
            this.mlabelDelCN.Location = new System.Drawing.Point(39, 43);
            this.mlabelDelCN.Name = "mlabelDelCN";
            this.mlabelDelCN.Size = new System.Drawing.Size(69, 21);
            this.mlabelDelCN.TabIndex = 1;
            this.mlabelDelCN.Text = "Min DelCN:";
            this.mlabelDelCN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mcheckBoxDelCN
            // 
            this.mcheckBoxDelCN.Location = new System.Drawing.Point(16, 19);
            this.mcheckBoxDelCN.Name = "mcheckBoxDelCN";
            this.mcheckBoxDelCN.Size = new System.Drawing.Size(144, 21);
            this.mcheckBoxDelCN.TabIndex = 0;
            this.mcheckBoxDelCN.Text = "Use DelCN Threshold";
            // 
            // frmOptions
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(488, 694);
            this.Controls.Add(this.mpanelMain);
            this.Controls.Add(this.panelOKCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmOptions";
            this.Text = "Options";
            this.panelOKCancel.ResumeLayout(false);
            this.mpanelMain.ResumeLayout(false);
            this.mtabControlOptions.ResumeLayout(false);
            this.mtabPageAlignmentOptions.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxAlignmentType.ResumeLayout(false);
            this.groupBoxAlignmentType.PerformLayout();
            this.groupBoxPredictionAlgo.ResumeLayout(false);
            this.groupBoxPredictionAlgo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.mtabPageExportOptions.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBoxXTandem.ResumeLayout(false);
            this.groupBoxXTandem.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.mgroupBoxXCorrThresholdsNonTryptic.ResumeLayout(false);
            this.mgroupBoxXCorrThresholdsNonTryptic.PerformLayout();
            this.mgroupBoxXCorrThresholds.ResumeLayout(false);
            this.mgroupBoxXCorrThresholds.PerformLayout();
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.ResumeLayout(false);
            this.mgroupBoxXCorrThresholdsPartiallyTryptic.PerformLayout();
            this.mgroupBoxDelCN.ResumeLayout(false);
            this.mgroupBoxDelCN.PerformLayout();
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
        public RegressionTypeIdentifier RegressionType
		{
			get
			{
				if (mradioButtonLinearEM.Checked)
					return RegressionTypeIdentifier.LINEAR_EM ; 
				else
                    return RegressionTypeIdentifier.MIXTURE_REGRESSION; 
			}
			set
			{
                if (value == RegressionTypeIdentifier.LINEAR_EM)
				{
					mradioButtonLinearEM.Checked = true ; 
				}
				else
				{
					mradioButtonMixtureRegression.Checked = true ; 
				}
			}
		}

		public Options Options
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
				MinObservationsForExport = value.MinObservationsForExport ; 
				//TODO: BLL Put the retention time predictor type back in!
				RegressionType  = value.Regression ; 
				RegressionOrder = value.RegressionOrder ; 
			}
			get
			{
				Options options = new Options() ; 

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
				options.MinObservationsForExport = MinObservationsForExport ; 				
                if (mradioButtonKangas.Checked)
                    options.PredictorType = Data.RetentionTimePredictionType.Kangas;
                else
                    options.PredictorType = Data.RetentionTimePredictionType.Krokhin;
				options.Regression = RegressionType ; 
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

		private void UpdatePredictionReference()
		{
			if (mradioButtonKangas.Checked) 
			{
				txtPredictionAlgorithmReference.Text = "Kangas ANN algorithm developed by Lars Kangas and Kostas Petritis.  See:  " +
					"K. Petritis, L.J. Kangas, P.L. Ferguson, G.A. Anderson, L. Paša-Tolic, M.S. Lipton, K.J. Auberry, " +
					"E.F. Strittmatter, Y. Shen, R. Zhao, R.D. Smith. 2003. \"Use of artificial neural networks for the " +
					"accurate prediction of peptide liquid chromatography elution times in proteome analyses\". " +
					"Analytical Chemistry, 75 (5) 1039-1048.";
			} 
			else
			{
				txtPredictionAlgorithmReference.Text = "Prediction algorithm developed by Oleg Krokhin.  See: " + 
				    "O.V. Krokhin, R. Craig, V. Spicer, W. Ens, K.G. Standing, R.C. Beavis, J.A. Wilkins. 2004. " + 
					"\"An improved model for prediction of retention times of tryptic peptides in ion pair reversed-phase HPLC " + 
                    "- Its application to protein peptide mapping by off-line HPLC-MALDI MS\". " +
                   "Molecular & Cellular Proteomics, 3 (9) 908-919.";
			}

		}

		private void mtextBoxMaxLogEValForAlignment_MouseHover(object sender, System.EventArgs e)
		{
			description.Text = "Specifies the maximum log(eval) that a match from X!Tandem can have for it to be used in the alignment" ;		
		}

		private void mtextBoxModifications_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "For datasets, this specifies the maximum number of modifications a peptide can have for it to be used in the alignment";						
		}

		private void mtextBoxMaxRank_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Maximum Rank of a Match for Loading";										
		}

		private void mtextBoxMinObservations_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Specifies the minimum number of observations needed for a mass tag to be exported into the database";
		}

		private void mtextBoxMinXCorrForAlignment_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "For SEQUEST results, this specifies the minimum XCorr a match can have for it to be used in the alignment";
		}

		private void mradioButtonKangas_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Use Prediction by Kangas to compute NET for a sequence.";
		}

		private void mradioButtonKrokhin_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Use Prediction by Krokhin to compute NET for a sequence.";
		}

		private void mradioButtonLinearEM_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Check to use Least Squares Linear Fit between Scan and NET"; 		
		}

		private void mradioButtonMixtureRegression_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Check to use Mixture Model Regression Fit between Scan and NET"; 				
		}

		private void mtextBoxRegressionOrder_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Specify order of Mixture Model Regression Fit between Scan and NET"; 				
		}

		private void mgroupBoxXCorrThresholds_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Specify threshold XCorr scores for different charges, for matches to be exported to database"; 
		}

		private void groupBox4_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Specify threshold XCorr scores for different charges of partially tryptic peptides, for matches to be exported to database"; 		
		}

		private void mgroupBoxXCorrThresholdsNonTryptic_MouseHover(object sender, System.EventArgs e)
		{
            description.Text = "Specify threshold XCorr scores for different charges of non tryptic peptides, for matches to be exported to database"; 				
		}

		private void mradioButtonKangas_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdatePredictionReference();
		}

		private void mradioButtonKrokhin_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdatePredictionReference();
		}


	}
}
