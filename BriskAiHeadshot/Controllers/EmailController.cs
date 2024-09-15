using BriskAiHeadshot.Models;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary1.Data.Framework;
using Newtonsoft.Json;
using ClassLibrary1.Mail;
using System.Text;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ClassLibrary1.Business.Entities;
using ClassLibrary1.Business;

namespace BriskAiHeadshot.Controllers
{

    public class EmailController : ControllerBase
    {
        //[HttpPost("emailrequest")]


        
        public ActionResult EmailTuneReady(string email)
        {

            //SelectResult result = Accounts.CheckIfEmailExist(erm.Mail);

            //if (!result.Succeeded)
            //{
            //    return Ok(JsonConvert.SerializeObject("Email is niet gelinked aan een account"));
            //}

            try
            {
                var mail = new Email(email)
                {
                    Body = $"Images are created..",
                    Subject = "Tune Ready"
                };
                mail.SendMail();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            return Ok(JsonConvert.SerializeObject("Email send"));
        }



        public string GenerateConfirmationToken(string email)
        {
            string tokenSource = email + DateTime.UtcNow.Ticks.ToString();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(tokenSource));
                string token = Convert.ToBase64String(hashedBytes);

                
                token = token.Replace('+', '-').Replace('/', '_');

                return token;
            }
        }


        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                return regex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        [HttpPost]
        [Route("SendConfirmationMail")]
        public ActionResult EmailConfirmation([FromBody] EmailRequestModel model)
        {
            //if (!IsValidEmail(model.Mail))
            //{
            //    return BadRequest(JsonConvert.SerializeObject("Invalid email format."));
            //}

            try
            {
                string confirmationToken = GenerateConfirmationToken(model.Mail);
                string confirmationLink = $"http://localhost:63342/LoginInAi/Html/EmailConfirmationPage.html?token={confirmationToken}";

                var result = Accounts.InsertEmailConfirmationToken(confirmationToken, model.Mail);


                var mail = new Email(model.Mail)
                {
                    
                    Body = $@"<html>
                <body>
                    <p>Gelieve uw e-mailadres te bevestigen door op de onderstaande knop te klikken:</p>
                    <table cellspacing='0' cellpadding='0'> <tr>
                        <td align='center' width='200' height='40' bgcolor='#00CCFF' style='border-radius: 4px; color: #ffffff; display: block;'>
                            <a href='{confirmationLink}' style='font-size: 16px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; line-height: 40px; width:100%; display:inline-block'>
                                Bevestig E-mail
                            </a>
                        </td>
                    </tr> </table>
                </body>
              </html>",
                    Subject = "Bevestig Uw E-mail"
                };

                mail.SendMail();

                return Ok(JsonConvert.SerializeObject("Confirmation Email Sent. Please check your inbox."));
            }
            catch (Exception ex)
            {
                return BadRequest(JsonConvert.SerializeObject($"An error occurred: {ex.Message}"));
            }
        }



        [HttpPost]
        [Route("api/checkConfirmationToken")]
        public IActionResult CheckConfirmationTokenController([FromBody] TokenModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Token))
            {
                return BadRequest("Invalid token supplied.");
            }

          
            SelectResult selectResult = Accounts.SelectEmailConfirmationToken(model.Token);

            if (selectResult.Succeeded)
            {
              
                var insertResult = Accounts.UpdateEmailVerifiedStatus(model.Token);

                if (insertResult.Succeeded)
                {
                    return Ok(new { message = "Token is valid and email verification can proceed." });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to update email verification status." });
                }
            }
            else
            {
                return BadRequest(new { message = "Invalid token supplied." });
            }
        }



    }
}
