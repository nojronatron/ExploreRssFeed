# RSS Feed Getter

An experimental project that will display RSS Feed data in a dynamic web page.

## Overview

This is a simple experimental Web App to get the RSS feed from a website and display it in a simple HTML page.

## Features

- Built using ASP.NET Blazor 8.0

## Requirements

- RSS Feed URLs must be known, valid, and configured in `appsettings.json`
- DotNET 8.0
- An internet connection

## Future

As time permits, I might add new features to this project. Some top-of-mind ideas:

- Dynamically adding RSS Feed URLs based on appsettings or user input.
- Updating the Style, perhaps to a theme or two.
- Better processing of feed data to ensure content is displayed correctly. For example, some RSS Feed elements contain HTML tags, others do not, so I assume there will be still others that contain all sorts of unexpected delimiters, character sets and encodings, etc.
- Add caching.
- Explore effecient use of Components and Render Fragments.
