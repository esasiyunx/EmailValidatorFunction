using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System;

namespace EmailValidatorFunction.EMail.Triggers;

public static class CheckEmailValidityTrigger {

    [FunctionName("CheckEmailValidity")]
    public static async Task<IActionResult> ValidateEmail(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req) {
        try {
            string email = req.Query["email"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // Check for email in both the query string and the request body
            email = email ?? data?.email;

            // If email is null or empty string, return a bad request
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

            // Create a regex pattern to validate the email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Check if the email matches the regex pattern
            if (regex.IsMatch(email.Trim())) {
                return new OkObjectResult("Email is valid.");
            } else {
                return new BadRequestObjectResult("Email is invalid.");
            }
        }
        catch (Exception ex) {
            return new BadRequestObjectResult("An error occurred while validating the email: " + ex.Message);
        }
    }
}
