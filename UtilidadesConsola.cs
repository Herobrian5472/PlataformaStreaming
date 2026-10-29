using System;

namespace PlataformaStreaming
{
    internal class UtilidadesConsola
    {
        public static void MostrarTexto(ConsoleColor color, string texto)
        {
            Console.ForegroundColor = color;

            Console.WriteLine(texto);

            Console.ResetColor();
        }
    }
}
