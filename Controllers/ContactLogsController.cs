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

    [ApiController]
    [Route("api/students/{moniker}/contactlogs")]
    public class ContactLogsController : ControllerBase
    {

        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ContactLogsController(IContactRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<ContactLogModel[]>> Get(string moniker)
        {
            try
            {
                var talks = await _repository.GetContactsByMonikerAsync(moniker);
                return _mapper.Map<ContactLogModel[]>(talks);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ContactLogModel>> Get(string moniker, int id)
        {
            try
            {
                var contact = await _repository.GetContactByMonikerAsync(moniker, id);
                if (contact == null) return NotFound("Contact not found.");

                return _mapper.Map<ContactLogModel>(contact);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ContactLogModel>> Post(ContactLogModel model, string moniker)
        {
            try
            {
                var student = await _repository.GetStudentAsync(moniker);
                if (student == null) return BadRequest("Student not found.");

                var contact = _mapper.Map<ContactLog>(model);
                contact.Student = student;

                

                _repository.Add(contact);

                if (await _repository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction(HttpContext, "Get", values:
                        new { moniker, id = contact.Id });


                    return Created(url, _mapper.Map<ContactLogModel>(contact));
                }
                else
                {
                    return BadRequest("Failed to save new contact");
                }
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ContactLogModel>> Put(string moniker, int id, ContactLogModel model)
        {
            try
            {
                var contact = await _repository.GetContactByMonikerAsync(moniker, id);
                if (contact == null) return NotFound("Could not find contact");



                _mapper.Map(model, contact);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<ContactLogModel>(contact);
                }
                else
                {
                    return BadRequest("Failed to update database.");
                }
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(string moniker, int id)
        {
            try
            {
                var talk = await _repository.GetContactByMonikerAsync(moniker, id);
                if (talk == null) return NotFound("Could not find contact");

                _repository.Delete(talk);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }

                else
                {
                    return BadRequest("Failed to delete the contact.");
                }
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }
    }

}
