using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RinaLessons.DataAcces;
using RinaLessons.Models;

namespace RinaLessons.Controllers{

    [Route ("api/v1/[controller]")]
    public class HomeworksController : Controller{

        private readonly RinnaLessonsDbContext _context;
        public HomeworksController (RinnaLessonsDbContext context){
           _context = context;
        }
        
         [HttpGet("{id}")]

        public IActionResult GetAllHomeworks(int id)
        {

            var task = _context.HomeWorks.ToList().Where(p => p.SubjectId == id);
            
            if(id == 0){
                return Json (new Response{
                    message = "Solicitud incorrecta",
                    info = "De ingresar el id de la materia "
                });
            }
            else if(task == null)
            {
                return Json (new Response{
                    message ="Solicitud",
                    info = "No hay tareas en esta materia"
                }); 
            }else 
            {
                return Ok(_context.HomeWorks.ToList());
            }
        }

        [HttpPost("{id}")]
        public IActionResult CreateHomework([FromBody] Homework newOne, int id){
            var subject = _context.Subjects.Any(p => p.SubjectId == id);

            if(subject == false){
                return Json(new Response{
                    message = "Solicitud Incorrecta",
                    info="Debe ingresar el Id de la materia a la cual desea agregar la tarea"
                });
            }
            else  {
                _context.HomeWorks.Add(newOne);
                _context.SaveChanges();
                return Json (new Response{
                    message = "solicitud completada"
                });
            }
        }
        [HttpPut("{idHomework}/{Idsubject}")]
        public IActionResult UpdateHomework ( [FromBody] Homework task, int idHomework, int Idsubject){

            //var Nombre = JsonConvert.DeserializeObject<Subject>(subject);
            var subjectEx = _context.HomeWorks.FirstOrDefault(s => s.SubjectId == Idsubject);
            var homework = _context.HomeWorks.Any(t => t.HomeworkId == idHomework);

           if(Idsubject == 0 || idHomework == 0 ){
               return BadRequest();
           } else if(subjectEx != null && homework != false){
            
               subjectEx.Description = task.Description;
               subjectEx.Title = task.Title;
               _context.HomeWorks.Add(task);
               _context.SaveChanges();
               return Ok (new Response{
                    message = "Solictud correcta",
                    info = "La Tarea ha sido Modificada"
                });
           }
           else{
               return Json (new Response{
                    message = "Solictud Incorrecta",
                    info = "La tarea no ha podido actualizarse"
                });
           }

        }

         [HttpDelete("{id}")]
        public IActionResult DeleteSubject(int id){
            var existinghomework = _context.HomeWorks.FirstOrDefault(ct=> ct.SubjectId == id);

                 
           if(id == 0){
               return BadRequest();
           } 
           else if(existinghomework == null){
                 return Json (new Response{
                     message = "Solicitud Incorrecta",
                        info = "La tarea no existe"
                });
            }
            else {
                
                _context.HomeWorks.Remove(existinghomework);
                _context.SaveChanges();
                return Json (new Response{
                     message = "Solicitud Correcta",
                        info = "La Asignatura no esta activa"
                });
            }
        }

    }
 }
    