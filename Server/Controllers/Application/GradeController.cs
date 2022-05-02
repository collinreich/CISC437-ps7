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
    public class GradeController : BaseController<Grade>, iBaseController<Grade>
    {

        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

        [HttpDelete]
        [Route("DeleteGrade/{itemID}")]
        public async Task<IActionResult> Delete(int itemID)
        {
            Grade itmGrade = await _context.Grades.Where(x => x.StudentId == itemID).FirstOrDefaultAsync();
            _context.Remove(itmGrade);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetGrades/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            Grade itmGrade = await _context.Grades.Where(x => x.StudentId == itemID).FirstOrDefaultAsync();
            return Ok(itmGrade);
        }

        [HttpGet]
        [Route("GetGrades")]
        public async Task<IActionResult> Get()
        {
            List<Grade> lstGrades = await _context.Grades.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstGrades);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGrade = await _context.Grades.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();

                if (existGrade != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existGrade = new Grade();

                existGrade.SchoolId = _Item.SchoolId;
                existGrade.StudentId = _Item.StudentId;
                existGrade.SectionId = _Item.SectionId;
                existGrade.GradeTypeCode = _Item.GradeTypeCode;
                existGrade.GradeCodeOccurrence = _Item.GradeCodeOccurrence;
                _context.Grades.Add(existGrade);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existGrade = await _context.Grades.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();


                if (existGrade == null)
                {
                    bExist = false;
                    existGrade = new Grade();
                }
                else
                    bExist = true;

                existGrade.SchoolId = _Item.SchoolId;
                existGrade.StudentId = _Item.StudentId;
                existGrade.SectionId = _Item.SectionId;
                existGrade.GradeTypeCode = _Item.GradeTypeCode;
                existGrade.GradeCodeOccurrence = _Item.GradeCodeOccurrence;
                if (bExist)
                    _context.Grades.Update(existGrade);
                else
                    _context.Grades.Add(existGrade);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
