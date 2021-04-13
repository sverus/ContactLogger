using AutoMapper;
using ContactLogger.Data.Entities;
using ContactLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactLogger.Data
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            this.CreateMap<Student, StudentModel>();
        }
    }
}
