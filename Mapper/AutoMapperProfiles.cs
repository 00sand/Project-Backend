using AutoMapper;
using System;
using TaskManagerAPI.Models.Domain;
using TaskManagerAPI.Models.DTO;

namespace TaskManagerAPI.Mapper
{
    public class AutoMapperProfiles : Profile
    {
            public AutoMapperProfiles()
            {
                CreateMap<Project, ProjectDTO>().ReverseMap();
                CreateMap<AddProjectDTO, Project>().ReverseMap();
                CreateMap<UpdateProjectDTO, Project>().ReverseMap();
                CreateMap<AddTaskDTO, TaskModel>()
                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.Parse(src.UserId)))
                  .ForMember(dest => dest.AssignedUserId, opt => opt.MapFrom(src => Guid.Parse(src.AssignedUserId)))
                  .ForMember(dest => dest.UserName, opt => opt.Ignore()); // Explicitly ignoring UserName during DTO to Model mapping

            // Separate mapping for reverse conversion
                 CreateMap<TaskModel, AddTaskDTO>()
                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.ToString()))
                  .ForMember(dest => dest.AssignedUserId, opt => opt.MapFrom(src => src.AssignedUserId.ToString()));
                CreateMap<TaskModel, TaskDTO>();
                CreateMap<UpdateTaskDTO, TaskModel>().ReverseMap();
                CreateMap<StatusCategory, StatusCategoryDTO>().ReverseMap();
                CreateMap<UserDTO, User>().ReverseMap();
                CreateMap<AddUserDTO, User>().ReverseMap();
                CreateMap<UpdateUserDTO, User>().ReverseMap();

            }

    }
}
