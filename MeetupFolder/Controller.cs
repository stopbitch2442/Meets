using Meets.WebApi.MeetupFolder;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace Meets.WebApi.Controller
{
    [ApiController]
    [Route("/meetups")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]

    public class MeetupController : ControllerBase
    {
      //  private static readonly ICollection<MeetupEntity> Meetups =
       //     new List<MeetupEntity>();

        private readonly DatabaseContext _context = new();


        /// <summary>Post meetup</summary>
        /// <response code="200">Put meetup.</response>
        /// <response code="404">Meetup with specified id was not found.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateMeetupDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateMeetup([FromBody] CreateMeetupDto createDto)
        {
            var newMeetup = new MeetupEntity
            {
                Id = Guid.NewGuid(),
                Topic = createDto.Topic,
                Place = createDto.Place,
                Duration = createDto.Duration
            };

            _context.Meetups.Add(newMeetup);
            await _context.SaveChangesAsync();

            var readDto = new ReadMeetupDto
            {
                Id = newMeetup.Id,
                Topic = newMeetup.Topic,
                Place = newMeetup.Place,
                Duration = newMeetup.Duration
            };
            return Ok(readDto);
        }

        /// <summary>Put meetup</summary>
        /// <param name="id" example="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx">Meetup id.</param>
        /// <response code="200">Put meetup.</response>
        /// <response code="404">Meetup with specified id was not found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMeetups()
        {
            var meetups = await _context.Meetups.ToListAsync();

            var readDtos = meetups.Select(meetup => new ReadMeetupDto
            {
                Id = meetup.Id,
                Topic = meetup.Topic,
                Place = meetup.Place,
                Duration = meetup.Duration
            });
            return Ok(readDtos);
        }
        /// <summary>Put meetup</summary>
        /// <param name="id" example="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx">Meetup id.</param>
        /// <response code="200">Put meetup.</response>
        /// <response code="404">Meetup with specified id was not found.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UpdateMeetupDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMeetup([FromRoute] Guid id, [FromBody] UpdateMeetupDto updateDto)
        {
            var oldMeetup = await _context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);
            if (oldMeetup is null)
            {
                return NotFound();
            }

            oldMeetup.Topic = updateDto.Topic;
            oldMeetup.Place = updateDto.Place;
            oldMeetup.Duration = updateDto.Duration;
            await _context.SaveChangesAsync();

            return NoContent();
        }



        /// <summary>Delete meetup with matching id.</summary>
        /// <param name="id" example="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx">Meetup id.</param>
        /// <response code="200">Deleted meetup.</response>
        /// <response code="404">Meetup with specified id was not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ReadMeetupDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMeetup([FromRoute] Guid id)
        {
            var meetupToDelete = await _context.Meetups.SingleOrDefaultAsync(meetup => meetup.Id == id);
            if (meetupToDelete is null)
            {
                return NotFound();
            }

            _context.Meetups.Remove(meetupToDelete);
            await _context.SaveChangesAsync();

            var readDto = new ReadMeetupDto
            {
                Id = meetupToDelete.Id,
                Topic = meetupToDelete.Topic,
                Place = meetupToDelete.Place,
                Duration = meetupToDelete.Duration
            };
            return Ok(readDto);
        }


    }
}