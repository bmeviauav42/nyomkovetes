using Jaeger.Samplers;
using Jaeger;
using Microsoft.Extensions.Logging;
using OpenTracing.Contrib.NetCore.CoreFx;
using OpenTracing.Util;
using OpenTracing;
using System.Reflection;
using System;
using Jaeger.Reporters;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JaegerServiceCollectionExtensions
    {
        // Registers and starts Jaeger.
        // Also registers OpenTracing related services!
        // Note: IHostEnvironment requires.NET Core 3.0. Either eliminate it if you don't need it, or use IHostingEnvironment 
        // instead.
        public static IServiceCollection AddJaeger(this IServiceCollection services, IHostEnvironment env, string serviceName = null)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton<ITracer>(serviceProvider =>
            {
                // Two sensible alternatives
                serviceName ??= serviceProvider.GetRequiredService<IHostEnvironment>().ApplicationName;
                // serviceName ??= Assembly.GetEntryAssembly().GetName().Name;

                // This will be the service service name logged with the spans, displayed by Jaeger UI
                Environment.SetEnvironmentVariable("JAEGER_SERVICE_NAME", serviceName);

                // Based on env we can have different configuration for development and production environments
                if (env != null && env.IsDevelopment())
                {
                    // Check if we receive the specified env vars: if not, set defaults

                    var agentHost = Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST"); // Host name for the jager agent
                    if (agentHost == null)
                        Environment.SetEnvironmentVariable("JAEGER_AGENT_HOST", "jaeger");

                    var agentPort = Environment.GetEnvironmentVariable("JAEGER_AGENT_PORT");
                    if (agentHost == null)
                        Environment.SetEnvironmentVariable("JAEGER_AGENT_PORT", "6831");

                    var sampler = Environment.GetEnvironmentVariable("JAEGER_SAMPLER_TYPE");
                    if (agentHost == null)
                        Environment.SetEnvironmentVariable("JAEGER_SAMPLER_TYPE", "const");
                }
                else
                {
                    // As for now do nothing, let's assume all environment variables are
                    // set properly in a production environment
                }

                // We could also create a new factory and configure it for our needs
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                // Get Jaeger config from environment
                var config = Jaeger.Configuration.FromEnv(loggerFactory);
                // We could further modify Jaeger config here
                var tracer = config.GetTracer();

                if (!GlobalTracer.IsRegistered())
                    // Allows code that can't use DI to also access the tracer.
                    GlobalTracer.Register(tracer);

                return tracer;
            });

            // This has significance only when you send spans directly to the collector via HttpSender,
            // e.g. if JAEGER_ENDPOINT is specified. This is not the case in out setup, but keep it here.
            // Prevent endless loops when OpenTracing is tracking HTTP requests to Jaeger.
            // See https://github.com/jaegertracing/jaeger-client-csharp/issues/154
            //services.Configure<HttpHandlerDiagnosticOptions>(options =>
            //{
            //    // Update Url properly
            //    Uri _jaegerUri = new Uri("http://localhost:14268/api/traces");

            //    options.IgnorePatterns.Add(request => _jaegerUri.IsBaseOf(request.RequestUri));

            //    // This is not needed, just demonstrates some possibilities
            //    options.OnError = (span, exception, message) =>
            //    {

            //    };
            //    options.OnRequest = (span, request) =>
            //    {

            //    };
            //});

            // Also add Open Tracing related services
            services.AddOpenTracing();

            return services;
        }
    }
}