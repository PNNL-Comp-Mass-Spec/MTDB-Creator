using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MTDBCreator
{

    public delegate void DelegateSetErrorMessage(string strError);
    public delegate void DelegateSetStatusMessage(string strStatus);
    public delegate void DelegateSetPercentComplete(int percentDone); 

	/// <summary>
	/// Summary description for frmStatus.
	/// </summary>
	public class frmStatus : System.Windows.Forms.Form
	{

        private delegate void SetControlString(string strMessage);
        private delegate void SetProgressValue(int complete);
        public event EventHandler CancelPressed;

        #region Members 
        private System.Windows.Forms.Label lblProgress;
		private System.Windows.Forms.ProgressBar mbar_progress;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button mbtn_cancel;
        private System.Windows.Forms.Label mlbl_status;
        private int mint_percent_done = 0;
        private int mint_step_size = 2;
        private System.ComponentModel.Container components = null;
        private ProgressBar m_progressTotal;
        private Label label3;

        /// <summary>
        /// Flag indicating there are errors.
        /// </summary>
        private bool m_hasErrors;
        #endregion


		public frmStatus()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            mbar_progress.Minimum   = 0 ; 
			mbar_progress.Maximum   = 100 ; 
			mbar_progress.Step      = mint_step_size ; 
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
            this.lblProgress = new System.Windows.Forms.Label();
            this.mbar_progress = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.mbtn_cancel = new System.Windows.Forms.Button();
            this.mlbl_status = new System.Windows.Forms.Label();
            this.m_progressTotal = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblProgress
            // 
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(10, 90);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(72, 20);
            this.lblProgress.TabIndex = 0;
            this.lblProgress.Text = "Progress:";
            // 
            // mbar_progress
            // 
            this.mbar_progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mbar_progress.ForeColor = System.Drawing.Color.Green;
            this.mbar_progress.Location = new System.Drawing.Point(88, 90);
            this.mbar_progress.Name = "mbar_progress";
            this.mbar_progress.Size = new System.Drawing.Size(483, 20);
            this.mbar_progress.Step = 2;
            this.mbar_progress.TabIndex = 1;
            this.mbar_progress.Value = 2;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Status:";
            // 
            // mbtn_cancel
            // 
            this.mbtn_cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.mbtn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mbtn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mbtn_cancel.Location = new System.Drawing.Point(252, 127);
            this.mbtn_cancel.Name = "mbtn_cancel";
            this.mbtn_cancel.Size = new System.Drawing.Size(80, 24);
            this.mbtn_cancel.TabIndex = 3;
            this.mbtn_cancel.Text = "Cancel";
            this.mbtn_cancel.Click += new System.EventHandler(this.mbtn_cancel_Click);
            // 
            // mlbl_status
            // 
            this.mlbl_status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mlbl_status.Location = new System.Drawing.Point(88, 50);
            this.mlbl_status.Name = "mlbl_status";
            this.mlbl_status.Size = new System.Drawing.Size(483, 37);
            this.mlbl_status.TabIndex = 4;
            // 
            // m_progressTotal
            // 
            this.m_progressTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_progressTotal.ForeColor = System.Drawing.Color.Green;
            this.m_progressTotal.Location = new System.Drawing.Point(88, 12);
            this.m_progressTotal.Name = "m_progressTotal";
            this.m_progressTotal.Size = new System.Drawing.Size(483, 20);
            this.m_progressTotal.Step = 2;
            this.m_progressTotal.TabIndex = 11;
            this.m_progressTotal.Value = 4;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 29);
            this.label3.TabIndex = 12;
            this.label3.Text = "Total Progress:";
            // 
            // frmStatus
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.mbtn_cancel;
            this.ClientSize = new System.Drawing.Size(579, 169);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_progressTotal);
            this.Controls.Add(this.mbar_progress);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mlbl_status);
            this.Controls.Add(this.mbtn_cancel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStatus";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Status";
            this.TopMost = true;
            this.ResumeLayout(false);

		}
		#endregion

		public void ClearErrorMessages() 
		{
            m_hasErrors = false;
		}

		public bool HasErrorMessages
		{
			get
			{
                return m_hasErrors;
			}
		}

		public void SetPrecentComplete(int percent_done)
		{
			if (!IsHandleCreated)
				return ; 
			// if the new status is greater, or if its been reset to a newer value then update
			if (mint_percent_done + mint_step_size < percent_done || mint_percent_done > percent_done)
			{
				mint_percent_done = percent_done ; 
				Invoke(new SetProgressValue(SetProgressVal), new object [] {percent_done}) ; 
			}					
		}


        public void SetTotalPrecentComplete(int percent_done)
        {           
            Invoke(new SetProgressValue(SetTotalProgressVal), new object[] { percent_done });       
        }

		/// <summary>
		/// Prevent this form from ever being closed.  It can be hidden/shown, but not closed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(CancelEventArgs e)
		{
			try
			{                
				base.OnClosing(e);
				e.Cancel = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + ex.StackTrace) ; 
			}
		}


        private void SetProgressVal(int percent)
        {
            mbar_progress.Value = percent;
        }
        private void SetTotalProgressVal(int percent)
        {
            m_progressTotal.Value = percent;
        }

		public void Reset()
		{
			try
			{
				mint_percent_done = 0 ;
                SetTotalProgressVal(0);
				SetProgressVal(0) ;

				this.ClearErrorMessages() ;				
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + ex.StackTrace) ; 
			}
		}

		public void SetErrorMessage(string strError)
		{
			try
			{
				if (!IsHandleCreated)
					return ; 
				Invoke(new SetControlString(this.SetErrorString), new object [] {strError}) ; 
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString()) ; 
			}
		}
		
		public void SetStatusMessage(string strStatus)
		{
			try
			{
				if (!IsHandleCreated)
					return ; 

				Invoke(new SetControlString(this.SetStatusString), new object [] {strStatus}) ; 
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString()) ; 
			}
		}

		private void SetErrorString(string message)
		{
            this.SetStatusMessage(message);
            m_hasErrors = true;			
		}
        
		private void SetStatusString(string strMessage)
		{
			try
			{
				this.mlbl_status.Text = strMessage ; 
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + ex.StackTrace) ; 
			}
		}


		private void mbtn_cancel_Click(object sender, System.EventArgs e)
		{
            // The user should know about this!
            if (CancelPressed != null)
                CancelPressed(this, e);

			this.DialogResult = DialogResult.Cancel;
			this.Hide();			
		}

		private void mbtn_close_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Hide();		
		}
	}
}
