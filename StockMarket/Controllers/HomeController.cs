using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockMarket.Models;
using StockMarket.Services;

namespace StockMarket.Controllers;

public class HomeController : Controller
{
    private readonly FinnhubService _finnhubService;
    private readonly TradingOptions _tradingOptions;
    private readonly IConfiguration _configuration;


    public HomeController(FinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
    {
        _finnhubService = finnhubService;
        _configuration = configuration;
        _tradingOptions = tradingOptions.Value;
    }

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
        {
            _tradingOptions.DefaultStockSymbol = "MSFT";
        }

        Dictionary<string, object>? stockQuoteDictionary  =
            await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);
        Dictionary<string, object>? companyProfileDictionary =
            await _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

        StockTrade stockTrade = new StockTrade()
        {
            StockSymbol = _tradingOptions.DefaultStockSymbol
        };

        if (companyProfileDictionary != null && stockQuoteDictionary  != null)
        {
            stockTrade = new StockTrade() 
                { StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]), 
                    StockName = Convert.ToString(companyProfileDictionary["name"]), 
                    Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()) };
        }

        ViewBag.Finnhub = _configuration["FinnhubToken"];
        return View(stockTrade);
    }
}