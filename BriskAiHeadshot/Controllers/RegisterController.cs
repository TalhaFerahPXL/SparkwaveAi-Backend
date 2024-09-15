using BriskAiHeadshot.Models;
using ClassLibrary1.Business;
using ClassLibrary1.Data.Framework;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BriskAiHeadshot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        [HttpPost]
        [Route("Register")]
        public ActionResult MakeNewUserFromBody([FromBody] RegisterModel vm)
        {
            SelectResult result = Accounts.CheckIfEmailExist(vm.email);

            if (result.Rows > 0)
            {
                string text = "Invalid Email";
                return BadRequest(JsonConvert.SerializeObject(text));
            }
            else
            {
                Accounts.AddAccounts(vm.email, vm.name, vm.password, false);
                string text = $"User {vm.name} has been created";
                return Ok(JsonConvert.SerializeObject(text));
            }
        }

    }
}
