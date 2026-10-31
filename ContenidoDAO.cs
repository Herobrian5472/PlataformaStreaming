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
                    (titulo, descripcion, anio_lanzamiento, tipo, duracion)
                VALUES
                    (@titulo, @descripcion, @anio, @tipo, @duracion);
                ";

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteTransaction transaccion = conexion.BeginTransaction()) // para poder manejar varias operaciones como una sola y que no valle en alguna tabla
                {
                    try
                    {
                        using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                        {
                            comando.Transaction = transaccion;

                            // Parameters.AddWithValue() vincula cada propiedad con su parametro SQL
                            comando.Parameters.AddWithValue("@titulo", contenido.Titulo);
                            comando.Parameters.AddWithValue("@descripcion", contenido.Descripcion);
                            comando.Parameters.AddWithValue("@anio", contenido.AnioLanzamiento);
                            comando.Parameters.AddWithValue("@tipo", contenido.Tipo);
                            comando.Parameters.AddWithValue("@duracion", contenido.Duracion);

                            comando.ExecuteNonQuery(); // ejecuta el INSERT porque estamos guardando datos, no recuperando filas, Y GUARDA LA PELICULA

                        }

                        // registrar en el historial
                        SqliteCommand comandoId = new SqliteCommand(
                            "SELECT last_insert_rowid();", conexion);

                        comandoId.Transaction = transaccion;

                        int contenidoId = Convert.ToInt32(comandoId.ExecuteScalar()); // executeScalar devuelve el primer valor de la consuta, en este caso, el ID del ultimo registro insertado en esa conexion

                        HistorialDAO historial = new HistorialDAO();

                        historial.RegistrarMovimiento( // guarda la ID, la operacio y lo detalles en Historial
                            conexion,
                            transaccion,
                            contenidoId,
                            "ALTA",
                            $"Se registró el contenido: {contenido.Titulo}"
                        );

                        transaccion.Commit(); // confirmar la transaccion
                    }
                    catch
                    {
                        transaccion.Rollback(); // deshace los cambios si algo sale mal con el try. Ej: funciona INSERT en Contenidos pero no en Historial
                        throw;
                    }
                    
                }
            }

        }

        public bool ModificarContenido(Contenido contenido)
        {
            string sqlAnterior = @"
                SELECT titulo, descripcion, anio_lanzamiento, duracion
                FROM Contenidos
                WHERE contenido_id = @id
                    AND tipo = @tipo
                    AND estado = 1;
            ";

            string sqlActualizar = @"
                UPDATE Contenidos
                SET titulo = @titulo,
                    descripcion = @descripcion,
                    anio_lanzamiento = @anio,
                    duracion = @duracion
                WHERE contenido_id = @id
                    AND tipo = @tipo
                    AND estado = 1;
            ";
            // UPDATE modifica registro existentes

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // obtener los valores anteriores
                        string tituloAnterior;
                        string descripcionAnterior;
                        int anioAnterior;
                        int duracionAnterior;

                        using (SqliteCommand comandoAnterior =
                            new SqliteCommand(sqlAnterior, conexion))
                        {
                            comandoAnterior.Transaction = transaccion;

                            comandoAnterior.Parameters.AddWithValue("@id", contenido.ContenidoId);
                            comandoAnterior.Parameters.AddWithValue("@tipo", contenido.Tipo);

                            using (SqliteDataReader lector = comandoAnterior.ExecuteReader())
                            {
                                if (!lector.Read())
                                {
                                    transaccion.Commit();
                                    return false;
                                }

                                tituloAnterior = lector.GetString(0);
                                descripcionAnterior = lector.IsDBNull(1) ? "" : lector.GetString(1);
                                anioAnterior = lector.GetInt32(2);
                                duracionAnterior = lector.GetInt32(3);
                            }
                        }

                        // actualizar el contenido
                        using (SqliteCommand comando =
                            new SqliteCommand(sqlActualizar, conexion))
                        {
                            comando.Transaction = transaccion;

                            comando.Parameters.AddWithValue("@titulo", contenido.Titulo);
                            comando.Parameters.AddWithValue("@descripcion", contenido.Descripcion);
                            comando.Parameters.AddWithValue("@anio", contenido.AnioLanzamiento);
                            comando.Parameters.AddWithValue("@duracion", contenido.Duracion);
                            comando.Parameters.AddWithValue("@id", contenido.ContenidoId);
                            comando.Parameters.AddWithValue("@tipo", contenido.Tipo);

                            int filasAfectadas = comando.ExecuteNonQuery();

                            if (filasAfectadas == 0)
                            {
                                transaccion.Commit();
                                return false;
                            }
                        }

                        // registrar la modificacion en el historial
                        string detalles =
                            $"Titulo: '{tituloAnterior}' -> '{contenido.Titulo}'. " +
                            $"Descripción: '{descripcionAnterior}' -> '{contenido.Descripcion}'. " +
                            $"Año: '{anioAnterior}' -> '{contenido.AnioLanzamiento}'. " +
                            $"Duración: '{duracionAnterior}' -> '{contenido.Duracion}'. ";

                        HistorialDAO historial = new HistorialDAO();

                        historial.RegistrarMovimiento(
                            conexion,
                            transaccion,
                            contenido.ContenidoId,
                            "MODIFICACION",
                            detalles
                        );

                        transaccion.Commit();

                        return true;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool DarBajaContenido(int id)
        {
            string sqlTitulo = @"
                SELECT titulo
                FROM Contenidos
                WHERE contenido_id = @id
                    AND tipo = @tipo
                    AND estado = 1;
            ";

            string sqlBaja = @"
                UPDATE Contenidos
                SET estado = 0
                WHERE contenido_id = @id
                    AND tipo = @tipo
                    AND estado = 1;
            ";


            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // obtener el titulo del contenido
                        string titulo;

                        using (SqliteCommand comandoTitulo =
                            new SqliteCommand(sqlTitulo, conexion))
                        {
                            comandoTitulo.Transaction = transaccion;

                            comandoTitulo.Parameters.AddWithValue("@id", id);
                            comandoTitulo.Parameters.AddWithValue("@tipo", "Pelicula");

                            object resultado = comandoTitulo.ExecuteScalar();

                            if (resultado == null || resultado == DBNull.Value) // || es un OR, que locura manito
                            {
                                transaccion.Commit();
                                return false;
                            }

                            titulo = Convert.ToString(resultado);
                        }

                        // dar de baja el contenido
                        using (SqliteCommand comando =
                            new SqliteCommand(sqlBaja, conexion))
                        {
                            comando.Transaction = transaccion;

                            comando.Parameters.AddWithValue("@id", id);
                            comando.Parameters.AddWithValue("@tipo", "Pelicula");

                            int filasAfectadas = comando.ExecuteNonQuery();

                            if (filasAfectadas == 0)
                            {
                                transaccion.Commit();
                                return false;
                            }
                        }

                        // registrar la operacion
                        HistorialDAO historial = new HistorialDAO();

                        historial.RegistrarMovimiento(
                            conexion,
                            transaccion,
                            id,
                            "BAJA",
                            $"Se dio de baja el contenido: {titulo}. Estado: activo -> inactivo."
                        );

                        transaccion.Commit();

                        return true;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw; // para mostrar el error
                    }
                }
            }
        }

        public bool DarAltaContenido(int id)
        {
            string sqlTitulo = @"
                SELECT titulo
                FROM Contenidos
                WHERE contenido_id = @id
                    AND tipo = @tipo
                    AND estado = 0;
            ";

            string sqlAlta = @"
                UPDATE Contenidos
                SET estado = 1
                WHERE contenido_id = @id
                    AND tipo = @tipo
                    AND estado = 0;
            ";


            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // obtener el titulo del contenido
                        string titulo;

                        using (SqliteCommand comandoTitulo =
                            new SqliteCommand(sqlTitulo, conexion))
                        {
                            comandoTitulo.Transaction = transaccion;

                            comandoTitulo.Parameters.AddWithValue("@id", id);
                            comandoTitulo.Parameters.AddWithValue("@tipo", "Pelicula");

                            object resultado = comandoTitulo.ExecuteScalar(); // ExecuteScalar busca un valor o null

                            if (resultado == null || resultado == DBNull.Value)
                            {
                                transaccion.Commit();
                                return false;
                            }

                            titulo = Convert.ToString(resultado);
                        }

                        // reactivar el contenido
                        using (SqliteCommand comando =
                            new SqliteCommand(sqlAlta, conexion))
                        {
                            comando.Transaction = transaccion;

                            comando.Parameters.AddWithValue("@id", id);
                            comando.Parameters.AddWithValue("@tipo", "Pelicula");

                            int filasAfectadas = comando.ExecuteNonQuery();

                            if (filasAfectadas == 0)
                            {
                                transaccion.Commit();
                                return false;
                            }
                        }

                        // registrar la reactivacion
                        HistorialDAO historial = new HistorialDAO();

                        historial.RegistrarMovimiento(
                            conexion,
                            transaccion,
                            id,
                            "REACTIVACION",
                            $"Se reactivó el contenido: {titulo}. Estado: inactivo -> activo."
                        );

                        transaccion.Commit();

                        return true;
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
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
            string sqlTitulo = @"
                SELECT titulo
                FROM Contenidos
                WHERE contenido_id = @id
                    AND tipo = @tipo;
            ";

            string sqlEliminar = @"
                DELETE FROM Contenidos
                WHERE contenido_id = @id
                    AND tipo = @tipo;
            ";

            using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
            {
                using (SqliteTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // obtener el título antes de eliminar el contenido
                        string titulo;

                        using (SqliteCommand comandoTitulo =
                            new SqliteCommand(sqlTitulo, conexion))
                        {
                            comandoTitulo.Transaction = transaccion;

                            comandoTitulo.Parameters.AddWithValue("@id", id);
                            comandoTitulo.Parameters.AddWithValue("@tipo", "Pelicula");

                            object resultado = comandoTitulo.ExecuteScalar();

                            if (resultado == null || resultado == DBNull.Value)
                            {
                                transaccion.Commit();
                                return false;
                            }

                            titulo = Convert.ToString(resultado);
                        }

                        // registrar la eliminacion en el historial
                        HistorialDAO historial = new HistorialDAO();

                        historial.RegistrarMovimiento(
                            conexion,
                            transaccion,
                            null,
                            "ELIMINACION",
                            $"Se eliminó definitivamente el contenido ID {id}: {titulo}."
                        );

                        // desvincular los registros anteriores del contenido
                        string sqlHistorial = @"
                            UPDATE Historial
                            SET detalles = 'Contenido ID ' || @id || ' | ' || COALESCE(detalles, ''),
                                contenido_id = NULL
                            WHERE contenido_id = @id;
                        ";

                        using (SqliteCommand comandoHistorial =
                            new SqliteCommand(sqlHistorial, conexion))
                        {
                            comandoHistorial.Transaction = transaccion;
                            comandoHistorial.Parameters.AddWithValue("@id", id);

                            comandoHistorial.ExecuteNonQuery();
                        }

                        // eliminar la relaciones con los géneros
                        string sqlGeneros = @"
                            DELETE FROM ContenidoGenero
                            WHERE contenido_id = @id;
                        ";

                        using (SqliteCommand comandoGeneros =
                            new SqliteCommand(sqlGeneros, conexion))
                        {
                            comandoGeneros.Transaction = transaccion;
                            comandoGeneros.Parameters.AddWithValue("@id", id);

                            comandoGeneros.ExecuteNonQuery();
                        }

                        // eliminar definitivamente el contenido
                        using (SqliteCommand comandoEliminar =
                            new SqliteCommand(sqlEliminar, conexion))
                        {
                            comandoEliminar.Transaction = transaccion;

                            comandoEliminar.Parameters.AddWithValue("@id", id);
                            comandoEliminar.Parameters.AddWithValue("@tipo", "Pelicula");

                            int filasAfectadas = comandoEliminar.ExecuteNonQuery();

                            if (filasAfectadas == 0)
                            {
                                transaccion.Rollback();
                                return false;
                            }
                        }

                        transaccion.Commit();

                        return true;

                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}