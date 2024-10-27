namespace StockMarket.ServicesContract;

public interface IFinnhubService
{
    Task<Dictionary<string, object?>> GetStockPriceQuote(string strockSymbol);
}