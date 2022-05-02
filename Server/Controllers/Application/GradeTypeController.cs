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
    public class GradeTypeController : BaseController<GradeType>, iBaseController<GradeType>
    {

        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

        [HttpDelete]
        [Route("DeleteGradeType/{itemID}")]
        public async Task<IActionResult> Delete(int itemID)
        {
            GradeType itmGradeType = await _context.GradeTypes.Where(x => x.SchoolId == itemID).FirstOrDefaultAsync();
            _context.Remove(itmGradeType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetGradeTypes/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            GradeType itmGradeType = await _context.GradeTypes.Where(x => x.SchoolId == itemID).FirstOrDefaultAsync();
            return Ok(itmGradeType);
        }

        [HttpGet]
        [Route("GetGradeTypes")]
        public async Task<IActionResult> Get()
        {
            List<GradeType> lstGradeTypes = await _context.GradeTypes.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstGradeTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeType = await _context.GradeTypes.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (existGradeType != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existGradeType = new GradeType();

                
                existGradeType.SchoolId = _Item.SchoolId;
                existGradeType.GradeTypeCode = _Item.GradeTypeCode;
                existGradeType.GradeTypeWeights = _Item.GradeTypeWeights;
                _context.GradeTypes.Add(existGradeType);
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
        public async Task<IActionResult> Put([FromBody] GradeType _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeType = await _context.GradeTypes.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();


                if (existGradeType == null)
                {
                    bExist = false;
                    existGradeType = new GradeType();
                }
                else
                    bExist = true;

                existGradeType.SchoolId = _Item.SchoolId;
                existGradeType.GradeTypeCode = _Item.GradeTypeCode;
                existGradeType.GradeTypeWeights = _Item.GradeTypeWeights;
                if (bExist)
                    _context.GradeTypes.Update(existGradeType);
                else
                    _context.GradeTypes.Add(existGradeType);
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
