using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace EmailValidatorFunction.EMail.Triggers;

public static class CheckEmailValidityTrigger {

    [FunctionName("CheckEmailValidity")]
    public static async Task<IActionResult> ValidateEmail(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req) {

        string email = req.Query["email"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);

        email = email ?? data?.email;

        if (string.IsNullOrWhiteSpace(email)) {
            return new BadRequestObjectResult("please pass an email on the query string or in the request body.");
        }

        // Description of Regex:
        // ^ : start character
        // [a-zA-Z0-9._%+-]+ : sequence of characters containing at least one uppercase or lowercase letter, number, period, underscore, plus, minus or percent sign.
        // @ : at sign
        // [a-zA-Z0-9.-]+ : sequence of characters containing at least one lowercase letter, one uppercase letter, one number, one period and one hyphen.
        // \. : a period
        // [a-zA-Z]{2,} : string of at least two characters containing only upper or lower case letters (domain name)
        // $ : end character

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

        if (regex.IsMatch(email.Trim())) {
            return new OkObjectResult("Email is valid.");
        } else {
            return new BadRequestObjectResult("Email is invalid.");
        }

    }
}
