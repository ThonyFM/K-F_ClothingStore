using System.Data;
using System.Data.SqlClient;

namespace K_F_ClothingStore.Models;

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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query =
                    "Exec sp_InsertDireccion @Ciudad, @Estado, @CodigoPostal, @Pais, @TipoDireccion, @CreadoPor, @NewID OUTPUT";
                using (var cmd = new SqlCommand(query, con))
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



    public Direccion ObtenerDireccionPorId(int id)
    {
        Direccion direccion = null;
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "Exec sp_GetDireccionById @ID";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            direccion = new Direccion
                            {
                                ID = reader.GetInt32(0),
                                Ciudad = reader.GetString(1),
                                Estado = reader.GetString(2),
                                CodigoPostal = reader.GetString(3),
                                Pais = reader.GetString(4),
                                TipoDireccion = reader.GetString(5),
                                FechaCreacion = reader.GetDateTime(6),
                                CreadoPor = reader.GetString(7),
                                FechaModificacion = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                                ModificadoPor = reader.IsDBNull(9) ? null : reader.GetString(9)
                            };
                        else
                            // Manejo cuando no se encuentra la dirección
                            throw new Exception($"No se encontró la dirección con ID: {id}");
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


  

    // Métodos para la tabla Persona
    public void AgregarPersona(Persona persona)
    {
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query =
                    "Exec sp_InsertPersona @Nombre1, @Nombre2, @Apellido1, @Apellido2, @DocumentoIdentidad, @Telefono, @Email, @FechaNacimiento, @Genero, @DireccionID, @CreadoPor, @NewID OUTPUT";
                using (var cmd = new SqlCommand(query, con))
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

 

    // Métodos para la tabla Cliente
    public void AgregarCliente(Cliente cliente)
    {
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query =
                    "Exec sp_InsertCliente @PersonaID, @CodigoCliente, @Estado, @CreadoPor, @NewID OUTPUT";
                using (var cmd = new SqlCommand(query, con))
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


    public Cliente ObtenerClientePorId(int id)
    {
        Cliente cliente = null;
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "Exec sp_GetClienteById @ID";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            cliente = new Cliente
                            {
                                ID = reader.GetInt32(0),
                                PersonaID = reader.GetInt32(1),
                                CodigoCliente = reader.GetInt32(2),
                                Estado = reader.GetString(3),
                                CreadoPor = reader.GetString(4),
                                FechaCreacion = reader.GetDateTime(5),
                                FechaModificacion = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                                ModificadoPor = reader.IsDBNull(7) ? null : reader.GetString(7)
                            };
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

  
    // Métodos para la tabla Producto
    public void AgregarProducto(Producto producto)
    {
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query =
                    "Exec sp_InsertProducto @NombreProducto, @Genero, @SegmentoEdad, @TipoProducto, @Color, @Talla, @UnidadesDisponibles, @Precio, @Descripcion";
                using (var cmd = new SqlCommand(query, con))
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

    
    public void ActualizarProducto(Producto producto)
    {
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query =
                    "Exec sp_UpdateProducto @ID, @NombreProducto, @Genero, @SegmentoEdad, @TipoProducto, @Color, @Talla, @UnidadesDisponibles, @Precio, @Descripcion";
                using (var cmd = new SqlCommand(query, con))
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "Exec sp_DeleteProducto @ID";
                using (var cmd = new SqlCommand(query, con))
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


    // Métodos para la tabla Factura
    public int AgregarFactura(Factura factura)
    {
        var facturaID = 0;

        using (var conexion = new SqlConnection(_conexion))
        {
            var comando = new SqlCommand("sp_InsertFactura", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.AddWithValue("@ClienteID", factura.ClienteID);
            comando.Parameters.AddWithValue("@Total", factura.Total);
            comando.Parameters.AddWithValue("@MetodoPago", factura.MetodoPago);
            comando.Parameters.AddWithValue("@Estado", factura.Estado);
            comando.Parameters.AddWithValue("@CreadoPor", factura.CreadoPor);

            // 🔹 Parámetro de salida para obtener el ID de la factura
            var outputIdParam = new SqlParameter("@FacturaID", SqlDbType.Int);
            outputIdParam.Direction = ParameterDirection.Output;
            comando.Parameters.Add(outputIdParam);

            conexion.Open();
            comando.ExecuteNonQuery();

            // 🔹 Obtener el ID de la factura
            facturaID = Convert.ToInt32(outputIdParam.Value);
        }

        return facturaID;
    }
    public bool ActualizarPerfilUsuario(RegistroViewModel perfil)
    {
        using (var con = new SqlConnection(_conexion))
        using (var cmd = new SqlCommand("sp_ActualizarPerfilUsuario", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PersonaID",      perfil.Persona.ID);
            cmd.Parameters.AddWithValue("@Nombre1",        (object?)perfil.Persona.Nombre1      ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Nombre2",        (object?)perfil.Persona.Nombre2      ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Apellido1",      (object?)perfil.Persona.Apellido1    ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Apellido2",      (object?)perfil.Persona.Apellido2    ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono",       (object?)perfil.Persona.Telefono     ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Genero",         (object?)perfil.Persona.Genero       ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@DireccionID",    perfil.Direccion.ID);
            cmd.Parameters.AddWithValue("@Ciudad",         (object?)perfil.Direccion.Ciudad      ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Estado",         (object?)perfil.Direccion.Estado      ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CodigoPostal",   (object?)perfil.Direccion.CodigoPostal?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Pais",           (object?)perfil.Direccion.Pais        ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TipoDireccion",  (object?)perfil.Direccion.TipoDireccion ?? DBNull.Value);

            var retorno = new SqlParameter("@ReturnValue", SqlDbType.Int)
            {
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(retorno);

            con.Open();
            cmd.ExecuteNonQuery();

            return (int)retorno.Value == 1; // ✅ éxito/fracaso real
        }
    }



    public Factura ObtenerFacturaPorId(int id)
    {
        Factura factura = null;

        try
        {
            using (var conn = new SqlConnection(_conexion))
            {
                conn.Open();
                using (var cmd = new SqlCommand("sp_ObtenerFacturaPorId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            factura = new Factura
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                ClienteID = reader.GetInt32(reader.GetOrdinal("ClienteID")),
                                Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                                MetodoPago = reader.GetString(reader.GetOrdinal("MetodoPago")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                CreadoPor = reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion"))
                                    ? null
                                    : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                ModificadoPor = reader.IsDBNull(reader.GetOrdinal("ModificadoPor"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("ModificadoPor"))
                            };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al obtener factura: " + ex.Message);
            throw;
        }

        return factura;
    }


    // Métodos para la tabla DetalleFactura
    public void AgregarDetalleFactura(DetalleFactura detalleFactura)
    {
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query =
                    "Exec sp_InsertDetalleFactura @FacturaID, @ProductoID, @Cantidad, @PrecioUnitario, @Subtotal, @NewID OUTPUT";
                using (var cmd = new SqlCommand(query, con))
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

  
    public DetalleFactura ObtenerDetalleFacturaPorId(int id)
    {
        DetalleFactura detalle = null;

        try
        {
            using (var conn = new SqlConnection(_conexion))
            {
                conn.Open();
                using (var cmd = new SqlCommand("sp_ObtenerDetalleFacturaPorId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            detalle = new DetalleFactura
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                FacturaID = reader.GetInt32(reader.GetOrdinal("FacturaID")),
                                ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                                Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                                Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                                NombreProducto = reader.IsDBNull(reader.GetOrdinal("NombreProducto"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("NombreProducto")),
                                FechaFactura = reader.GetDateTime(reader.GetOrdinal("FechaFactura"))
                            };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al obtener detalle de factura: " + ex.Message);
            throw;
        }

        return detalle;
    }


  

    public Usuario ObtenerUsuarioPorNombreUsuario(string? nombreUsuario)
    {
        Usuario usuario = null;
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "Exec sp_GetUsuarioByNombreUsuario @NombreUsuario";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            usuario = new Usuario
                            {
                                ID = reader.GetInt32(0),
                                NombreUsuario = reader.GetString(1),
                                ContrasenaHash = reader.GetString(2),
                                Email = reader.GetString(3),
                                Rol = reader.GetString(4),
                                Estado = reader.GetString(5),
                                FechaCreacion = reader.GetDateTime(6),
                                FechaModificacion = reader.IsDBNull(7) ? null : reader.GetDateTime(7)
                            };
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query =
                    "Exec sp_InsertUsuario @NombreUsuario, @ContrasenaHash, @Email, @Rol, @Estado, @NewID OUTPUT";
                using (var cmd = new SqlCommand(query, con))
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



    public Persona ObtenerPersonaPorCedula(string documentoIdentidad)
    {
        Persona persona = null;
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "Exec BuscarPersonaPorCedula @DocumentoIdentidad";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DocumentoIdentidad", documentoIdentidad);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            persona = new Persona
                            {
                                ID = reader.GetInt32(0),
                                Nombre1 = reader.GetString(1),
                                Nombre2 = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Apellido1 = reader.GetString(3),
                                Apellido2 = reader.GetString(4),
                                DocumentoIdentidad = reader.GetString(5),
                                Telefono = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Email = reader.IsDBNull(7) ? null : reader.GetString(7),
                                FechaNacimiento = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                                Genero = reader.IsDBNull(9) ? null : reader.GetString(9),
                                DireccionID = reader.GetInt32(10),
                                CreadoPor = reader.GetString(11),
                                FechaModificacion = reader.IsDBNull(12) ? null : reader.GetDateTime(12),
                                ModificadoPor = reader.IsDBNull(13) ? null : reader.GetString(13)
                            };
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "EXEC sp_ObtenerUsuarioPorEmail @Email";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            usuario = new Usuario
                            {
                                ID = reader.GetInt32(0),
                                NombreUsuario = reader.GetString(1),
                                ContrasenaHash = reader.GetString(2),
                                Email = reader.GetString(3),
                                Rol = reader.GetString(4),
                                Estado = reader.GetString(5),
                                FechaCreacion = reader.GetDateTime(6),
                                FechaModificacion = reader.IsDBNull(7) ? null : reader.GetDateTime(7)
                            };
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



    public Persona ObtenerPersonaPorTelefono(string telefono)
    {
        Persona persona = null;
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "Exec sp_BuscarPersonaPorTelefono @Telefono";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Telefono", telefono);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            persona = new Persona
                            {
                                ID = reader.GetInt32(0),
                                Nombre1 = reader.GetString(1),
                                Nombre2 = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Apellido1 = reader.GetString(3),
                                Apellido2 = reader.GetString(4),
                                DocumentoIdentidad = reader.GetString(5),
                                Telefono = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Email = reader.IsDBNull(7) ? null : reader.GetString(7),
                                FechaNacimiento = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                                Genero = reader.IsDBNull(9) ? null : reader.GetString(9),
                                FechaCreacion = reader.GetDateTime(10),
                                CreadoPor = reader.GetString(11),
                                FechaModificacion = reader.IsDBNull(12) ? null : reader.GetDateTime(12),
                                ModificadoPor = reader.IsDBNull(13) ? null : reader.GetString(13),
                                DireccionID = reader.GetInt32(15)
                            };
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "Exec sp_GetTodosLosProductos";
                using (var cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
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
                                    ? null
                                    : Convert.ToDateTime(dr["FechaModificacion"])
                            });
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "Exec ObtenerPersonaPorEmail @Email";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
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
                                FechaNacimiento = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                                Genero = reader.IsDBNull(9) ? null : reader.GetString(9),

                                DireccionID = reader.GetInt32(14)
                            };
                        else
                            throw new Exception("No se encontró una persona con ese correo electrónico.");
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "EXEC sp_ActualizarCantidadCarrito @ClienteID, @ProductoID, @NuevaCantidad";
                using (var cmd = new SqlCommand(query, con))
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "EXEC sp_AgregarAlCarrito @ClienteID, @ProductoID, @Cantidad";
                using (var cmd = new SqlCommand(query, con))
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

        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "EXEC sp_ObtenerCarritoPorClienteID @ClienteID";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            carrito.Add(new CarritoItem
                            {
                                ProductoID = reader.GetInt32(2),
                                NombreProducto = reader.GetString(3),
                                Precio = reader.GetDecimal(9),
                                ImagenUrl = reader.IsDBNull(10) ? null : reader.GetString(10),
                                Descripcion = reader.IsDBNull(11) ? null : reader.GetString(11),
                                Cantidad = reader.GetInt32(12)
                            });
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "Exec sp_ObtenerClientePorPersonaID @PersonaID";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@PersonaID", personaID);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            cliente = new Cliente
                            {
                                ID = reader.GetInt32(0),
                                PersonaID = reader.GetInt32(1),
                                CodigoCliente = reader.GetInt32(2),
                                Estado = reader.GetString(3),
                                CreadoPor = reader.GetString(4),
                                FechaCreacion = reader.GetDateTime(5),
                                FechaModificacion = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                                ModificadoPor = reader.IsDBNull(7) ? null : reader.GetString(7)
                            };
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "EXEC sp_EliminarProductoCarrito @ClienteID, @ProductoID";
                using (var cmd = new SqlCommand(query, con))
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
        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "EXEC sp_EliminarCarritoPorCliente @ClienteID";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                    con.Open();
                    var filasAfectadas = cmd.ExecuteNonQuery();
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

        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "EXEC ObtenerDetallesFacturaPorFacturaID @FacturaID";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FacturaID", facturaId);
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var detalle = new DetalleFactura
                            {
                                ID = int.Parse(reader["ID"].ToString()),
                                FacturaID = int.Parse(reader["FacturaID"].ToString()),
                                ProductoID = int.Parse(reader["ProductoID"].ToString()),
                                Cantidad = int.Parse(reader["Cantidad"].ToString()),
                                PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                                Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                                Producto = new Producto
                                {
                                    ID = int.Parse(reader["ProductoID"].ToString()),
                                    NombreProducto = reader["NombreProducto"].ToString()
                                }
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

    public string ObtenerNombreProductoPorID(int productoID)
    {
        var nombre = string.Empty;

        using (var conn = new SqlConnection(_conexion))
        {
            using (var cmd = new SqlCommand("ObtenerNombreProductoPorID", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID", productoID);

                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value) nombre = result.ToString();
            }
        }

        return nombre;
    }


    public RegistroViewModel ObtenerPerfilUsuario(int usuarioId)
    {
        var perfil = new RegistroViewModel();

        using (var con = new SqlConnection(_conexion))
        {
            using (var cmd = new SqlCommand("sp_ObtenerPerfilUsuario", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioId);

                con.Open();
                using (var reader = cmd.ExecuteReader())
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


    public bool EliminarPerfilUsuario(int usuarioId)
    {
        using (var con = new SqlConnection(_conexion))
        using (var cmd = new SqlCommand("sp_EliminarPerfilUsuario", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UsuarioID", usuarioId);

            // Parámetro de retorno
            var returnValue = new SqlParameter("@ReturnValue", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnValue);

            con.Open();
            cmd.ExecuteNonQuery();

            return (int)returnValue.Value == 1;
        }
    }

    public bool AgregarDevolucion(Devolucion devolucion)
    {
        try
        {
            using (var conn = new SqlConnection(_conexion))
            {
                conn.Open();
                using (var cmd = new SqlCommand("sp_AgregarDevolucion", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacturaID", devolucion.FacturaID);
                    cmd.Parameters.AddWithValue("@DetalleFacturaID", devolucion.DetalleFacturaID);
                    cmd.Parameters.AddWithValue("@ProductoID", devolucion.ProductoID);
                    cmd.Parameters.AddWithValue("@Cantidad", devolucion.Cantidad);
                    cmd.Parameters.AddWithValue("@Motivo", devolucion.Motivo);
                    cmd.Parameters.AddWithValue("@Estado", devolucion.Estado);
                    cmd.Parameters.AddWithValue("@CreadoPor", devolucion.CreadoPor);

                    var filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al agregar devolución: " + ex.Message);
            return false;
        }
    }



    public List<DetalleFactura> ObtenerProductosCompradosPorCliente(int clienteID)
    {
        List<DetalleFactura> productosComprados = new List<DetalleFactura>();

        using (var con = new SqlConnection(_conexion))
        {
            try
            {
                var query = "EXEC sp_ObtenerProductosCompradosPorCliente @ClienteID";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ClienteID", clienteID);
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            productosComprados.Add(new DetalleFactura
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                FacturaID = Convert.ToInt32(reader["FacturaID"]),
                                ProductoID = Convert.ToInt32(reader["ProductoID"]),
                                NombreProducto =
                                    reader["NombreProducto"].ToString(), // 🆕 Aquí el nombre del producto
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                                Subtotal = Convert.ToDecimal(reader["Subtotal"]),
                                FechaFactura =
                                    Convert.ToDateTime(
                                        reader["FechaFactura"]) // 🆕 Para validar si aplica devolución
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos comprados: " + ex.Message);
            }
        }

        return productosComprados;
    }


   
}