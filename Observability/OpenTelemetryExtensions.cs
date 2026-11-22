using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WebApplicationRestaurant.Observability;

public static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder AddRestaurantObservability(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        if (builder.Environment.IsDevelopment())
        {
            builder.Logging.AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
                options.TimestampFormat = "HH:mm:ss ";
            });
            builder.Logging.AddDebug();
        }
        else
        {
            builder.Logging.AddJsonConsole(options =>
            {
                options.IncludeScopes = true;
                options.UseUtcTimestamp = true;
                options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
            });
        }

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.ParseStateValues = true;
        });

        var serviceVersion = typeof(OpenTelemetryExtensions).Assembly.GetName().Version?.ToString() ?? "1.0.0";

        var otelBuilder = builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(
                serviceName: RestaurantTelemetry.ServiceName,
                serviceVersion: serviceVersion))
            .WithTracing(tracing => tracing
                .AddSource(RestaurantTelemetry.ActivitySourceName)
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.Filter = httpContext =>
                        !httpContext.Request.Path.StartsWithSegments("/lib")
                        && !httpContext.Request.Path.StartsWithSegments("/css")
                        && !httpContext.Request.Path.StartsWithSegments("/js")
                        && !httpContext.Request.Path.Equals("/favicon.ico", StringComparison.OrdinalIgnoreCase);
                })
                .AddHttpClientInstrumentation()
                .AddNpgsql())
            .WithMetrics(metrics => metrics
                .AddMeter(RestaurantTelemetry.MeterName)
                .AddMeter("Npgsql")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation());

        var otlpEndpoint = builder.Configuration["OpenTelemetry:OtlpEndpoint"];
        if (string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        }

        if (!string.IsNullOrWhiteSpace(otlpEndpoint) && Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var otlpUri))
        {
            var otlpProtocol = builder.Configuration["OpenTelemetry:OtlpProtocol"]
                ?? builder.Configuration["OTEL_EXPORTER_OTLP_PROTOCOL"];

            var protocol = string.Equals(otlpProtocol, "http/protobuf", StringComparison.OrdinalIgnoreCase)
                ? OtlpExportProtocol.HttpProtobuf
                : OtlpExportProtocol.Grpc;

            otelBuilder.UseOtlpExporter(protocol, otlpUri);
        }

        return builder;
    }
}
