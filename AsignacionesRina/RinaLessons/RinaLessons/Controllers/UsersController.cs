using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RinaLessons.Models;
using RinaLessons.DataAcces;
using RinaLessons.DTO;
using System.Net;

namespace RinaLessons.Controllers{

    [Route ("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly RinnaLessonsDbContext _context;

        public UsersController(RinnaLessonsDbContext context){
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllUser(){
             return Ok(_context.Users.ToList());
        }

        [HttpPost ("login")]
        public IActionResult userLogin ([FromBody] int UserId, string Password) {
          var user = _context.Users.FirstOrDefault( u => u.UserId == UserId && u.Password == Password);
            if (user == null) {
                return Unauthorized ();
            } 
            else if(user.Role == 1) {
              return Json (new Response{
                        message = "Ok",
                        info = "Profesor"
              });
            
            }else{
                    return Json (new Response{
                        message = "Ok",
                        info = "Estudiante"
                    });
            }
        }
        

        [HttpGet("GetAllTeacher")]
        public IActionResult GetAllTeacher(){

            return Ok(_context.Users.ToList().Where(ct => ct.Role == 1));
        }
        [HttpGet("GetAllStudents")]
        public ActionResult GetAllStudents(){

            return Ok(_context.Users.ToList().Where(ct => ct.Role == 2));
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user){

            // var existingUser = _context.Users.FirstOrDefault( eu => eu.Email == user.Email  
            //                                                     || eu.Cellphone == user.Cellphone);

            // if(existingUser == null){

                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok();
            //     return Created("{user/created}", user);
            // }
            // else{
            //     return Json (new Response{
            //             message = "Solicitud Incorrecta",
            //             info = "Ya existe registrado"
            //     });
            
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] User user){
          

             var existingUser = _context.Users.SingleOrDefault(eu => eu.UserName == user.UserName);
                                                               
                 
           if(user == null){
               return BadRequest();
           } 
            else if(existingUser == null){
                existingUser.UserName = user.UserName;
                existingUser.Cellphone = user.Cellphone;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                
                _context.Users.Add(existingUser);
                _context.SaveChanges();
                return Json(new Response{
                        message = "Solicitud Correcta",
                        info = "Usuario Actualizado"
                });
            }
            else{
                return Json (new Response{
                        message = "Solicitud Incorrecta",
                        info = "Ya existe registrado"
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id){
            var existingUser = _context.Users.FirstOrDefault(ct=> ct.UserId == id);

                 
           if(id == 0){
               return BadRequest();
           } 
           else if(existingUser == null){
                 return Json (new Response{
                     message = "Solicitud Incorrecta",
                        info = "El usuario no existe"
                });
            }
            else {
               
                _context.Users.Update(existingUser);
                _context.SaveChanges();
                return Json (new Response{
                     message = "Solicitud Correcta",
                        info = "El usuario ya no esta activo"
                });

            }
        }

    }
}