using Microsoft.Data.Sqlite;

namespace PlataformaStreaming
{
    internal class HistorialDAO
    {
        public void RegistrarMovimiento(
            SqliteConnection conexion,
            SqliteTransaction transaccion,
            int? contenidoId, // por si es nulo
            string operacion, 
            string detalles)
        {
            string sql = @"
                INSERT INTO Historial 
                    (contenido_id, operacion, detalles)
                VALUES 
                    (@contenidoId, @operacion, @detalles);
            ";

            // contenidoId: identifica la pelicula o serie afectada
            // operacion: indica que accion se realizó, ej ALTA o BAJA
            // detalles: guarda una descripcion del cambio
            // fecha_hora no se envia poruqe SQLite la completa automaticamente

            
            using (SqliteCommand comando = new SqliteCommand(sql, conexion))
            {
                comando.Transaction = transaccion;

                comando.Parameters.AddWithValue(
                    "@contenidoId",
                    contenidoId.HasValue ? (object)contenidoId.Value : DBNull.Value
                );

                comando.Parameters.AddWithValue("@operacion", operacion); 
                comando.Parameters.AddWithValue("@detalles", detalles);

                comando.ExecuteNonQuery();
            }
            
        }
    }
}