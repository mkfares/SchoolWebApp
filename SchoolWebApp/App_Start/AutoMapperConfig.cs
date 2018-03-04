using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;

namespace SchoolWebApp.App_Start
{
    /// <summary>
    /// Configuration of AutoMapper class
    /// Add all the required mappings between model and view models here
    /// </summary>
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeViewModel>().ReverseMap();
                cfg.CreateMap<Faculty, FacultyViewModel>().ReverseMap();
            });
        }
    }
}