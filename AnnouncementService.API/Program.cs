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
    builder.Services.AddScoped<IBookRepo, RedisBookRepo>();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}