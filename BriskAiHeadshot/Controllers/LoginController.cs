using BriskAiHeadshot.Models;
using BriskAiHeadshot.Payload;
using ClassLibrary1.Business;
using ClassLibrary1.Data.Framework;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BriskAiHeadshot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly JwtToken _jwtTokenService;

        public LoginController(JwtToken jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }



        

        [HttpPost]

        public ActionResult CheckLoginCredentials([FromBody] LoginModel user)
        {


                SelectResult result = Accounts.CheckLoginCredentials(user.email, user.password);


                if (result.Succeeded)
                {
                var IsGoogleUser = result.DataTable.Rows[0]["isGoogleUser"];
                var IsVerified = result.DataTable.Rows[0]["EmailVerified"];
                string name = result.DataTable.Rows[0]["name"].ToString();
                    if (!string.IsNullOrEmpty(name))
                    {
                       var token = _jwtTokenService.GenerateJwtToken(user.email, name, IsVerified.ToString(), IsGoogleUser.ToString());
                        return Ok(JsonConvert.SerializeObject(token));
                        
                    }
                    else
                    {
                        return BadRequest("Cannot find user's name.");
                    }
                }
                else
                {
                    return BadRequest("Invalid login information.");
                }
            
            
        }


        [HttpPost("GoogleValidation")]
        public async Task<ActionResult> CheckGoogleCredentials([FromBody] GoogleModel user)
        {
            var result = Accounts.CheckIfEmailExist(user.email);

            if (result.Rows == 1)
            {
                var isGoogleUser = result.DataTable.Rows[0]["isGoogleUser"] != DBNull.Value && Convert.ToBoolean(result.DataTable.Rows[0]["isGoogleUser"]);
                if (isGoogleUser)
                {
                    if (await VerifyGoogleToken.Verify(user.token))
                    {
                        return Ok(new { message = "Succes" });
                    }
                    else
                    {
                        return Unauthorized("Invalid Google-token.");
                    }
                }
                else
                {
                    return Conflict("User has already signed in with a non-Google account using this email address.");

                }
            }
            else
            {
                if (await VerifyGoogleToken.Verify(user.token))
                {
                    Accounts.AddAccounts(user.email, user.name, null, true);
                    return Ok(new { message = "Google user added", email = user.email });
                }
                else
                {
                    return Unauthorized("invalid Google-token.");
                }
            }
        }




    }
}
