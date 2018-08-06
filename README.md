# Weather.Universal
A weather app that is universal across several Windows operating systems and devices. Do not fear Windows 7 and 8 users (as well as earlier). We have you covered :)

[![Build & Test][win-build-badge]][win-build]

[win-build-badge]:
https://dynamensions.visualstudio.com/Weather/_apis/build/status/Weather-CI?branchName=master
[win-build]: https://dynamensions.visualstudio.com/Weather/_build?_a=completed&definitionId=9

## Design
Created using a MVVM archetecture. It also utilizes Inversion of Control (IoC) where the driver for the application is the Core project. These projects will start as Visual Basic .NET projects, but there may also be C# projects if more users come into the mix.

## Weather Sources
At the moment thse project will source NOAA's feeds and services. But will extend to other facets as the project grows.
