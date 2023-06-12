using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoAAVD
{
    public partial class Incidencias : Form
    {
        bool EDIT;
        
        public Incidencias()
        {
            InitializeComponent();
            CBMonto.Enabled = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Incidencias_Load(object sender, EventArgs e)
        {
            //CB Incidencia
            CBTipoIncidencia.Items.Add("Percepcion");
            CBTipoIncidencia.Items.Add("Deduccion");
            CBTipoIncidencia.SelectedIndex = 0;

            //CB Porcentual
            for (int i = 1; i < 100; i++)
            {
                CBMonto.Items.Add(i.ToString());
            }
            CBMonto.SelectedIndex = 0;

            EDIT = Variables.EDIT;
            if (EDIT)
            {
                string tipo_incidencia = Variables.TIPO_INCIDENCIA;
                string descripcion = Variables.DESCRIPCION;
                string monto = Variables.MONTO;
                bool porcentual = Variables.PORCENTUAL;

                txtIncidencia.Text = descripcion;

                //Bloquear edicion percepciones y deducciones fijas
                if (descripcion == "Bono gerente" ||
                    descripcion == "IMSS" || descripcion == "ISR" ||
                    descripcion == "Prima vacacional")
                {
                    txtIncidencia.ReadOnly = true;
                    CBTipoIncidencia.Enabled = false;
                }

                //Seleccionar tipo de incidencia en Combo
                CBTipoIncidencia.SelectedItem = tipo_incidencia;

                //Activar opciones de monto porcentual
                if (porcentual)
                {
                    RBMontoP.Checked = true;
                    txtMonto.ReadOnly = true;
                    CBMonto.Enabled = true;
                    CBMonto.SelectedItem = monto;
                }
                else
                {
                    txtMonto.Text = monto;
                }

            }

        }

        private void RBMontoP_Click(object sender, EventArgs e)
        {
            txtMonto.ReadOnly = true;
            CBMonto.Enabled = true;
        }

        private void RBMontoF_Click(object sender, EventArgs e)
        {
            txtMonto.ReadOnly = false;
            CBMonto.Enabled = false;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string descripcion = txtIncidencia.Text;
            Empresas empresa = this.Owner as Empresas;
            string monto = "";
            if (RBMontoF.Checked)
            {
                monto = txtMonto.Text;
            }
            else if (RBMontoP.Checked)
            {
                decimal auxMonto = decimal.Parse(CBMonto.SelectedItem.ToString());
                auxMonto = auxMonto / 100;
                monto = auxMonto.ToString();
            }

            if (descripcion != "" && monto != "")
            {
                if (EDIT)
                {
                    //ELIMINAR INCIDENCIA ANTIGUA ANTES DE AGREGAR LA NUEVA
                    string tipo_incidencia = Variables.TIPO_INCIDENCIA;
                    string antiguaDescripcion = Variables.DESCRIPCION;
                    if (tipo_incidencia == "Percepcion")
                    {

                        for (int i = 0; i < empresa.dataGridPercepcion.Rows.Count; i++)
                        {
                            string auxDescripcion = empresa.dataGridPercepcion.CurrentRow.Cells[0].Value.ToString();
                            if (antiguaDescripcion == auxDescripcion)
                            {
                                empresa.dataGridPercepcion.Rows.RemoveAt(empresa.dataGridPercepcion.SelectedRows[i].Index);
                            }

                        }

                    }
                    else if (tipo_incidencia == "Deduccion")
                    {
                        for (int i = 0; i < empresa.dataGridDeduccion.Rows.Count; i++)
                        {
                            string auxDescripcion = empresa.dataGridDeduccion.CurrentRow.Cells[0].Value.ToString();
                            if (antiguaDescripcion == auxDescripcion)
                            {
                                empresa.dataGridDeduccion.Rows.RemoveAt(empresa.dataGridDeduccion.SelectedRows[i].Index);
                            }

                        }
                    }
                }

                if (CBTipoIncidencia.SelectedItem.ToString() == "Percepcion")
                {
                    empresa.dataGridPercepcion.Rows.Add(txtIncidencia.Text, monto);
                }
                else if (CBTipoIncidencia.SelectedItem.ToString() == "Deduccion")
                {
                    empresa.dataGridDeduccion.Rows.Add(txtIncidencia.Text, monto);
                }

                this.Close();
            }
            else
            {
                alertLabel.Visible = true;
            }
        }


        #region MisFunciones
        private void soloNumeros(KeyPressEventArgs e)
        {
            lDigitos.Visible = false;
            //Para obligar a que sólo se introduzcan números
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso
            {
                e.Handled = false;
            }
            else if (e.KeyChar == 46) //permitir punto
            {
                e.Handled = false;
            }
            else
            {
                //el resto de teclas pulsadas se desactivan
                e.Handled = true;
                lDigitos.Visible = true;
            }
        }
        #endregion MisFunciones

        private void txtMonto_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e);
        }
    }
}
