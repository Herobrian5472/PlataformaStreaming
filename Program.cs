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
        static async Task Main(string[] args)
        {
            try
            {

                string imagen = await ImagenAscii.ObtenerImagenAsync();

                Console.WriteLine(imagen);

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
                    UtilidadesConsola.MostrarTexto(ConsoleColor.Green,$"\n¡Bienvenido {usuario.NombreUsuario}!");

                    MenuContenidos menu = new MenuContenidos(usuario);
                    menu.Mostrar();
                }
                
            }
            catch (Exception ex)
            {
                UtilidadesConsola.MostrarTexto(ConsoleColor.Red,$"Ocurrió un error inesperado: {ex.Message}");
            }

            Console.WriteLine("\nPresioná una tecla para salir...");
            Console.ReadKey();
        }
    }
}