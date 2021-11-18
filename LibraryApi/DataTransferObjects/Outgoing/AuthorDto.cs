using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Outgoing
{
    public class AuthorDto
    {
        public long AuthorId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }

    public class AuthorDtoProfile : Profile
    {
        public AuthorDtoProfile() 
        {
             CreateMap<Models.Author, AuthorDto>()
                .ForMember(x => x.AuthorId, o => o.MapFrom(f => f.Id))
                .ForMember(authorDto => authorDto.FirstName, x => x.MapFrom(author => author.Person.FirstName))
                .ForMember(authorDto => authorDto.LastName, x => x.MapFrom(author => author.Person.LastName));
        }

       
    }
}
