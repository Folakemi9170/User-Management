using AutoMapper;
using UserManagement.Application.DTO.DepartmentDto;
using UserManagement.Application.DTO.EmployeeDto;
using UserManagement.Application.DTO.JobTitleDto;
using UserManagement.Application.DTO.PermissionDto;
using UserManagement.Application.DTO.ProductDto;
using UserManagement.Application.DTO.Roles;
using UserManagement.Application.DTO.UserDto;
using UserManagement.Application.DTO.UserRoleDto;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Entities.Product;

namespace UserManagement
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain → DTO
            CreateMap<Employee, EmployeeResponse>()
                .ForMember(dest => dest.Fullname,
                           opt => opt.MapFrom(src => $"{src.Firstname} {src.Middlename} {src.Lastname}"))
                //.ForMember(dest => dest.Email,
                //            opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.DeptName, opt => opt.MapFrom(src => src.Department.DeptName))
                .ForMember(dest => dest.JobTitleName, opt => opt.MapFrom(src => src.JobTitle.Name))
                .ReverseMap();

            CreateMap<CreateEmployee, Employee>()
                .ForMember(dest => dest.Email,
                            opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<Department, ResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DeptName, opt => opt.MapFrom(src => src.DeptName));
            CreateMap<CreateDeptDto, Department>();
            CreateMap<CreateEmployee, Employee>();
            //CreateMap<UpdateEmployeeDto, Employee>();

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<User, UserLoginResponse>()
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Employee.Role.Permissions));
            CreateMap<User, CreateUserResponse>();
           
            CreateMap<CreateRoleDto, Roles>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Roles, RoleResponseDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
            CreateMap<Roles, PermissionRoleResponse>()
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions))
             .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
            .ForMember(dest => dest.RoleDescription, opt => opt.MapFrom(src => src.Description));


            CreateMap<JobTitle, JobTitleResponse>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DeptName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
            CreateMap<CreateJobTitle, JobTitle>();

            CreateMap<CreateProduct, Products>();
            CreateMap<UpdateProductDto, Products>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember, context) => {
                        if (srcMember == null)
                            return false;

                        if (srcMember is int intValue && intValue == 0)
                            return false;

                        return true;}));
            CreateMap<Products, ProductResponseDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DeptName))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Firstname + " " + src.CreatedBy.Lastname))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssignedTo.Firstname + " " + src.AssignedTo.Lastname))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Email))
                .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.ProductCategory.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.ProductStatus, opt => opt.MapFrom(src => src.ProductStatus.Name));

            CreateMap<Permission, PermissionResponse>();
            CreateMap<CreatePermissionDto, Permission>();
        }
    }
}
