using System;
using System.Threading.Tasks;
using Tutorials.Currency.Models;

namespace Tutorials.Currency
{
    public interface ICurrencyService
    {
        Task<CurrencyModel[]> GetToday();
        Task<CurrencyModel[]> GetByDate(DateTime date);
    }
}