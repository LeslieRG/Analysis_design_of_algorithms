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

        [HttpPost("{login}")]
        public IActionResult Authentication([FromBody] Authentication validation){
            var user = _context.Users.Any(u => u.UserName == validation.UserName ||
                                         u.Email == validation.Email
                                         || u.Cellphone == validation.Cellphone
                                          && u.Password == validation.Password);
            if (user == true){
                return Ok();
            }else{
                return Unauthorized();
            }
        }

        [HttpGet("GetAllTeacher")]
        public IActionResult GetAllTeacher(){

            return Ok(_context.Users.ToList().Where(ct => ct.RoleId == 2));
        }
        [HttpGet("GetAllStudents")]
        public ActionResult GetAllStudents(){

            return Ok(_context.Users.ToList().Where(ct => ct.RoleId == 3));
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user){

            var existingUser = _context.Users.SingleOrDefault(eu => eu.UserId == user.UserId 
                                                                || eu.Email == user.Email 
                                                                || eu.Cellphone == user.Cellphone);

            if(existingUser == null){

                _context.Users.Add(user);
                _context.SaveChanges();
                return Created("{user/created}", user);
            }
            else{
                return Json (new Response{
                        message = "Solicitud Incorrecta",
                        info = "Ya existe registrado"
                });
            }
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
                existingUser.Status = 0;
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