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
    public partial class Principal : Form
    {

        private Form activeForm = null;
        string USER;
        int TIPEUSER;
        bool cerrarSesion;

        public Principal()
        {
            InitializeComponent();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            USER = Variables.USER;
            TIPEUSER = Variables.TYPE_USER;
            panelReportes.Visible = false;
            cerrarSesion = false;

            //Cargar nombre de usuario
            btnUser.Text = USER;
            if (TIPEUSER == 0)//Admi
            {
                //cassandra.ObtenerEDPE("", true);
                btnResumenPagos.Visible = false;
                btnReciboNomina.Visible = false;
                btnTipoUsuario.Text = "Gerente general";
            }
            else
            {
                if (TIPEUSER == 1)
                {
                    btnEmpresas.Visible = false;
                    btnTipoUsuario.Text = "Gerente de nómina";
                }
                if (TIPEUSER == 2)
                {
                    btnEmpresas.Visible = false;
                    btnDepartamentos.Visible = false;
                    btnNominas.Visible = false;
                    btnEmpleados.Visible = false;
                    btnReportes.Visible = false;
                    btnTipoUsuario.Text = "Empleado";
                }
            }
            

        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            if (panelReportes.Visible == false)
            {
                panelReportes.Visible = true;
            }
            else
            {
                panelReportes.Visible = false;
            }
        }

        private void openChildForm(Form _formHijo)
        {
            if (activeForm != null) //si existe un form abierto lo cerramos
            {
                activeForm.Close();
            }

            activeForm = _formHijo; //guardar formulario activo
            _formHijo.TopLevel = false; //indicar que el formulario se comportara como un control
            _formHijo.FormBorderStyle = FormBorderStyle.None;
            _formHijo.Dock = DockStyle.Fill; //rellenar todo el panel
            panelPrincipal.Controls.Add(_formHijo); //agregar al panel
            panelPrincipal.Tag = _formHijo; //asociar el formulario con el panel
            _formHijo.BringToFront(); //mostrarlo al frente
            _formHijo.Show();
        }

        private void btnEmpresas_Click(object sender, EventArgs e)
        {
            openChildForm(new Empresas());
        }

        private void btnDepartamentos_Click(object sender, EventArgs e)
        {
            openChildForm(new Departamentos());
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            openChildForm(new Empleados());
        }

        private void btnNominas_Click(object sender, EventArgs e)
        {
            openChildForm(new GenerarNominas());
        }

        private void btnReporteNominaG_Click(object sender, EventArgs e)
        {
            openChildForm(new ReporteNominaGeneral());
        }

        private void btnNominaHeadcounter_Click(object sender, EventArgs e)
        {
            openChildForm(new ReporteHeadcounter());
        }

        private void btnReporteNomina_Click(object sender, EventArgs e)
        {
            openChildForm(new ReporteNomina());
        }

        private void btnResumenPagos_Click(object sender, EventArgs e)
        {
            openChildForm(new ResumenPagos());
        }

        private void btnReciboNomina_Click(object sender, EventArgs e)
        {
            openChildForm(new RecibosNomina());
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Cerrar sesión?",
                   "AVISO", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                cerrarSesion = true;
                this.Close();
            }
        }

        private void Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!cerrarSesion)
            {
                DialogResult dialogResult = MessageBox.Show("¿Cerrar sesión?",
                   "AVISO", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    cerrarSesion = true;
                    this.Close();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
