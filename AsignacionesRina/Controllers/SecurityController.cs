using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using xquelaWS.Models;
using xquelaWS.DTO;
using xquelaWS.Lib;

 namespace xquelaWS.Controllers{
 
    [Route("api/security/getTokenID")]
    public class SecurityController: Controller
    {
         private readonly xquelaWsDbContext _context;
         private readonly xquelaWS.Lib.SecurityLib _securitylib;

        public SecurityController(xquelaWsDbContext context)
        {
            _securitylib = new xquelaWS.Lib.SecurityLib(context);
           _context = context;
        }

            [HttpPost]
            public IActionResult getTokenID([FromHeader] string whatHeader)
            {
                var headerObj = JsonConvert.DeserializeObject<RequestHeaderMember>(whatHeader);
                if(whatHeader == null){
                    return BadRequest();
                }else{
                    return Ok(_securitylib.CheckMemberAccess(whatHeader));
                }
            }
    }
    
}