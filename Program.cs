using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace PlataformaStreaming
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (SqliteConnection conexion = ConexionDB.ObtenerConexion())
                {

                    MenuContenidos menu = new MenuContenidos();
                    menu.Mostrar();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.WriteLine("\nPresioná una tecla para salir...");
            Console.ReadKey();
        }
    }
}