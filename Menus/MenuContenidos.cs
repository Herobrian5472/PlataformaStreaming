using System;
using System.Collections.Generic;
using static PlataformaStreaming.UtilidadesConsola;
using PlataformaStreaming.Modelos;
using PlataformaStreaming.Servicios;
using PlataformaStreaming.Datos;
using PlataformaStreaming.Metodos;
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

            int opcion = 12;

            if (administrador)
            {

                while (opcion != 0)
                {
                    Console.WriteLine("\n--- MENU DE CONTENIDOS ---");
                    Console.WriteLine("1. Listar películas");
                    Console.WriteLine("2. Listar series");
                    Console.WriteLine("3. Agregar contenido");
                    Console.WriteLine("4. Modificar contenido");
                    Console.WriteLine("5. Eliminar contenido");
                    Console.WriteLine("6. Dar contenido de baja");
                    Console.WriteLine("7. Ver contenidos dados de baja");
                    Console.WriteLine("8. Reactivar contenido");
                    Console.WriteLine();
                    Console.WriteLine("9. Descargar historial de movimientos");
                    Console.WriteLine("10. Otorgar 'Administrador' a un usuario");
                    Console.WriteLine("11. Quitar privilegio de administrador");
                    Console.WriteLine();
                    Console.WriteLine("0. Cerrar sesión");
                    Console.WriteLine();
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
                                ListarContenidos(contenidoDAO.ObtenerPeliculas());
                                break;
                            }
                        case 2:
                            {
                                ListarContenidos(contenidoDAO.ObtenerSeries());
                                break;
                            }

                        case 3:
                            {
                                AgregarNuevoContenido(contenidoDAO);
                                break;
                            }

                        case 4:
                            {
                                List<Contenido> contenidos = contenidoDAO.ObtenerTodosActivos(); // Obtener tanto pelis como series

                                Console.WriteLine("ID del contenido a modificar: ");

                                if (!int.TryParse(Console.ReadLine(), out int id))
                                {
                                    UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "ID inválido.");
                                    break;
                                }

                                Contenido contenido = null; // creamos el objeto sin ninguna pelicula seleccionada

                                foreach (Contenido p in contenidos)
                                {
                                    if (p.ContenidoId == id) // comparamos el id de cada pelicula con lo que ingresó el usuario
                                    {
                                        contenido = p; // si el id existe se lo asignamos a pelicula
                                        break;
                                    }
                                }

                                if (contenido == null)
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Red,
                                        "No existe un contenido con ese ID."
                                    );

                                    break;
                                }

                                Console.WriteLine("CONTENIDO SELECCIONADO");

                                Console.WriteLine($"Título: {contenido.Titulo} | Tipo: {contenido.Tipo} | Año: {contenido.AnioLanzamiento} ");

                                string confirmacion;

                                do
                                {
                                    Console.Write("\n¿Está seguro de que desea modificar este contenido? (S/N): ");
                                    confirmacion = Console.ReadLine() ?? "";

                                    if (!confirmacion.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                                        !confirmacion.Equals("N", StringComparison.OrdinalIgnoreCase))
                                    {
                                        UtilidadesConsola.MostrarTexto(
                                            ConsoleColor.DarkYellow,
                                            "Opción inválida. Debe ingresar S o N. Inténtelo nuevamente."
                                        );
                                    }

                                } while (!confirmacion.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                                         !confirmacion.Equals("N", StringComparison.OrdinalIgnoreCase));

                                if (confirmacion.Equals("N", StringComparison.OrdinalIgnoreCase))
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.DarkYellow,
                                        "Modificación cancelada."
                                    );

                                    break;
                                }

                                bool modificando = true;

                                while (modificando)
                                {
                                    Console.WriteLine("\n¿Qué dato desea modificar?");
                                    Console.WriteLine("1. Título");
                                    Console.WriteLine("2. Descripción");
                                    Console.WriteLine("3. Año de lanzamiento");
                                    Console.WriteLine("4. Género");
                                    Console.WriteLine("5. Duración / Capítulos");
                                    Console.WriteLine("0. Cancelar");

                                    Console.Write("Seleccione una opción: ");

                                    string opcionModificar = Console.ReadLine() ?? "";

                                    switch (opcionModificar)
                                    {
                                        case "1":
                                            Console.Write("Nuevo título: ");
                                            string titulo = Console.ReadLine() ?? "";

                                            if (string.IsNullOrWhiteSpace(titulo))
                                            {
                                                UtilidadesConsola.MostrarTexto(
                                                    ConsoleColor.DarkYellow,
                                                    "El título no puede estar vacío."
                                                );
                                                break;
                                            }

                                            contenido.Titulo = titulo;
                                            break;

                                        case "2":
                                            Console.Write("Nueva descripción: ");
                                            contenido.Descripcion = Console.ReadLine() ?? "";
                                            break;

                                        case "3":
                                            Console.Write("Nuevo año: ");

                                            if (!int.TryParse(Console.ReadLine(), out int anio))
                                            {
                                                UtilidadesConsola.MostrarTexto(
                                                    ConsoleColor.DarkYellow,
                                                    "Año inválido."
                                                );
                                                break;
                                            }

                                            contenido.AnioLanzamiento = anio;
                                            break;

                                        case "4":
                                            contenido.Genero = LeerGenero();
                                            break;

                                        case "5":
                                            contenido.Duracion = LeerDuracion(contenido.Tipo);
                                            break;

                                        case "0":
                                            modificando = false;
                                            break;

                                        default:
                                            UtilidadesConsola.MostrarTexto(
                                                ConsoleColor.DarkYellow,
                                                "Opción inválida."
                                            );
                                            break;
                                    }

                                    if (opcionModificar == "0")
                                        break;

                                    if (opcionModificar == "1" ||
                                        opcionModificar == "2" ||
                                        opcionModificar == "3" ||
                                        opcionModificar == "4" ||
                                        opcionModificar == "5")
                                    {
                                        bool modificado = contenidoDAO.ModificarContenido(contenido, usuario.UsuarioId);

                                        if (modificado)
                                        {
                                            UtilidadesConsola.MostrarTexto(
                                                ConsoleColor.Green,
                                                "Contenido modificado correctamente."
                                            );

                                            string continuar;

                                            do
                                            {
                                                Console.Write("\n¿Desea seguir modificando este contenido? (S/N): ");
                                                continuar = Console.ReadLine() ?? "";

                                                if (!continuar.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                                                    !continuar.Equals("N", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    UtilidadesConsola.MostrarTexto(
                                                        ConsoleColor.DarkYellow,
                                                        "Opción inválida. Ingrese S o N."
                                                    );
                                                }

                                            } while (!continuar.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                                                     !continuar.Equals("N", StringComparison.OrdinalIgnoreCase));

                                            if (continuar.Equals("N", StringComparison.OrdinalIgnoreCase))
                                            {
                                                modificando = false;
                                            }
                                        }
                                        else
                                        {
                                            UtilidadesConsola.MostrarTexto(
                                                ConsoleColor.Red,
                                                "No se pudo modificar el contenido."
                                            );

                                            modificando = false;
                                        }

                                    }
                                }

                            }

                            break;
                        
                        case 5:
                            {
                                while (true)
                                {
                                    List<Contenido> peliculas = contenidoDAO.ObtenerTodosActivos();

                                    if (peliculas.Count == 0)
                                    {
                                        UtilidadesConsola.MostrarTexto(
                                            ConsoleColor.DarkYellow,
                                            "No hay contenidos para eliminar."
                                            );
                                        break;
                                    }

                                    Console.WriteLine("ID del contenido a eliminar: ");

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
                                        "Contenido seleccionado: " +
                                        pelicula.Titulo + " (" +
                                        pelicula.AnioLanzamiento + ")"
                                        );
                                    Console.ResetColor();

                                    while (true)
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow, "¿Seguro que desea eliminarlo? (S/N): ");
                                        string confirmacion = (Console.ReadLine() ?? "").Trim().ToUpper();

                                        if (confirmacion == "S")
                                        {
                                            bool eliminado = contenidoDAO.EliminarContenido(id, usuario.UsuarioId);

                                            if (eliminado)
                                            {
                                                UtilidadesConsola.MostrarTexto(
                                                    ConsoleColor.Green,
                                                    "Contenido eliminado correctamente."
                                                    );
                                                break;
                                            }
                                            else
                                            {
                                                UtilidadesConsola.MostrarTexto(
                                                    ConsoleColor.Red,
                                                    "No se pudo eliminar el contenido."
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

                        case 6:
                            {
                                List<Contenido> contenidos = contenidoDAO.ObtenerTodosActivos();

                                if (contenidos.Count == 0)
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.DarkYellow,
                                        "No hay contenidos para dar de baja."
                                        );
                                    break;
                                }

                                Console.WriteLine("ID del contenido que desea dar de baja: ");

                                if (!int.TryParse(Console.ReadLine(), out int id))
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Red,
                                        "ID inválido."
                                    );
                                    break;
                                }

                                bool resultado = contenidoDAO.DarBajaContenido(id, usuario.UsuarioId);

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

                        case 7:
                            {
                                List<Contenido> peliculasBajas = contenidoDAO.MostrarBajas();

                                if (peliculasBajas.Count == 0)
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Yellow,
                                        "No hay contenidos dados de baja."
                                    );
                                }
                                else
                                {
                                    UtilidadesConsola.MostrarTexto(
                                        ConsoleColor.Yellow,
                                        "--- CONTENIDOS DADOS DE BAJA ---"
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

                        case 8:
                            {
                                List<Contenido> contenidosBajas = contenidoDAO.MostrarBajas();

                                if (contenidosBajas.Count == 0)
                                {
                                    MostrarTexto(
                                        ConsoleColor.Yellow,"No hay contenidos dados de baja.");
                                }
                                else
                                {
                                    MostrarTexto(ConsoleColor.Yellow,"--- CONTENIDOS PARA REACTIVAR---");

                                    foreach (Contenido contenido in contenidosBajas)
                                    {
                                        Console.WriteLine(
                                            contenido.ContenidoId + " - " +
                                            contenido.Titulo + " (" +
                                            contenido.AnioLanzamiento + ")"
                                        );
                                    }

                                    Console.WriteLine("\nID del contenido a reactivar: ");

                                    if (int.TryParse(Console.ReadLine(), out int id))
                                    {
                                        bool resultado = contenidoDAO.DarAltaContenido(id, usuario.UsuarioId);

                                        if (resultado)
                                        {
                                            MostrarTexto(ConsoleColor.Green,"contenido reactivado correctamente.");
                                        }
                                        else
                                        {
                                            MostrarTexto(ConsoleColor.Red,"No se encontró un contenido dado de baja con ese ID.");
                                        }
                                    }
                                    else
                                    {
                                        MostrarTexto(ConsoleColor.Red,"ID inválido.");
                                    }
                                }

                                break;
                            }

                        case 9:
                            {
                                HistorialDAO historialDAO = new HistorialDAO();

                                List<Historial> historial = historialDAO.ObtenerHistorial();

                                // Si no hay movimientos, volvemos al menú.
                                if (historial.Count == 0)
                                {
                                    break;
                                }
                                    

                                // Mostramos el historial en consola.
                                historialDAO.MostrarHistorial(historial);

                                bool respuestaValida = false;

                                while (!respuestaValida)
                                {
                                    Console.WriteLine();
                                    Console.Write("¿Desea exportar el historial a un archivo TXT? (S/N): ");

                                    string respuesta = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

                                    if (respuesta == "S" || respuesta == "SI")
                                    {
                                        historialDAO.DescargarHistorial(historial);
                                        respuestaValida = true;
                                    }
                                    else if (respuesta == "N" || respuesta == "NO")
                                    {
                                        UtilidadesConsola.MostrarTexto(
                                            ConsoleColor.Yellow,
                                            "Volviendo al menú principal..."
                                        );

                                        respuestaValida = true;
                                    }
                                    else
                                    {
                                        UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow,"Opción inválida. Ingrese S para sí o N para no.");
                                    }
                                }

                                break;
                            }

                        case 10:
                            {
                                GestionarRolUsuario("Usuario", "Administrador");
                                break;
                            }

                        case 11:
                            {
                                GestionarRolUsuario("Administrador", "Usuario");
                                break;
                            }


                        case 0:
                            {
                                UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow, "Cerrando sesión...");
                                return;
                            }

                        default:
                            UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "Opcion no válida.");
                            break;
                    }
                }
            }
            else
            {
                while (opcion != 0)
                {
                    Console.WriteLine("\n--- MENU DE USUARIO ---");
                    Console.WriteLine("1. Listar películas");
                    Console.WriteLine("2. Listar series");
                    Console.WriteLine("3. Ver mi perfil.");
                    Console.WriteLine("4. Buscar contenido por título.");
                    Console.WriteLine();
                    Console.WriteLine("0. Cerrar sesión");
                    Console.WriteLine();
                    Console.Write("Seleccione una opción: ");

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
                                ListarContenidos(contenidoDAO.ObtenerPeliculas());
                                break;
                            }

                        case 2:
                            {
                                ListarContenidos(contenidoDAO.ObtenerSeries());
                                break; 
                            }

                        case 3:
                            {
                                MostrarMiPerfil();
                                break;
                            }

                        case 4:
                            {
                                BuscarContenidoPorTitulo(contenidoDAO);
                                break;
                            }

                        case 0:
                            UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow, "Cerrando sesión...");
                            return;

                        default:
                            UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "Opcion no válida.");
                            break;
                    }
                }
            }
        }

        // Gestiona cambios de rol desde el menú administrativo.
        private void GestionarRolUsuario(string rolActual, string rolNuevo)
        {
            UsuarioDAO usuarioDAO = new UsuarioDAO();
            List<Usuario> usuarios = usuarioDAO.ObtenerUsuariosPorRol(rolActual);

            if (usuarios.Count == 0)
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow,
                    rolActual == "Usuario"
                        ? "No hay usuarios comunes para convertir en administradores."
                        : "No hay otros administradores disponibles.");
                return;
            }

            Console.WriteLine(rolNuevo == "Administrador"
                ? "\n--- USUARIOS DISPONIBLES PARA PROMOVER ---"
                : "\n--- ADMINISTRADORES REGISTRADOS ---");

            foreach (Usuario usuarioListado in usuarios)
            {
                Console.WriteLine($"{usuarioListado.NombreUsuario} - {usuarioListado.Nombre} {usuarioListado.Apellido}");
            }

            string nombreUsuario = Menu.EntradaConsola.LeerTextoObligatorio(
                "Ingrese el nombre de usuario: ");

            Usuario? usuarioSeleccionado = usuarioDAO.BuscarPorNombreUsuario(nombreUsuario);

            if (usuarioSeleccionado == null)
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "No existe un usuario con ese nombre.");
                return;
            }

            if (!string.Equals(usuarioSeleccionado.Rol, rolActual, StringComparison.OrdinalIgnoreCase))
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow,
                    rolNuevo == "Administrador"
                        ? "Ese usuario ya tiene privilegios de administrador."
                        : "Ese usuario no tiene el rol de administrador.");
                return;
            }

            if (rolNuevo == "Usuario")
            {
                if (string.Equals(usuarioSeleccionado.NombreUsuario, usuario.NombreUsuario,
                    StringComparison.OrdinalIgnoreCase))
                {
                    UtilidadesConsola.MostrarTexto(ConsoleColor.Red,
                        "No podés quitarte el rol de administrador desde la sesión actual.");
                    return;
                }

                if (usuarioDAO.ContarAdministradores() <= 1)
                {
                    UtilidadesConsola.MostrarTexto(ConsoleColor.Red,
                        "No se puede quitar el rol: debe quedar al menos un administrador.");
                    return;
                }
            }

            bool actualizado = usuarioDAO.CambiarRol(nombreUsuario, rolNuevo);

            if (actualizado)
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Green,
                    rolNuevo == "Administrador"
                        ? $"Se otorgó el rol de administrador a {nombreUsuario}."
                        : $"Se quitó el rol de administrador a {nombreUsuario}.");
            }
            else
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Red,
                    "No se pudo actualizar el rol del usuario.");
            }
        }

        // Solicita y valida los datos del contenido sin llenar el case del menú de while.
        private void AgregarNuevoContenido(ContenidoDAO contenidoDAO)
        {
            string tipo = LeerTipoContenido();

            Contenido nuevoContenido;

            if (tipo == "Pelicula")
            {
                nuevoContenido = new Pelicula();
            }
            else
            {
                nuevoContenido = new Serie();
            }

            nuevoContenido.Titulo = Menu.EntradaConsola.LeerTextoObligatorio("Título: ");

            nuevoContenido.Descripcion = Menu.EntradaConsola.LeerTextoObligatorio("Descripción: ");

            nuevoContenido.AnioLanzamiento = LeerAnio();

            nuevoContenido.Genero = LeerGenero();

            nuevoContenido.Duracion = LeerDuracion(tipo);

            contenidoDAO.AgregarContenido(nuevoContenido, usuario.UsuarioId);

            UtilidadesConsola.MostrarTexto(ConsoleColor.Green, $"{nuevoContenido.Tipo} guardada correctamente: {nuevoContenido.Titulo} ({nuevoContenido.AnioLanzamiento}).");
        }

        private int LeerAnio()
        {
            while (true)
            {
                string entrada = Menu.EntradaConsola.LeerTextoObligatorio("Año de lanzamiento: ");
                if (int.TryParse(entrada, out int anio) && anio >= 1900 && anio <= DateTime.Now.Year)
                    return anio;

                UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow,
                    $"Ingrese un año entre 1900 y {DateTime.Now.Year}.");
            }
        }

        private int LeerDuracion(string tipo)
        {
            while (true)
            {
                Console.Write(tipo == "Pelicula" ? "Duración en minutos: " : "Cantidad de episodios: ");
                string entrada = Menu.EntradaConsola.LeerTextoObligatorio("");

                if (int.TryParse(entrada, out int duracion) && duracion > 0)
                    return duracion;

                UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow,
                    "La duración debe ser un número entero mayor a cero.");
            }
        }

        private string LeerTipoContenido()
        {
            while (true)
            {
                Console.WriteLine("Tipo de contenido:");
                Console.WriteLine("1. Película");
                Console.WriteLine("2. Serie");
                string opcion = Menu.EntradaConsola.LeerTextoObligatorio("Seleccione una opción: ");

                if (opcion == "1") return "Pelicula";
                if (opcion == "2") return "Serie";

                UtilidadesConsola.MostrarTexto(ConsoleColor.DarkYellow, "Seleccione 1 o 2.");
            }
        }

        private void MostrarMiPerfil()
        {
            Console.WriteLine("\n--- MI PERFIL ---");
            Console.WriteLine($"ID: {usuario.UsuarioId}");
            Console.WriteLine($"Nombre: {usuario.Nombre}");
            Console.WriteLine($"Apellido: {usuario.Apellido}");
            Console.WriteLine($"Nombre de usuario: {usuario.NombreUsuario}");
            Console.WriteLine($"Correo electrónico: {usuario.Email}");
            Console.WriteLine($"Rol: {usuario.Rol}");
            Console.WriteLine($"Fecha de registro: {usuario.FechaRegistro:dd/MM/yyyy HH:mm}");
            Console.WriteLine("\nPor seguridad, no se muestran los datos de su contraseña.");
        }

        private void BuscarContenidoPorTitulo(ContenidoDAO contenidoDAO)
        {
            string tituloBuscado = Menu.EntradaConsola.LeerTextoObligatorio("Ingrese el título que desea buscar: ");
            List<Contenido> resultados = contenidoDAO.BuscarPorTitulo(tituloBuscado);

            if (resultados.Count == 0)
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow,
                    $"No existe contenido que coincida con el título '{tituloBuscado}'.");
                return;
            }

            ListarContenidos(resultados, mostrarDescripcion: true, preguntarReproduccion: true, reproducirDirectamente: true);
        }

        private string LeerGenero()
        {
            return Menu.EntradaConsola.LeerTextoObligatorio("Género: ");
        }

        private void ListarContenidos(List<Contenido> contenidos, bool mostrarDescripcion = false, bool preguntarReproduccion = true, bool reproducirDirectamente = false)
        {
            if (contenidos.Count == 0)
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow,
                    "No hay contenidos para mostrar.");
                return;
            }

            foreach (Contenido contenido in contenidos)
            {
                string unidad = contenido.Tipo.Equals("Pelicula", StringComparison.OrdinalIgnoreCase) ? "minutos" : "episodios";

                Console.WriteLine($"\n{contenido.ContenidoId} - {contenido.Titulo} ({contenido.AnioLanzamiento})");
                Console.WriteLine($"Género: {contenido.Genero} | Duración: {contenido.Duracion} {unidad}");

                if (mostrarDescripcion)
                    Console.WriteLine($"Descripción: {contenido.Descripcion}");
            }

            if (preguntarReproduccion)
            {
                if (reproducirDirectamente) { PreguntarReproduccionDirecta(contenidos); }

                else {PreguntarSiDeseaReproducir(contenidos);}
                    
            }
        }

        private void PreguntarReproduccionDirecta(List<Contenido> contenidos)
        {
            Console.Write("\n¿Desea reproducir este contenido? (S/N): ");
            string respuesta = (Console.ReadLine() ?? "").Trim();

            while (!respuesta.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                   !respuesta.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Red,
                    "Opción inválida. Ingrese S o N.");

                Console.Write("\n¿Desea reproducir este contenido? (S/N): ");
                respuesta = (Console.ReadLine() ?? "").Trim();
            }

            if (respuesta.Equals("N", StringComparison.OrdinalIgnoreCase))
                return;

            if (contenidos.Count == 1)
            {
                MostrarMenuReproduccion(contenidos[0]);
                return;
            }

            Console.Write("Ingrese el ID del contenido que desea reproducir: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "ID inválido.");
                return;
            }

            Contenido? seleccionado = contenidos.Find(c => c.ContenidoId == id);

            if (seleccionado == null)
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Red,
                    "El ID no corresponde a los contenidos listados.");
                return;
            }

            MostrarMenuReproduccion(seleccionado);
        }

        private void PreguntarSiDeseaReproducir(List<Contenido> contenidos)
        {
            string respuesta;

            do
            {
                Console.Write("\n¿Desea reproducir alguno de estos contenidos? (S/N): ");
                respuesta = (Console.ReadLine() ?? "").Trim();

                if (!respuesta.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                    !respuesta.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    UtilidadesConsola.MostrarTexto(ConsoleColor.Red,
                        "Opción inválida. Ingrese S o N.");
                }

            } while (!respuesta.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                     !respuesta.Equals("N", StringComparison.OrdinalIgnoreCase));

            if (respuesta.Equals("N", StringComparison.OrdinalIgnoreCase))
                return;

            Console.Write("Ingrese el ID del contenido: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Red, "ID inválido.");
                return;
            }

            Contenido? seleccionado = contenidos.Find(c => c.ContenidoId == id);

            if (seleccionado == null)
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Red,
                    "El ID no corresponde a los contenidos listados.");
                return;
            }

            do
            {
                Console.Write($"¿Desea reproducir '{seleccionado.Titulo}'? (S/N): ");
                respuesta = (Console.ReadLine() ?? "").Trim();

                if (!respuesta.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                    !respuesta.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    UtilidadesConsola.MostrarTexto(ConsoleColor.Red,
                        "Opción inválida. Ingrese S o N.");
                }

            } while (!respuesta.Equals("S", StringComparison.OrdinalIgnoreCase) &&
                     !respuesta.Equals("N", StringComparison.OrdinalIgnoreCase));

            if (respuesta.Equals("S", StringComparison.OrdinalIgnoreCase))
                MostrarMenuReproduccion(seleccionado);
        }

        private void MostrarMenuReproduccion(Contenido contenido)
        {
            bool reproduciendo = true;
            int capituloActual = 1;

            bool esSerie = string.Equals(
                contenido.Tipo,
                "Serie",
                StringComparison.OrdinalIgnoreCase);

            string tipo = esSerie ? "SERIE" : "PELÍCULA";

            UtilidadesConsola.MostrarTexto(
                ConsoleColor.Green,
                $"\nREPRODUCIENDO {tipo}: {contenido.Titulo}");

            while (true)
            {
                Console.WriteLine(reproduciendo
                    ? "\nEstado: REPRODUCIENDO"
                    : "\nEstado: PAUSADO");

                if (esSerie)
                {
                    Console.WriteLine($"Capítulo: {capituloActual} de {contenido.Duracion}");
                }

                Console.WriteLine("1. PAUSAR");
                Console.WriteLine("2. REPRODUCIR");

                if (esSerie)
                {
                    Console.WriteLine("3. SIGUIENTE CAPÍTULO");
                    Console.WriteLine("4. CAPÍTULO ANTERIOR");
                }

                Console.WriteLine("0. VOLVER");
                Console.Write("Seleccione una opción: ");

                string opcion = (Console.ReadLine() ?? "").Trim();

                if (opcion == "1")
                {
                    if (!reproduciendo)
                    {
                        UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow,
                            "La reproducción ya se encuentra pausada.");
                    }
                    else
                    {
                        reproduciendo = false;

                        UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow,
                            "Reproducción pausada.");
                    }
                }
                else if (opcion == "2")
                {
                    if (reproduciendo)
                    {
                        UtilidadesConsola.MostrarTexto(ConsoleColor.Green,
                            "La reproducción ya se encuentra en curso.");
                    }
                    else
                    {
                        reproduciendo = true;

                        UtilidadesConsola.MostrarTexto(ConsoleColor.Green,
                            "Reproduciendo...");
                    }
                }
                else if (opcion == "3" && esSerie)
                {
                    if (capituloActual < contenido.Duracion)
                    {
                        capituloActual++;

                        UtilidadesConsola.MostrarTexto(ConsoleColor.Green,
                            $"Pasando al capítulo {capituloActual} de {contenido.Duracion}.");
                    }
                    else
                    {
                        UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow,
                            "Ya estás en el último capítulo de esta serie.");
                    }
                }
                else if (opcion == "4" && esSerie)
                {
                    if (capituloActual > 1)
                    {
                        capituloActual--;

                        UtilidadesConsola.MostrarTexto(ConsoleColor.Green,
                            $"Volviendo al capítulo {capituloActual} de {contenido.Duracion}.");
                    }
                    else
                    {
                        UtilidadesConsola.MostrarTexto(ConsoleColor.Yellow,
                            "Ya estás en el primer capítulo de esta serie.");
                    }
                }
                else if (opcion == "0")
                {
                    break;
                }
                else
                {
                    UtilidadesConsola.MostrarTexto(ConsoleColor.Red,
                        "Opción no válida.");
                }
            }
        }

    }
}
