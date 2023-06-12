namespace ProyectoAAVD
{
    partial class Empresas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnEliminarP = new System.Windows.Forms.Button();
            this.dataGridPercepcion = new System.Windows.Forms.DataGridView();
            this.Percepcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Monto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnEliminarD = new System.Windows.Forms.Button();
            this.dataGridDeduccion = new System.Windows.Forms.DataGridView();
            this.Deduccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Monto2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alertLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lDigitos = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.LCargando = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkMensual = new System.Windows.Forms.CheckBox();
            this.checkCatorcenal = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.checkQuincenal = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRazonS = new System.Windows.Forms.TextBox();
            this.checkSemanal = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRegistroP = new System.Windows.Forms.TextBox();
            this.txtRegistroF = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDomicilioF = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridEmpresas = new System.Windows.Forms.DataGridView();
            this.btnIncidencias = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPercepcion)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDeduccion)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEmpresas)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(563, 158);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(285, 240);
            this.tabControl1.TabIndex = 40;
            this.tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnEliminarP);
            this.tabPage1.Controls.Add(this.dataGridPercepcion);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(277, 210);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Percepciones";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnEliminarP
            // 
            this.btnEliminarP.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminarP.ForeColor = System.Drawing.Color.Red;
            this.btnEliminarP.Location = new System.Drawing.Point(7, 6);
            this.btnEliminarP.Name = "btnEliminarP";
            this.btnEliminarP.Size = new System.Drawing.Size(134, 38);
            this.btnEliminarP.TabIndex = 41;
            this.btnEliminarP.TabStop = false;
            this.btnEliminarP.Text = "Eliminar percepcion";
            this.btnEliminarP.UseVisualStyleBackColor = true;
            this.btnEliminarP.Click += new System.EventHandler(this.btnEliminarP_Click);
            // 
            // dataGridPercepcion
            // 
            this.dataGridPercepcion.AllowUserToAddRows = false;
            this.dataGridPercepcion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridPercepcion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridPercepcion.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridPercepcion.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridPercepcion.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridPercepcion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridPercepcion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPercepcion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Percepcion,
            this.Monto});
            this.dataGridPercepcion.EnableHeadersVisualStyles = false;
            this.dataGridPercepcion.Location = new System.Drawing.Point(7, 50);
            this.dataGridPercepcion.MultiSelect = false;
            this.dataGridPercepcion.Name = "dataGridPercepcion";
            this.dataGridPercepcion.ReadOnly = true;
            this.dataGridPercepcion.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridPercepcion.RowHeadersVisible = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridPercepcion.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridPercepcion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridPercepcion.Size = new System.Drawing.Size(267, 154);
            this.dataGridPercepcion.TabIndex = 29;
            this.dataGridPercepcion.TabStop = false;
            this.dataGridPercepcion.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridPercepcion_CellContentDoubleClick);
            // 
            // Percepcion
            // 
            this.Percepcion.HeaderText = "Percepcion";
            this.Percepcion.Name = "Percepcion";
            this.Percepcion.ReadOnly = true;
            // 
            // Monto
            // 
            this.Monto.HeaderText = "Monto";
            this.Monto.Name = "Monto";
            this.Monto.ReadOnly = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnEliminarD);
            this.tabPage2.Controls.Add(this.dataGridDeduccion);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(277, 210);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Deducciones";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnEliminarD
            // 
            this.btnEliminarD.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminarD.ForeColor = System.Drawing.Color.Red;
            this.btnEliminarD.Location = new System.Drawing.Point(6, 6);
            this.btnEliminarD.Name = "btnEliminarD";
            this.btnEliminarD.Size = new System.Drawing.Size(134, 38);
            this.btnEliminarD.TabIndex = 42;
            this.btnEliminarD.TabStop = false;
            this.btnEliminarD.Text = "Eliminar deduccion";
            this.btnEliminarD.UseVisualStyleBackColor = true;
            this.btnEliminarD.Click += new System.EventHandler(this.btnEliminarD_Click);
            // 
            // dataGridDeduccion
            // 
            this.dataGridDeduccion.AllowUserToAddRows = false;
            this.dataGridDeduccion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridDeduccion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridDeduccion.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridDeduccion.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridDeduccion.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridDeduccion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridDeduccion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDeduccion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Deduccion,
            this.Monto2});
            this.dataGridDeduccion.EnableHeadersVisualStyles = false;
            this.dataGridDeduccion.Location = new System.Drawing.Point(6, 50);
            this.dataGridDeduccion.MultiSelect = false;
            this.dataGridDeduccion.Name = "dataGridDeduccion";
            this.dataGridDeduccion.ReadOnly = true;
            this.dataGridDeduccion.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridDeduccion.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridDeduccion.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridDeduccion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridDeduccion.Size = new System.Drawing.Size(268, 156);
            this.dataGridDeduccion.TabIndex = 32;
            this.dataGridDeduccion.TabStop = false;
            this.dataGridDeduccion.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridDeduccion_CellContentDoubleClick);
            // 
            // Deduccion
            // 
            this.Deduccion.HeaderText = "Deduccion";
            this.Deduccion.Name = "Deduccion";
            this.Deduccion.ReadOnly = true;
            // 
            // Monto2
            // 
            this.Monto2.HeaderText = "Monto";
            this.Monto2.Name = "Monto2";
            this.Monto2.ReadOnly = true;
            // 
            // alertLabel
            // 
            this.alertLabel.AutoSize = true;
            this.alertLabel.Font = new System.Drawing.Font("Century Gothic", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alertLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.alertLabel.Location = new System.Drawing.Point(331, 59);
            this.alertLabel.Name = "alertLabel";
            this.alertLabel.Size = new System.Drawing.Size(226, 16);
            this.alertLabel.TabIndex = 39;
            this.alertLabel.Text = "Todos los campos son obligatorios";
            this.alertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.alertLabel.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lDigitos);
            this.groupBox2.Controls.Add(this.txtDireccion);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtCorreo);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtTelefono);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(26, 277);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(531, 117);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Datos de contacto";
            // 
            // lDigitos
            // 
            this.lDigitos.AutoSize = true;
            this.lDigitos.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lDigitos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.lDigitos.Location = new System.Drawing.Point(65, 20);
            this.lDigitos.Name = "lDigitos";
            this.lDigitos.Size = new System.Drawing.Size(133, 16);
            this.lDigitos.TabIndex = 55;
            this.lDigitos.Text = "Solo se aceptan digitos";
            this.lDigitos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lDigitos.Visible = false;
            // 
            // txtDireccion
            // 
            this.txtDireccion.Location = new System.Drawing.Point(8, 85);
            this.txtDireccion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(502, 23);
            this.txtDireccion.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 65);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "Dirección";
            // 
            // txtCorreo
            // 
            this.txtCorreo.Location = new System.Drawing.Point(230, 40);
            this.txtCorreo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Size = new System.Drawing.Size(271, 23);
            this.txtCorreo.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(227, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 17);
            this.label7.TabIndex = 12;
            this.label7.Text = "Correo";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Location = new System.Drawing.Point(8, 40);
            this.txtTelefono.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(187, 23);
            this.txtTelefono.TabIndex = 11;
            this.txtTelefono.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTelefono_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Telefono";
            // 
            // LCargando
            // 
            this.LCargando.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LCargando.AutoSize = true;
            this.LCargando.BackColor = System.Drawing.Color.Transparent;
            this.LCargando.Font = new System.Drawing.Font("Century Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LCargando.Location = new System.Drawing.Point(334, 18);
            this.LCargando.Name = "LCargando";
            this.LCargando.Size = new System.Drawing.Size(202, 38);
            this.LCargando.TabIndex = 36;
            this.LCargando.Text = "Cargando...";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkMensual);
            this.groupBox1.Controls.Add(this.checkCatorcenal);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.checkQuincenal);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtRazonS);
            this.groupBox1.Controls.Add(this.checkSemanal);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtRegistroP);
            this.groupBox1.Controls.Add(this.txtRegistroF);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtDomicilioF);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(25, 70);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(532, 203);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos generales";
            // 
            // checkMensual
            // 
            this.checkMensual.AutoSize = true;
            this.checkMensual.Location = new System.Drawing.Point(422, 129);
            this.checkMensual.Name = "checkMensual";
            this.checkMensual.Size = new System.Drawing.Size(79, 21);
            this.checkMensual.TabIndex = 9;
            this.checkMensual.Text = "Mensual";
            this.checkMensual.UseVisualStyleBackColor = true;
            // 
            // checkCatorcenal
            // 
            this.checkCatorcenal.AutoSize = true;
            this.checkCatorcenal.Location = new System.Drawing.Point(421, 98);
            this.checkCatorcenal.Name = "checkCatorcenal";
            this.checkCatorcenal.Size = new System.Drawing.Size(101, 21);
            this.checkCatorcenal.TabIndex = 7;
            this.checkCatorcenal.Text = "Catorcenal";
            this.checkCatorcenal.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(324, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(139, 17);
            this.label9.TabIndex = 18;
            this.label9.Text = "Frecuencia de pago";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(422, 46);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(89, 23);
            this.dateTimePicker1.TabIndex = 5;
            // 
            // checkQuincenal
            // 
            this.checkQuincenal.AutoSize = true;
            this.checkQuincenal.Location = new System.Drawing.Point(327, 129);
            this.checkQuincenal.Name = "checkQuincenal";
            this.checkQuincenal.Size = new System.Drawing.Size(93, 21);
            this.checkQuincenal.TabIndex = 8;
            this.checkQuincenal.Text = "Quincenal";
            this.checkQuincenal.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(418, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Fecha de inicio";
            // 
            // txtRazonS
            // 
            this.txtRazonS.Location = new System.Drawing.Point(8, 46);
            this.txtRazonS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRazonS.Name = "txtRazonS";
            this.txtRazonS.Size = new System.Drawing.Size(394, 23);
            this.txtRazonS.TabIndex = 1;
            // 
            // checkSemanal
            // 
            this.checkSemanal.AutoSize = true;
            this.checkSemanal.Location = new System.Drawing.Point(327, 98);
            this.checkSemanal.Name = "checkSemanal";
            this.checkSemanal.Size = new System.Drawing.Size(83, 21);
            this.checkSemanal.TabIndex = 6;
            this.checkSemanal.Text = "Semanal";
            this.checkSemanal.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "Razón social";
            // 
            // txtRegistroP
            // 
            this.txtRegistroP.Location = new System.Drawing.Point(9, 132);
            this.txtRegistroP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRegistroP.Name = "txtRegistroP";
            this.txtRegistroP.Size = new System.Drawing.Size(262, 23);
            this.txtRegistroP.TabIndex = 3;
            // 
            // txtRegistroF
            // 
            this.txtRegistroF.Location = new System.Drawing.Point(8, 91);
            this.txtRegistroF.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRegistroF.Name = "txtRegistroF";
            this.txtRegistroF.Size = new System.Drawing.Size(263, 23);
            this.txtRegistroF.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Registro patronal";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(222, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Registro federal de contribuyente";
            // 
            // txtDomicilioF
            // 
            this.txtDomicilioF.Location = new System.Drawing.Point(8, 173);
            this.txtDomicilioF.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDomicilioF.Name = "txtDomicilioF";
            this.txtDomicilioF.Size = new System.Drawing.Size(503, 23);
            this.txtDomicilioF.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Domicilio fiscal";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 35;
            this.label1.Text = "Empresas";
            // 
            // dataGridEmpresas
            // 
            this.dataGridEmpresas.AllowUserToAddRows = false;
            this.dataGridEmpresas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridEmpresas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridEmpresas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridEmpresas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridEmpresas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridEmpresas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridEmpresas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridEmpresas.EnableHeadersVisualStyles = false;
            this.dataGridEmpresas.Location = new System.Drawing.Point(26, 405);
            this.dataGridEmpresas.MultiSelect = false;
            this.dataGridEmpresas.Name = "dataGridEmpresas";
            this.dataGridEmpresas.ReadOnly = true;
            this.dataGridEmpresas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridEmpresas.RowHeadersVisible = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridEmpresas.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridEmpresas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridEmpresas.Size = new System.Drawing.Size(813, 215);
            this.dataGridEmpresas.TabIndex = 34;
            this.dataGridEmpresas.TabStop = false;
            this.dataGridEmpresas.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridEmpresas_CellContentDoubleClick);
            // 
            // btnIncidencias
            // 
            this.btnIncidencias.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIncidencias.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncidencias.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnIncidencias.Location = new System.Drawing.Point(715, 114);
            this.btnIncidencias.Name = "btnIncidencias";
            this.btnIncidencias.Size = new System.Drawing.Size(120, 38);
            this.btnIncidencias.TabIndex = 14;
            this.btnIncidencias.Text = "Nueva incidencia";
            this.btnIncidencias.UseVisualStyleBackColor = true;
            this.btnIncidencias.Click += new System.EventHandler(this.btnIncidencias_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAgregar.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAgregar.Location = new System.Drawing.Point(689, 20);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(150, 36);
            this.btnAgregar.TabIndex = 15;
            this.btnAgregar.Text = "Agregar empresa";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.ForeColor = System.Drawing.Color.Red;
            this.btnEliminar.Location = new System.Drawing.Point(536, 20);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(150, 36);
            this.btnEliminar.TabIndex = 16;
            this.btnEliminar.Text = "Eliminar empresa";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Visible = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // Empresas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 640);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnIncidencias);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.alertLabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.LCargando);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridEmpresas);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Empresas";
            this.Text = "Empresas";
            this.Load += new System.EventHandler(this.Empresas_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPercepcion)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDeduccion)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEmpresas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btnEliminarP;
        public System.Windows.Forms.DataGridView dataGridPercepcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Percepcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Monto;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnEliminarD;
        public System.Windows.Forms.DataGridView dataGridDeduccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Deduccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Monto2;
        private System.Windows.Forms.Label alertLabel;
        public System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label LCargando;
        public System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkMensual;
        private System.Windows.Forms.CheckBox checkCatorcenal;
        private System.Windows.Forms.CheckBox checkQuincenal;
        private System.Windows.Forms.CheckBox checkSemanal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtRegistroF;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtRegistroP;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtDomicilioF;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtRazonS;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridEmpresas;
        private System.Windows.Forms.Button btnIncidencias;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEliminar;
        public System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lDigitos;
    }
}