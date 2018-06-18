using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using xquelaWS.DTO;
using xquelaWS.Models;

namespace xquelaWS.Controllers {
[Route("api/[controller]")]
    public class FriendsController : Controller {
        private readonly xquelaWsDbContext _context;
        private readonly xquelaWS.Lib.UserLib _userLib;
        private readonly xquelaWS.Lib.SecurityLib _securityLib;
        private readonly xquelaWS.Lib.FriendsLib _friendLib;

        public FriendsController (xquelaWsDbContext context) {
            _userLib = new xquelaWS.Lib.UserLib (context);
            _context = context;
            _securityLib = new xquelaWS.Lib.SecurityLib (context);
            _friendLib = new xquelaWS.Lib.FriendsLib (context, _userLib);
        }

        [HttpPost ("AddFriend")]
        public IActionResult AddFriend ([FromHeader] string whatHeader,  [FromBody] FriendsUser users) {
            var headerObj = JsonConvert.DeserializeObject<RequestHeaderUser> (whatHeader);
            var tokenMember = _securityLib.IsValidToken (headerObj.tokenID);            
            var user = _context.Users.FirstOrDefault (ct => ct.uuid == headerObj.uuid);
            var bodyObj = _friendLib.FriendsAdd (users, headerObj.uuid);
            var updating = JsonConvert.SerializeObject (bodyObj.success);

            if (whatHeader == null || users == null) {
                return BadRequest ();
            } else if (tokenMember.response != "Ok") {
                return StatusCode ((int) HttpStatusCode.Unauthorized, tokenMember);
            }

            var listResulted = new List<FriendsUpdate> ();

            if (bodyObj.success.Count > 0) {
                if (user.friendsInfo != null) {
                     var one = JsonConvert.DeserializeObject<List<FriendObj>>(user.friendsInfo);
                     foreach(FriendObj element in bodyObj.success) {
                        one.Add(element);

                     }
                user.friendsInfo = JsonConvert.SerializeObject(one);
                _context.SaveChanges ();
                }
                
                user.friendsInfo += updating;
                _context.SaveChanges ();

            }

            bodyObj.success.ForEach (item => {
                var friendTmp = new FriendsUpdate () {
                FriendID = item.Id_User,
                Message = "Added"
                };
                listResulted.Add (friendTmp);
            });

            bodyObj.fail.ForEach (item => {
                var friendTmp = new FriendsUpdate () {
                FriendID = item.Id_User,
                Message = "No Added"
                };
                listResulted.Add (friendTmp);
            });

        return Json(new ResponseUpdate{
            response ="OK",
            data= listResulted,
            info = "User Updated"
        });
        }

        [HttpPost ("DeleteFriend")]
        public IActionResult DeleteFriend ([FromHeader] string whatHeader,  [FromBody] FriendsUser users) {
            var headerObj = JsonConvert.DeserializeObject<RequestHeaderUser> (whatHeader);
            var tokenMember = _securityLib.IsValidToken (headerObj.tokenID);            
            var user = _context.Users.FirstOrDefault (ct => ct.uuid == headerObj.uuid);
            var bodyObj = _friendLib.FriendsAdd (users, headerObj.uuid);
            var updating = JsonConvert.SerializeObject (bodyObj.success);

            if (whatHeader == null || users == null) {
                return BadRequest ();
            } else if (tokenMember.response != "Ok") {
                return StatusCode ((int) HttpStatusCode.Unauthorized, tokenMember);
            }

            var listResulted = new List<FriendsUpdate> ();

            if (bodyObj.success.Count > 0) {
                if (user.friendsInfo != null) {
                     var one = JsonConvert.DeserializeObject<List<FriendObj>>(user.friendsInfo);
                     foreach(FriendObj element in bodyObj.success) {
                        one.Remove(element);

                     }
                user.friendsInfo = JsonConvert.SerializeObject(one);
                _context.SaveChanges ();
                }
                
                user.friendsInfo += updating;
                _context.SaveChanges ();

            }

            bodyObj.success.ForEach (item => {
                var friendTmp = new FriendsUpdate () {
                FriendID = item.Id_User,
                Message = "Friend Deleted"
                };
                listResulted.Add (friendTmp);
            });

            bodyObj.fail.ForEach (item => {
                var friendTmp = new FriendsUpdate () {
                FriendID = item.Id_User,
                Message = "No Deleted"
                };
                listResulted.Add (friendTmp);
            });

        return Json(new ResponseUpdate{
            response ="OK",
            data= listResulted,
            info = "User Updated"
        });
    }
    }
}
