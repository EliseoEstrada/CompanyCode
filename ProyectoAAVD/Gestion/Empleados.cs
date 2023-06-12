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
    public partial class Empleados : Form
    {
        List<Empresa> empresas;
        List<Departamento> departamentos;
        List<Puesto> puestos;
        List<Empleado> empleados;
        EnlaceCassandra cassandra;
        int empresaActual = 0;
        string departamentoActual = "";
        string puestoActual = "";
        int empleadoActual = 0;

        string idEmpresa;
        string idDepartamento;
        string idPuesto;
        int TYPE_USER;

        bool EDIT;
        string no_empleado = "";
        string antiguoCorreo = "";
        string antiguoPuesto = "";
        string antiguoDepartamentoId = "";

        public Empleados()
        {
            InitializeComponent();
        }

        private void Empleados_Load(object sender, EventArgs e)
        {
            TYPE_USER = Variables.TYPE_USER;
            empresas = Variables.EMPRESAS;
            departamentos = Variables.DEPARTAMENTOS;
            puestos = Variables.PUESTOS;
            empleados = Variables.EMPLEADOS;

            if (TYPE_USER == 0) //Admi
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

            if (empresas.Count > 0)
            {
                cassandra = new EnlaceCassandra();

                //Combo Bancos
                CBBanco.Items.Add("BANORTE");
                CBBanco.Items.Add("BANAMEX");
                CBBanco.Items.Add("BBVA BANCOMER");
                CBBanco.Items.Add("HSBC");
                CBBanco.Items.Add("AFIRME");
                CBBanco.Items.Add("BANREGIO");
                CBBanco.SelectedIndex = 0;

                //Combo Empresas
                for (int i = 0; i < empresas.Count; i++)
                {
                    CBEmpresas.Items.Add(empresas[i].razon_social.ToString());
                }

                empresaActual = 0;
                CBEmpresas.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No hay empresas registradas", "AVISO");
                btnAgregar.Enabled = false;
            }
            LCargando.Visible = false;
        }

        private void CBDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            departamentoActual = CBDepartamento.SelectedItem.ToString();
            for(int i = 0; i <departamentos.Count; i++)
            {
                string auxDepartamento = departamentos[i].departamento;
                string auxIdEmpresa = departamentos[i].idempresa.ToString();
                if (departamentoActual == auxDepartamento &&
                    idEmpresa == auxIdEmpresa)
                {
                    txtSueldoB.Text = departamentos[i].sueldo_base.ToString();
                    //Variable global
                    idDepartamento = departamentos[i].iddepartamento.ToString();
                    break;
                }
            }
            calcularSueldoD();
        }

        private void CBPuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            puestoActual = CBPuesto.SelectedItem.ToString();
            for (int i = 0; i < puestos.Count; i++)
            {
                string auxPuesto = puestos[i].puesto;
                string auxIdEmpresa = puestos[i].idempresa.ToString();
                if (puestoActual == auxPuesto &&
                    idEmpresa == auxIdEmpresa)
                {
                    txtNivelSalarial.Text = puestos[i].nivel_salarial.ToString();
                    //Variable global
                    idPuesto = puestos[i].idpuesto.ToString();
                    break;
                }
            }
            calcularSueldoD();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //string idDepartamento = departamentos[departamentoActual].iddepartamento.ToString();
            //string idPuesto = puestos[puestoActual].idpuesto.ToString();
            string nombre = txtNombre.Text;
            string apellidos = txtApellido.Text;
            string fechaN = dateTimePicker1.Text;
            string CURP = txtCURP.Text;
            string NSS = txtNSS.Text;
            string RFC = txtRFC.Text;
            string domicilio = txtDomicilio.Text;
            string email = txtEmail.Text;
            string telefono = txtTelefono.Text;
            string celular = txtCelular.Text;
            string empresa = empresas[empresaActual].razon_social.ToString();
            string departamento = CBDepartamento.SelectedItem.ToString();
            string puesto = CBPuesto.SelectedItem.ToString();
            string nivelS = txtNivelSalarial.Text;
            string sueldoD = txtSueldoD.Text;
            string fechaI = txtFechaInicio.Text;
            string banco = CBBanco.SelectedItem.ToString();
            string noCuenta = txtNoCuenta.Text;
            string contraseña = txtContraseña.Text;

            if (nombre != "" && apellidos != "" && fechaN != "" && CURP != "" && NSS != "" &&
                RFC != "" && domicilio != "" && email != "" && telefono != "" && celular != "" &&
                empresa != "" && departamento != "" && puesto != "" && nivelS != "" && sueldoD != ""
                && fechaI != "" && banco != "" && noCuenta != "" && contraseña != "")
            {
                bool sucess = false;
                alertLabel.Visible = false;
                LCargando.Visible = true;

                bool admin = false;
                if(TYPE_USER == 0)
                {
                    admin = true;
                }

                if (!EDIT)
                {
                    sucess = cassandra.AgregarEmpleado(idEmpresa, idPuesto, idDepartamento, contraseña,
                        nombre, apellidos, fechaN, CURP, NSS, RFC, domicilio, banco, noCuenta,
                        email, telefono, celular, fechaI, puesto, admin);
                }
                else
                {
                    sucess = cassandra.EditarEmpleado(idEmpresa, no_empleado, idPuesto, idDepartamento, contraseña,
                        nombre, apellidos, fechaN, CURP, NSS, RFC, domicilio, banco, noCuenta,
                        email, telefono, celular, fechaI, puesto, admin, antiguoCorreo, antiguoPuesto, antiguoDepartamentoId);
                }

                if (sucess)
                {
                    empleados = Variables.EMPLEADOS;
                    cargarDataGridEmpleados();

                    if (!EDIT)
                    {
                        MessageBox.Show("Empleado agregado con exito", "AVISO");
                    }
                    else
                    {
                        MessageBox.Show("Empleado editado con exito", "AVISO");
                    }

                    iniciarValores();
                    cargarCBDepartamentos();
                    cargarCBPuestos();
                }
                else
                {
                    MessageBox.Show("No se pudo completar la operacion", "ERROR");
                }
            }
            else
            {
                alertLabel.Visible = true;
            }
        }

        private void dataGridEmpleados_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EDIT = true;
            string idEmpleado = dataGridEmpleados.CurrentRow.Cells[0].Value.ToString();
            for (int i = 0; i < empleados.Count; i++)
            {
                //Comparar id de la lista empleados conel id de el renglon de la datagrid
                string auxIdEmpleado = empleados[i].no_empleado.ToString();
                if (auxIdEmpleado == idEmpleado)
                {
                    empleadoActual = i;
                    txtNombre.Text = empleados[empleadoActual].Nombre;
                    txtApellido.Text = empleados[empleadoActual].Apellidos;
                    dateTimePicker1.Value = Convert.ToDateTime(empleados[empleadoActual].fecha_nacimiento.ToString());
                    txtTelefono.Text = empleados[empleadoActual].telefonos[0].ToString();
                    txtCelular.Text = empleados[empleadoActual].telefonos[1].ToString();
                    txtCURP.Text = empleados[empleadoActual].curp;
                    txtNSS.Text = empleados[empleadoActual].nss;
                    txtRFC.Text = empleados[empleadoActual].rfc;
                    txtEmail.Text = empleados[empleadoActual].email;
                    txtContraseña.Text = empleados[empleadoActual].Password;
                    txtDomicilio.Text = empleados[empleadoActual].domicilio;


                    //Recorrer departamentos para encontrar el del empleado
                    string idDepto = empleados[empleadoActual].idDepartamento.ToString();
                    for(int j =0; j <departamentos.Count; j++)
                    {
                        string axuIdDepto = departamentos[j].iddepartamento.ToString();
                        if (idDepto == axuIdDepto)
                        {
                            CBDepartamento.SelectedItem = departamentos[j].departamento;
                            break;
                        }
                    }

                    //Recorrer puestos para encontrar el del empleado
                    string idPsto = empleados[empleadoActual].idPuesto.ToString();
                    for (int j = 0; j < puestos.Count; j++)
                    {
                        string axuIdPsto = puestos[j].idpuesto.ToString();
                        if (idPsto == axuIdPsto)
                        {
                            CBPuesto.SelectedItem = puestos[j].puesto;
                            //Variables para editar y eliminar
                            antiguoPuesto = puestos[j].puesto;
                            break;
                        }
                    }

                    CBBanco.SelectedItem = empleados[empleadoActual].banco;
                    txtNoCuenta.Text = empleados[empleadoActual].numero_cuenta.ToString();
                    txtFechaInicio.Text = empleados[empleadoActual].fecha_inicio.ToString();

                    //Variables para editar y eliminar
                    no_empleado = empleados[empleadoActual].no_empleado.ToString();
                    antiguoCorreo = empleados[empleadoActual].email;
                    antiguoDepartamentoId = empleados[empleadoActual].idDepartamento.ToString();

                    btnEliminar.Visible = true;
                    btnAgregar.Text = "Guardar cambios";
                    break;
                }
            }
        }

        private void CBEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            empresaActual = CBEmpresas.SelectedIndex;
            idEmpresa = empresas[empresaActual].idEmpresa.ToString();
            cargarDataGridEmpleados();
            iniciarValores();
            cargarCBDepartamentos();
            cargarCBPuestos();

            DateTime fechaInicio = Convert.ToDateTime(empresas[empresaActual].ultima_nomina.ToString());
            txtFechaInicio.Text = fechaInicio.ToString("yyyy-MM-dd");
            LCargando.Visible = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Eliminar empleado?", "AVISO", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                LCargando.Visible = true;
                bool succes = cassandra.EliminarEmpleado(idEmpresa, no_empleado, antiguoCorreo,
                antiguoDepartamentoId, antiguoPuesto);

                if (succes)
                {
                    empleados = Variables.EMPLEADOS;
                    cargarDataGridEmpleados();
                    iniciarValores();
                    cargarCBDepartamentos();
                    cargarCBPuestos();
                    MessageBox.Show("Empleado eliminado con exito", "AVISO");
                }
                else
                {
                    LCargando.Visible = false;
                    MessageBox.Show("No se pudo completar la operacion", "ERROR");
                }
            }
           
        }

        #region MisFunciones

        private void calcularSueldoD()
        {
            if (txtNivelSalarial.Text != "" && txtSueldoB.Text != "")
            {
                decimal nivelSalarial = decimal.Parse(txtNivelSalarial.Text);
                decimal sueldoBase = decimal.Parse(txtSueldoB.Text);
                decimal sueldoDiario = nivelSalarial * sueldoBase;
                txtSueldoD.Text = sueldoDiario.ToString();
            }
        }

        private void iniciarValores()
        {
            LCargando.Visible = true;
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtTelefono.Text = "";
            txtCelular.Text = "";
            txtCURP.Text = "";
            txtNSS.Text = "";
            txtRFC.Text = "";
            txtEmail.Text = "";
            txtContraseña.Text = "";
            txtDomicilio.Text = "";
            txtNivelSalarial.Text = "";
            txtSueldoB.Text = "";
            txtSueldoD.Text = "";
            txtNoCuenta.Text = "";
            EDIT = false;
            CBDepartamento.Enabled = true;
            CBPuesto.Enabled = true;
            btnEliminar.Visible = false;
            btnAgregar.Enabled = true;
            btnAgregar.Text = "Agregar empleado";
            LCargando.Visible = false;
        }

        private void cargarDataGridEmpleados()
        {
            dataGridEmpleados.Rows.Clear();
            for(int i = 0; i < empleados.Count; i++)
            {
                string auxIdEmpresa = empleados[i].idEmpresa.ToString();
                if (idEmpresa == auxIdEmpresa)
                {
                    string auxIdEmpleado = empleados[i].no_empleado.ToString();
                    string nombre = empleados[i].Nombre;
                    string apellido = empleados[i].Apellidos;
                    string fechaN = empleados[i].fecha_nacimiento.ToString();
                    string curp = empleados[i].curp;
                    string nss = empleados[i].nss;
                    string rfc = empleados[i].rfc;
                    dataGridEmpleados.Rows.Add(auxIdEmpleado, nombre, apellido, fechaN, curp, nss, rfc);
                }
            }

        }

        private void cargarCBDepartamentos()
        {
            CBDepartamento.Items.Clear();
            for (int i = 0; i < departamentos.Count; i++)
            {
                string auxIdEmpresa = departamentos[i].idempresa.ToString();
                if(auxIdEmpresa == idEmpresa)
                {
                    CBDepartamento.Items.Add(departamentos[i].departamento);
                }
            }

            if (CBDepartamento.Items.Count > 0)
            {
                if (dataGridEmpleados.Rows.Count == 0) //Primer empleado
                {
                    CBDepartamento.SelectedItem = "Administración";
                    CBDepartamento.Enabled = false;
                }
                else
                {
                    CBDepartamento.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("No hay departamentos registrados", "AVISO");
                btnAgregar.Enabled = false;
            }

        }

        private void cargarCBPuestos()
        {
            CBPuesto.Items.Clear();
            for (int i = 0; i < puestos.Count; i++)
            {
                string auxIdEmpresa = puestos[i].idempresa.ToString();
                if (auxIdEmpresa == idEmpresa)
                {
                    CBPuesto.Items.Add(puestos[i].puesto);
                }
            }

            if (CBPuesto.Items.Count > 0)
            {
                if (dataGridEmpleados.Rows.Count == 0) //Primer empleado
                {
                    CBPuesto.SelectedItem = "Gerente";
                    CBPuesto.Enabled = false;
                }
                else
                {
                    CBPuesto.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("No hay puestos registrados", "AVISO");
                btnAgregar.Enabled = false;
            }
        }

        private void soloNumeros(KeyPressEventArgs e, int opcion)
        {
            if(opcion == 1)
            {
                lDigitos.Visible = false;
            }
            else if (opcion == 2)
            {
                lDigitos2.Visible = false;
            }
            else
            {
                lDigitos3.Visible = false;
            }

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

                if (opcion == 1)
                {
                    lDigitos.Visible = true;
                }
                else if (opcion == 2)
                {
                    lDigitos2.Visible = true;
                }
                else
                {
                    lDigitos3.Visible = true;
                }
            }
        }

        #endregion MisFunciones

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e,1);
        }

        private void txtNoCuenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e,3);
        }

        private void txtCelular_KeyPress(object sender, KeyPressEventArgs e)
        {
            soloNumeros(e,2);
        }
    }
}
