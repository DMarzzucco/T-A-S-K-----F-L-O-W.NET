using TASK_FLOW.NET.Configuration;
using TASK_FLOW.NET.Configuration.Swagger;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddHttpContextAccessor();
//Cors Policy
builder.Services.AddCorsPolicy();
//Jwt Configurations
builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddJWTAuthentication(builder.Configuration);
//RegisterFilters
builder.Services.AddCustomController();
//Register Services
builder.Services.AddCustomServices();
// Swagger Configurations
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfigurations();
//Mapper
builder.Services.AddMapperConfig();
builder.Services.AddMvc();
//Port Listen
builder.WebHost.UseUrls("http://*:5024");

//app config
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
