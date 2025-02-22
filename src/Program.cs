using src.Repository.Implmentations;
using src.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITelex, TelexRepository>();

builder.Services.AddCors(
    options => options.AddPolicy("AllowAll", builder =>{
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    })
);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();


app.MapControllers();
app.UseCors("AllowAll");
app.UseHttpsRedirection();


app.Run();