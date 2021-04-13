using AutoMapper;
using ContactLogger.Data;
using ContactLogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactLogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;
        // private readonly LinkGenerator _linkGenerator;


        //, LinkGenerator linkGenerator
        public StudentsController(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
           // _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<StudentModel[]>> Get(bool includeContacts = false)
        {
            try
            {
                var results = await _repository.GetAllStudentsAsync(includeContacts);

                return _mapper.Map<StudentModel[]>(results);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }
    }
}
