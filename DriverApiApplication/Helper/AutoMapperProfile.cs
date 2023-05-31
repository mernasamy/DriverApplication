namespace DriverApiApplication.Helper;

using AutoMapper;
using DriverApiApplication.Models;
using DriverApiApplication.Models.Dto;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CreateDriver -> Driver
        CreateMap<CreateDriver, Driver>();

        // UpdateDriver -> Driver
        CreateMap<UpdateDriver, Driver>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    return true;
                }
            ));
    }
}