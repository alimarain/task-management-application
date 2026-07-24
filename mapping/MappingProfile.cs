using AutoMapper;
using TaskManagementApi.DTOs;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Models.Entities;

namespace TaskManagementApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, AuthResponseDto>();

        // Project
        CreateMap<Project, ProjectDto>();
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();

        // Task
        CreateMap<TaskItem, TaskDto>();
        CreateMap<CreateTaskDto, TaskItem>();
        CreateMap<UpdateTaskDto, TaskItem>();
    }
}