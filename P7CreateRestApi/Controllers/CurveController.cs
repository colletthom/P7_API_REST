using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurveController : ControllerBase
    {
        private readonly LocalDbContext _context;
        private readonly CurveService _curveRepository;

        public CurveController(LocalDbContext context, CurveService curveRepository )
        {
            _context = context;
            _curveRepository = curveRepository;
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
        [Authorize(Policy = "AccessWriteActions")]
        //[Route("add")]
        [Route("")]
        public async Task<IActionResult> Add([FromBody]CurvePoint curvePoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var addCurve = await _curveRepository.AddCurve(curvePoint);
            return Ok(addCurve);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        //[Route("update/{id}")]
        [Route("{id}")]
        //public IActionResult ShowUpdateForm(int id)
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get CurvePoint by Id and to model then show to the form
            var _curve = await _curveRepository.GetCurveById(id);
            if (_curve == null)
                return NotFound();
            return Ok(_curve);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Curve and return Curve list
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updateCurve = await _curveRepository.UpdateCurveById(id, curvePoint);
            if (!updateCurve)
                return NotFound();

            var _curveList = _context.CurvePoints;
            return Ok(_curveList);
        }

        [HttpDelete]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        //public IActionResult DeleteBid(int id)
        public async Task<IActionResult> DeleteById (int id)
        {
            // TODO: Find Curve by Id and delete the Curve, return to Curve list
            var _curve = await _curveRepository.DeleteCurveById(id);
            if (!_curve)
                return NotFound();

            var _curveList = _context.CurvePoints;
            return Ok(_curveList);

        }
    }
}