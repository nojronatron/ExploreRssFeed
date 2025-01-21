# RSS Feed Getter

An experimental project that will display RSS Feed data in a dynamic web page.

## Overview

This is a simple experimental Web App to get the RSS feed from a website and display it in a simple HTML page.

## Features

- Built using ASP.NET Blazor 8.0 following best practices whenever possible.
- Blazor Interactive Razor Pages.
- Use configuration file to set RSS feed source targets.
- Fetches, processes, and neatly displays RSS feed data.
- Enable processing some native HTML content from the feed.
- Caches RSS feed data for an hour.
- Bundle-ready using LigerShark's WebOptimizer package.
- Dynamically adding RSS Feed URLs, using EntityFrameworkCore SQLite.

## Requirements

- RSS Feed URLs must be known, valid, and configured in `appsettings.json`
- DotNET 9.0
- An internet connection

## Future

As time permits new features may be added. Some top-of-mind ideas:

- Update style.
- Implement themes.
- Ensure accessibility.
- Ensure responsive design for many form factors, devices.
- Display publish date of each RSS feed item.
- Better processing of feed data to ensure HTML and other content is displayed correctly but executable code is processed.
- Explore effecient use of Components and Render Fragments.
- Paginate results.
- Use SQL Server or PostgreSQL to better support deployment to cloud.

## References and Kudos

- [LigerShark WebOptimizer](https://github.com/ligershark/WebOptimizer)
- [ASP.NET Core Blazor in .NET 9.0](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-9.0)
