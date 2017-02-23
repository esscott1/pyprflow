using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using workflow.Model;

namespace workflow.Middleware
{
	public class UserKeyValidatorsMiddleware
	{
		private readonly RequestDelegate _next;
		private IWorkflowRepository WorkflowRepo { get; set; }

		public UserKeyValidatorsMiddleware(RequestDelegate next, IWorkflowRepository _repo)
		{
			_next = next;
			WorkflowRepo = _repo;
		}

		public async Task Invoke(HttpContext context)
		{
			if (!context.Request.Headers.Keys.Contains("user-key"))
			{
				context.Response.StatusCode = 400; //Bad Request                
				await context.Response.WriteAsync("User Key is missing");
				return;
			}
			else
			{
				if (!WorkflowRepo.CheckValidUserKey(context.Request.Headers["user-key"]))
				{
					context.Response.StatusCode = 401; //UnAuthorized
					await context.Response.WriteAsync("Invalid User Key");
					return;
				}
			}

			await _next.Invoke(context);
		}

	}

	#region ExtensionMethod
	public static class UserKeyValidatorsExtension
	{
		public static IApplicationBuilder ApplyUserKeyValidation(this IApplicationBuilder app)
		{
			app.UseMiddleware<UserKeyValidatorsMiddleware>();
			return app;
		}
	}
	#endregion
}
