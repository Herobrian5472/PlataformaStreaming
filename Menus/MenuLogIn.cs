using System;
using PlataformaStreaming.Modelos;
using PlataformaStreaming.Servicios;

namespace PlataformaStreaming
{
    internal class MenuLogIn
    {
        private readonly Autenticacion autenticacion;

        public MenuLogIn(Autenticacion autenticacion)
        {
            this.autenticacion = autenticacion;
        }

        public Usuario? Mostrar()
        {
            bool menuActivo = true;

            while (menuActivo)
            {
                Console.WriteLine("\n=== MENU INICIAL ===");
                Console.WriteLine("1. Crear una cuenta");
                Console.WriteLine("2. Inicia sesión");
                Console.WriteLine("0. Salir");

                Console.Write("\nElegí una opción:");

                if (!int.TryParse(Console.ReadLine(), out int opcion))
                {
                    Console.WriteLine("Ingresá una opción válida.");
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        RegistrarUsuario();
                        break;

                    case 2:
                        Usuario? usuario = SolicitarInicioSesion();

                        if (usuario != null)
                        {
                            return usuario;
                        }
                        // cuando el login es correcto, sale del menú y devuelve el usuario a quien llamó a Mostrar()

                        break;

                    case 0:
                        menuActivo = false;
                        Console.WriteLine("¡Hasta pronto!");
                        break;

                    default:
                        Console.WriteLine("La opción ingresada no existe.");
                        break;
                }
            }

            return null;
        }

        private void RegistrarUsuario()
        {
            Console.WriteLine("\n=== CREAR UNA CUENTA===");

            string nombre = LeerTextoObligatorio("Nombre: ");
            string apellido = LeerTextoObligatorio("Apellido: ");
            string nombreUsuario = LeerTextoObligatorio("Nombre de usuario: ");
            string email = LeerTextoObligatorio("Correo electrónico: ");
            string contrasenia = LeerTextoObligatorio("Contraseña: ");

            string resultado = autenticacion.Registrar(
                nombre,
                apellido,
                nombreUsuario,
                email,
                contrasenia
            );

            Console.WriteLine("\n" + resultado);
        }

        private Usuario? SolicitarInicioSesion()
        {
            Console.WriteLine("\n=== INICIAR SESION ===");

            string nombreUsuario = LeerTextoObligatorio("Nombre de usuario: ");
            string contrasenia = LeerTextoObligatorio("Contraseña: ");

            Usuario? usuario = autenticacion.IniciarSesion(
                nombreUsuario,
                contrasenia
            );

            if (usuario == null)
            {
                Console.WriteLine("\nUsuario o contraseña incorrectos.");
            }

            return usuario;
        }

        // funcion auziliar para evitar que el usuario deje campos vacios, si escribe espacios o presiona enter
        private string LeerTextoObligatorio(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);

                string texto = (Console.ReadLine() ?? "").Trim();

                if (!string.IsNullOrWhiteSpace(texto))
                {
                    return texto;
                }

                Console.WriteLine("Este campo no puede estar vacío");
            }
        }
    }
}
