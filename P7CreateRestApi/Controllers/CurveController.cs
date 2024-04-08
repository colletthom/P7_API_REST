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
        private readonly LogService _logService;

        public CurveController(LocalDbContext context, CurveService curveRepository, LogService logService)
        {
            _context = context;
            _curveRepository = curveRepository;
            _logService = logService;
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
            string logDescription = "Le AddCurve a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le AddBid a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 1, 2, logDescription);
                return BadRequest(ModelState);
            }
            var addCurve = await _curveRepository.AddCurve(curvePoint);
            await _logService.CreateLog(HttpContext, 1, 2, logDescription);
            return Ok(addCurve);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var _curve = await _context.CurvePoints.ToListAsync();
            string logDescription = "Le GetAll a réussi";

            await _logService.CreateLog(HttpContext, 2, 2, logDescription);
            return Ok(_curve);
        }

        [HttpGet]
        [Authorize(Policy = "AccessGetAction")]
        //[Route("update/{id}")]
        [Route("{id}")]
        //public IActionResult ShowUpdateForm(int id)
        public async Task<IActionResult> GetById(int id)
        {
            // TODO: get CurvePoint by Id and to model then show to the form
            var _curve = await _context.CurvePoints.FindAsync(id);
            string logDescription = "Le GetCurveById a réussi";
            if (_curve == null)
            {
                logDescription = "Le GetCurveById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 3, 2, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 3, 2, logDescription);
            return Ok(_curve);
        }

        [HttpPut]
        [Authorize(Policy = "AccessWriteActions")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateById(int id, [FromBody] CurvePoint curvePoint)
        {
            // TODO: check required fields, if valid call service to update Curve and return Curve list
            string logDescription = "Le UpdateCurveById a réussi";
            if (!ModelState.IsValid)
            {
                logDescription = "Le UpdateCurveById a échoué Model non valide";
                await _logService.CreateLog(HttpContext, 4, 2, logDescription);
                return BadRequest(ModelState);
            }                

            var updateCurve = await _curveRepository.UpdateCurveById(id, curvePoint);
            if (!updateCurve)
            {
                logDescription = "Le UpdateCurveById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 4, 2, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 4, 2, logDescription);
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
            string logDescription = "Le DeleteCurveById a réussi";
            var _curve = await _curveRepository.DeleteCurveById(id);
            if (!_curve)
            {
                logDescription = "Le DeleteCurveById a échoué {id} non trouvé";
                await _logService.CreateLog(HttpContext, 5, 2, logDescription);
                return NotFound();
            }

            await _logService.CreateLog(HttpContext, 5, 2, logDescription);
            var _curveList = _context.CurvePoints;
            return Ok(_curveList);

        }
    }
}