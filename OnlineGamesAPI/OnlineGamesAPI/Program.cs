using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineGamesAPI.Data;
using OnlineGamesAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDatabase>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
builder.Services.AddControllers();

var app = builder.Build();
using (var scope =
  app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<AppDatabase>())
    await context!.Database.EnsureCreatedAsync();

// Configure the HTTP request pipeline.
app.UseCors();

app.UseWebSockets();

app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

AppOptions ao = new AppOptions();
ao.Credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile("serviceAccountKey.json");
FirebaseApp.Create(ao);
app.Use(async (ctx, next) => {
    try {
        string token = "";
        // If request is a websocket and points to one of our websocket endpoints
        if (ctx.Request.HttpContext.WebSockets.IsWebSocketRequest && ctx.Request.Path.ToString().Contains("/ws")) {
            // Set the token string using the querystring, since websocket requests cant be given headers
            token = ctx.Request.Query["auth"];
        } else if (ctx.Request.Headers.ContainsKey("auth")) { // Since it isn't a websocket, check to make sure it contains an auth key in the request header
            // TODO, delete this. Only used for testing requests with postman.
            if (ctx.Request.Headers["auth"].Equals("dev")) {
                ctx.Request.Headers.Add("user", "{\"Uid\": \"dev\"}");
                await next(ctx);
                return;
            } else {
                // Set the token to the value found in the request header
                token = ctx.Request.Headers["auth"];
            }
        }
        // Use the token to verify the request's user
        FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
        if (decodedToken != null) {
            // Append the user's data to the request's header
            ctx.Request.Headers.Add("user", JsonConvert.SerializeObject(decodedToken));
            await next(ctx);
        } else {
            // Verification failed, user isn't authorized to continue
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    } catch (FirebaseAuthException ex) {
        Console.WriteLine(ex);
        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
    } catch (Exception ex) {
        Console.WriteLine(ex.Message);
        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }
});

app.Run();
