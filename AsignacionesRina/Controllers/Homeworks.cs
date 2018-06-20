using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RinaLessons.DataAcces;
using RinaLessons.Models;

namespace RinaLessons.Controllers{
    
    // Jean estuvo aqui...

    [Route ("api/v1/[controller]")]
    public class HomeworksController : Controller{

        private readonly RinnaLessonsDbContext _context;
        public HomeworksController (RinnaLessonsDbContext context){
           _context = context;
        }
        
        // [HttpGet("{id}")]

        // public IActionResult GetAllHomeworks(int id)
        // {

        //     var task = _context.HomeWorks.ToList().Where(p => p.TeacherId == id);
            
        //     if()
        //     {

        //     }else if(task == null)
        //     {
        //         return Json (new Response{
        //             message ="Solicitud",
        //             info = "No hay tareas en esta materia"
        //         }); 
        //     }else 
        //     {

        //     }
            
        //     return Ok(_context.HomeWorks.ToList());
        // }
    }
}
