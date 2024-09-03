using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NzWalk.API.Models.Domain;
using NzWalk.API.Models.DTO;
using NzWalk.API.Repositories;

namespace NzWalk.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController : ControllerBase
        {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalkController(IMapper mapper, IWalkRepository walkRepository)
            {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
            }


        // Create Walk using Post
        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] AddWalkDTO addWalkDTO)
            {

            // mapping dto to domain moodel
            var walkDomainModel = mapper.Map<Walk>(addWalkDTO);
            await walkRepository.CreateAsync(walkDomainModel);

            // Map back to dto model
            return Ok(mapper.Map<WalkDTO>(walkDomainModel));
            }


        // Get all Walks
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn,[FromQuery] string? filterQuery , [FromQuery] string? sortBy, [FromQuery] bool? isAscending , [FromQuery] int pageNumber= 1 , [FromQuery] int pageSize = 100)
            {
            var walks = await walkRepository.GetAllAsync(filterOn , filterQuery, sortBy , isAscending?? true, pageNumber,pageSize); // Ensure this returns List<Walk>
            var walkDTOs = mapper.Map<List<WalkDTO>>(walks); // Map List<Walk> to List<WalkDTO>
            return Ok(walkDTOs);
            }

        // get by id single walk
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
            {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
                {
                return NotFound();
                }
            // map model to Dto
            return Ok(mapper.Map<WalkDTO>(walkDomainModel));
            }

        // Update walk
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkDTO updateWalkDTO)
            {
            //map dto to domain Model
            var walkDomainModel = mapper.Map<Walk>(updateWalkDTO);

            // retrieve repostory
           walkDomainModel= await walkRepository.UpdateRegionAsync(id, walkDomainModel);

           if (walkDomainModel == null)
                {
                return NotFound();
                }
           return Ok(mapper.Map<WalkDTO>(walkDomainModel));
            }

        // delete the walk

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
            {
            var   deletedWalkDomainModel= await walkRepository.DeleteAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(deletedWalkDomainModel));

        }

        }
    }