using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RinaLessons.Models;
using RinaLessons.DataAcces;

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

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int userId, string email, string cellphone, [FromBody] User user){
          

             var existingUser = _context.Users.SingleOrDefault(eu => eu.UserId == userId
                                                                || eu.Email == email
                                                                || eu.Cellphone == cellphone);
            

            if(existingUser != null){
                existingUser.UserName = user.UserName;
                existingUser.Cellphone = user.Cellphone;
                existingUser.Email = user.Email;
                
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

            if(existingUser == null){
                 return Json (new Response{
                     message = "Solicitud Incorrecta",
                        info = "El usuario no existe"
                });
            }
            else {
                existingUser.status = 0;
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