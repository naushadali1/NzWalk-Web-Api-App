using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NzWalk.API.Data;
using NzWalk.API.Models.Domain;
using NzWalk.API.Models.DTO;
using NzWalk.API.Repositories;

namespace NzWalk.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
        {
        private readonly NZWalkDbContext dbContext;
        private readonly IRegionRepository iRegionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalkDbContext dbContext, IRegionRepository iRegionRepository, IMapper mapper)
            {
            this.dbContext = dbContext;
            this.iRegionRepository = iRegionRepository;
            this.mapper = mapper;
            }

        // Get All Regions
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
            {
            // Get data from database (Domain Model)
            var regionsDomain = await iRegionRepository.GetAllRegionsAsync();

            // convert Domain model to RegionDTo via a Automapper
            // Return Dto data to client (expose Dtos data to client)
            return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
            }


        //Get single region by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegions([FromRoute] Guid id)
            {

            // get data from Data Base (Model>Domain)
            var regionsDomain = await iRegionRepository.GetRegionsAsync(id);
            if (regionsDomain == null)
                {
                return NotFound();
                }

            // Return Dto to client (expose Dtos data to client)
            return Ok(mapper.Map<RegionDTO>(regionsDomain));
            }

        // Post Verb to create a region
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto createRegionDto)
            {

            // Map DTO to domain Model
            var regionDomainModel = mapper.Map<Region>(createRegionDto);

            // Use domain model to create region
            regionDomainModel = await iRegionRepository.CreateRegionAsync(regionDomainModel);

            // map domain back to Dto
            var regionDtos = mapper.Map<RegionDTO>(regionDomainModel);

            // return Dtos to create data
            return CreatedAtAction(nameof(GetRegions), new { id = regionDomainModel.Id }, regionDtos);
            }

        // Update region data using put verb
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDto)
            {

            // dto to domain Model

            var regionDomainModel = mapper.Map<Region>(updateRegionDto);

            //check if the region exists
            regionDomainModel = await iRegionRepository.UpdateRegionAsync(id, regionDomainModel);
            if (regionDomainModel == null)
                {
                return BadRequest();
                }

            // map to dto back
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            // return uppdated region
            return Ok(regionDto);
            }

        // Delete the region
        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
            {

            var regionDomainModel = await iRegionRepository.DeleteRegionAsync(id);
            if (regionDomainModel == null)
                {
                return BadRequest();
                }

            // map to dto and return it 
            return Ok(mapper.Map<RegionDTO>(regionDomainModel));

            }

        }
    }
