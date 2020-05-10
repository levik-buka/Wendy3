﻿namespace Wendy
{
    partial class MainGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGrid));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tulostaBtn = new System.Windows.Forms.Button();
            this.asetuksetBtn = new System.Windows.Forms.Button();
            this.tasauslaskuBtn = new System.Windows.Forms.Button();
            this.arviolaskuBtn = new System.Windows.Forms.Button();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Aikajakso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Asunto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lukema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Kulutus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Perusmaksut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Käyttömaksut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tasauslasku = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Loppusumma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ALV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.selectedSumStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Aikajakso,
            this.Asunto,
            this.Lukema,
            this.Kulutus,
            this.Perusmaksut,
            this.Käyttömaksut,
            this.Tasauslasku,
            this.Loppusumma,
            this.ALV});
            this.dataGridView.Location = new System.Drawing.Point(129, 10);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(889, 441);
            this.dataGridView.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedSumStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 455);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1030, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.tulostaBtn);
            this.groupBox1.Controls.Add(this.asetuksetBtn);
            this.groupBox1.Controls.Add(this.tasauslaskuBtn);
            this.groupBox1.Controls.Add(this.arviolaskuBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(111, 448);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Valikko";
            // 
            // tulostaBtn
            // 
            this.tulostaBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tulostaBtn.Location = new System.Drawing.Point(6, 390);
            this.tulostaBtn.Name = "tulostaBtn";
            this.tulostaBtn.Size = new System.Drawing.Size(98, 23);
            this.tulostaBtn.TabIndex = 2;
            this.tulostaBtn.Text = "Tulosta...";
            this.tulostaBtn.UseVisualStyleBackColor = true;
            // 
            // asetuksetBtn
            // 
            this.asetuksetBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.asetuksetBtn.Location = new System.Drawing.Point(6, 419);
            this.asetuksetBtn.Name = "asetuksetBtn";
            this.asetuksetBtn.Size = new System.Drawing.Size(98, 23);
            this.asetuksetBtn.TabIndex = 3;
            this.asetuksetBtn.Text = "Asetukset";
            this.asetuksetBtn.UseVisualStyleBackColor = true;
            // 
            // tasauslaskuBtn
            // 
            this.tasauslaskuBtn.Location = new System.Drawing.Point(6, 49);
            this.tasauslaskuBtn.Name = "tasauslaskuBtn";
            this.tasauslaskuBtn.Size = new System.Drawing.Size(98, 23);
            this.tasauslaskuBtn.TabIndex = 1;
            this.tasauslaskuBtn.Text = "Uusi &tasauslasku";
            this.tasauslaskuBtn.UseVisualStyleBackColor = true;
            // 
            // arviolaskuBtn
            // 
            this.arviolaskuBtn.Location = new System.Drawing.Point(7, 20);
            this.arviolaskuBtn.Name = "arviolaskuBtn";
            this.arviolaskuBtn.Size = new System.Drawing.Size(98, 23);
            this.arviolaskuBtn.TabIndex = 0;
            this.arviolaskuBtn.Text = "Uusi &arviolasku";
            this.arviolaskuBtn.UseVisualStyleBackColor = true;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // Aikajakso
            // 
            this.Aikajakso.HeaderText = "Aikajakso";
            this.Aikajakso.Name = "Aikajakso";
            this.Aikajakso.ReadOnly = true;
            // 
            // Asunto
            // 
            this.Asunto.HeaderText = "Asunto";
            this.Asunto.Name = "Asunto";
            this.Asunto.ReadOnly = true;
            // 
            // Lukema
            // 
            this.Lukema.HeaderText = "Lukema (m3) arvio/oikea";
            this.Lukema.Name = "Lukema";
            this.Lukema.ReadOnly = true;
            // 
            // Kulutus
            // 
            this.Kulutus.HeaderText = "Kulutus (m3) arvio/oikea";
            this.Kulutus.Name = "Kulutus";
            this.Kulutus.ReadOnly = true;
            // 
            // Perusmaksut
            // 
            this.Perusmaksut.HeaderText = "Perusmaksut vesi/jäte/yhteensä";
            this.Perusmaksut.Name = "Perusmaksut";
            this.Perusmaksut.ReadOnly = true;
            // 
            // Käyttömaksut
            // 
            this.Käyttömaksut.HeaderText = "Käyttömaksut vesi/jäte/yhteensä";
            this.Käyttömaksut.Name = "Käyttömaksut";
            this.Käyttömaksut.ReadOnly = true;
            // 
            // Tasauslasku
            // 
            this.Tasauslasku.HeaderText = "Tasauslasku";
            this.Tasauslasku.Name = "Tasauslasku";
            this.Tasauslasku.ReadOnly = true;
            this.Tasauslasku.Width = 75;
            // 
            // Loppusumma
            // 
            this.Loppusumma.HeaderText = "Loppusumma";
            this.Loppusumma.Name = "Loppusumma";
            this.Loppusumma.ReadOnly = true;
            this.Loppusumma.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Loppusumma.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ALV
            // 
            this.ALV.HeaderText = "ALV";
            this.ALV.Name = "ALV";
            this.ALV.ReadOnly = true;
            this.ALV.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ALV.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ALV.Width = 70;
            // 
            // selectedSumStatus
            // 
            this.selectedSumStatus.Name = "selectedSumStatus";
            this.selectedSumStatus.Size = new System.Drawing.Size(28, 17);
            this.selectedSumStatus.Text = "0.00";
            // 
            // MainGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1030, 477);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.dataGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainGrid";
            this.Text = "Wendy 3";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button tulostaBtn;
        private System.Windows.Forms.Button asetuksetBtn;
        private System.Windows.Forms.Button tasauslaskuBtn;
        private System.Windows.Forms.Button arviolaskuBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Aikajakso;
        private System.Windows.Forms.DataGridViewTextBoxColumn Asunto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lukema;
        private System.Windows.Forms.DataGridViewTextBoxColumn Kulutus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Perusmaksut;
        private System.Windows.Forms.DataGridViewTextBoxColumn Käyttömaksut;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Tasauslasku;
        private System.Windows.Forms.DataGridViewTextBoxColumn Loppusumma;
        private System.Windows.Forms.DataGridViewTextBoxColumn ALV;
        private System.Windows.Forms.ToolStripStatusLabel selectedSumStatus;
    }
}
