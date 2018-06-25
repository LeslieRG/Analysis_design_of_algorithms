using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RinaLessons.DataAcces;
using RinaLessons.Models;

namespace RinaLessons.Controllers {

    [Route ("api/v1/[controller]")]
    public class EnrollmentsController : Controller {

        private readonly RinnaLessonsDbContext _context;
        public EnrollmentsController (RinnaLessonsDbContext context) {
            _context = context;
        }

        [HttpPost ("requestEnrollment")]

        public IActionResult EnrollmentRequest ([FromBody] Enrollment request) {

            var subject = (from c in _context.Subjects where c.SubjectId == request.SubjectId select new {
                teacher = c.UserId
            }).FirstOrDefault ();

            if (subject != null) {

                request.status = 1;
                _context.Enrollments.Add (request);
                _context.SaveChanges ();
                return Ok ();

            } else {
                return BadRequest ();
            }
        }

        [HttpPost ("responseEnroll/{enrollId}")]

        public IActionResult ResponseEnrollment (int enrollId) {

            var response = _context.Enrollments.FirstOrDefault (e => e.EnrollmentId == enrollId);

            if (response != null) {

                response.status = 2;
                _context.Enrollments.Add (response);
                _context.SaveChanges ();
                return Ok ();

            } else {
                return BadRequest ();
            }
        }

        [HttpPost ("Asigned/{subjectid}")]
        public IActionResult AssignedHomework (int subjectid, int UserId) {
            var homework = _context.HomeWorks.Where (x => x.SubjectId == subjectid).ToList ();

            var enroll = _context.Subjects.FirstOrDefault (s => s.SubjectId == subjectid);

            List<Homework> available = new List<Homework> ();

            homework.ForEach (item => {
                if (_context.Asigned.Any (x => x.HomeworkId != item.HomeworkId)) {
                    available.Add (item);
                }
            });

            if (available.Count == 0) return Json (new Response {
                message = "Solicitud Incorrecta",
                info = "No hay tareas en esta materia"
            });

            var homeworkToAssign = available.FirstOrDefault ();

            AssignamentsStudent newAssignment = new AssignamentsStudent () {
                HomeworkId = homeworkToAssign.HomeworkId,
                SubjectID = subjectid,
                UserId = UserId
            };

            _context.Asigned.Add (newAssignment);
            _context.SaveChanges ();

            return Ok ();
        }

    }

}