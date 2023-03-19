This Azure function, called ValidateEmail, validates email addresses from HTTP Post requests. The validation is done using a regular expression. If the email address does not match the regular expression, an error is returned; if it does, validation is successful. 

Function content
The ValidateEmail function listens to HTTP post requests and performs email address validation using the parameter named email.

Input parameters
The HTTP Post request receives an email address with the parameter named email.

Output
If the email address passes the validation process, a HTTP 200 (OK) and the message "Email is valid" is returned. If the email address fails validation, HTTP 400 (Bad Request) and the message "Email is invalid" is returned.

How to use
An email address is sent to the ValidateEmail function via an HTTP post request. This email address is sent with the parameter named email. If the email address passes the validation process, a HTTP 200 (OK) and the message "Email is valid" is returned. If the email address fails the validation process, an HTTP 400 (Bad Request) response is returned with the message "Email is invalid".

Note: sample http mail requests are available in test.http, if you are using vscode you can test directly with the httpyac extension. sample screenshot below;

![image](https://user-images.githubusercontent.com/72391210/226203790-0ddf8608-9959-4bf6-ad2e-5ebdd47e18f3.png)
