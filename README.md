# Hive Financial Systems Take Home Assessment

Hello there! Thank you for your interest in working at Hive Financial Systems. We are
excited to learn more about you and your experience.

This folder has a very simple .NET 7 Web API project, along with an xUnit test project.
The goal is to get this API "production ready", by adding a few small features,
cleaning up any issues you see with the code, and achieving an appropriate level of
unit test coverage.

This take home assessment should take between one and two hours. Once you've completed
all of the tasks below, please:

 1. Ensure that all changes have been committed to the local offline git repository
 1. ZIP up the contents of the current folder, including the `.git` subdirectory
 1. Email the ZIP file back to the Hive developer who reached out to you

Thank you again for your interest! We look forward to following up with you.

## Overview

Feel free to use any software or tool that works best for you. If you do not have an
IDE readily available, the simplest is probably the free
[Visual Studio Code](https://code.visualstudio.com/) along with the
[.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0).

With these installed, you can either use your IDE to build and run the solution or
run the following through a command-line interface in the root of this folder:

```shell
dotnet build

dotnet run --project HiveFS.TakeHomeAssessment
```

This will start a web server with a Swagger execution page for testing the API, available
at [https://localhost:7184/swagger/](https://localhost:7184/swagger/).

## Features

### :apple: :banana: :pear: :watermelon: :peach: :grapes: :strawberry: :cherries:

While our weather "business" is booming, we need to diversify our portfolio. Product has
determined that fruits are the wave of the future.

Using the public API available at [Fruityvice](https://www.fruityvice.com/), create an API
endpoint that, given the name of a fruit, returns:

 - number of calories 
 - grams of carbohydrates

Design the API endpoint as you best see fit.

### Caching

Compared to the weather, fruit data is much more static. Please add appropriate caching
to the application, both to cut down on unnecessary calls to the upstream data provider
as well as optimizing responses to callers of our API.

### Logging

Our API needs enhancements in its observability, starting with logging. Please add
appropriate logging, as you see fit, to the project, logging information at the level
best fit to the criticality of the log message.

### Error Handling

Problems happen all the time, especially when relying on systems outside of our control.
Please add error handling, to your satisfaction, returning appropriate responses to the
API caller in case of issue.

## Quality

There are a number of places in the template solution that, let's just say, are non-
optimal. Do your best to make the code as ideally as you see fit, treating it as if
this was a real API you were looking to get into production.

Use the latest features of .NET 7 that you are comfortable with; feel free to pull in
any NuGet packages that you think would enhance the solution.

## Tests

It should go without saying the importance of tests in maintaining a high quality code
base. Unfortunately, the template project was not designed with testability in mind.

Please refactor the code as needed, keeping the existing functionality, but making it
so we can achieve a high level of unit test code coverage. Add appropriate unit tests
for the enhancements implemented above.

For the scope of this project, just focus on unit tests.
