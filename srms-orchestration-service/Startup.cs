﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using srms_orchestration_service.Client;
using srms_orchestration_service.Client.AttachmentsService;
using srms_orchestration_service.Client.ContactsService;
using srms_orchestration_service.Client.EventsService;
using srms_orchestration_service.Config;
using srms_orchestration_service.Middleware;
using srms_orchestration_service.Services;
using srms_orchestration_service.Services.Impl;
using srms_orchestration_service.Services.Interfaces;

namespace srms_orchestration_service
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Relationships Management System Orchestration service", Version = "v1" });
                c.OperationFilter<SwaggerHeaderConfig>();
            });

            ConfigureClientsParameters(services);
            services.AddTransient<RestClient>();
            services.AddTransient<ContactsServiceClient>();
            services.AddTransient<EventsServiceClient>();
            services.AddTransient<AttachmentServiceClient>();
            services.AddSingleton<IContactsService, ContactsServiceImpl>();
            services.AddSingleton<IAttachmentService, AttachmentServiceImpl>();
            services.AddSingleton<IFactsService, FactsServiceImpl>();
            services.AddSingleton<IEventsService, EventsServiceImpl>();
            services.AddSingleton<JwtServiceImpl>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SRMS Orchestration Service v1"));
            }

            app.UseRouting();

            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureClientsParameters(IServiceCollection services)
        {
            services.Configure<ContactsServiceClientConfig>(Configuration.GetSection("Services:SRMS.Contacts.Service"));
            services.Configure<EventsServiceClientConfig>(Configuration.GetSection("Services:SRMS.Events.Service"));
            services.Configure<AttachmentsServiceClientConfig>(Configuration.GetSection("Services:SRMS.Attachments.Service"));
        }
    }
}
