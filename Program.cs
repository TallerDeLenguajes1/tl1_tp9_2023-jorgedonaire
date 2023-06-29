using System.Text.Json;
using System.Net;
using EspacioMonedas;
class Program
{
    private static void Main(){
        var url = $"https://api.coindesk.com/v1/bpi/currentprice.json";
        var request = (HttpWebRequest) WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        try
        {
            using (WebResponse response = request.GetResponse())
            {
                using (Stream streamReader = response.GetResponseStream())
                {
                    if (streamReader != null)
                    {
                        using (StreamReader objReader = new StreamReader(streamReader))
                        {
                            string responseBody = objReader.ReadToEnd(); //string obtenido de la api
                            var datosPrecios = JsonSerializer.Deserialize<Root>(responseBody); //deserializa en la clase root (Monedas.cs) a una var datosPrecios
                            preciosDisponibles(datosPrecios);

                            Console.WriteLine("**** Elija una moneda ****");
                            Console.WriteLine("1-USD\n2-EUR\n3-BGP");
                            string menuIngresado;
                            int menu;
                            do
                            {
                                menuIngresado = Console.ReadLine();
                            } while (string.IsNullOrEmpty(menuIngresado));
                        
                            bool control = int.TryParse(menuIngresado,out menu);
                            if (control)
                            {
                                switch (menu)
                                {
                                    case 1:
                                        mostrarCaracteristicas("USD",datosPrecios.bpi.USD);
                                    break;
                                    case 2:
                                        mostrarCaracteristicas("EUR",datosPrecios.bpi.EUR);
                                    break;
                                    case 3:
                                        mostrarCaracteristicas("GBP",datosPrecios.bpi.GBP);
                                    break;
                                    default:
                                        Console.WriteLine("Opcion ingresada incorrecta");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (WebException) //System.Exception tambien funciona
        { 
           Console.WriteLine("Problemas de acceso a la API");
        }


    }

    private static void preciosDisponibles(Root? datosPrecios){
        Console.WriteLine("***** Precios Disponibles para {0} ******", datosPrecios.chartName);
        Console.WriteLine(datosPrecios.bpi.USD.code + ":" + datosPrecios.bpi.USD.rate_float);
        Console.WriteLine(datosPrecios.bpi.EUR.code + ":" + datosPrecios.bpi.EUR.rate_float);
        Console.WriteLine(datosPrecios.bpi.GBP.code + ":" + datosPrecios.bpi.GBP.rate_float);
    }

    private static void mostrarCaracteristicas(string nombreMoneda, dynamic moneda){
        Console.WriteLine("**** Caracteristicas: {0} ******", nombreMoneda);
        Console.WriteLine("Codigo: {0}",moneda.code);
        Console.WriteLine("Simbolo: {0}",moneda.symbol);
        Console.WriteLine("Rate:{0}",moneda.rate);
        Console.WriteLine("Descripcion:{0}",moneda.description);
        Console.WriteLine("Rate float:{0}",moneda.rate_float);
    }
}