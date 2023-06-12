using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;
using System.Configuration;
using System.Windows.Forms;

namespace ProyectoAAVD
{
    class EnlaceCassandra
    {
        static private string _dbServer { set; get; }
        static private string _dbKeySpace { set; get; }
        static private Cluster _cluster;
        static private ISession _session;

        private static void conectar()
        {
            _dbServer = ConfigurationManager.AppSettings["Cluster"].ToString();
            _dbKeySpace = ConfigurationManager.AppSettings["KeySpace"].ToString();

            _cluster = Cluster.Builder()
                .AddContactPoint(_dbServer)
                .Build();

            _session = _cluster.Connect(_dbKeySpace);
        }


        private static void conectar2()
        {
            _cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .Build();

            _session = _cluster.Connect("keyspace3");
        }

        private static void desconectar()
        {
            _cluster.Dispose();
        }

        public void InsertaDatos(int id, string dato)
        {
            try
            {
                conectar();
                
                string qry = "insert into ejemplo(campo1, campo2) values(";
                qry = qry + id.ToString();
                qry = qry + ",'";
                qry = qry + dato;
                qry = qry + "');";


                string query = "insert into ejemplo(campo1, campo2) values({0}, '{1}');";
                qry = string.Format(query, id, dato);

                _session.Execute(qry);
            }
            catch(Exception e)
            {
                throw e;   
            }
            finally
            {
                // desconectar o cerrar la conexión
                desconectar();
            }
        }

        //public IEnumerable<Ejemplo> Get_One(int dato)
        //{
        //    string query = "SELECT campo1, campo2 FROM ejemplo WHERE campo1 = ?;";
        //    conectar();
        //    IMapper mapper = new Mapper(_session);
        //    IEnumerable<Ejemplo> users = mapper.Fetch<Ejemplo>(query, dato);

        //    desconectar();
        //    return users.ToList();
        //}

        //public List<Ejemplo> Get_All()
        //{
        //    string query = "SELECT campo1, campo2 FROM ejemplo;";
        //    conectar();
            
        //    IMapper mapper = new Mapper(_session);
        //    IEnumerable<Ejemplo> users = mapper.Fetch<Ejemplo>(query);

        //    desconectar();
        //    return users.ToList();
            
        //}

        // Ejemplo de leer row x row
        public void GetOne()
        {
            conectar();

            string query ="SELECT campo1, campo2 FROM ejemplo;";

            // Execute a query on a connection synchronously 
            var rs = _session.Execute(query);
            
            // Iterate through the RowSet 
            foreach (var row in rs)
            {
                var value = row.GetValue<int>("campo1");
                // Do something with the value 
                var texto = row.GetValue<string>("campo2");
                // Do something with the value 

                MessageBox.Show(texto, value.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                /*
                RowSet rsUsers = session.Execute(qry);

                ////////////////////////////////////////////////
                var users = new List<UserModel>();
                foreach (var userRow in rsUsers)
                {
                    //users.Add(ReflectionTools.GetSingleEntryDynamicFromReader<UserModel>(userRow));
                }

                foreach (UserModel user in users)
                {
                    Console.WriteLine("{0} {1} {2} {3} {4}", user.Id, user.FirstName, user.LastName, user.Country, user.IsActive);
                }
                */

            }
        }

        #region Usuario
        public bool Sesion(string _user, string _password)
        {
            bool find = false;

            string query, query2, query3, query4, query5;

            query = "SELECT user, password, tipo_usuario, idEmpresa " +
                "FROM Usuarios " +
                "WHERE User = '" + _user +
                "' AND Password = '" + _password +
                "' ALLOW FILTERING;";

            try
            {
                conectar();
                // Execute a query on a connection synchronously 
                var rs = _session.Execute(query);

                // Iterate through the RowSet 
                foreach (var row in rs)
                {
                    var user = row.GetValue<string>("user");
                    var password = row.GetValue<string>("password");
                    var tipe_user = row.GetValue<int>("tipo_usuario");
                    var idEmpresa = row.GetValue<Guid>("idempresa");
                    // Do something with the value 

                    if (user == _user && password == _password)
                    {
                        find = true;
                        Variables.USER = user;
                        Variables.PASSWORD = password;
                        Variables.TYPE_USER = tipe_user;

                        IEnumerable<Empresa> empresas;
                        IEnumerable<Departamento> departamentos;
                        IEnumerable<Puesto> puestos;
                        IEnumerable<Empleado> empleados;

                        IMapper mapper = new Mapper(_session);

                        if (tipe_user == 0) //Admi
                        {
                            //Buscar todas las empresas
                            query2 = "SELECT idEmpresa, Razon_social, Domicilio_fiscal, Registro_patronal, " +
                               "Registro_federal, Frecuencia_pago, Fecha_inicio, " +
                               "Telefono, Correo, Direccion, Incidencias, Ultima_Nomina " +
                               "FROM Empresas;";

                            //Buscar todos los departamentos
                            query3 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                                "FROM Departamentos;";

                            //Buscar todos los puestos
                            query4 = "SELECT idEmpresa,idPuesto, Puesto, Nivel_Salarial " +
                                "FROM Puestos;";

                            //Buscar todos los empleados
                            query5 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                                "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                                "Fecha_inicio, ultima_nomina, prima_vacacional " +
                                "FROM Empleados;";

                            empresas = mapper.Fetch<Empresa>(query2);
                            Variables.EMPRESAS = empresas.ToList();

                            departamentos = mapper.Fetch<Departamento>(query3);
                            Variables.DEPARTAMENTOS = departamentos.ToList();

                            puestos = mapper.Fetch<Puesto>(query4);
                            Variables.PUESTOS = puestos.ToList();

                            empleados = mapper.Fetch<Empleado>(query5);
                            Variables.EMPLEADOS = empleados.ToList();
                        }
                        else//Gerente o empleado
                        {
                            if (tipe_user == 1 || tipe_user == 2) //Obtener datos de empresa para gerente
                            {

                                //Obtener departamentos de la empresa
                                query3 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                                    "FROM Departamentos WHERE idempresa = " + idEmpresa + ";";
                                //Obtener puestos de la empresa
                                query4 = "SELECT idEmpresa,idPuesto, Puesto, Nivel_Salarial " +
                                    "FROM Puestos WHERE idEmpresa = " + idEmpresa + ";";
                                //Obtener empleados de la empresa
                                query5 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                                    "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                                    "Fecha_inicio, ultima_nomina, prima_vacacional " +
                                    "FROM Empleados WHERE idEmpresa = " + idEmpresa + ";";

                                departamentos = mapper.Fetch<Departamento>(query3);
                                Variables.DEPARTAMENTOS = departamentos.ToList();

                                puestos = mapper.Fetch<Puesto>(query4);
                                Variables.PUESTOS = puestos.ToList();

                                empleados = mapper.Fetch<Empleado>(query5);
                                Variables.EMPLEADOS = empleados.ToList();
                            }

                            //Obtener datos de la empresa
                            query2 = "SELECT idEmpresa, Razon_social, Domicilio_fiscal, Registro_patronal," +
                                " Registro_federal, Frecuencia_pago, Fecha_inicio, " +
                                "Telefono, Correo, Direccion, Incidencias, Ultima_Nomina " +
                                "FROM Empresas WHERE idEmpresa = " + idEmpresa + ";";

                            empresas = mapper.Fetch<Empresa>(query2);
                            Variables.EMPRESAS = empresas.ToList();
                            Variables.COMPANY = Variables.EMPRESAS[0].razon_social;

                            //Obtener datos del empleado
                            string query6 = "SELECT no_empleado FROM Empleados WHERE email = '" + user +
                                "' ALLOW FILTERING;";

                            var rs2 = _session.Execute(query6);
                            foreach (var row2 in rs2)
                            {
                                var idUsuario = row2.GetValue<Guid>("no_empleado");

                                query5 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                                "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                                "Fecha_inicio, ultima_nomina, prima_vacacional " +
                                "FROM Empleados WHERE idEmpresa = " + idEmpresa +
                                " AND No_empleado = " + idUsuario + ";";

                                empleados = mapper.Fetch<Empleado>(query5);
                                Variables.EMPLOYEE_INFO = empleados.ToList();
                                Variables.EMPLOYEE = Variables.EMPLOYEE_INFO[0].Nombre + " " + Variables.EMPLOYEE_INFO[0].Apellidos;
                            }
                        }
                    }
                }

                desconectar();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al conectar con cassandra", "ERROR");
                MessageBox.Show(e.ToString(), "ERROR");
            }
            finally
            {
                desconectar();
            }

            return find;
        }
        
        #endregion Usuario

        #region Empresas 
        public bool AgregarEmpresa(string _razonSocial, string _domicilioF, string _registroP, 
            string _registroF, string[] _frecuenciaPago, string _fechaInicio,
            string _telefono, string _correo, string _direccion, string _incidencias)
        {
            bool sucess = true;
            string qry = "";
            string query = "";

            query = "INSERT INTO Empresas( " +
                     "idEmpresa, " +
                     "Razon_social, " +
                     "Domicilio_fiscal, " +
                     "Registro_patronal, " +
                     "Registro_federal, " +
                     "Frecuencia_pago, " +
                     "Fecha_inicio, " +
                     "Telefono, " +
                     "Correo, " +
                     "Direccion, " +
                     "Incidencias, " +
                     "Ultima_Nomina) " +
                     "VALUES ( uuid(), '{0}', '{1}', '{2}', '{3}', ['{4}','{5}','{6}','{7}'], " +
                            "'{8}', {9}, '{10}','{11}', [{12}], '{8}');";

            qry = string.Format(query,
                _razonSocial,
                _domicilioF,
                _registroP,
                _registroF,
                _frecuenciaPago[0],
                _frecuenciaPago[1],
                _frecuenciaPago[2],
                _frecuenciaPago[3],
                _fechaInicio,
                _telefono,
                _correo,
                _direccion,
                _incidencias,
                _fechaInicio);

            string query2 = "SELECT idEmpresa, Razon_social, Domicilio_fiscal, Registro_patronal, Registro_federal, " +
                "Frecuencia_pago, Fecha_inicio, Telefono, Correo, Direccion, Incidencias, Ultima_Nomina " +
                "FROM Empresas;";

            try
            {
                conectar();
                //Agregar empresa
                _session.Execute(qry);
                //Actualizar empresas
                IEnumerable<Empresa> empresas;
                IMapper mapper = new Mapper(_session);
                empresas = mapper.Fetch<Empresa>(query2);
                Variables.EMPRESAS = empresas.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        public bool EditarEmpresa(string _idEmpresa, string _razonSocial, string _domicilioF, string _registroP,
            string _registroF, string[] _frecuenciaPago, string _fechaInicio,
            string _telefono, string _correo, string _direccion, string _incidencias)
        {
            bool sucess = true;
            string qry = "UPDATE Empresas " +
                  "SET Razon_social = '" + _razonSocial + "', " +
                  "Domicilio_fiscal = '" + _domicilioF + "', " +
                  "Registro_patronal = '" + _registroP + "', " +
                  "Registro_federal = '" + _registroF + "', " +
                  "Frecuencia_pago = ['" + _frecuenciaPago[0] + "', " +
                  "'" + _frecuenciaPago[1] + "'," +
                  "'" + _frecuenciaPago[2] + "'," +
                  "'" + _frecuenciaPago[3] + "'], " +
                  "Fecha_inicio = '" + _fechaInicio + "', " +
                  "Telefono = " + _telefono + ", " +
                  "Correo = '" + _correo + "', " +
                  "Direccion = '" + _direccion + "', " +
                  "Incidencias = [" + _incidencias + "] " +
                  "WHERE idEmpresa = " + _idEmpresa + "; ";

            string query2 = "SELECT idEmpresa, Razon_social, Domicilio_fiscal, Registro_patronal, Registro_federal, " +
                "Frecuencia_pago, Fecha_inicio, Telefono, Correo, Direccion, Incidencias, Ultima_Nomina " +
                "FROM Empresas;";

            try
            {
                conectar();
                //Actualizar empresa
                _session.Execute(qry);

                //Actualizar lista de empresas
                IEnumerable<Empresa> empresas;
                IMapper mapper = new Mapper(_session);
                empresas = mapper.Fetch<Empresa>(query2);
                Variables.EMPRESAS = empresas.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        public bool EliminarEmpresa(string _idEmpresa)
        {
            bool sucess = true;
            string query = "BEGIN BATCH ";
            query = query + "DELETE FROM Empresas WHERE idEmpresa = " + _idEmpresa + "; ";
            query = query + "DELETE FROM Departamentos WHERE idEmpresa = " + _idEmpresa + "; ";
            query = query + "DELETE FROM Puestos WHERE idEmpresa = " + _idEmpresa + "; ";

            for (int i = 0; i <Variables.EMPLEADOS.Count; i++)
            {
                if(Variables.EMPLEADOS[i].idEmpresa.ToString() == _idEmpresa)
                {
                    string user = Variables.EMPLEADOS[i].email;
                    string idEmpleado = Variables.EMPLEADOS[i].no_empleado.ToString();
                    query = query + "DELETE FROM Usuarios WHERE User = '" + user + "'; ";
                    query = query + "DELETE FROM Nominas WHERE idEmpresa = " + _idEmpresa + " " +
                        "AND idEmpleado = " + idEmpleado + "; ";
                }
            }
            query = query + "DELETE FROM Empleados WHERE idEmpresa = " + _idEmpresa + "; ";
            query = query + "APPLY BATCH;";

            string query2, query3, query4,query5;
            //Buscar todas las empresas
            query2 = "SELECT idEmpresa, Razon_social, Domicilio_fiscal, Registro_patronal, " +
               "Registro_federal, Frecuencia_pago, Fecha_inicio, " +
               "Telefono, Correo, Direccion, Incidencias, Ultima_Nomina " +
               "FROM Empresas;";

            //Buscar todos los departamentos
            query3 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                "FROM Departamentos;";

            //Buscar todos los puestos
            query4 = "SELECT idEmpresa,idPuesto, Puesto, Nivel_Salarial " +
                "FROM Puestos;";

            //Buscar todos los empleados
            query5 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                "Fecha_inicio, ultima_nomina, prima_vacacional " +
                "FROM Empleados;";

            try
            {
                conectar();
                _session.Execute(query);

                //Actualizar datos
                IEnumerable<Empresa> empresas;
                IEnumerable<Departamento> departamentos;
                IEnumerable<Puesto> puestos;
                IEnumerable<Empleado> empleados;

                IMapper mapper = new Mapper(_session);

                empresas = mapper.Fetch<Empresa>(query2);
                Variables.EMPRESAS = empresas.ToList();

                departamentos = mapper.Fetch<Departamento>(query3);
                Variables.DEPARTAMENTOS = departamentos.ToList();

                puestos = mapper.Fetch<Puesto>(query4);
                Variables.PUESTOS = puestos.ToList();

                empleados = mapper.Fetch<Empleado>(query5);
                Variables.EMPLEADOS = empleados.ToList();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        #endregion Empresas

        #region Departamentos
        //--------------------------------DEPARTAMENTOS--------------------------
        public bool AgregarDepartamento(string _idEmpresa, string _Departamento, string _Sueldo_base, bool _admin)
        {
            bool sucess = true;

            string qry = "";
            string query = "";
            query = "INSERT INTO Departamentos (" +
                "idEmpresa, " +
                "idDepartamento, " +
                "Departamento, " +
                "Sueldo_base, " +
                "Gerente) " +
                "VALUES ( {0}, uuid(), '{1}', {2}, 'Sin gerente' );";
            qry = string.Format(query, _idEmpresa, _Departamento, _Sueldo_base);

            //query para actualizar lista
            string query2;
            if (_admin)
            {
                query2 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos;";
            }
            else
            {
                query2 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos WHERE idempresa = " + _idEmpresa + ";";
            }

            try
            {
                conectar();
                //Agregar departamento
                _session.Execute(qry);

                //Actualizar lista de departamentos
                IMapper mapper = new Mapper(_session);
                IEnumerable<Departamento> departamentos;
                departamentos = mapper.Fetch<Departamento>(query2);
                Variables.DEPARTAMENTOS = departamentos.ToList();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        public bool EditarDepartamento(string _idEmpresa, string _idDepartamento, string _Departamento, string _Sueldo_base, bool _admin)
        {
            bool sucess = true;

            string qry = "UPDATE Departamentos SET " +
                "Departamento = '" + _Departamento + "', " +
                "Sueldo_base = " + _Sueldo_base + " " +
                "WHERE idEmpresa = " + _idEmpresa + " " +
                "AND idDepartamento = " + _idDepartamento + ";";

            //query para actualizar lista
            string query2;
            if (_admin)
            {
                query2 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos;";
            }
            else
            {
                query2 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos WHERE idempresa = " + _idEmpresa + ";";
            }

            try
            {
                conectar();
                //Agregar departamento
                _session.Execute(qry);

                //Actualizar lista de departamentos
                IMapper mapper = new Mapper(_session);
                IEnumerable<Departamento> departamentos;
                departamentos = mapper.Fetch<Departamento>(query2);
                Variables.DEPARTAMENTOS = departamentos.ToList();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        public bool EliminarDepartamento(string _idEmpresa, string _idDepartamento)
        {
            bool sucess = true;
            string query = "DELETE FROM Departamentos WHERE idEmpresa = " + _idEmpresa + " " +
                "AND idDepartamento = " + _idDepartamento + "; ";

            string query2 = "";
            if (Variables.TYPE_USER == 0)
            {
                query2 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos;";
            }
            else
            {
                query2 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos WHERE idempresa = " + _idEmpresa + ";";
            }

            try
            {
                conectar();
                _session.Execute(query);

                //Actualizar lista de departamentos
                IMapper mapper = new Mapper(_session);
                IEnumerable<Departamento> departamentos;
                departamentos = mapper.Fetch<Departamento>(query2);
                Variables.DEPARTAMENTOS = departamentos.ToList();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }
        #endregion Departamentos

        #region Puestos
        //--------------------------------------PUESTOS--------------------------
        public bool AgregarPuesto(string _idEmpresa, string _puesto, string _nivelS, bool _admin)
        {
            bool sucess = true;
            string qry = "";
            string query = "", query2 = "";

            query = "INSERT INTO Puestos( " +
                    "idEmpresa, " +
                    "idPuesto, " +
                    "Puesto, " +
                    "Nivel_Salarial) " +
                    "VALUES ( {0}, uuid(), '{1}', {2});";
            qry = string.Format(query, _idEmpresa, _puesto, _nivelS);

            if (_admin)
            {
                query2 = "SELECT idEmpresa,idPuesto, Puesto, Nivel_Salarial " +
                    "FROM Puestos;";
            }
            else
            {
                query2 = "SELECT idEmpresa,idPuesto, Puesto, Nivel_Salarial " +
                    "FROM Puestos WHERE idEmpresa = " + _idEmpresa + ";";
            }

            try
            {
                conectar();
                //Agregar puesto
                _session.Execute(qry);

                //Actualizar lista de departamentos
                IMapper mapper = new Mapper(_session);
                IEnumerable<Puesto> puestos;
                puestos = mapper.Fetch<Puesto>(query2);
                Variables.PUESTOS = puestos.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        public bool EditarPuesto(string _idEmpresa, string _idPuesto, string _puesto, string _nivelS, bool _admin)
        {
            bool sucess = true;
            string query = "UPDATE Puestos SET " +
                    "Puesto = '" + _puesto + "', " +
                    "Nivel_Salarial = " + _nivelS + " " +
                    "WHERE idEmpresa = " + _idEmpresa + " " +
                    "AND idPuesto = " + _idPuesto + ";";
            string query2 = "";
            if (_admin)
            {
                query2 = "SELECT idEmpresa,idPuesto, Puesto, Nivel_Salarial " +
                    "FROM Puestos;";
            }
            else
            {
                query2 = "SELECT idEmpresa,idPuesto, Puesto, Nivel_Salarial " +
                    "FROM Puestos WHERE idEmpresa = " + _idEmpresa + ";";
            }

            try
            {
                conectar();
                //Agregar puesto
                _session.Execute(query);

                //Actualizar lista de departamentos
                IMapper mapper = new Mapper(_session);
                IEnumerable<Puesto> puestos;
                puestos = mapper.Fetch<Puesto>(query2);
                Variables.PUESTOS = puestos.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        public bool EliminarPuesto(string _idEmpresa, string _idPuesto)
        {
            bool sucess = true;
            string query = "DELETE FROM Puestos WHERE idEmpresa = " + _idEmpresa + " " +
                "AND idPuesto = " + _idPuesto + "; ";

            string query2 = "";
            if (Variables.TYPE_USER == 0)  //Admin
            {
                query2 = "SELECT idEmpresa,idPuesto, Puesto, Nivel_Salarial " +
                    "FROM Puestos;";
            }
            else
            {
                query2 = "SELECT idEmpresa,idPuesto, Puesto, Nivel_Salarial " +
                    "FROM Puestos WHERE idEmpresa = " + _idEmpresa + ";";
            }

            try
            {
                conectar();
                _session.Execute(query);

                //Actualizar lista de puestos
                IMapper mapper = new Mapper(_session);
                IEnumerable<Puesto> puestos;
                puestos = mapper.Fetch<Puesto>(query2);
                Variables.PUESTOS = puestos.ToList();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        #endregion Puestos

        #region Empleados

        public bool AgregarEmpleado(string _idEmpresa, string _idPuesto, string _idDepartamento, 
            string _contraseña, string _nombre, string _apellidos, string _fechaN, 
            string _CURP, string _NSS, string _RFC, string _domicilio, string _banco, string _noCuenta, string _email,
            string _telefono, string _celular, string _fechaI, string _puesto, bool _admin)
        {
            bool sucess = true;

            string qry = "";
            string query = "", query2 = "", query3 = "";
            int tipoEmpleado = 2;
            if (_puesto == "Gerente")
            {
                tipoEmpleado = 1;
            }

            query = "BEGIN BATCH " +
                    //Empleado
                    "INSERT INTO Empleados( " +
                    "idEmpresa, " +
                    "No_empleado, " +
                    "idDepartamento, " +
                    "idPuesto, " +
                    "Password, " +
                    "Nombre, " +
                    "Apellidos, " +
                    "Fecha_Nacimiento, " +
                    "CURP, " +
                    "NSS, " +
                    "RFC, " +
                    "Domicilio, " +
                    "Banco,  " +
                    "Numero_cuenta, " +
                    "Email, " +
                    "Telefonos, " +
                    "Fecha_inicio, " +
                    "ultima_nomina, " +
                    "prima_vacacional) " +
                    "VALUES ( " +
                    "{0}, uuid(), {1}, {2}, '{3}', '{4}', '{5}', '{6}', '{7}', " +
                    "'{8}', '{9}', '{10}', '{11}', {12}, '{13}', [{14},{15}], " +
                    "'{16}','{17}', '{18}'); " +
                    //Usuario1
                    "INSERT INTO Usuarios(" +
                    "User, " +
                    "Password, " +
                    "Tipo_usuario, " +
                    "idEmpresa) " +
                    "VALUES('{19}', '{20}', {21}, {22});";

            qry = string.Format(query,
                _idEmpresa,
                _idDepartamento,
                _idPuesto,
                _contraseña,
                _nombre,
                _apellidos,
                _fechaN,
                _CURP,
                _NSS,
                _RFC,
                _domicilio,
                _banco,
                _noCuenta,
                _email,
                _telefono,
                _celular,
                _fechaI,
                _fechaI,
                _fechaI,
                //Usuario
                _email,
                _contraseña,
                tipoEmpleado,
                _idEmpresa);

            //Si es gerente o encargado actualizar departamento
            if (tipoEmpleado == 1)
            {
                qry = qry + " UPDATE Departamentos SET " +
                    "Gerente = '" + _nombre + " " + _apellidos + "' " +
                    "WHERE idEmpresa = " + _idEmpresa + " " +
                    "AND idDepartamento = " + _idDepartamento + "; " +
                    "APPLY BATCH;";
            }
            else
            {
                qry = qry + "APPLY BATCH;";
            }

            //query para actualizar lista de empleados
            if (_admin)
            {
                query2 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                    "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                    "Fecha_inicio, ultima_nomina, prima_vacacional " +
                    "FROM Empleados;";

                query3 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos;";
            }
            else
            {
                query2 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                    "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                    "Fecha_inicio, ultima_nomina, prima_vacacional " +
                    "FROM Empleados WHERE idEmpresa = " + _idEmpresa + ";";

                query3 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos WHERE idempresa = " + _idEmpresa + ";";
            }

            try
            {
                conectar();
                //Agregar empleado
                _session.Execute(qry);

                //Actualizar lista de departamentos
                IMapper mapper = new Mapper(_session);
                IEnumerable<Empleado> empleados;
                empleados = mapper.Fetch<Empleado>(query2);
                Variables.EMPLEADOS = empleados.ToList();

                //Actualizar departamentos para actualizar gerente
                IEnumerable<Departamento> departamentos;
                departamentos = mapper.Fetch<Departamento>(query3);
                Variables.DEPARTAMENTOS = departamentos.ToList();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
                //throw e;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        public bool EditarEmpleado(string _idEmpresa, string _no_empleado, string _idPuesto, string _idDepartamento,
            string _contraseña, string _nombre, string _apellidos, string _fechaN, string _CURP, 
            string _NSS, string _RFC, string _domicilio, string _banco, string _noCuenta, string _email,
            string _telefono, string _celular, string _fechaI, string _puesto, bool _admin, 
            string antiguoCorreo, string antiguoPuesto, string antiguoDeptoID)
        {
            bool sucess = true;

            string qry = "";
            string query = "", query2 = "", query3 = "";
            bool antiguoGerente = false;

            int tipoEmpleado = 3;
            if (_puesto == "Gerente")
            {
                tipoEmpleado = 1;
            }

            if(antiguoPuesto == "Gerente")
            {
                antiguoGerente = true;
            }

            qry = "BEGIN BATCH " +
                    //Empleado
                    "UPDATE Empleados " +
                    "SET idDepartamento = " + _idDepartamento + ", " +
                    "idPuesto = " + _idPuesto + ", " +
                    "Password = '" + _contraseña + "', " +
                    "Nombre = '" + _nombre + "', " +
                    "Apellidos = '" + _apellidos + "', " +
                    "Fecha_Nacimiento = '" + _fechaN + "', " +
                    "CURP = '" + _CURP + "', " +
                    "NSS = '" + _NSS + "', " +
                    "RFC = '" + _RFC + "', " +
                    "Domicilio = '" + _domicilio + "', " +
                    "Banco = '" + _banco + "', " +
                    "Numero_cuenta = " + _noCuenta + ", " +
                    "Email = '" + _email + "', " +
                    "Telefonos = [" + _telefono + ", " + _celular + "] " +
                    "WHERE idEmpresa = " + _idEmpresa + " " +
                    "AND No_empleado = " + _no_empleado + "; ";
                    //Usuario

            if(antiguoCorreo == _email) //Mismo correo
            {
                qry = qry + "UPDATE Usuarios " +
                    "SET Password = '" + _contraseña + "', " +
                    "Tipo_usuario = " + tipoEmpleado + " " +
                    "WHERE User = '" + antiguoCorreo + "'; ";
            }
            else
            {
                //Agregar nueva cuenta
                qry = qry + "INSERT INTO Usuarios(" +
                    "User, " +
                    "Password, " +
                    "Tipo_usuario, " +
                    "idEmpresa) " +
                    "VALUES('" + _email + "', " +
                    "'" + _contraseña + "', " +
                    tipoEmpleado + ", " +
                    _idEmpresa + "); ";

                //Eliminar cuenta pasada
                qry = qry + "DELETE FROM Usuarios " +
                    "WHERE User = '" + antiguoCorreo + "'; ";
            }

            //Si era antiguo gerente borrar de la tabla de departamentos
            if (antiguoGerente)
            {
                qry = qry + " UPDATE Departamentos SET " +
                    "Gerente = 'Sin gerente' " +
                    "WHERE idEmpresa = " + _idEmpresa + " " +
                    "AND idDepartamento = " + antiguoDeptoID + "; ";
            }

            //Si es gerente actualizar departamento
            if (tipoEmpleado == 1)
            {
                qry = qry + " UPDATE Departamentos SET " +
                    "Gerente = '" + _nombre + " " + _apellidos + "' " +
                    "WHERE idEmpresa = " + _idEmpresa + " " +
                    "AND idDepartamento = " + _idDepartamento + "; " +
                    "APPLY BATCH;";
            }
            else
            {
                qry = qry + "APPLY BATCH;";
            }

            //query para actualizar lista de empleados
            if (_admin)
            {
                query2 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                    "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                    "Fecha_inicio, ultima_nomina, prima_vacacional " +
                    "FROM Empleados;";

                query3 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos;";
            }
            else
            {
                query2 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                    "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                    "Fecha_inicio, ultima_nomina, prima_vacacional " +
                    "FROM Empleados WHERE idEmpresa = " + _idEmpresa + ";";

                query3 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos WHERE idempresa = " + _idEmpresa + ";";
            }

            try
            {
                conectar();
                //Agregar empleado
                _session.Execute(qry);

                //Actualizar lista de departamentos
                IMapper mapper = new Mapper(_session);
                IEnumerable<Empleado> empleados;
                empleados = mapper.Fetch<Empleado>(query2);
                Variables.EMPLEADOS = empleados.ToList();

                //Actualizar departamentos para actualizar gerente
                IEnumerable<Departamento> departamentos;
                departamentos = mapper.Fetch<Departamento>(query3);
                Variables.DEPARTAMENTOS = departamentos.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
                //throw e;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        public bool EliminarEmpleado(string _idEmpresa, string _no_empleado, string _correo, 
            string _idDepartamento, string _puesto)
        {
            bool sucess = true;

            string qry = "", query2 = "", query3 = "";

            qry = "BEGIN BATCH " +
                    //Empleado
                    "DELETE FROM Empleados " +
                    "WHERE idEmpresa = " + _idEmpresa + " " +
                    "AND No_empleado = " + _no_empleado +"; ";
            //Usuario
            qry = qry + " DELETE FROM Usuarios " +
                    "WHERE User = '" + _correo + "'; ";
            //Departamento
            if (_puesto == "Gerente")
            {
                qry = qry + " UPDATE Departamentos SET " +
                    "Gerente = 'Sin gerente' " +
                    "WHERE idEmpresa = " + _idEmpresa + " " +
                    "AND idDepartamento = " + _idDepartamento + "; ";
            }

            qry = qry + "APPLY BATCH;";

            //query para actualizar lista de empleados
            if (Variables.TYPE_USER == 0)
            {
                query2 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                    "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                    "Fecha_inicio, ultima_nomina, prima_vacacional " +
                    "FROM Empleados;";

                query3 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos;";
            }
            else
            {
                query2 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                    "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                    "Fecha_inicio, ultima_nomina, prima_vacacional " +
                    "FROM Empleados WHERE idEmpresa = " + _idEmpresa + ";";

                query3 = "SELECT idEmpresa,idDepartamento, Departamento, Sueldo_base, Gerente " +
                    "FROM Departamentos WHERE idempresa = " + _idEmpresa + ";";
            }

            try
            {
                conectar();
                //Agregar empleado
                _session.Execute(qry);

                //Actualizar lista de empleados
                IMapper mapper = new Mapper(_session);
                IEnumerable<Empleado> empleados;
                empleados = mapper.Fetch<Empleado>(query2);
                Variables.EMPLEADOS = empleados.ToList();

                //Actuaizar departamentos
                IEnumerable<Departamento> departamentos;
                departamentos = mapper.Fetch<Departamento>(query3);
                Variables.DEPARTAMENTOS = departamentos.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
                //throw e;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        #endregion Empleados

        #region Nominas
        public bool generarNominas(List<Empleado> _empleados,
            List<Tuple<string, int, string, decimal, decimal, decimal, decimal>> _listMontosNominas,
            List<Tuple<string, string, string, decimal, bool>> _listDatosEmpleados,
            string _frecuencia, string _hoy, string _fechaInicio, string _fechaFinal, 
            string _idEmpresa, string _fechaNuevaN)
        {
            bool sucess = true;

            try
            {
                string qry = "";
                string query = "";
                string query2 = "";
                string query3 = "";
                string query4 = "";
                string idEmpleado = "";
                string departamento = "";
                string puesto = "";
                string incidencias = "";
                int diasLaborados = 0;
                decimal sueldoD = 0;
                decimal totalP = 0;
                decimal totalD = 0;
                decimal totalB = 0;
                decimal totalN = 0;

                if(Variables.TYPE_USER == 0) //Admi
                {
                    query2 = "SELECT idEmpresa, Razon_social, Domicilio_fiscal, Registro_patronal, Registro_federal, " +
                        "Frecuencia_pago, Fecha_inicio, Telefono, Correo, Direccion, Incidencias, Ultima_Nomina " +
                        "FROM Empresas;";

                    query3 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                            "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                            "Fecha_inicio, ultima_nomina, prima_vacacional " +
                            "FROM Empleados; ";
                }
                else
                {
                    query2 = "SELECT idEmpresa, Razon_social, Domicilio_fiscal, Registro_patronal," +
                        " Registro_federal, Frecuencia_pago, Fecha_inicio, " +
                        "Telefono, Correo, Direccion, Incidencias, Ultima_Nomina " +
                        "FROM Empresas WHERE idEmpresa = " + _idEmpresa + ";";

                    query3 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                            "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                            "Fecha_inicio, ultima_nomina, prima_vacacional " +
                            "FROM Empleados WHERE idEmpresa = " + _idEmpresa +"; ";

                    string idUsuario = Variables.EMPLOYEE_INFO[0].no_empleado.ToString();

                    query4 = "SELECT idEmpresa,No_empleado, idPuesto, idDepartamento, Password, Nombre, Apellidos," +
                            "Fecha_Nacimiento, CURP, NSS, RFC, Domicilio, Banco, Numero_cuenta, Email, Telefonos," +
                            "Fecha_inicio, ultima_nomina, prima_vacacional " +
                            "FROM Empleados WHERE idEmpresa = " + _idEmpresa +
                            " AND No_empleado = " + idUsuario + ";";

                }

                conectar();

                qry = "BEGIN BATCH ";
                //Por cada empleado
                for (int i = 0; i < _empleados.Count; i++)
                {
                    idEmpleado = _empleados[i].no_empleado.ToString();
                    bool primaVacacional = false;

                    for (int j = 0; j < _listMontosNominas.Count; j++)
                    {
                        if(idEmpleado == _listMontosNominas[j].Item1)
                        {
                            diasLaborados = _listMontosNominas[j].Item2;
                            incidencias = _listMontosNominas[j].Item3;
                            totalP = _listMontosNominas[j].Item4;
                            totalD = _listMontosNominas[j].Item5;
                            totalB = _listMontosNominas[j].Item6;
                            totalN = _listMontosNominas[j].Item7;
                            break;
                        }
                    }

                    for (int j = 0; j < _listDatosEmpleados.Count; j++)
                    {
                        if (idEmpleado == _listDatosEmpleados[j].Item1)
                        {
                            departamento = _listDatosEmpleados[j].Item2;
                            puesto = _listDatosEmpleados[j].Item3;
                            sueldoD = _listDatosEmpleados[j].Item4;
                            primaVacacional = _listDatosEmpleados[j].Item5;
                            break;
                        }
                    }

                    query = " INSERT INTO Nominas (" +
                        "idEmpresa, " +
                        "idNomina, " +
                        "idEmpleado, " +
                        "idDepartamento, " +
                        "idPuesto, " +
                        "Empleado, " +
                        "Departamento, " +
                        "Puesto, " +
                        "Fecha_ingreso, " +
                        "Fecha_Nacimiento, " +
                        "Frecuencia, " +
                        "Fecha_nomina, " +
                        "Fecha_inicio, " +
                        "Fecha_final, " +
                        "Dias_trabajados, " +
                        "Salario_diario, " +
                        "Incidencias, " +
                        "Total_percepciones, " +
                        "Total_deducciones, " +
                        "Sueldo_neto, " +
                        "Sueldo_bruto, " +
                        "Banco, " +
                        "Numero_cuenta) " +
                        "VALUES (" +
                        _empleados[i].idEmpresa.ToString() + ", " +
                        "uuid(), " +
                        _empleados[i].no_empleado + ", " +
                        _empleados[i].idDepartamento + ", " +
                        _empleados[i].idPuesto + ", " +
                        "'" + _empleados[i].Nombre + " " + _empleados[i].Apellidos + "', " +
                        "'" + departamento + "', " +
                        "'" + puesto + "', " +
                        "'" + _empleados[i].fecha_inicio.ToString() + "', " +
                        "'" + _empleados[i].fecha_nacimiento.ToString() + "', " +
                        "'" + _frecuencia + "', " +
                        "'" + _hoy + "', " +
                        "'" + _fechaInicio + "', " +
                        "'" + _fechaFinal + "', " +
                        diasLaborados.ToString() + ", " +
                        sueldoD.ToString() + ", " +
                        "[ " + incidencias + "], " +
                        totalP.ToString() + ", " +
                        totalD.ToString() + ", " +
                        totalN.ToString() + ", " +
                        totalB.ToString() + ", " +
                        "'" + _empleados[i].banco + "', " +
                        _empleados[i].numero_cuenta.ToString() + "); ";

                    qry = qry + query;

                    //Actualizar fecha de nueva nomina en empresa y prima vacacional
                    if (primaVacacional)
                    {
                        qry = qry + "UPDATE Empleados " +
                            "SET ultima_nomina = '" + _fechaNuevaN + "', " +
                            "prima_vacacional = '" + _fechaInicio + "' " +
                            "WHERE idEmpresa = " + _idEmpresa + " " +
                            "AND no_empleado = " + _empleados[i].no_empleado + "; ";
                    }
                    else
                    {
                        qry = qry + "UPDATE Empleados " +
                        "SET ultima_nomina = '" + _fechaNuevaN + "' " +
                        "WHERE idEmpresa = " + _idEmpresa + " " +
                        "AND no_empleado = " + _empleados[i].no_empleado + "; ";
                    }
                    
                }
                //Actualizar fecha de nueva nomina en empresa
                qry = qry + "UPDATE Empresas " +
                    "SET Ultima_Nomina = '" + _fechaNuevaN + "' " +
                    "WHERE idEmpresa = " + _idEmpresa + "; " +
                    "APPLY BATCH;";

                _session.Execute(qry);

                //Actualizar empresas
                IMapper mapper = new Mapper(_session);
                IEnumerable<Empresa> empresas;
                empresas = mapper.Fetch<Empresa>(query2);
                Variables.EMPRESAS = empresas.ToList();

                IEnumerable<Empleado> empleados;
                empleados = mapper.Fetch<Empleado>(query3);
                Variables.EMPLEADOS = empleados.ToList();

                //Si es empleado actualizar fecha de ultima nomina
                if (Variables.TYPE_USER == 1)
                {
                    IEnumerable<Empleado> empleado;
                    empleado = mapper.Fetch<Empleado>(query4);
                    Variables.EMPLOYEE_INFO = empleado.ToList();
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                sucess = false;
            }
            finally
            {
                desconectar();
            }
            return sucess;
        }

        #endregion Nominas

        #region Reportes
        public void DGV_ReporteNominaGeneral(DataGridView _datagrid, string _idEmpresa, string _fechaInicio, string _fechaFinal)
        {
            string qry;
            string query = "SELECT idEmpresa, idNomina, idEmpleado, idDepartamento, idPuesto, Empleado, Departamento, Puesto, " +
                "Fecha_ingreso, Fecha_Nacimiento, Frecuencia, Fecha_nomina, Fecha_inicio, Fecha_final, Dias_trabajados, Salario_diario, " +
                "Incidencias, Total_percepciones, Total_deducciones, Sueldo_neto, Sueldo_bruto, Banco, Numero_cuenta " +
                "FROM Nominas " +
                "WHERE idEmpresa = {0} " +
                "AND Fecha_inicio >= '{1}' AND Fecha_inicio <= '{2}' ALLOW FILTERING; ";

            qry = string.Format(query, _idEmpresa, _fechaInicio, _fechaFinal);

            try
            {
                conectar();
                IMapper mapper = new Mapper(_session);
                IEnumerable<Nomina> nominas = mapper.Fetch<Nomina>(qry);
                List<Nomina> auxNominas = nominas.ToList();

                if (auxNominas.Count >0)
                {

                    for (int i = 0; i< auxNominas.Count; i++)
                    {
                        string departamento = auxNominas[i].Departamento;
                        string puesto = auxNominas[i].Puesto;

                        string empleado = auxNominas[i].Empleado;
                        string fechaI = auxNominas[i].Fecha_ingreso.ToString();
                        string fechaN = auxNominas[i].Fecha_Nacimiento.ToString();
                        string fechaNomina = auxNominas[i].Fecha_inicio.ToString();
                        string salarioD = "$" + auxNominas[i].Salario_diario.ToString();

                        _datagrid.Rows.Add(departamento, puesto, empleado, fechaI, fechaN, fechaNomina, salarioD);
                    }

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");

            }
            finally
            {
                desconectar();
            }
        }

        public void DGV_ReporteHeadcounter1(DataGridView _datagrid, string _departamento,
            string _empresa, string _idEmpresa, List<Departamento> departamentos, 
            string _fechaInicio, string _fechaFinal, string ultimaNomina)
        {
            List<Tuple<string,string,string>> listEmpleados; //idempleado, iddepartamento, idpuesto
            List<Tuple<string, int>> listCountDepto; //dpto, contador

            string query = "SELECT idEmpresa, idNomina, idEmpleado, idDepartamento, idPuesto, Empleado, Departamento, Puesto, " +
                "Fecha_ingreso, Fecha_Nacimiento, Frecuencia, Fecha_nomina, Fecha_inicio, Fecha_final, Dias_trabajados, Salario_diario, " +
                "Incidencias, Total_percepciones, Total_deducciones, Sueldo_neto, Sueldo_bruto, Banco, Numero_cuenta " +
                "FROM Nominas " +
                "WHERE idEmpresa = " + _idEmpresa + " " +
                "AND Fecha_inicio >= '" + _fechaInicio +"' " +
                "AND Fecha_inicio <= '" + _fechaFinal +"' ALLOW FILTERING; ";
            try
            {
                conectar();
                IMapper mapper = new Mapper(_session);
                IEnumerable<Nomina> nominas = mapper.Fetch<Nomina>(query);
                Variables.NOMINAS = nominas.ToList();
                List<Nomina> nomina = Variables.NOMINAS;

                listEmpleados = new List<Tuple<string, string, string>>();
                listCountDepto = new List<Tuple<string, int>>();

                
                //Recorrer nominas para crear lista de empleados
                for(int i = 0; i < nomina.Count; i++)
                {
                    string idEmpleado = nomina[i].idEmpleado.ToString();
                    string idDepartamento = nomina[i].idDepartamento.ToString();
                    string idPuesto = nomina[i].idPuesto.ToString();

                    var tupla =Tuple.Create(idEmpleado, idDepartamento, idPuesto);

                    if (listEmpleados.Count > 0)
                    {
                        //Recorrer lista de empleados para encontrar repetidos
                        bool repetido = false; 
                        for(int j = 0; j < listEmpleados.Count; j++)
                        {
                            if(listEmpleados[j].Item1 == idEmpleado)
                            {
                                repetido = true;
                                break;
                            }
                        }
                        if (!repetido)
                        {
                            listEmpleados.Add(tupla);
                        }
                    }
                    else //Primer empleado
                    {
                        listEmpleados.Add(tupla);
                    }
                }

                //Pasar lista de empleados para segunda parte de headcounter
                Variables.LISTEMPLEADOS = listEmpleados;


                //Crear listas de contadores
                for (int i = 0; i < departamentos.Count; i++)
                {
                    var tupla = Tuple.Create(departamentos[i].iddepartamento.ToString(), 0);
                    listCountDepto.Add(tupla);
                }

                //Recorrer lista de contadores departamentos
                for (int i = 0; i < listCountDepto.Count; i++)
                {
                    string idDepartamento = listCountDepto[i].Item1;
                    int contador = listCountDepto[i].Item2;
                    //Recorrer lista de empleados para encontrar los que trabajan en ese puesto
                    for (int j = 0; j < listEmpleados.Count; j++)
                    {
                        string empleadoDepto = listEmpleados[j].Item2;
                        if (idDepartamento == empleadoDepto)
                        {
                            //Actualizar contador
                            contador++;
                            var tupla = Tuple.Create(idDepartamento, contador);
                            listCountDepto[i] = tupla;
                        }
                    }
                }

                //Colocar datagrid
                if (_departamento == "Todos los departamentos")
                {
                    for (int i = 0; i < departamentos.Count; i++)
                    {
                        string idDepartamento = departamentos[i].iddepartamento.ToString();
                        string depto = departamentos[i].departamento;
                        string gerente = departamentos[i].gerente;
                        int count = 0; ;
                        //recorrer contadores
                        for (int j = 0; j < listCountDepto.Count; j++)
                        {
                            if (idDepartamento == listCountDepto[j].Item1)
                            {
                                count = listCountDepto[j].Item2;
                                break;
                            }
                        }

                        _datagrid.Rows.Add(_empresa, depto, gerente, count, ultimaNomina);
                    }
                }
                else//Filtro departamento
                {
                    for (int i = 0; i < departamentos.Count; i++)
                    {
                        if (departamentos[i].departamento == _departamento)
                        {
                            string idDepartamento = departamentos[i].iddepartamento.ToString();
                            string depto = departamentos[i].departamento;
                            string gerente = departamentos[i].gerente;
                            int count = 0; ;
                            //recorrer contadores
                            for (int j = 0; j < listCountDepto.Count; j++)
                            {
                                if (idDepartamento == listCountDepto[j].Item1)
                                {
                                    count = listCountDepto[j].Item2;
                                    break;
                                }
                            }

                            _datagrid.Rows.Add(_empresa, depto, gerente, count, ultimaNomina);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            {
                desconectar();
            }
        }

        public void DGV_ReporteHeadcounter2(DataGridView _datagrid, string _departamento, string _gerente,
            string _empresa, List<Puesto> puestos, List<Departamento> departamentos)
        {
            List<Tuple<string, string, string>> listEmpleados; //idempleado, iddepartamento, idpuesto
            List<Tuple<string, int>> listCountPuesto; //idpuesto, contador

            //Recibir lista de empleados de primer parte del headcounter
            listEmpleados = Variables.LISTEMPLEADOS;

            //Crear listas de contadores
            listCountPuesto = new List<Tuple<string, int>>();
            for (int i = 0; i < puestos.Count; i++)
            {
                var tupla = Tuple.Create(puestos[i].idpuesto.ToString(), 0);
                listCountPuesto.Add(tupla);
            }

            //Recorrer lista de contadores puesto
            for (int i = 0; i < listCountPuesto.Count; i++)
            {
                string idPuesto = listCountPuesto[i].Item1;
                int contador = listCountPuesto[i].Item2;
                //Recorrer lista de empleados para encontrar los que trabajan en ese puesto
                for (int j = 0; j < listEmpleados.Count; j++)
                {
                    string axuIdDepartamento = listEmpleados[j].Item2;
                    string empleadoPuesto = listEmpleados[j].Item3;
                    string empleadoDepto = "";
                    //Buscar departamento de empleado
                    for(int k = 0; k < departamentos.Count; k++)
                    {
                        if(axuIdDepartamento == departamentos[k].iddepartamento.ToString())
                        {
                            empleadoDepto = departamentos[k].departamento;
                        }
                    }

                    if (empleadoDepto == _departamento && idPuesto == empleadoPuesto)
                    {
                        //Actualizar contador
                        contador++;
                        var tupla = Tuple.Create(idPuesto, contador);
                        listCountPuesto[i] = tupla;
                    }
                }
            }

            //Colocar datagrid
            for (int i = 0; i < puestos.Count; i++)
            {
                string _puesto = puestos[i].puesto; 
                string idPuesto = puestos[i].idpuesto.ToString();
                int count = 0; 
                //recorrer contadores
                for (int j = 0; j < listCountPuesto.Count; j++)
                {
                    if (idPuesto == listCountPuesto[j].Item1)
                    {
                        count = listCountPuesto[j].Item2;
                        break;
                    }
                }

                _datagrid.Rows.Add(_empresa, _departamento, _gerente , _puesto, count.ToString());
            }
        }

        public void DGV_ReporteNomina(DataGridView _datagrid, string _departamento,
            string _empresa, string _idEmpresa, List<Departamento> _departamentos,
            string _fechaInicio, string _fechaFinal, string _año, string _mes)
        {

            List<Tuple<
                string, //idDepartamento
                string, //departamento,
                string, //gerente
                decimal, //contadorIMMS
                decimal, //contadorISR
                decimal, //contadorSueldoB
                decimal>> listContadores;  //contadorSueldoN

            string query = "SELECT idEmpresa, idNomina, idEmpleado, idDepartamento, idPuesto, " +
                "Empleado, Departamento, Puesto, Fecha_ingreso, Fecha_Nacimiento, Frecuencia, Fecha_nomina, Fecha_inicio, " +
                "Fecha_final, Dias_trabajados, Salario_diario, Incidencias, Total_percepciones, " +
                "Total_deducciones, Sueldo_neto, Sueldo_bruto, Banco, Numero_cuenta " +
                "FROM Nominas " +
                "WHERE Fecha_inicio >= '" + _fechaInicio + "' " +
                "AND Fecha_inicio <= '" + _fechaFinal + "' ALLOW FILTERING; ";
            try
            {
                conectar();
                IMapper mapper = new Mapper(_session);
                IEnumerable<Nomina> nominas = mapper.Fetch<Nomina>(query);
                List<Nomina> nomina = nominas.ToList();

                //Crear lista de contadores por departamento
                listContadores = new List<Tuple<string, string, string, decimal, decimal, decimal, decimal>>();
                if(_departamento == "Todos los departamentos") //Sin filtro
                {
                    for (int i = 0; i < _departamentos.Count; i++)
                    {
                        string idDepto = _departamentos[i].iddepartamento.ToString();
                        string departamento = _departamentos[i].departamento;
                        string gerente = _departamentos[i].gerente;
                        decimal valor = 0;
                        var tupla = Tuple.Create(idDepto, departamento, gerente, valor, valor, valor, valor);
                        listContadores.Add(tupla);
                    }
                }
                else //Con filtro
                {
                    for (int i = 0; i < _departamentos.Count; i++)
                    {
                        string idDepto = _departamentos[i].iddepartamento.ToString();
                        string departamento = _departamentos[i].departamento;
                        string gerente = _departamentos[i].gerente;
                        if (_departamento == departamento)
                        {
                            decimal valor = 0;
                            var tupla = Tuple.Create(idDepto, departamento, gerente, valor, valor, valor, valor);
                            listContadores.Add(tupla);
                            break;
                        }
                    }
                }

                //Iniciar contadores
                //Recorrer lista de contadores
                for(int i =0; i < listContadores.Count; i++)
                {
                    string idDepto = listContadores[i].Item1;
                    string departamento = listContadores[i].Item2;
                    string gerente = listContadores[i].Item3;
                    decimal IMSS = listContadores[i].Item4;
                    decimal ISR = listContadores[i].Item5;
                    decimal sueldoB = listContadores[i].Item6;
                    decimal sueldoN = listContadores[i].Item7;

                    //Recorer nominas
                    for(int j = 0; j < nomina.Count; j++)
                    {
                        string auxIdDepartamento = nomina[j].idDepartamento.ToString();
                        //Si la nomina coincide con el departamento actual, sumar contadores
                        if (auxIdDepartamento == idDepto)
                        {
                            //Recorrer incidencias de la nomina
                            for (int k = 0; k < nomina[j].Incidencias.Count; k++)
                            {
                                if (nomina[j].Incidencias[k].Item2 == "IMSS")
                                {
                                    IMSS += nomina[j].Incidencias[k].Item3;
                                }
                                if (nomina[j].Incidencias[k].Item2 == "ISR")
                                {
                                    ISR += nomina[j].Incidencias[k].Item3;
                                }
                            }
                            sueldoB += nomina[j].Sueldo_bruto;
                            sueldoN += nomina[j].Sueldo_neto;
                        }
                    }
                    //Actualizar lista con nuevos valores
                    var tupla = Tuple.Create(idDepto, departamento, gerente, IMSS, ISR, sueldoB, sueldoN);
                    listContadores[i] = tupla;
                }

                
                //Colocar datagrid
                decimal totalIMSS = 0;
                decimal totalISR = 0;
                decimal totalSueldoB = 0;
                decimal totalSueldoN = 0;
                for(int i = 0; i < listContadores.Count; i++)
                {
                    string departamento = listContadores[i].Item2;
                    string gerente = listContadores[i].Item3;
                    decimal IMSS = listContadores[i].Item4;
                    decimal ISR = listContadores[i].Item5;
                    decimal sueldoB = listContadores[i].Item6;
                    decimal sueldoN = listContadores[i].Item7;
                    _datagrid.Rows.Add(_empresa, departamento, gerente, _año, _mes,
                        "$" + IMSS, "$" + ISR, "$" + sueldoB, "$" + sueldoN);

                    totalIMSS += IMSS;
                    totalISR += ISR;
                    totalSueldoB += sueldoB;
                    totalSueldoN += sueldoN;
                }
                //Totales
                if(listContadores.Count > 0)
                {
                    _datagrid.Rows.Add("", "", "", "","",
                        "∑$" + totalIMSS, "∑$" + totalISR, "∑$" + totalSueldoB, "∑$" + totalSueldoN);
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
            }
            finally
            {
                desconectar();
            }
        }

        #endregion Reportes

        #region ReportesUsuario
        public void DGV_ResumenPagos(DataGridView _datagrid, string _año,
            string _fechaInicio, string _fechaFin)
        {
            string idEmpresa = Variables.EMPLOYEE_INFO[0].idEmpresa.ToString();
            string idEmpleado = Variables.EMPLOYEE_INFO[0].no_empleado.ToString();
            string RFC = Variables.EMPLOYEE_INFO[0].rfc;
            string NSS = Variables.EMPLOYEE_INFO[0].nss;
            List<Tuple<int, //Mes
                decimal,    //IMSS
                decimal,    //ISR
                decimal,    //SueldoB
                decimal>> listTotal; //SueldoN

            string qry;
            string query = "SELECT idEmpresa, idNomina, idEmpleado, idDepartamento, idPuesto, Empleado, Departamento, Puesto, " +
                "Fecha_ingreso, Fecha_Nacimiento, Frecuencia, Fecha_nomina, Fecha_inicio, Fecha_final, Dias_trabajados, " +
                "Salario_diario, Incidencias, Total_percepciones, Total_deducciones, Sueldo_neto, Sueldo_bruto, Banco, Numero_cuenta " +
                "FROM Nominas " +
                "WHERE idEmpresa = {0} " +
                "AND idEmpleado = {1} " +
                "AND Fecha_inicio >= '{2}' " +
                "AND Fecha_inicio <= '{3}' ALLOW FILTERING;";

            qry = string.Format(query, idEmpresa, idEmpleado, _fechaInicio, _fechaFin);
            List<Nomina> nomina = null;
            try
            {
                conectar();
                IMapper mapper = new Mapper(_session);
                IEnumerable<Nomina> nominas = mapper.Fetch<Nomina>(qry);
                nomina = nominas.ToList();

                //Crear lista de totales por mes
                listTotal = new List<Tuple<int, decimal, decimal, decimal, decimal>>();
                for(int i = 1; i < 13; i++)
                {
                    decimal valor = 0;
                    var tupla = Tuple.Create(i, valor, valor, valor, valor);
                    listTotal.Add(tupla);
                }

                string aux = "";
                //Recorrer meses del año
                for (int i = 1; i < 13; i++)
                {
                    decimal IMSS = 0;
                    decimal ISR = 0;
                    decimal sueldoB = 0;
                    decimal sueldoN = 0;
                    //Recorrer nominas
                    for (int j = 0; j < nomina.Count; j++)
                    {
                        DateTime fechaI = DateTime.Parse(nomina[j].Fecha_inicio.ToString());
                        aux = fechaI.ToString("MM");
                        int mes = int.Parse(aux);

                        if(mes == i)
                        {
                            //Recorrer incidencias de la nomina
                            for(int k = 0; k < nomina[j].Incidencias.Count; k++)
                            {
                                string concepto = nomina[j].Incidencias[k].Item2;
                                if(concepto == "IMSS")
                                {
                                    IMSS += nomina[j].Incidencias[k].Item3;
                                }
                                if (concepto == "ISR")
                                {
                                    ISR += nomina[j].Incidencias[k].Item3;
                                }
                            }

                            sueldoB += nomina[j].Sueldo_bruto;
                            sueldoN += nomina[j].Sueldo_neto;
                        }
                    }

                    //Actualizar valores de lista
                    var tupla = Tuple.Create(i, IMSS, ISR, sueldoB, sueldoN);
                    listTotal[i-1] = tupla;
                }

                //Agregar a datagrid
                string[] meses = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio",
                "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};
                _datagrid.Rows.Clear();
                for(int i = 0; i < listTotal.Count; i++)
                {
                    decimal IMSS = listTotal[i].Item2;
                    decimal ISR = listTotal[i].Item3;
                    decimal sueldoB = listTotal[i].Item4;
                    decimal sueldoN = listTotal[i].Item5;

                    _datagrid.Rows.Add(RFC, NSS, _año, meses[i], "∑$" + IMSS, "∑$" + ISR,
                        "∑$" + sueldoB, "∑$" + sueldoN);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");

            }
            finally
            {
                desconectar();
            }
        }

        public List<Nomina>DGV_RecibosNomina(DataGridView _datagrid)
        {
            string idEmpresa = Variables.EMPLOYEE_INFO[0].idEmpresa.ToString();
            string idEmpleado = Variables.EMPLOYEE_INFO[0].no_empleado.ToString();
            string qry;
            string query = "SELECT idEmpresa, idNomina, idEmpleado, idDepartamento, idPuesto, Empleado, " +
                "Departamento, Puesto, Fecha_ingreso, Fecha_Nacimiento, Frecuencia, Fecha_nomina, Fecha_inicio, Fecha_final, " +
                "Dias_trabajados, Salario_diario, Incidencias, Total_percepciones, Total_deducciones, " +
                "Sueldo_neto, Sueldo_bruto, Banco, Numero_cuenta " +
                "FROM Nominas " +
                "WHERE idEmpresa = {0} " +
                "AND idEmpleado = {1};";

            qry = string.Format(query, idEmpresa, idEmpleado);
            List<Nomina> auxNominas = null;
            try
            {
                conectar();
                IMapper mapper = new Mapper(_session);
                IEnumerable<Nomina> nominas = mapper.Fetch<Nomina>(qry);
                auxNominas = nominas.ToList();

                _datagrid.DataSource = null;
                _datagrid.Rows.Clear();
                if (auxNominas.Count > 0)
                {
                    _datagrid.DataSource = auxNominas;
                    _datagrid.Columns[0].Visible = false;
                    _datagrid.Columns[1].HeaderText = "ID NOMINA";
                    _datagrid.Columns[2].Visible = false;
                    _datagrid.Columns[3].Visible = false;
                    _datagrid.Columns[4].Visible = false;
                    _datagrid.Columns[5].Visible = false;
                    _datagrid.Columns[6].Visible = false;
                    _datagrid.Columns[7].Visible = false;
                    _datagrid.Columns[8].Visible = false;
                    _datagrid.Columns[9].Visible = false;
                    _datagrid.Columns[10].HeaderText = "FRECUENCIA";
                    _datagrid.Columns[11].Visible = false;
                    _datagrid.Columns[12].HeaderText = "FECHA DE NOMINA";
                    _datagrid.Columns[13].Visible = false;
                    _datagrid.Columns[14].HeaderText = "DIAS LABORADOS";
                    _datagrid.Columns[15].HeaderText = "SALARIO DIARIO";
                    _datagrid.Columns[16].HeaderText = "TOTAL PERCEPCIONES";
                    _datagrid.Columns[17].HeaderText = "TOTAL DEDUCCIONES";
                    _datagrid.Columns[18].HeaderText = "SUELDO NETO";
                    _datagrid.Columns[19].HeaderText = "SUELDO BRUTO";
                    _datagrid.Columns[20].Visible = false;
                    _datagrid.Columns[21].Visible = false;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");

            }
            finally
            {
                desconectar();
            }
            return auxNominas;
        }
        #endregion ReportesUsuario

    }
}
