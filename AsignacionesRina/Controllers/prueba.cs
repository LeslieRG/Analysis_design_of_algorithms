 using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using xquelaWS.Models;
using xquelaWS.Lib;
using xquelaWS.DTO;
using System.Net;

namespace xquelaWS.Controllers{
    public class PruebaController : Controller{
        
        private readonly xquelaWsDbContext _context;
        private readonly xquelaWS.Lib.UserLib _userLib;
        private readonly xquelaWS.Lib.SecurityLib _securityLib;
        public PruebaController(xquelaWsDbContext context){
            
            _userLib = new xquelaWS.Lib.UserLib(context);
            _context = context;
            _securityLib = new xquelaWS.Lib.SecurityLib(context);
        }
//Actualizar Usuarios
        [HttpPut("updateUser")]
        public IActionResult UpdateUsers([FromHeader] string whatHeader, [FromBody] User whatBody)
        {
            var headerUpdate  = JsonConvert.DeserializeObject<RequestHeaderUser>(whatHeader);
            var userInfo = _userLib.checkUserID(whatHeader);
            var target = _context.Users.FirstOrDefault(ct => ct.uuid == headerUpdate.uuid);
        
            if(headerUpdate == null)
            {
                return BadRequest();
            }
            else if(userInfo.response != "OK"){
                return BadRequest(userInfo);

            }  else if(userInfo.response == "Ok"){
             
                 target.lastName = whatBody.lastName;
                 target.firstName = whatBody.firstName;
                 target.email = whatBody.email;
                 target.mobileNumber = whatBody.mobileNumber;
                 target.birthdate = whatBody.birthdate;
                 target.moreInfo = whatBody.moreInfo;
                 target.password = whatBody.password;
                 target.userRoleUuid = whatBody.userRoleUuid;

                 _context.Users.Update(target);
                 _context.SaveChanges();
                 return Ok(target);
            }
            else{
                return StatusCode((int)HttpStatusCode.Unauthorized, userInfo );
            }
        }
    }
}
  