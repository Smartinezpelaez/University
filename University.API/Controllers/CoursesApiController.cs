using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using University.API.Models;

namespace University.API.Controllers
{

    public class CoursesApiController : ApiController
    {
       
        private UniversityEntities db = new UniversityEntities();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Listado de cursos</returns>
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var courses = db.Course.ToList().Select(x=> CourseToCourseDTO(x));
            return Ok(courses);
        }
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            var course = CourseToCourseDTO(db.Course.Find(id));
            return Ok(course);
        }

        [Authorize]
        [HttpPost]
        public IHttpActionResult Post (Course courseDto)
        {
            var course = new Course {
                CourseID = courseDto.CourseID,
                Title= courseDto.Title,
                Credits = courseDto.Credits
            };
             course = db.Course.Add(course);
            db.SaveChanges();
            return Ok(course);
        }

        [HttpPut]
        public IHttpActionResult Put(Course course)
        {
            db.Course.AddOrUpdate(course);
            db.SaveChanges();
            return Ok(course);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var course = db.Course.Find(id);
            if (course == null)
                return NotFound();
            db.Course.Remove(course);
            db.SaveChanges();
            return Ok();
        }



        private CourseDto CourseToCourseDTO(Course course)
        {
            return new CourseDto
            {
                CourseID = course.CourseID,
                Title=course.Title,
                Credits=course.Credits.Value

            };

        }
            
            public class CourseDto
        {
            public int CourseID { get; set; }
            public String Title { get; set; }
            public int Credits { get; set; }
        }
    }
}
