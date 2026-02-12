var builder = DistributedApplication.CreateBuilder(args);

// Add the API project
var api = builder.AddProject<Projects.PoopNPour_Api>("api")
    .WithExternalHttpEndpoints();

// Add the WebApp (Vue.js/Vite) project
var webapp = builder.AddJavaScriptApp("webapp", "../../src/PoopNPour.WebApp", "dev")
    .WithWorkingDirectory("../../src/PoopNPour.WebApp")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .WithEnvironment("VITE_API_URL", api.GetEndpoint("http"))
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
