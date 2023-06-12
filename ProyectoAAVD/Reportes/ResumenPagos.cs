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
    public partial class ResumenPagos : Form
    {
        string COMPANY;
        string EMPLOYEE;
        DateTime fechaInicio;
        DateTime ultimaNomina;

        EnlaceCassandra cassandra;

        public ResumenPagos()
        {
            InitializeComponent();
        }

        private void ResumenPagos_Load(object sender, EventArgs e)
        {
            COMPANY = Variables.COMPANY;
            EMPLOYEE = Variables.EMPLOYEE;
            lEmpresa.Text = COMPANY;
            lEmpleado.Text = EMPLOYEE;

            //CB Años
            fechaInicio = DateTime.Parse(Variables.EMPLOYEE_INFO[0].fecha_inicio.ToString());
            ultimaNomina = DateTime.Parse(Variables.EMPLOYEE_INFO[0].ultima_nomina.ToString());

            string _fechaInicio = fechaInicio.ToString("yyyy");
            string _ultimaNomina = ultimaNomina.ToString("yyyy");
            int yearI = int.Parse(_fechaInicio);
            int yearF = int.Parse(_ultimaNomina);
            for (int i = yearI; i <= yearF; i++)
            {
                CBAños.Items.Add(i.ToString());
                CBAños.SelectedItem = i.ToString();
            }

            cassandra = new EnlaceCassandra();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            LCargando.Visible = true;
            string año = CBAños.SelectedItem.ToString();

            string firstDate = año + "-01-01";
            string lastDate = año + "-12-31";

            cassandra.DGV_ResumenPagos(dataGridReporte, año, firstDate, lastDate);
            LCargando.Visible = false;
        }
    }
}
