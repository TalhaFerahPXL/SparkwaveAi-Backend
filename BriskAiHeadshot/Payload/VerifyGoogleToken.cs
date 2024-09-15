using Google.Apis.Auth;


namespace BriskAiHeadshot.Payload
{
    public static class VerifyGoogleToken
    {
        public static async Task<bool> Verify(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    
                    Audience = new List<string>() { "" }
                };

            
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);


                return payload != null;  
            }
            catch (InvalidJwtException)
            {
                
                return false;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
    }
}

