using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using PlataformaStreaming;
using PlataformaStreaming.Datos;
using PlataformaStreaming.Servicios;
using PlataformaStreaming.Modelos;

namespace PlataformaStreaming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // ================ CREAR ADMIN

                /*
                UsuarioDAO usuarioDAO = new UsuarioDAO();

                Console.Write("\nIngresá tu nombre de usuario para asignar el rol de administrador: ");
                string nombreUsuarioAdmin = Console.ReadLine() ?? "";

                bool actualizado = usuarioDAO.CambiarRol(nombreUsuarioAdmin, "Administrador");

                if (actualizado)
                {
                    Console.WriteLine("¡Rol de administrador asignado correctamente!");
                }
                else
                {
                    Console.WriteLine("No se encontró ese usuario.");
                }
                */

                // ============ INICIAR

                Console.Title = "TUX Streaming";

                // validar licencia
                if (!ValidadorLicencia.Validar())
                {
                    return;
                }

                // inicializar la base de datos
                ConexionDB.InicializarBaseDeDatos();

                // creamos los objetos necesario para autenticacion
                UsuarioDAO usuarioDAO = new UsuarioDAO();
                Autenticacion autenticacion = new Autenticacion(usuarioDAO);

                while (true)
                {
                    // mostrar el menú de login
                    MenuLogIn menuLogin = new MenuLogIn(autenticacion);
                    Usuario? usuario = menuLogin.Mostrar();

                    // Si el menú devuelve null, el usuario eligió salir.
                    if (usuario == null)
                    {
                        break;
                    }

                    // si se autenticó, ingresar al menú de contenidos
                    if (usuario != null)
                    {
                       UtilidadesConsola.MostrarTexto(ConsoleColor.Green, (
                            "\n¡Bienvenido " + usuario.NombreUsuario + "!"
                        ));

                        MenuContenidos menu = new MenuContenidos(usuario);
                        menu.Mostrar();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); // temporal: volver a poner "Error: " + ex.Message
            }

            Console.WriteLine("\nPresioná una tecla para salir...");
            Console.ReadKey();
        }
    }
}