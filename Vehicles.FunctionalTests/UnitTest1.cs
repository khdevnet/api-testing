using System.Reflection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using Vehicles.FunctionalTests.Clients;

namespace Vehicles.FunctionalTests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var factory = new WebApplicationFactory<Program>();

        var c = factory.CreateClient();

        var responseMessage = await c.GetAsync("/swagger/v1/swagger.json");
        var nswagJson = await responseMessage.Content.ReadAsStringAsync();

        var document = await OpenApiDocument.FromJsonAsync(nswagJson);


        var settings = new CSharpClientGeneratorSettings
        {
            CSharpGeneratorSettings =
            {
                Namespace = "Vehicles.FunctionalTests.Clients"
            }
        };

        var generator = new CSharpClientGenerator(document, settings);
        var code = generator.GenerateFile();

        var currentPath = Directory.GetCurrentDirectory();
        var projectPath = Path.Combine(currentPath, "..", "..", "..");
        File.WriteAllText(Path.Combine(projectPath, "Clients", "Clients.cs"), code);
    }

    [Fact]
    public async Task Test2()
    {
        var factory = new WebApplicationFactory<Program>();

        var c = factory.CreateClient();
        var vc = new VehiclesClient(c);
        var d = await vc.GetAsync();

    }
}