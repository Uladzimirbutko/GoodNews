using AutoMapper;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<News, NewsDto>().ReverseMap();

            CreateMap<RssSource, RssSourceDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Role, RoleDto>().ReverseMap();

        }

    }
}