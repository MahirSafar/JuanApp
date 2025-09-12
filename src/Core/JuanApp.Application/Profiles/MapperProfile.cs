using AutoMapper;
using JuanApp.Application.Models;
using JuanApp.Domain.Models;

namespace JuanApp.Application.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Slider, SliderDto>().ReverseMap();
            CreateMap<CreateSliderDto, Slider>().ReverseMap();
        }
    }
}
