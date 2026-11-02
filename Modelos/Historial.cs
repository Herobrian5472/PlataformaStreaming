namespace PlataformaStraming.Modelos {
    internal class Historial {
        public int HistorialId { get; set; }
        public int? ContenidoId { get; set; }
        public string Operacion { get; set; }
        public string FechaHora { get; set; }
        public string? Detalles { get; set; }
        public int? UsuarioId { get; set; }
    }
}
