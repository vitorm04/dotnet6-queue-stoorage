using Azure.Storage.Queues;
using AzureStorageTest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config => 
{
    config.EnableAnnotations();
});
builder.Services.AddTransient<QueueClient>(x =>
{
    return new QueueClient(builder.Configuration.GetConnectionString("QueueStorage"), "notification");
});

builder.Services.AddHostedService<Worker>();
var app = builder.Build();


// App Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
