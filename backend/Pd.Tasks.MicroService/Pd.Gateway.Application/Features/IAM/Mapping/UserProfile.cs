using AutoMapper;
using Pd.Tasks.Application.Features.UsersManagement.Models;
using Pd.Tasks.Application.Features.UsersManagement.Requests;

namespace Pd.Tasks.Application.Features.IAM.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserCommand, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.PasswordResetToken, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordResetTokenExpiry, opt => opt.Ignore());
        }
    }
}
