using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Repositories
{
    public class BidRepository
    {
        private LocalDbContext _context { get; } 

        public BidRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<Bid> AddBid(Bid bid)
        {
            var _bid = new Bid
            {
                Account = bid.Account,
                BidType = bid.BidType,
                BidQuantity = bid.BidQuantity,
                AskQuantity = bid.AskQuantity,
                Bid2 = bid.Bid2,
                Ask = bid.Ask,
                Benchmark = bid.Benchmark,
                BidListDate = bid.BidListDate,
                Commentary = bid.Commentary,
                BidSecurity = bid.BidSecurity,
                BidStatus = bid.BidStatus,
                Trader = bid.Trader,
                Book = bid.Book,
                CreationName = bid.CreationName,
                CreationDate = bid.CreationDate,
                RevisionName = bid.RevisionName,
                RevisionDate = bid.RevisionDate,
                DealName = bid.DealName,
                DealType = bid.DealType,
                SourceListId = bid.SourceListId,
                Side = bid.Side
            };
            _context.Bids.Add(_bid);
            await _context.SaveChangesAsync();
            return _bid;
        }

        public async Task<Bid> GetBidById(int id)
        {
            return await _context.Bids.FindAsync(id);
        }

        public async Task<Bid> UpdateBid(int id, Bid bid)
        {
            var _bid = _context.Bids.Find(id);
            if (_bid == null)
            {
                return null;
            }

            _bid.Account = bid.Account;
            _bid.BidType = bid.BidType;
            _bid.BidQuantity = bid.BidQuantity;
            _bid.AskQuantity = bid.AskQuantity;
            _bid.Bid2 = bid.Bid2;
            _bid.Ask = bid.Ask;
            _bid.Benchmark = bid.Benchmark;
            _bid.BidListDate = bid.BidListDate;
            _bid.Commentary = bid.Commentary;
            _bid.BidSecurity = bid.BidSecurity;
            _bid.BidStatus = bid.BidStatus;
            _bid.Trader = bid.Trader;
            _bid.Book = bid.Book;
            _bid.CreationName = bid.CreationName;
            _bid.CreationDate = bid.CreationDate;
            _bid.RevisionName = bid.RevisionName;
            _bid.RevisionDate = bid.RevisionDate;
            _bid.DealName = bid.DealName;
            _bid.DealType = bid.DealType;
            _bid.SourceListId = bid.SourceListId;
            _bid.Side = bid.Side;

            await _context.SaveChangesAsync();

            return _bid;
        }

        public async Task<bool> DeleteBid(int id)
        {
            var bid = await _context.Bids.FindAsync(id);
            if (bid == null)
            {
                return false; // Or throw an exception
            }

            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
