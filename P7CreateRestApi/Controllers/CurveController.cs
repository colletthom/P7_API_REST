using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public CurveController(LocalDbContext context)
        {
            _context = context;
        }
        // TODO: Inject Curve Point service

        /*[HttpGet]
        //[Route("liste")]
        //public IActionResult Home()
        {
            var _curve = _context.CurvePoints;
            if (_curve == null)
                return NotFound();
            return Ok(_curve);
        }*/

        /*
        [HttpGet]
        [Route("validate")]
        public IActionResult Validate([FromBody]CurvePoint curvePoint)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return Ok();
        }*/

        //[HttpGet]
        [HttpPost]
        //[Route("add")]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]CurvePoint curvePoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _curve = new CurvePoint
            {
                
                Id = curvePoint.Id,
                CurveId = curvePoint.CurveId,
                AsOfDate = curvePoint.AsOfDate,
                Term = curvePoint.Term,
                CurvePointValue = curvePoint.CurvePointValue,
                CreationDate = curvePoint.CreationDate
            };
            _context.CurvePoints.Add(_curve);
            await _context.SaveChangesAsync();
            return Ok(_curve);
        }

        [HttpGet]
        //[Route("update/{id}")]
        [Route("{id}")]
        //public IActionResult ShowUpdateForm(int id)
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get CurvePoint by Id and to model then show to the form
            var _curve = await _context.CurvePoints.FindAsync(id);
            if (_curve == null)
                return NotFound();
            return Ok(_curve);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Curve and return Curve list
            var _curve = _context.CurvePoints.Find(id);
            if (_curve == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _curve.CurveId = curvePoint.CurveId;
            _curve.AsOfDate = curvePoint.AsOfDate;
            _curve.Term = curvePoint.Term;
            _curve.CurvePointValue = curvePoint.CurvePointValue;

            await _context.SaveChangesAsync();

            var _curveList = _context.CurvePoints;
            return Ok(_curveList);
        }

        [HttpDelete]
        [Route("{id}")]
        //public IActionResult DeleteBid(int id)
        public async Task<IActionResult> DeleteById (int id)
        {
            // TODO: Find Curve by Id and delete the Curve, return to Curve list
            var _curve = _context.CurvePoints.Find(id);
            if (_curve == null)
                return NotFound();

            _context.CurvePoints.Remove(_curve);
            await _context.SaveChangesAsync();

            var _curveList = _context.CurvePoints;
            return Ok(_curveList);

        }
    }
}