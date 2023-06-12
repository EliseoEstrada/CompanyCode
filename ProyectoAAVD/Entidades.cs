using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoAAVD
{
    class Entidades
    {
    }

    class Empresa
    {
        public Guid idEmpresa { get; set; }
        public string razon_social { get; set; }
        public string domicilio_fiscal { get; set; }
        public string registro_patronal { get; set; }
        public string registro_federal { get; set; }
        public List<string> frecuencia_pago { get; set; }
        public Cassandra.LocalDate fecha_inicio { get; set; }
        public double telefono { get; set; }
        public string correo { get; set; }
        public string direccion { get; set; }
        public List<Tuple<string, string, decimal>> incidencias { get; set; }
        public Cassandra.LocalDate ultima_nomina { get; set; }
    }

    class Departamento
    {
        public Guid idempresa { get; set; }
        public Guid iddepartamento { get; set; }
        public string departamento { get; set; }
        public decimal sueldo_base { get; set; }
        public string gerente { get; set; }
    }
    class Puesto
    {
        public Guid idempresa { get; set; }
        public Guid idpuesto { get; set; }
        public string puesto { get; set; }
        public decimal nivel_salarial { get; set; }
    }

    class Empleado
    {
        public Guid idEmpresa { get; set; }
        public Guid no_empleado { get; set; }
        public Guid idDepartamento { get; set; }
        public Guid idPuesto { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public Cassandra.LocalDate fecha_nacimiento { get; set; }
        public string curp { get; set; }
        public string nss { get; set; }
        public string rfc { get; set; }
        public string domicilio { get; set; }
        public string banco { get; set; }
        public Int64 numero_cuenta { get; set; }
        public string email { get; set; }
        public List<Int64> telefonos { get; set; }
        public Cassandra.LocalDate fecha_inicio { get; set; }
        public Cassandra.LocalDate ultima_nomina { get; set; }
        public Cassandra.LocalDate prima_vacacional { get; set; }
    }

    class Nomina
    {
        public Guid idEmpresa { get; set; }
        public Guid idNomina { get; set; }
        public Guid idEmpleado { get; set; }
        public Guid idDepartamento { get; set; }
        public Guid idPuesto { get; set; }
        public string Empleado { get; set; }
        public string Departamento { get; set; }
        public string Puesto { get; set; }
        public Cassandra.LocalDate Fecha_ingreso { get; set; }
        public Cassandra.LocalDate Fecha_Nacimiento { get; set; }
        public string Frecuencia { get; set; }
        public Cassandra.LocalDate Fecha_nomina { get; set; }
        public Cassandra.LocalDate Fecha_inicio { get; set; }
        public Cassandra.LocalDate Fecha_final { get; set; }
        public int Dias_trabajados { get; set; }
        public decimal Salario_diario { get; set; }
        public List<Tuple<string, string, decimal>> Incidencias { get; set; }
        public decimal Total_percepciones { get; set; }
        public decimal Total_deducciones { get; set; }
        public decimal Sueldo_neto { get; set; }
        public decimal Sueldo_bruto { get; set; }
        public string Banco { get; set; }
        public Int64 Numero_cuenta { get; set; }
    }
}
