using System;


namespace PlataformaStreaming
{
    internal class Serie : Contenido
    {
        public Serie() : base("Serie")
        {
        }

        public override string ObtenerUnidadDuracion()
        {
            return "capítulos";
        }
    }
}

