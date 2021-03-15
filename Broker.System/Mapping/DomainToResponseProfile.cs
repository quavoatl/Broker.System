using AutoMapper;
using Broker.System.Controllers.V1.Responses;
using Broker.System.Domain;

namespace Broker.System.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Limit, LimitResponse>();
        }
    }
}