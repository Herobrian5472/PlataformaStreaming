using Microsoft.Data.Sqlite;
using PlataformaStreaming.Interfaces;
using PlataformaStreaming.Modelos;

namespace PlataformaStreaming
{
    internal class HistorialDAO : IHistorialDAO
    {
        public List<Historial> ObtenerHistorial() {
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

        public void DescargarHistorial(List<Historial> historial) {
            string Ruta = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Archivos", "historial.txt"));

            try {

                if (!File.Exists(Ruta)) {
                    File.Create(Ruta).Close();
                }

                List<string> lineas = [];

                lineas.Add("================================================================================");
                lineas.Add("                       HISTORIAL - PLATAFORMA STREAMING");
                lineas.Add("================================================================================");
                lineas.Add("");

                lineas.Add(
                    "ID".PadRight(6) +
                    "CONTENIDO".PadRight(12) +
                    "OPERACION".PadRight(18) +
                    "FECHA Y HORA".PadRight(23) +
                    "DETALLES"
                );

                lineas.Add(new string('-', 110));

                foreach (Historial h in historial) {
                    lineas.Add(
                        h.HistorialId.ToString().PadRight(6) +
                        h.ContenidoId.ToString().PadRight(12) +
                        h.Operacion.ToString().PadRight(18) +
                        h.FechaHora.ToString().PadRight(23) +
                        h.Detalles +
                        " | Usuario: " + h.UsuarioId
                    );

                    lineas.Add("");
                }

                lineas.Add("================================================================================");

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
