using AutoMapper;

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
            this.CreateMap<Student, StudentModel>()
                .ReverseMap();
            //.ForMember(t => t.Contacts, opt => opt.Ignore());
            this.CreateMap<ContactLog, ContactLogModel>()
                .ReverseMap()
             .ForMember(t => t.Student, opt => opt.Ignore());
            
        }
    }
}
