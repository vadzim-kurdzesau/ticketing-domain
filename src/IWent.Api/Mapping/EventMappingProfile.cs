using AutoMapper;
using IWent.Persistence.Entities;

namespace IWent.Api.Mapping;

public class EventMappingProfile : Profile
{
    public EventMappingProfile()
    {
        CreateMap<Venue, Models.Venue>();
        CreateMap<Section, Models.Section>();
        CreateMap<Row, Models.Row>();
        CreateMap<Seat, Models.Seat>();
    }
}
