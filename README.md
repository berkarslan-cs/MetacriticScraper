# MetacriticScraper

[![Build Status](https://dev.azure.com/berkarslan-cs/MetacriticScraper/_apis/build/status/berkarslan-cs.MetacriticScraper?branchName=master)](https://dev.azure.com/berkarslan-cs/MetacriticScraper/_build/latest?definitionId=2&branchName=master)

## What is MetacriticScraper
This is a scraper project for Metacritic based on .NET Core. A simple UI is also provided.

## How to use it?
It has a single API method, which is a GET endpoint and it accepts the following filter parameters:
- Platform: Possible values are: 0 (PC), 1 (PS4), 2 (XBox One)
- MinMetaScore: Metascore int type value which is between 0-100.
- MinUserScore: User score decimal type value which is between 0-10.
- MinReleaseDate: Minimum release date for the games. Setting this property will return the games which are released between MinReleaseDate-DateTime.Now.

A sample API request url can be like the following: https://localhost:44380/api/Metacritic?Platform=0&MinMetaScore=75&MinUserScore=6.5&MinReleaseDate=2019-11-01

UI can be browsed from the following url: https://localhost:44380/index.html

## Sample UI
![image](https://user-images.githubusercontent.com/14029115/95655323-13d3e180-0b0f-11eb-86a3-380df57b3bb3.png)
