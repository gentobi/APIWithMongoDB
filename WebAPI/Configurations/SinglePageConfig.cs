using Microsoft.AspNetCore.Builder;
using System.IO;

namespace WebAPI.Configurations
{
	public static class SinglePageConfig
	{
		public static void UseSinglePage(this IApplicationBuilder app)
		{
			//
			// Should not re-order codes
			app.Use(async (context, next) =>
			{
				await next();

				// If there's no available file and the request doesn't contain an extension, we're probably trying to access a page.
				// Rewrite request to use app root
				if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value) && !context.Request.Path.Value.StartsWith("/api"))
				{
					context.Request.Path = "/index.html";
					context.Response.StatusCode = 200; // Make sure we update the status code, otherwise it returns 404
					await next();
				}
			});
			//
			// Use static files
			app.UseDefaultFiles();
			app.UseStaticFiles();
		}
	}
}
