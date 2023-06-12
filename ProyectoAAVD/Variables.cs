using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoAAVD
{
    class Variables
    {
        static public List<Empresa> EMPRESAS;
        static public List<Departamento> DEPARTAMENTOS;
        static public List<Puesto> PUESTOS;
        static public List<Empleado> EMPLEADOS;

        static public DateTime HOY = DateTime.Now;

        //Reporte headcounter
        static public List<Nomina> NOMINAS;
        static public List<Tuple<string, string, string>> LISTEMPLEADOS;


        //LOGIN 
        static public string USER =  "admi";
        static public string PASSWORD = "admi";
        static public int TYPE_USER = 0;
        static public string COMPANY = "admi";
        static public string EMPLOYEE = "admi";
        static public List<Empleado> EMPLOYEE_INFO; //Variable para recibos
        

        //EDITAR PERCEPCIONES/DEDUCCIONES
        static public bool EDIT;
        static public string TIPO_INCIDENCIA; //Percepcion-Deduccion
        static public string DESCRIPCION;
        static public string MONTO;
        static public bool PORCENTUAL;

    }
}
