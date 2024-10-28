namespace StockMarket.ServicesContract;

public interface IFinnhubService
{
    Task<Dictionary<string, object?>> GetStockPriceQuote(string strockSymbol);
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);


}