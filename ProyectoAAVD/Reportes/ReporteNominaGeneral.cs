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
    public partial class ReporteNominaGeneral : Form
    {
        List<Empresa> empresas;
        int empresaActual = 0;
        EnlaceCassandra cassandra;
        string firstDate = "";
        string lastDate = "";

        DateTime hoy;
        int añoActual = 0;
        public ReporteNominaGeneral()
        {
            InitializeComponent();
            hoy = DateTime.Now;
            string año = hoy.ToString("yyyy");
            añoActual = int.Parse(año);
        }

        private void ReporteNominaGeneral_Load(object sender, EventArgs e)
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
            if (empresas.Count > 0)
            {
                for (int i = 0; i < empresas.Count; i++)
                {
                    CBEmpresas.Items.Add(empresas[i].razon_social.ToString());
                }
                empresaActual = 0;
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

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            LCargando.Visible = true;
            if (dataGridReporte.Rows.Count > 0)
            {
                dataGridReporte.Rows.Clear();
            }

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

            cassandra = new EnlaceCassandra();
            cassandra.DGV_ReporteNominaGeneral(dataGridReporte, idEmpresa, firstDate, lastDate);

            LCargando.Visible = false;
        }

        private void CBEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            empresaActual = CBEmpresas.SelectedIndex;
            agregarCBAños();
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
    }
}
