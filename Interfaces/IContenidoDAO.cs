using PlataformaStreaming;

namespace PlataformaStreaming.Interfaces {
    internal interface IContenidoDAO {
        public List<Contenido> ObtenerPeliculas();
        public List<Contenido> ObtenerSeries();
        public List<Contenido> ObtenerTodosActivos();
        public List<Contenido> BuscarPorTitulo(string tituloBuscado);
        public void AgregarContenido(Contenido contenido);
        public bool ModificarContenido(Contenido contenido);
        public bool DarBajaContenido(int id);
        public bool DarAltaContenido(int id);
        public List<Contenido> MostrarBajas();
        public bool EliminarContenido(int id);
    }
}
