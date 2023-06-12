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
    public partial class ReporteNomina : Form
    {
        List<Empresa> empresas;
        List<Departamento> departamentos;
        List<Departamento> auxDepartamentos;
        int empresaActual;
        string idEmpresa = "";
        EnlaceCassandra cassandra;
        string firstDate = "";
        string lastDate = "";
        DateTime hoy;
        int añoActual = 0;
        public ReporteNomina()
        {
            InitializeComponent();
            hoy = DateTime.Now;
            string año = hoy.ToString("yyyy");
            añoActual = int.Parse(año);
        }

        private void ReporteNomina_Load(object sender, EventArgs e)
        {
            if (Variables.TYPE_USER == 0) //Admi
            {
                CBEmpresas.Visible = true;
                lEmpresa.Visible = false;
            }
            else
            {
                lEmpresa.Text = Variables.COMPANY;
                CBEmpresas.Visible = false;
                lEmpresa.Visible = true;
            }

            empresas = Variables.EMPRESAS;
            departamentos = Variables.DEPARTAMENTOS;

            if (empresas.Count > 0)
            {
                cassandra = new EnlaceCassandra();
                for (int i = 0; i < empresas.Count; i++)
                {
                    CBEmpresas.Items.Add(empresas[i].razon_social.ToString());
                }

                CBEmpresas.SelectedIndex = 0;

                //CB Años
                agregarCBAños();

                //CB meses
                CBMeses.Items.Add("Todo el año");
                CBMeses.Items.Add("Enero");
                CBMeses.Items.Add("Febrero");
                CBMeses.Items.Add("Marzo");
                CBMeses.Items.Add("Abril");
                CBMeses.Items.Add("Mayo");
                CBMeses.Items.Add("Junio");
                CBMeses.Items.Add("Julio");
                CBMeses.Items.Add("Agosto");
                CBMeses.Items.Add("Septiembre");
                CBMeses.Items.Add("Octubre");
                CBMeses.Items.Add("Noviembre");
                CBMeses.Items.Add("Diciembre");
                CBMeses.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No hay empresas registradas", "AVISO");
                btnFiltrar.Enabled = false;
            }
        }

        private void CBEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridReporte.Rows.Clear();
            empresaActual = CBEmpresas.SelectedIndex;
            idEmpresa = empresas[empresaActual].idEmpresa.ToString();

            agregarCBAños();

            if (departamentos.Count > 0)
            {
                btnFiltrar.Enabled = true;
                auxDepartamentos = new List<Departamento>();
                CBDepartamento.Items.Clear();
                CBDepartamento.Items.Add("Todos los departamentos");
                for (int i = 0; i < departamentos.Count; i++)
                {
                    if(departamentos[i].idempresa.ToString() == idEmpresa)
                    {
                        CBDepartamento.Items.Add(departamentos[i].departamento);
                        //Agregar departamento a la lista auxiliar de departamentos de empresa
                        auxDepartamentos.Add(departamentos[i]);
                    }
                }

                if(CBDepartamento.Items.Count > 1)
                {
                    CBDepartamento.SelectedIndex = 0;
                }
                else
                {
                    CBDepartamento.Items.Clear();
                    MessageBox.Show("No hay departamentos registrados", "AVISO");
                    btnFiltrar.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("No hay departamentos registrados", "AVISO");
                btnFiltrar.Enabled = false;
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            LCargando.Visible = true;
            dataGridReporte.Rows.Clear();

            //Obtener rango de fechas para filtro
            int año = int.Parse(CBAños.SelectedItem.ToString());
            int mes = CBMeses.SelectedIndex;

            if (mes != 0) //Mes
            {
                obtenerDiasMes(año, mes);
            }
            else //Todo el año
            {
                firstDate = año.ToString() + "-01-01";
                lastDate = año.ToString() + "-12-31";
            }

            string idEmpresa = empresas[empresaActual].idEmpresa.ToString();
            string _empresa = empresas[empresaActual].razon_social;

            string departamento = CBDepartamento.SelectedItem.ToString();
            string _año = CBAños.SelectedItem.ToString();
            string _mes = CBMeses.SelectedItem.ToString();

            cassandra.DGV_ReporteNomina(dataGridReporte, departamento, _empresa, idEmpresa, 
                auxDepartamentos, firstDate, lastDate, _año, _mes);
            LCargando.Visible = false;
        }

        #region MisFunciones
        private void obtenerDiasMes(int _año, int _mes)
        {
            string date = _año.ToString() + "-" + _mes.ToString();

            int ultimoDiaMes = DateTime.DaysInMonth(_año, _mes);
            int primerDiaMes = ultimoDiaMes - (ultimoDiaMes - 1);

            firstDate = date + "-" + primerDiaMes.ToString();
            lastDate = date + "-" + ultimoDiaMes.ToString();
        }

        private void agregarCBAños()
        {
            CBAños.Items.Clear();
            string firstDate = empresas[empresaActual].fecha_inicio.ToString();
            int startIndex = 0;
            int length = 4;
            firstDate = firstDate.Substring(startIndex, length);
            int year = int.Parse(firstDate);
            for (int i = year; i <= añoActual; i++)
            {
                CBAños.Items.Add(i.ToString());
            }

            CBAños.SelectedItem = añoActual.ToString();
        }

        #endregion MisFunciones

        private void CBDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
