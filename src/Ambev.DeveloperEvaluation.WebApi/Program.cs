using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();
            
            // Execute migrations automatically on startup with retry logic
            await ApplyDatabaseMigrationsAsync(app.Services);
            
            async Task ApplyDatabaseMigrationsAsync(IServiceProvider services)
            {
                const int maxRetries = 10;
                const int delaySeconds = 5;
                
                for (int attempt = 1; attempt <= maxRetries; attempt++)
                {
                    try
                    {
                        using var scope = services.CreateScope();
                        var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();
                        
                        Log.Information("Attempting to apply database migrations (attempt {Attempt}/{MaxRetries})...", 
                                      attempt, maxRetries);
                        
                        // Test connection first
                        await context.Database.CanConnectAsync();
                        
                        // Apply migrations
                        await context.Database.MigrateAsync();
                        
                        Log.Information("Database migrations applied successfully");
                        return;
                    }
                    catch (Exception ex) when (attempt < maxRetries)
                    {
                        Log.Warning(ex, "Failed to apply migrations on attempt {Attempt}/{MaxRetries}. " + 
                                      "Retrying in {DelaySeconds} seconds...", 
                                      attempt, maxRetries, delaySeconds);
                        
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                    }
                    catch (Exception ex)
                    {
                        Log.Fatal(ex, "Failed to apply database migrations after {MaxRetries} attempts", maxRetries);
                        throw;
                    }
                }
            }
            
            app.UseMiddleware<ValidationExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
