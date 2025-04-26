namespace K_F_ClothingStore.Models
{
    using System.Data;
    using System.Data.SqlClient;

    public class AccesoDatos
    {
        private readonly string _conexion;

        public AccesoDatos(IConfiguration configuracion)
        {
            _conexion = configuracion.GetConnectionString("DefaultConnection");
        }


        // Métodos para la tabla Direccion
        public void AgregarDireccion(Direccion direccion)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_InsertDireccion @Ciudad, @Estado, @CodigoPostal, @Pais, @TipoDireccion, @CreadoPor, @NewID OUTPUT";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
                        cmd.Parameters.AddWithValue("@Estado", direccion.Estado);
                        cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);
                        cmd.Parameters.AddWithValue("@Pais", direccion.Pais);
                        cmd.Parameters.AddWithValue("@TipoDireccion", direccion.TipoDireccion);
                        cmd.Parameters.AddWithValue("@CreadoPor", direccion.CreadoPor);
                        cmd.Parameters.Add("@NewID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        direccion.ID = (int)cmd.Parameters["@NewID"].Value;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar la dirección: " + ex.Message);
                }
            }
        }

        public List<Direccion> ObtenerDirecciones()
        {
            List<Direccion> direcciones = new List<Direccion>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllDirecciones";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                direcciones.Add(new Direccion
                                {
                                    ID = reader.GetInt32(i: 0),
                                    Ciudad = reader.GetString(i: 1),
                                    Estado = reader.GetString(i: 2),
                                    CodigoPostal = reader.GetString(i: 3),
                                    Pais = reader.GetString(i: 4),
                                    TipoDireccion = reader.IsDBNull(i: 5) ? null : reader.GetString(i: 5),
                                    FechaCreacion = reader.GetDateTime(i: 6),
                                    CreadoPor = reader.GetString(i: 7),
                                    FechaModificacion = reader.IsDBNull(i: 8) ? null : reader.GetDateTime(i: 8),
                                    ModificadoPor = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener las direcciones: " + ex.Message);
                }
            }

            return direcciones;
        }

        public Direccion ObtenerDireccionPorId(int id)
        {
            Direccion direccion = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetDireccionById @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                direccion = new Direccion
                                {
                                    ID = reader.GetInt32(i: 0),
                                    Ciudad = reader.GetString(i: 1),
                                    Estado = reader.GetString(i: 2),
                                    CodigoPostal = reader.GetString(i: 3),
                                    Pais = reader.GetString(i: 4),
                                    TipoDireccion = reader.GetString(i: 5),
                                    FechaCreacion = reader.GetDateTime(i: 6),
                                    CreadoPor = reader.GetString(i: 7),
                                    FechaModificacion = reader.IsDBNull(i: 8) ? null : reader.GetDateTime(i: 8),
                                    ModificadoPor = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9)
                                };
                            }
                            else
                            {
                                // Manejo cuando no se encuentra la dirección
                                throw new Exception($"No se encontró la dirección con ID: {id}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la dirección: " + ex.Message);
                }
            }

            return direccion;
        }


        public void ActualizarDireccion(Direccion direccion)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_UpdateDireccion @ID, @Ciudad, @Estado, @CodigoPostal, @Pais, @TipoDireccion, @ModificadoPor";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", direccion.ID);
                        cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
                        cmd.Parameters.AddWithValue("@Estado", direccion.Estado);
                        cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);
                        cmd.Parameters.AddWithValue("@Pais", direccion.Pais);
                        cmd.Parameters.AddWithValue("@TipoDireccion", direccion.TipoDireccion);
                        cmd.Parameters.AddWithValue("@ModificadoPor", direccion.ModificadoPor);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar la dirección: " + ex.Message);
                }
            }
        }

        public void EliminarDireccion(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeleteDireccion @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la dirección: " + ex.Message);
                }
            }
        }

        // Métodos para la tabla Persona
        public void AgregarPersona(Persona persona)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_InsertPersona @Nombre1, @Nombre2, @Apellido1, @Apellido2, @DocumentoIdentidad, @Telefono, @Email, @FechaNacimiento, @Genero, @DireccionID, @CreadoPor, @NewID OUTPUT";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre1", persona.Nombre1);
                        cmd.Parameters.AddWithValue("@Nombre2", persona.Nombre2);
                        cmd.Parameters.AddWithValue("@Apellido1", persona.Apellido1);
                        cmd.Parameters.AddWithValue("@Apellido2", persona.Apellido2);
                        cmd.Parameters.AddWithValue("@DocumentoIdentidad", persona.DocumentoIdentidad);
                        cmd.Parameters.AddWithValue("@Telefono", persona.Telefono);
                        cmd.Parameters.AddWithValue("@Email", persona.Email);
                        cmd.Parameters.AddWithValue("@FechaNacimiento", persona.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@Genero", persona.Genero);
                        cmd.Parameters.AddWithValue("@DireccionID", persona.DireccionID);
                        cmd.Parameters.AddWithValue("@CreadoPor", persona.CreadoPor);
                        cmd.Parameters.Add("@NewID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        persona.ID = (int)cmd.Parameters["@NewID"].Value;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar la persona: " + ex.Message);
                }
            }
        }

        public List<Persona> ObtenerPersonas()
        {
            List<Persona> personas = new List<Persona>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllPersonas";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                personas.Add(new Persona
                                {
                                    ID = reader.GetInt32(i: 0),
                                    Nombre1 = reader.GetString(i: 1),
                                    Nombre2 = reader.IsDBNull(i: 2) ? null : reader.GetString(i: 2),
                                    Apellido1 = reader.GetString(i: 3),
                                    Apellido2 = reader.GetString(i: 4),
                                    DocumentoIdentidad = reader.GetString(i: 5),
                                    Telefono = reader.IsDBNull(i: 6) ? null : reader.GetString(i: 6),
                                    Email = reader.IsDBNull(i: 7) ? null : reader.GetString(i: 7),
                                    FechaNacimiento = reader.IsDBNull(i: 8) ? null : reader.GetDateTime(i: 8),
                                    Genero = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9),
                                    DireccionID = reader.GetInt32(i: 10),
                                    CreadoPor = reader.GetString(i: 11),
                                    FechaModificacion = reader.IsDBNull(i: 12) ? null : reader.GetDateTime(i: 12),
                                    ModificadoPor = reader.IsDBNull(i: 13) ? null : reader.GetString(i: 13)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener las personas: " + ex.Message);
                }
            }

            return personas;
        }

        public Persona ObtenerPersonaPorId(int id)
        {
            Persona persona = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetPersonaById @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                persona = new Persona
                                {
                                    ID = reader.GetInt32(i: 0),
                                    Nombre1 = reader.GetString(i: 1),
                                    Nombre2 = reader.IsDBNull(i: 2) ? null : reader.GetString(i: 2),
                                    Apellido1 = reader.GetString(i: 3),
                                    Apellido2 = reader.GetString(i: 4),
                                    DocumentoIdentidad = reader.GetString(i: 5),
                                    Telefono = reader.IsDBNull(i: 6) ? null : reader.GetString(i: 6),
                                    Email = reader.IsDBNull(i: 7) ? null : reader.GetString(i: 7),
                                    FechaNacimiento = reader.IsDBNull(i: 8) ? null : reader.GetDateTime(i: 8),
                                    Genero = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9),
                                    DireccionID = reader.GetInt32(i: 10),
                                    CreadoPor = reader.GetString(i: 11),
                                    FechaModificacion = reader.IsDBNull(i: 12) ? null : reader.GetDateTime(i: 12),
                                    ModificadoPor = reader.IsDBNull(i: 13) ? null : reader.GetString(i: 13)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la persona: " + ex.Message);
                }
            }

            return persona;
        }

        public void ActualizarPersona(Persona persona)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_UpdatePersona @ID, @Nombre1, @Nombre2, @Apellido1, @Apellido2, @Telefono, @Email, @FechaNacimiento, @Genero, @DireccionID, @ModificadoPor";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", persona.ID);
                        cmd.Parameters.AddWithValue("@Nombre1", persona.Nombre1);
                        cmd.Parameters.AddWithValue("@Nombre2", persona.Nombre2);
                        cmd.Parameters.AddWithValue("@Apellido1", persona.Apellido1);
                        cmd.Parameters.AddWithValue("@Apellido2", persona.Apellido2);
                        cmd.Parameters.AddWithValue("@Telefono", persona.Telefono);
                        cmd.Parameters.AddWithValue("@Email", persona.Email);
                        cmd.Parameters.AddWithValue("@FechaNacimiento", persona.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@Genero", persona.Genero);
                        cmd.Parameters.AddWithValue("@DireccionID", persona.DireccionID);
                        cmd.Parameters.AddWithValue("@ModificadoPor", persona.ModificadoPor);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar la persona: " + ex.Message);
                }
            }
        }

        public void EliminarPersona(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeletePersona @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la persona: " + ex.Message);
                }
            }
        }

        // Métodos para la tabla Cliente
        public void AgregarCliente(Cliente cliente)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_InsertCliente @PersonaID, @CodigoCliente, @Estado, @CreadoPor, @NewID OUTPUT";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PersonaID", cliente.PersonaID);
                        cmd.Parameters.AddWithValue("@CodigoCliente", cliente.CodigoCliente);
                        cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                        cmd.Parameters.AddWithValue("@CreadoPor", cliente.CreadoPor);
                        cmd.Parameters.Add("@NewID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        cliente.ID = (int)cmd.Parameters["@NewID"].Value;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar el cliente: " + ex.Message);
                }
            }
        }

        public List<Cliente> ObtenerClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllClientes";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clientes.Add(new Cliente
                                {
                                    ID = reader.GetInt32(i: 0),
                                    PersonaID = reader.GetInt32(i: 1),
                                    CodigoCliente = reader.GetInt32(i: 2),
                                    Estado = reader.GetString(i: 3),
                                    CreadoPor = reader.GetString(i: 4),
                                    FechaCreacion = reader.GetDateTime(i: 5),
                                    FechaModificacion = reader.IsDBNull(i: 6) ? null : reader.GetDateTime(i: 6),
                                    ModificadoPor = reader.IsDBNull(i: 7) ? null : reader.GetString(i: 7)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los clientes: " + ex.Message);
                }
            }

            return clientes;
        }

        public Cliente ObtenerClientePorId(int id)
        {
            Cliente cliente = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetClienteById @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cliente = new Cliente
                                {
                                    ID = reader.GetInt32(i: 0),
                                    PersonaID = reader.GetInt32(i: 1),
                                    CodigoCliente = reader.GetInt32(i: 2),
                                    Estado = reader.GetString(i: 3),
                                    CreadoPor = reader.GetString(i: 4),
                                    FechaCreacion = reader.GetDateTime(i: 5),
                                    FechaModificacion = reader.IsDBNull(i: 6) ? null : reader.GetDateTime(i: 6),
                                    ModificadoPor = reader.IsDBNull(i: 7) ? null : reader.GetString(i: 7)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el cliente: " + ex.Message);
                }
            }

            return cliente;
        }

        public void ActualizarCliente(Cliente cliente)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_UpdateCliente @ID, @CodigoCliente, @Estado, @ModificadoPor";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", cliente.ID);
                        cmd.Parameters.AddWithValue("@CodigoCliente", cliente.CodigoCliente);
                        cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                        cmd.Parameters.AddWithValue("@ModificadoPor", cliente.ModificadoPor);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar el cliente: " + ex.Message);
                }
            }
        }

        public void EliminarCliente(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeleteCliente @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el cliente: " + ex.Message);
                }
            }
        }

        // Métodos para la tabla Empleado
        public void AgregarEmpleado(Empleado empleado)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_InsertEmpleado @PersonaID, @Puesto, @FechaContratacion, @Salario, @Estado, @CreadoPor, @NewID OUTPUT";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PersonaID", empleado.PersonaID);
                        cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                        cmd.Parameters.AddWithValue("@FechaContratacion", empleado.FechaContratacion);
                        cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                        cmd.Parameters.AddWithValue("@Estado", empleado.Estado);
                        cmd.Parameters.AddWithValue("@CreadoPor", empleado.CreadoPor);
                        cmd.Parameters.Add("@NewID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        empleado.ID = (int)cmd.Parameters["@NewID"].Value;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar el empleado: " + ex.Message);
                }
            }
        }

        public List<Empleado> ObtenerEmpleados()
        {
            List<Empleado> empleados = new List<Empleado>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllEmpleados";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                empleados.Add(new Empleado
                                {
                                    ID = reader.GetInt32(i: 0),
                                    PersonaID = reader.GetInt32(i: 1),
                                    Puesto = reader.GetString(i: 2),
                                    FechaContratacion = reader.GetDateTime(i: 3),
                                    Salario = reader.GetDecimal(i: 4),
                                    Estado = reader.GetString(i: 5),
                                    CreadoPor = reader.GetString(i: 6),
                                    FechaCreacion = reader.GetDateTime(i: 7),
                                    FechaModificacion = reader.IsDBNull(i: 8) ? null : reader.GetDateTime(i: 8),
                                    ModificadoPor = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los empleados: " + ex.Message);
                }
            }

            return empleados;
        }

        public Empleado ObtenerEmpleadoPorId(int id)
        {
            Empleado empleado = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetEmpleadoById @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                empleado = new Empleado
                                {
                                    ID = reader.GetInt32(i: 0),
                                    PersonaID = reader.GetInt32(i: 1),
                                    Puesto = reader.GetString(i: 2),
                                    FechaContratacion = reader.GetDateTime(i: 3),
                                    Salario = reader.GetDecimal(i: 4),
                                    Estado = reader.GetString(i: 5),
                                    CreadoPor = reader.GetString(i: 6),
                                    FechaCreacion = reader.GetDateTime(i: 7),
                                    FechaModificacion = reader.IsDBNull(i: 8) ? null : reader.GetDateTime(i: 8),
                                    ModificadoPor = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el empleado: " + ex.Message);
                }
            }

            return empleado;
        }

        public void ActualizarEmpleado(Empleado empleado)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_UpdateEmpleado @ID, @Puesto, @FechaContratacion, @Salario, @Estado, @ModificadoPor";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", empleado.ID);
                        cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                        cmd.Parameters.AddWithValue("@FechaContratacion", empleado.FechaContratacion);
                        cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                        cmd.Parameters.AddWithValue("@Estado", empleado.Estado);
                        cmd.Parameters.AddWithValue("@ModificadoPor", empleado.ModificadoPor);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar el empleado: " + ex.Message);
                }
            }
        }

        public void EliminarEmpleado(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeleteEmpleado @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el empleado: " + ex.Message);
                }
            }
        }

        // Métodos para la tabla Producto
        public void AgregarProducto(Producto producto)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_InsertProducto @NombreProducto, @Genero, @SegmentoEdad, @TipoProducto, @Color, @Talla, @UnidadesDisponibles, @Precio, @Descripcion";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                        cmd.Parameters.AddWithValue("@Genero", producto.Genero);
                        cmd.Parameters.AddWithValue("@SegmentoEdad", producto.SegmentoEdad);
                        cmd.Parameters.AddWithValue("@TipoProducto", producto.TipoProducto);
                        cmd.Parameters.AddWithValue("@Color", producto.Color);
                        cmd.Parameters.AddWithValue("@Talla", producto.Talla);
                        cmd.Parameters.AddWithValue("@UnidadesDisponibles", producto.UnidadesDisponibles);
                        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar el producto: " + ex.Message);
                }
            }
        }

        public List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllProductos";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    ID = reader.GetInt32(i: 0),
                                    NombreProducto = reader.GetString(i: 1),
                                    Genero = reader.GetString(i: 2),
                                    SegmentoEdad = reader.GetString(i: 3),
                                    TipoProducto = reader.GetString(i: 4),
                                    Color = reader.GetString(i: 5),
                                    Talla = reader.GetString(i: 6),
                                    UnidadesDisponibles = reader.GetInt32(i: 7),
                                    Precio = reader.GetDecimal(i: 8),
                                    Descripcion = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9),
                                    FechaCreacion = reader.GetDateTime(i: 10),
                                    FechaModificacion = reader.IsDBNull(i: 11) ? null : reader.GetDateTime(i: 11)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los productos: " + ex.Message);
                }
            }

            return productos;
        }

        public Producto ObtenerProductoPorId(int id)
        {
            Producto producto = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetProductoById @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                producto = new Producto
                                {
                                    ID = reader.GetInt32(i: 0),
                                    NombreProducto = reader.GetString(i: 1),
                                    Genero = reader.GetString(i: 2),
                                    SegmentoEdad = reader.GetString(i: 3),
                                    TipoProducto = reader.GetString(i: 4),
                                    Color = reader.GetString(i: 5),
                                    Talla = reader.GetString(i: 6),
                                    UnidadesDisponibles = reader.GetInt32(i: 7),
                                    Precio = reader.GetDecimal(i: 8),
                                    Descripcion = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9),
                                    FechaCreacion = reader.GetDateTime(i: 10),
                                    FechaModificacion = reader.IsDBNull(i: 11) ? null : reader.GetDateTime(i: 11)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el producto: " + ex.Message);
                }
            }

            return producto;
        }

        public void ActualizarProducto(Producto producto)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_UpdateProducto @ID, @NombreProducto, @Genero, @SegmentoEdad, @TipoProducto, @Color, @Talla, @UnidadesDisponibles, @Precio, @Descripcion";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", producto.ID);
                        cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                        cmd.Parameters.AddWithValue("@Genero", producto.Genero);
                        cmd.Parameters.AddWithValue("@SegmentoEdad", producto.SegmentoEdad);
                        cmd.Parameters.AddWithValue("@TipoProducto", producto.TipoProducto);
                        cmd.Parameters.AddWithValue("@Color", producto.Color);
                        cmd.Parameters.AddWithValue("@Talla", producto.Talla);
                        cmd.Parameters.AddWithValue("@UnidadesDisponibles", producto.UnidadesDisponibles);
                        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar el producto: " + ex.Message);
                }
            }
        }

        public void EliminarProducto(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeleteProducto @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el producto: " + ex.Message);
                }
            }
        }

        // Métodos para la tabla Venta
        public void AgregarVenta(Venta venta)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_InsertVenta @EmpleadoID, @ClienteID, @ProductoID, @Cantidad, @Total";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@EmpleadoID", venta.EmpleadoID);
                        cmd.Parameters.AddWithValue("@ClienteID", venta.ClienteID);
                        cmd.Parameters.AddWithValue("@ProductoID", venta.ProductoID);
                        cmd.Parameters.AddWithValue("@Cantidad", venta.Cantidad);
                        cmd.Parameters.AddWithValue("@Total", venta.Total);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar la venta: " + ex.Message);
                }
            }
        }

        public List<Venta> ObtenerVentas()
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllVentas";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ventas.Add(new Venta
                                {
                                    VentaID = reader.GetInt32(i: 0),
                                    EmpleadoID = reader.GetInt32(i: 1),
                                    ClienteID = reader.GetInt32(i: 2),
                                    ProductoID = reader.GetInt32(i: 3),
                                    Cantidad = reader.GetInt32(i: 4),
                                    Total = reader.GetDecimal(i: 5),
                                    Fecha = reader.GetDateTime(i: 6)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener las ventas: " + ex.Message);
                }
            }

            return ventas;
        }

        public Venta ObtenerVentaPorId(int id)
        {
            Venta venta = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetVentaById @VentaID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@VentaID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                venta = new Venta
                                {
                                    VentaID = reader.GetInt32(i: 0),
                                    EmpleadoID = reader.GetInt32(i: 1),
                                    ClienteID = reader.GetInt32(i: 2),
                                    ProductoID = reader.GetInt32(i: 3),
                                    Cantidad = reader.GetInt32(i: 4),
                                    Total = reader.GetDecimal(i: 5),
                                    Fecha = reader.GetDateTime(i: 6)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la venta: " + ex.Message);
                }
            }

            return venta;
        }

        public void ActualizarVenta(Venta venta)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_UpdateVenta @VentaID, @EmpleadoID, @ClienteID, @ProductoID, @Cantidad, @Total";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@VentaID", venta.VentaID);
                        cmd.Parameters.AddWithValue("@EmpleadoID", venta.EmpleadoID);
                        cmd.Parameters.AddWithValue("@ClienteID", venta.ClienteID);
                        cmd.Parameters.AddWithValue("@ProductoID", venta.ProductoID);
                        cmd.Parameters.AddWithValue("@Cantidad", venta.Cantidad);
                        cmd.Parameters.AddWithValue("@Total", venta.Total);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar la venta: " + ex.Message);
                }
            }
        }

        public void EliminarVenta(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeleteVenta @VentaID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@VentaID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la venta: " + ex.Message);
                }
            }
        }

        // Métodos para la tabla Factura
        public int AgregarFactura(Factura factura)
        {
            int facturaID = 0;

            using (SqlConnection conexion = new SqlConnection(_conexion))
            {
                SqlCommand comando = new SqlCommand("sp_InsertFactura", conexion);
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@ClienteID", factura.ClienteID);
                comando.Parameters.AddWithValue("@Total", factura.Total);
                comando.Parameters.AddWithValue("@MetodoPago", factura.MetodoPago);
                comando.Parameters.AddWithValue("@Estado", factura.Estado);
                comando.Parameters.AddWithValue("@CreadoPor", factura.CreadoPor);

                // 🔹 Parámetro de salida para obtener el ID de la factura
                SqlParameter outputIdParam = new SqlParameter("@FacturaID", SqlDbType.Int);
                outputIdParam.Direction = ParameterDirection.Output;
                comando.Parameters.Add(outputIdParam);

                conexion.Open();
                comando.ExecuteNonQuery();

                // 🔹 Obtener el ID de la factura
                facturaID = Convert.ToInt32(outputIdParam.Value);
            }

            return facturaID;
        }

        public List<Factura> ObtenerFacturas()
        {
            List<Factura> facturas = new List<Factura>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllFacturas";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                facturas.Add(new Factura
                                {
                                    ID = reader.GetInt32(i: 0),
                                    ClienteID = reader.GetInt32(i: 1),
                                    FechaEmision = reader.GetDateTime(i: 2),
                                    Total = reader.GetDecimal(i: 3),
                                    MetodoPago = reader.GetString(i: 4),
                                    Estado = reader.GetString(i: 5),
                                    CreadoPor = reader.GetString(i: 6),
                                    FechaCreacion = reader.GetDateTime(i: 7),
                                    FechaModificacion = reader.IsDBNull(i: 8) ? null : reader.GetDateTime(i: 8),
                                    ModificadoPor = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener las facturas: " + ex.Message);
                }
            }

            return facturas;
        }

        public Factura ObtenerFacturaPorId(int id)
        {
            Factura factura = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_ObtenerFacturaPorId @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                factura = new Factura
                                {
                                    ID = reader.GetInt32(i: 0),
                                    ClienteID = reader.GetInt32(i: 1),
                                    Total = reader.GetDecimal(i: 2),
                                    MetodoPago = reader.GetString(i: 3),
                                    Estado = reader.GetString(i: 4),
                                    FechaCreacion = reader.GetDateTime(i: 5),
                                    FechaModificacion = reader.IsDBNull(i: 6) ? null : reader.GetDateTime(i: 6),
                                    CreadoPor = reader.GetString(i: 7),
                                    ModificadoPor = reader.IsDBNull(i: 8) ? null : reader.GetString(i: 8)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la factura: " + ex.Message);
                }
            }

            return factura;
        }

        public void ActualizarFactura(Factura factura)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_UpdateFactura @ID, @ClienteID, @EmpleadoID, @Total, @MetodoPago, @Estado, @DescuentoID, @ModificadoPor";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", factura.ID);
                        cmd.Parameters.AddWithValue("@ClienteID", factura.ClienteID);
                        cmd.Parameters.AddWithValue("@Total", factura.Total);
                        cmd.Parameters.AddWithValue("@MetodoPago", factura.MetodoPago);
                        cmd.Parameters.AddWithValue("@Estado", factura.Estado);
                        cmd.Parameters.AddWithValue("@ModificadoPor", factura.ModificadoPor);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar la factura: " + ex.Message);
                }
            }
        }

        public void EliminarFactura(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeleteFactura @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la factura: " + ex.Message);
                }
            }
        }

        // Métodos para la tabla DetalleFactura
        public void AgregarDetalleFactura(DetalleFactura detalleFactura)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_InsertDetalleFactura @FacturaID, @ProductoID, @Cantidad, @PrecioUnitario, @Subtotal, @NewID OUTPUT";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FacturaID", detalleFactura.FacturaID);
                        cmd.Parameters.AddWithValue("@ProductoID", detalleFactura.ProductoID);
                        cmd.Parameters.AddWithValue("@Cantidad", detalleFactura.Cantidad);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", detalleFactura.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@Subtotal", detalleFactura.Subtotal);
                        cmd.Parameters.Add("@NewID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        detalleFactura.ID = (int)cmd.Parameters["@NewID"].Value;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar el detalle de factura: " + ex.Message);
                }
            }
        }

        public List<DetalleFactura> ObtenerDetallesFactura()
        {
            List<DetalleFactura> detallesFactura = new List<DetalleFactura>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllDetallesFactura";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                detallesFactura.Add(new DetalleFactura
                                {
                                    ID = reader.GetInt32(i: 0),
                                    FacturaID = reader.GetInt32(i: 1),
                                    ProductoID = reader.GetInt32(i: 2),
                                    Cantidad = reader.GetInt32(i: 3),
                                    PrecioUnitario = reader.GetDecimal(i: 4),
                                    Subtotal = reader.GetDecimal(i: 5)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los detalles de factura: " + ex.Message);
                }
            }

            return detallesFactura;
        }

        public DetalleFactura ObtenerDetalleFacturaPorId(int id)
        {
            DetalleFactura detalleFactura = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetDetalleFacturaById @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                detalleFactura = new DetalleFactura
                                {
                                    ID = reader.GetInt32(i: 0),
                                    FacturaID = reader.GetInt32(i: 1),
                                    ProductoID = reader.GetInt32(i: 2),
                                    Cantidad = reader.GetInt32(i: 3),
                                    PrecioUnitario = reader.GetDecimal(i: 4),
                                    Subtotal = reader.GetDecimal(i: 5)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el detalle de factura: " + ex.Message);
                }
            }

            return detalleFactura;
        }

        public void ActualizarDetalleFactura(DetalleFactura detalleFactura)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_UpdateDetalleFactura @ID, @FacturaID, @ProductoID, @Cantidad, @PrecioUnitario, @Subtotal";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", detalleFactura.ID);
                        cmd.Parameters.AddWithValue("@FacturaID", detalleFactura.FacturaID);
                        cmd.Parameters.AddWithValue("@ProductoID", detalleFactura.ProductoID);
                        cmd.Parameters.AddWithValue("@Cantidad", detalleFactura.Cantidad);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", detalleFactura.PrecioUnitario);
                        cmd.Parameters.AddWithValue("@Subtotal", detalleFactura.Subtotal);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar el detalle de factura: " + ex.Message);
                }
            }
        }

        public void EliminarDetalleFactura(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeleteDetalleFactura @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el detalle de factura: " + ex.Message);
                }
            }
        }

        // Métodos para la tabla Devolucion
        public void AgregarDevolucion(Devolucion devolucion)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_InsertDevolucion @FacturaID, @DetalleFacturaID, @ProductoID, @Cantidad, @Motivo, @Estado, @CreadoPor, @NewID OUTPUT";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FacturaID", devolucion.FacturaID);
                        cmd.Parameters.AddWithValue("@DetalleFacturaID", devolucion.DetalleFacturaID);
                        cmd.Parameters.AddWithValue("@ProductoID", devolucion.ProductoID);
                        cmd.Parameters.AddWithValue("@Cantidad", devolucion.Cantidad);
                        cmd.Parameters.AddWithValue("@Motivo", devolucion.Motivo);
                        cmd.Parameters.AddWithValue("@Estado", devolucion.Estado);
                        cmd.Parameters.AddWithValue("@CreadoPor", devolucion.CreadoPor);
                        cmd.Parameters.Add("@NewID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        devolucion.ID = (int)cmd.Parameters["@NewID"].Value;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar la devolución: " + ex.Message);
                }
            }
        }

        public List<Devolucion> ObtenerDevoluciones()
        {
            List<Devolucion> devoluciones = new List<Devolucion>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllDevoluciones";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                devoluciones.Add(new Devolucion
                                {
                                    ID = reader.GetInt32(i: 0),
                                    FacturaID = reader.GetInt32(i: 1),
                                    DetalleFacturaID = reader.GetInt32(i: 2),
                                    ProductoID = reader.GetInt32(i: 3),
                                    Cantidad = reader.GetInt32(i: 4),
                                    Motivo = reader.GetString(i: 5),
                                    FechaDevolucion = reader.GetDateTime(i: 6),
                                    Estado = reader.GetString(i: 7),
                                    CreadoPor = reader.GetString(i: 8),
                                    FechaCreacion = reader.GetDateTime(i: 9),
                                    FechaModificacion = reader.IsDBNull(i: 10) ? null : reader.GetDateTime(i: 10),
                                    ModificadoPor = reader.IsDBNull(i: 11) ? null : reader.GetString(i: 11)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener las devoluciones: " + ex.Message);
                }
            }

            return devoluciones;
        }

        public Devolucion ObtenerDevolucionPorId(int id)
        {
            Devolucion devolucion = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetDevolucionById @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                devolucion = new Devolucion
                                {
                                    ID = reader.GetInt32(i: 0),
                                    FacturaID = reader.GetInt32(i: 1),
                                    DetalleFacturaID = reader.GetInt32(i: 2),
                                    ProductoID = reader.GetInt32(i: 3),
                                    Cantidad = reader.GetInt32(i: 4),
                                    Motivo = reader.GetString(i: 5),
                                    FechaDevolucion = reader.GetDateTime(i: 6),
                                    Estado = reader.GetString(i: 7),
                                    CreadoPor = reader.GetString(i: 8),
                                    FechaCreacion = reader.GetDateTime(i: 9),
                                    FechaModificacion = reader.IsDBNull(i: 10) ? null : reader.GetDateTime(i: 10),
                                    ModificadoPor = reader.IsDBNull(i: 11) ? null : reader.GetString(i: 11)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la devolución: " + ex.Message);
                }
            }

            return devolucion;
        }

        public void ActualizarDevolucion(Devolucion devolucion)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_UpdateDevolucion @ID, @FacturaID, @DetalleFacturaID, @ProductoID, @Cantidad, @Motivo, @Estado, @ModificadoPor";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", devolucion.ID);
                        cmd.Parameters.AddWithValue("@FacturaID", devolucion.FacturaID);
                        cmd.Parameters.AddWithValue("@DetalleFacturaID", devolucion.DetalleFacturaID);
                        cmd.Parameters.AddWithValue("@ProductoID", devolucion.ProductoID);
                        cmd.Parameters.AddWithValue("@Cantidad", devolucion.Cantidad);
                        cmd.Parameters.AddWithValue("@Motivo", devolucion.Motivo);
                        cmd.Parameters.AddWithValue("@Estado", devolucion.Estado);
                        cmd.Parameters.AddWithValue("@ModificadoPor", devolucion.ModificadoPor);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar la devolución: " + ex.Message);
                }
            }
        }

        public void EliminarDevolucion(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeleteDevolucion @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la devolución: " + ex.Message);
                }
            }
        }

        // Métodos para la tabla Proveedor
        public void AgregarProveedor(Proveedor proveedor)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_InsertProveedor @NombreEmpresa, @NombreContacto, @Telefono, @Email, @DireccionID, @Estado, @CreadoPor";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@NombreEmpresa", proveedor.NombreEmpresa);
                        cmd.Parameters.AddWithValue("@NombreContacto", proveedor.NombreContacto);
                        cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                        cmd.Parameters.AddWithValue("@Email", proveedor.Email);
                        cmd.Parameters.AddWithValue("@DireccionID", proveedor.DireccionID);
                        cmd.Parameters.AddWithValue("@Estado", proveedor.Estado);
                        cmd.Parameters.AddWithValue("@CreadoPor", proveedor.CreadoPor);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar el proveedor: " + ex.Message);
                }
            }
        }

        public List<Proveedor> ObtenerProveedores()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllProveedores";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                proveedores.Add(new Proveedor
                                {
                                    ID = reader.GetInt32(i: 0),
                                    NombreEmpresa = reader.GetString(i: 1),
                                    NombreContacto = reader.IsDBNull(i: 2) ? null : reader.GetString(i: 2),
                                    Telefono = reader.IsDBNull(i: 3) ? null : reader.GetString(i: 3),
                                    Email = reader.IsDBNull(i: 4) ? null : reader.GetString(i: 4),
                                    DireccionID = reader.GetInt32(i: 5),
                                    Estado = reader.GetString(i: 6),
                                    CreadoPor = reader.GetString(i: 7),
                                    FechaCreacion = reader.GetDateTime(i: 8),
                                    FechaModificacion = reader.IsDBNull(i: 9) ? null : reader.GetDateTime(i: 9),
                                    ModificadoPor = reader.IsDBNull(i: 10) ? null : reader.GetString(i: 10)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los proveedores: " + ex.Message);
                }
            }

            return proveedores;
        }

        public Proveedor ObtenerProveedorPorId(int id)
        {
            Proveedor proveedor = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetProveedorById @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                proveedor = new Proveedor
                                {
                                    ID = reader.GetInt32(i: 0),
                                    NombreEmpresa = reader.GetString(i: 1),
                                    NombreContacto = reader.IsDBNull(i: 2) ? null : reader.GetString(i: 2),
                                    Telefono = reader.IsDBNull(i: 3) ? null : reader.GetString(i: 3),
                                    Email = reader.IsDBNull(i: 4) ? null : reader.GetString(i: 4),
                                    DireccionID = reader.GetInt32(i: 5),
                                    Estado = reader.GetString(i: 6),
                                    CreadoPor = reader.GetString(i: 7),
                                    FechaCreacion = reader.GetDateTime(i: 8),
                                    FechaModificacion = reader.IsDBNull(i: 9) ? null : reader.GetDateTime(i: 9),
                                    ModificadoPor = reader.IsDBNull(i: 10) ? null : reader.GetString(i: 10)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el proveedor: " + ex.Message);
                }
            }

            return proveedor;
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_UpdateProveedor @ID, @NombreEmpresa, @NombreContacto, @Telefono, @Email, @DireccionID, @Estado, @ModificadoPor";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", proveedor.ID);
                        cmd.Parameters.AddWithValue("@NombreEmpresa", proveedor.NombreEmpresa);
                        cmd.Parameters.AddWithValue("@NombreContacto", proveedor.NombreContacto);
                        cmd.Parameters.AddWithValue("@Telefono", proveedor.Telefono);
                        cmd.Parameters.AddWithValue("@Email", proveedor.Email);
                        cmd.Parameters.AddWithValue("@DireccionID", proveedor.DireccionID);
                        cmd.Parameters.AddWithValue("@Estado", proveedor.Estado);
                        cmd.Parameters.AddWithValue("@ModificadoPor", proveedor.ModificadoPor);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar el proveedor: " + ex.Message);
                }
            }
        }

        public void EliminarProveedor(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_DeleteProveedor @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el proveedor: " + ex.Message);
                }
            }
        }

        public Usuario ObtenerUsuarioPorNombreUsuario(string? nombreUsuario)
        {
            Usuario usuario = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetUsuarioByNombreUsuario @NombreUsuario";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    ID = reader.GetInt32(i: 0),
                                    NombreUsuario = reader.GetString(i: 1),
                                    ContrasenaHash = reader.GetString(i: 2),
                                    Email = reader.GetString(i: 3),
                                    Rol = reader.GetString(i: 4),
                                    Estado = reader.GetString(i: 5),
                                    FechaCreacion = reader.GetDateTime(i: 6),
                                    FechaModificacion = reader.IsDBNull(i: 7) ? null : reader.GetDateTime(i: 7)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el usuario: " + ex.Message);
                }
            }

            return usuario;
        }

        // Métodos para la tabla Usuario
        public void AgregarUsuario(Usuario usuario)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query =
                        "Exec sp_InsertUsuario @NombreUsuario, @ContrasenaHash, @Email, @Rol, @Estado, @NewID OUTPUT";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                        cmd.Parameters.AddWithValue("@ContrasenaHash", usuario.ContrasenaHash);
                        cmd.Parameters.AddWithValue("@Email", usuario.Email);
                        cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                        cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
                        cmd.Parameters.Add("@NewID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        usuario.ID = (int)cmd.Parameters["@NewID"].Value;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar el usuario: " + ex.Message);
                }
            }
        }

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetAllUsuarios";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usuarios.Add(new Usuario
                                {
                                    ID = reader.GetInt32(i: 0),
                                    NombreUsuario = reader.GetString(i: 1),
                                    ContrasenaHash = reader.GetString(i: 2),
                                    Email = reader.GetString(i: 3),
                                    Rol = reader.GetString(i: 4),
                                    Estado = reader.GetString(i: 5),
                                    FechaCreacion = reader.GetDateTime(i: 6),
                                    FechaModificacion = reader.IsDBNull(i: 7) ? null : reader.GetDateTime(i: 7)
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los usuarios: " + ex.Message);
                }
            }

            return usuarios;
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            Usuario usuario = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetUsuarioById @ID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    ID = reader.GetInt32(i: 0),
                                    NombreUsuario = reader.GetString(i: 1),
                                    ContrasenaHash = reader.GetString(i: 2),
                                    Email = reader.GetString(i: 3),
                                    Rol = reader.GetString(i: 4),
                                    Estado = reader.GetString(i: 5),
                                    FechaCreacion = reader.GetDateTime(i: 6),
                                    FechaModificacion = reader.IsDBNull(i: 7) ? null : reader.GetDateTime(i: 7)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el usuario: " + ex.Message);
                }
            }

            return usuario;
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_UpdateUsuario @ID, @NombreUsuario, @ContrasenaHash, @Email, @Rol, @Estado";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", usuario.ID);
                        cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                        cmd.Parameters.AddWithValue("@ContrasenaHash", usuario.ContrasenaHash);
                        cmd.Parameters.AddWithValue("@Email", usuario.Email);
                        cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                        cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar el usuario: " + ex.Message);
                }
            }
        }

        public Persona ObtenerPersonaPorCedula(string documentoIdentidad)
        {
            Persona persona = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec BuscarPersonaPorCedula @DocumentoIdentidad";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@DocumentoIdentidad", documentoIdentidad);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                persona = new Persona
                                {
                                    ID = reader.GetInt32(i: 0),
                                    Nombre1 = reader.GetString(i: 1),
                                    Nombre2 = reader.IsDBNull(i: 2) ? null : reader.GetString(i: 2),
                                    Apellido1 = reader.GetString(i: 3),
                                    Apellido2 = reader.GetString(i: 4),
                                    DocumentoIdentidad = reader.GetString(i: 5),
                                    Telefono = reader.IsDBNull(i: 6) ? null : reader.GetString(i: 6),
                                    Email = reader.IsDBNull(i: 7) ? null : reader.GetString(i: 7),
                                    FechaNacimiento = reader.IsDBNull(i: 8) ? null : reader.GetDateTime(i: 8),
                                    Genero = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9),
                                    DireccionID = reader.GetInt32(i: 10),
                                    CreadoPor = reader.GetString(i: 11),
                                    FechaModificacion = reader.IsDBNull(i: 12) ? null : reader.GetDateTime(i: 12),
                                    ModificadoPor = reader.IsDBNull(i: 13) ? null : reader.GetString(i: 13)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la persona por cédula: " + ex.Message);
                }
            }

            return persona;
        }

        public Usuario ObtenerUsuarioPorEmail(string email)
        {
            Usuario usuario = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "EXEC sp_ObtenerUsuarioPorEmail @Email";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    ID = reader.GetInt32(i: 0),
                                    NombreUsuario = reader.GetString(i: 1),
                                    ContrasenaHash = reader.GetString(i: 2),
                                    Email = reader.GetString(i: 3),
                                    Rol = reader.GetString(i: 4),
                                    Estado = reader.GetString(i: 5),
                                    FechaCreacion = reader.GetDateTime(i: 6),
                                    FechaModificacion = reader.IsDBNull(i: 7) ? null : reader.GetDateTime(i: 7)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el usuario por email: " + ex.Message);
                }
            }

            return usuario;
        }

        public Usuario ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            Usuario usuario = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "EXEC sp_ObtenerUsuarioPorNombreUsuario @NombreUsuario";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new Usuario
                                {
                                    ID = reader.GetInt32(i: 0),
                                    NombreUsuario = reader.GetString(i: 1),
                                    ContrasenaHash = reader.GetString(i: 2),
                                    Email = reader.GetString(i: 3),
                                    Rol = reader.GetString(i: 4),
                                    Estado = reader.GetString(i: 5),
                                    FechaCreacion = reader.GetDateTime(i: 6),
                                    FechaModificacion = reader.IsDBNull(i: 7) ? null : reader.GetDateTime(i: 7)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el usuario por nombre de usuario: " + ex.Message);
                }
            }

            return usuario;
        }

        public Persona ObtenerPersonaPorTelefono(string telefono)
        {
            Persona persona = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_BuscarPersonaPorTelefono @Telefono";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Telefono", telefono);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                persona = new Persona
                                {
                                    ID = reader.GetInt32(i: 0),
                                    Nombre1 = reader.GetString(i: 1),
                                    Nombre2 = reader.IsDBNull(i: 2) ? null : reader.GetString(i: 2),
                                    Apellido1 = reader.GetString(i: 3),
                                    Apellido2 = reader.GetString(i: 4),
                                    DocumentoIdentidad = reader.GetString(i: 5),
                                    Telefono = reader.IsDBNull(i: 6) ? null : reader.GetString(i: 6),
                                    Email = reader.IsDBNull(i: 7) ? null : reader.GetString(i: 7),
                                    FechaNacimiento = reader.IsDBNull(i: 8) ? null : reader.GetDateTime(i: 8),
                                    Genero = reader.IsDBNull(i: 9) ? null : reader.GetString(i: 9),
                                    FechaCreacion = reader.GetDateTime(i: 10),
                                    CreadoPor = reader.GetString(i: 11),
                                    FechaModificacion = reader.IsDBNull(i: 12) ? null : reader.GetDateTime(i: 12),
                                    ModificadoPor = reader.IsDBNull(i: 13) ? null : reader.GetString(i: 13),
                                    DireccionID = reader.GetInt32(i: 15)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la persona por teléfono: " + ex.Message);
                }
            }

            return persona;
        }

        public List<Producto> ObtenerTodosLosProductos()
        {
            List<Producto> listaProductos = new List<Producto>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_GetTodosLosProductos";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                listaProductos.Add(new Producto
                                {
                                    ID = Convert.ToInt32(dr["ID"]),
                                    NombreProducto = dr["NombreProducto"].ToString(),
                                    Genero = dr["Genero"].ToString(),
                                    SegmentoEdad = dr["SegmentoEdad"].ToString(),
                                    TipoProducto = dr["TipoProducto"].ToString(),
                                    Color = dr["Color"].ToString(),
                                    Talla = dr["Talla"].ToString(),
                                    UnidadesDisponibles = Convert.ToInt32(dr["UnidadesDisponibles"]),
                                    Precio = Convert.ToDecimal(dr["Precio"]),
                                    Descripcion = dr["Descripcion"] == DBNull.Value
                                        ? null
                                        : dr["Descripcion"].ToString(),
                                    ImagenUrl = dr["ImagenUrl"] == DBNull.Value
                                        ? null
                                        : dr["ImagenUrl"].ToString(), // 🔹 Agregar esta línea
                                    FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                    FechaModificacion = dr["FechaModificacion"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(dr["FechaModificacion"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los productos: " + ex.Message);
                }
            }

            return listaProductos;
        }

// Método para obtener una persona por su email
        public Persona ObtenerPersonaPorEmail(string email)
        {
            Persona persona = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec ObtenerPersonaPorEmail @Email";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                persona = new Persona
                                {
                                    ID = reader.GetInt32(0),
                                    Nombre1 = reader.GetString(1),
                                    Nombre2 = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    Apellido1 = reader.GetString(3),
                                    Apellido2 = reader.GetString(4),
                                    DocumentoIdentidad = reader.GetString(5),
                                    Telefono = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Email = reader.GetString(7),
                                    FechaNacimiento = reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8),
                                    Genero = reader.IsDBNull(9) ? null : reader.GetString(9),

                                    DireccionID = reader.GetInt32(14)
                                };
                            }
                            else
                            {
                                throw new Exception("No se encontró una persona con ese correo electrónico.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener la persona por email: " + ex.Message);
                }
            }

            return persona;
        }




        public void ActualizarCantidadCarrito(int clienteID, int productoID, int nuevaCantidad)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "EXEC sp_ActualizarCantidadCarrito @ClienteID, @ProductoID, @NuevaCantidad";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                        cmd.Parameters.AddWithValue("@ProductoID", productoID);
                        cmd.Parameters.AddWithValue("@NuevaCantidad", nuevaCantidad);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar la cantidad en el carrito: " + ex.Message);
                }
            }
        }


        public void AgregarAlCarrito(int clienteID, int productoID, int cantidad)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "EXEC sp_AgregarAlCarrito @ClienteID, @ProductoID, @Cantidad";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                        cmd.Parameters.AddWithValue("@ProductoID", productoID);
                        cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar al carrito: " + ex.Message);
                }
            }
        }
      
        public List<CarritoItem> ObtenerCarritoPorClienteID(int clienteID)
        {
            List<CarritoItem> carrito = new List<CarritoItem>();

            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "EXEC sp_ObtenerCarritoPorClienteID @ClienteID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carrito.Add(new CarritoItem
                                {
                                    ProductoID = reader.GetInt32(2),
                                    NombreProducto = reader.GetString(3),
                                    Precio = reader.GetDecimal(9),
                                    ImagenUrl = reader.IsDBNull(10) ? null : reader.GetString(10),
                                    Descripcion = reader.IsDBNull(11) ? null : reader.GetString(11),
                                    Cantidad = reader.GetInt32(12),
                                  
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el carrito: " + ex.Message);
                }
            }

            return carrito;
        }
       
        

        public Cliente ObtenerClientePorPersonaID(int personaID)
        {
            Cliente cliente = null;
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "Exec sp_ObtenerClientePorPersonaID @PersonaID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PersonaID", personaID);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cliente = new Cliente
                                {
                                    ID = reader.GetInt32(0),
                                    PersonaID = reader.GetInt32(1),
                                    CodigoCliente = reader.GetInt32(2),
                                    Estado = reader.GetString(3),
                                    CreadoPor = reader.GetString(4),
                                    FechaCreacion = reader.GetDateTime(5),
                                    FechaModificacion = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                    ModificadoPor = reader.IsDBNull(7) ? null : reader.GetString(7)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el cliente por PersonaID: " + ex.Message);
                }
            }

            return cliente;
        }

       

        public void EliminarProductoCarrito(int clienteID, int productoID)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "EXEC sp_EliminarProductoCarrito @ClienteID, @ProductoID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                        cmd.Parameters.AddWithValue("@ProductoID", productoID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el producto del carrito: " + ex.Message);
                }
            }
        }

        public void EliminarCarritoPorCliente(int clienteID)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "EXEC sp_EliminarCarritoPorCliente @ClienteID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                        con.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el carrito: " + ex.Message);
                }
            }
        }

        public List<DetalleFactura> ObtenerDetallesFacturaPorFacturaID(int facturaId)
        {
            var detallesFactura = new List<DetalleFactura>();

            using (SqlConnection con = new SqlConnection(_conexion))
            {
                try
                {
                    string query = "EXEC ObtenerDetallesFacturaPorFacturaID @FacturaID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FacturaID", facturaId);
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var detalle = new DetalleFactura
                                {
                                    // Si alguna columna es string, conviértela a int
                                    ID = int.Parse(reader["ID"].ToString()), // Convertir a int
                                    FacturaID = int.Parse(reader["FacturaID"].ToString()), // Convertir a int
                                    ProductoID = int.Parse(reader["ProductoID"].ToString()), // Convertir a int
                                    Cantidad = int.Parse(reader["Cantidad"].ToString()), // Convertir a int
                                    PrecioUnitario = reader.GetDecimal(4), // Asegúrate de que sea decimal
                                    Subtotal = reader.GetDecimal(5) // Asegúrate de que sea decimal
                                };

                                detallesFactura.Add(detalle);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los detalles de la factura: " + ex.Message);
                }
            }

            return detallesFactura;
        }


 public RegistroViewModel ObtenerPerfilUsuario(int usuarioId)
{
    RegistroViewModel perfil = new RegistroViewModel();

    using (SqlConnection con = new SqlConnection(_conexion))
    {
        using (SqlCommand cmd = new SqlCommand("sp_ObtenerPerfilUsuario", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UsuarioID", usuarioId);

            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    perfil.Usuario = new Usuario
                    {
                        ID = Convert.ToInt32(reader["UsuarioID"]),
                        NombreUsuario = reader["NombreUsuario"].ToString(),
                        ContrasenaHash = reader["ContrasenaHash"].ToString(),
                        Email = reader["UsuarioEmail"].ToString(), // ✅ Corregido
                        Rol = reader["Rol"].ToString(),
                        Estado = reader["Estado"].ToString()
                    };

                    perfil.Persona = new Persona
                    {
                        ID = Convert.ToInt32(reader["PersonaID"]),
                        Nombre1 = reader["Nombre1"].ToString(),
                        Nombre2 = reader["Nombre2"].ToString(),
                        Apellido1 = reader["Apellido1"].ToString(),
                        Apellido2 = reader["Apellido2"].ToString(),
                        Telefono = reader["Telefono"].ToString(),
                        Email = reader["PersonaEmail"].ToString(), // ✅ Corregido
                        Genero = reader["Genero"].ToString(),
                        DireccionID = Convert.ToInt32(reader["DireccionID"])
                    };
                    perfil.Cliente = new Cliente
                    {
                        ID = Convert.ToInt32(reader["ClienteID"]),
                        PersonaID = Convert.ToInt32(reader["PersonaID"])
                    };

                    perfil.Direccion = new Direccion
                    {
                        ID = Convert.ToInt32(reader["DireccionID"]),
                        Ciudad = reader["Ciudad"].ToString(),
                        Estado = reader["EstadoDireccion"].ToString(),
                        CodigoPostal = reader["CodigoPostal"].ToString(),
                        Pais = reader["Pais"].ToString(),
                        TipoDireccion = reader["TipoDireccion"].ToString()
                    };
                }
            }
        }
    }

    return perfil;
}
public bool ActualizarPerfilUsuario(RegistroViewModel perfil)
{
    using (SqlConnection con = new SqlConnection(_conexion))
    using (SqlCommand cmd = new SqlCommand("sp_ActualizarPerfilUsuario", con))
    {
        cmd.CommandType = CommandType.StoredProcedure;
        
        // Parámetros de Usuario
        cmd.Parameters.AddWithValue("@UsuarioID", perfil.Usuario.ID);
        cmd.Parameters.AddWithValue("@NombreUsuario", perfil.Usuario.NombreUsuario);
        cmd.Parameters.AddWithValue("@Email", perfil.Usuario.Email); // Para referencia, no se actualiza
        
        // Parámetros de Persona
        cmd.Parameters.AddWithValue("@PersonaID", perfil.Persona.ID);
        cmd.Parameters.AddWithValue("@Nombre1", perfil.Persona.Nombre1);
        cmd.Parameters.AddWithValue("@Nombre2", perfil.Persona.Nombre2 ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Apellido1", perfil.Persona.Apellido1);
        cmd.Parameters.AddWithValue("@Apellido2", perfil.Persona.Apellido2 ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Telefono", perfil.Persona.Telefono ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Genero", perfil.Persona.Genero ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@PersonaEmail", perfil.Persona.Email ?? (object)DBNull.Value);
        
        // Parámetros de Dirección
        cmd.Parameters.AddWithValue("@DireccionID", perfil.Direccion.ID);
        cmd.Parameters.AddWithValue("@Ciudad", perfil.Direccion.Ciudad ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@EstadoDireccion", perfil.Direccion.Estado ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@CodigoPostal", perfil.Direccion.CodigoPostal ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Pais", perfil.Direccion.Pais ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@TipoDireccion", perfil.Direccion.TipoDireccion ?? (object)DBNull.Value);
        
        // Parámetro de retorno
        SqlParameter returnValue = new SqlParameter("@ReturnValue", SqlDbType.Int);
        returnValue.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(returnValue);
        
        con.Open();
        cmd.ExecuteNonQuery();
        
        return (int)returnValue.Value == 1;
    }
}
public bool EliminarPerfilUsuario(int usuarioId)
{
    using (SqlConnection con = new SqlConnection(_conexion))
    using (SqlCommand cmd = new SqlCommand("sp_EliminarPerfilUsuario", con))
    {
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@UsuarioID", usuarioId);
        
        // Parámetro de retorno
        SqlParameter returnValue = new SqlParameter("@ReturnValue", SqlDbType.Int);
        returnValue.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(returnValue);
        
        con.Open();
        cmd.ExecuteNonQuery();
        
        return (int)returnValue.Value == 1;
    }
}

    }
}