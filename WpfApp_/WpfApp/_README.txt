Technologies used:
* C#, .net core 3.1, WPF, unit & integration test using xUnit.

Instructions to use:
Run the application in Visual Studio 2019

* Manual scrapping is done.(Neither of HTML agility pack or google api used).

Projet details:
This software is designed to scrap google screen and render position of a partucular URL.
Detailed requirement is known to code reviewer.

Note that:
It ignored positions under ads sections or under any other social networking links on google page.
It only gets position of the actual website (smokeball.com.au) in search result.
*But, the code logic can be modified to get all appearances of the web page(smokeball.com.au) including under ads(See code comment in GoogleScrappingService.cs).

*Couple of Unit & integration tests are written.

Limitations:
1. It scraps only google search page(as in requirement)
2. The page size  is hardcoded to 100(as in requirement)

