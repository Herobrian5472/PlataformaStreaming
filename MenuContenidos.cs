using System;
using System.Collections.Generic;
using static PlataformaStreaming.UtilidadesConsola;

namespace PlataformaStreaming
{
    internal class MenuContenidos
    {
        public void Mostrar() // 
        {
            ContenidoDAO contenidoDAO = new ContenidoDAO(); 

            int opcion = 0;

            while (opcion != 7)
            {
                Console.WriteLine("\n--- MENU DE CONTENIDOS ---");
                Console.WriteLine("1. Listar películas");
                Console.WriteLine("2. Agregar película");
                Console.WriteLine("3. Modificar película");
                Console.WriteLine("4. Eliminar película");
                Console.WriteLine("5. Dar de baja una película");
                Console.WriteLine("6. Ver peliculas dadas de baja");
                Console.WriteLine("7. Reactivar una pelicula");
                Console.WriteLine("8. Volver");
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
                            Console.WriteLine(
                                pelicula.ContenidoId + " - " +
                                pelicula.Titulo + " (" +
                                pelicula.AnioLanzamiento + ")");
                        }

                        break;
                    }

                    case 2:
                    { 
                        // objeto nueva pelicula
                        Contenido nuevoContenido = new Contenido();

                        // titulo
                        Console.Write("Titulo: ");
                        nuevoContenido.Titulo = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(nuevoContenido.Titulo))
                        {
                                UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "El titulo no puede estar vacío.");
                            break;
                        }

                        // descripcion
                        Console.Write("Descripcion: ");
                        nuevoContenido.Descripcion = Console.ReadLine();

                        // año
                        Console.Write("Año de lanzamiento: ");
                        string entradaAnio = Console.ReadLine();

                        if (!int.TryParse(entradaAnio, out int anio))
                        {
                                UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Ingrese un año valido.");
                            break;
                        }

                        nuevoContenido.AnioLanzamiento = anio;

                        // tipo
                        nuevoContenido.Tipo = "Pelicula";

                        // agregar pelicula
                        contenidoDAO.AgregarContenido(nuevoContenido);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Película guardada correctamente:");
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
                            List<Contenido> peliculas = contenidoDAO.ObtenerPeliculas();

                            Console.WriteLine("ID de la película a eliminar: ");

                            if (!int.TryParse(Console.ReadLine(), out int id))
                            {
                                UtilidadesConsola.MostrarTexto(
                                    ConsoleColor.Red,
                                    "ID inválido."
                                );
                                break;
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
                                    "No existe una película con ese ID."
                                    );
                                break;
                            }

                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine(
                                "Pelicula seleccionada: " +
                                pelicula.Titulo + " (" +
                                pelicula.AnioLanzamiento + ")"
                                );
                            Console.ResetColor();

                            UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow, "¿Seguro que desea eliminarla? (S/N): ");
                            string confirmacion = Console.ReadLine();

                            if (confirmacion != null && confirmacion.Trim().ToUpper() == "S")
                            {
                                bool eliminado = contenidoDAO.EliminarContenido(id);

                                if (eliminado)
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Green,
                                        "Película eliminada correctamente."
                                        );
                                }
                                else
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Red,
                                        "No se pudo elimianr la película."
                                        );
                                }
                            }
                            else
                            {
                                UtilidadesConsola.MostrarTexto(
                                    ConsoleColor.Yellow,
                                    "Eliminacion cancenlada.\nDebe usar S para confirmar.\n"

                                    );
                            }

                            break;
                            
                        }

                    case 5:
                        {
     
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
                        UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow, "Volviendo al menu principal...");
                        break;

                    default:
                        UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "Opcion no válida.");
                        break;
                }
            }
        }



    }
}