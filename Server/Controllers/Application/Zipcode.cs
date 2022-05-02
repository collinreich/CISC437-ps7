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
    public class ZipcodeController : BaseController<Zipcode>, iBaseController<Zipcode>
    {

        public ZipcodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

        [HttpDelete]
        [Route("DeleteZipcode/{itemID}")]
        public async Task<IActionResult> Delete(int itemID)
        {
            Zipcode itmZipcode = await _context.Zipcodes.Where(x => x.Zip == itemID.ToString()).FirstOrDefaultAsync();
            _context.Remove(itmZipcode);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("GetZipcodes/{itemID}")]
        public async Task<IActionResult> Get(int itemID)
        {
            Zipcode itmZipcode = await _context.Zipcodes.Where(x => x.Zip == itemID.ToString()).FirstOrDefaultAsync();
            return Ok(itmZipcode);
        }

        [HttpGet]
        [Route("GetZipcodes")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> lstZipcodes = await _context.Zipcodes.OrderBy(x => x.Zip).ToListAsync();
            return Ok(lstZipcodes);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existZipcode = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();

                if (existZipcode != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existZipcode = new Zipcode();

                existZipcode.Zip = _Item.Zip;
                existZipcode.City = _Item.City;
                existZipcode.State = _Item.State;
                _context.Zipcodes.Add(existZipcode);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Item)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existZipcode = await _context.Zipcodes.Where(x => x.Zip == _Item.Zip).FirstOrDefaultAsync();


                if (existZipcode == null)
                {
                    bExist = false;
                    existZipcode = new Zipcode();
                }
                else
                    bExist = true;

                existZipcode.Zip = _Item.Zip;
                existZipcode.City = _Item.City;
                existZipcode.State = _Item.State;
                if (bExist)
                    _context.Zipcodes.Update(existZipcode);
                else
                    _context.Zipcodes.Add(existZipcode);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Item.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
