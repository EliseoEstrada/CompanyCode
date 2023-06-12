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
    public partial class Empresas : Form
    {
        bool EDIT;
        EnlaceCassandra cassandra;
        List<Empresa> empresas;
        int empresaActual;
        string idEmpresa;
        DateTime hoy;
        public Empresas()
        {
            InitializeComponent();
        }

        private void Empresas_Load(object sender, EventArgs e)
        {
            iniciarValores();
            hoy = Variables.HOY;
            //Obtener empresas 
            empresaActual = 0;
            empresas = Variables.EMPRESAS;
            dateTimePicker1.MaxDate = hoy;
            if (empresas.Count > 0 || empresas == null)
            {
                dataGridEmpresas.DataSource = empresas;
                dataGridEmpresas.Columns[0].HeaderText = "ID EMPRESA";
                dataGridEmpresas.Columns[1].HeaderText = "RAZON SOCIAL";
                dataGridEmpresas.Columns[2].HeaderText = "DOMICILIO FISCAL";
                dataGridEmpresas.Columns[3].HeaderText = "REGISTRO PATRONAL";
                dataGridEmpresas.Columns[4].HeaderText = "REGISTRO FEDERAL";
                dataGridEmpresas.Columns[5].HeaderText = "FECHA DE INICIO";
                dataGridEmpresas.Columns[6].HeaderText = "TELEFONO";
                dataGridEmpresas.Columns[7].HeaderText = "CORREO";
                dataGridEmpresas.Columns[8].HeaderText = "DIRECCION";
                dataGridEmpresas.Columns[9].HeaderText = "ULTIMA NOMINA";
            }
            else
            {
                MessageBox.Show("No hay empresas registradas", "Aviso");
            }


            //Llenar combo incidencias
            dataGridPercepcion.Rows.Add();
            dataGridPercepcion.Rows[0].Cells[0].Value = "Sueldo";
            dataGridPercepcion.Rows[0].Cells[1].Value = "1.0";
            dataGridPercepcion.Rows.Add();
            dataGridPercepcion.Rows[1].Cells[0].Value = "Bono gerente";
            dataGridPercepcion.Rows[1].Cells[1].Value = "1000";
            dataGridPercepcion.Rows.Add();
            dataGridPercepcion.Rows[2].Cells[0].Value = "Prima vacacional";
            dataGridPercepcion.Rows[2].Cells[1].Value = "0.5";

            dataGridDeduccion.Rows.Add();
            dataGridDeduccion.Rows[0].Cells[0].Value = "IMSS";
            dataGridDeduccion.Rows[0].Cells[1].Value = "200.0";
            dataGridDeduccion.Rows.Add();
            dataGridDeduccion.Rows[1].Cells[0].Value = "ISR";
            dataGridDeduccion.Rows[1].Cells[1].Value = "0.10";

            LCargando.Visible = false;
            alertLabel.Visible = false;
        }

        private void btnIncidencias_Click(object sender, EventArgs e)
        {
            Incidencias incidencia = new Incidencias();
            AddOwnedForm(incidencia);
            Variables.EDIT = false;
            incidencia.ShowDialog();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            bool sucess = false;
            string razonSocial = txtRazonS.Text;
            string domicilioF = txtDomicilioF.Text;
            string registroP = txtRegistroP.Text;
            string registroF = txtRegistroF.Text;
            string fechaInicio = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string telefono = txtTelefono.Text;
            string correo = txtCorreo.Text;
            string direccion = txtDireccion.Text;

            //-------Checkbox frecuencia
            string[] frecuencia = new string[] { "", "", "", "" };
            if (checkSemanal.Checked == true)
            {
                frecuencia[0] = "Semanal";
            }
            if (checkCatorcenal.Checked == true)
            {
                frecuencia[1] = "Catorcenal";
            }
            if (checkQuincenal.Checked == true)
            {
                frecuencia[2] = "Quincenal";
            }
            if (checkMensual.Checked == true)
            {
                frecuencia[3] = "Mensual";
            }

            //---------------------Verificar que haya incidencias
            string percepciones = "";
            string deducciones = "";
            string Incidencia = "";

            foreach (DataGridViewRow row in dataGridPercepcion.Rows)
            {
                string descripcion = row.Cells[0].Value.ToString();
                string monto = row.Cells[1].Value.ToString();
                percepciones = armarIncidencia(percepciones, "Percepcion", descripcion, monto, false);
            }

            foreach (DataGridViewRow row in dataGridDeduccion.Rows)
            {
                string _descripcion = row.Cells[0].Value.ToString();
                string _monto = row.Cells[1].Value.ToString();
                deducciones = armarIncidencia(deducciones, "Deduccion", _descripcion, _monto, false);
            }
            deducciones = armarIncidencia(deducciones, "", "", "", true); //quitar coma
            Incidencia = percepciones + " " + deducciones;

            //--------------------------------Verificar campos vacios
            if (razonSocial == "" || domicilioF == "" || registroP == "" || registroF == "" || 
                telefono == "" || correo == "" || direccion == "")
            {
                alertLabel.Visible = true;
            }
            else
            {
                alertLabel.Visible = false;
                cassandra = new EnlaceCassandra();
                LCargando.Visible = true;

                if (!EDIT)                //Agregar empresa
                {
                   sucess = cassandra.AgregarEmpresa(razonSocial, domicilioF, registroP,
                       registroF, frecuencia, fechaInicio, telefono, correo, direccion, Incidencia);
                }
                else                      //Editar empresa
                {
                    string idEmpresa = empresas[empresaActual].idEmpresa.ToString();
                    sucess = cassandra.EditarEmpresa(idEmpresa, razonSocial, domicilioF, registroP,
                       registroF, frecuencia, fechaInicio, telefono, correo, direccion, Incidencia);
                }
               

                if (sucess)
                {
                    if (!EDIT)
                    {
                        MessageBox.Show("Empresa agregada con exito", "AVISO");
                    }
                    else
                    {
                        MessageBox.Show("Empresa editada con exito", "AVISO");
                    }
                    //Actualizar ventana
                    this.Empresas_Load(sender, e);
                }
                else
                {
                    LCargando.Visible = false;
                    MessageBox.Show("No se pudo realizar la operacion", "ERROR");
                }
            }


        }

        private void dataGridEmpresas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EDIT = true;

            //Obtener id de empresa
            for (int i = 0; i < empresas.Count; i++)
            {
                if (empresas[i].razon_social.ToString() == dataGridEmpresas.CurrentRow.Cells[1].Value.ToString())
                {
                    empresaActual = i;
                    break;
                }
            }

            idEmpresa = empresas[empresaActual].idEmpresa.ToString();
            txtRazonS.Text = empresas[empresaActual].razon_social.ToString();
            txtDomicilioF.Text = empresas[empresaActual].domicilio_fiscal.ToString();
            txtRegistroP.Text = empresas[empresaActual].registro_patronal.ToString();
            txtRegistroF.Text = empresas[empresaActual].registro_federal.ToString();

            checkSemanal.Checked = false;
            checkCatorcenal.Checked = false;
            checkQuincenal.Checked = false;
            checkMensual.Checked = false;

            if (empresas[empresaActual].frecuencia_pago[0].ToString() == "Semanal")
            {
                checkSemanal.Checked = true;
            }
            if (empresas[empresaActual].frecuencia_pago[1].ToString() == "Catorcenal")
            {
                checkCatorcenal.Checked = true;
            }
            if (empresas[empresaActual].frecuencia_pago[2].ToString() == "Quincenal")
            {
                checkQuincenal.Checked = true;
            }
            if (empresas[empresaActual].frecuencia_pago[3].ToString() == "Mensual")
            {
                checkMensual.Checked = true;
            }
            txtTelefono.Text = empresas[empresaActual].telefono.ToString();
            txtCorreo.Text = empresas[empresaActual].correo.ToString();
            txtDireccion.Text = empresas[empresaActual].direccion.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(empresas[empresaActual].fecha_inicio.ToString());

            string _percepcion = "";
            string _deduccion = "";
            string _monto = "";
            string aux = "";

            //Percepciones y deducciones

            dataGridPercepcion.Rows.Clear();
            dataGridDeduccion.Rows.Clear();
            for (int i = 0; i < empresas[empresaActual].incidencias.Count; i++)
            {
                aux = empresas[empresaActual].incidencias[i].Item1.ToString();
                if (aux == "Percepcion")
                {
                    _percepcion = empresas[empresaActual].incidencias[i].Item2.ToString();
                    _monto = empresas[empresaActual].incidencias[i].Item3.ToString();
                    dataGridPercepcion.Rows.Add(_percepcion, _monto);
                }
                if (aux == "Deduccion")
                {
                    _deduccion = empresas[empresaActual].incidencias[i].Item2.ToString();
                    _monto = empresas[empresaActual].incidencias[i].Item3.ToString();
                    dataGridDeduccion.Rows.Add(_deduccion, _monto);
                }
            }
            btnAgregar.Text = "Actualizar empresa";
            btnEliminar.Visible = true;
        }

        private void dataGridPercepcion_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridPercepcion.CurrentRow.Cells[0].Value.ToString() != "Sueldo" )
            {
                editarIncidencia(1);
            }
            else if(dataGridPercepcion.CurrentRow.Cells[0].Value.ToString() == "Sueldo")
            {
                MessageBox.Show("El sueldo se calcula en base al sueldo diario y dias laborados", "AVISO");
            }
            //else if (dataGridPercepcion.CurrentRow.Cells[0].Value.ToString() == "Prima vacacional")
            //{
            //    MessageBox.Show("La prima vacacional se calcula en base al sueldo diario del empleado", "AVISO");
            //}
        }

        private void dataGridDeduccion_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            editarIncidencia(2);
        }

        #region MisFunciones

        private void iniciarValores()
        {
            LCargando.Visible = true;
            txtRazonS.Text = "";
            txtDomicilioF.Text = "";
            txtRegistroP.Text = "";
            txtRegistroF.Text = "";
            txtTelefono.Text = "";
            txtCorreo.Text = "";
            txtDireccion.Text = "";
            checkSemanal.Checked = false;
            checkCatorcenal.Checked = false;
            checkQuincenal.Checked = false;
            checkMensual.Checked = false;
            dataGridPercepcion.Rows.Clear();
            dataGridDeduccion.Rows.Clear();
            dataGridEmpresas.DataSource = null;
            EDIT = false;
        }

        private string armarIncidencia(string _cadena, string _incidencia, 
            string _descripcion, string _monto, bool ultimo)
        {
            if (!ultimo)
            {
                string aux = " ( '{0}', '{1}', {2}),";
                string _string = string.Format(aux, _incidencia, _descripcion, _monto);
                _cadena = _cadena + _string;
            }
            else
            {
                _cadena = _cadena.TrimEnd(',');
            }
            return _cadena;
        }

        private void soloNumeros(KeyPressEventArgs e)
        {
            lDigitos.Visible = false;
            //Para obligar a que sólo se introduzcan números
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
              if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso
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

        private void editarIncidencia(int _tipoIncidencia)
        {
            Incidencias incidecia = new Incidencias();
            string descripcion = "";
            string monto = "";

            if (_tipoIncidencia == 1) //PERCEPCION
            {
                descripcion = dataGridPercepcion.CurrentRow.Cells[0].Value.ToString();
                monto = dataGridPercepcion.CurrentRow.Cells[1].Value.ToString();
                Variables.TIPO_INCIDENCIA = "Percepcion";
            }
            else if(_tipoIncidencia == 2) //DEDUCCION
            {
                descripcion = dataGridDeduccion.CurrentRow.Cells[0].Value.ToString();
                monto = dataGridDeduccion.CurrentRow.Cells[1].Value.ToString();
                Variables.TIPO_INCIDENCIA = "Deduccion";
            }
  
            decimal montoD = decimal.Parse(monto);
            if (montoD < 1) //Porcentual
            {
                montoD = Math.Round(montoD * 100, 1);
                int auxMonto = decimal.ToInt32(montoD);
                monto = auxMonto.ToString();
                Variables.PORCENTUAL = true;
            }
            else
            {
                Variables.PORCENTUAL = false;
            }

            Variables.EDIT = true;
            Variables.DESCRIPCION = descripcion;
            Variables.MONTO = monto;

            Incidencias incidencia = new Incidencias();
            AddOwnedForm(incidencia);
            incidencia.ShowDialog();
        }


        #endregion MisFunciones

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e);
        }

        private void btnEliminarD_Click(object sender, EventArgs e)
        {
            string descripcion = dataGridDeduccion.CurrentRow.Cells[0].Value.ToString();
            if (descripcion != "IMSS" && descripcion != "ISR")
            {
                dataGridDeduccion.Rows.RemoveAt(dataGridDeduccion.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Las deducciones basicas son bligatorias", "Aviso");
            }
        }

        private void btnEliminarP_Click(object sender, EventArgs e)
        {
            string descripcion = dataGridPercepcion.CurrentRow.Cells[0].Value.ToString();
            if (descripcion != "Sueldo" && descripcion != "Bono gerente"
                 && descripcion != "Prima vacacional")
            {
                dataGridPercepcion.Rows.RemoveAt(dataGridPercepcion.CurrentRow.Index);
            }
            else
            {
                MessageBox.Show("Las percepciones basicas son obligatorias", "Aviso");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Eliminar empresa?",
                   "AVISO", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                LCargando.Visible = true;
                cassandra = new EnlaceCassandra();
                bool sucess = cassandra.EliminarEmpresa(idEmpresa);
                if (sucess)
                {
                    //Actualizar ventana
                    LCargando.Visible = false;
                    MessageBox.Show("Empresa eliminada con exito", "AVISO");
                    btnEliminar.Visible = false;
                    this.Empresas_Load(sender, e);
                }
                else
                {
                    LCargando.Visible = false;
                    MessageBox.Show("No se pudo realizar la operacion", "ERROR");
                }
            }
        }
    }
}
