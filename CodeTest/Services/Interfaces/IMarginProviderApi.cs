using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaIngreso.Services.Interfaces
{
    public interface IMarginProviderApi
    {
        Task<double> GetMarginAsync(string code);
    }
}
