using InnateGlory;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace ams.Controllers
{
    public class ApiTestController : Controller
    {
        [HttpPost("/test/api_1")]
        public IActionResult Test1([FromBody] testModal x, [FromServices] UserManager<amsUser> users, [FromServices] DataService dataService)
        {
            dataService.GamePlatforms.test();
            
            var xx = users.CurrentUser;
            return ApiResult.Success(new { aa = 1, bb = 2, cc = DateTime.Now });
        }

        [Api("/test/api_3"), PlatformInfo(PlatformType = PlatformType.test1)]
        public IActionResult Test1(InnateGlory.Models.test1PlatformInfo models, [FromServices] DataService dataService)
        {
            dataService.GamePlatforms.test();

            return ApiResult.Success(new { aa = 1, bb = 2, cc = DateTime.Now });
        }

        [Api("/test/api_3"), PlatformInfo(PlatformType = PlatformType.test2)]
        public IActionResult Test1(InnateGlory.Models.test2PlatformInfo models, [FromServices] DataService dataService)
        {
            dataService.GamePlatforms.test();

            return ApiResult.Success(new { aa = 1, bb = 2, cc = DateTime.Now });
        }


        [Api("/test/api_2")]
        public object Test2(int? a, UserId? b, testJsonModal x, [FromServices] IAuthenticationService auth)
        {
            return new { aa = 1, bb = 2 };
            //return Json(new { aa = 1, bb = 2 });
        }

        //[WebSocketAction]
        //[HttpPost("/test/api_3")]
        //public void WebSocketTest()
        //{
        //}

        //[Api("/test/SignIn")]
        //public async Task<IActionResult> Test_SignIn([FromServices] UserManager<amsUser> users)
        //{
        //    var user = new amsUser() { Id = 555 }; //users.GetUser(100);
        //    await users.SignInAsync(user);
        //    return ApiResult.OK;
        //}

        //[Api("/test/SignOut")]
        //public async Task<IActionResult> Test_SignOut([FromServices] UserManager<amsUser> users, [FromServices] amsUser user)
        //{
        //    await users.SignInAsync(user);
        //    //await users.SignOutAsync();
        //    return ApiResult.OK;
        //}
    }

    public class testModal
    {
        //[xxx(10, 100)]
        public UserId? a { get; set; }
        public int? b { get; set; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class testJsonModal
    {
        //[xxx(10, 100)]
        [JsonProperty]
        public UserId? a { get; set; }
        [JsonProperty]
        public int? b { get; set; }
    }
}