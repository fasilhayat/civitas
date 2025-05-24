namespace Civitas.Api.Health;

using HealthChecks.UI.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

/// <summary>
/// Specialized version of UIResponseWriter part of
/// https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/tree/master/src/HealthChecks.UI.Client
/// </summary>
public static class HealthWriter
{
    private const string DefaultContentType = "application/json";
    private static readonly byte[] EmptyResponse = "{}"u8.ToArray();
    private static readonly Lazy<JsonSerializerOptions> Options = new(CreateJsonOptions);
    private static List<string>? _assemblies;

    /// <summary>
    /// Write the Health in HealthUI format
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="report"></param>
    /// <returns></returns>
    public static async Task WriteHealthUiResponse(HttpContext httpContext, HealthReport? report)
    {
        if (report != null)
        {
            httpContext.Response.ContentType = DefaultContentType;
            var uiReport = GenerateReport(report);
            await JsonSerializer.SerializeAsync(httpContext.Response.Body, uiReport, Options.Value);
        }
        else
        {
            await httpContext.Response.WriteAsync(Encoding.UTF8.GetString(EmptyResponse));
        }
    }

    /// <summary>
    /// Write the Health in HealthUI format
    /// </summary>
    /// <param name="report">The health report to write.</param>
    /// <returns>Returns a stream containing the serialized health report in HealthUI format.</returns>
    public static Stream WriteHealthUiResponse(HealthReport? report)
    {
        MemoryStream memoryStream;
        if (report != null)
        {
            var uiReport = GenerateReport(report);
            memoryStream = new();
            JsonSerializer.Serialize(memoryStream, uiReport, typeof(UIHealthReport), Options.Value);
            memoryStream.Position = 0;

            return memoryStream;
        }

        memoryStream = new(EmptyResponse);
        return memoryStream;
    }

    /// <summary>
    /// Generates a UIHealthReport from the provided HealthReport.
    /// </summary>
    /// <param name="report">Report to convert.</param>
    /// <returns>The generated UIHealthReport.</returns>
    private static UIHealthReport GenerateReport(HealthReport? report)
    {
        var entries = new Dictionary<string, HealthReportEntry>();
        if (report?.Entries != null)
        {
            // Transfer existing entries if exists
            entries = report.Entries.Select(dict => dict).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        entries.Add("Assembly info", new HealthReportEntry(report.Status, Assembly.GetEntryAssembly()?.FullName ?? "NA", TimeSpan.Zero, null, null, new string[] { "version" }));

        // Only get assemblies once as it is a heavy task
        if (_assemblies == null)
        {
            _assemblies = new();
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly =>
            {
                if (assembly.FullName != null) _assemblies.Add(assembly.FullName);
            });
        }

        entries.Add("Assemblies", new HealthReportEntry(report.Status, string.Join("<br>", _assemblies), TimeSpan.Zero, null, null, new string[] { "version" }));
        var healthReport = new HealthReport(entries, report.Status, report.TotalDuration);

        var uiReport = UIHealthReport.CreateFrom(healthReport);
        return uiReport;
    }

    /// <summary>
    /// Generates the JSON serializer options used for serialization of the health report.
    /// </summary>
    /// <returns>The configured <see cref="JsonSerializerOptions"/>.</returns>
    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        options.Converters.Add(new JsonStringEnumConverter());

        // for compatibility with older UI versions ( <3.0 ) we arrange
        // timespan serialization as s
        options.Converters.Add(new TimeSpanConverter());

        return options;
    }
}

/// <summary>
/// For compatibility with older UI versions ( less than 3.0 ) we arrange timespan serialization as string.
/// </summary>
internal class TimeSpanConverter : JsonConverter<TimeSpan>
{
    /// <summary>
    /// Reads a TimeSpan value from the JSON reader.
    /// </summary>
    /// <param name="reader">Ref to the JSON reader.</param>
    /// <param name="typeToConvert">the type to convert.</param>
    /// <param name="options"></param>
    /// <returns>The TimeSpan value read from the JSON.</returns>
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return default;
    }

    /// <summary>
    /// Writes a TimeSpan value to the JSON writer as a string representation of the TimeSpan.
    /// </summary>
    /// <param name="writer">The JSON writer to write to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">Options for the JSON serialization.</param>
    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}