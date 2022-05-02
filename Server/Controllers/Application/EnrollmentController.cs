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
    public class EnrollmentController : BaseController<Enrollment>, iBaseController<Enrollment>
    {

        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

        [HttpDelete]
        [Route("DeleteEnrollment/{itemID}")]
        public async Task<IActionResult> Delete(int itemID)
        {
            Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.StudentId == itemID).FirstOrDefaultAsync();
            _context.Remove(itmEnrollment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetEnrollments/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.StudentId == itemID).FirstOrDefaultAsync();
            return Ok(itmEnrollment);
        }

        [HttpGet]
        [Route("GetEnrollments")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> lstEnrollments = await _context.Enrollments.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstEnrollments);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existEnrollment = await _context.Enrollments.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();

                if (existEnrollment != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existEnrollment = new Enrollment();

                existEnrollment.StudentId = _Item.StudentId;
                existEnrollment.SectionId = _Item.SectionId;
                existEnrollment.SchoolId = _Item.SchoolId;
                _context.Enrollments.Add(existEnrollment);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existEnrollment = await _context.Enrollments.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();


                if (existEnrollment == null)
                {
                    bExist = false;
                    existEnrollment = new Enrollment();
                }
                else
                    bExist = true;

                existEnrollment.StudentId = _Item.StudentId;
                existEnrollment.SectionId = _Item.SectionId;
                existEnrollment.SchoolId = _Item.SchoolId;
                if (bExist)
                    _context.Enrollments.Update(existEnrollment);
                else
                    _context.Enrollments.Add(existEnrollment);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
