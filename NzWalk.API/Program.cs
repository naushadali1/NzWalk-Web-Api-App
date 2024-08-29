using Microsoft.EntityFrameworkCore;
using NzWalk.API.Data;
using NzWalk.API.Repositories;

var builder = WebApplication.CreateBuilder(args);


var NzWalkConString = builder.Configuration.GetConnectionString("NzWalkConString");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NZWalkDbContext>(options => options.UseSqlServer(NzWalkConString));
builder.Services.AddScoped<IRegionRepository, SqlRegionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
