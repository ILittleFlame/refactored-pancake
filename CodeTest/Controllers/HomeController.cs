using System;
using System.Linq;
using System.Web.Mvc;
using Quote.Contracts;
using Quote.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using PruebaIngreso.Models;

namespace PruebaIngreso.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteEngine quote;

        public HomeController(IQuoteEngine quote)
        {
            this.quote = quote;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls12;

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                TourCode = "E-U10-PRVPARKTRF",
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            var tour = result.Tours.FirstOrDefault();
            ViewBag.Message = "Test 1 Correcto";
            return View(tour);
        }

        public ActionResult Test2()
        {
            ViewBag.Message = "Test 2 Correcto";
            return View();
        }

      
        public async Task<ActionResult> Test3()
            {
                var serviceCode = "E-U10-UNILATIN"; // Se puede cambiar esto a otros códigos para probar
                var margin = await GetMarginFromApi(serviceCode);

                ViewBag.Margin = margin;

                return View();
            }

            private async Task<double> GetMarginFromApi(string serviceCode)
            {
                var apiUrl = $"https://refactored-pancake.free.beeceptor.com/margin/{serviceCode}";

                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var response = await httpClient.GetAsync(apiUrl);
                        //Validacion del codigo de estado
                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();
                            var marginData = JsonConvert.DeserializeObject<MarginResponse>(responseData);
                            return marginData.Margin;
                        }
                        else
                        {
                            return 0.0;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores 
                        Console.WriteLine($"Error al invocar la API: {ex.Message}");
                        return 0.0;
                    }
                }
            }
            public ActionResult Test4()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            return View(result.TourQuotes);
        }
    }
}