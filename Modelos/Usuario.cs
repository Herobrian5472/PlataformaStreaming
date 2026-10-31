using System;

namespace PlataformaStreaming.Modelos
{
    public class Usuario //Clase que representa a una persona registrada
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string NombreUsuario { get; set; } = "";
        public string Email { get; set; } = "";
        public string ContraseniaHash { get; set; } = "";
        public string ContraseniaSalt { get; set; } = "";
        public string Rol { get; set; } = "Usuario"; //Asigna el rol común por  defecto
        public DateTime FechaRegistro { get; set; }
        public bool SuscripcionActiva { get; set; } //Indica si la cuenta tiene una suscripcion activa.
    }
}
