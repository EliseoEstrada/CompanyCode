namespace ProyectoAAVD
{
    partial class Incidencias
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lDigitos = new System.Windows.Forms.Label();
            this.labelPorcentual = new System.Windows.Forms.Label();
            this.CBMonto = new System.Windows.Forms.ComboBox();
            this.RBMontoP = new System.Windows.Forms.RadioButton();
            this.labelFijo = new System.Windows.Forms.Label();
            this.RBMontoF = new System.Windows.Forms.RadioButton();
            this.txtMonto = new System.Windows.Forms.TextBox();
            this.CBTipoIncidencia = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIncidencia = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.alertLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.CBTipoIncidencia);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtIncidencia);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(19, 32);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(304, 263);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informacion de la incidencia";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lDigitos);
            this.groupBox2.Controls.Add(this.labelPorcentual);
            this.groupBox2.Controls.Add(this.CBMonto);
            this.groupBox2.Controls.Add(this.RBMontoP);
            this.groupBox2.Controls.Add(this.labelFijo);
            this.groupBox2.Controls.Add(this.RBMontoF);
            this.groupBox2.Controls.Add(this.txtMonto);
            this.groupBox2.Location = new System.Drawing.Point(16, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 141);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tipo de monto ";
            // 
            // lDigitos
            // 
            this.lDigitos.AutoSize = true;
            this.lDigitos.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDigitos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.lDigitos.Location = new System.Drawing.Point(107, 23);
            this.lDigitos.Name = "lDigitos";
            this.lDigitos.Size = new System.Drawing.Size(159, 17);
            this.lDigitos.TabIndex = 56;
            this.lDigitos.Text = "Solo se aceptan digitos";
            this.lDigitos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lDigitos.Visible = false;
            // 
            // labelPorcentual
            // 
            this.labelPorcentual.AutoSize = true;
            this.labelPorcentual.Location = new System.Drawing.Point(12, 104);
            this.labelPorcentual.Name = "labelPorcentual";
            this.labelPorcentual.Size = new System.Drawing.Size(18, 17);
            this.labelPorcentual.TabIndex = 36;
            this.labelPorcentual.Text = "%";
            // 
            // CBMonto
            // 
            this.CBMonto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBMonto.FormattingEnabled = true;
            this.CBMonto.Location = new System.Drawing.Point(32, 101);
            this.CBMonto.Name = "CBMonto";
            this.CBMonto.Size = new System.Drawing.Size(208, 25);
            this.CBMonto.TabIndex = 7;
            // 
            // RBMontoP
            // 
            this.RBMontoP.AutoSize = true;
            this.RBMontoP.Location = new System.Drawing.Point(10, 77);
            this.RBMontoP.Name = "RBMontoP";
            this.RBMontoP.Size = new System.Drawing.Size(143, 21);
            this.RBMontoP.TabIndex = 6;
            this.RBMontoP.Text = "Monto porcentual";
            this.RBMontoP.UseVisualStyleBackColor = true;
            this.RBMontoP.Click += new System.EventHandler(this.RBMontoP_Click);
            // 
            // labelFijo
            // 
            this.labelFijo.AutoSize = true;
            this.labelFijo.Location = new System.Drawing.Point(7, 50);
            this.labelFijo.Name = "labelFijo";
            this.labelFijo.Size = new System.Drawing.Size(15, 17);
            this.labelFijo.TabIndex = 32;
            this.labelFijo.Text = "$";
            // 
            // RBMontoF
            // 
            this.RBMontoF.AutoSize = true;
            this.RBMontoF.Checked = true;
            this.RBMontoF.Location = new System.Drawing.Point(10, 20);
            this.RBMontoF.Name = "RBMontoF";
            this.RBMontoF.Size = new System.Drawing.Size(91, 21);
            this.RBMontoF.TabIndex = 4;
            this.RBMontoF.TabStop = true;
            this.RBMontoF.Text = "Monto fijo";
            this.RBMontoF.UseVisualStyleBackColor = true;
            this.RBMontoF.Click += new System.EventHandler(this.RBMontoF_Click);
            // 
            // txtMonto
            // 
            this.txtMonto.Location = new System.Drawing.Point(27, 47);
            this.txtMonto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMonto.Name = "txtMonto";
            this.txtMonto.Size = new System.Drawing.Size(213, 23);
            this.txtMonto.TabIndex = 5;
            this.txtMonto.TabStop = false;
            this.txtMonto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMonto_KeyPress);
            // 
            // CBTipoIncidencia
            // 
            this.CBTipoIncidencia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBTipoIncidencia.FormattingEnabled = true;
            this.CBTipoIncidencia.Location = new System.Drawing.Point(16, 38);
            this.CBTipoIncidencia.Name = "CBTipoIncidencia";
            this.CBTipoIncidencia.Size = new System.Drawing.Size(268, 25);
            this.CBTipoIncidencia.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Descripcion";
            // 
            // txtIncidencia
            // 
            this.txtIncidencia.Location = new System.Drawing.Point(16, 87);
            this.txtIncidencia.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIncidencia.Name = "txtIncidencia";
            this.txtIncidencia.Size = new System.Drawing.Size(268, 23);
            this.txtIncidencia.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tipo de incidencia";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancelar.Location = new System.Drawing.Point(93, 302);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(104, 38);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAgregar.Location = new System.Drawing.Point(203, 302);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(120, 38);
            this.btnAgregar.TabIndex = 8;
            this.btnAgregar.Text = "Agregar incidencia";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // alertLabel
            // 
            this.alertLabel.AutoSize = true;
            this.alertLabel.Font = new System.Drawing.Font("Century Gothic", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alertLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.alertLabel.Location = new System.Drawing.Point(16, 12);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Size = new System.Drawing.Size(226, 16);
            this.alertLabel.TabIndex = 45;
            this.alertLabel.Text = "Todos los campos son obligatorios";
            this.alertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.alertLabel.Visible = false;
            // 
            // Incidencias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 356);
            this.Controls.Add(this.alertLabel);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Incidencias";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Incidencias";
            this.Load += new System.EventHandler(this.Incidencias_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelFijo;
        public System.Windows.Forms.TextBox txtMonto;
        public System.Windows.Forms.ComboBox CBTipoIncidencia;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtIncidencia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.ComboBox CBMonto;
        private System.Windows.Forms.Label labelPorcentual;
        private System.Windows.Forms.Label alertLabel;
        public System.Windows.Forms.RadioButton RBMontoP;
        public System.Windows.Forms.RadioButton RBMontoF;
        private System.Windows.Forms.Label lDigitos;
    }
}