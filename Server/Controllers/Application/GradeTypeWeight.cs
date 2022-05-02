using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Application
{
    public class GradeTypeWeightController : BaseController<GradeTypeWeight>, iBaseController<GradeTypeWeight>
    {

        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

        [HttpDelete]
        [Route("DeleteGradeTypeWeight/{itemID}")]
        public async Task<IActionResult> Delete(int itemID)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == itemID).FirstOrDefaultAsync();
            _context.Remove(itmGradeTypeWeight);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetGradeTypeWeights/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == itemID).FirstOrDefaultAsync();
            return Ok(itmGradeTypeWeight);
        }

        [HttpGet]
        [Route("GetGradeTypeWeights")]
        public async Task<IActionResult> Get()
        {
            List<GradeTypeWeight> lstGradeTypeWeights = await _context.GradeTypeWeights.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstGradeTypeWeights);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (existGradeTypeWeight != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existGradeTypeWeight = new GradeTypeWeight();

                existGradeTypeWeight.SchoolId = _Item.SchoolId;
                existGradeTypeWeight.SectionId = _Item.SectionId;
                existGradeTypeWeight.GradeTypeCode = _Item.GradeTypeCode;
                _context.GradeTypeWeights.Add(existGradeTypeWeight);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();


                if (existGradeTypeWeight == null)
                {
                    bExist = false;
                    existGradeTypeWeight = new GradeTypeWeight();
                }
                else
                    bExist = true;

                existGradeTypeWeight.SchoolId = _Item.SchoolId;
                existGradeTypeWeight.SectionId = _Item.SectionId;
                existGradeTypeWeight.GradeTypeCode = _Item.GradeTypeCode;
                if (bExist)
                    _context.GradeTypeWeights.Update(existGradeTypeWeight);
                else
                    _context.GradeTypeWeights.Add(existGradeTypeWeight);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
