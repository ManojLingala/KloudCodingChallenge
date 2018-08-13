using System;
using System.Linq;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KloudCodingChallenge.Filters
{
    public class IPAuthenticationFilter : ActionFilterAttribute
    {
        //Default I.P Whitelisting (127.0.0.1) , API key Identity check is implemented and these can be uncommented 
         //tested via composing from postman / fiddler 

       /* public override void OnActionExecuting(ActionExecutingContext context)
        {
            //set a default API key 
            //string yourApiKey = "X-some-key";
            //bool isValidAPIKey = false;
            //IEnumerable<string> lsHeaders;
            ////Validate that the api key exists
            //var checkApiKeyExists = context.Request.Headers.TryGetValues("API_KEY", out lsHeaders);

            //if (checkApiKeyExists)
            //{
            //    if (lsHeaders.FirstOrDefault().Equals(yourApiKey))
            //    {
            //        isValidAPIKey = true;
            //    }
            //}

            ////If the key is not valid, return an http status code.
            //if (!isValidAPIKey)
            //    throw new ApiException(403, "Authentication Failed");

            //base.OnActionExecuting(context);
        }

        //private bool IsIpAddressAllowed(string IpAddress)
        //{
        //    if (!string.IsNullOrWhiteSpace(IpAddress))
        //    {
        //        string[] addresses = Convert.ToString(ConfigurationManager.AppSettings["WhitelistedIPAddress"]).Split(',');
        //        return addresses.Where(a => a.Trim().Equals(IpAddress, StringComparison.InvariantCultureIgnoreCase)).Any();
        //    }
        //    return false;
        //}*/

    }

}