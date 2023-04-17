# MetacriticScraper

[![Build Status](https://dev.azure.com/berkarslan-cs/MetacriticScraper/_apis/build/status/berkarslan-cs.MetacriticScraper?branchName=master)](https://dev.azure.com/berkarslan-cs/MetacriticScraper/_build/latest?definitionId=2&branchName=master)

## About MetacriticScraper
MetacriticScraper is a scraper project for Metacritic, built using .NET Core. It also includes a simple UI.

## How to Run MetacriticScraper
You can run MetacriticScraper in several ways, such as using Docker, IIS Express, or Self Hosting. To run the project using Docker (assuming Docker is installed), enter the following command:

```
docker run -dp 8081:443 berkarslan/metacriticscraper
```

Then, navigate to https://localhost:8081/index.html to access the project.

## How to Use MetacriticScraper
MetacriticScraper includes a single API method, which is a GET endpoint. The API accepts the following filter parameters:
- **Platform**: Possible values are: 0 (PC), 1 (PS4), 2 (XBox One)
- **MinMetaScore**: Metascore int type value which is between 0-100.
- **MinUserScore**: User score decimal type value which is between 0-10.
- **MinReleaseDate**: Minimum release date for the games. Setting this property will return the games which are released between MinReleaseDate-DateTime.Now.

A sample API request URL can look like the following:
https://localhost:8081/api/Metacritic?Platform=0&MinMetaScore=75&MinUserScore=6.5&MinReleaseDate=2019-11-01

## Sample UI Screenshot
![image](https://user-images.githubusercontent.com/14029115/95655323-13d3e180-0b0f-11eb-86a3-380df57b3bb3.png)
