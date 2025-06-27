# RSS Feed Getter

An experimental project that will display RSS Feed data in a dynamic web page.

## Overview

This is a simple experimental Web App to get the RSS feed from a website and display it in a simple HTML page.

## Status

[![Debug Build and Test](https://github.com/nojronatron/ExploreRssFeed/actions/workflows/test.yml/badge.svg)](https://github.com/nojronatron/ExploreRssFeed/actions/workflows/test.yml)
[![Release Build](https://github.com/nojronatron/ExploreRssFeed/actions/workflows/build.yml/badge.svg)](https://github.com/nojronatron/ExploreRssFeed/actions/workflows/build.yml)

## Features

- Built using ASP.NET Blazor 8.0 following best practices whenever possible.
- Blazor Interactive Razor Pages.
- Use configuration file to set RSS feed source targets.
- Fetches, processes, and neatly displays RSS feed data.
- Enable processing some native HTML content from the feed.
- Caches RSS feed data for an hour.
- Bundle-ready using LigerShark's WebOptimizer package.
- Dynamically add RSS Feed URLs in Blazor page, backed by EntityFrameworkCore SQLite.
- Display publish date of each RSS feed item.
- User can click on a feed item to open the website of the feed target URL.
- User can select whether feed sources open in same or new tab/window.

## Requirements

- DotNET 8.0 Runtime.
- An internet connection.
- You must know the RSS Feed website url (there is no auto discovery) to add it in the Configure web page.

_Note_: This project uses [.NET Aspire 9](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) for development and testing. See also [Aspireify.net](https://aspireify.net/) and [.Net Aspire Blog Articles](https://devblogs.microsoft.com/dotnet/category/dotnet-aspire/).

## Future

As time permits new features may be added. Some top-of-mind ideas:

- Update style.
- Implement themes.
- Ensure accessibility.
- Ensure responsive design for many form factors, devices.
- Better processing of feed data to ensure HTML and other content is displayed correctly but executable code is ~~processed~~ disarmed.
- Explore efficient use of Components and Render Fragments.
- Paginate results.
- Use SQL Server or PostgreSQL to better support deployment to cloud.
- Make the cache timeout configurable (examples: appsettings, environment variable, or simply adding a push-button to clear current cache).
- Upgrade to .NET 9
- Upgrade .NET Aspire to 9.3
- Update Entity Model to calculate the route when requested, rather than store it.

## References and Kudos

- [.NET Aspire 9.3](https://aspireify.net/a/250519/.net-aspire-9.3-is-here-and-enhanced-with-github-copilot)
- [.NET 9.x](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/overview)
- [Aspireify.net](https://aspireify.net/)
- [LigerShark WebOptimizer](https://github.com/ligershark/WebOptimizer)
- [ASP.NET Core Blazor in .NET 8.0](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-8.0)
