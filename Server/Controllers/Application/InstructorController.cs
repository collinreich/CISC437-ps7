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
    public class InstructorController : BaseController<Instructor>, iBaseController<Instructor>
    {

        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

        [HttpDelete]
        [Route("DeleteInstructor/{itemID}")]
        public async Task<IActionResult> Delete(int itemID)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.InstructorId == itemID).FirstOrDefaultAsync();
            _context.Remove(itmInstructor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetInstructors/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.InstructorId == itemID).FirstOrDefaultAsync();
            return Ok(itmInstructor);
        }

        [HttpGet]
        [Route("GetInstructors")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> lstInstructors = await _context.Instructors.OrderBy(x => x.InstructorId).ToListAsync();
            return Ok(lstInstructors);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existInstructor = await _context.Instructors.Where(x => x.InstructorId == _Item.InstructorId).FirstOrDefaultAsync();

                if (existInstructor != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existInstructor = new Instructor();

                existInstructor.InstructorId = _Item.InstructorId;
                existInstructor.SchoolId = _Item.SchoolId;
                _context.Instructors.Add(existInstructor);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existInstructor = await _context.Instructors.Where(x => x.InstructorId == _Item.InstructorId).FirstOrDefaultAsync();


                if (existInstructor == null)
                {
                    bExist = false;
                    existInstructor = new Instructor();
                }
                else
                    bExist = true;

                existInstructor.InstructorId = _Item.InstructorId;
                existInstructor.SchoolId = _Item.SchoolId;
                if (bExist)
                    _context.Instructors.Update(existInstructor);
                else
                    _context.Instructors.Add(existInstructor);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
