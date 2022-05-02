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
    public class StudentController : BaseController<Student>, iBaseController<Student>
    {

        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

        [HttpDelete]
        [Route("DeleteStudent/{itemID}")]
        public async Task<IActionResult> Delete(int itemID)
        {
            Student itmStudent = await _context.Students.Where(x => x.StudentId == itemID).FirstOrDefaultAsync();
            _context.Remove(itmStudent);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetStudents/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            Student itmStudent = await _context.Students.Where(x => x.StudentId == itemID).FirstOrDefaultAsync();
            return Ok(itmStudent);
        }

        [HttpGet]
        [Route("GetStudents")]
        public async Task<IActionResult> Get()
        {
            List<Student> lstStudents = await _context.Students.OrderBy(x => x.StudentId).ToListAsync();
            return Ok(lstStudents);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existStudent = await _context.Students.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();

                if (existStudent != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existStudent = new Student();

                existStudent.StudentId = _Item.StudentId;
                existStudent.SchoolId = _Item.SchoolId;
                _context.Students.Add(existStudent);
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
        public async Task<IActionResult> Put([FromBody] Student _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existStudent = await _context.Students.Where(x => x.StudentId == _Item.StudentId).FirstOrDefaultAsync();


                if (existStudent == null)
                {
                    bExist = false;
                    existStudent = new Student();
                }
                else
                    bExist = true;

                existStudent.StudentId = _Item.StudentId;
                existStudent.SchoolId = _Item.SchoolId;
                if (bExist)
                    _context.Students.Update(existStudent);
                else
                    _context.Students.Add(existStudent);
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
