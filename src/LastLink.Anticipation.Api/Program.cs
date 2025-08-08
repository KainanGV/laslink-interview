using LastLink.Anticipation.API.Errors;
using LastLink.Anticipation.Application.Interfaces;
using LastLink.Anticipation.Application.Services;
using LastLink.Anticipation.Application.UseCases;
using LastLink.Anticipation.Domain.Interfaces;
using LastLink.Anticipation.Infra.Data;
using LastLink.Anticipation.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using IAnticipationSimulator = LastLink.Anticipation.Application.Interfaces.IAnticipationSimulator;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AnticipationDbContext>(options =>
    options.UseInMemoryDatabase("AnticipationDb"));

// Repositories
builder.Services.AddScoped<IAnticipationRequestRepository, AnticipationRequestRepository>();

// Services
builder.Services.AddScoped<IAnticipationSimulator, AnticipationSimulator>();

// UseCases (interfaces)
builder.Services.AddScoped<ICreateAnticipationRequestUseCase, CreateAnticipationRequestUseCase>();
builder.Services.AddScoped<IListAnticipationRequestsByCreatorUseCase, ListAnticipationRequestsByCreatorUseCase>();
builder.Services.AddScoped<IApproveAnticipationRequestUseCase, ApproveAnticipationRequestUseCase>();
builder.Services.AddScoped<IRejectAnticipationRequestUseCase, RejectAnticipationRequestUseCase>();
builder.Services.AddScoped<ISimulateAnticipationRequestUseCase, SimulateAnticipationRequestUseCase>();

// Controllers / Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// ⬇️ Middleware de erros (ProblemDetails)
app.UseApiExceptionHandling();

app.MapControllers();
app.Run();
