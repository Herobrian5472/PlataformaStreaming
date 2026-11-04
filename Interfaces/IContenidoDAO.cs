using PlataformaStreaming;

namespace PlataformaStraming.Interfaces {
    internal interface IContenidoDAO {
        public List<Contenido> ObtenerPeliculas();
        public void AgregarContenido(Contenido contenido);
        public bool ModificarContenido(Contenido contenido);
        public bool DarBajaContenido(int id);
        public bool DarAltaContenido(int id);
        public List<Contenido> MostrarBajas();
        public bool EliminarContenido(int id);
    }
}
