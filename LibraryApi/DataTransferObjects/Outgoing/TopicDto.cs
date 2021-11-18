using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Outgoing
{
    public class TopicDto
    {
        public string Description { get; set; }
    }

    public class TopicDtoProfile : Profile
    {
        public TopicDtoProfile()
        {
            CreateMap<Models.Topic, TopicDto>().ReverseMap();
        }
        
    }

}
