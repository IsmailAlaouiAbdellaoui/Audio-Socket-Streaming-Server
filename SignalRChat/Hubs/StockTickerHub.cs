using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs 
{
    public class StockTickerHub : Hub
    {
        private readonly StockTicker _stockTicker;

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("from_client",user,message);
            
        }
        public string test(string line)
        {
            //Clients.All.SendAsync("from_client", user, message);
            return "test used , line is " + line;

        }

        public StockTickerHub(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return _stockTicker.GetAllStocks();
        }

        public ChannelReader<Stock> StreamStocks()
        {
            return _stockTicker.StreamStocks().AsChannelReader(10);
        }

        public string GetMarketState()
        {
            return _stockTicker.MarketState.ToString();
        }

        public async Task OpenMarket()
        {
            await _stockTicker.OpenMarket();
        }

        public async Task CloseMarket()
        {
            await _stockTicker.CloseMarket();
        }

        public async Task Reset()
        {
            await _stockTicker.Reset();
        }

    }
}
