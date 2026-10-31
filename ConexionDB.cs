using Microsoft.Data.Sqlite;  // permite usar la clase SQLite instalada con NuGet
using System.IO;

namespace PlataformaStreaming // mantiene la clase dentro del espacio de nombres del proyecto
{
    internal class ConexionDB
    {
        public static SqliteConnection ObtenerConexion()
        {
            string rutaDB = Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory,
                    "..", "..", "..",
                    "streaming.db"
                )
            );

            string cadenaConexion = $"Data Source={rutaDB}";

            return new SqliteConnection(cadenaConexion);

        }

        // al iniciar la conexion Juli creó la tabla de Usuarios
        public static void InicializarBaseDeDatos()
        {
            using (SqliteConnection conexion = ObtenerConexion())
            {
                conexion.Open();

                string consultaUsuarios = @"
                    CREATE TABLE IF NOT EXISTS Usuarios
                    (
                        usuario_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Apellido TEXT NOT NULL,
                        NombreUsuario TEXT NOT NULL UNIQUE,
                        Email TEXT NOT NULL UNIQUE,
                        ContraseniaHash TEXT NOT NULL,
                        ContraseniaSalt TEXT NOT NULL,
                        Rol TEXT NOT NULL DEFAULT 'Usuario',
                        FechaRegistro TEXT NOT NULL,
                        SuscripcionActiva INTEGER NOT NULL DEFAULT 0
                    );";

                using (SqliteCommand comando = new SqliteCommand(
                    consultaUsuarios, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                // hay que hacer lo mismo que ella pero con las tablas Contenido e Historial, por si el profe borra streamind.db
            }
        }
    }
}

// Si streaming.db no existe, SQLite lo crea automaticamente al abrir la conexión