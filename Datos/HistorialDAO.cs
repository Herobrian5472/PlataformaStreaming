using Microsoft.Data.Sqlite;
using PlataformaStraming.Modelos;

namespace PlataformaStreaming
{
    internal class HistorialDAO
    {
        public static List<Historial> ObtenerHistorial() {
            List<Historial> logs = [];

            string sql = @"
                SELECT historial_id, contenido_id, operacion, fecha_hora, detalles, usuario_id
                FROM Historial;
                ";

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion()) {
                conexion.Open();

                using (SqliteCommand comando = new SqliteCommand(sql, conexion)) {
                    using (SqliteDataReader lector = comando.ExecuteReader()) {

                        while (lector.Read()) {
                            Historial historial = new Historial {
                                HistorialId = lector.GetInt32(0),
                                ContenidoId = lector.IsDBNull(1) ? 0 : lector.GetInt32(1),
                                Operacion = lector.GetString(2),
                                FechaHora = lector.GetString(3),
                                Detalles = lector.GetString(4),
                                UsuarioId = lector.IsDBNull(5) ? 0 : lector.GetInt32(5)
                            };

                            logs.Add(historial);
                        }
                    }
                }
            }

            if (logs.Count == 0) {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow, "Los registros del historial están vacíos.");
            }

            return logs;
        }

        public static void DescargarHistorial(List<Historial> historial) {
            string Ruta = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "historial.txt"));

            try {

                if (!File.Exists(Ruta)) {
                    File.Create(Ruta).Close();
                }

                List<string> lineas = [];

                foreach (Historial h in historial) {
                    lineas.Add($"{h.HistorialId} - {h.ContenidoId} - {h.Operacion} - {h.FechaHora} - {h.Detalles} - {h.UsuarioId}");
                }
                File.WriteAllLines(Ruta, lineas);

                UtilidadesConsola.MostrarTexto(ConsoleColor.Green, $"El historial se descargó correctamente en: {Ruta}");
            } catch (Exception ex) {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "Ocurrió un error inesperado, el historial no fue descargado.");
            }
        }
                
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
