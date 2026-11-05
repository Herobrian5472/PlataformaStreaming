using PlataformaStreaming.Modelos;

namespace PlataformaStreaming.Interfaces {
    internal interface IUsuarioDAO {
        public bool ExisteUsuario(string nombreUsuario);
        public bool ExisteEmail(string email);
        public void RegistrarUsuario(Usuario usuario);
        public Usuario? BuscarPorNombreUsuario(string nombreUsuario);
        public bool CambiarRol(string nombreUsuario, string nuevoRol);
        public List<Usuario> ObtenerUsuariosPorRol(string rol);
        public int ContarAdministradores();
    }
}
