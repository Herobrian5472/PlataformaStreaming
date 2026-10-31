using System;
using System.Collections.Generic;
using PlataformaStreaming.Datos;
using PlataformaStreaming.Servicios;
using PlataformaStreaming.Modelos;

namespace PlataformaStreaming.Metodos
{
    public static class Menu
    {
        public static void MostrarMenuInicial()
        {
            Console.WriteLine("\n=== MENÚ INICIAL ===");
            Console.WriteLine("1. Crear una cuenta");
            Console.WriteLine("2. Iniciar sesión");
            Console.WriteLine("0. Salir");
        }

        public static void RegistrarUsuario(Autenticacion autenticacion)
        {
            Console.WriteLine("\n=== CREAR UNA CUENTA ===");

            string nombre = EntradaConsola.LeerTextoObligatorio("Nombre: ");
            string apellido = EntradaConsola.LeerTextoObligatorio("Apellido: ");
            string nombreUsuario = EntradaConsola.LeerTextoObligatorio("Nombre de usuario: ");
            string email = EntradaConsola.LeerTextoObligatorio("Correo electrónico: ");

            Console.Write("Contraseña: ");
            string contrasenia = Console.ReadLine() ?? "";

            string resultado = autenticacion.Registrar(
                nombre, apellido, nombreUsuario, email, contrasenia);

            Console.WriteLine($"\n{resultado}");
        }

        public static Usuario? SolicitarInicioSesion(Autenticacion autenticacion)
        {
            Console.WriteLine("\n=== INICIAR SESIÓN ===");

            string nombreUsuario = EntradaConsola.LeerTextoObligatorio("Nombre de usuario: ");

            Console.Write("Contraseña: ");
            string contrasenia = Console.ReadLine() ?? "";

            Usuario? usuario = autenticacion.IniciarSesion(nombreUsuario, contrasenia);

            if (usuario == null)
                Console.WriteLine("\nUsuario o contraseña incorrectos.");

            return usuario;
        }

        public static class EntradaConsola
        {
            public static int LeerOpcion()
            {
                while (true)
                {
                    Console.Write("\nElegí una opción: ");
                    if (int.TryParse(Console.ReadLine(), out int opcion))
                        return opcion;

                    Console.WriteLine("Entrada inválida. Ingresá un número.");
                }
            }

            public static string LeerTextoObligatorio(string mensaje)
            {
                while (true)
                {
                    Console.Write(mensaje);
                    string texto = (Console.ReadLine() ?? "").Trim();

                    if (!string.IsNullOrWhiteSpace(texto))
                        return texto;

                    Console.WriteLine("Este campo no puede estar vacío.");
                }
            }

        }
    }
}
