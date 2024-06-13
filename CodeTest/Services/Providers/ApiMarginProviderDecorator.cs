using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PruebaIngreso.Services.Interfaces;
using PruebaIngreso.Models;

public class ApiMarginProviderDecorator : IMarginProviderApi
{
    private readonly IMarginProviderApi inner;
    private readonly HttpClient httpClient;

    public ApiMarginProviderDecorator(IMarginProviderApi inner, HttpClient httpClient)
    {
        this.inner = inner;
        this.httpClient = httpClient;
    }

    public async Task<double> GetMarginAsync(string code)
    {
        try
        {
            string url = $"https://refactored-pancake.free.beeceptor.com/margin/{code}";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<MarginResponse>(responseBody);
                return responseObject.Margin;
            }
        }
        catch (HttpRequestException)
        {
            // Manejo de excepciones (opcional)
        }

        // Si no hay respuesta exitosa, devuelve el margen por defecto
        return await inner.GetMarginAsync(code);
    }

}
