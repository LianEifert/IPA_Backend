using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectEstimaterBackend.Models.Data;
using ProjectEstimaterBackend.Profiles;
using ProjectEstimaterBackend.Services;



var host = new HostBuilder()
        .ConfigureFunctionsWebApplication(builder =>
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("IocAppOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        })
        .ConfigureServices((services) =>
        {
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AddParticipantViewModelProfile>();
                cfg.AddProfile<UpdateParticipantViewModelProfile>();
                cfg.AddProfile<AddVotingViewModelProfile>();
                cfg.AddProfile<UpdateVotingViewModelProfile>();
                cfg.AddProfile<VotingViewModelProfile>();
                cfg.AddProfile<ParticipantViewmodelProfile>();

            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSingleton<IDataService<Participant>, ParticipantDataService>();
            services.AddSingleton<IDataService<Voting>, VotingDataService>();
            services.AddSingleton<IVotingDataService, VotingDataService>();
            services.AddSingleton<IVotingParticipantService, VotingDataService>();
        })
    .Build();

host.Run();
