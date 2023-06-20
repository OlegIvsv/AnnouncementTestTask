using Announcement.Infrastructure.Database;
using Announcement.Infrastructure.DateAndTime;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
    {
        string redisConnString = builder.Configuration.GetConnectionString("RedisConnection")!;
        return ConnectionMultiplexer.Connect(redisConnString);
    });
    builder.Services.AddScoped<IAnnouncementRepo, RedisAnnouncementRepo>();
    builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    builder.Host.UseSerilog((hostingContext, loggerConfig) =>
    {
        loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
    });
    /*  By default details is shown only in the Development environment */
    builder.Services.AddProblemDetails();
    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.All;
        options.RequestBodyLogLimit = 1024 * 8;
        options.ResponseBodyLogLimit = 1024 * 8;
    });
}   

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    // Create search index if it does not exist yet
    new RedisAnnouncementSearchSetup(
        app.Services.GetService<IConnectionMultiplexer>()!,
        app.Services.GetService<ILogger<RedisAnnouncementSearchSetup>>()!)
        .Setup();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}