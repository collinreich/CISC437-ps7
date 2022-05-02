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
    public class SectionController : BaseController<Section>, iBaseController<Section>
    {

        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }
        
        [HttpDelete]
        [Route("DeleteSection/{itemID}")]
        public async Task<IActionResult> Delete(int itemID)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionNo == itemID).FirstOrDefaultAsync();
            _context.Remove(itmSection);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetSections/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionNo == itemID).FirstOrDefaultAsync();
            return Ok(itmSection);
        }

        [HttpGet]
        [Route("GetSections")]
        public async Task<IActionResult> Get()
        {
            List<Section> lstSections = await _context.Sections.OrderBy(x => x.SectionNo).ToListAsync();
            return Ok(lstSections);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSection = await _context.Sections.Where(x => x.SectionNo == _Item.SectionNo).FirstOrDefaultAsync();

                if (existSection != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existSection = new Section();

                existSection.SectionId = _Item.SectionId;
                existSection.SectionNo = _Item.SectionNo;
                existSection.SectionNo = _Item.SectionNo;
                existSection.SchoolId = _Item.SchoolId;
                _context.Sections.Add(existSection);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.SectionNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existSection = await _context.Sections.Where(x => x.SectionNo == _Item.SectionNo).FirstOrDefaultAsync();


                if (existSection == null)
                {
                    bExist = false;
                    existSection = new Section();
                }
                else
                    bExist = true;

                existSection.SectionId = _Item.SectionId;
                existSection.SectionNo = _Item.SectionNo;
                existSection.SectionNo = _Item.SectionNo;
                existSection.SchoolId = _Item.SchoolId;
                if (bExist)
                    _context.Sections.Update(existSection);
                else
                    _context.Sections.Add(existSection);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.SectionNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
