using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NzWalk.API.Data;
using NzWalk.API.Models.Domain;
using NzWalk.API.Models.DTO;
using NzWalk.API.Repositories;
using System.Text.Json;

namespace NzWalk.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
        {
        private readonly NZWalkDbContext dbContext;
        private readonly IRegionRepository iRegionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalkDbContext dbContext, IRegionRepository iRegionRepository, IMapper mapper, ILogger<RegionsController> logger)
            {
            this.dbContext = dbContext;
            this.iRegionRepository = iRegionRepository;
            this.mapper = mapper;
            this.logger = logger;
            }

        // Get All Regions
        [HttpGet]
      //  [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAllRegions()
            {
            logger.LogInformation("The getAll region action was invoked");
            logger.LogWarning("This is warning");
            logger.LogError("This is an Error");

            // Get data from database (Domain Model)
            var regionsDomain = await iRegionRepository.GetAllRegionsAsync();

            // convert Domain model to RegionDTo via a Automapper
            // Return Dto data to client (expose Dtos data to client)
            logger.LogInformation($"Fineshed getall method request with data {JsonSerializer.Serialize(regionsDomain)}");
            return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
            }


        //Get single region by Id
        [HttpGet]
       // [Authorize(Roles = "Reader")]
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
       // [Authorize(Roles = "Writer")]
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
     //   [Authorize(Roles = "Writer")]
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
        [Route("{id:Guid}")]
        // [Authorize(Roles = "Writer")]
        [AllowAnonymous]
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
