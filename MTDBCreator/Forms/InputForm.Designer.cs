namespace MTDBCreator.Forms
{
    partial class InputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputForm));
            this.mbuttonOK = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.mbutton_addXtandem = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.mbutton_addSequest = new System.Windows.Forms.Button();
            this.mbutton_addPhrp = new System.Windows.Forms.Button();
            this.listViewDatasets = new System.Windows.Forms.ListView();
            this.columnHeaderDataset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // mbuttonOK
            // 
            this.mbuttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.mbuttonOK.BackColor = System.Drawing.Color.White;
            this.mbuttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.mbuttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbuttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mbuttonOK.ForeColor = System.Drawing.Color.Black;
            this.mbuttonOK.Location = new System.Drawing.Point(699, 556);
            this.mbuttonOK.Name = "mbuttonOK";
            this.mbuttonOK.Size = new System.Drawing.Size(113, 41);
            this.mbuttonOK.TabIndex = 4;
            this.mbuttonOK.Text = "OK";
            this.mbuttonOK.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(818, 556);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 41);
            this.button1.TabIndex = 5;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // mbutton_addXtandem
            // 
            this.mbutton_addXtandem.BackColor = System.Drawing.Color.White;
            this.mbutton_addXtandem.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.mbutton_addXtandem.FlatAppearance.BorderSize = 2;
            this.mbutton_addXtandem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbutton_addXtandem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mbutton_addXtandem.ForeColor = System.Drawing.Color.Black;
            this.mbutton_addXtandem.Image = global::MTDBCreator.StoredQueries.xtandem;
            this.mbutton_addXtandem.Location = new System.Drawing.Point(12, 48);
            this.mbutton_addXtandem.Name = "mbutton_addXtandem";
            this.mbutton_addXtandem.Size = new System.Drawing.Size(195, 79);
            this.mbutton_addXtandem.TabIndex = 6;
            this.mbutton_addXtandem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.mbutton_addXtandem.UseVisualStyleBackColor = false;
            this.mbutton_addXtandem.Click += new System.EventHandler(this.mbutton_addXtandem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "I have PHRP Results from....";
            // 
            // mbutton_addSequest
            // 
            this.mbutton_addSequest.BackColor = System.Drawing.Color.White;
            this.mbutton_addSequest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.mbutton_addSequest.FlatAppearance.BorderSize = 2;
            this.mbutton_addSequest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbutton_addSequest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mbutton_addSequest.ForeColor = System.Drawing.Color.Black;
            this.mbutton_addSequest.Image = global::MTDBCreator.StoredQueries.sequest;
            this.mbutton_addSequest.Location = new System.Drawing.Point(12, 133);
            this.mbutton_addSequest.Name = "mbutton_addSequest";
            this.mbutton_addSequest.Size = new System.Drawing.Size(195, 105);
            this.mbutton_addSequest.TabIndex = 8;
            this.mbutton_addSequest.UseVisualStyleBackColor = false;
            this.mbutton_addSequest.Click += new System.EventHandler(this.mbutton_addSequest_Click);
            // 
            // mbutton_addPhrp
            // 
            this.mbutton_addPhrp.BackColor = System.Drawing.Color.White;
            this.mbutton_addPhrp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.mbutton_addPhrp.FlatAppearance.BorderSize = 2;
            this.mbutton_addPhrp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mbutton_addPhrp.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mbutton_addPhrp.ForeColor = System.Drawing.Color.Black;
            this.mbutton_addPhrp.Location = new System.Drawing.Point(12, 244);
            this.mbutton_addPhrp.Name = "mbutton_addPhrp";
            this.mbutton_addPhrp.Size = new System.Drawing.Size(195, 114);
            this.mbutton_addPhrp.TabIndex = 9;
            this.mbutton_addPhrp.Text = "Dataset Description File";
            this.mbutton_addPhrp.UseVisualStyleBackColor = false;
            this.mbutton_addPhrp.Click += new System.EventHandler(this.mbutton_addPhrp_Click);
            // 
            // listViewDatasets
            // 
            this.listViewDatasets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewDatasets.BackColor = System.Drawing.Color.White;
            this.listViewDatasets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderDataset,
            this.columnHeader2});
            this.listViewDatasets.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewDatasets.FullRowSelect = true;
            this.listViewDatasets.GridLines = true;
            this.listViewDatasets.HideSelection = false;
            this.listViewDatasets.Location = new System.Drawing.Point(213, 48);
            this.listViewDatasets.MultiSelect = false;
            this.listViewDatasets.Name = "listViewDatasets";
            this.listViewDatasets.Size = new System.Drawing.Size(718, 502);
            this.listViewDatasets.TabIndex = 15;
            this.listViewDatasets.UseCompatibleStateImageBehavior = false;
            this.listViewDatasets.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderDataset
            // 
            this.columnHeaderDataset.Text = "Dataset Name";
            this.columnHeaderDataset.Width = 252;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Folder";
            this.columnHeader2.Width = 409;
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(943, 610);
            this.Controls.Add(this.listViewDatasets);
            this.Controls.Add(this.mbutton_addPhrp);
            this.Controls.Add(this.mbutton_addSequest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mbutton_addXtandem);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mbuttonOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InputForm";
            this.Text = "Select Data";
            this.Load += new System.EventHandler(this.InputForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button mbuttonOK;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button mbutton_addXtandem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button mbutton_addSequest;
        private System.Windows.Forms.Button mbutton_addPhrp;
        private System.Windows.Forms.ListView listViewDatasets;
        private System.Windows.Forms.ColumnHeader columnHeaderDataset;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}