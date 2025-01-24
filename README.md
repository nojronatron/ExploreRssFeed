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

## Requirements

- DotNET 8.0 Runtime.
- An internet connection.
- You must know the RSS Feed website url (there is no auto discovery) to add it in the Configure web page.

## Future

As time permits new features may be added. Some top-of-mind ideas:

- Update style.
- Implement themes.
- Ensure accessibility.
- Ensure responsive design for many form factors, devices.
- Better processing of feed data to ensure HTML and other content is displayed correctly but executable code is processed.
- Explore efficient use of Components and Render Fragments.
- Paginate results.
- Use SQL Server or PostgreSQL to better support deployment to cloud.

## References and Kudos

- [LigerShark WebOptimizer](https://github.com/ligershark/WebOptimizer)
- [ASP.NET Core Blazor in .NET 8.0](https://learn.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-8.0)
