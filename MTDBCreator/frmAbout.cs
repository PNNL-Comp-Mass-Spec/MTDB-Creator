using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MTDBCreator
{
	/// <summary>
	/// Summary description for frmAbout.
	/// </summary>
	public class frmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button mbuttonOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel mlinkLabelDownload;
		private System.Windows.Forms.RichTextBox richTextBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			InitializeAboutBox() ;
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
			this.label1 = new System.Windows.Forms.Label();
			this.mbuttonOK = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.mlinkLabelDownload = new System.Windows.Forms.LinkLabel();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(608, 40);
			this.label1.TabIndex = 0;
			this.label1.Text = "MTDBCreator";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// mbuttonOK
			// 
			this.mbuttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mbuttonOK.Location = new System.Drawing.Point(224, 370);
			this.mbuttonOK.Name = "mbuttonOK";
			this.mbuttonOK.Size = new System.Drawing.Size(104, 24);
			this.mbuttonOK.TabIndex = 3;
			this.mbuttonOK.Text = "OK";
			this.mbuttonOK.Click += new System.EventHandler(this.mbuttonOK_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(256, 32);
			this.label2.TabIndex = 4;
			this.label2.Text = "Download Latest version from: ";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// mlinkLabelDownload
			// 
			this.mlinkLabelDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.mlinkLabelDownload.Location = new System.Drawing.Point(296, 48);
			this.mlinkLabelDownload.Name = "mlinkLabelDownload";
			this.mlinkLabelDownload.Size = new System.Drawing.Size(280, 32);
			this.mlinkLabelDownload.TabIndex = 5;
			this.mlinkLabelDownload.TabStop = true;
			this.mlinkLabelDownload.Text = "http://omics.pnl.gov/software";
			this.mlinkLabelDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// richTextBox1
			// 
			this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox1.Location = new System.Drawing.Point(8, 88);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(578, 266);
			this.richTextBox1.TabIndex = 6;
			this.richTextBox1.Text = "richTextBox1";
			// 
			// frmAbout
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(608, 408);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.mlinkLabelDownload);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.mbuttonOK);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmAbout";
			this.Text = "frmAbout";
			this.ResumeLayout(false);

		}
		#endregion

		private void InitializeAboutBox()
		{
			string strAbout ;

			strAbout = "Program written by Deep Jaitly and maintained by Matthew Monroe for PNNL's National Center for Research Resources and the Department of Energy (PNNL, Richland, WA). \n\n" ;
			

			strAbout += "Program date: " + MTDBCreator.frmMain.PROGRAM_DATE ;
			#if BASIC 
				strAbout += " (Public Distribution Version)\n" ;
			#else
				strAbout += "\n" ;
			#endif 

			strAbout += "E-mail: matthew.monroe@pnl.gov or proteomics@pnl.gov \n" +
					    "Website: http://ncrr.pnl.gov/ or http://omics.pnl.gov" + "\n\n" ;

			strAbout += "For information on the AMT Tag approach and algorithms, please see: \"Advances in Proteomics Data Analysis and Display Using an Accurate Mass and Time Tag Approach,\" J.D. Zimmer, M.E. Monroe, W.J. Qian, and R.D. Smith. Mass Spectrometry Reviews, 25, 450-482 (2006).\n\n" ;

			#if !BASIC 
			strAbout += "The methods embodied in this software to derive the Kangas/Petritis retention time " +
                "prediction values are covered by U.S. patent 7,136,759 and pending patent 2005-0267688A1.  " +
                "The software is made available solely for non-commercial research purposes on an " +
                "\"as is\" basis by Battelle Memorial Institute.  If rights to deploy and distribute  " +
                "the code for commercial purposes are of interest, please contact Bruce Harrer " +
                "at bruce.harrer@pnl.gov\n\n";
			#endif

			strAbout += "Licensed under the Apache License, Version 2.0; you may not use this file except in compliance with the License.  " +
                        "You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0" + "\n\n" ;
            

            strAbout += "Notice: This computer software was prepared by Battelle Memorial Institute, " +
                              "hereinafter the Contractor, under Contract No. DE-AC05-76RL0 1830 with the " +
                              "Department of Energy (DOE).  All rights in the computer software are reserved " +
                              "by DOE on behalf of the United States Government and the Contractor as " +
                              "provided in the Contract.  NEITHER THE GOVERNMENT NOR THE CONTRACTOR MAKES ANY " +
                              "WARRANTY, EXPRESS OR IMPLIED, OR ASSUMES ANY LIABILITY FOR THE USE OF THIS " +
                              "SOFTWARE.  This notice including this sentence must appear on any copies of " +
                              "this computer software." ;
		
			richTextBox1.Text = strAbout ;

		}

		private void mbuttonOK_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK ; 
			this.Hide() ;

		}

	}
}
