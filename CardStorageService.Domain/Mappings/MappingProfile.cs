using AutoMapper;

namespace CardStorageService.Domain
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountToCreate>();
            CreateMap<Account, AccountToUpdate>();
            CreateMap<Account, AccountResponse>();
            CreateMap<AccountSession, AccountSessionToCreate>();
            CreateMap<AccountSession, AccountSessionResponse>();
            CreateMap<Card, CardToCreate>();
            CreateMap<Card, CardToUpdate>();
            CreateMap<Card, CardResponse>();
            CreateMap<Client, ClientToCreate>();
            CreateMap<Client, ClientToUpdate>();
            CreateMap<Client, ClientResponse>();
        }
    }
}
