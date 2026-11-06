using Microsoft.Data.Sqlite;
using PlataformaStreaming.Modelos;

namespace PlataformaStreaming.Interfaces {
    internal interface IHistorialDAO {
        public List<Historial> ObtenerHistorial();
        public void DescargarHistorial(List<Historial> historial);
        public void RegistrarMovimiento(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int? contenidoId,
            int usuarioId,
            string operacion,
            string detalles);
    }
}
