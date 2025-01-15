# RSS Feed Getter

An experimental project that will display RSS Feed data in a dynamic web page.

## Overview

This is a simple experimental Web App to get the RSS feed from a website and display it in a simple HTML page.

## Features

- Built using ASP.NET Blazor 8.0 following best practices whenever possible.
- Uses Streamed, interactive pages.
- Use configuration file to set RSS feed source targets.
- Fetches, processes, and neatly displays RSS feed data.
- Enable processing some native HTML content from the feed.
- Caches RSS feed data for an hour.
- Bundle-ready using LigerShark's WebOptimizer package.

## Requirements

- RSS Feed URLs must be known, valid, and configured in `appsettings.json`
- DotNET 8.0
- An internet connection

## Future

As time permits, I might add new features to this project. Some top-of-mind ideas:

- Dynamically adding RSS Feed URLs.
- Update style.
- Implement themes.
- Ensure accessibility.
- Display publish date of each RSS feed item.
- Better processing of feed data to ensure content can be safely displayed, especially HTML content, but not executable code.
- Explore effecient use of Components and Render Fragments.
- Paginate results.

## References and Kudos

- [LigerShark WebOptimizer](https://github.com/ligershark/WebOptimizer)
