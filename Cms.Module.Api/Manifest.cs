using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Cms.Module.Api",
    Author = "The Orchard Core Team",
    Website = "https://orchardcore.net",
    Version = "0.0.1",
    Description = "Cms.Module.Api",
    Category = "Cms.Module.Api"
)]
[assembly: Feature(
    Id = "Cms.Module.Api",
    Name = "Cms.Module.Api",
    Description = "Provides a Test api",
    Category = "Samples"
)]
[assembly: Feature(
    Id = "Cms.Module.Data",
    Name = "Cms.Module.Data",
    Description = "Sample for creating separate table and index.",
    Category = "Samples",
    Dependencies = ["Cms.Module.Api"]
)]
[assembly: Feature(
    Id = "Cms.Module.Scripting",
    Name = "Cms.Module.Scripting",
    Description = "Adding debugging to the script task",
    Dependencies = ["OrchardCore.Workflows"],
    Category = "Samples"
)]

