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
    public partial class Departamentos : Form
    {
        List<Empresa> empresas;
        List<Departamento> departamentos;
        List<Puesto> puestos;
        List<Empleado> empleados;
        EnlaceCassandra cassandra;
        int empresaActual = 0, departamentoActual = 0, puestoActual = 0;
        string idEmpresa = "";
        string idDepartamento = "";
        string idPuesto = "";
        //Contadores de departamentos y puestos
        int totalD = 0, totalP = 0;
        bool EDIT = false, EDIT2 = false;

        int TYPE_USER;

        public Departamentos()
        {
            InitializeComponent();
        }

        private void Departamentos_Load(object sender, EventArgs e)
        {
            TYPE_USER = Variables.TYPE_USER;
            empresas = Variables.EMPRESAS;
            departamentos = Variables.DEPARTAMENTOS;
            puestos = Variables.PUESTOS;

            if(TYPE_USER == 0) //Admi
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

            //Combo Empresas
            if (empresas.Count > 0)
            {
                cassandra = new EnlaceCassandra();
                for (int i = 0; i < empresas.Count; i++)
                {
                    CBEmpresas.Items.Add(empresas[i].razon_social.ToString());
                }
                CBEmpresas.SelectedIndex = 0;
            }
            else
            {
                LCargando.Visible = false;
                MessageBox.Show("No hay empresas registradas", "Aviso");
                btnAgregarD.Enabled = false;
                btnAgregarP.Enabled = false;
            }
            
        }

        private void dataGridDepartamentos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EDIT = true;
            string idDepto = dataGridDepartamentos.CurrentRow.Cells[0].Value.ToString();
            for (int i = 0; i < departamentos.Count; i++)
            {
                string auxIdDepartamento = departamentos[i].iddepartamento.ToString();
                //Comparar id de la lista departamentos conel id de el renglon de la datagrid
                if (auxIdDepartamento == idDepto)
                {
                    departamentoActual = i;
                    break;
                }
            }
            idDepartamento = departamentos[departamentoActual].iddepartamento.ToString();
            string departamento = departamentos[departamentoActual].departamento.ToString();
            txtDepartamento.Text = departamento;
            txtDepartamento.ReadOnly = false;
            if (departamento == "Administración")
            {
                txtDepartamento.ReadOnly = true;
                btnEliminarD.Visible = false;
            }
            else
            {
                btnEliminarD.Visible = true;
            }
            txtSueldoB.Text = departamentos[departamentoActual].sueldo_base.ToString();
            btnAgregarD.Text = "Actualizar departamento";
        }

        private void dataGridPuestos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EDIT2 = true;
            string _idPuesto = dataGridPuestos.CurrentRow.Cells[0].Value.ToString();
            for (int i = 0; i < puestos.Count; i++)
            {
                string auxIdPuesto = puestos[i].idpuesto.ToString();
                if (auxIdPuesto == _idPuesto)
                {
                    puestoActual = i;
                    break;
                }
            }
            idPuesto = puestos[puestoActual].idpuesto.ToString();
            string puesto = puestos[puestoActual].puesto.ToString();
            txtPuesto.Text = puesto;
            txtPuesto.ReadOnly = false;
            if (puesto == "Gerente")
            {
                txtPuesto.ReadOnly = true;
                btnEliminarP.Visible = false;
            }
            else
            {
                btnEliminarP.Visible = true;
            }
            txtNivelS.Text = puestos[puestoActual].nivel_salarial.ToString();
            btnAgregarP.Text = "Actualizar puesto";
        }

        private void btnAgregarD_Click(object sender, EventArgs e)
        {
            string idEmpresa = empresas[empresaActual].idEmpresa.ToString();
            string departamento = txtDepartamento.Text;
            string sueldoB = txtSueldoB.Text;
            bool succes = false;

            if (departamento != "" && sueldoB != "")
            {
                alertLabel.Visible = false;
                alertLabel2.Visible = false;
                LCargando.Visible = true;

                if (!EDIT)
                {
                    if (TYPE_USER == 0) //admi
                    {
                        succes = cassandra.AgregarDepartamento(idEmpresa, departamento, sueldoB, true);
                    }
                    else
                    {
                        succes = cassandra.AgregarDepartamento(idEmpresa, departamento, sueldoB, false);
                    }
                }
                else
                {
                    if (TYPE_USER == 0) //admi
                    {
                        succes = cassandra.EditarDepartamento(idEmpresa, idDepartamento, departamento, sueldoB, true);
                    }
                    else
                    {
                        succes = cassandra.EditarDepartamento(idEmpresa, idDepartamento, departamento, sueldoB, false);
                    }
                }


                if (succes)
                {
                    txtDepartamento.Text = "";
                    txtSueldoB.Text = "";
                    txtDepartamento.ReadOnly = false;
                    departamentos = Variables.DEPARTAMENTOS;
                    cargarDataGridDepartamentos();

                    if (!EDIT)
                    {
                        MessageBox.Show("Departamento agregado con exito", "Aviso");
                    }
                    else
                    {
                        MessageBox.Show("Departamento editado con exito", "Aviso");
                    }

                }
                else
                {
                    MessageBox.Show("No se pude realizar la operacion", "Error");
                }
                LCargando.Visible = false;
            }
            else
            {
                alertLabel.Visible = true;
            }
        }

        private void txtSueldoB_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e,1);
        }

        private void txtNivelS_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e,2);
        }

        private void btnAgregarP_Click(object sender, EventArgs e)
        {
            string puesto = txtPuesto.Text;
            string nivelS = txtNivelS.Text;
            bool succes = false;

            if (puesto != "" && nivelS != "")
            {
                alertLabel.Visible = false;
                alertLabel2.Visible = false;
                LCargando.Visible = true;
                if (!EDIT2)
                {
                    if (TYPE_USER == 0) //admi
                    {
                        succes = cassandra.AgregarPuesto(idEmpresa, puesto, nivelS, true);
                    }
                    else
                    {
                        succes = cassandra.AgregarPuesto(idEmpresa, puesto, nivelS, false);
                    }
                }
                else
                {
                    if (TYPE_USER == 0) //admi
                    {
                        succes = cassandra.EditarPuesto(idEmpresa, idPuesto, puesto, nivelS, true);
                    }
                    else
                    {
                        succes = cassandra.EditarPuesto(idEmpresa, idPuesto, puesto, nivelS, false);
                    }
                }

                if (succes)
                {
                    txtPuesto.Text = "";
                    txtNivelS.Text = "";
                    txtPuesto.ReadOnly = false;
                    puestos = Variables.PUESTOS;
                    cargarDataGridPuestos();
                    if (!EDIT2)
                    {
                        MessageBox.Show("Puesto agregado con exito", "Aviso");
                    }
                    else
                    {
                        MessageBox.Show("Puesto editado con exito", "Aviso");
                    }
                    
                }
                else
                {
                    MessageBox.Show("No se pude realizar la operacion", "Error");
                }
                LCargando.Visible = false;

            }
            else
            {
                alertLabel2.Visible = true;
            }
        }

        private void CBEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            LCargando.Visible = true;
            iniciarValores();
            empresaActual = CBEmpresas.SelectedIndex;
            idEmpresa = empresas[empresaActual].idEmpresa.ToString();

            cargarDataGridDepartamentos();
            cargarDataGridPuestos();

            LCargando.Visible = false;
        }

        private void btnEliminarD_Click(object sender, EventArgs e)
        {
            empleados = Variables.EMPLEADOS;
            bool vacio = true;
            for(int i = 0; i < empleados.Count; i++)
            {
                if(empleados[i].idEmpresa.ToString() == idEmpresa)
                {
                    if(empleados[i].idDepartamento.ToString() == idDepartamento)
                    {
                        vacio = false;
                        break;
                    }
                }
            }

            if (vacio)
            {
                DialogResult dialogResult = MessageBox.Show("¿Eliminar departamento?", 
                    "AVISO", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    LCargando.Visible = true;
                    bool sucess = cassandra.EliminarDepartamento(idEmpresa, idDepartamento);
                    if (sucess)
                    {
                        txtDepartamento.Text = "";
                        txtSueldoB.Text = "";
                        departamentos = Variables.DEPARTAMENTOS;
                        cargarDataGridDepartamentos();
                        LCargando.Visible = false;
                        MessageBox.Show("Departamento eliminado con exito", "AVISO");
                    }
                    else
                    {
                        LCargando.Visible = false;
                        MessageBox.Show("No se pudo realizar la operacion", "ERROR");
                    }
                }
            }
            else
            {
                MessageBox.Show("No debe de haber ningun empeado en el departamento", "ERROR");
            }
        }

        private void btnEliminarP_Click(object sender, EventArgs e)
        {
            empleados = Variables.EMPLEADOS;
            bool vacio = true;
            for (int i = 0; i < empleados.Count; i++)
            {
                if (empleados[i].idEmpresa.ToString() == idEmpresa)
                {
                    if (empleados[i].idPuesto.ToString() == idPuesto)
                    {
                        vacio = false;
                        break;
                    }
                }
            }

            if(vacio)
            {
                DialogResult dialogResult = MessageBox.Show("¿Eliminar puesto?", "AVISO", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    LCargando.Visible = true;
                    bool sucess  = cassandra.EliminarPuesto(idEmpresa, idPuesto);
                    if (sucess)
                    {
                        txtPuesto.Text = "";
                        txtNivelS.Text = "";
                        puestos = Variables.PUESTOS;
                        cargarDataGridPuestos();
                        LCargando.Visible = false;
                        MessageBox.Show("Puesto eliminado con exito", "AVISO");
                    }
                    else
                    {
                        LCargando.Visible = false;
                        MessageBox.Show("No se pudo realizar la operacion", "ERROR");
                    }
                }
            }
            else
            {
                MessageBox.Show("No debe de haber ningun empeado en el departamento", "ERROR");
            }

        }

        #region  MisFunciones
        private void soloNumeros(KeyPressEventArgs e, int opcion)
        {
            if (opcion == 1)
            {
                lDigitos.Visible = false;
            }
            else
            {
                lDigitos2.Visible = false;
            }

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

                if(opcion == 1)
                {
                    lDigitos.Visible = true;
                }
                else
                {
                    lDigitos2.Visible = true;
                }
            }
        }

        private void cargarDataGridDepartamentos()
        {
            totalD = 0;
            dataGridDepartamentos.Rows.Clear();
            for(int i = 0; i < departamentos.Count; i++)
            {
                string auxIdEmpresa = departamentos[i].idempresa.ToString();
                if(idEmpresa == auxIdEmpresa)
                {
                    string auxIdDepartamento = departamentos[i].iddepartamento.ToString();
                    string departamento = departamentos[i].departamento;
                    string sueldoB = departamentos[i].sueldo_base.ToString();
                    string gerente = departamentos[i].gerente;
                    dataGridDepartamentos.Rows.Add(auxIdDepartamento, departamento, sueldoB, gerente);
                    totalD++;
                }
            }

            //Si no hay departamentos, colocar departamento por default
            if(totalD == 0)
            {
                txtDepartamento.Text = "Administración";
                txtDepartamento.ReadOnly = true;
            }
        }

        private void cargarDataGridPuestos()
        {
            totalP = 0;
            dataGridPuestos.Rows.Clear();
            for (int i = 0; i < puestos.Count; i++)
            {
                string auxIdEmpresa = puestos[i].idempresa.ToString();
                if (idEmpresa == auxIdEmpresa)
                {
                    string auxIdPuesto = puestos[i].idpuesto.ToString();
                    string puesto = puestos[i].puesto;
                    string nivelS = puestos[i].nivel_salarial.ToString();
                    dataGridPuestos.Rows.Add(auxIdPuesto, puesto, nivelS);
                    totalP++;
                }
            }

            //Si no hay departamentos, colocar departamento por default
            if (totalP == 0)
            {
                txtPuesto.Text = "Gerente";
                txtPuesto.ReadOnly = true;
            }
        }

        private void iniciarValores()
        {
            txtDepartamento.Text = "";
            txtDepartamento.ReadOnly = false;
            txtSueldoB.Text = "";
            txtPuesto.Text = "";
            txtPuesto.ReadOnly = false;
            txtNivelS.Text = "";
            EDIT = false;
            EDIT2 = false;
            btnAgregarD.Text = "Agregar departamento";
            btnEliminarD.Visible = false;
            btnAgregarP.Text = "Agregar puesto";
            btnEliminarP.Visible = false;
        }

        #endregion Misfunciones
    }
}
