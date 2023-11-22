using AutoMapper;

namespace IWent.Api.Mapping;

public class EventMappingProfile : Profile
{
    public EventMappingProfile()
    {
        CreateMap<Persistence.Models.Venue, Models.Venue>();
        CreateMap<Persistence.Models.Section, Models.Section>();
        CreateMap<Persistence.Models.Row, Models.Row>();
        CreateMap<Persistence.Models.Seat, Models.Seat>();
    }
}
