

namespace conversor
{
    using System.Threading.Tasks;
    using conversor.Pages;
    using conversor.Service;
    using Microsoft.AspNetCore.Mvc;

    public class CurrencyConverterController : Controller
    {
        private readonly CurrencyConverterService _currencyConverterService;

        public CurrencyConverterController(CurrencyConverterService currencyConverterService)
        {
            _currencyConverterService = currencyConverterService;
        }

        public IActionResult Conversor()
        {
            var model = new IndexModel();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConvertCurrency(string fromCurrency, string toCurrency, decimal amount)
        {
            try
            {
                var convertedAmount = await _currencyConverterService.ConvertCurrencyAsync(fromCurrency, toCurrency, amount);
                var model = new IndexModel
                {
                    ConvertedAmount = convertedAmount
                };
                return View("Index", model);
            }
            catch (Exception ex)
            {
                var model = new IndexModel
                {
                    ErrorMessage = ex.Message
                };
                return View("Index", model);
            }
        }
    }




}
