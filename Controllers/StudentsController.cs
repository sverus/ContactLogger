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
        private readonly LinkGenerator _linkGenerator;


        
        public StudentsController(IContactRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
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

        [HttpGet("{moniker}")]
        public async Task<ActionResult<StudentModel>> Get(string moniker)
        {
            try
            {
                var result = await _repository.GetStudentAsync(moniker);
                if (result == null) return NotFound();

                return _mapper.Map<StudentModel>(result);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

        public async Task<ActionResult<StudentModel>> Post(StudentModel model)
        {
            try
            {
                var existing = await _repository.GetStudentAsync(model.Moniker);
                if (existing != null)
                {
                    return BadRequest("Moniker in use.");
                }
                var location = _linkGenerator.GetPathByAction("Get", "Students", new { moniker = model.Moniker });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use moniker.");
                }
                var student = _mapper.Map<Student>(model);
                _repository.Add(student);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/students/{student.Moniker}", _mapper.Map<StudentModel>(student));
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            return BadRequest();
        }

        [HttpPut("{moniker}")]
        public async Task<ActionResult<StudentModel>> Put(string moniker, StudentModel model)
        {
            try
            {
                var oldStudent = await _repository.GetStudentAsync(moniker);
                if (oldStudent == null)
                {
                    return NotFound($"Student does not exist with moniker of {moniker}.");
                }
                _mapper.Map(model, oldStudent);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<StudentModel>(oldStudent);
                }
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            return BadRequest();
        }
        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var student = await _repository.GetStudentAsync(moniker);
                if (student == null)
                {
                    return NotFound($"Student does not exist with moniker of {moniker}.");
                }
                _repository.Delete(student);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            return BadRequest("Failed to delete the student.");
        }
    }
}
