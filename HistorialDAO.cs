using Microsoft.Data.Sqlite;

namespace PlataformaStreaming
{
    internal class HistorialDAO
    {
        public void RegistrarMovimiento(int contenidoId, string operacion, string detalles)
        {
            string sql = @"
                INSERT INTO Historial (contenido_id, operacion, detalles)
                VALUES (@contenidoId, @operacion, @detalles);
            ";

            // contenidoId: identifica la pelicula o serie afectada
            // operacion: indica que accion se realizó, ej ALTA o BAJA
            // detalles: guarda una descripcion del cambio
            // fecha_hora no se envia poruqe SQLite la completa automaticamente

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@contenidoId", contenidoId);
                    comando.Parameters.AddWithValue("@operacion", operacion); 
                    comando.Parameters.AddWithValue("@detalles", detalles);

                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}