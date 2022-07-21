using Microsoft.EntityFrameworkCore;
using FeeApi.Models;

// var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost", "http://localhost:8081", "https://localhost:8001")
                        //   builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                      });
});
// Add services to the container.

builder.Services.AddControllers();
//InMemory (Uses RAM as database)
// builder.Services.AddDbContext<FeeContext>(opt =>
    // opt.UseInMemoryDatabase("FeeList"));

// sqlite
// var connectionString = builder.Configuration.GetConnectionString("sqlite") ?? "Data Source=sqlite.db";
// builder.Services.AddSqlite<FeeContext>(connectionString);

//postgresql
var connectionString = builder.Configuration["dbContextSettings:ConnectionString"];
builder.Services.AddDbContext<FeeContext>(options =>
                options.UseNpgsql(connectionString));

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "TodoApi", Version = "v1" });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi v1"));
}

// app.UseHttpsRedirection();

app.UseCors("MyAllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();


//###########################################################################################
// postgreSQL - check and wait until database is ready for connections
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<FeeContext>();
    await waitForDb(services, context);
}

static async Task waitForDb(IServiceProvider services, FeeContext context) {
    // create your own connection checker here
    // see https://stackoverflow.com/questions/19211082/testing-an-entity-framework-database-connection
    var maxAttemps = 12;
    var delay = 5000;

    for (int i = 0; i < maxAttemps; i++) {
        if (context.Database.CanConnect()) {
            if (context.Database.GetPendingMigrations().Any()) {
                context.Database.Migrate();
            }
            return;
        }
        await Task.Delay(delay);
    }
}
//###########################################################################################

app.Run();