using PlataformaStreaming;

namespace PlataformaStreaming.Interfaces {
    internal interface IContenidoDAO {
        public List<Contenido> ObtenerPeliculas();
        public List<Contenido> ObtenerSeries();
        public List<Contenido> ObtenerTodosActivos();
        public List<Contenido> BuscarPorTitulo(string tituloBuscado);
        public void AgregarContenido(Contenido contenido, int usuarioId);
        public bool ModificarContenido(Contenido contenido, int usuarioId);
        public bool DarBajaContenido(int id, int usuarioId);
        public bool DarAltaContenido(int id, int usuarioId);
        public List<Contenido> MostrarBajas();
        public bool EliminarContenido(int id, int usuarioId);
    }
}
