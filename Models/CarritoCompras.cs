namespace K_F_ClothingStore.Models {
    public class CarritoCompras {
        public int ID { get; set; }
        public int ClienteID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAgregado { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public CarritoCompras()
        {
            
        }
        public CarritoCompras(int id, int clienteId, int productoId, int cantidad, DateTime fechaAgregado)
        {
            ID = id;
            ClienteID = clienteId;
            ProductoID = productoId;
            Cantidad = cantidad;
            FechaAgregado = fechaAgregado;
        }
    }
}
