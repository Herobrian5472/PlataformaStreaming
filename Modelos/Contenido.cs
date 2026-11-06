namespace PlataformaStreaming
{
    internal abstract class Contenido
    {
        public int ContenidoId { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public int AnioLanzamiento { get; set; }

        public string Tipo { get; protected set; }

        public string Genero { get; set; } = string.Empty;

        public int Duracion { get; set; }

        // Cada clase hija deberá implementar este método.
        public abstract string ObtenerUnidadDuracion();

        protected Contenido(string tipo)
        {
            Tipo = tipo;
        }
    }

}
