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
    public partial class Login : Form
    {
        EnlaceCassandra cassandra;
        public Login()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            cassandra = new EnlaceCassandra();
            alertLabel.Visible = false;
            string user = txtUser.Text;
            string password = txtPassword.Text;

            if (user != "" && password != "")
            {
                this.alertLabel.Visible = false;

                bool find = false;
                find = cassandra.Sesion(user, password);

                if (find)
                {
                    this.Hide();
                    Principal pPrincipal = new Principal();
                    pPrincipal.FormClosed += logOut; //sobrecargar funcion en ventana principal
                    pPrincipal.ShowDialog();
                }
                else
                {
                    alertLabel.Text = "Usuario o contraseña incorrectos";
                    alertLabel.Visible = true;
                }
            }
            else
            {
                alertLabel.Text = "Usuario y contraseña obligatorios";
                alertLabel.Visible = true;
            }
        }

        private void logOut(object sender, FormClosedEventArgs e)
        {
            txtUser.Text = "";
            txtPassword.Text = "";
            this.Show();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
