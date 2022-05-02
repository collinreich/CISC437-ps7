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

    public class SchoolController : BaseController<School>, iBaseController<School>
    {
        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

    [HttpDelete]
    [Route("DeleteSchool/{itemID}")]
    public async Task<IActionResult> Delete(int itemID)
    {
        School itmSchool = await _context.Schools.Where(x => x.SchoolId == itemID).FirstOrDefaultAsync();
        _context.Remove(itmSchool);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    [Route("GetSchools/{itemID}")]
    public async Task<IActionResult> Get(int itemID)
    {
        School itmSchool = await _context.Schools.Where(x => x.SchoolId == itemID).FirstOrDefaultAsync();
        return Ok(itmSchool);
    }

    [HttpGet]
    [Route("GetSchools")]
    public async Task<IActionResult> Get()
    {
        List<School> lstSchools = await _context.Schools.OrderBy(x => x.SchoolId).ToListAsync();
        return Ok(lstSchools);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] School _Item)
    {
        bool bExist = false;

        var trans = _context.Database.BeginTransaction();
        try
        {
            var existSchool = await _context.Schools.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();

            if (existSchool != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
            }

            existSchool = new School();

            existSchool.SchoolId = _Item.SchoolId;
            _context.Schools.Add(existSchool);
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
    public async Task<IActionResult> Put([FromBody] School _Item)
    {
        bool bExist = false;

        var trans = _context.Database.BeginTransaction();
        try
        {
            var existSchool = await _context.Schools.Where(x => x.SchoolId == _Item.SchoolId).FirstOrDefaultAsync();


            if (existSchool == null)
            {
                bExist = false;
                existSchool = new School();
            }
            else
                bExist = true;

                existSchool.SchoolId = _Item.SchoolId;
                if (bExist)
                _context.Schools.Update(existSchool);
            else
                _context.Schools.Add(existSchool);
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
