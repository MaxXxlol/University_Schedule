using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using University_Schedule.Models;

namespace University_Schedule.Controllers
{
    public class UniversityController : ApiController
    {
        UniversityDBEntities db = new UniversityDBEntities();
        
        // GET: api/University/StudentCourse
        [Route("api/University/StudentCourse")]
        public object[] GetStudentCourse()
        {
            var tmp = db.StudentSet;
            List<object> str = new List<object>();
            foreach (var x in tmp)
            {
                str.Add(new { FIO = x.FIO, Course = x.CourseSet });
            }
            return str.ToArray();
        }

        // GET: api/University/TeacherCourse
        [Route("api/University/TeacherCourse")]
        public object[] GetTeacherCourse()
        {
            var tmp = db.TeacherSet;
            List<object> str = new List<object>();
            foreach (var x in tmp)
            {
                str.Add(new { Id = x.Id, FIO = x.FIO, Course = x.CourseSet });
            }
            return str.ToArray();
        }

        // GET: api/University/StudentGrade
        [Route("api/University/StudentGrade")]
        public object[] GetStudentGrade()
        {
            var tmp = db.StudentSet;
            List<object> str = new List<object>();
            foreach (var x in tmp)
            {
                str.Add(new { FIO = x.FIO, Grade = x.Grade });
            }
            return str.ToArray();
        }

        // GET: api/University/StudentInfo
        [Route("api/University/StudentInfo")]
        public object[] GetStudentInfo()
        {
            var tmp = db.StudentSet;
            List<object> str = new List<object>();
            foreach (var x in tmp.OrderBy(y => y.FIO))
            {
                str.Add(new { Id = x.Id, FIO = x.FIO, Grade = x.Grade, Course = x.CourseSet });
            }
            return str.ToArray();
        }

        // GET: api/University/CourseWith5Student
        [Route("api/University/CourseWith5Student")]
        public object[] GetCourseWith5Student()
        {
            var tmp = db.CourseSet;
            List<object> str = new List<object>();
            foreach (var x in tmp.Where(x=>x.StudentSet.Count>2))
            {
                str.Add(new { Name = x.Name, Teacher = x.TeacherSet });
            }
            return str.ToArray();
        }

        // GET: api/University/TeacherWith1Course
        [Route("api/University/TeacherWith1Course")]
        public object[] GetTeacherWith1Course()
        {
            var tmp = db.TeacherSet;
            List<object> str = new List<object>();
            foreach (var x in tmp.Where(x => x.CourseSet.Count == 1))
            {
                str.Add(new { FIO = x.FIO, Course = x.CourseSet });
            }
            return str.ToArray();
        }

        // POST: api/University/NewStudent
        [Route("api/University/NewStudent")]
        public void PostNewStudent([FromBody]JObject value)
        {
            db.StudentSet.Add(new StudentSet(value["Id"].ToObject<int>(), value["FIO"].ToString(), value["Grade"].ToObject<int>()));
            db.SaveChanges();
        }

        // POST: api/University/AddCourseToStudent
        [Route("api/University/AddCourseToStudent")]
        public void PostAddCourseToStudent([FromBody]JObject value)
        {
            var subj = db.StudentSet.Find(value["SubjectId"].ToObject<int>());
            subj.AddCourse(db.CourseSet.Find(value["CourseId"].ToObject<int>()));
            db.SaveChanges();
        }

        // POST: api/University/NewTeacher
        [Route("api/University/NewTeacher")]
        public void PostNewTeacher([FromBody]JObject value)
        {
            db.TeacherSet.Add(new TeacherSet(value["Id"].ToObject<int>(), value["FIO"].ToString()));
            db.SaveChanges();
        }

        // POST: api/University/AddCourseToTeacher
        [Route("api/University/AddCourseToTeacher")]
        public void PostAddCourseToTeacher([FromBody]JObject value)
        {
            var subj = db.TeacherSet.Find(value["SubjectId"].ToObject<int>());
            subj.AddCourse(db.CourseSet.Find(value["CourseId"].ToObject<int>()));
            db.SaveChanges();
        }

        // POST: api/University/NewCourse
        [Route("api/University/NewCourse")]
        public void PostNewCourse([FromBody]JObject value)
        {
            db.CourseSet.Add(new CourseSet(value["Id"].ToObject<int>(), value["TeacherId"].ToObject<int>(), value["Name"].ToString()));
            db.SaveChanges();
        }

        // PUT: api/University/UpdateStudent
        [Route("api/University/UpdateStudent")]
        public void PutUpdateStudent([FromBody]JObject value)
        {
            db.StudentSet.Find(value["Id"].ToObject<int>()).UpdateStudentSet(value["FIO"].ToString(), value["Grade"].ToObject<int>());
            db.SaveChanges();
        }

        // PUT: api/University/UpdateTeacher
        [Route("api/University/UpdateTeacher")]
        public void PutUpdateTeacher([FromBody]JObject value)
        {
            db.TeacherSet.Find(value["Id"].ToObject<int>()).UpdateTeacherSet(value["FIO"].ToString());
        }

        // PUT: api/University/UpdateCourse
        [Route("api/University/UpdateCourse")]
        public void PutUpdateCourse([FromBody]JObject value)
        {
            db.CourseSet.Find(value["Id"].ToObject<int>()).UpdateCourseSet(value["TeacherId"].ToObject<int>(), value["Name"].ToString());
        }

        // DELETE: api/University/DeleteStudent
        [Route("api/University/DeleteStudent")]
        public void DeleteDeleteStudent([FromBody]JObject value)
        {
            db.StudentSet.Remove(db.StudentSet.Find(value["Id"].ToObject<int>()));
            db.SaveChanges();
        }

        // DELETE: api/University/DeleteTeacher
        [Route("api/University/DeleteTeacher")]
        public void DeleteDeleteTeacher([FromBody]JObject value)
        {
            db.TeacherSet.Remove(db.TeacherSet.Find(value["Id"].ToObject<int>()));
        }

        // DELETE: api/University/DeleteCourse
        [Route("api/University/DeleteCourse")]
        public void DeleteDeleteCourse([FromBody]JObject value)
        {
            db.CourseSet.Remove(db.CourseSet.Find(value["Id"].ToObject<int>()));
        }


    }
}
