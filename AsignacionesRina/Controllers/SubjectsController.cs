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
             var classes = _context.Subject.ToList().Where(ct => ct.TeacherId == TeacherId);
            
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

        var teacher = _context.Users.Any(t => t.UserId == TeacherId && t.RoleId == 2);
           if(TeacherId == 0){
               return BadRequest();
           } else if(teacher == true){
               _context.Subject.Add(subject);
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
        public IActionResult UpdateSubjectName (int IdSubject, int TeacherId, [FromBody] string name){

            //var Nombre = JsonConvert.DeserializeObject<Subject>(subject);
            var subjectEx = _context.Subject.FirstOrDefault(s => s.SubjectId == IdSubject);
            var teacher = _context.Users.Any(t => t.UserId == TeacherId && t.RoleId == 2);

           if(IdSubject == 0 || TeacherId == 0 ){
               return BadRequest();
           } else if(subjectEx != null && teacher != false){
            
               subjectEx.SubjectName = name;
               _context.Add(name);
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
            var existingSubject = _context.Subject.FirstOrDefault(ct=> ct.SubjectId == id);

                 
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
                existingSubject.Status = 0;
                _context.Subject.Update(existingSubject);
                _context.SaveChanges();
                return Json (new Response{
                     message = "Solicitud Correcta",
                        info = "La Asignatura no esta activa"
                });
            }
        }

    }


    
}