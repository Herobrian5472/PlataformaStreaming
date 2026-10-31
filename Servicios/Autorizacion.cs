using PlataformaStreaming.Modelos;

namespace PlataformaStreaming.Servicios
{
    public class Autorizacion
    {
        public bool EsAdministrador(Usuario? usuario)
        {
            return usuario != null && string.Equals(usuario.Rol, "Administrador", StringComparison.OrdinalIgnoreCase);
        }

        
    }
}
