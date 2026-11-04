using Microsoft.Data.Sqlite;
using PlataformaStraming.Modelos;

namespace PlataformaStraming.Interfaces {
    internal interface IHistorialDAO {
        public List<Historial> ObtenerHistorial();
        public void DescargarHistorial(List<Historial> historial);
        public void RegistrarMovimiento(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int? contenidoId,
            string operacion,
            string detalles);
    }
}
