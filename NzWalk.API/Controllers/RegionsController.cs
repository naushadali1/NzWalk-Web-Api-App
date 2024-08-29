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

        public RegionsController(NZWalkDbContext dbContext , IRegionRepository iRegionRepository)
        {
            this.dbContext = dbContext;
            this.iRegionRepository = iRegionRepository;
            }
       
        // Get All Regions
        [HttpGet]
        public async Task<IActionResult>GetAllRegions()
            { 
            // Get data from database (Domain Model)
            var regionsDomain = await iRegionRepository.GetAllRegionsAsync();

            //Map domain to DTOs
            var regionDto =  new List<RegionDTO>();

            foreach (var regionDomain in regionsDomain) {
                regionDto.Add(
                    new RegionDTO()
                        {
                        RegionImageUrl = regionDomain.RegionImageUrl,
                        Name = regionDomain.Name,
                        Id = regionDomain.Id,
                        Code = regionDomain.Code,

                        }
                    );
                }

            // Return Dto data to client (expose Dtos data to client)`
            return Ok(regionDto);
            }


        //Get single region by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async  Task<IActionResult> GetRegions([FromRoute] Guid id) {

            //  var region= dbContext.Regions.Find(id);
            //Or Use the below method 

            // get data from Data Base (Model>Domain)
            var regionsDomain = await iRegionRepository.GetRegionsAsync(id);
            if (regionsDomain==null)
            {
                return NotFound();
            }

            //Map to Dtos  
            var regionDto = new RegionDTO
                {
                Id = regionsDomain.Id,
                Name = regionsDomain.Name,
                Code = regionsDomain.Code,
                RegionImageUrl = regionsDomain.RegionImageUrl,
                };

            // Return Dto to client (expose Dtos data to client)
            return Ok(regionDto);
            }

        // Post Verb to create a region
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto createRegionDto)
            {

            // Map DTO to domain Model
            var regionDomainModel = new Region
                {
                Name = createRegionDto.Name,
                Code = createRegionDto.Code,
                RegionImageUrl = createRegionDto.RegionImageUrl,
                };

            // Use domain model to create region
           regionDomainModel = await iRegionRepository.CreateRegionAsync(regionDomainModel);

            // map domain back to Dto
            var regionDtos = new RegionDTO
                {
                Name=regionDomainModel.Name,
                Code=regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
                Id = regionDomainModel.Id
                };

            // return Dtos to create data
            return CreatedAtAction(nameof(GetRegions), new {id = regionDomainModel.Id}, regionDtos);
            }

        // Update region data using put verb
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id , [FromBody] UpdateRegionDTO updateRegionDto)
            {

            // dto to domain Model

            var regionDomainModel = new Region
                {
                Code = updateRegionDto.Code,
                RegionImageUrl = updateRegionDto.RegionImageUrl,
                Name = updateRegionDto.Name,
                };

            //check if the region exists
         regionDomainModel=   await iRegionRepository.UpdateRegionAsync(id, regionDomainModel);
            if (regionDomainModel == null)
                {
                return BadRequest();
                }
                
         

            // map to dto back
            var regionDto = new RegionDTO
                {
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
                Id = regionDomainModel.Id
                };

            // return uppdated region
            return Ok(regionDto);
            }

        // Delete the region
        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id) {
            
            var regionDomainModel = await iRegionRepository.DeleteRegionAsync(id);
            if(regionDomainModel == null)
                {
                return BadRequest();
                }

            // map to dto
            var regionDto = new RegionDTO
                {
                Name= regionDomainModel.Name,
                Code = regionDomainModel.Code,
                Id = regionDomainModel.Id
                };
            return Ok(regionDto);
            
            }

        }
    }
