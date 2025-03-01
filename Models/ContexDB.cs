using System.Data.SqlClient;
using System.Data;
using K_F_ClothingStore.Models;

namespace K_F_ClothingStore.Models
{
    public class ContexDB
    {
        private readonly string _conexion;

        public ContexDB(IConfiguration configuracion)
        {
            _conexion = configuracion.GetConnectionString("Conexion");
        }

        // ================================================
        // DIRECCIÓN
        // ================================================
        public void AgregarDireccion(Direccion direccion)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertDireccion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
                    cmd.Parameters.AddWithValue("@Estado", direccion.Estado);
                    cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);
                    cmd.Parameters.AddWithValue("@Pais", direccion.Pais);
                    cmd.Parameters.AddWithValue("@TipoDireccion", (object)direccion.TipoDireccion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreadoPor", direccion.CreadoPor);

                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(newId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    direccion.ID = (int)newId.Value;
                }
            }
        }

        public List<Direccion> ObtenerTodasDirecciones()
        {
            List<Direccion> direcciones = new List<Direccion>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllDirecciones", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            direcciones.Add(new Direccion
                            {
                                ID = reader.GetInt32(0),
                                Ciudad = reader.GetString(1),
                                Estado = reader.GetString(2),
                                CodigoPostal = reader.GetString(3),
                                Pais = reader.GetString(4),
                                TipoDireccion = reader.IsDBNull(5) ? null : reader.GetString(5),
                                FechaCreacion = reader.GetDateTime(6),
                                CreadoPor = reader.GetString(7),
                                FechaModificacion = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8),
                                ModificadoPor = reader.IsDBNull(9) ? null : reader.GetString(9)
                            });
                        }
                    }
                }
            }
            return direcciones;
        }

        public Direccion ObtenerDireccionPorId(int id)
        {
            Direccion direccion = new Direccion();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDireccionById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            direccion.ID = reader.GetInt32(0);
                            direccion.Ciudad = reader.GetString(1);
                            direccion.Estado = reader.GetString(2);
                            direccion.CodigoPostal = reader.GetString(3);
                            direccion.Pais = reader.GetString(4);
                            direccion.TipoDireccion = reader.IsDBNull(5) ? null : reader.GetString(5);
                            direccion.FechaCreacion = reader.GetDateTime(6);
                            direccion.CreadoPor = reader.GetString(7);
                            direccion.FechaModificacion = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8);
                            direccion.ModificadoPor = reader.IsDBNull(9) ? null : reader.GetString(9);
                        }
                    }
                }
            }
            return direccion;
        }

        public void ActualizarDireccion(Direccion direccion)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateDireccion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", direccion.ID);
                    cmd.Parameters.AddWithValue("@Ciudad", direccion.Ciudad);
                    cmd.Parameters.AddWithValue("@Estado", direccion.Estado);
                    cmd.Parameters.AddWithValue("@CodigoPostal", direccion.CodigoPostal);
                    cmd.Parameters.AddWithValue("@Pais", direccion.Pais);
                    cmd.Parameters.AddWithValue("@TipoDireccion", (object)direccion.TipoDireccion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModificadoPor", direccion.ModificadoPor);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarDireccion(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteDireccion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // PERSONA
        // ================================================
        public void AgregarPersona(Persona persona)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertPersona", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre1", persona.Nombre1);
                    cmd.Parameters.AddWithValue("@Nombre2", (object)persona.Nombre2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Apellido1", persona.Apellido1);
                    cmd.Parameters.AddWithValue("@Apellido2", persona.Apellido2);
                    cmd.Parameters.AddWithValue("@DocumentoIdentidad", persona.DocumentoIdentidad);
                    cmd.Parameters.AddWithValue("@Telefono", (object)persona.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)persona.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", (object)persona.FechaNacimiento ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Genero", (object)persona.Genero ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionID", persona.DireccionID);
                    cmd.Parameters.AddWithValue("@CreadoPor", persona.CreadoPor);

                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(newId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    persona.ID = (int)newId.Value;
                }
            }
        }

        public List<Persona> ObtenerTodasPersonas()
        {
            List<Persona> personas = new List<Persona>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllPersonas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            personas.Add(new Persona
                            {
                                ID = reader.GetInt32(0),
                                Nombre1 = reader.GetString(1),
                                Nombre2 = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Apellido1 = reader.GetString(3),
                                Apellido2 = reader.GetString(4),
                                DocumentoIdentidad = reader.GetString(5),
                                Telefono = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Email = reader.IsDBNull(7) ? null : reader.GetString(7),
                                FechaNacimiento = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8),
                                Genero = reader.IsDBNull(9) ? null : reader.GetString(9),
                                DireccionID = reader.GetInt32(10),
                                FechaCreacion = reader.GetDateTime(11),
                                CreadoPor = reader.GetString(12),
                                FechaModificacion = reader.IsDBNull(13) ? null : (DateTime?)reader.GetDateTime(13),
                                ModificadoPor = reader.IsDBNull(14) ? null : reader.GetString(14)
                            });
                        }
                    }
                }
            }
            return personas;
        }

        public Persona ObtenerPersonaPorId(int id)
        {
            Persona persona = new Persona();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetPersonaById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            persona.ID = reader.GetInt32(0);
                            persona.Nombre1 = reader.GetString(1);
                            persona.Nombre2 = reader.IsDBNull(2) ? null : reader.GetString(2);
                            persona.Apellido1 = reader.GetString(3);
                            persona.Apellido2 = reader.GetString(4);
                            persona.DocumentoIdentidad = reader.GetString(5);
                            persona.Telefono = reader.IsDBNull(6) ? null : reader.GetString(6);
                            persona.Email = reader.IsDBNull(7) ? null : reader.GetString(7);
                            persona.FechaNacimiento = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8);
                            persona.Genero = reader.IsDBNull(9) ? null : reader.GetString(9);
                            persona.DireccionID = reader.GetInt32(10);
                            persona.FechaCreacion = reader.GetDateTime(11);
                            persona.CreadoPor = reader.GetString(12);
                            persona.FechaModificacion = reader.IsDBNull(13) ? null : (DateTime?)reader.GetDateTime(13);
                            persona.ModificadoPor = reader.IsDBNull(14) ? null : reader.GetString(14);
                        }
                    }
                }
            }
            return persona;
        }

        public void ActualizarPersona(Persona persona)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdatePersona", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", persona.ID);
                    cmd.Parameters.AddWithValue("@Nombre1", persona.Nombre1);
                    cmd.Parameters.AddWithValue("@Nombre2", (object)persona.Nombre2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Apellido1", persona.Apellido1);
                    cmd.Parameters.AddWithValue("@Apellido2", persona.Apellido2);
                    cmd.Parameters.AddWithValue("@Telefono", (object)persona.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)persona.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", (object)persona.FechaNacimiento ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Genero", (object)persona.Genero ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionID", persona.DireccionID);
                    cmd.Parameters.AddWithValue("@ModificadoPor", persona.ModificadoPor);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarPersona(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeletePersona", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // CLIENTE
        // ================================================
        public void AgregarCliente(Cliente cliente)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertCliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonaID", cliente.PersonaID);
                    cmd.Parameters.AddWithValue("@CodigoCliente", cliente.CodigoCliente);
                    cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                    cmd.Parameters.AddWithValue("@CreadoPor", cliente.CreadoPor);

                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(newId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    cliente.ID = (int)newId.Value;
                }
            }
        }

        public List<Cliente> ObtenerTodosClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllClientes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new Cliente
                            {
                                ID = reader.GetInt32(0),
                                PersonaID = reader.GetInt32(1),
                                CodigoCliente = reader.GetInt32(2),
                                Estado = reader.GetString(3),
                                FechaCreacion = reader.GetDateTime(4),
                                CreadoPor = reader.GetString(5),
                                FechaModificacion = reader.IsDBNull(6) ? null : (DateTime?)reader.GetDateTime(6),
                                ModificadoPor = reader.IsDBNull(7) ? null : reader.GetString(7)
                            });
                        }
                    }
                }
            }
            return clientes;
        }

        public Cliente ObtenerClientePorId(int id)
        {
            Cliente cliente = new Cliente();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetClienteById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente.ID = reader.GetInt32(0);
                            cliente.PersonaID = reader.GetInt32(1);
                            cliente.CodigoCliente = reader.GetInt32(2);
                            cliente.Estado = reader.GetString(3);
                            cliente.FechaCreacion = reader.GetDateTime(4);
                            cliente.CreadoPor = reader.GetString(5);
                            cliente.FechaModificacion = reader.IsDBNull(6) ? null : (DateTime?)reader.GetDateTime(6);
                            cliente.ModificadoPor = reader.IsDBNull(7) ? null : reader.GetString(7);
                        }
                    }
                }
            }
            return cliente;
        }

        public void ActualizarCliente(Cliente cliente)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateCliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", cliente.ID);
                    cmd.Parameters.AddWithValue("@CodigoCliente", cliente.CodigoCliente);
                    cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                    cmd.Parameters.AddWithValue("@ModificadoPor", cliente.ModificadoPor);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarCliente(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteCliente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // EMPLEADO
        // ================================================
        public void AgregarEmpleado(Empleado empleado)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertEmpleado", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PersonaID", empleado.PersonaID);
                    cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                    cmd.Parameters.AddWithValue("@FechaContratacion", empleado.FechaContratacion);
                    cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                    cmd.Parameters.AddWithValue("@Estado", empleado.Estado);
                    cmd.Parameters.AddWithValue("@CreadoPor", empleado.CreadoPor);

                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(newId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    empleado.ID = (int)newId.Value;
                }
            }
        }

        public List<Empleado> ObtenerTodosEmpleados()
        {
            List<Empleado> empleados = new List<Empleado>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllEmpleados", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empleados.Add(new Empleado
                            {
                                ID = reader.GetInt32(0),
                                PersonaID = reader.GetInt32(1),
                                Puesto = reader.GetString(2),
                                FechaContratacion = reader.GetDateTime(3),
                                Salario = reader.GetDecimal(4),
                                Estado = reader.GetString(5),
                                FechaCreacion = reader.GetDateTime(6),
                                CreadoPor = reader.GetString(7),
                                FechaModificacion = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8),
                                ModificadoPor = reader.IsDBNull(9) ? null : reader.GetString(9)
                            });
                        }
                    }
                }
            }
            return empleados;
        }

        public Empleado ObtenerEmpleadoPorId(int id)
        {
            Empleado empleado = new Empleado();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetEmpleadoById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            empleado.ID = reader.GetInt32(0);
                            empleado.PersonaID = reader.GetInt32(1);
                            empleado.Puesto = reader.GetString(2);
                            empleado.FechaContratacion = reader.GetDateTime(3);
                            empleado.Salario = reader.GetDecimal(4);
                            empleado.Estado = reader.GetString(5);
                            empleado.FechaCreacion = reader.GetDateTime(6);
                            empleado.CreadoPor = reader.GetString(7);
                            empleado.FechaModificacion = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8);
                            empleado.ModificadoPor = reader.IsDBNull(9) ? null : reader.GetString(9);
                        }
                    }
                }
            }
            return empleado;
        }

        public void ActualizarEmpleado(Empleado empleado)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateEmpleado", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
        }

        public void EliminarEmpleado(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteEmpleado", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // PRODUCTO
        // ================================================
        public void AgregarProducto(Producto producto)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertProducto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Genero", producto.Genero);
                    cmd.Parameters.AddWithValue("@SegmentoEdad", producto.SegmentoEdad);
                    cmd.Parameters.AddWithValue("@TipoProducto", producto.TipoProducto);
                    cmd.Parameters.AddWithValue("@Color", producto.Color);
                    cmd.Parameters.AddWithValue("@Talla", producto.Talla);
                    cmd.Parameters.AddWithValue("@UnidadesDisponibles", producto.UnidadesDisponibles);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Descripcion", (object)producto.Descripcion ?? DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Producto> ObtenerTodosProductos()
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllProductos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto
                            {
                                ID = reader.GetInt32(0),
                                NombreProducto = reader.GetString(1),
                                Genero = reader.GetString(2),
                                SegmentoEdad = reader.GetString(3),
                                TipoProducto = reader.GetString(4),
                                Color = reader.GetString(5),
                                Talla = reader.GetString(6),
                                UnidadesDisponibles = reader.GetInt32(7),
                                Precio = reader.GetDecimal(8),
                                Descripcion = reader.IsDBNull(9) ? null : reader.GetString(9),
                                FechaCreacion = reader.GetDateTime(10),
                                FechaModificacion = reader.IsDBNull(11) ? null : (DateTime?)reader.GetDateTime(11)
                            });
                        }
                    }
                }
            }
            return productos;
        }

        public Producto ObtenerProductoPorId(int id)
        {
            Producto producto = new Producto();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetProductoById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto.ID = reader.GetInt32(0);
                            producto.NombreProducto = reader.GetString(1);
                            producto.Genero = reader.GetString(2);
                            producto.SegmentoEdad = reader.GetString(3);
                            producto.TipoProducto = reader.GetString(4);
                            producto.Color = reader.GetString(5);
                            producto.Talla = reader.GetString(6);
                            producto.UnidadesDisponibles = reader.GetInt32(7);
                            producto.Precio = reader.GetDecimal(8);
                            producto.Descripcion = reader.IsDBNull(9) ? null : reader.GetString(9);
                            producto.FechaCreacion = reader.GetDateTime(10);
                            producto.FechaModificacion = reader.IsDBNull(11) ? null : (DateTime?)reader.GetDateTime(11);
                        }
                    }
                }
            }
            return producto;
        }

        public void ActualizarProducto(Producto producto)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateProducto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", producto.ID);
                    cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                    cmd.Parameters.AddWithValue("@Genero", producto.Genero);
                    cmd.Parameters.AddWithValue("@SegmentoEdad", producto.SegmentoEdad);
                    cmd.Parameters.AddWithValue("@TipoProducto", producto.TipoProducto);
                    cmd.Parameters.AddWithValue("@Color", producto.Color);
                    cmd.Parameters.AddWithValue("@Talla", producto.Talla);
                    cmd.Parameters.AddWithValue("@UnidadesDisponibles", producto.UnidadesDisponibles);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Descripcion", (object)producto.Descripcion ?? DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarProducto(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteProducto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // VENTA
        // ================================================
        public void AgregarVenta(Venta venta)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpleadoID", venta.EmpleadoID);
                    cmd.Parameters.AddWithValue("@ClienteID", venta.ClienteID);
                    cmd.Parameters.AddWithValue("@ProductoID", venta.ProductoID);
                    cmd.Parameters.AddWithValue("@Cantidad", venta.Cantidad);
                    cmd.Parameters.AddWithValue("@Total", venta.Total);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Venta> ObtenerTodasVentas()
        {
            List<Venta> ventas = new List<Venta>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllVentas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ventas.Add(new Venta
                            {
                                VentaID = reader.GetInt32(0),
                                EmpleadoID = reader.GetInt32(1),
                                ClienteID = reader.GetInt32(2),
                                ProductoID = reader.GetInt32(3),
                                Cantidad = reader.GetInt32(4),
                                Total = reader.GetDecimal(5),
                                Fecha = reader.GetDateTime(6)
                            });
                        }
                    }
                }
            }
            return ventas;
        }

        public Venta ObtenerVentaPorId(int id)
        {
            Venta venta = new Venta();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetVentaById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VentaID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            venta.VentaID = reader.GetInt32(0);
                            venta.EmpleadoID = reader.GetInt32(1);
                            venta.ClienteID = reader.GetInt32(2);
                            venta.ProductoID = reader.GetInt32(3);
                            venta.Cantidad = reader.GetInt32(4);
                            venta.Total = reader.GetDecimal(5);
                            venta.Fecha = reader.GetDateTime(6);
                        }
                    }
                }
            }
            return venta;
        }

        public void ActualizarVenta(Venta venta)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
        }

        public void EliminarVenta(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteVenta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VentaID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // DESCUENTO
        // ================================================
        public void AgregarDescuento(Descuento descuento)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertDescuento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tipo", descuento.Tipo);
                    cmd.Parameters.AddWithValue("@Valor", descuento.Valor);
                    cmd.Parameters.AddWithValue("@Descripcion", (object)descuento.Descripcion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaInicio", descuento.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", (object)descuento.FechaFin ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", descuento.Estado);

                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(newId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    descuento.ID = (int)newId.Value;
                }
            }
        }

        public List<Descuento> ObtenerTodosDescuentos()
        {
            List<Descuento> descuentos = new List<Descuento>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllDescuentos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            descuentos.Add(new Descuento
                            {
                                ID = reader.GetInt32(0),
                                Tipo = reader.GetString(1),
                                Valor = reader.GetDecimal(2),
                                Descripcion = reader.IsDBNull(3) ? null : reader.GetString(3),
                                FechaInicio = reader.GetDateTime(4),
                                FechaFin = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5),
                                Estado = reader.GetString(6)
                            });
                        }
                    }
                }
            }
            return descuentos;
        }

        public Descuento ObtenerDescuentoPorId(int id)
        {
            Descuento descuento = new Descuento();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDescuentoById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            descuento.ID = reader.GetInt32(0);
                            descuento.Tipo = reader.GetString(1);
                            descuento.Valor = reader.GetDecimal(2);
                            descuento.Descripcion = reader.IsDBNull(3) ? null : reader.GetString(3);
                            descuento.FechaInicio = reader.GetDateTime(4);
                            descuento.FechaFin = reader.IsDBNull(5) ? null : (DateTime?)reader.GetDateTime(5);
                            descuento.Estado = reader.GetString(6);
                        }
                    }
                }
            }
            return descuento;
        }

        public void ActualizarDescuento(Descuento descuento)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateDescuento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", descuento.ID);
                    cmd.Parameters.AddWithValue("@Tipo", descuento.Tipo);
                    cmd.Parameters.AddWithValue("@Valor", descuento.Valor);
                    cmd.Parameters.AddWithValue("@Descripcion", (object)descuento.Descripcion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaInicio", descuento.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", (object)descuento.FechaFin ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", descuento.Estado);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarDescuento(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteDescuento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // FACTURA
        // ================================================
        public void AgregarFactura(Factura factura)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertFactura", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClienteID", factura.ClienteID);
                    cmd.Parameters.AddWithValue("@EmpleadoID", factura.EmpleadoID);
                    cmd.Parameters.AddWithValue("@Total", factura.Total);
                    cmd.Parameters.AddWithValue("@MetodoPago", factura.MetodoPago);
                    cmd.Parameters.AddWithValue("@Estado", factura.Estado);
                    cmd.Parameters.AddWithValue("@DescuentoID", (object)factura.DescuentoID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreadoPor", factura.CreadoPor);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Factura> ObtenerTodasFacturas()
        {
            List<Factura> facturas = new List<Factura>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllFacturas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            facturas.Add(new Factura
                            {
                                ID = reader.GetInt32(0),
                                ClienteID = reader.GetInt32(1),
                                EmpleadoID = reader.GetInt32(2),
                                FechaEmision = reader.GetDateTime(3),
                                Total = reader.GetDecimal(4),
                                MetodoPago = reader.GetString(5),
                                Estado = reader.GetString(6),
                                DescuentoID = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7),
                                FechaCreacion = reader.GetDateTime(8),
                                CreadoPor = reader.GetString(9),
                                FechaModificacion = reader.IsDBNull(10) ? null : (DateTime?)reader.GetDateTime(10),
                                ModificadoPor = reader.IsDBNull(11) ? null : reader.GetString(11)
                            });
                        }
                    }
                }
            }
            return facturas;
        }

        public Factura ObtenerFacturaPorId(int id)
        {
            Factura factura = new Factura();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetFacturaById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            factura.ID = reader.GetInt32(0);
                            factura.ClienteID = reader.GetInt32(1);
                            factura.EmpleadoID = reader.GetInt32(2);
                            factura.FechaEmision = reader.GetDateTime(3);
                            factura.Total = reader.GetDecimal(4);
                            factura.MetodoPago = reader.GetString(5);
                            factura.Estado = reader.GetString(6);
                            factura.DescuentoID = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7);
                            factura.FechaCreacion = reader.GetDateTime(8);
                            factura.CreadoPor = reader.GetString(9);
                            factura.FechaModificacion = reader.IsDBNull(10) ? null : (DateTime?)reader.GetDateTime(10);
                            factura.ModificadoPor = reader.IsDBNull(11) ? null : reader.GetString(11);
                        }
                    }
                }
            }
            return factura;
        }

        public void ActualizarFactura(Factura factura)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateFactura", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", factura.ID);
                    cmd.Parameters.AddWithValue("@ClienteID", factura.ClienteID);
                    cmd.Parameters.AddWithValue("@EmpleadoID", factura.EmpleadoID);
                    cmd.Parameters.AddWithValue("@Total", factura.Total);
                    cmd.Parameters.AddWithValue("@MetodoPago", factura.MetodoPago);
                    cmd.Parameters.AddWithValue("@Estado", factura.Estado);
                    cmd.Parameters.AddWithValue("@DescuentoID", (object)factura.DescuentoID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModificadoPor", factura.ModificadoPor);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarFactura(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteFactura", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // DETALLE FACTURA
        // ================================================
        public void AgregarDetalleFactura(DetalleFactura detalle)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertDetalleFactura", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacturaID", detalle.FacturaID);
                    cmd.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                    cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@Subtotal", detalle.Subtotal);

                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(newId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    detalle.ID = (int)newId.Value;
                }
            }
        }

        public List<DetalleFactura> ObtenerTodosDetallesFactura()
        {
            List<DetalleFactura> detalles = new List<DetalleFactura>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllDetallesFactura", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detalles.Add(new DetalleFactura
                            {
                                ID = reader.GetInt32(0),
                                FacturaID = reader.GetInt32(1),
                                ProductoID = reader.GetInt32(2),
                                Cantidad = reader.GetInt32(3),
                                PrecioUnitario = reader.GetDecimal(4),
                                Subtotal = reader.GetDecimal(5)
                            });
                        }
                    }
                }
            }
            return detalles;
        }

        public DetalleFactura ObtenerDetalleFacturaPorId(int id)
        {
            DetalleFactura detalle = new DetalleFactura();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDetalleFacturaById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            detalle.ID = reader.GetInt32(0);
                            detalle.FacturaID = reader.GetInt32(1);
                            detalle.ProductoID = reader.GetInt32(2);
                            detalle.Cantidad = reader.GetInt32(3);
                            detalle.PrecioUnitario = reader.GetDecimal(4);
                            detalle.Subtotal = reader.GetDecimal(5);
                        }
                    }
                }
            }
            return detalle;
        }

        public void ActualizarDetalleFactura(DetalleFactura detalle)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateDetalleFactura", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", detalle.ID);
                    cmd.Parameters.AddWithValue("@FacturaID", detalle.FacturaID);
                    cmd.Parameters.AddWithValue("@ProductoID", detalle.ProductoID);
                    cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                    cmd.Parameters.AddWithValue("@Subtotal", detalle.Subtotal);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarDetalleFactura(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteDetalleFactura", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // DEVOLUCIÓN
        // ================================================
        public void AgregarDevolucion(Devolucion devolucion)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertDevolucion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacturaID", devolucion.FacturaID);
                    cmd.Parameters.AddWithValue("@DetalleFacturaID", devolucion.DetalleFacturaID);
                    cmd.Parameters.AddWithValue("@ProductoID", devolucion.ProductoID);
                    cmd.Parameters.AddWithValue("@Cantidad", devolucion.Cantidad);
                    cmd.Parameters.AddWithValue("@Motivo", devolucion.Motivo);
                    cmd.Parameters.AddWithValue("@Estado", devolucion.Estado);
                    cmd.Parameters.AddWithValue("@CreadoPor", devolucion.CreadoPor);

                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(newId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    devolucion.ID = (int)newId.Value;
                }
            }
        }

        public List<Devolucion> ObtenerTodasDevoluciones()
        {
            List<Devolucion> devoluciones = new List<Devolucion>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllDevoluciones", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            devoluciones.Add(new Devolucion
                            {
                                ID = reader.GetInt32(0),
                                FacturaID = reader.GetInt32(1),
                                DetalleFacturaID = reader.GetInt32(2),
                                ProductoID = reader.GetInt32(3),
                                Cantidad = reader.GetInt32(4),
                                Motivo = reader.GetString(5),
                                FechaDevolucion = reader.GetDateTime(6),
                                Estado = reader.GetString(7),
                                CreadoPor = reader.GetString(8),
                                FechaCreacion = reader.GetDateTime(9),
                                FechaModificacion = reader.IsDBNull(10) ? null : (DateTime?)reader.GetDateTime(10),
                                ModificadoPor = reader.IsDBNull(11) ? null : reader.GetString(11)
                            });
                        }
                    }
                }
            }
            return devoluciones;
        }

        public Devolucion ObtenerDevolucionPorId(int id)
        {
            Devolucion devolucion = new Devolucion();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDevolucionById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            devolucion.ID = reader.GetInt32(0);
                            devolucion.FacturaID = reader.GetInt32(1);
                            devolucion.DetalleFacturaID = reader.GetInt32(2);
                            devolucion.ProductoID = reader.GetInt32(3);
                            devolucion.Cantidad = reader.GetInt32(4);
                            devolucion.Motivo = reader.GetString(5);
                            devolucion.FechaDevolucion = reader.GetDateTime(6);
                            devolucion.Estado = reader.GetString(7);
                            devolucion.CreadoPor = reader.GetString(8);
                            devolucion.FechaCreacion = reader.GetDateTime(9);
                            devolucion.FechaModificacion = reader.IsDBNull(10) ? null : (DateTime?)reader.GetDateTime(10);
                            devolucion.ModificadoPor = reader.IsDBNull(11) ? null : reader.GetString(11);
                        }
                    }
                }
            }
            return devolucion;
        }

        public void ActualizarDevolucion(Devolucion devolucion)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateDevolucion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
        }

        public void EliminarDevolucion(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteDevolucion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // PROVEEDOR
        // ================================================
        public void AgregarProveedor(Proveedor proveedor)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertProveedor", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreEmpresa", proveedor.NombreEmpresa);
                    cmd.Parameters.AddWithValue("@NombreContacto", (object)proveedor.NombreContacto ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Telefono", (object)proveedor.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)proveedor.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionID", proveedor.DireccionID);
                    cmd.Parameters.AddWithValue("@Estado", proveedor.Estado);
                    cmd.Parameters.AddWithValue("@CreadoPor", proveedor.CreadoPor);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Proveedor> ObtenerTodosProveedores()
        {
            List<Proveedor> proveedores = new List<Proveedor>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllProveedores", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proveedores.Add(new Proveedor
                            {
                                ID = reader.GetInt32(0),
                                NombreEmpresa = reader.GetString(1),
                                NombreContacto = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Telefono = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                                DireccionID = reader.GetInt32(5),
                                Estado = reader.GetString(6),
                                FechaCreacion = reader.GetDateTime(7),
                                FechaModificacion = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8),
                                CreadoPor = reader.GetString(9),
                                ModificadoPor = reader.IsDBNull(10) ? null : reader.GetString(10)
                            });
                        }
                    }
                }
            }
            return proveedores;
        }

        public Proveedor ObtenerProveedorPorId(int id)
        {
            Proveedor proveedor = new Proveedor();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetProveedorById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            proveedor.ID = reader.GetInt32(0);
                            proveedor.NombreEmpresa = reader.GetString(1);
                            proveedor.NombreContacto = reader.IsDBNull(2) ? null : reader.GetString(2);
                            proveedor.Telefono = reader.IsDBNull(3) ? null : reader.GetString(3);
                            proveedor.Email = reader.IsDBNull(4) ? null : reader.GetString(4);
                            proveedor.DireccionID = reader.GetInt32(5);
                            proveedor.Estado = reader.GetString(6);
                            proveedor.FechaCreacion = reader.GetDateTime(7);
                            proveedor.FechaModificacion = reader.IsDBNull(8) ? null : (DateTime?)reader.GetDateTime(8);
                            proveedor.CreadoPor = reader.GetString(9);
                            proveedor.ModificadoPor = reader.IsDBNull(10) ? null : reader.GetString(10);
                        }
                    }
                }
            }
            return proveedor;
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateProveedor", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", proveedor.ID);
                    cmd.Parameters.AddWithValue("@NombreEmpresa", proveedor.NombreEmpresa);
                    cmd.Parameters.AddWithValue("@NombreContacto", (object)proveedor.NombreContacto ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Telefono", (object)proveedor.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)proveedor.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DireccionID", proveedor.DireccionID);
                    cmd.Parameters.AddWithValue("@Estado", proveedor.Estado);
                    cmd.Parameters.AddWithValue("@ModificadoPor", proveedor.ModificadoPor);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarProveedor(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteProveedor", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ================================================
        // USUARIO
        // ================================================
        public void AgregarUsuario(Usuario usuario)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@ContrasenaHash", usuario.ContrasenaHash);
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Estado", usuario.Estado);

                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int);
                    newId.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(newId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    usuario.ID = (int)newId.Value;
                }
            }
        }

        public List<Usuario> ObtenerTodosUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllUsuarios", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new Usuario
                            {
                                ID = reader.GetInt32(0),
                                NombreUsuario = reader.GetString(1),
                                ContrasenaHash = reader.GetString(2),
                                Email = reader.GetString(3),
                                Rol = reader.GetString(4),
                                Estado = reader.GetString(5),
                                FechaCreacion = reader.GetDateTime(6),
                                FechaModificacion = reader.IsDBNull(7) ? null : (DateTime?)reader.GetDateTime(7)
                            });
                        }
                    }
                }
            }
            return usuarios;
        }

        public Usuario ObtenerUsuarioPorId(int id)
        {
            Usuario usuario = new Usuario();
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetUsuarioById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario.ID = reader.GetInt32(0);
                            usuario.NombreUsuario = reader.GetString(1);
                            usuario.ContrasenaHash = reader.GetString(2);
                            usuario.Email = reader.GetString(3);
                            usuario.Rol = reader.GetString(4);
                            usuario.Estado = reader.GetString(5);
                            usuario.FechaCreacion = reader.GetDateTime(6);
                            usuario.FechaModificacion = reader.IsDBNull(7) ? null : (DateTime?)reader.GetDateTime(7);
                        }
                    }
                }
            }
            return usuario;
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
        }

        public void EliminarUsuario(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}