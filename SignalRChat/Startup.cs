using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SignalRChat.Hubs;


namespace SignalRChat
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;


            });//.AddMessagePackProtocol();
            //services.AddSignalR()

            //services.AddSingleton<StockTicker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //loggerFactory.AddConsole(LogLevel.Debug);
            //loggerFactory.AddDebug(LogLevel.Debug);

            app.UseWebSockets();

            //app.MapWebSocketManager("/ws", serviceProvider.GetService<ChatMessageHandler>());
            //app.MapWebSocketManager("/test", serviceProvider.GetService<TestMessageHandler>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //var websockets = new WebSocketOptions
            //{
            //    KeepAliveInterval = TimeSpan.FromSeconds(20),
            //    ReceiveBufferSize = 6*1024
            //};

            //app.UseWebSockets(websockets);

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/ws")
            //    {
            //        if (context.WebSockets.IsWebSocketRequest)
            //        {
            //            var socket = await context.WebSockets.AcceptWebSocketAsync();
            //            await Echo(/*context,*/ socket);
            //        }
            //        else
            //        {
            //            context.Response.StatusCode = 400;
            //        }
            //    }
            //    else
            //    {
            //        await next();
            //    }

            //});

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseFileServer();
            app.UseCookiePolicy();

            /////working part using SignalR///////
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub", options =>
                {
                    options.ApplicationMaxBufferSize = 100000;
                    options.TransportMaxBufferSize = 100000;



                });
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.useme
        }

        #region Echo
        private async Task Echo(/*HttpContext context, */WebSocket webSocket)
        {
            //System.Diagnostics.Debug.WriteLine("client connected !");
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            //while (!result.CloseStatus.HasValue)
            //{
            //    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            //}
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        #endregion
    }
}
