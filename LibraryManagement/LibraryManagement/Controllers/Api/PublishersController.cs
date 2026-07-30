using AutoMapper;
using LibraryManagement.DTOs;
using LibraryManagement.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers.Api
{
    [ApiController]
    [Route("api/publishers")]
    public class PublishersController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public PublishersController(IUnitofWork unitofWork, IMapper mapper)
        {
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var publishers = _unitofWork.Publisher.GetAll().ToList();
            var totalRecords = publishers.Count;
            var paged = publishers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PagedResult<PublisherDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = _mapper.Map<List<PublisherDto>>(paged)
            };

            return Ok(result);
        }
    }
}
