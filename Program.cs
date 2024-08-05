using OrchardCore.Logging;
using ToDoModule.Services.Interfaces;
using ToDoModule.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLogHost();

//builder.Services
//    .AddOrchardCms()

    builder.Services
    .AddOrchardCms()
    .AddSetupFeatures("OrchardCore.AutoSetup");
// // Orchard Specific Pipeline
// .ConfigureServices( services => {
// })
// .Configure( (app, routes, services) => {
// })
;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var storageAccountConnectionString = builder.Configuration.GetConnectionString("AzureStorageAccount");
var tableName = builder.Configuration["TableName"];
builder.Services.AddSingleton<IToDoItemService>(new ToDoItemService(storageAccountConnectionString, tableName));

builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.UseOrchardCore();

app.Run();
