using System.Linq.Expressions;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
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

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

        if (regex.IsMatch(email.Trim())) {
            return new OkObjectResult("Email is valid.");
        } else {
            return new BadRequestObjectResult("Email is invalid.");
        }

    }
}
