using PlataformaStreaming.Modelos;

namespace PlataformaStraming.Interfaces {
    internal interface IUsuarioDAO {
        public bool ExisteUsuario(string nombreUsuario);
        public bool ExisteEmail(string email);
        public void RegistrarUsuario(Usuario usuario);
        public Usuario? BuscarPorNombreUsuario(string nombreUsuario);
        public bool CambiarRol(string nombreUsuario, string nuevoRol);
    }
}
