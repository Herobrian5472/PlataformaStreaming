using System;
using System.Collections.Generic;
using static PlataformaStreaming.UtilidadesConsola;
using PlataformaStreaming.Modelos;
using PlataformaStreaming.Servicios;
using System.ComponentModel.Design;

namespace PlataformaStreaming
{
    internal class MenuContenidos
    {
        // atributo de la clase
        internal readonly Usuario usuario;

        //constructor de menú segun el usuario
        public MenuContenidos(Usuario usuario)
        {
            this.usuario = usuario;
        }

        // metodo
        public void Mostrar() // 
        {
            // booleano de Administrador para mostrar el menú respectivo
            Autorizacion autorizacion = new Autorizacion();
            bool administrador = autorizacion.EsAdministrador(usuario);

            ContenidoDAO contenidoDAO = new ContenidoDAO(); 

            int opcion = 0;

            if (administrador)
            {

                while (opcion != 9)
                {
                    Console.WriteLine("\n--- MENU DE CONTENIDOS ---");
                    Console.WriteLine("1. Listar películas");
                    Console.WriteLine("2. Agregar película");
                    Console.WriteLine("3. Modificar película");
                    Console.WriteLine("4. Eliminar película");
                    Console.WriteLine("5. Dar de baja una película");
                    Console.WriteLine("6. Ver peliculas dadas de baja");
                    Console.WriteLine("7. Reactivar una pelicula");
                    Console.WriteLine();
                    Console.WriteLine("8. Descargar historial de movimientos");
                    Console.WriteLine("9. Otorgar 'Administrador' a un usuario");
                    Console.WriteLine();
                    Console.WriteLine("0. Cerrar sesión");
                    Console.Write("Seleccione una opcion: ");

                    if (!int.TryParse(Console.ReadLine(), out opcion)) // si no ingresa un número
                    {
                        UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "Ingrese un número válido.");
                        opcion = 0;
                        continue;
                    }

                    switch (opcion)
                    {
                        case 1:
                            {
                                List<Contenido> peliculas = contenidoDAO.ObtenerPeliculas();

                                foreach (Contenido pelicula in peliculas)
                                {
                                    string tiempo;

                                    if (pelicula.Tipo == "Pelicula")
                                    {
                                        tiempo = " minutos";
                                    }
                                    else
                                    {
                                        tiempo = " capítulos";
                                    }
                                    Console.WriteLine(
                                        pelicula.ContenidoId + " - " +
                                        pelicula.Titulo + " (" +
                                        pelicula.AnioLanzamiento + ") " +
                                        pelicula.Tipo + " - " +
                                        pelicula.Duracion +
                                        tiempo + ".");
                                }

                                break;
                            }

                        case 2:
                            {
                                // objeto nueva pelicula
                                Contenido nuevoContenido = new Contenido();

                                // titulo
                                while (true)
                                {
                                    Console.Write("Titulo: ");
                                    nuevoContenido.Titulo = Console.ReadLine() ?? ""; 

                                    if (string.IsNullOrWhiteSpace(nuevoContenido.Titulo))
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "El titulo no puede estar vacío.");
                                        continue;
                                    }
                                    break;
                                }

                                // descripcion
                                while (true)
                                {
                                    Console.Write("Descripción: ");
                                    nuevoContenido.Descripcion = Console.ReadLine() ?? "";

                                    if (string.IsNullOrWhiteSpace(nuevoContenido.Descripcion))
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "El titulo no puede estar vacío.");
                                        break;
                                    }
                                    break;
                                }

                                // año
                                int anio;

                                while (true)
                                {
                                    Console.Write("Año de lanzamiento: ");
                                    string entradaAnio = Console.ReadLine() ?? "";

                                    if (!int.TryParse(entradaAnio, out anio))
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Ingrese un año valido.");
                                        continue;
                                    }
                                    if (anio < 1900 || anio > DateTime.Now.Year)
                                    {
                                        MostrarTexto(ConsoleColor.DarkYellow, "El año está fuera del rango permitido.");
                                        continue;
                                    }
                                    break;
                                }

                                nuevoContenido.AnioLanzamiento = anio;

                                // tipo
                                while (true)
                                {
                                    Console.Write("\n1) Pelicula\n2) Serie\n");
                                    string entradaTipo = Console.ReadLine() ?? "";

                                    if (entradaTipo == "1" || entradaTipo == "2")
                                    {
                                        if (entradaTipo == "1")
                                        {
                                            nuevoContenido.Tipo = "Pelicula";
                                        }
                                        else
                                        {
                                            nuevoContenido.Tipo = "Serie";
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Ingrese un número valido.");
                                        continue;
                                    }
          
                                }

                                // duracion
                                while (true)
                                {
                                    Console.Write("¿Cuánto dura?\n");
                                    MostrarTexto(ConsoleColor.Cyan, "Si es Película son minutos, si es Serie son episodios.");
                                    string entradaDuracion = Console.ReadLine() ?? "";

                                    if (!int.TryParse(entradaDuracion, out int duracion))
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Ingrese un número valido.");
                                        continue;
                                    }

                                    if (duracion <= 0)
                                    {
                                        MostrarTexto(ConsoleColor.DarkYellow, "La duración debe ser mayor a cero.");
                                        continue;
                                    }
                                    nuevoContenido.Duracion = duracion;

                                    break;

                                }

                                // agregar pelicula
                                contenidoDAO.AgregarContenido(nuevoContenido);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"{nuevoContenido.Tipo} guardada correctamente:");
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.WriteLine(
                                        nuevoContenido.Titulo + " (" +
                                        nuevoContenido.AnioLanzamiento + ")");
                                Console.ResetColor();
                                Console.WriteLine();

                                break;
                            }

                        case 3:
                            {
                                List<Contenido> peliculas = contenidoDAO.ObtenerPeliculas(); // obtener peliculas antes de modificar

                                Console.WriteLine("ID de la película a modificar: ");

                                if (!int.TryParse(Console.ReadLine(), out int id))
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "ID inválido.");
                                    break;
                                }

                                Contenido pelicula = null; // creamos el objeto sin ninguna pelicula seleccionada

                                foreach (Contenido p in peliculas)
                                {
                                    if (p.ContenidoId == id) // comparamos el id de cada pelicula con lo que ingresó el usuario
                                    {
                                        pelicula = p; // si el id existe se lo asignamos a pelicula
                                        break;
                                    }
                                }

                                if (pelicula == null)
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Red,
                                        "No existe una película con ese ID."
                                    );

                                    break;
                                }

                                pelicula.ContenidoId = id;

                                Console.Write("Nuevo titulo: ");
                                pelicula.Titulo = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(pelicula.Titulo))
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "El título no puede estar vacío.");
                                    break;
                                }

                                Console.WriteLine("Nueva descripcion: ");
                                pelicula.Descripcion = Console.ReadLine();

                                Console.Write("Nuevo año: ");

                                if (!int.TryParse(Console.ReadLine(), out int anio))
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Año invalido.");
                                    break;
                                }

                                pelicula.AnioLanzamiento = anio;

                                bool modificado = contenidoDAO.ModificarContenido(pelicula);

                                if (modificado)
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.Green, "Película modificada correctamente.");
                                }
                                else
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "No se pudo modificar la película.");
                                }

                                break;

                            }

                        case 4:
                            {
                                while (true)
                                {
                                    List<Contenido> peliculas = contenidoDAO.ObtenerPeliculas();

                                    if (peliculas.Count == 0)
                                    {
                                        UtilidadesConsola.MostrarTexto(
                                            ConsoleColor.DarkYellow,
                                            "No hay películas para eliminar."
                                            );
                                        break;
                                    }

                                    Console.WriteLine("ID de la película a eliminar: ");

                                    if (!int.TryParse(Console.ReadLine(), out int id))
                                    {
                                        UtilidadesConsola.MostrarTexto(
                                            ConsoleColor.Red,
                                            "ID inválido."
                                        );
                                        continue;
                                    }

                                    Contenido pelicula = null;

                                    foreach (Contenido p in peliculas)
                                    {
                                        if (p.ContenidoId == id)
                                        {
                                            pelicula = p;
                                            break;
                                        }
                                    }
                                    if (pelicula == null)
                                    {
                                        UtilidadesConsola.MostrarTexto(
                                            ConsoleColor.Red,
                                            "ID inválido."
                                            );
                                        continue;
                                    }

                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine(
                                        "Pelicula seleccionada: " +
                                        pelicula.Titulo + " (" +
                                        pelicula.AnioLanzamiento + ")"
                                        );
                                    Console.ResetColor();

                                    while (true)
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow, "¿Seguro que desea eliminarla? (S/N): ");
                                        string confirmacion = (Console.ReadLine() ?? "").Trim().ToUpper();

                                        if (confirmacion == "S")
                                        {
                                            bool eliminado = contenidoDAO.EliminarContenido(id);

                                            if (eliminado)
                                            {
                                                UtilidadesConsola.MostrarTexto(
                                                    ConsoleColor.Green,
                                                    "Película eliminada correctamente."
                                                    );
                                                break;
                                            }
                                            else
                                            {
                                                UtilidadesConsola.MostrarTexto(
                                                    ConsoleColor.Red,
                                                    "No se pudo eliminar la película."
                                                    );
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            UtilidadesConsola.MostrarTexto(
                                                ConsoleColor.Yellow,
                                                "Ingrese S para confirmar o N para cancelar.\n"
                                            );

                                            continue;
                                        }
                                    }

                                    break;

                                }
                                break;

                            }

                        case 5:
                            {
                                List<Contenido> peliculas = contenidoDAO.ObtenerPeliculas();

                                if (peliculas.Count == 0)
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.DarkYellow,
                                        "No hay películas para dar de Baja."
                                        );
                                    break;
                                }

                                Console.WriteLine("ID de la película que desea dar de baja: ");

                                if (!int.TryParse(Console.ReadLine(), out int id))
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Red,
                                        "ID inválido."
                                    );
                                    break;
                                }

                                bool resultado = contenidoDAO.DarBajaContenido(id);

                                if (resultado)
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Green,
                                        "Baja realizada correctamente.");
                                    break;
                                }
                                else
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Red,
                                        "No se pudo dar de baja el contenido.");
                                    break;
                                }
                            }

                        case 6:
                            {
                                List<Contenido> peliculasBajas = contenidoDAO.MostrarBajas();

                                if (peliculasBajas.Count == 0)
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Yellow,
                                        "No hay películas dadas de baja."
                                    );
                                }
                                else
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Yellow,
                                        "--- PELICULAS DADAS DE BAJA ---"
                                        );

                                    foreach (Contenido pelicula in peliculasBajas)
                                    {
                                        Console.WriteLine(
                                            pelicula.ContenidoId + " - " +
                                            pelicula.Titulo + " (" +
                                            pelicula.AnioLanzamiento + ")"
                                        );
                                    }
                                }

                                break;
                            }

                        case 7:
                            {
                                List<Contenido> peliculasBajas = contenidoDAO.MostrarBajas();

                                if (peliculasBajas.Count == 0)
                                {
                                    MostrarTexto(
                                        ConsoleColor.Yellow,
                                        "No hay películas dadas de baja."
                                    );
                                }
                                else
                                {
                                    MostrarTexto(
                                        ConsoleColor.Yellow,
                                        "--- PELICULAS PARA REACTIVAR---"
                                    );

                                    foreach (Contenido pelicula in peliculasBajas)
                                    {
                                        Console.WriteLine(
                                            pelicula.ContenidoId + " - " +
                                            pelicula.Titulo + " (" +
                                            pelicula.AnioLanzamiento + ")"
                                        );
                                    }

                                    Console.WriteLine("\nID de la película a reactivar: ");

                                    if (int.TryParse(Console.ReadLine(), out int id))
                                    {
                                        bool resultado = contenidoDAO.DarAltaContenido(id);

                                        if (resultado)
                                        {
                                            MostrarTexto(
                                                ConsoleColor.Green,
                                                "Película reactivada correctamente."
                                            );
                                        }
                                        else
                                        {
                                            MostrarTexto(
                                                ConsoleColor.Red,
                                                "No se encontró una película dada de baja con ese ID."
                                            );
                                        }
                                    }
                                    else
                                    {
                                        MostrarTexto(
                                            ConsoleColor.Red,
                                            "ID inválido."
                                        );
                                    }
                                }

                                break;
                            }

                        case 8:
                            var historial = HistorialDAO.ObtenerHistorial();
                            HistorialDAO.DescargarHistorial(historial);
                            break;

                        case 9:
                            Console.WriteLine("Programando operación...");
                            break;


                        case 0:
                            UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow, "Cerrando sesión...");
                            return;

                        default:
                            UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "Opcion no válida.");
                            break;
                    }
                }
            }
            else
            {
                while (opcion != 4)
                {
                    Console.WriteLine("\n--- MENU DE CONTENIDOS ---");
                    Console.WriteLine("1. Listar películas");
                    Console.WriteLine("2. Agregar película");
                    Console.WriteLine("3. Modificar película");
                    Console.WriteLine();
                    Console.WriteLine("4. Cerrar sesión");
                    Console.Write("Seleccione una opcion: ");

                    if (!int.TryParse(Console.ReadLine(), out opcion)) // si no ingresa un número
                    {
                        UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "Ingrese un número válido.");
                        opcion = 0;
                        continue;
                    }

                    switch (opcion)
                    {
                        case 1:
                            {
                                List<Contenido> peliculas = contenidoDAO.ObtenerPeliculas();

                                foreach (Contenido pelicula in peliculas)
                                {
                                    string tiempo;

                                    if (pelicula.Tipo == "Pelicula")
                                    {
                                        tiempo = " minutos";
                                    }
                                    else
                                    {
                                        tiempo = " capítulos";
                                    }
                                    Console.WriteLine(
                                        pelicula.ContenidoId + " - " +
                                        pelicula.Titulo + " (" +
                                        pelicula.AnioLanzamiento + ") " +
                                        pelicula.Tipo + " - " +
                                        pelicula.Duracion +
                                        tiempo + ".");
                                }

                                break;
                            }

                        case 2:
                            {
                                // objeto nueva pelicula
                                Contenido nuevoContenido = new Contenido();

                                // titulo
                                while (true)
                                {
                                    Console.Write("Titulo: ");
                                    nuevoContenido.Titulo = Console.ReadLine() ?? "";

                                    if (string.IsNullOrWhiteSpace(nuevoContenido.Titulo))
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "El titulo no puede estar vacío.");
                                        continue;
                                    }
                                    break;
                                }

                                // descripcion
                                while (true)
                                {
                                    Console.Write("Descripción: ");
                                    nuevoContenido.Descripcion = Console.ReadLine() ?? "";

                                    if (string.IsNullOrWhiteSpace(nuevoContenido.Descripcion))
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "El titulo no puede estar vacío.");
                                        break;
                                    }
                                    break;
                                }

                                // año
                                int anio;

                                while (true)
                                {
                                    Console.Write("Año de lanzamiento: ");
                                    string entradaAnio = Console.ReadLine() ?? "";

                                    if (!int.TryParse(entradaAnio, out anio))
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Ingrese un año valido.");
                                        continue;
                                    }
                                    if (anio < 1900 || anio > DateTime.Now.Year)
                                    {
                                        MostrarTexto(ConsoleColor.DarkYellow, "El año está fuera del rango permitido.");
                                        continue;
                                    }
                                    break;
                                }

                                nuevoContenido.AnioLanzamiento = anio;

                                // tipo
                                while (true)
                                {
                                    Console.Write("\n1) Pelicula\n2) Serie\n");
                                    string entradaTipo = Console.ReadLine() ?? "";

                                    if (entradaTipo == "1" || entradaTipo == "2")
                                    {
                                        if (entradaTipo == "1")
                                        {
                                            nuevoContenido.Tipo = "Pelicula";
                                        }
                                        else
                                        {
                                            nuevoContenido.Tipo = "Serie";
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Ingrese un número valido.");
                                        continue;
                                    }

                                }

                                // duracion
                                while (true)
                                {
                                    Console.Write("¿Cuánto dura?\n");
                                    MostrarTexto(ConsoleColor.Cyan, "Si es Película son minutos, si es Serie son episodios.");
                                    string entradaDuracion = Console.ReadLine() ?? "";

                                    if (!int.TryParse(entradaDuracion, out int duracion))
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Ingrese un número valido.");
                                        continue;
                                    }

                                    if (duracion <= 0)
                                    {
                                        MostrarTexto(ConsoleColor.DarkYellow, "La duración debe ser mayor a cero.");
                                        continue;
                                    }
                                    nuevoContenido.Duracion = duracion;

                                    break;

                                }

                                // agregar pelicula
                                contenidoDAO.AgregarContenido(nuevoContenido);

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"{nuevoContenido.Tipo} guardada correctamente:");
                                Console.BackgroundColor = ConsoleColor.Gray;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.WriteLine(
                                        nuevoContenido.Titulo + " (" +
                                        nuevoContenido.AnioLanzamiento + ")");
                                Console.ResetColor();
                                Console.WriteLine();

                                break;
                            }

                        case 3:
                            {
                                List<Contenido> peliculas = contenidoDAO.ObtenerPeliculas(); // obtener peliculas antes de modificar

                                Console.WriteLine("ID de la película a modificar: ");

                                if (!int.TryParse(Console.ReadLine(), out int id))
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "ID inválido.");
                                    break;
                                }

                                Contenido pelicula = null; // creamos el objeto sin ninguna pelicula seleccionada

                                foreach (Contenido p in peliculas)
                                {
                                    if (p.ContenidoId == id) // comparamos el id de cada pelicula con lo que ingresó el usuario
                                    {
                                        pelicula = p; // si el id existe se lo asignamos a pelicula
                                        break;
                                    }
                                }

                                if (pelicula == null)
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Red,
                                        "No existe una película con ese ID."
                                    );

                                    break;
                                }

                                pelicula.ContenidoId = id;

                                Console.Write("Nuevo titulo: ");
                                pelicula.Titulo = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(pelicula.Titulo))
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "El título no puede estar vacío.");
                                    break;
                                }

                                Console.WriteLine("Nueva descripcion: ");
                                pelicula.Descripcion = Console.ReadLine();

                                Console.Write("Nuevo año: ");

                                if (!int.TryParse(Console.ReadLine(), out int anio))
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Año invalido.");
                                    break;
                                }

                                pelicula.AnioLanzamiento = anio;

                                bool modificado = contenidoDAO.ModificarContenido(pelicula);

                                if (modificado)
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.Green, "Película modificada correctamente.");
                                }
                                else
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "No se pudo modificar la película.");
                                }

                                break;

                            }

                        case 4:
                            UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow, "Cerrando sesión...");
                            return;

                        default:
                            UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "Opcion no válida.");
                            break;
                    }
                }
            }
        }
    }
}
