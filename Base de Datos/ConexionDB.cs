using Microsoft.Data.Sqlite;  // permite usar la clase SQLite instalada con NuGet
using PlataformaStreaming.Datos;
using PlataformaStreaming.Servicios;
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
                    "Base de Datos" , "streaming.db"
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

                // TABLA USUARIOS
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

                // TABLA CONTENIDOS
                string consultaContenidos = @"
                    CREATE TABLE IF NOT EXISTS Contenidos
                    (
                        contenido_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        titulo TEXT NOT NULL,
                        descripcion TEXT,
                        anio_lanzamiento INTEGER,
                        tipo TEXT NOT NULL,
                        genero TEXT NOT NULL DEFAULT '',
                        estado INTEGER NOT NULL,
                        duracion INTEGER NOT NULL
                    );";

                using (SqliteCommand comando = new SqliteCommand(consultaContenidos, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                // TABLA HISTORIAL

                string consultaHistorial = @"
                    CREATE TABLE IF NOT EXISTS Historial
                    (
                        historial_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        contenido_id INTEGER,
                        operacion TEXT NOT NULL,
                        fecha_hora TIME NOT NULL DEFAULT (datetime('now', 'localtime')),
                        detalles TEXT,
                        usuario_id INTEGER,
                        FOREIGN KEY (contenido_id) REFERENCES Contenidos(contenido_id),
                        FOREIGN KEY (usuario_id) REFERENCES Usuarios(usuario_id)
                    );";

                using (SqliteCommand comando = new SqliteCommand(consultaHistorial, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                // hay que hacer lo mismo que ella pero con las tablas Contenido e Historial, por si el profe borra streamind.db
            }

            // Verificar si existe el usuario ADMINISTRADOR:

            UsuarioDAO usuarioDAO = new();

            if (!usuarioDAO.ExisteUsuario("ADMINISTRADOR")) {

                using (SqliteConnection conexion = ObtenerConexion()) {
                    conexion.Open();

                    Autenticacion autenticacion = new(usuarioDAO);

                    autenticacion.Registrar(
                        "Administrador",
                        "Administrador",
                        "ADMINISTRADOR",
                        "administrador@admin.com",
                        "Admin123");

                    usuarioDAO.CambiarRol("ADMINISTRADOR", "Administrador");
                }
            }

        }
    }
}

// Si streaming.db no existe, SQLite lo crea automaticamente al abrir la conexión
