using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xquelaWS.DTO;
using xquelaWS.Lib;
using xquelaWS.Models;

namespace xquelaWS.Controllers {
    [Route ("api/[controller]")]
    public class UsersController : Controller {
        private readonly xquelaWsDbContext _context;
        private readonly xquelaWS.Lib.UserLib _userLib;
        private readonly xquelaWS.Lib.SecurityLib _securityLib;
        public UsersController (xquelaWsDbContext context) {
            _userLib = new xquelaWS.Lib.UserLib (context);
            _context = context;
            _securityLib = new xquelaWS.Lib.SecurityLib (context);
        }
        //get all users
        [HttpGet]
        public IActionResult GetAllUser ([FromHeader] string tokenID) {
            var users = _securityLib.IsValidToken (tokenID);
            if (users.response == "Ok") {
                //  return Ok(_context.Users.ToList().Where(ct => ct.status != 2));
                return Ok (_context.Users.ToList ());
            } else {
                return StatusCode ((int) HttpStatusCode.Unauthorized, users);
            }

        }

        //get user by token
        [HttpGet ("{id}")]
        public IActionResult GetUserById (string id) {
            var user = this._context.Users.SingleOrDefault (ct => ct.uuid == id && ct.status != 2);
            if (user != null) {
                return Ok (user);
            } else {
                return NotFound ();
            }
        }
        /// <summary>
        /// Este metodo permite crear un usuario necesita via post un string whatHeader con el token del member, 
        /// y el body requiere de los datos del usuario a Agregar
        /// </summary>
        /// <param name="whatHeader"></param>
        /// <param name="user"></param>
        /// <returns>useradded</returns>
        /// 
        
        [HttpPost ("addUser")]
        public IActionResult UsersPost ([FromHeader] string whatHeader, [FromBody] User user) {
            var headerLogin = JsonConvert.DeserializeObject<RequestHeaderUser> (whatHeader);
            var checkMember = _securityLib.IsValidToken (headerLogin.tokenID);
            var userUuid = _userLib.checkAddUser (user.userID, user.mobileNumber, user.email);

            if (checkMember.response.ToLower () == "Ok".ToLower () && userUuid.response.ToLower () != "Ok".ToLower ()) {
                    user.userRoleUuid = "b057c21c-2497-11e8-a279-19770cd407a2";
                    user.status = 0;
                    this._context.Users.Add (user);
                    this._context.SaveChanges ();
                    return Json (new Response {
                        response = "Ok",
                            data = (new ResponseUser {
                                uuid = user.uuid,
                                    firstname = user.firstName,
                                    roleUuid = user.userRoleUuid
                            }),
                            info = "User Added"
                    });

            }else if(userUuid.response.ToLower () == "Ok".ToLower ()){
               return Json (new Response {
                    response = "Error",
                        data = null,
                        info = "User Exists"
                        });

            }else if (checkMember.response != "Ok"){
                return StatusCode ((int) HttpStatusCode.Unauthorized, checkMember);
            }
             else {
                return BadRequest();
            }
        }
        
        /// <summary>
        /// Una vez creado el usuario con el status inactivo, se ejecuta este metodo para cambiarle 
        /// el estatus a Activo, debe enviarse desde el body un boleano de valor true(plano sin nombre de variable al enviarse) que indique que si quiere actualizarse el estado
        /// </summary>
        /// <param name="whatHeader"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [HttpPost ("activeUser")]
        public IActionResult ActiveUser ([FromHeader] string whatHeader, [FromBody] bool active) {
            var header = JsonConvert.DeserializeObject<RequestHeaderUser> (whatHeader);
            var users = _securityLib.IsValidToken (header.tokenID);
            var target = _context.Users.FirstOrDefault (ct => ct.uuid == header.uuid);

            if (header == null) {

                return BadRequest ();
            } else {
                if (users.response == "Ok") {
                    if (active == true) {
                        target.status = 1;
                          this._context.SaveChanges ();
                          return Json (new Response {
                        response = "Ok",
                            data = null,
                            info = "User Updated"
                    });
                    } else {
                        target.status = 2;
                        this._context.SaveChanges ();
                          return Json (new Response {
                        response = "Ok",
                            data = null,
                            info = "User Updated"
                    });
                }
                  
                } else {
                    return StatusCode ((int) HttpStatusCode.Unauthorized, users);
                }
            }
        }

        //metodo login from header post
        [HttpPost ("login")]
        public IActionResult userLogin ([FromHeader] string whatHeader) {
            var headerObj = JsonConvert.DeserializeObject<RequestHeaderUser> (whatHeader);
            var tokenMember = _securityLib.IsValidToken (headerObj.tokenID);
            if (whatHeader == null) {
                return BadRequest ();
            } else if (tokenMember.response != "Ok") {
                return StatusCode ((int) HttpStatusCode.Unauthorized, tokenMember);
            } else {
                var login = _userLib.userLoginAccess (whatHeader);
                if (login.response == "Ok") {
                    return Ok (login);
                } else {
                    return StatusCode ((int) HttpStatusCode.Unauthorized, login);
                }
            }
        }
        // //Actualizar Usuarios solo la info personal

        [HttpPost ("updateUser")]
        public IActionResult UpdateUsers ([FromHeader] string whatHeader, [FromBody] User whatBody) {
            var headerUpdate = JsonConvert.DeserializeObject<RequestHeaderUser> (whatHeader);
            var checkMember = _securityLib.IsValidToken (headerUpdate.tokenID);
            var userInfo = _userLib.checkUserID (whatHeader);
            var target2 = (from t in _context.Users where t.uuid == headerUpdate.uuid

                select new {
                    name = t.firstName,
                        role = t.userRoleUuid,
                        id = t.userID
                }).FirstOrDefault ();

            var target = _context.Users.FirstOrDefault (ct => ct.uuid == headerUpdate.uuid);

            if (headerUpdate == null) {
                return BadRequest ();
            } else if (userInfo.response != "Ok") {
                return Json (userInfo);
            } else if (checkMember.response != "Ok") {
                return Json (checkMember);
            } else if (userInfo.response == "Ok") {

                target.userRoleUuid = whatBody.userRoleUuid;
                target.firstName = whatBody.firstName;
                target.lastName = whatBody.lastName;
                target.email = whatBody.email;
                target.mobileNumber = whatBody.mobileNumber;
                target.entity_uuid = whatBody.entity_uuid;
                target.birthdate = whatBody.birthdate;
                target.moreInfo = whatBody.moreInfo;
                target.password = whatBody.password;

                _context.Users.Update (target);
                _context.SaveChanges ();
                return Json (new Response {
                    response = "Ok",
                        data = (new ResponseUser {
                            firstname = target2.name,
                                userID = target2.id,
                                roleUuid = target2.role
                        }),
                        info = "User Updated"
                });
            } else {
                return StatusCode ((int) HttpStatusCode.Unauthorized, userInfo);
            }
        }

        //change status to user to simulate delete
        [HttpPost ("{id}")]
        public IActionResult DeleteUsers (string id) {
            UserLib userLib = new UserLib (_context);

            var userValidation = userLib.IfValidUser (id);

            if (userValidation.response != "OK") {
                return BadRequest ();
            } else {
                var statusUser = _context.Users.SingleOrDefault (ct => ct.userID == id);
                statusUser.status = 2;
                _context.Users.Update (statusUser);
                _context.SaveChanges ();
                return Ok ();
            }
        }
    }
}