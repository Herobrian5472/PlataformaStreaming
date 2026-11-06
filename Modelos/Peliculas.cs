using System;


namespace PlataformaStreaming
{
    internal class Pelicula : Contenido
    {
        public Pelicula() : base("Pelicula")
        {
        }

        public override string ObtenerUnidadDuracion()
        {
            return "minutos";
        }
    }
}

