using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Borders;
using iText.Kernel.Colors;

namespace ProyectoAAVD
{
    public partial class RecibosNomina : Form
    {
        string empresa;
        string nombreEmpleado;
        string idEmpresa;
        string idEmpleado;
        List<Nomina> nominas;
        List<Empleado> empleado;
        int nominaActual = 0;
        EnlaceCassandra cassandra;

        public RecibosNomina()
        {
            InitializeComponent();
        }

        private void RecibosNomina_Load(object sender, EventArgs e)
        {
            LCargando.Visible = true;
            empresa = Variables.COMPANY;
            nombreEmpleado = Variables.EMPLOYEE;
            lEmpresa.Text = empresa;
            lEmpleado.Text = nombreEmpleado;
            empleado = Variables.EMPLOYEE_INFO;
            
            cassandra = new EnlaceCassandra();
            nominas = cassandra.DGV_RecibosNomina(dataGridReporte);
            LCargando.Visible = false;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < nominas.Count; i++)
            {
                if(nominas[i].idNomina.ToString() == dataGridReporte.CurrentRow.Cells[1].Value.ToString())
                {
                    nominaActual = i;
                    break;
                }
            }

            crearPdf();
        }

        #region MisFunciones
        private void crearPdf()
        {
            //string empresa = nominas[nominaActual].Empresa;
            string frecuencia = nominas[nominaActual].Frecuencia;
            string fechaInicio = nominas[nominaActual].Fecha_inicio.ToString();
            string fechaFin = nominas[nominaActual].Fecha_final.ToString();
            string periodo = fechaInicio + " al " + fechaFin;
            string nombrePDF = "Nomina - "+nombreEmpleado + " - " +
                periodo + " - " + frecuencia + ".pdf";

            PdfWriter pdfWriter = new PdfWriter(nombrePDF);
            PdfDocument pdf = new PdfDocument(pdfWriter);
            Document documento = new Document(pdf, PageSize.LETTER);

            documento.SetMargins(60, 20, 55, 20);

            //Parametros de parrafos
            PdfFont fontColumnas = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont fontContenido = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            //Titulo
            var titulo = new Paragraph(empresa);
            titulo.SetTextAlignment(TextAlignment.CENTER).SetFont(fontColumnas);
            titulo.SetFontSize(18);
            documento.Add(titulo);

            string aux = "";

            #region Tabla
            //Tabla 1
            string[] columns = { "ID Nomina", "Frecuencia.", "Periodo"};
            float[] sizes = { 2, 1, 1 };
            Table tabla = new Table(UnitValue.CreatePercentArray(sizes));
            tabla.SetWidth(UnitValue.CreatePercentValue(100));

            //Agregar columnas 
            foreach (string columna in columns)
            {
                tabla.AddHeaderCell(new Cell().Add(new Paragraph(columna).SetFont(fontColumnas)));
            }

            //Id nomina
            string idNomina = nominas[nominaActual].idNomina.ToString();
            var renglon = new Paragraph(idNomina).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //frecuencia
            renglon = new Paragraph(frecuencia).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //periodo
            renglon = new Paragraph(periodo).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            documento.Add(tabla);
            #endregion Tabla

            #region Tabla1
            //Tabla 1
            string[] columns1 = { "Empleado", "IMSS.", "CURP", "RFC" };
            float[] sizes1 = { 4, 2, 2, 2 };
            tabla = new Table(UnitValue.CreatePercentArray(sizes1));
            tabla.SetWidth(UnitValue.CreatePercentValue(100));

            //Agregar columnas 
            foreach (string columna in columns1)
            {
                tabla.AddHeaderCell(new Cell().Add(new Paragraph(columna).SetFont(fontColumnas)));
            }

            //Nombre
            renglon = new Paragraph(nombreEmpleado).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //IMSS
            string imss = empleado[0].nss;
            renglon = new Paragraph(imss).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //CURP
            string curp = empleado[0].curp;
            renglon = new Paragraph(curp).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //RFC
            string rfc = empleado[0].rfc;
            renglon = new Paragraph(rfc).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            documento.Add(tabla);
            #endregion Tabla1

            #region Tabla2
            //Tabla 1
            string[] columns2 = { "ID Empleado", "Departamento", "Puesto"};
            float[] sizes2 = { 4, 2, 2 };
            tabla = new Table(UnitValue.CreatePercentArray(sizes2));
            tabla.SetWidth(UnitValue.CreatePercentValue(100));

            //Agregar columnas 
            foreach (string columna in columns2)
            {
                tabla.AddHeaderCell(new Cell().Add(new Paragraph(columna).SetFont(fontColumnas)));
            }

            //no empleado
            string idEmpleado = empleado[0].no_empleado.ToString();
            renglon = new Paragraph(idEmpleado).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //departamento
            string departamento = nominas[nominaActual].Departamento;
            renglon = new Paragraph(departamento).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //puesto
            string puesto = nominas[nominaActual].Puesto;
            renglon = new Paragraph(puesto).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            documento.Add(tabla);
            #endregion Tabla2

            #region Tabla3
            //Tabla 1
            string[] columns3 = { "Dias laborados", "Salario diario", "Fecha de alta",
            "Deposito a cuenta No."};
            float[] sizes3 = { 2, 2, 2, 2};
            tabla = new Table(UnitValue.CreatePercentArray(sizes3));
            tabla.SetWidth(UnitValue.CreatePercentValue(100));

            //Agregar columnas 
            foreach (string columna in columns3)
            {
                tabla.AddHeaderCell(new Cell().Add(new Paragraph(columna).SetFont(fontColumnas)));
            }

            //Dias laborados
            string diasL = nominas[nominaActual].Dias_trabajados.ToString();
            renglon = new Paragraph(diasL).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //salario diario
            string salarioD = "$" + nominas[nominaActual].Salario_diario.ToString();
            renglon = new Paragraph(salarioD).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //Fecha de alta
            string fechaAlta = empleado[0].fecha_inicio.ToString();
            renglon = new Paragraph(fechaAlta).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            //Cuenta
            string cuenta = empleado[0].numero_cuenta.ToString();
            renglon = new Paragraph(cuenta).SetFont(fontContenido).SetFontSize(10);
            tabla.AddCell(new Cell().Add(renglon));

            documento.Add(tabla);
            #endregion Tabla3

            var espacio = new Paragraph("");
            documento.Add(espacio);

            #region TablaIncidencias
            //Tabla 1
            string[] columns4 = { "Percepciones", "Deducciones"};
            float[] sizes4 = { 1,1 };
            tabla = new Table(UnitValue.CreatePercentArray(sizes4));
            tabla.SetWidth(UnitValue.CreatePercentValue(100));

            //Agregar columnas 
            foreach (string columna in columns4)
            {
                var colm = new Paragraph(columna);
                colm.SetTextAlignment(TextAlignment.CENTER);
                tabla.AddHeaderCell(new Cell().Add(colm).SetFont(fontColumnas));
            }

            //Tabla de incidencias
            string[] columnIncidencia = { "Concepto", "Monto" };
            float[] sizesIncidencia = { 2, 1 };

            //Tabla percepciones
            var tablaPercepcion = new Table(UnitValue.CreatePercentArray(sizesIncidencia));
            tablaPercepcion.SetWidth(UnitValue.CreatePercentValue(100));
            tablaPercepcion.SetBorder(new SolidBorder(ColorConstants.WHITE, 1));

            foreach (string columna in columnIncidencia)
            {
                var colm = new Paragraph(columna);
                colm.SetTextAlignment(TextAlignment.CENTER);
                tablaPercepcion.AddHeaderCell(new Cell().Add(colm));
            }

            //Tabla deducciones
            var tablaDeduccion = new Table(UnitValue.CreatePercentArray(sizesIncidencia));
            tablaDeduccion.SetWidth(UnitValue.CreatePercentValue(100));
            tablaDeduccion.SetBorder(new SolidBorder(ColorConstants.WHITE, 1));

            foreach (string columna in columnIncidencia)
            {
                tablaDeduccion.AddHeaderCell(new Cell().Add(new Paragraph(columna)));
            }


            //Agregar percepciones y deducciones a sus tablas
            for (int i = 0; i < nominas[nominaActual].Incidencias.Count; i++)
            {
                string tipoIncidencia = nominas[nominaActual].Incidencias[i].Item1;
                string concepto = nominas[nominaActual].Incidencias[i].Item2;
                string monto = "$"+ nominas[nominaActual].Incidencias[i].Item3.ToString();
                if (tipoIncidencia == "Percepcion")
                {
                    renglon = new Paragraph(concepto).SetFont(fontContenido).SetFontSize(10);
                    tablaPercepcion.AddCell(new Cell().Add(renglon));
                    renglon = new Paragraph(monto).SetFont(fontContenido).SetFontSize(10);
                    tablaPercepcion.AddCell(new Cell().Add(renglon));
                }
                else if(tipoIncidencia == "Deduccion")
                {
                    renglon = new Paragraph(concepto).SetFont(fontContenido).SetFontSize(10);
                    tablaDeduccion.AddCell(new Cell().Add(renglon));
                    renglon = new Paragraph(monto).SetFont(fontContenido).SetFontSize(10);
                    tablaDeduccion.AddCell(new Cell().Add(renglon));
                }
            }
            tabla.AddCell(new Cell().Add(tablaPercepcion));
            tabla.AddCell(new Cell().Add(tablaDeduccion));

            //Parrafos de total

            float[] sizesTotal = { 2, 1 };
            var tablaTotalP = new Table(UnitValue.CreatePercentArray(sizesTotal));
            tablaTotalP.SetWidth(UnitValue.CreatePercentValue(100));
            tablaTotalP.SetBorder(new SolidBorder(ColorConstants.WHITE, 1));
            string totalP = nominas[nominaActual].Total_percepciones.ToString();
            renglon = new Paragraph("Total percepciones").SetFont(fontContenido).SetFontSize(10).SetFont(fontColumnas);
            tablaTotalP.AddCell(new Cell().Add(renglon).SetTextAlignment(TextAlignment.RIGHT));
            renglon = new Paragraph("$" +totalP).SetFont(fontContenido).SetFontSize(10);
            tablaTotalP.AddCell(new Cell().Add(renglon));
            tabla.AddCell(new Cell().Add(tablaTotalP));

            var tablaTotalD = new Table(UnitValue.CreatePercentArray(sizesTotal));
            tablaTotalD.SetWidth(UnitValue.CreatePercentValue(100));
            tablaTotalD.SetBorder(new SolidBorder(ColorConstants.WHITE, 1));
            string totalD = nominas[nominaActual].Total_deducciones.ToString();
            renglon = new Paragraph("Total deducciones").SetFont(fontContenido).SetFontSize(10).SetFont(fontColumnas);
            tablaTotalD.AddCell(new Cell().Add(renglon).SetTextAlignment(TextAlignment.RIGHT));
            renglon = new Paragraph("$" + totalD).SetFont(fontContenido).SetFontSize(10);
            tablaTotalD.AddCell(new Cell().Add(renglon));
            tabla.AddCell(new Cell().Add(tablaTotalD));

            documento.Add(tabla);
            #endregion TablaIncidencias

            //espacio
            aux = "";
            renglon = new Paragraph(aux);
            renglon.SetFontSize(12);
            documento.Add(renglon);

            //NETO
            aux = "NETO A PAGAR $" + nominas[nominaActual].Sueldo_neto.ToString();
            string montoEnLetra = NumeroALetras(nominas[nominaActual].Sueldo_neto);
            aux = aux + " (" + montoEnLetra + " MN) ";
            renglon = new Paragraph(aux);
            renglon.SetTextAlignment(TextAlignment.LEFT).SetFontSize(12).SetFont(fontColumnas);
            documento.Add(renglon);

            documento.Close();

            MessageBox.Show("PDF generado con exito", "AVISO");
        }

        private string NumeroALetras(decimal value)
        {
            string num2Text; value = Math.Truncate(value);
            if (value == 0) num2Text = "CERO";
            else if (value == 1) num2Text = "UNO";
            else if (value == 2) num2Text = "DOS";
            else if (value == 3) num2Text = "TRES";
            else if (value == 4) num2Text = "CUATRO";
            else if (value == 5) num2Text = "CINCO";
            else if (value == 6) num2Text = "SEIS";
            else if (value == 7) num2Text = "SIETE";
            else if (value == 8) num2Text = "OCHO";
            else if (value == 9) num2Text = "NUEVE";
            else if (value == 10) num2Text = "DIEZ";
            else if (value == 11) num2Text = "ONCE";
            else if (value == 12) num2Text = "DOCE";
            else if (value == 13) num2Text = "TRECE";
            else if (value == 14) num2Text = "CATORCE";
            else if (value == 15) num2Text = "QUINCE";
            else if (value < 20) num2Text = "DIECI" + NumeroALetras(value - 10);
            else if (value == 20) num2Text = "VEINTE";
            else if (value < 30) num2Text = "VEINTI" + NumeroALetras(value - 20);
            else if (value == 30) num2Text = "TREINTA";
            else if (value == 40) num2Text = "CUARENTA";
            else if (value == 50) num2Text = "CINCUENTA";
            else if (value == 60) num2Text = "SESENTA";
            else if (value == 70) num2Text = "SETENTA";
            else if (value == 80) num2Text = "OCHENTA";
            else if (value == 90) num2Text = "NOVENTA";
            else if (value < 100) num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
            else if (value == 100) num2Text = "CIEN";
            else if (value < 200) num2Text = "CIENTO " + NumeroALetras(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) num2Text = "QUINIENTOS";
            else if (value == 700) num2Text = "SETECIENTOS";
            else if (value == 900) num2Text = "NOVECIENTOS";
            else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
            else if (value == 1000) num2Text = "MIL";
            else if (value < 2000) num2Text = "MIL " + NumeroALetras(value % 1000);
            else if (value < 1000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "UN MILLON";
            }
            else if (value < 2000000)
            {
                num2Text = "UN MILLON " + NumeroALetras(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000) num2Text = "UN BILLON";
            else if (value < 2000000000000) num2Text = "UN BILLON " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }
            return num2Text;
        }

        #endregion MisFunciones

    }
}
