using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolWebApp.App_Start
{
    /// <summary>
    /// Configuration of AutoMapper class
    /// Add all the mappings here
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