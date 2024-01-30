internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // this will serve wwwroot/index.html when path is '/'
        app.UseDefaultFiles();

        // this will serve js, css, images etc.
        app.UseStaticFiles();

        // this ensures index.html is served for any requests with a path
        // and prevents a 404 when the user refreshes the browser
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value != "/")
            {
                context.Response.ContentType = "text/html";

                await context.Response.SendFileAsync(
                    builder.Environment.ContentRootFileProvider.GetFileInfo("wwwroot/browser/index.html")
                );

                return;
            }

            await next();
        });
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}