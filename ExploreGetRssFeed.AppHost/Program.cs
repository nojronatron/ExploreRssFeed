var builder = DistributedApplication.CreateBuilder(args);

var web = builder.AddProject<Projects.ExploreGetRssFeed>("GetRssFeedWeb");

builder.Build().Run();
