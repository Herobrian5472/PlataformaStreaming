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

            // leer la clave del archivo
            string claveArchivo = File.ReadAllText(rutaArchivo).Trim();

            // temporal
            /*
            Console.WriteLine($"Ruta real: {rutaArchivo}");
            Console.WriteLine($"La clave leída: {claveArchivo}");
            Console.WriteLine($"Clave esperada: {claveValida}");
            Console.ReadKey();
            */

            // compararla con claveValida
            if (claveArchivo == claveValida)
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
