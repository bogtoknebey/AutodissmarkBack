using Autodissmark.Domain.Options;
using Autodissmark.Application.Author;
using Autodissmark.Application.ManualUpload;
using Autodissmark.Application.Text;
using Autodissmark.ApplicationDataAccess;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.ExternalServices.TextToSpeach.Contracts;
using Autodissmark.ExternalServices.Translate.Contracts;
using Autodissmark.ExternalServices.Translate.GoogleTranslate;
using Autodissmark.ExternalServices.WebDriverTaskBuilder;
using Autodissmark.TextProcessor.ManuallyUpload;
using Autodissmark.TextProcessor.TextProcessor;
using Autodissmark.TextProcessorDataAccess;
using Autodissmark.TextProcessorDataAccess.Repositories.ReadRepositories;
using Autodissmark.TextProcessorDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.TextProcessorDataAccess.Repositories.WriteRepositories;
using Autodissmark.TextProcessorDataAccess.Repositories.WriteRepositories.Contracts;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Autodissmark.Core.FileService.Contracts;
using Autodissmark.Core.FileService;
using Autodissmark.Application.Voiceover.CommonVoiceover;
using Autodissmark.Application.Voiceover.ManualVoiceover;
using Autodissmark.Application.Voiceover.AutoVoiceover;
using Autodissmark.Application.Syntax;
using Autodissmark.AudioMixer.Mixer;
using Autodissmark.Application.Diss;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Autodissmark.API.Services.JWTBuilder;
using Autodissmark.Application.Login;
using Autodissmark.Core.Constants;
using Autodissmark.Domain.Enums;

var builder = WebApplication.CreateBuilder(args);

#region DI
builder.Services.AddScoped<IWebDriverTaskBuilder, WebDriverTaskBuilder>();
builder.Services.AddScoped<ITranslate, Translator>();
builder.Services.AddScoped<ITextToSpeach, Autodissmark.ExternalServices.TextToSpeach.Voxworker.TextToSpeach>();
builder.Services.AddScoped<IAuthorReadRepository, AuthorReadRepository>();
builder.Services.AddScoped<IAuthorWriteRepository, AuthorWriteRepository>();
builder.Services.AddScoped<IAuthorLogic, AuthorLogic>();
builder.Services.AddScoped<ITextProcessorLogic, TextProcessorLogic>();
builder.Services.AddScoped<IDictionaryReadRepository, DictionaryReadRepository>();
builder.Services.AddScoped<IDictionaryWordWriteRepository, DictionaryWordWriteRepository>();
builder.Services.AddScoped<IDictionaryWriteRepository, DictionaryWriteRepository>();
builder.Services.AddScoped<ITextProcessorManuallyUploadLogic, TextProcessorManuallyUploadLogic>();
builder.Services.AddScoped<IBeatWriteRepository, BeatWriteRepository>();
builder.Services.AddScoped<IApplicationManualUploadLogic, ApplicationManualUploadLogic>();
builder.Services.AddScoped<IDictionaryWordReadRepository, DictionaryWordReadRepository>();
builder.Services.AddScoped<ITextLogic, TextLogic>();
builder.Services.AddScoped<ITextReadRepository, TextReadRepository>();
builder.Services.AddScoped<ITextWriteRepository, TextWriteRepository>();
builder.Services.AddScoped<IAcapellaReadRepository, AcapellaReadRepository>();
builder.Services.AddScoped<IAcapellaWriteRepository, AcapellaWriteRepository>();
builder.Services.AddScoped<IManualVoiceoverLogic, ManualVoiceoverLogic>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICommonVoiceoverLogic, CommonVoiceoverLogic>();
builder.Services.AddScoped<IVoiceReadRepository, VoiceReadRepository>();
builder.Services.AddScoped<IAutoVoiceoverLogic, AutoVoiceoverLogic>();
builder.Services.AddScoped<ISyntaxLogic, SyntaxLogic>();
builder.Services.AddScoped<IMixer, Mixer>();
builder.Services.AddScoped<IDissLogic, DissLogic>();
builder.Services.AddScoped<IBeatReadRepository, BeatReadRepository>();
builder.Services.AddScoped<IDissReadRepository, DissReadRepository>();
builder.Services.AddScoped<IDissWriteRepository, DissWriteRepository>();
builder.Services.AddScoped<ILoginLogic, LoginLogic>();
#endregion

#region MsSql configuration
string applicationConnectionString = builder.Configuration.GetConnectionString("Application");
builder.Services.AddDbContext<ApplicationDataContext>(options =>
{
    options.UseSqlServer(applicationConnectionString);
});

string textProcessorConnectionString = builder.Configuration.GetConnectionString("TextProcessor");
builder.Services.AddDbContext<TextProcessorDataContext>(options =>
{
    options.UseSqlServer(textProcessorConnectionString);
});
#endregion

#region Mapping configuration
var profileAssemblies = new[]
{
    Assembly.GetAssembly(typeof(Autodissmark.API.MappingProfile.MappingProfile)),
    Assembly.GetAssembly(typeof(Autodissmark.Application.MappingProfile.MappingProfile)),
    Assembly.GetAssembly(typeof(Autodissmark.ApplicationDataAccess.MappingProfile.MappingProfile)),
    Assembly.GetAssembly(typeof(Autodissmark.TextProcessorDataAccess.MappingProfile.MappingProfile))
};
builder.Services.AddAutoMapper(config => config.AddExpressionMapping(), profileAssemblies);
#endregion


builder.Services.Configure<FilePathOptions>(builder.Configuration.GetSection("DataDirectories"));
builder.Services.Configure<FFmpegOptions>(builder.Configuration.GetSection("FFmpeg"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    options.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
});

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
            },
            new string[] {}
        }
    });
});
#endregion

#region Jwt
var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
builder.Services.Configure<JwtOptions>(jwtSection);

var jwtOptions = jwtSection.Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = jwtOptions.ValidateIssuer,
            ValidateAudience = jwtOptions.ValidateAudience,
            ValidateLifetime = jwtOptions.ValidateLifetime,
            ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            RoleClaimType = "Role"
        };
    });

builder.Services.AddScoped<IJwtTokenBuilder>(provider =>
{
    return new JwtTokenBuilder(jwtOptions.Issuer, jwtOptions.Audience, jwtOptions.Key);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Role.User.ToString(), policy => policy.RequireRole(Role.User.ToString()));
    options.AddPolicy(Role.Admin.ToString(), policy => policy.RequireRole(Role.Admin.ToString()));
});
#endregion

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
