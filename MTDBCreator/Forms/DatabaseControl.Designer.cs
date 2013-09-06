namespace MTDBCreator.Forms
{
    partial class DatabaseControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseControl));
            this.mtree_proteins = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // mtree_proteins
            // 
            this.mtree_proteins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtree_proteins.ImageIndex = 0;
            this.mtree_proteins.ImageList = this.imageList1;
            this.mtree_proteins.Location = new System.Drawing.Point(0, 0);
            this.mtree_proteins.Name = "mtree_proteins";
            this.mtree_proteins.SelectedImageIndex = 0;
            this.mtree_proteins.Size = new System.Drawing.Size(271, 340);
            this.mtree_proteins.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "gas-16.png");
            this.imageList1.Images.SetKeyName(1, "corkscrew-16.png");
            this.imageList1.Images.SetKeyName(2, "database-16.png");
            this.imageList1.Images.SetKeyName(3, "tree-structure-16.png");
            this.imageList1.Images.SetKeyName(4, "tree-structure-16.png");
            this.imageList1.Images.SetKeyName(5, "circle-16.png");
            // 
            // DatabaseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.mtree_proteins);
            this.Name = "DatabaseControl";
            this.Size = new System.Drawing.Size(271, 340);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView mtree_proteins;
        private System.Windows.Forms.ImageList imageList1;
    }
}
