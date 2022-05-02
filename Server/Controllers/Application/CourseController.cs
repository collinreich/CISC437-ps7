using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Application.Crse
{
    public class CourseController : BaseController<Course>, iBaseController<Course>
    {

        public CourseController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

        [HttpGet]
        [Route("GetCourses")]
        public async Task<IActionResult> Get()
        {
            List<Course> lstCourses = await _context.Courses.OrderBy(x => x.CourseNo).ToListAsync();
            return Ok(lstCourses);
        }

        [HttpGet]
        [Route("GetCourses/{pCourseNo}")]
        public async Task<IActionResult> Get(int pCourseNo)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == pCourseNo).FirstOrDefaultAsync();
            return Ok(itmCourse);
        }

        [HttpDelete]
        [Route("DeleteCourse/{pCourseNo}")]
        public async Task<IActionResult> Delete(int pCourseNo)
        {
            Course itmCourse = await _context.Courses.Where(x => x.CourseNo == pCourseNo).FirstOrDefaultAsync();
            _context.Remove(itmCourse);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Course _Course)
        {

            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();


                if (existCourse == null)
                {
                    bExist = false;
                    existCourse = new Course();
                }
                else
                    bExist = true;

                existCourse.Cost = _Course.Cost;
                existCourse.Description = _Course.Description;
                existCourse.Prerequisite = _Course.Prerequisite;
                existCourse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                existCourse.SchoolId = _Course.SchoolId;
                if (bExist)
                    _context.Courses.Update(existCourse);
                else
                    _context.Courses.Add(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Course.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Course _Course)
        {

            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var existCourse = await _context.Courses.Where(x => x.CourseNo == _Course.CourseNo).FirstOrDefaultAsync();

                if (existCourse != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                existCourse = new Course();

                existCourse.Cost = _Course.Cost;
                existCourse.Description = _Course.Description;
                existCourse.Prerequisite = _Course.Prerequisite;
                existCourse.PrerequisiteSchoolId = _Course.PrerequisiteSchoolId;
                existCourse.SchoolId = _Course.SchoolId;
                _context.Courses.Add(existCourse);
                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Course.CourseNo);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        //[HttpPost]
        //[Route("GetCourses")]
        //public async Task<DataEnvelope<CourseDTO>> GetCoursesPost([FromBody] DataSourceRequest gridRequest)
        //{
        //    DataEnvelope<CourseDTO> dataToReturn = null;
        //    IQueryable<CourseDTO> queriableStates = _context.Courses
        //            .Select(sp => new CourseDTO
        //            {
        //                Cost = sp.Cost,
        //                CourseNo = sp.CourseNo,
        //                CreatedBy = sp.CreatedBy,
        //                CreatedDate = sp.CreatedDate,
        //                Description = sp.Description,
        //                ModifiedBy = sp.ModifiedBy,
        //                ModifiedDate = sp.ModifiedDate,
        //                Prerequisite = sp.Prerequisite,
        //                PrerequisiteSchoolId = sp.PrerequisiteSchoolId,
        //                SchoolId = sp.SchoolId
        //            });

        //    // use the Telerik DataSource Extensions to perform the query on the data
        //    // the Telerik extension methods can also work on "regular" collections like List<T> and IQueriable<T>
        //    try
        //    {

        //        DataSourceResult processedData = await queriableStates.ToDataSourceResultAsync(gridRequest);

        //        if (gridRequest.Groups.Count > 0)
        //        {
        //            // If there is grouping, use the field for grouped data
        //            // The app must be able to serialize and deserialize it
        //            // Example helper methods for this are available in this project
        //            // See the GroupDataHelper.DeserializeGroups and JsonExtensions.Deserialize methods
        //            dataToReturn = new DataEnvelope<CourseDTO>
        //            {
        //                GroupedData = processedData.Data.Cast<AggregateFunctionsGroup>().ToList(),
        //                TotalItemCount = processedData.Total
        //            };
        //        }
        //        else
        //        {
        //            // When there is no grouping, the simplistic approach of 
        //            // just serializing and deserializing the flat data is enough
        //            dataToReturn = new DataEnvelope<CourseDTO>
        //            {
        //                CurrentPageData = processedData.Data.Cast<CourseDTO>().ToList(),
        //                TotalItemCount = processedData.Total
        //            };
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //fixme add decent exception handling
        //    }
        //    return dataToReturn;
        //}

    }
}
