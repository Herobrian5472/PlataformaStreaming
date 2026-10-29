namespace PlataformaStreaming
{
    internal class Contenido
    {
        public int ContenidoId { get; set; } // Las propiedades { get; set; } permiten consultar y asignar esos valores desde otras partes del programa.
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int AnioLanzamiento { get; set; }
        public string Tipo { get; set; }
        public int Duracion { get; set; }
    }
}
