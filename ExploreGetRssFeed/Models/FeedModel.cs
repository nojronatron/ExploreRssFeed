﻿using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ExploreGetRssFeed.Models
{
    public class FeedModel
    {
        public int? Id { get; set; }
        
        [Display(Name = "Title")]
        public string Title { get; set; } = "Untitled";
        
        [Display(Name = "Link")]
        public string Link { get; set; } = string.Empty;
        
        [Display(Name = "Creator")]
        public string Creator { get; set; } = "Unlisted";
        
        [Display(Name = "Publish Date")]
        public DateTime PubDate { get; set; }
        
        [Display(Name = "Description")]
        public string Description { get; set; } = "<p>No description</p>";
        
        [Display(Name = "Content")]
        public string Content { get; set; } = "None";
        
        [Display(Name = "NewTab")]
        public bool NewTab { get; set; } = false;

        public MarkupString DescriptionAsHtml => new MarkupString(Description);
        public MarkupString LinkAsHtml => new MarkupString($"<a href=\"{Link}\">{Link}</a>");

        private static TimeSpan DefaultTimeout => TimeSpan.FromSeconds(2);

        public static FeedModel Create(string title, string link, string pubDate, string description, bool newTab=false)
        {
            string cleanTitle = title.Trim();
            string cleanLink = CleanLink(link);
            DateTime cleanPubDate = string.IsNullOrWhiteSpace(pubDate) ? DateTime.Now : DateTime.Parse(pubDate);
            string cleanDescription = CleanDescription(description, cleanTitle);

            return new FeedModel()
            {
                Title = cleanTitle,
                Link = cleanLink,
                PubDate = cleanPubDate,
                Description = cleanDescription,
                NewTab = newTab,
            };
        }

        /// <summary>
        /// Get the content between the first pair of paragraph html elements.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="alternateDescription"></param>
        /// <returns>Plain text content or if no description, the alternate description provided</returns>
        public static string CleanDescription(string description, string alternateDescription)
        {
            // 3 cases:
            // 1. description is empty. Return the default description.
            // 2. description has content but no paragraph tags: Return the trimmed description.
            // 3. description has content AND paragraph tags: Capture the content between the first
            // pair of paragraph tags, then trim and return it

            if (string.IsNullOrWhiteSpace(description))
            {
                return alternateDescription;
            }

            string pattern = @"<p>(.*?)</p>";
            var match = Regex.Match(description, pattern, RegexOptions.IgnoreCase, DefaultTimeout);
            return match.Success ? 
                match.Groups[1].Value.Trim() : 
                description.Trim();
        }

        /// <summary>
        /// Clean the link by removing any leading characters before the 
        /// http(s) protocol and excluding the closing </link> tag.
        /// </summary>
        /// <param name="link"></param>
        /// <returns>String of only the link text</returns>
        public static string CleanLink(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return string.Empty;
            }

            string pattern = @"<link>(.*?)</link>";
            var match = Regex.Match(link, pattern, RegexOptions.IgnoreCase, DefaultTimeout);
            return match.Success ? match.Groups[1].Value.Trim() : link.Trim();
        }

        /// <summary>
        /// Format the date to a more readable format.
        /// </summary>
        /// <returns>Example: Monday, 04 July 2024</returns>
        public string PubDateFormatted()
        {
            return string.Format("{0:ddd, d MMM yyyy}", PubDate);
        }
    }
}
