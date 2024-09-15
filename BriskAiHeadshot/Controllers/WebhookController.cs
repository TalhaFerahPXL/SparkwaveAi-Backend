using BriskAiHeadshot.Models;
using BriskAiHeadshot.Payload;
using Microsoft.AspNetCore.Mvc;

namespace BriskAiHeadshot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebhookController : ControllerBase
    {
        

       
            [HttpPost]
            [Route("api/webhook")] 
            public IActionResult ReceiveCallback([FromBody] WebhookRequestBody requestBody)
            {

                var prompt = requestBody.Prompt;
                int tuneId = prompt.Tune_Id;


            //methode vind emali bij tune en send

            EmailController emailController = new EmailController();
            emailController.EmailTuneReady("");
            

                return Ok($" {tuneId.ToString()}");
            }



        //[HttpPost]
        //[Route("api/EmailConfirmation")]
        //public IActionResult EmailConfirmation([FromBody] EmailRequestModel requestBody)
        //{



            

        //    EmailController emailController = new EmailController();
        //    emailController.EmailConfirmation(requestBody.Mail);


        //    return Ok($" ");
        //}

    }
}
