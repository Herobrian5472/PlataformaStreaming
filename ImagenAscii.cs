
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlataformaStreaming
{
    public class ImagenAscii
    {
        private static readonly HttpClient cliente = new HttpClient();

        private const string UrlApi = "https://img2txt.genzouw.com";

        private const string UrlImagen =
            "https://drive.google.com/uc?export=download&id=1CbeQj8_pdQ8rzdh3Xk31d72p-5-jXtSY";

        public static async Task<string> ObtenerImagenAsync()
        {
            string urlCompleta = UrlApi + "?url=" +
                Uri.EscapeDataString(UrlImagen) + "&w=80";

            string resultado = await cliente.GetStringAsync(urlCompleta);

            return resultado;
        }
    }
}

/*
https://drive.google.com/file/d/1CbeQj8_pdQ8rzdh3Xk31d72p-5-jXtSY/view?usp=sharing
*/