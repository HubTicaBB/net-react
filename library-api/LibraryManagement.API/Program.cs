using LibraryManagement.Application;
using LibraryManagement.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Library Management API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed database
using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<LibraryManagement.Infrastructure.Data.ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        context.Database.EnsureCreated();

        // Create roles
        if (!await roleManager.RoleExistsAsync("Librarian"))
        {
            await roleManager.CreateAsync(new IdentityRole("Librarian"));
        }
        if (!await roleManager.RoleExistsAsync("Member"))
        {
            await roleManager.CreateAsync(new IdentityRole("Member"));
        }

        // Create default librarian user
        if (await userManager.FindByEmailAsync("librarian@library.com") == null)
        {
            var librarian = new IdentityUser
            {
                UserName = "librarian@library.com",
                Email = "librarian@library.com"
            };
            await userManager.CreateAsync(librarian, "Librarian123!");
            await userManager.AddToRoleAsync(librarian, "Librarian");
        }

        // Seed books
        if (!context.Books.Any())
        {
            var books = new[]
            {
                new LibraryManagement.Domain.Entities.Book
                {
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Isbn = "978-0743273565",
                    PublishedYear = 1925,
                    AvailableCopies = 5
                },
                new LibraryManagement.Domain.Entities.Book
                {
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Isbn = "978-0061120084",
                    PublishedYear = 1960,
                    AvailableCopies = 3
                },
                new LibraryManagement.Domain.Entities.Book
                {
                    Title = "1984",
                    Author = "George Orwell",
                    Isbn = "978-0451524935",
                    PublishedYear = 1949,
                    AvailableCopies = 4
                },
                new LibraryManagement.Domain.Entities.Book
                {
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Isbn = "978-0141439518",
                    PublishedYear = 1813,
                    AvailableCopies = 6
                },
                new LibraryManagement.Domain.Entities.Book
                {
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    Isbn = "978-0316769174",
                    PublishedYear = 1951,
                    AvailableCopies = 2
                }
            };

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
