using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RinaLessons.Models;
using RinaLessons.DataAcces;
using Newtonsoft.Json;


namespace RinaLessons.Controllers{

    [Route ("api/v1/[controller]")]

    public class SubjectsController : Controller{
        private readonly RinnaLessonsDbContext _context; 
         public  SubjectsController (RinnaLessonsDbContext context){
             _context = context;
         }

        [HttpGet("{TeacherId}")]
        public IActionResult GetAllSubjectByTeacher(int TeacherId){
             var classes = _context.Subjects.ToList().Where(ct => ct.UserId == TeacherId);
            
           if(TeacherId == 0){
               return BadRequest();
           } 
           else if(classes == null){
                return Json (new Response{
                    message = "Solicitud Incorrecta",
                    info = "Usted no ha creado asignaturas"
                });
            }else{
                return Ok(classes);
            }
        }

        [HttpPost("{TeacherId}")]
        public IActionResult CreateSubject(int TeacherId, [FromBody] Subject subject){

        var teacher = _context.Users.Any(t => t.UserId == TeacherId && t.Role == 2);
           if(TeacherId == 0){
               return BadRequest();
           } else if(teacher == true){
               _context.Subjects.Add(subject);
               _context.SaveChanges();
                return Json (new Response{
                    message = "Solicitud Correcta",
                    info = "Asignatura Creada"
                });
           }
           else{
               return Json (new Response{
                    message = "Solictud Incorrecta",
                    info = "La Asignatura no ha Creada"
                });
           }
        }

        [HttpPut("{idSubject}/{IdTeacher}")]
        public IActionResult UpdateSubjectName ( [FromBody] Subject sub){

            //var Nombre = JsonConvert.DeserializeObject<Subject>(subject);
            var subjectEx = _context.Subjects.FirstOrDefault(s => s.SubjectId == sub.SubjectId);
            var teacher = _context.Users.Any(t => t.UserId == sub.UserId && t.Role == 2);

           if(sub.SubjectId == 0 || sub.UserId == 0 ){
               return BadRequest();
           } else if(subjectEx != null && teacher != false){
            
               subjectEx.SubjectName = sub.SubjectName;
               _context.Subjects.Add(sub);
               _context.SaveChanges();
               return Ok (new Response{
                    message = "Solictud correcta",
                    info = "La Asignatura ha sido Modificada"
                });
           }
           else{
               return Json (new Response{
                    message = "Solictud Incorrecta",
                    info = "La Asignatura no ha podido actualizarse"
                });
           }

        }

         [HttpDelete("{id}")]
        public IActionResult DeleteSubject(int id){
            var existingSubject = _context.Subjects.FirstOrDefault(ct=> ct.SubjectId == id);

                 
           if(id == 0){
               return BadRequest();
           } 
           else if(existingSubject == null){
                 return Json (new Response{
                     message = "Solicitud Incorrecta",
                        info = "La Asignatura no existe"
                });
            }
            else {
                
                _context.Subjects.Update(existingSubject);
                _context.SaveChanges();
                return Json (new Response{
                     message = "Solicitud Correcta",
                        info = "La Asignatura no esta activa"
                });
            }
        }

    }


    
}