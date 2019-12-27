using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Solstice.Api.Helpers;
using Solstice.API.DATA;
using Solstice.API.DTOs;

namespace Solstice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactRecordsController : ControllerBase
    {
        private readonly IContactRepository _repo;
        private readonly IMapper _mapper;
        public ContactRecordsController(IContactRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// • Retrieve all contact records
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var contacts = await _repo.GetAllContacts();
            var response = _mapper.Map<IEnumerable<ContactDTO>>(contacts);
            return Ok(response);
        }

        /// <summary>
        /// • Search for a record by email or phone number
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> GetSingleFiltered([FromQuery]contactParams contactParams)
        {
            var contacts = await _repo.GetContactsfiltered(contactParams);
            var response = _mapper.Map<ContactDTO>(contacts.FirstOrDefault());
            return Ok(response);
        }


        /// <summary>
        /// • Retrieve all records from the same state or city
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetAllFiltered([FromQuery]contactParams contactParams)
        {
            var contacts = await _repo.GetContactsfiltered(contactParams);
            var response = _mapper.Map<IEnumerable<ContactDTO>>(contacts);
            return Ok(response);
        }


        /// <summary>
        /// • Retrieve a contact record
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var contact = await _repo.GetContactRecord(id);
            return Ok(_mapper.Map<ContactDTO>(contact));
        }

        /// <summary>
        /// • Create a contact record
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///        {
        ///         "ProfileImage": "http://placehold.it/32x32",
        ///         "Name": {
        ///             "FirstName": "Kirsten",
        ///             "LastName": "Rich"
        ///         },
        ///         "Company": "FLUMBO",
        ///         "Email": "kirsten.rich@flumbo.com",
        ///         "PhoneNumbers": [
        ///             {
        ///                 "Number": "+54 (827) 413-2654",
        ///                 "IsPersonal": false
        ///             },
        ///             {
        ///                 "Number": "+54 (844) 512-3273",
        ///                 "IsPersonal": false
        ///             },
        ///             {
        ///                 "Number": "+54 (831) 461-2623",
        ///                 "IsPersonal": true
        ///             },
        ///             {
        ///                 "Number": "+54 (871) 440-3087",
        ///                 "IsPersonal": false
        ///             },
        ///             {
        ///                 "Number": "+54 (908) 553-2541",
        ///                 "IsPersonal": true
        ///             }
        ///         ],
        ///         "address": {
        ///             "Number": 197,
        ///             "Street": "Banner Avenue",
        ///             "City": "Kaka",
        ///             "State": "Missouri",
        ///             "ZipCode": 7721
        ///         },
        ///         "Birthdate": "1996-10-28"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ContactToAddDTO contact)
        {
            var a = await _repo.Add(contact);
            return Ok(a);
        }

        /// <summary>
        /// • Update a contact record
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST 
        ///        {
        ///         "Id": 21,
        ///         "ProfileImage": "http://placehold.it/32x32",
        ///         "Name": {
        ///             "FirstName": "Carlos",
        ///             "LastName": "Ramos"
        ///         },
        ///         "Company": "Solstice",
        ///         "Email": "Carlos.Ramos@Solstice.com",
        ///         "PhoneNumbers": [
        ///             {
        ///                 "Id": 73,
        ///                 "Number": "+54 (827) 413-2654",
        ///                 "IsPersonal": true
        ///             },
        ///             {
        ///                 "Id": 74,
        ///                 "Number": "+54 (844) 512-3273",
        ///                 "IsPersonal": false
        ///             }
        ///         ],
        ///         "address": {
        ///             "Number": 1498,
        ///             "Street": "Hardouin Avenue",
        ///             "City": "Austin",
        ///             "State": "Texas",
        ///             "ZipCode": 78703
        ///         },
        ///         "Birthdate": "1992-11-29"
        ///     }
        ///
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ContactToAddDTO contactToUpdate)
        {
            var response = await _repo.UpdateContactRecord(id, contactToUpdate);
            return Ok(_mapper.Map<ContactDTO>(response));
        }

        /// <summary>
        /// • Delete a contact record
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _repo.GetContactRecord(id);
            var a = await _repo.Delete(contact);
            return Ok(a);
        }

    }
}
