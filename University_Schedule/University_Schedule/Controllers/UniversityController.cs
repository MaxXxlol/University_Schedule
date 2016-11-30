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

        // GET: api/University/Student/id
        [Route("api/University/Student/{id}")]
        public object GetStudent(int id)
        {
            try
            {
                var tmp = db.StudentSet.Find(id);
                object student = new { Id = tmp.Id, FIO = tmp.FIO, Grade = tmp.Grade, Course = tmp.CourseSet };
                return student;
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.NotFound);
                return ex.Message;
            }
            
        }

        // GET: api/University/Teacher/id
        [Route("api/University/Teacher/{id}")]
        public object GetTeacher(int id)
        {
            try
            {
                var tmp = db.TeacherSet.Find(id);
                object teacher = new { Id = tmp.Id, FIO = tmp.FIO, Course = tmp.CourseSet };
                return teacher;
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.NotFound);
                return ex.Message;
            }
        }
        // GET: api/University/Course/id
        [Route("api/University/Course/{id}")]
        public object GetCourse(int id)
        {
            try
            {
                var tmp = db.CourseSet.Find(id);
                object course = new { Id = tmp.Id, teacher = tmp.TeacherSet, students = tmp.StudentSet };
                return course;
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.NotFound);
                return ex.Message;
            }
        }

        // GET: api/University/Student
        [Route("api/University/Student")]
        public object[] GetStudent()
        {
            try
            {
                var tmp = db.StudentSet;
                List<object> str = new List<object>();
                foreach (var x in tmp)
                {
                    str.Add(new { FIO = x.FIO, Course = x.CourseSet });
                }
                return str.ToArray();
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.NotFound);
                return new[] {ex.Message};
            }
        }

        // GET: api/University/Teacher
        [Route("api/University/Teacher")]
        public object[] GetTeacher()
        {
            try
            {
                var tmp = db.TeacherSet;
                List<object> str = new List<object>();
                foreach (var x in tmp)
                {
                    str.Add(new { Id = x.Id, FIO = x.FIO, Course = x.CourseSet });
                }
                return str.ToArray();
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.NotFound);
                return new[] { ex.Message };
            }
        }

        // GET: api/University/StudentGrade
        [Route("api/University/StudentGrade")]
        public object[] GetStudentGrade()
        {
            try
            {
                var tmp = db.StudentSet;
                List<object> str = new List<object>();
                foreach (var x in tmp)
                {
                    str.Add(new { FIO = x.FIO, Grade = x.Grade });
                }
                return str.ToArray();
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.NotFound);
                return new[] { ex.Message };
            }
        }

        // GET: api/University/StudentInfo
        [Route("api/University/StudentInfo")]
        public object[] GetStudentInfo()
        {
            try
            {
                var tmp = db.StudentSet;
                List<object> str = new List<object>();
                foreach (var x in tmp.OrderBy(y => y.FIO))
                {
                    str.Add(new { Id = x.Id, FIO = x.FIO, Grade = x.Grade, Course = x.CourseSet });
                }
                return str.ToArray();
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.NotFound);
                return new[] { ex.Message };
            }
        }

        // GET: api/University/CourseWith5Student
        [Route("api/University/CourseWith5Student")]
        public object[] GetCourseWith5Student()
        {
            try
            {
                var tmp = db.CourseSet;
                List<object> str = new List<object>();
                foreach (var x in tmp.Where(x => x.StudentSet.Count >= 5 ))
                {
                    str.Add(new { Name = x.Name, Teacher = x.TeacherSet });
                }
                return str.ToArray();
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.NotFound);
                return new[] { ex.Message };
            }
        }

        // GET: api/University/TeacherWith1Course
        [Route("api/University/TeacherWith1Course")]
        public object[] GetTeacherWith1Course()
        {
            try
            {
                var tmp = db.TeacherSet;
                List<object> str = new List<object>();
                foreach (var x in tmp.Where(x => x.CourseSet.Count == 1))
                {
                    str.Add(new { FIO = x.FIO, Course = x.CourseSet });
                }
                return str.ToArray();
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.NotFound);
                return new[] { ex.Message };
            }
        }

        // POST: api/University/NewStudent
        [Route("api/University/NewStudent")]
        public string PostNewStudent([FromBody]JObject value)
        {
            try
            {
                this.StatusCode(HttpStatusCode.Created);
                db.StudentSet.Add(new StudentSet(value["Id"].ToObject<int>(), value["FIO"].ToString(), value["Grade"].ToObject<int>()));
                db.SaveChanges();
                return "Done!";
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.BadRequest);
                return ex.Message;
            }
        }

        // POST: api/University/AddCourseToStudent
        [Route("api/University/AddCourseToStudent")]
        public string PostAddCourseToStudent([FromBody]JObject value)
        {
            try
            {
                this.StatusCode(HttpStatusCode.Created);
                var subj = db.StudentSet.Find(value["SubjectId"].ToObject<int>());
                subj.AddCourse(db.CourseSet.Find(value["CourseId"].ToObject<int>()));
                db.SaveChanges();
                return "Done!";
            }
            catch(Exception ex)
            {
                this.StatusCode(HttpStatusCode.BadRequest);
                return ex.Message;
            }
        }

        // POST: api/University/NewTeacher
        [Route("api/University/NewTeacher")]
        public string PostNewTeacher([FromBody]JObject value)
        {
            try
            {
                db.TeacherSet.Add(new TeacherSet(value["Id"].ToObject<int>(), value["FIO"].ToString()));
                db.SaveChanges();
                return "Done!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        // POST: api/University/AddCourseToTeacher
        [Route("api/University/AddCourseToTeacher")]
        public string PostAddCourseToTeacher([FromBody]JObject value)
        {
            try
            {
                var subj = db.TeacherSet.Find(value["SubjectId"].ToObject<int>());
                subj.AddCourse(db.CourseSet.Find(value["CourseId"].ToObject<int>()));
                db.SaveChanges();
                return "Done!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        // POST: api/University/NewCourse
        [Route("api/University/NewCourse")]
        public string PostNewCourse([FromBody]JObject value)
        {
            try
            {
                db.CourseSet.Add(new CourseSet(value["Id"].ToObject<int>(), value["TeacherId"].ToObject<int>(), value["Name"].ToString()));
                db.SaveChanges();
                return "Done!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        // PUT: api/University/UpdateStudent
        [Route("api/University/UpdateStudent")]
        public string PutUpdateStudent([FromBody]JObject value)
        {
            try
            {
                db.StudentSet.Find(value["Id"].ToObject<int>()).UpdateStudentSet(value["FIO"].ToString(), value["Grade"].ToObject<int>());
                db.SaveChanges();
                return "Updated!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        // PUT: api/University/UpdateTeacher
        [Route("api/University/UpdateTeacher")]
        public string PutUpdateTeacher([FromBody]JObject value)
        {
            try
            {
                db.TeacherSet.Find(value["Id"].ToObject<int>()).UpdateTeacherSet(value["FIO"].ToString());
                db.SaveChanges();
                return "Updated!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        // PUT: api/University/UpdateCourse
        [Route("api/University/UpdateCourse")]
        public string PutUpdateCourse([FromBody]JObject value)
        {
            try
            {
                db.CourseSet.Find(value["Id"].ToObject<int>()).UpdateCourseSet(value["TeacherId"].ToObject<int>(), value["Name"].ToString());
                db.SaveChanges();
                return "Updated!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        // DELETE: api/University/DeleteStudent
        [Route("api/University/DeleteStudent")]
        public string DeleteDeleteStudent([FromBody]JObject value)
        {
            try
            {
                int id = value["Id"].ToObject<int>();
                var subject = db.StudentSet.Find(id);
                subject.CourseSet.Clear();
                db.StudentSet.Remove(db.StudentSet.Find(id));
                db.SaveChanges();
                return "Deleted!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        // DELETE: api/University/DeleteTeacher
        [Route("api/University/DeleteTeacher")]
        public string DeleteDeleteTeacher([FromBody]JObject value)
        {
            try
            {
                int id = value["Id"].ToObject<int>();
                var subject = db.TeacherSet.Find(id);
                subject.CourseSet.Clear();
                db.TeacherSet.Remove(db.TeacherSet.Find(id));
                db.SaveChanges();
                return "Deleted!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        // DELETE: api/University/DeleteCourse
        [Route("api/University/DeleteCourse")]
        public string DeleteDeleteCourse([FromBody]JObject value)
        {
            try
            {
                int id = value["Id"].ToObject<int>();
                var subject = db.CourseSet.Find(id);
                subject.StudentSet.Clear();
                db.CourseSet.Remove(db.CourseSet.Find(id));
                db.SaveChanges();
                return "Deleted!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
