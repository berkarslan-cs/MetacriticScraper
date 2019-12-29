# MetacriticScraper

[![Build Status](https://dev.azure.com/berkarslan-cs/MetacriticScraper/_apis/build/status/berkarslan-cs.MetacriticScraper?branchName=master)](https://dev.azure.com/berkarslan-cs/MetacriticScraper/_build/latest?definitionId=2&branchName=master)

## What is MetacriticScraper
This is a scraper project for Metacritic based on .NET Core. UI will also be provided.

##How to use it?
It has a single API method, which is a GET endpoint and it accepts the following filter parameters:
- Platform: Possible values are: 0 (PC), 1 (PS4), 2 (XBox One)
- MinMetaScore: Metascore int type value which is between 0-100.
- MinUserScore: User score decimal type value which is between 0-10.
- MinReleaseDate: Minimum release date for the games. Setting this property will return the games which are released between MinReleaseDate-DateTime.Now.

A sample request url can be like the following: "https://localhost:44380/api/Metacritic?Platform=0&MinMetaScore=75&MinUserScore=6.5&MinReleaseDate=2019-11-01"
