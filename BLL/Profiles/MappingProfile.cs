using AutoMapper;
using BLL.ViewModels;
using BLL.ViewModels.SessionsViewModel;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, options => options.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName, options => options.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.AvailableSlots, options => options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel, Session>()
                .ReverseMap();

            CreateMap<Trainer, TrainerAndCategorySelectViewModel>();
            CreateMap<Category, TrainerAndCategorySelectViewModel>()
                .ForMember(dest => dest.Name, Option => Option.MapFrom(src => src.CategoryName));

            CreateMap<MemberShip, MemberShipViewModel>()
                .ForMember(dest => dest.MemberId, options => options.MapFrom(src => src.Member.Id))
                .ForMember(dest => dest.PlanId, options => options.MapFrom(src => src.Plan.Id))
                .ForMember(dest => dest.Member, options => options.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.Plan, options => options.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.StartDate, options => options.MapFrom(src => src.CreatedAt.ToShortDateString()))
                .ForMember(dest => dest.EndDate, options => options.MapFrom(src => src.EndDate.ToShortDateString()))
                .ForMember(dest => dest.Status, options => options.MapFrom(src => src.Status));

            CreateMap<Member, MembersAndPlansDropListViewModel>();
            CreateMap<Plan, MembersAndPlansDropListViewModel>();

            //CreateMap<Member, MembersViewModel>()
            //    .ForMember(dest => dest.BookingDate, options => options.MapFrom(src => src.MemberSessions.Select(ms => ms.CreatedAt.ToShortDateString())));

            CreateMap<Member, GetMembersToNewBooking>();
            //CreateMap<Session, MembersViewModel>()
            //    .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Description));

            //CreateMap<MemberSession, SessionScheduleViewModel>()
            //    .ForMember(dest => dest.MemberName, Options => Options.MapFrom(src => src.Member.Name))
            //    .ForMember(dest => dest.BookingDate, Options => Options.MapFrom(src => src.CreatedAt))
            //    .ForMember(dest => dest.SessionStartDate, Options => Options.MapFrom(src => src.Session.StartDate))
            //    .ForMember(dest => dest.SessionEndDate, Options => Options.MapFrom(src => src.Session.EndDate));
        }
    }
}
