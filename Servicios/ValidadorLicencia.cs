using System;
using System.IO;
using System.Threading;
using static PlataformaStreaming.UtilidadesConsola;

namespace PlataformaStreaming
{
    internal class ValidadorLicencia
    {
        private static readonly string claveValida =
            "Tux aprueba el TPI";

        public static bool Validar()
        {
            MostrarTexto(
                ConsoleColor.Gray,
                "Iniciando Plataforma Streaming...\nComprobando licencia..."
            );

            // demorar 3 segundos
            Thread.Sleep(3000);

            // comprobar si existe licencia.txt
            string rutaArchivo = Path.Combine( // path combine para crear la ruta 
                AppContext.BaseDirectory, // obtiene la ubicacion desde donde se ejecuta el programa
                "Archivos",
                "licencia.txt"
            );
            // tuve que forzar la creacion de licencia.txt dentro de .csproj para que el programa pueda encontrarlo

            //temporal
            /*
            Console.WriteLine($"Buscamos la licencia en: {rutaArchivo}");
            Console.WriteLine($"¿Existe el archivo?: {File.Exists(rutaArchivo)}");
            */

            // comprueba que exista el archivo
            if (!File.Exists(rutaArchivo))
            {
                MostrarTexto(
                    ConsoleColor.Red,
                    "Licencia inexistente."
                );
                Console.ReadKey();
                return false;
            }

            //Intenta leer la clave del archivo pero está atento a posibles errores.
            string claveArchivo;

            try
            {
                claveArchivo = File.ReadAllText(rutaArchivo).Trim();
            }
            catch (IOException)
            {
                MostrarTexto(
                    ConsoleColor.Red,
                    "No se pudo leer el archivo de licencia."
                );

                return false;
            }
            catch (UnauthorizedAccessException)
            {
                MostrarTexto(
                    ConsoleColor.Red,
                    "No tenés permisos para acceder al archivo de licencia."
                );

                return false;
            }

            //Hace una comparación exacta entre la clave válida y la clave que se almacena en el archivo.
            if (string.Equals(claveArchivo, claveValida, StringComparison.Ordinal))
            {
                MostrarTexto(
                    ConsoleColor.Green,
                    "[✓] Licencia vigente\nAcceso concedido."
                );
                return true;
            }

            MostrarTexto(
                ConsoleColor.Red,
                "[X] Error en licencia\nFalla en el acceso."
            );
            Console.ReadKey();
            return false;
        }
    }
}
