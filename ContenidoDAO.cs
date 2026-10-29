using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace PlataformaStreaming
{
    internal class ContenidoDAO
    {
        public List<Contenido> ObtenerPeliculas() // lista de objetos 'Contenido'
        {
            List<Contenido> peliculas = new List<Contenido>();

            string sql = @"
                SELECT contenido_id, titulo, descripcion, anio_lanzamiento, tipo, duracion
                FROM Contenidos
                WHERE tipo = @tipo
                    AND estado = 1;
            ";

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion()) // reutilizar conexion de ConexionDB
            {
                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@tipo", "Pelicula"); // Parameters y AddWithValue son de Microsoft.Data.Sqlite -> tipo es el marcador de la consulta SQL y le asignamos el valor Película

                    using (SqliteDataReader lector = comando.ExecuteReader()) // objeto lector de tipo SqliteDataReader (lee resultados del SELECT fila por fila)
                    {

                        while (lector.Read())
                        {
                            Contenido contenido = new Contenido();

                            contenido.ContenidoId = lector.GetInt32(0); // GetInt32 obtiene el valor de la columna 0 y lo devuelve como int
                            contenido.Titulo = lector.GetString(1);
                            contenido.Descripcion = lector.IsDBNull(2) ? "" : lector.GetString(2); // si el valor es Null lo deja vacío "", sino toma el texto
                            contenido.AnioLanzamiento = lector.IsDBNull(3) ? 0 : lector.GetInt32(3); // el ? pregunta si cumple la condicion (es Nulo?), si lo hace (true) deja un 0, sino, toma el valor como Int32
                            contenido.Tipo = lector.GetString(4);
                            contenido.Duracion = lector.IsDBNull(5) ? 0 : lector.GetInt32(5);

                            peliculas.Add(contenido); // agregar cada pelicula encontrada a la lista

                        }
                    }
                }
            }

            // mostrar mensaje si no hay peliculas registradas
            if (peliculas.Count == 0) // Count es un len()
            {
                UtilidadesConsola.MostrarTexto(
                    ConsoleColor.Yellow,
                    "No hay películas registradas."
                );

            }
            
            return peliculas; // devuelve la lista a quien llamó al método ObtenerPeliculas()
            
            
        }

        // void significa que el metodo no devuelve un resultado
        public void AgregarContenido(Contenido contenido) // recibe un objeto con los datos de una pelicula o serie
        {
            string sql = @"
                INSERT INTO Contenidos
                    (titulo, descripcion, anio_lanzamiento, tipo)
                VALUES
                    (@titulo, @descripcion, @anio, @tipo);
                ";

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                {
                    // Parameters.AddWithValue() vincula cada propiedad con su parametro SQL
                    comando.Parameters.AddWithValue("@titulo", contenido.Titulo);
                    comando.Parameters.AddWithValue("@descripcion", contenido.Descripcion);
                    comando.Parameters.AddWithValue("@anio", contenido.AnioLanzamiento);
                    comando.Parameters.AddWithValue("@tipo", contenido.Tipo);

                    comando.ExecuteNonQuery(); // ejecuta el INSERT porque estamos guardando datos, no recuperando filas, Y GUARDA LA PELICULA

                    // registrar en el historial
                    SqliteCommand comandoId = new SqliteCommand(
                        "SELECT last_insert_rowid();", conexion);

                    int contenidoId = Convert.ToInt32(comandoId.ExecuteScalar()); // executeScalar devuelve el primer valor de la consuta, en este caso, el ID del ultimo registro insertado en esa conexion

                    HistorialDAO historial = new HistorialDAO();

                    historial.RegistrarMovimiento( // guarda la ID, la operacio y lo detalles en Historial
                        contenidoId,  
                        "ALTA",
                        $"Se registró el contenido: {contenido.Titulo}"
                    );
                }
            }

        }

        public bool ModificarContenido(Contenido contenido)
        {
            string sql = @"
                UPDATE Contenidos
                SET titulo = @titulo,
                    descripcion = @descripcion,
                    anio_lanzamiento = @anio
                WHERE contenido_id = @id
                    AND tipo = @tipo
                    AND estado = 1;
            ";
            // UPDATE modifica registro existentes

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@titulo", contenido.Titulo);
                    comando.Parameters.AddWithValue("@descripcion", contenido.Descripcion);
                    comando.Parameters.AddWithValue("@anio", contenido.AnioLanzamiento);
                    comando.Parameters.AddWithValue("@id", contenido.ContenidoId);
                    comando.Parameters.AddWithValue("@tipo", "Pelicula");

                    return comando.ExecuteNonQuery() > 0; // permite saber si se modificó alguna fila (devuelve bool, false si no encontró el id)
                }
            }
        }

        public bool DarBajaContenido(int id)
        {
            string sql = @"
                UPDATE Contenidos
                SET estado = 0
                WHERE contenido_id = @id
                    AND tipo = @tipo
                    AND estado = 1;
            ";


            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@tipo", "Pelicula");

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DarAltaContenido(int id)
        {
            string sql = @"
                UPDATE Contenidos
                SET estado = 1
                WHERE contenido_id = @id
                    AND tipo = @tipo
                    AND estado = 0;
            ";


            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@tipo", "Pelicula");

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Contenido> MostrarBajas()
        {
            List<Contenido> peliculas = new List<Contenido>();

            string sql = @"
                SELECT contenido_id, titulo, descripcion,
                    anio_lanzamiento, tipo, duracion
                FROM Contenidos
                WHERE tipo = @tipo
                    AND estado = 0;
            ";

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@tipo", "Pelicula");

                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Contenido contenido = new Contenido();

                            contenido.ContenidoId = lector.GetInt32(0);
                            contenido.Titulo = lector.GetString(1);
                            contenido.Descripcion = lector.IsDBNull(2) ? "" : lector.GetString(2);
                            contenido.AnioLanzamiento = lector.IsDBNull(3) ? 0 : lector.GetInt32(3);
                            contenido.Tipo = lector.GetString(4);
                            contenido.Duracion = lector.IsDBNull(5) ? 0 : lector.GetInt32(5);

                            peliculas.Add(contenido);
                        }
                    }
                }
            }

            return peliculas;

        }

        public bool EliminarContenido(int id) // temporal, lo ideal es marcar una película como BAJADA o INACTIVA
        {
            string sql = @"
                DELETE FROM Contenidos
                WHERE contenido_id = @id
                    AND tipo = @tipo;
                ";

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@tipo", "Pelicula");

                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

    }
}