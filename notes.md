# Solution Notes
1. Error handling is handled globally in the middleware ErrorHandlingMiddleware
1. Authentication is implemented using JWT.  Using Swagger, first call the Authenticate controller post login method and pass in the 
userid and password stored in the appsettings. Click the authorize button on the Swagger page and enter the token 
prefixed by Bearer: and a space.  Then you can call the APIs.
1. Logging is implemented using Serilog, it logs to the console and a log file.
1. Authorization is enforced on the base controller HiveFsBaseController which all the controller inherit.
