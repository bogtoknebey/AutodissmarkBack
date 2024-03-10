using Autodissmark.ExternalServices.TextToSpeach.Contracts;
using Autodissmark.ExternalServices.Translate.Contracts;
using Autodissmark.ExternalServices.Translate.GoogleTranslate;
using Autodissmark.ExternalServices.WebDriverTaskBuilder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IWebDriverTaskBuilder, WebDriverTaskBuilder>();
builder.Services.AddScoped<ITranslate, Translator>();
builder.Services.AddScoped<ITextToSpeach, Autodissmark.ExternalServices.TextToSpeach.Voxworker.TextToSpeach>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
