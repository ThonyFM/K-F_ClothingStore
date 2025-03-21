CREATE TABLE Direccion
(
    ID                INT IDENTITY(1,1) PRIMARY KEY,
    Ciudad            NVARCHAR(50) NOT NULL,
    Estado            NVARCHAR(50) NOT NULL,
    CodigoPostal      NVARCHAR(20) NOT NULL,
    Pais              NVARCHAR(50) NOT NULL,
    TipoDireccion     NVARCHAR(20),
    FechaCreacion     DATETIME DEFAULT GETDATE(),
    CreadoPor         NVARCHAR(50) NOT NULL,
    FechaModificacion DATETIME NULL,
    ModificadoPor     NVARCHAR(50) NULL,
);

CREATE TABLE Persona
(
    ID                 INT IDENTITY(1,1) PRIMARY KEY,
    Nombre1            NVARCHAR(50) NOT NULL,
    Nombre2            NVARCHAR(50),
    Apellido1          NVARCHAR(50) NOT NULL,
    Apellido2          NVARCHAR(50) NOT NULL,
    DocumentoIdentidad NVARCHAR(20) UNIQUE NOT NULL,
    Telefono           NVARCHAR(15),
    Email              NVARCHAR(255) UNIQUE,
    FechaNacimiento    DATE,
    Genero             NVARCHAR(50),
    FechaRegistro      DATETIME DEFAULT GETDATE(),
    FechaCreacion      DATETIME DEFAULT GETDATE(),
    CreadoPor          NVARCHAR(50) NOT NULL,
    FechaModificacion  DATETIME NULL,
    ModificadoPor      NVARCHAR(50) NULL,
    DireccionID        INT NOT NULL
        CONSTRAINT FK_Direccion_Persona FOREIGN KEY (DireccionID) REFERENCES Direccion(ID)
);
CREATE TABLE Cliente
(
    ID                INT IDENTITY(1,1) PRIMARY KEY,
    PersonaID         INT NOT NULL,
    CodigoCliente     INT NOT NULL,
    Estado            NVARCHAR(50) NOT NULL,
    CreadoPor         NVARCHAR(50) NOT NULL,
    FechaCreacion     DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL,
    ModificadoPor     NVARCHAR(50) NULL,
    CONSTRAINT FK_Cliente_Persona FOREIGN KEY (PersonaID) REFERENCES Persona (ID)
);
CREATE TABLE Empleado
(
    ID                INT IDENTITY(1,1) PRIMARY KEY,
    PersonaID         INT            NOT NULL,
    Puesto            NVARCHAR(100) NOT NULL,
    FechaContratacion DATE           NOT NULL,
    Salario           DECIMAL(10, 2) NOT NULL,
    Estado            NVARCHAR(50) NOT NULL,
    CreadoPor         NVARCHAR(50) NOT NULL,
    FechaCreacion     DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL,
    ModificadoPor     NVARCHAR(50) NULL,
    CONSTRAINT FK_Empleado_Persona FOREIGN KEY (PersonaID) REFERENCES Persona (ID)
);
CREATE TABLE Producto
(
    ID                  INT IDENTITY(1,1) PRIMARY KEY,
    NombreProducto      NVARCHAR(100) NOT NULL,
    Genero              NVARCHAR(20) NOT NULL,
    SegmentoEdad        NVARCHAR(50) NOT NULL,
    TipoProducto        NVARCHAR(50) NOT NULL,
    Color               NVARCHAR(30) NOT NULL,
    Talla               NVARCHAR(20) NOT NULL,
    UnidadesDisponibles INT            NOT NULL,
    Precio              DECIMAL(10, 2) NOT NULL,
    ImagenUrl               NVARCHAR(20),
    Descripcion         NVARCHAR(500) NULL,
    FechaCreacion       DATETIME DEFAULT GETDATE(),
    FechaModificacion   DATETIME NULL
);
CREATE TABLE Venta
(
    VentaID    INT IDENTITY(1,1) PRIMARY KEY,
    EmpleadoID INT            NOT NULL,
    ClienteID  INT            NOT NULL,
    ProductoID INT            NOT NULL,
    Cantidad   INT            NOT NULL,
    Total      DECIMAL(10, 2) NOT NULL,
    Fecha      DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Venta_Empleado FOREIGN KEY (EmpleadoID) REFERENCES Empleado (ID),
    CONSTRAINT FK_Venta_Cliente FOREIGN KEY (ClienteID) REFERENCES Cliente (ID),
    CONSTRAINT FK_Venta_Producto FOREIGN KEY (ProductoID) REFERENCES Producto (ID)
);

CREATE TABLE Descuento
(
    ID          INT IDENTITY(1,1) PRIMARY KEY,
    Tipo        NVARCHAR(50) NOT NULL,
    Valor       DECIMAL(10, 2) NOT NULL,
    Descripcion NVARCHAR(255) NULL,
    FechaInicio DATETIME       NOT NULL,
    FechaFin    DATETIME NULL,
    Estado      NVARCHAR(50) NOT NULL
);

CREATE TABLE Factura
(
    ID                INT IDENTITY(1,1) PRIMARY KEY,
    ClienteID         INT NOT NULL,
    EmpleadoID        INT NOT NULL,
    FechaEmision      DATETIME DEFAULT GETDATE(),
    Total             DECIMAL,
    MetodoPago        NVARCHAR(50) NOT NULL,
    Estado            NVARCHAR(50) NOT NULL,
    DescuentoID       INT NULL,
    FechaCreacion     DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL,
    CreadoPor         NVARCHAR(50) NOT NULL,
    ModificadoPor     NVARCHAR(50) NULL,
    CONSTRAINT FK_Factura_Cliente FOREIGN KEY (ClienteID) REFERENCES Cliente (ID),
    CONSTRAINT FK_Factura_Empleado FOREIGN KEY (EmpleadoID) REFERENCES Empleado (ID),
    CONSTRAINT FK_Factura_Descuento FOREIGN KEY (DescuentoID) REFERENCES Descuento (ID)
);
CREATE TABLE DetalleFactura
(
    ID             INT IDENTITY(1,1) PRIMARY KEY,
    FacturaID      INT            NOT NULL,
    ProductoID     INT            NOT NULL,
    Cantidad       INT            NOT NULL,
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    Subtotal       DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_DetalleFactura_Factura FOREIGN KEY (FacturaID) REFERENCES Factura (ID),
    CONSTRAINT FK_DetalleFactura_Producto FOREIGN KEY (ProductoID) REFERENCES Producto (ID)
);
CREATE TABLE Devolucion
(
    ID                INT IDENTITY(1,1) PRIMARY KEY,
    FacturaID         INT NOT NULL,
    DetalleFacturaID  INT NOT NULL,
    ProductoID        INT NOT NULL,
    Cantidad          INT NOT NULL,
    Motivo            NVARCHAR(255) NOT NULL,
    FechaDevolucion   DATETIME DEFAULT GETDATE(),
    Estado            NVARCHAR(50) NOT NULL,
    CreadoPor         NVARCHAR(50) NOT NULL,
    FechaCreacion     DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL,
    ModificadoPor     NVARCHAR(50) NULL,
    CONSTRAINT FK_Devolucion_Factura FOREIGN KEY (FacturaID) REFERENCES Factura (ID),
    CONSTRAINT FK_Devolucion_DetalleFactura FOREIGN KEY (DetalleFacturaID) REFERENCES DetalleFactura (ID),
    CONSTRAINT FK_Devolucion_Producto FOREIGN KEY (ProductoID) REFERENCES Producto (ID)
);
CREATE TABLE Proveedor
(
    ID                INT IDENTITY(1,1) PRIMARY KEY,
    NombreEmpresa     NVARCHAR(100) NOT NULL,
    NombreContacto    NVARCHAR(100) NULL,
    Telefono          NVARCHAR(15) NULL,
    Email             NVARCHAR(255) NULL,
    DireccionID       INT NOT NULL,
    Estado            NVARCHAR(50) NOT NULL,
    FechaCreacion     DATETIME DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL,
    CreadoPor         NVARCHAR(50) NOT NULL,
    ModificadoPor     NVARCHAR(50) NULL,
    CONSTRAINT FK_Proveedor_Direccion FOREIGN KEY (DireccionID) REFERENCES Direccion (ID)
);


ALTER TABLE Persona
    ADD UsuarioID INT NULL,
    CONSTRAINT FK_Persona_Usuario FOREIGN KEY (UsuarioID) REFERENCES Usuario(ID);

GO


CREATE PROCEDURE sp_InsertDireccion @Ciudad NVARCHAR(50),
    @Estado NVARCHAR(50),
    @CodigoPostal NVARCHAR(20),
    @Pais NVARCHAR(50),
    @TipoDireccion NVARCHAR(20) = NULL,
    @CreadoPor NVARCHAR(50),
    @NewID INT OUTPUT
AS
BEGIN
    SET
NOCOUNT ON;
INSERT INTO Direccion (Ciudad, Estado, CodigoPostal, Pais, TipoDireccion, CreadoPor)
VALUES (@Ciudad, @Estado, @CodigoPostal, @Pais, @TipoDireccion, @CreadoPor);

SET
@NewID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdateDireccion @ID INT,
    @Ciudad NVARCHAR(50),
    @Estado NVARCHAR(50),
    @CodigoPostal NVARCHAR(20),
    @Pais NVARCHAR(50),
    @TipoDireccion NVARCHAR(20) = NULL,
    @ModificadoPor NVARCHAR(50)
AS
BEGIN
    SET
NOCOUNT ON;
UPDATE Direccion
SET Ciudad            = @Ciudad,
    Estado            = @Estado,
    CodigoPostal      = @CodigoPostal,
    Pais              = @Pais,
    TipoDireccion     = @TipoDireccion,
    FechaModificacion = GETDATE(),
    ModificadoPor     = @ModificadoPor
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_DeleteDireccion @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
    
    -- Verificar si la direcci�n est� en uso
    IF
EXISTS (SELECT 1 FROM Persona WHERE DireccionID = @ID)
BEGIN
        RAISERROR
('No se puede eliminar la direcci�n porque est� en uso.', 16, 1);
        RETURN;
END

DELETE
FROM Direccion
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetDireccionById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Direccion
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetAllDirecciones
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Direccion;
END
GO

CREATE PROCEDURE sp_InsertPersona @Nombre1 NVARCHAR(50),
    @Nombre2 NVARCHAR(50) = NULL,
    @Apellido1 NVARCHAR(50),
    @Apellido2 NVARCHAR(50),
    @DocumentoIdentidad NVARCHAR(20),
    @Telefono NVARCHAR(15) = NULL,
    @Email NVARCHAR(255) = NULL,
    @FechaNacimiento DATE = NULL,
    @Genero NVARCHAR(50) = NULL,
    @DireccionID INT,
    @CreadoPor NVARCHAR(50),
    @NewID INT OUTPUT
AS
BEGIN
    SET
NOCOUNT ON;
INSERT INTO Persona (Nombre1, Nombre2, Apellido1, Apellido2, DocumentoIdentidad, Telefono, Email, FechaNacimiento,
                     Genero, DireccionID, CreadoPor)
VALUES (@Nombre1, @Nombre2, @Apellido1, @Apellido2, @DocumentoIdentidad, @Telefono, @Email, @FechaNacimiento, @Genero,
        @DireccionID, @CreadoPor);

SET
@NewID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdatePersona @ID INT,
    @Nombre1 NVARCHAR(50),
    @Nombre2 NVARCHAR(50) = NULL,
    @Apellido1 NVARCHAR(50),
    @Apellido2 NVARCHAR(50),
    @Telefono NVARCHAR(15) = NULL,
    @Email NVARCHAR(255) = NULL,
    @FechaNacimiento DATE = NULL,
    @Genero NVARCHAR(50) = NULL,
    @DireccionID INT,
    @ModificadoPor NVARCHAR(50)
AS
BEGIN
    SET
NOCOUNT ON;
UPDATE Persona
SET Nombre1           = @Nombre1,
    Nombre2           = @Nombre2,
    Apellido1         = @Apellido1,
    Apellido2         = @Apellido2,
    Telefono          = @Telefono,
    Email             = @Email,
    FechaNacimiento   = @FechaNacimiento,
    Genero            = @Genero,
    DireccionID       = @DireccionID,
    FechaModificacion = GETDATE(),
    ModificadoPor     = @ModificadoPor
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_DeletePersona @ID INT
AS
BEGIN
    SET
NOCOUNT ON;

    -- Verificar si la persona es un cliente o empleado
    IF
EXISTS (SELECT 1 FROM Cliente WHERE PersonaID = @ID) OR EXISTS (SELECT 1 FROM Empleado WHERE PersonaID = @ID)
BEGIN
        RAISERROR
('No se puede eliminar la persona porque est� asociada a un cliente o empleado.', 16, 1);
        RETURN;
END

DELETE
FROM Persona
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetPersonaById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Persona
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetAllPersonas
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Persona;
END
GO

CREATE PROCEDURE sp_InsertCliente @PersonaID INT,
    @CodigoCliente INT,
    @Estado NVARCHAR(50),
    @CreadoPor NVARCHAR(50),
    @NewID INT OUTPUT
AS
BEGIN
    SET
NOCOUNT ON;
INSERT INTO Cliente (PersonaID, CodigoCliente, Estado, CreadoPor)
VALUES (@PersonaID, @CodigoCliente, @Estado, @CreadoPor);

SET
@NewID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdateCliente @ID INT,
    @CodigoCliente INT,
    @Estado NVARCHAR(50),
    @ModificadoPor NVARCHAR(50)
AS
BEGIN
    SET
NOCOUNT ON;
UPDATE Cliente
SET CodigoCliente     = @CodigoCliente,
    Estado            = @Estado,
    FechaModificacion = GETDATE(),
    ModificadoPor     = @ModificadoPor
WHERE ID = @ID;
END
GO
CREATE PROCEDURE sp_DeleteCliente @ID INT
AS
BEGIN
    SET
NOCOUNT ON;

    -- Verificar si el cliente tiene ventas o facturas
    IF
EXISTS (SELECT 1 FROM Venta WHERE ClienteID = @ID) OR EXISTS (SELECT 1 FROM Factura WHERE ClienteID = @ID)
BEGIN
        RAISERROR
('No se puede eliminar el cliente porque tiene ventas o facturas asociadas.', 16, 1);
        RETURN;
END

DELETE
FROM Cliente
WHERE ID = @ID;
END
GO
CREATE PROCEDURE sp_GetClienteById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Cliente
WHERE ID = @ID;
END
GO
CREATE PROCEDURE sp_GetAllClientes
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Cliente;
END
GO

CREATE PROCEDURE sp_InsertEmpleado @PersonaID INT,
    @Puesto NVARCHAR(100),
    @FechaContratacion DATE,
    @Salario DECIMAL(10,2),
    @Estado NVARCHAR(50),
    @CreadoPor NVARCHAR(50),
    @NewID INT OUTPUT
AS
BEGIN
    SET
NOCOUNT ON;
INSERT INTO Empleado (PersonaID, Puesto, FechaContratacion, Salario, Estado, CreadoPor)
VALUES (@PersonaID, @Puesto, @FechaContratacion, @Salario, @Estado, @CreadoPor);

SET
@NewID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdateEmpleado @ID INT,
    @Puesto NVARCHAR(100),
    @FechaContratacion DATE,
    @Salario DECIMAL(10,2),
    @Estado NVARCHAR(50),
    @ModificadoPor NVARCHAR(50)
AS
BEGIN
    SET
NOCOUNT ON;
UPDATE Empleado
SET Puesto            = @Puesto,
    FechaContratacion = @FechaContratacion,
    Salario           = @Salario,
    Estado            = @Estado,
    FechaModificacion = GETDATE(),
    ModificadoPor     = @ModificadoPor
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_DeleteEmpleado @ID INT
AS
BEGIN
    SET
NOCOUNT ON;

    -- Verificar si el empleado ha realizado ventas o facturas
    IF
EXISTS (SELECT 1 FROM Venta WHERE EmpleadoID = @ID) OR EXISTS (SELECT 1 FROM Factura WHERE EmpleadoID = @ID)
BEGIN
        RAISERROR
('No se puede eliminar el empleado porque tiene ventas o facturas asociadas.', 16, 1);
        RETURN;
END

DELETE
FROM Empleado
WHERE ID = @ID;
END
GO
CREATE PROCEDURE sp_GetEmpleadoById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Empleado
WHERE ID = @ID;
END
GO
CREATE PROCEDURE sp_GetAllEmpleados
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Empleado;
END
GO
CREATE PROCEDURE sp_InsertProducto @NombreProducto NVARCHAR(100),
    @Genero NVARCHAR(20),
    @SegmentoEdad NVARCHAR(50),
    @TipoProducto NVARCHAR(50),
    @Color NVARCHAR(30),
    @Talla NVARCHAR(20),
    @UnidadesDisponibles INT,
    @Precio DECIMAL(10,2),
    @Descripcion NVARCHAR(500)
AS
BEGIN
INSERT INTO Producto (NombreProducto, Genero, SegmentoEdad, TipoProducto, Color, Talla, UnidadesDisponibles, Precio,
                      Descripcion)
VALUES (@NombreProducto, @Genero, @SegmentoEdad, @TipoProducto, @Color, @Talla, @UnidadesDisponibles, @Precio,
        @Descripcion);
END;
GO

CREATE PROCEDURE sp_UpdateProducto @ID INT,
    @NombreProducto NVARCHAR(100),
    @Genero NVARCHAR(20),
    @SegmentoEdad NVARCHAR(50),
    @TipoProducto NVARCHAR(50),
    @Color NVARCHAR(30),
    @Talla NVARCHAR(20),
    @UnidadesDisponibles INT,
    @Precio DECIMAL(10,2),
    @Descripcion NVARCHAR(500)
AS
BEGIN
UPDATE Producto
SET NombreProducto      = @NombreProducto,
    Genero              = @Genero,
    SegmentoEdad        = @SegmentoEdad,
    TipoProducto        = @TipoProducto,
    Color               = @Color,
    Talla               = @Talla,
    UnidadesDisponibles = @UnidadesDisponibles,
    Precio              = @Precio,
    Descripcion         = @Descripcion
WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteProducto @ID INT
AS
BEGIN
DELETE
FROM Producto
WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetProducto @ID INT
AS
BEGIN
SELECT *
FROM Producto
WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_InsertVenta @EmpleadoID INT,
    @ClienteID INT,
    @ProductoID INT,
    @Cantidad INT,
    @Total DECIMAL(10,2)
AS
BEGIN
INSERT INTO Venta (EmpleadoID, ClienteID, ProductoID, Cantidad, Total)
VALUES (@EmpleadoID, @ClienteID, @ProductoID, @Cantidad, @Total);
END;
GO

CREATE PROCEDURE sp_UpdateVenta @VentaID INT,
    @EmpleadoID INT,
    @ClienteID INT,
    @ProductoID INT,
    @Cantidad INT,
    @Total DECIMAL(10,2)
AS
BEGIN
UPDATE Venta
SET EmpleadoID = @EmpleadoID,
    ClienteID  = @ClienteID,
    ProductoID = @ProductoID,
    Cantidad   = @Cantidad,
    Total      = @Total
WHERE VentaID = @VentaID;
END;
GO

CREATE PROCEDURE sp_DeleteVenta @VentaID INT
AS
BEGIN
DELETE
FROM Venta
WHERE VentaID = @VentaID;
END;
GO

CREATE PROCEDURE sp_GetVenta @VentaID INT
AS
BEGIN
SELECT *
FROM Venta
WHERE VentaID = @VentaID;
END;
GO

CREATE PROCEDURE sp_InsertFactura @ClienteID INT,
    @EmpleadoID INT,
    @Total DECIMAL(10,2),
    @MetodoPago NVARCHAR(50),
    @Estado NVARCHAR(50),
    @DescuentoID INT,
    @CreadoPor NVARCHAR(50)
AS
BEGIN
INSERT INTO Factura (ClienteID, EmpleadoID, Total, MetodoPago, Estado, DescuentoID, CreadoPor)
VALUES (@ClienteID, @EmpleadoID, @Total, @MetodoPago, @Estado, @DescuentoID, @CreadoPor);
END;
GO

CREATE PROCEDURE sp_UpdateFactura @ID INT,
    @ClienteID INT,
    @EmpleadoID INT,
    @Total DECIMAL(10,2),
    @MetodoPago NVARCHAR(50),
    @Estado NVARCHAR(50),
    @DescuentoID INT,
    @ModificadoPor NVARCHAR(50)
AS
BEGIN
UPDATE Factura
SET ClienteID         = @ClienteID,
    EmpleadoID        = @EmpleadoID,
    Total             = @Total,
    MetodoPago        = @MetodoPago,
    Estado            = @Estado,
    DescuentoID       = @DescuentoID,
    FechaModificacion = GETDATE(),
    ModificadoPor     = @ModificadoPor
WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteFactura @ID INT
AS
BEGIN
DELETE
FROM Factura
WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetFactura @ID INT
AS
BEGIN
SELECT *
FROM Factura
WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_InsertProveedor @NombreEmpresa NVARCHAR(100),
    @NombreContacto NVARCHAR(100),
    @Telefono NVARCHAR(15),
    @Email NVARCHAR(255),
    @DireccionID INT,
    @Estado NVARCHAR(50),
    @CreadoPor NVARCHAR(50)
AS
BEGIN
INSERT INTO Proveedor (NombreEmpresa, NombreContacto, Telefono, Email, DireccionID, Estado, CreadoPor)
VALUES (@NombreEmpresa, @NombreContacto, @Telefono, @Email, @DireccionID, @Estado, @CreadoPor);
END;
GO

CREATE PROCEDURE sp_UpdateProveedor @ID INT,
    @NombreEmpresa NVARCHAR(100),
    @NombreContacto NVARCHAR(100),
    @Telefono NVARCHAR(15),
    @Email NVARCHAR(255),
    @DireccionID INT,
    @Estado NVARCHAR(50),
    @ModificadoPor NVARCHAR(50)
AS
BEGIN
UPDATE Proveedor
SET NombreEmpresa     = @NombreEmpresa,
    NombreContacto    = @NombreContacto,
    Telefono          = @Telefono,
    Email             = @Email,
    DireccionID       = @DireccionID,
    Estado            = @Estado,
    FechaModificacion = GETDATE(),
    ModificadoPor     = @ModificadoPor
WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_DeleteProveedor @ID INT
AS
BEGIN
DELETE
FROM Proveedor
WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_GetProveedor @ID INT
AS
BEGIN
SELECT *
FROM Proveedor
WHERE ID = @ID;
END;
GO

CREATE PROCEDURE sp_InsertDescuento @Tipo NVARCHAR(50),
    @Valor DECIMAL(10,2),
    @Descripcion NVARCHAR(255) = NULL,
    @FechaInicio DATETIME,
    @FechaFin DATETIME = NULL,
    @Estado NVARCHAR(50),
    @NewID INT OUTPUT
AS
BEGIN
    SET
NOCOUNT ON;
INSERT INTO Descuento (Tipo, Valor, Descripcion, FechaInicio, FechaFin, Estado)
VALUES (@Tipo, @Valor, @Descripcion, @FechaInicio, @FechaFin, @Estado);

SET
@NewID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdateDescuento @ID INT,
    @Tipo NVARCHAR(50),
    @Valor DECIMAL(10,2),
    @Descripcion NVARCHAR(255) = NULL,
    @FechaInicio DATETIME,
    @FechaFin DATETIME = NULL,
    @Estado NVARCHAR(50)
AS
BEGIN
    SET
NOCOUNT ON;
UPDATE Descuento
SET Tipo        = @Tipo,
    Valor       = @Valor,
    Descripcion = @Descripcion,
    FechaInicio = @FechaInicio,
    FechaFin    = @FechaFin,
    Estado      = @Estado
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_DeleteDescuento @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
DELETE
FROM Descuento
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetDescuentoById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Descuento
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetAllDescuentos
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Descuento;
END
GO

CREATE PROCEDURE sp_InsertDetalleFactura @FacturaID INT,
    @ProductoID INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2),
    @Subtotal DECIMAL(10,2),
    @NewID INT OUTPUT
AS
BEGIN
    SET
NOCOUNT ON;
INSERT INTO DetalleFactura (FacturaID, ProductoID, Cantidad, PrecioUnitario, Subtotal)
VALUES (@FacturaID, @ProductoID, @Cantidad, @PrecioUnitario, @Subtotal);

SET
@NewID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdateDetalleFactura @ID INT,
    @FacturaID INT,
    @ProductoID INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2),
    @Subtotal DECIMAL(10,2)
AS
BEGIN
    SET
NOCOUNT ON;
UPDATE DetalleFactura
SET FacturaID      = @FacturaID,
    ProductoID     = @ProductoID,
    Cantidad       = @Cantidad,
    PrecioUnitario = @PrecioUnitario,
    Subtotal       = @Subtotal
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_DeleteDetalleFactura @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
DELETE
FROM DetalleFactura
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetDetalleFacturaById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM DetalleFactura
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetAllDetallesFactura
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM DetalleFactura;
END
GO

CREATE PROCEDURE sp_InsertDevolucion @FacturaID INT,
    @DetalleFacturaID INT,
    @ProductoID INT,
    @Cantidad INT,
    @Motivo NVARCHAR(255),
    @Estado NVARCHAR(50),
    @CreadoPor NVARCHAR(50),
    @NewID INT OUTPUT
AS
BEGIN
    SET
NOCOUNT ON;
INSERT INTO Devolucion (FacturaID, DetalleFacturaID, ProductoID, Cantidad, Motivo, Estado, CreadoPor)
VALUES (@FacturaID, @DetalleFacturaID, @ProductoID, @Cantidad, @Motivo, @Estado, @CreadoPor);

SET
@NewID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdateDevolucion @ID INT,
    @FacturaID INT,
    @DetalleFacturaID INT,
    @ProductoID INT,
    @Cantidad INT,
    @Motivo NVARCHAR(255),
    @Estado NVARCHAR(50),
    @ModificadoPor NVARCHAR(50)
AS
BEGIN
    SET
NOCOUNT ON;
UPDATE Devolucion
SET FacturaID         = @FacturaID,
    DetalleFacturaID  = @DetalleFacturaID,
    ProductoID        = @ProductoID,
    Cantidad          = @Cantidad,
    Motivo            = @Motivo,
    Estado            = @Estado,
    FechaModificacion = GETDATE(),
    ModificadoPor     = @ModificadoPor
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_DeleteDevolucion @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
DELETE
FROM Devolucion
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetDevolucionById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Devolucion
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetAllDevoluciones
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Devolucion;
END
GO

CREATE PROCEDURE sp_InsertUsuario @NombreUsuario NVARCHAR(50),
    @ContrasenaHash NVARCHAR(255),
    @Email NVARCHAR(255),
    @Rol NVARCHAR(50),
    @Estado NVARCHAR(50),
    @NewID INT OUTPUT
AS
BEGIN
    SET
NOCOUNT ON;
INSERT INTO Usuario (NombreUsuario, ContrasenaHash, Email, Rol, Estado)
VALUES (@NombreUsuario, @ContrasenaHash, @Email, @Rol, @Estado);

SET
@NewID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_UpdateUsuario @ID INT,
    @NombreUsuario NVARCHAR(50),
    @ContrasenaHash NVARCHAR(255),
    @Email NVARCHAR(255),
    @Rol NVARCHAR(50),
    @Estado NVARCHAR(50)
AS
BEGIN
    SET
NOCOUNT ON;
UPDATE Usuario
SET NombreUsuario     = @NombreUsuario,
    ContrasenaHash    = @ContrasenaHash,
    Email             = @Email,
    Rol               = @Rol,
    Estado            = @Estado,
    FechaModificacion = GETDATE()
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_DeleteUsuario @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
DELETE
FROM Usuario
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetUsuarioById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Usuario
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetAllUsuarios
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Usuario;
END
GO



CREATE PROCEDURE sp_GetVentaById @VentaID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Venta
WHERE VentaID = @VentaID;
END
GO

CREATE PROCEDURE sp_GetAllVentas
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Venta;
END
GO



CREATE PROCEDURE sp_GetFacturaById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Factura
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetAllFacturas
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Factura;
END
GO



CREATE PROCEDURE sp_GetProveedorById @ID INT
AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Proveedor
WHERE ID = @ID;
END
GO

CREATE PROCEDURE sp_GetAllProveedores
    AS
BEGIN
    SET
NOCOUNT ON;
SELECT *
FROM Proveedor;
END
GO
CREATE PROCEDURE sp_GetUsuarioByNombreUsuario @NombreUsuario NVARCHAR(50)
AS
BEGIN
SELECT *
FROM Usuario
WHERE NombreUsuario = @NombreUsuario;
END
GO
CREATE PROCEDURE BuscarPersonaPorCedula @DocumentoIdentidad NVARCHAR(20)
AS
BEGIN
    SET
NOCOUNT ON;

SELECT p.ID,
       p.Nombre1,
       p.Nombre2,
       p.Apellido1,
       p.Apellido2,
       p.DocumentoIdentidad,
       p.Telefono,
       p.Email,
       p.FechaNacimiento,
       p.Genero,
       p.DireccionID,
       d.Ciudad,
       d.Estado,
       d.CodigoPostal,
       d.Pais,
       d.TipoDireccion
FROM Persona p
         INNER JOIN Direccion d ON p.DireccionID = d.ID
WHERE p.DocumentoIdentidad = @DocumentoIdentidad;
END
GO
CREATE PROCEDURE sp_ObtenerUsuarioPorEmail @Email NVARCHAR(255)
AS
BEGIN
    SET
NOCOUNT ON;

SELECT ID,
       NombreUsuario,
       ContrasenaHash,
       Email,
       Rol,
       Estado,
       FechaCreacion,
       FechaModificacion
FROM Usuario
WHERE Email = @Email;
END
GO
CREATE PROCEDURE sp_ObtenerUsuarioPorNombreUsuario @NombreUsuario NVARCHAR(50)
AS
BEGIN
    SET
NOCOUNT ON;

SELECT ID,
       NombreUsuario,
       ContrasenaHash,
       Email,
       Rol,
       Estado,
       FechaCreacion,
       FechaModificacion
FROM Usuario
WHERE NombreUsuario = @NombreUsuario;
END

CREATE PROCEDURE sp_BuscarPersonaPorTelefono @Telefono NVARCHAR(15)
AS
BEGIN
    SET
NOCOUNT ON;

SELECT *
FROM Persona
WHERE Telefono = @Telefono;
END;
go
create PROCEDURE sp_GetTodosLosProductos
    AS
BEGIN
SELECT
    ID,
    NombreProducto,
    Genero,
    SegmentoEdad,
    TipoProducto,
    Color,
    Talla,
    UnidadesDisponibles,
    Precio,
    Descripcion,
    ImagenUrl,  -- 🔹 Asegúrate de incluir esta columna
    FechaCreacion,
    FechaModificacion
FROM Producto;
END;
GO
