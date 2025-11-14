using App.DTOs;
using App.Features.Commands.CreateContact;
using App.Features.Commands.DeleteContact;
using App.Features.Commands.UpdateContact;
using App.Features.Queries.GetAllContacts;
using App.Features.Queries.GetContactById;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactDto contact)
        {
            var result = await _mediator.Send(new CreateContactCommand(contact));
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var query = new GetAllContactsQuery();
            var contacts = await _mediator.Send(query, ct);
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var query = new GetContactByIdQuery(id);
            var contact = await _mediator.Send(query);
            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteContactCommand(id));
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ContactDto command, CancellationToken ct)
        {
            await _mediator.Send(new UpdateContactCommand(command), ct);
            return NoContent();
        }
    }
}