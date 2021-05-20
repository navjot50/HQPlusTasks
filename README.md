## Explanation and design decisions

I have implemented all the three tasks. The solution consists of 7 projects.

Project | Remark
------------ | -------------
**HQPlus.Data** | Class library referenced by HQPlus.Task3.Api and HQPlus.Task2
**HQPlus.Task3.Api** | Implementation of Task3 as a REST Web api
**HQPlus.Task3.Api.Tests** | Test project corresponding to HQPlus.Task3.Api
**HQPlus.Task2** | Implementation of Task2 as a worker service
**HQPlus.Task2.Tests** | Test project corresponding to HQPlus.Task2
**HQPlus.Task1** | Implementation of Task1 as a class library
**HQPlus.Task1.Tests** | Test project corresponding to HQPlus.Task1

I enumerated the projects in the same order I implemented them through the week.

**Disclaimer: I am on a Mac, so I am using Rider as my IDE and I have used XUnit as my testing framework (as I have experience with XUnit only)**

### HQPlus.Data
Consists of Data Model classes along with extension methods which are shared between HQPlus.Task3.Api and HQPlus.Task2.

### HQPlus.Task3.Api
It is a REST web api which exposes a GET endpoint http://localhost:5000/api/hotels/{id} (Versioning not implemented as discussed)

#### Endpoint Input
The endpoint accepts two parameters:
1. id - passed as a path variable
2. arrivalDate - passed as a query parameter

**Remarks:**
1. The id is represented as a long value in the api.
2. The arrivalDate must be in ISO format viz. yyyy-MM-dd. If the date is not given in this format the api will respond with 400 bad request status.

#### Possible Outputs
1. BadRequest - if the input parameters viz. are in incorrect format (see Remarks above)
2. NotFound - if there is not hotel for given id
3. Ok with response json - filters the hotel and hotelRates for given id and arrivalDate.

**Remarks:**
The response json is in the same format as in the hotelsrates.json

### HQPlus.Task3.Tests
Consists of Unit tests and Integration tests.
1. The unit tests make sure that we are filtering the data correctly (in this case the storage layer is the hotelsrates.json)
2. Integration tests consists of api endpoint tests which make sure that correct output status and response is given for all scenarios

### HQPlus.Task2
It is a background worker service which produces the required Excel report.

**Remarks:**
1. Implementing it as a Console application was also an option but I went with a worker service because the optional question
   mentioned that this functionality will be invoked daily to send out excel report emails.
2. In this case it makes sense to implement it as a worker service as we can simply add a Cron job on top of it.
   The solution can also be expanded to generate multiple reports at different times in a day, hence I implemented it as a worker service.
   
**Implementation details:**
1. The service reads only hotelRates data from hotelsrates.json file as the report only makes use of the hotelRates data
2. The reading logic makes use of `IAsyncEnumerable` which yields each hotelRate object read from the json file. By using yield
   we enable lazy evaluation of data in case we have a very long list of hotelRate objects.
3. The hotelRate objects are then mapped to the expected Excel report row objects and populated in a .Net DataTable
4. Finally, the DataTable is populated in an Excel sheet. I am using `ClosedXML` as the Excel library.
5. An Output.xlsx file is generated in the project content root on completion.

**Remarks:**

1. At the moment, the implementation only reads the required data and maps it to the excel expectation. The solution can also
   be made more general to use data engineering concepts like Dataframes to make the generation of report variants more quick
   and easy. **Deedle** is one of the .Net libraries to achieve this.
   I did not implement Dataframes because the scope and knowledge of this task was limited and I did not want to overengineer
   things. But I had these concepts in back of my mind throughout the implementation of this task.
2. The name, sheet name, and the location where to generate the excel report are configuration via appsettings.json file
   (Currently, these options reside in appsettings.Development.json file)
3. ClosedXML is a nice library, but there are some open bugs (for e.g. worksheet.AdjustToContents() throws exception on Mac)
   There were some number format issues as well. Because of this I had to resort to some 4-5 hacky lines of code. These bugs may
   or may not be applicable to Windows machines.
   
#### Answer to optional architecture question

We can use a Cron job on top of background worker service. Popular cron jobs libraries are Quartz.net and Hangfire.
   
### HQPlus.Task2.Tests
1. Unit tests make sure that the data is read correctly from hotelsrates.json file and they make sure that the hotelRate objects
   are correctly mapped to excel rows.
2. Integration tests make sure that excel sheet is generated with correct data and the interaction b/w Worker service and the report
   generator.
   
### HQPlus.Task1
It is a class library which implements a Booking.com data extractor.

**Remarks:**
1. It could have also been implemented as a Console application. But I went on with a class library as it was not known to me where this
   piece of code is required.
2. I made sure that the library exposes only the public contract hiding all the implementation details inside it.

**Implemenation details:**
1. The library exposes `IBookingDotComScraper` interface which has three methods. One accepts the html string, one accepts a stream of
   html content and one accepts a file path. All the methods give same json output given that the arguments lead to same html content.
   There is another parameter for filtering the nulls in output json. It has a default setting to ignore nulls in output json but it
   can be overriden by passing false to the invocation.

2. All other classes have internal visibility as they are implementation details. The public behavior is exposed by the interface only.

3. There is a factory class which exposes the `Create` method for clients to instantiate a scraper.

4. I tried to keep my class count less in this project as I believe such projects are more inclined to functional approach rather than
   OOP approach. There is a class `BookingDotComComponentsScraper` which has functions to scrape the target html sections in the html page. It is a stateless class with functions only. Currently, this is not a static class but we can also make the class and all the functions static (functional approach). I did not implement them as static functions because they are controversial in OOP environments. But it will totally fit that way as well. We can even move all these functions inside the class which implements the
   `IBookingDotComScraper` class. We can use partial classes for more readability.

5. The project uses AngleSharp library for DOM parsing.

### HQPlus.Task1.Tests

I have implemented End to End tests only which test the public behavior of `IBookingDotComScraper`. I did not implement fine-grained unit tests because scrapers can often break depending on DOM changes. One should be pragmatic and measure the value to maintainability ratio of tests. In this case, I went ahead with End to End tests only.
   



