# Build Configuration

## TypeScript Client Generation

The build process includes an automatic step to generate TypeScript client code from the OpenAPI specification using NSwag.

### Automatic Generation

By default, the TypeScript client is automatically generated after each build. This requires:

1. The API to be running at `https://localhost:7172`
2. Swagger endpoint to be accessible at `/swagger/v1/swagger.json`

### Disabling Automatic Generation

To skip TypeScript generation during build:

```bash
dotnet build /p:GenerateTypeScript=false
```

Or set it in your project file or Directory.Build.props:

```xml
<PropertyGroup>
  <GenerateTypeScript>false</GenerateTypeScript>
</PropertyGroup>
```

### Manual Generation

You can manually generate the TypeScript client:

```bash
cd src/PoopNPour.Api
dotnet nswag run nswag.json
```

### Output Location

The generated TypeScript client is placed at:
- `../client/src/api/api.ts` (relative to the API project)

### Configuration

The NSwag configuration is in `src/PoopNPour.Api/nswag.json`. You can customize:

- **Output path**: Change `codeGenerators.openApiToTypeScriptClient.output`
- **TypeScript version**: Change `typeScriptVersion`
- **HTTP client**: Change `template` (currently "Axios")
- **Swagger URL**: Change `documentGenerator.fromDocument.url`

### CI/CD Integration

For CI/CD pipelines where the API isn't running:

1. **Option 1**: Generate the OpenAPI spec file first, then update `nswag.json` to use the file instead of URL
2. **Option 2**: Set `GenerateTypeScript=false` and run generation as a separate step after deploying the API
3. **Option 3**: Use a build agent that can start the API, generate the client, then stop the API

### Troubleshooting

**Error: Could not fetch OpenAPI spec**
- Ensure the API is running before building
- Check that Swagger is enabled in Development mode
- Verify the URL in `nswag.json` matches your API URL

**Error: NSwag command not found**
- Run `dotnet restore` to restore packages
- Ensure `NSwag.MSBuild` package is referenced

**Generation skipped**
- Check build output for messages about TypeScript generation
- Verify `GenerateTypeScript` property is not set to `false`
