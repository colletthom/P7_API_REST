using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Repositories
{
    public class TradeService
    {
        private LocalDbContext _context { get; }

        public TradeService(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<Trade> AddTrade(Trade trade)
        {
            var _trade = new Trade
            {
                Account = trade.Account,
                AccountType = trade.AccountType,
                SellQuantity = trade.SellQuantity,
                BuyPrice = trade.BuyPrice,
                SellPrice = trade.SellPrice,
                TradeDate = trade.TradeDate,
                TradeSecurity = trade.TradeSecurity,
                TradeStatus = trade.TradeStatus,
                Trader = trade.Trader,
                Benchmark = trade.Benchmark,
                Book = trade.Book,
                CreationName = trade.CreationName,
                CreationDate = trade.CreationDate,
                RevisionName = trade.RevisionName,
                RevisionDate = trade.RevisionDate,
                DealName = trade.DealName,
                DealType = trade.DealType,
                SourceListId = trade.SourceListId,
                Side = trade.Side,
            };
            _context.Trades.Add(_trade);
            await _context.SaveChangesAsync();
            return _trade;
        }

        public async Task<Trade> GetTradeById(int id)
        {
            return await _context.Trades.FindAsync(id);
        }

        public async Task<bool> UpdateTradeById(int id, Trade trade)
        {
            var _trade = _context.Trades.Find(id);
            if (_trade == null)
            {
                return false;
            }

            _trade.Account = trade.Account;
            _trade.AccountType = trade.AccountType;
            _trade.SellQuantity = trade.SellQuantity;
            _trade.BuyPrice = trade.BuyPrice;
            _trade.SellPrice = trade.SellPrice;
            _trade.TradeDate = trade.TradeDate;
            _trade.TradeSecurity = trade.TradeSecurity;
            _trade.TradeStatus = trade.TradeStatus;
            _trade.Trader = trade.Trader;
            _trade.Benchmark = trade.Benchmark;
            _trade.Book = trade.Book;
            _trade.CreationName = trade.CreationName;
            _trade.CreationDate = trade.CreationDate;
            _trade.RevisionName = trade.RevisionName;
            _trade.RevisionDate = trade.RevisionDate;
            _trade.DealName = trade.DealName;
            _trade.DealType = trade.DealType;
            _trade.SourceListId = trade.SourceListId;
            _trade.Side = trade.Side;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTradeById(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade == null)
            {
                return false; // Or throw an exception
            }

            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
