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
    public class GradeConversionController : BaseController<GradeConversion>, iBaseController<GradeConversion>
    {

        public GradeConversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

        [HttpDelete]
        [Route("DeleteGradeConversion/{itemID}")]
        public async Task<IActionResult> Delete(int itemID)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == itemID).FirstOrDefaultAsync();
            _context.Remove(itmGradeConversion);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetGradeConversions/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == itemID).FirstOrDefaultAsync();
            return Ok(itmGradeConversion);
        }

        [HttpGet]
        [Route("GetGradeConversions")]
        public async Task<IActionResult> Get()
        {
            List<GradeConversion> lstGradeConversions = await _context.GradeConversions.OrderBy(x => x.SchoolId).ToListAsync();
            return Ok(lstGradeConversions);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

                if (existGradeConversion != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existGradeConversion = new GradeConversion();

                
                existGradeConversion.SchoolId = _Item.SchoolId;
                existGradeConversion.LetterGrade = _Item.LetterGrade;
                existGradeConversion.GradePoint = _Item.GradePoint;
                existGradeConversion.MaxGrade = _Item.MaxGrade;
                existGradeConversion.MinGrade = _Item.MinGrade;
                _context.GradeConversions.Add(existGradeConversion);
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
        public async Task<IActionResult> Put([FromBody] GradeConversion _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();


                if (existGradeConversion == null)
                {
                    bExist = false;
                    existGradeConversion = new GradeConversion();
                }
                else
                    bExist = true;

                existGradeConversion.SchoolId = _Item.SchoolId;
                existGradeConversion.LetterGrade = _Item.LetterGrade;
                existGradeConversion.GradePoint = _Item.GradePoint;
                existGradeConversion.MaxGrade = _Item.MaxGrade;
                existGradeConversion.MinGrade = _Item.MinGrade;
                if (bExist)
                    _context.GradeConversions.Update(existGradeConversion);
                else
                    _context.GradeConversions.Add(existGradeConversion);
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
