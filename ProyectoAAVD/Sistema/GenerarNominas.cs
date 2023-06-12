using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace ProyectoAAVD
{
    public partial class GenerarNominas : Form
    {
        List<Empresa> empresas;
        List<Departamento> departamentos;
        List<Puesto> puestos;
        List<Empleado> empleados;
        EnlaceCassandra cassandra;
        string idEmpresa;
        string idEmpleado;
        int empresaActual = 0;
        DateTime fechaNuevaN;
        int diasFrecuencia;
        decimal sueldoBase;
        decimal totalPercepciones;
        decimal totalDeducciones;
        decimal totalNeto;

        string empresaSeleccionada = "";
        int TYPE_USER;

        List<Tuple<string, //idEmpleado
            string, //descripcion
            string, //tipo
            string>> ListPercepciones;//monto
        List<Tuple<string, string, string, string>> ListDeducciones;
        
        List<Tuple<
            string, //Id
            string, //puesto
            decimal, //Sueldo diario
            int, //diasL
            decimal, //tPercepciones
            decimal, //tDeducciones
            decimal>> ListEmpleados; //tNeto

        bool cambioFrecuencia = false;

        public GenerarNominas()
        {
            InitializeComponent();
        }

        private void GenerarNominas_Load(object sender, EventArgs e)
        {
            empresas = Variables.EMPRESAS;
            departamentos = Variables.DEPARTAMENTOS;
            puestos = Variables.PUESTOS;

            TYPE_USER = Variables.TYPE_USER;
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
                //Combo empresas
                CBEmpresas.Items.Clear();
                for (int i = 0; i < empresas.Count; i++)
                {
                    CBEmpresas.Items.Add(empresas[i].razon_social.ToString());
                }
                CBEmpresas.SelectedIndex = empresaActual;
                empresaSeleccionada = CBEmpresas.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("No hay empresas registradas", "AVISO");
                btnGenerarN.Enabled = false;
                btnGuardar.Enabled = false;
            }
        }

        private void CBFrecuencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Actualizar CB dias laborados
            string frecuencia = CBFrecuencia.SelectedItem.ToString();
            if (frecuencia == "Semanal")
            {
                diasFrecuencia = 7;
            }
            else if (frecuencia == "Catorcenal")
            {
                diasFrecuencia = 14;
            }
            else if (frecuencia == "Quincenal")
            {
                diasFrecuencia = 15;
            }
            else if (frecuencia == "Mensual")
            {
                diasFrecuencia = 30;
            }

            //CB dias laborados
            CBDiasLaborados.Items.Clear();
            for (int i = 1; i <= diasFrecuencia; i++)
            {
                CBDiasLaborados.Items.Add(i.ToString());
            }

            //Seleccionar dias por default
            if (diasFrecuencia == 7)
            {
                CBDiasLaborados.SelectedIndex = diasFrecuencia - 2; //6 dias
            }
            else if (diasFrecuencia == 14)
            {
                CBDiasLaborados.SelectedIndex = diasFrecuencia - 2; //12 dias
            }
            else if (diasFrecuencia == 15)
            {
                CBDiasLaborados.SelectedIndex = diasFrecuencia - 3; //13 dias
            }
            else if (diasFrecuencia == 30)
            {
                CBDiasLaborados.SelectedIndex = diasFrecuencia - 5; //26 dias
            }

            //Calcular fechas de nomina
            calcularFechas();

            if (cambioFrecuencia)
            {
                reiniciarTuplas();
            }

            cambioFrecuencia = true;
        }

        private void CBDiasLaborados_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Si es la segunda vez que se cambia la frecuencia
            if(cambioFrecuencia && txtEmpleado.Text != "")
            {
                decimal sueldoD = decimal.Parse(txtSueldoD.Text);
                int diasLaborados = (CBDiasLaborados.SelectedIndex + 1);
                txtDiasL.Text = diasLaborados.ToString();
                //Calcular sueldo (sueldo diario * dias laborados)
                sueldoBase = sueldoD * diasLaborados;
                sueldoBase = Math.Round(sueldoBase, 2);
                
                //ACTUALIZAR VALORES DE MONTOS 
                foreach (DataGridViewRow row in dataGridPercepcionN.Rows)
                {
                    string descripcion = row.Cells[0].Value.ToString();
                    string tipoP = row.Cells[1].Value.ToString();
                    //Actualizar DG de sueldo base
                    if (descripcion == "Sueldo")
                    {
                        row.Cells[2].Value = sueldoBase;
                    }
                    //Actualizar DG de percepciones porcentuales
                    if (tipoP != "Monto fijo")
                    {
                        decimal auxTipo = decimal.Parse(tipoP);
                        auxTipo = auxTipo * sueldoBase;
                        auxTipo = Math.Round(auxTipo, 2);
                        row.Cells[2].Value = auxTipo.ToString();
                    }
                }
                foreach (DataGridViewRow row in dataGridDeduccionN.Rows)
                {
                    string tipoD = row.Cells[1].Value.ToString();
                    //Actualizar DG de deducciones porcentuales
                    if (tipoD != "Monto fijo")
                    {
                        decimal auxTipo = decimal.Parse(tipoD);
                        auxTipo = auxTipo * sueldoBase;
                        auxTipo = Math.Round(auxTipo, 2);
                        row.Cells[2].Value = auxTipo.ToString();
                    }
                }

                actualizarMontos();
            }
        }

        private void dataGridEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            idEmpleado = dataGridEmpleados.CurrentRow.Cells[0].Value.ToString();
            string nombre = dataGridEmpleados.CurrentRow.Cells[1].Value.ToString(); 
            string apellidos= dataGridEmpleados.CurrentRow.Cells[2].Value.ToString();
            dataGridPercepcionN.Rows.Clear();
            dataGridDeduccionN.Rows.Clear();
            //Colocar datos de empleado
            for (int i = 0; i < ListEmpleados.Count; i++)
            {
                string auxIdEmpleado = ListEmpleados[i].Item1;
                if(auxIdEmpleado == idEmpleado)
                {
                    txtEmpleado.Text = nombre + " " + apellidos;
                    txtPuesto.Text = ListEmpleados[i].Item2;
                    txtSueldoD.Text = ListEmpleados[i].Item3.ToString();
                    txtDiasL.Text = ListEmpleados[i].Item4.ToString();
                    txtTotalPerpcepciones.Text = ListEmpleados[i].Item5.ToString();
                    txtTotalDeducciones.Text = ListEmpleados[i].Item6.ToString();
                    txtNeto.Text = ListEmpleados[i].Item7.ToString();
                }
            }

            //Colocar percepciones y deducciones del empleado
            string descripcion;
            string tipoIncidencia;
            string monto;
            //Percepciones
            for (int i = 0; i < ListPercepciones.Count; i++)
            {
                if (idEmpleado == ListPercepciones[i].Item1)
                {
                    descripcion = ListPercepciones[i].Item2;
                    tipoIncidencia = ListPercepciones[i].Item3;
                    monto = ListPercepciones[i].Item4;
                    dataGridPercepcionN.Rows.Add(descripcion, tipoIncidencia, monto);

                    if (descripcion == "Sueldo")
                    {
                        //Variable pra agregar o quitar incidencias porcentuales
                        sueldoBase = decimal.Parse(monto);
                    }

                }
            }
            //Deducciones
            for (int i = 0; i < ListDeducciones.Count; i++)
            {
                if (idEmpleado == ListDeducciones[i].Item1)
                {
                    descripcion = ListDeducciones[i].Item2;
                    tipoIncidencia = ListDeducciones[i].Item3;
                    monto = ListDeducciones[i].Item4;
                    dataGridDeduccionN.Rows.Add(descripcion, tipoIncidencia, monto);
                }
            }
        }

        private void dataGridPercepcion_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (txtEmpleado.Text != "")
            {
                string percepcion = dataGridPercepcion.CurrentRow.Cells[0].Value.ToString();
                string monto = dataGridPercepcion.CurrentRow.Cells[1].Value.ToString();
                string tipoIncidencia = "Monto fijo";

                bool repetido = false;
                foreach (DataGridViewRow row in dataGridPercepcionN.Rows)
                {
                    if (row.Cells[0].Value.ToString() == percepcion)
                    {
                        repetido = true;
                    }
                }

                if (percepcion != "Sueldo" && percepcion != "Bono gerente" &&
                    percepcion != "Prima vacacional" && repetido == false)
                {
                    //Convertir monto porcentual en fijo
                    decimal auxMonto = decimal.Parse(monto);
                    if (auxMonto < 1) //PORCENTUAL
                    {
                        auxMonto = auxMonto * sueldoBase;
                        auxMonto = Math.Round(auxMonto, 2);
                        tipoIncidencia = monto;
                        monto = auxMonto.ToString();
                    }
                    dataGridPercepcionN.Rows.Add(percepcion, tipoIncidencia, monto);
                }

                //Agregar prima vacacional
                if(percepcion == "Prima vacacional" && repetido == false)
                {

                    bool primaDisponible = false;
                    string _fechaInicio = txtFechaInicio.Text;
                    DateTime fechaUltimaPrima;
                    DateTime fechaInicio = Convert.ToDateTime(_fechaInicio);
                    string fechaD = "";

                    for (int i = 0; i < empleados.Count; i++)
                    {
                        if(empleados[i].no_empleado.ToString() == idEmpleado)
                        {
                            if (empleados[i].prima_vacacional != null)
                            {
                                string auxFecha = empleados[i].prima_vacacional.ToString();
                                fechaUltimaPrima = Convert.ToDateTime(auxFecha);
                                DateTime fechaPrimaDisponible = fechaUltimaPrima.AddYears(1);
                                fechaD = fechaPrimaDisponible.ToString("yyyy-MM-dd");
                                if (fechaPrimaDisponible <= fechaInicio)
                                {
                                    primaDisponible = true;
                                }
                            }
                            else
                            {
                                primaDisponible = true;
                            }
                            break;
                        }
                    }

                    if (primaDisponible)
                    {
                        //Convertir monto porcentual en fijo
                        decimal auxMonto = decimal.Parse(monto);
                        if (auxMonto < 1) //PORCENTUAL
                        {
                            auxMonto = auxMonto * sueldoBase;
                            auxMonto = Math.Round(auxMonto, 2);
                            tipoIncidencia = monto;
                            monto = auxMonto.ToString();
                        }
                        dataGridPercepcionN.Rows.Add(percepcion, tipoIncidencia, monto);
                    }
                    else
                    {
                        MessageBox.Show("La prima vacacional se puede aplicar hasta el " +
                            fechaD, "ERROR");
                    }
                }

                actualizarMontos();
            }
        }

        private void dataGridDeduccion_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (txtEmpleado.Text != "")
            {
                string deduccion = dataGridDeduccion.CurrentRow.Cells[0].Value.ToString();
                string monto = dataGridDeduccion.CurrentRow.Cells[1].Value.ToString();
                string tipoIncidencia = "Monto fijo";

                bool repetido = false;
                foreach (DataGridViewRow row in dataGridDeduccionN.Rows)
                {
                    if (row.Cells[0].Value.ToString() == deduccion)
                    {
                        repetido = true;
                    }
                }

                if (deduccion != "IMSS" && deduccion != "ISR" && repetido == false)
                {
                    //Convertir monto porcentual en fijo
                    decimal auxMonto = decimal.Parse(monto);
                    if (auxMonto < 1) //PORCENTUAL
                    {
                        auxMonto = auxMonto * sueldoBase;
                        auxMonto = Math.Round(auxMonto, 2);
                        tipoIncidencia = monto;
                        monto = auxMonto.ToString();
                    }
                    dataGridDeduccionN.Rows.Add(deduccion, tipoIncidencia, monto);
                }

                actualizarMontos();
            }
        }

        private void dataGridPercepcionN_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string percepcion = dataGridPercepcionN.CurrentRow.Cells[0].Value.ToString();
            if (percepcion != "Sueldo base" && percepcion != "Bono gerente")
            {
                dataGridPercepcionN.Rows.RemoveAt(dataGridPercepcionN.CurrentRow.Index);
                actualizarMontos();
            }
            else
            {
                MessageBox.Show("No se pueden borrar estas percepcion", "Aviso");
            }
        }

        private void dataGridDeduccionN_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string deduccion = dataGridDeduccionN.CurrentRow.Cells[0].Value.ToString();
            if (deduccion != "IMSS" && deduccion != "ISR")
            {
                dataGridDeduccionN.Rows.RemoveAt(dataGridDeduccionN.CurrentRow.Index);
                actualizarMontos();
            }
            else
            {
                MessageBox.Show("No se puede borrar esta deducciones", "Aviso");
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtEmpleado.Text != "")
            {
                int diasLaborados = int.Parse(txtDiasL.Text);
                int eliminarPercepcion = 0;
                int eliminarDeduccion = 0;

                //Guardar total de tupla a eliminar
                for (int i = 0; i < ListPercepciones.Count; i++)
                {
                    if (idEmpleado == ListPercepciones[i].Item1)
                    {
                        eliminarPercepcion++;
                    }
                }
                for (int i = 0; i < ListDeducciones.Count; i++)
                {
                    if (idEmpleado == ListDeducciones[i].Item1)
                    {
                        eliminarDeduccion++;
                    }
                }

                //Eliminar percepciones y deducciones de la tupla
                for (int i = 0; i < eliminarPercepcion; i++)
                {
                    for (int j = 0; j < ListPercepciones.Count; j++)
                    {
                        if (idEmpleado == ListPercepciones[j].Item1)
                        {
                            ListPercepciones.RemoveAt(j);
                            break;
                        }
                    }
                }
                for (int i = 0; i < eliminarDeduccion; i++)
                {
                    for (int j = 0; j < ListDeducciones.Count; j++)
                    {
                        if (idEmpleado == ListDeducciones[j].Item1)
                        {
                            ListDeducciones.RemoveAt(j);
                            break;
                        }
                    }
                }

                //Agregar nuevas percepciones y deducciones
                string descripcion;
                string tipoMonto;
                string monto;
                foreach (DataGridViewRow row in dataGridPercepcionN.Rows)
                {
                    descripcion = row.Cells[0].Value.ToString();
                    tipoMonto = row.Cells[1].Value.ToString();
                    monto = row.Cells[2].Value.ToString();
                    var tuple = Tuple.Create(idEmpleado, descripcion, tipoMonto, monto);
                    ListPercepciones.Add(tuple);
                }
                foreach (DataGridViewRow row in dataGridDeduccionN.Rows)
                {
                    descripcion = row.Cells[0].Value.ToString();
                    tipoMonto = row.Cells[1].Value.ToString();
                    monto = row.Cells[2].Value.ToString();
                    var tuple = Tuple.Create(idEmpleado, descripcion, tipoMonto, monto);
                    ListDeducciones.Add(tuple);
                }

                //Actualizar 
                for (int i = 0; i < ListEmpleados.Count; i++)
                {
                    if (idEmpleado == ListEmpleados[i].Item1)
                    {
                        string departamento = ListEmpleados[i].Item2;
                        decimal salarioD = ListEmpleados[i].Item3;
                        var tuple = Tuple.Create(idEmpleado, departamento, salarioD, diasLaborados, 
                            totalPercepciones, totalDeducciones, totalNeto);
                        ListEmpleados[i] = tuple;
                        break;
                    }
                }

                MessageBox.Show("Nomina de empleado actualizada", "AVISO");

            }
        }

        private void btnGenerarN_Click(object sender, EventArgs e)
        {
            List<Empleado> empleadosNomina = new List<Empleado>();
            List<Tuple<string, //IdEmpleado
                int, //dias laborados
                string, //incidencias
                decimal, //total percepciones
                decimal, //total deducciones
                decimal, //Total bruto
                decimal>> //Total neto
                listMontosNominas =
                new List<Tuple<string, int, string, decimal, decimal, decimal, decimal>>();

            List<Tuple<
                string, //Id empleado
                string, //departamento
                string, //puesto
                decimal, //sueldo d
                bool> //Prima vacacional
                > listDatosEmpleado = new List<Tuple<string, string, string, decimal, bool>>();

            DateTime now = DateTime.Now;
            string hoy = now.ToString("yyyy-MM-dd");
            string frecuecncia = CBFrecuencia.SelectedItem.ToString();
            string fechaInicio = txtFechaInicio.Text;
            string fechaFin = txtFechaFinal.Text;
            string idEmpresa = empresas[empresaActual].idEmpresa.ToString();
            string fechaNuevaNomina = fechaNuevaN.ToString("yyyy-MM-dd");
            string descripcion = "";
            string monto = "";
            string puesto = "";
            int diasLaborados = 0;
            decimal sueldoD = 0;
            decimal totalP = 0;
            decimal totalD = 0;
            decimal totalB = 0;
            decimal totalN = 0;

            //Crear lista de los empleados a generar la nomina
            for (int i = 0; i < empleados.Count; i++)
            {
                string auxIdEmpresa = empleados[i].idEmpresa.ToString();
                //Si el empleado pertenece a la empresa actual
                if (auxIdEmpresa == idEmpresa)
                {
                    empleadosNomina.Add(empleados[i]);
                }
            }

            //Preparar datos de cada empleado
            for (int i = 0; i < empleadosNomina.Count; i++)
            {
                string _idEmpleado = empleadosNomina[i].no_empleado.ToString();
                string idDepto = empleadosNomina[i].idDepartamento.ToString();

                string percepciones = "";
                string deducciones = "";
                string Incidencia = "";

                bool primaVacacional = false;

                //Crear string de percepcioens y deducciones
                for (int j = 0; j < ListPercepciones.Count; j++)
                {
                    if (_idEmpleado == ListPercepciones[j].Item1)
                    {
                        descripcion = ListPercepciones[j].Item2;
                        monto = ListPercepciones[j].Item4;
                        percepciones = armarIncidencias(percepciones, "Percepcion", descripcion, monto, false);
                        if (descripcion == "Sueldo")
                        {
                            totalB = decimal.Parse(ListPercepciones[j].Item4);
                        }
                        //Si esta la prima actualizarlarla en la lista
                        if(descripcion == "Prima vacacional")
                        {
                            primaVacacional = true;
                        }

                    }
                }
                for (int j = 0; j < ListDeducciones.Count; j++)
                {
                    if (_idEmpleado == ListDeducciones[j].Item1)
                    {
                        descripcion = ListDeducciones[j].Item2;
                        monto = ListDeducciones[j].Item4;
                        deducciones = armarIncidencias(deducciones, "Deduccion", descripcion, monto, false);
                    }
                }
                deducciones = armarIncidencias(deducciones, "", "", "", true); //quitar coma
                Incidencia = percepciones + " " + deducciones;

                //Pasar datos de montos finales a tupla auxiliar
                for (int j = 0; j < ListEmpleados.Count; j++)
                {
                    if (_idEmpleado == ListEmpleados[j].Item1)
                    {
                        puesto = ListEmpleados[j].Item2;
                        sueldoD = ListEmpleados[j].Item3;
                        diasLaborados = ListEmpleados[j].Item4;
                        totalP = ListEmpleados[j].Item5;
                        totalD = ListEmpleados[j].Item6;
                        totalN = ListEmpleados[j].Item7;
                    }
                }

                string dpto = "";
                for(int j = 0; j < Variables.DEPARTAMENTOS.Count; j++)
                {
                    if(idDepto == Variables.DEPARTAMENTOS[j].iddepartamento.ToString())
                    {
                        dpto = Variables.DEPARTAMENTOS[j].departamento;
                    }
                }

                //Agregar tupla auxiliar a lista de montos de nomina
                var tuple = Tuple.Create(_idEmpleado, diasLaborados, Incidencia, 
                    totalP, totalD, totalB, totalN);
                listMontosNominas.Add(tuple);

                //Agregar tupla auxiliar a lista de datos extras de empleado
                var tuple2 = Tuple.Create(_idEmpleado, dpto, puesto, sueldoD, primaVacacional);
                listDatosEmpleado.Add(tuple2);

            }

            bool sucess = false;
            LCargando.Visible = true;
            cassandra = new EnlaceCassandra();
            sucess = cassandra.generarNominas(empleadosNomina, listMontosNominas, listDatosEmpleado, frecuecncia,
                hoy, fechaInicio, fechaFin, idEmpresa, fechaNuevaNomina);
            
            if (sucess)
            {
                crearPdf(empleadosNomina, listDatosEmpleado);
                //Actualizar empresas
                empresas = Variables.EMPRESAS;
                empleados = Variables.EMPLEADOS;
                CBEmpresas.Items.Clear();
                for (int i = 0; i < empresas.Count; i++)
                {
                    CBEmpresas.Items.Add(empresas[i].razon_social.ToString());
                }
                CBEmpresas.SelectedItem = empresaSeleccionada;

                LCargando.Visible = false;
                MessageBox.Show("Nominas generadas con exito", "AVISO");
            }
            else
            {
                LCargando.Visible = false;
                MessageBox.Show("Error al generar las nominas", "ERROR");
            }
        }

        private void CBEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            iniciarValores();
        }

        #region MisFunciones

        private void iniciarValores()
        {
            LCargando.Visible = true;
            txtEmpleado.Text = "";
            txtPuesto.Text = "";
            txtSueldoD.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFinal.Text = "";
            txtDiasL.Text = "";
            cambioFrecuencia = false;

            btnGenerarN.Enabled = true;
            btnGuardar.Enabled = true;
            CBFrecuencia.Enabled = true;
            CBDiasLaborados.Enabled = true;

            empresaActual = CBEmpresas.SelectedIndex;
            empresaSeleccionada = CBEmpresas.SelectedItem.ToString();
            idEmpresa = empresas[empresaActual].idEmpresa.ToString();

            //Fecha de calculo de nomina
            txtFechaInicio.Text = empresas[empresaActual].ultima_nomina.ToString();

            //COMBO FRECUENCIA
            CBFrecuencia.Items.Clear();
            string frecuencias;
            for (int i = 0; i < empresas[empresaActual].frecuencia_pago.Count; i++)
            {
                frecuencias = empresas[empresaActual].frecuencia_pago[i].ToString();
                if (frecuencias != "")
                {
                    CBFrecuencia.Items.Add(frecuencias);
                }
            }
            CBFrecuencia.SelectedIndex = 0;

            //DG Incidencias
            cargarDataGridIncidencias();
            //DG Empleados
            cargarDataGridEmpleados();

            if (dataGridEmpleados.Rows.Count == 0)
            {
                MessageBox.Show("No hay empleados registrados en la empresa", "AVISO");
                btnGenerarN.Enabled = false;
                btnGuardar.Enabled = false;
                CBFrecuencia.Enabled = false;
                CBDiasLaborados.Enabled = false;
            }
            else
            {
                crearTuplasEmpleados();
            }

            LCargando.Visible = false;
        }

        private void cargarDataGridIncidencias()
        {
            string incidencia;
            //Limpiar datagrids
            dataGridPercepcion.Rows.Clear();
            dataGridDeduccion.Rows.Clear();
            dataGridPercepcionN.Rows.Clear();
            dataGridDeduccionN.Rows.Clear();
            string percepcion = "";
            string deduccion = "";
            string monto = "";
            for (int i = 0; i < empresas[empresaActual].incidencias.Count; i++)
            {
                //Definir si es percepcion o deduccion
                incidencia = empresas[empresaActual].incidencias[i].Item1.ToString();
                if (incidencia == "Percepcion")
                {
                    //Colocar descripcion y precio en el DG
                    percepcion = empresas[empresaActual].incidencias[i].Item2.ToString();
                    monto = empresas[empresaActual].incidencias[i].Item3.ToString();
                    dataGridPercepcion.Rows.Add(percepcion, monto);
                }
                if (incidencia == "Deduccion")
                {
                    deduccion = empresas[empresaActual].incidencias[i].Item2.ToString();
                    monto = empresas[empresaActual].incidencias[i].Item3.ToString();
                    dataGridDeduccion.Rows.Add(deduccion, monto);
                }
            }
        }

        private void cargarDataGridEmpleados()
        {
            dataGridEmpleados.Rows.Clear();
            for(int i  = 0; i < empleados.Count; i++)
            {
                string auxIdEmpresa = empleados[i].idEmpresa.ToString();
                //comparar id de empresa con id de empresa en empleado
                if(idEmpresa == auxIdEmpresa)
                {
                    string idEmpleado = empleados[i].no_empleado.ToString();
                    string nombre = empleados[i].Nombre;
                    string apellidos = empleados[i].Apellidos;
                    string banco = empleados[i].banco;
                    string noCuenta = empleados[i].numero_cuenta.ToString();
                    //Agregar renglon datagrid
                    dataGridEmpleados.Rows.Add(idEmpleado, nombre, apellidos, banco, noCuenta);

                }
            }

        }

        private void calcularFechas()
        {
            string fechaInicio = txtFechaInicio.Text;
            DateTime fechaFinal = Convert.ToDateTime(fechaInicio);
            fechaFinal = fechaFinal.AddDays(diasFrecuencia);
            txtFechaFinal.Text = fechaFinal.ToString("yyyy-MM-dd");
            //Actualizar variable de nueva fecha de nomina para la empresa
            fechaNuevaN = fechaFinal.AddDays(1);
        }

        private void crearTuplasEmpleados()
        {
            //TUPLA DE INCIDENCIAS BASICAS PARA CADA EMPLEADO
            ListPercepciones = new List<Tuple<string, string, string, string>>();
            ListDeducciones = new List<Tuple<string, string, string, string>>();
            ListEmpleados = new List<Tuple<string, string, decimal, int, decimal, decimal, decimal>>();

            int diasLaborados = (CBDiasLaborados.SelectedIndex + 1);

            //Recorrer datagrid de empleados
            foreach (DataGridViewRow row in dataGridEmpleados.Rows)
            {
                string auxIdEmpleado = row.Cells[0].Value.ToString();
                //Recorrer listas de empleados
                for(int i = 0; i < empleados.Count; i++)
                {
                    string _idEmpleado = empleados[i].no_empleado.ToString();
                    //Empleado encontrado
                    if(_idEmpleado == auxIdEmpleado)
                    {
                        string puesto = "";
                        decimal sueldo_base = 0;
                        decimal nivel_salarial = 0;

                        string auxIdDepartamento = empleados[i].idDepartamento.ToString();
                        string auxIdPuesto = empleados[i].idPuesto.ToString();

                        //Recorrer departamentos para encontrar el del empleado
                        for(int j = 0; j < departamentos.Count; j++)
                        {
                            string idDepartamento = departamentos[j].iddepartamento.ToString();
                            //Departamento del empleado
                            if (auxIdDepartamento == idDepartamento)
                            {
                                sueldo_base = departamentos[j].sueldo_base;
                                break;
                            }
                        }
                        
                        //Recorrer puestos para encontrar el del empleado
                        for (int j = 0; j < puestos.Count; j++)
                        {
                            string idPuesto = puestos[j].idpuesto.ToString();
                            //puesto del empleado
                            if (auxIdPuesto == idPuesto)
                            {
                                puesto = puestos[j].puesto;
                                nivel_salarial = puestos[j].nivel_salarial;
                                break;
                            }
                        }
                        ////////////////////////////INCIDENCIAS/////////////////
                        string descripcion;
                        string tipoIncidencia;
                        string monto;

                        totalPercepciones = 0;
                        totalDeducciones = 0;

                        decimal sueldoD = sueldo_base * nivel_salarial;
                        sueldoD = Math.Round(sueldoD, 2);
                        sueldoBase = sueldoD * diasLaborados;

                        //Percepciones generales
                        foreach (DataGridViewRow row2 in dataGridPercepcion.Rows)
                        {
                            descripcion = row2.Cells[0].Value.ToString();
                            tipoIncidencia = "Monto fijo";
                            monto = row2.Cells[1].Value.ToString();

                            //Multiplicar monto porcentual * sueldo
                            decimal auxMonto = decimal.Parse(monto);

                            if (auxMonto < 1) //PORCENTUAL
                            {
                                //Cambiar 'Monto fijo' por su valor porcentual
                                tipoIncidencia = auxMonto.ToString();
                                //Multiplicar sueldo base * monto porcentual
                                auxMonto = auxMonto * sueldoBase;
                                auxMonto = Math.Round(auxMonto, 2);
                                monto = auxMonto.ToString();
                            }

                            //Agregar percepcion de sueldo
                            if (descripcion == "Sueldo")
                            {
                                monto = sueldoBase.ToString();
                                var tuple = Tuple.Create(_idEmpleado, descripcion, tipoIncidencia, monto);
                                ListPercepciones.Add(tuple);
                                totalPercepciones += decimal.Parse(monto);
                            }

                            //Agregar percepcion de bono gerente
                            if (puesto == "Gerente")
                            {
                                if (descripcion == "Bono gerente")
                                {
                                    var tuple = Tuple.Create(_idEmpleado, descripcion, tipoIncidencia, monto);
                                    ListPercepciones.Add(tuple);
                                    totalPercepciones += decimal.Parse(monto);
                                }
                            }
                        }

                        //Deducciones generales
                        foreach (DataGridViewRow row2 in dataGridDeduccion.Rows)
                        {
                            descripcion = row2.Cells[0].Value.ToString();
                            tipoIncidencia = "Monto fijo";
                            monto = row2.Cells[1].Value.ToString();

                            //Multiplicar monto porcentual * sueldo
                            decimal auxMonto = decimal.Parse(monto);
                            if (auxMonto < 1) //PORCENTUAL
                            {
                                tipoIncidencia = auxMonto.ToString();
                                auxMonto = auxMonto * sueldoBase;
                                auxMonto = Math.Round(auxMonto, 2);
                                monto = auxMonto.ToString();
                            }

                            //Deduccion basica ISR Y IMSS
                            if (descripcion == "ISR" || descripcion == "IMSS")
                            {
                                var tuple = Tuple.Create(_idEmpleado, descripcion, tipoIncidencia, monto);
                                ListDeducciones.Add(tuple);
                                totalDeducciones += decimal.Parse(monto);
                            }
                        }
                        //Crear tupla con montos finales
                        totalNeto = totalPercepciones - totalDeducciones;
                        if (totalNeto < 0)
                            totalNeto = 0;
                        totalNeto = Math.Round(totalNeto, 2);

                        //Agregar a tupla de empleado
                        var tupla = Tuple.Create(_idEmpleado, puesto, sueldoD, diasLaborados,
                            totalPercepciones, totalDeducciones, totalNeto);
                        ListEmpleados.Add(tupla);
                    }
                }
            }
        }

        public void calcularPercepciones()
        {
            totalPercepciones = 0;
            foreach (DataGridViewRow row in dataGridPercepcionN.Rows)
            {
                decimal monto = decimal.Parse(row.Cells[2].Value.ToString());
                totalPercepciones += monto;
            }
            totalPercepciones = Math.Round(totalPercepciones, 2);
            //txtTotalPerpcepciones.Text = totalPercepciones.ToString();
        }

        public void calcularDeducciones()
        {
            totalDeducciones = 0;
            foreach (DataGridViewRow row in dataGridDeduccionN.Rows)
            {
                decimal monto = decimal.Parse(row.Cells[2].Value.ToString());
                totalDeducciones += monto;
            }
            totalDeducciones = Math.Round(totalDeducciones, 2);
            //txtTotalDeducciones.Text = totalDeducciones.ToString();
        }

        public void calcularNeto()
        {
            calcularPercepciones();
            calcularDeducciones();

            totalNeto = totalPercepciones - totalDeducciones;
            if (totalNeto < 0)
                totalNeto = 0;
            totalNeto = Math.Round(totalNeto, 2);
            //txtNeto.Text = totalNeto.ToString();
        }

        public void actualizarMontos()
        {
            calcularNeto();
            txtTotalPerpcepciones.Text = totalPercepciones.ToString();
            txtTotalDeducciones.Text = totalDeducciones.ToString();
            txtNeto.Text = totalNeto.ToString();
        }

        public void reiniciarTuplas()
        {
            dataGridPercepcionN.Rows.Clear();
            dataGridDeduccionN.Rows.Clear();
            ListPercepciones.Clear();
            ListDeducciones.Clear();
            ListEmpleados.Clear();
            txtEmpleado.Text = "";
            txtPuesto.Text = "";
            txtSueldoD.Text = "";
            txtDiasL.Text = "";
            txtTotalPerpcepciones.Text = "";
            txtTotalDeducciones.Text = "";
            txtNeto.Text = "";
            sueldoBase = 0;
            totalPercepciones = 0;
            totalDeducciones = 0;
            totalNeto = 0;
            crearTuplasEmpleados();
        }

        private string armarIncidencias(string _cadena, string _incidencia,
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

        private void crearPdf(List<Empleado> _empleados,
            List<Tuple<string, string, string, decimal, bool>> _listDatosEmpleados)
        {
            string frecuencia = CBFrecuencia.SelectedItem.ToString();
            string empresa = CBEmpresas.SelectedItem.ToString();
            string fechaInicio = txtFechaInicio.Text;
            string fechaFin = txtFechaFinal.Text;
            string nombreArchivo = empresa + " - Calculo de Nomina - " +
                fechaInicio + " al " + fechaFin + " - " + frecuencia;
            string nombrePDF = nombreArchivo + ".pdf";

            PdfWriter pdfWriter = new PdfWriter(nombrePDF);
            PdfDocument pdf = new PdfDocument(pdfWriter);
            Document documento = new Document(pdf, PageSize.LETTER);

            documento.SetMargins(60, 20, 55, 20);

            PdfFont fontColumnas = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont fontContenido = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            //Titulo
            var titulo = new Paragraph(CBEmpresas.SelectedItem.ToString());
            titulo.SetTextAlignment(TextAlignment.CENTER);
            titulo.SetFontSize(18).SetFont(fontColumnas);
            documento.Add(titulo);

            //Titulo
            var reporte = new Paragraph("REPORTE CSV");
            reporte.SetTextAlignment(TextAlignment.CENTER);
            reporte.SetFontSize(14);
            documento.Add(reporte);

            string aux ="";

            //Tipo de nomina
            aux = "FRECUENCIA: " + CBFrecuencia.SelectedItem.ToString();
            var parrafo = new Paragraph(aux);
            parrafo.SetTextAlignment(TextAlignment.LEFT).SetFontSize(12);
            documento.Add(parrafo);

            //Periodo
            DateTime fecha = DateTime.Now;
            aux = "NOMINA GENERADA: " + fecha.ToString();
            parrafo = new Paragraph(aux);
            documento.Add(parrafo);

            aux = "PERIODO: DEL " + txtFechaInicio.Text + " AL " + txtFechaFinal.Text;
            parrafo = new Paragraph(aux);
            documento.Add(parrafo);
            

            /////CSV
            string nombreCSV = nombreArchivo + ".txt";
            string ruta = AppDomain.CurrentDomain.BaseDirectory + "/" + nombreCSV;
            StreamWriter writer = new StreamWriter(ruta);

            //Parametros de tabla
            string[] columns = { "No. Empleado", "Empleado", "Fecha", "Monto", "Banco", "No. Cuenta" };
            float[] sizes = { 4,4, 2, 1, 2, 2};
            Table tabla = new Table(UnitValue.CreatePercentArray(sizes));
            tabla.SetWidth(UnitValue.CreatePercentValue(100));

            //Agregar columnas 
            foreach (string columna in columns)
            {
                tabla.AddHeaderCell(new Cell().Add(new Paragraph(columna).SetFont(fontColumnas)));
            }
            for (int i = 0; i < _empleados.Count; i++)
            {
                string idEmpleado = _empleados[i].no_empleado.ToString();
                string nombre = _empleados[i].Nombre + " " + _empleados[i].Apellidos;

                string banco = _empleados[i].banco;
                string noCuenta = _empleados[i].numero_cuenta.ToString();
                string monto = "$";

                for (int j = 0; j < ListEmpleados.Count; j++)
                {
                    if (idEmpleado == ListEmpleados[j].Item1)
                    {
                        monto = monto + ListEmpleados[j].Item7.ToString();
                    }
                }
                //No empleado
                var renglon = new Paragraph(idEmpleado).SetFont(fontContenido).SetFontSize(10);
                tabla.AddCell(new Cell().Add(renglon));
                //Nombre
                renglon = new Paragraph(nombre).SetFont(fontContenido).SetFontSize(10);
                tabla.AddCell(new Cell().Add(renglon));
                //Fecha
                renglon = new Paragraph(fechaInicio + " al " + fechaFin).SetFont(fontContenido).SetFontSize(10);
                tabla.AddCell(new Cell().Add(renglon));
                //Monto
                renglon = new Paragraph(monto).SetFont(fontContenido).SetFontSize(10);
                tabla.AddCell(new Cell().Add(renglon));
                //Banco
                renglon = new Paragraph(banco).SetFont(fontContenido).SetFontSize(10);
                tabla.AddCell(new Cell().Add(renglon));
                //NoCuenta
                renglon = new Paragraph(noCuenta).SetFont(fontContenido).SetFontSize(10);
                tabla.AddCell(new Cell().Add(renglon));

                //CSV
                string contenido = string.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                    idEmpleado, nombre, fechaInicio + " al " + fechaFin, monto, banco, noCuenta);
                writer.WriteLine(contenido);
            }

            documento.Add(tabla);
            documento.Close();
            writer.Close();
        }

        #endregion Mis funciones

    }
}
