using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Flight, Flight>();
            CreateMap<UpsertFlightDto, Flight>();
            CreateMap<Configuration, Configuration>();
            CreateMap<AppUserDto, PermissionUser>()
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PermissionUser, AppUserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AppUser.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.AppUser.PhoneNumber));
            CreateMap<AppUser, AppUserDto>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<AppUserDto, AppUser>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));
            CreateMap<PermissionDto, Permission>()
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator.Id))
                .ForMember(dest => dest.Creator, opt => opt.Ignore())
                .ForMember(dest => dest.PermissionUsers, opt => opt.Ignore());
            CreateMap<UpdatePermissionDto, Permission>();
            CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.AppUserDtos, opt => opt.MapFrom(src => src.PermissionUsers));
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ?
                DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}
