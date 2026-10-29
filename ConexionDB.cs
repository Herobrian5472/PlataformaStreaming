using Microsoft.Data.Sqlite;  // permite usar la clase SQLite instalada con NuGet
using System.IO;

namespace PlataformaStreaming // mantiene la clase dentro del espacio de nombres del proyecto
{
    internal class ConexionDB
    {
        private static string rutaDB = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "streaming.db"));

        private static string cadenaConexion = $"Data Source={rutaDB}";

        public static SqliteConnection ObtenerConexion() // ObtenerConexion() crea la conexion y la abre
        {
            SqliteConnection conexion = new SqliteConnection(cadenaConexion);
            conexion.Open();

            return conexion; // devuelve la conexion abierta para poder usarla en otra parte del programa
        }
    }
}

// Si streaming.db no existe, SQLite lo crea automaticamente al abrir la conexión