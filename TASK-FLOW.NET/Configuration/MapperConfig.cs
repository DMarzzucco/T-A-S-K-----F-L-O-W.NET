using AutoMapper;
using TASK_FLOW.NET.Mapper;

namespace TASK_FLOW.NET.Configuration
{
    public static class MapperConfig
    {
        public static IServiceCollection AddMapperConfig(this IServiceCollection services)
        {
            var mappConfig = new MapperConfiguration(csf =>
            {
                csf.AddProfile<MappingProfile>();
            });
            IMapper mapper = mappConfig.CreateMapper();

            services.AddSingleton(mapper);
            return services;
        }
    }
}
