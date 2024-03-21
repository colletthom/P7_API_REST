using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Repositories
{
    public class CurveService
    {
        private LocalDbContext _context { get; }

        public CurveService(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<CurvePoint> AddCurve(CurvePoint curve)
        {
            var _curve = new CurvePoint
            {
                CurveId = curve.CurveId,
                AsOfDate = curve.AsOfDate,
                Term = curve.Term,
                CurvePointValue = curve.CurvePointValue,
                CreationDate = curve.CreationDate
            };
            _context.CurvePoints.Add(_curve);
            await _context.SaveChangesAsync();
            return _curve;
        }

        public async Task<CurvePoint> GetCurveById(int id)
        {
            return await _context.CurvePoints.FindAsync(id);
        }

        public async Task<bool> UpdateCurveById(int id, CurvePoint curve)
        {
            var _curve = _context.CurvePoints.Find(id);
            if (_curve == null)
            {
                return false;
            }

            _curve.CurveId = curve.CurveId;
            _curve.AsOfDate = curve.AsOfDate;
            _curve.Term = curve.Term;
            _curve.CurvePointValue = curve.CurvePointValue;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCurveById(int id)
        {
            var curve = await _context.CurvePoints.FindAsync(id);
            if (curve == null)
            {
                return false; // Or throw an exception
            }

            _context.CurvePoints.Remove(curve);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
