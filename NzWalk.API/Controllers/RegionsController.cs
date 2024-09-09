using AutoMapper;
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

        // GET: api/regions - Retrieve all regions
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
            {
            logger.LogInformation("The GetAllRegions action was invoked");
            try
                {
                // Fetch all regions from the repository
                var regionsDomain = await iRegionRepository.GetAllRegionsAsync();

                // Log the serialized data for debugging
                logger.LogInformation($"Completed request with data: {JsonSerializer.Serialize(regionsDomain)}");

                // Map domain models to DTOs and return them
                return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
                }
            catch (Exception ex)
                {
                logger.LogError(ex, "Error retrieving regions.");
                return StatusCode(500, "Internal server error");
                }
            }

        // GET: api/regions/{id} - Retrieve a specific region by ID
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetRegions([FromRoute] Guid id)
            {
            // Fetch the region from the repository
            var regionsDomain = await iRegionRepository.GetRegionsAsync(id);
            if (regionsDomain == null)
                {
                return NotFound();
                }

            // Map domain model to DTO and return
            return Ok(mapper.Map<RegionDTO>(regionsDomain));
            }

        // POST: api/regions - Create a new region
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto createRegionDto)
            {
            // Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(createRegionDto);

            // Save the new region via repository
            regionDomainModel = await iRegionRepository.CreateRegionAsync(regionDomainModel);

            // Map domain model back to DTO and return
            var regionDtos = mapper.Map<RegionDTO>(regionDomainModel);
            return CreatedAtAction(nameof(GetRegions), new { id = regionDomainModel.Id }, regionDtos);
            }

        // PUT: api/regions/{id} - Update a region by ID
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDto)
            {
            // Map DTO to domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionDto);

            // Update region in the repository
            regionDomainModel = await iRegionRepository.UpdateRegionAsync(id, regionDomainModel);
            if (regionDomainModel == null)
                {
                return BadRequest();
                }

            // Map updated domain model back to DTO and return
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDto);
            }

        // DELETE: api/regions/{id} - Delete a region by ID
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
            {
            // Delete region via repository
            var regionDomainModel = await iRegionRepository.DeleteRegionAsync(id);
            if (regionDomainModel == null)
                {
                return BadRequest();
                }

            // Map deleted domain model to DTO and return
            return Ok(mapper.Map<RegionDTO>(regionDomainModel));
            }
        }
    }
