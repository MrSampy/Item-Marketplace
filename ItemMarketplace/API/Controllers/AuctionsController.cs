using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Business.Validation;
using Microsoft.AspNetCore.Authorization;
using Business.Models.Add;
using Business.Models.Update;
using AutoMapper;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController
    {
        private readonly IAuctionsService auctionsService;
        private IMapper mapper;


        public AuctionsController(IAuctionsService auctionsService, IMapper mapper)
        {
            this.auctionsService = auctionsService;
            this.mapper = mapper;
        }

        // GET: api/auctions/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SaleModel>> GetSaleById(int id)
        {
            try
            {
                return new ObjectResult(await auctionsService.GetByIdAsync(id));
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        // GET: api/auctions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleModel>>> GetSalesByFilter([FromQuery] FilterSerchModel filterSerchModel)
        {
            return new ObjectResult(await auctionsService.GetSalesByFilter(filterSerchModel));
        }

        // POST: api/auctions
        [HttpPost]
        public async Task<ActionResult> AddSale([FromBody] SaleToAddModel sale) 
        {
            try
            {
                await auctionsService.AddAsync(mapper.Map<SaleModel>(sale));
                return new OkResult();
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }        
        }

        // PUT: api/auctions
        [HttpPut]
        public async Task<ActionResult> UpdateSale([FromBody] SaleToUpdateModel sale)
        {
            try
            {
                await auctionsService.UpdateAsync(mapper.Map<SaleModel>(sale));
                return new OkResult();
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        // DELETE: api/auctions/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteSale(int id)
        {
            try
            {
                await auctionsService.DeleteAsync(id);
                return new OkResult();
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
