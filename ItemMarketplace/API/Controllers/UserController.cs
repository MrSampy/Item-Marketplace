using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Models.Add;
using Business.Models.Update;
using Business.Services;
using Business.Validation;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController
    {
        private readonly IUserService userService;
        private IMapper mapper;


        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        //GET: api/user
        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            return new ObjectResult(await userService.GetAllAsync());
        }

        //GET: api/user/id
        [MapToApiVersion("1.0")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserModel>> GetSaleById(int id)
        {
            try
            {
                return new ObjectResult(await userService.GetByIdAsync(id));
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //POST: api/user
        [MapToApiVersion("1.0")]
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] UserToAddModel model)
        {
            try
            {
                await userService.AddAsync(mapper.Map<UserModel>(model));
                return new OkResult();
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //POST: api/user/credentials
        [MapToApiVersion("1.0")]
        [HttpPost("credentials")]
        public async Task<ActionResult> PostUserCredentials([FromBody] UserCredentialsToAddModel model)
        {
            try
            {
                await userService.AddUserCredentials(mapper.Map<UserCredentialsModel>(model));
                return new OkResult();
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //PUT: api/user
        [MapToApiVersion("1.0")]
        [HttpPut]
        public async Task<ActionResult> PutUser([FromBody] UserToUpdateModel model)
        {
            try
            {
                await userService.UpdateAsync(mapper.Map<UserModel>(model));
                return new OkResult();
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //PUT: api/user/credentials
        [MapToApiVersion("1.0")]
        [HttpPut("credentials")]
        public async Task<ActionResult> PutUserCredentials([FromBody] UserCredentialsToUpdateModel model)
        {
            try
            {
                await userService.UpdateUserCredentials(mapper.Map<UserCredentialsModel>(model));
                return new OkResult();
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //DELETE: api/user/id
        [MapToApiVersion("1.0")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await userService.DeleteAsync(id);
                return new OkResult();
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }

        //DELETE: api/user/credentials/id
        [MapToApiVersion("1.0")]
        [HttpDelete("credentials/{id:int}")]
        public async Task<ActionResult> DeleteUserCredentials(int id)
        {
            try
            {
                await userService.DeleteUserCredentials(id);
                return new OkResult();
            }
            catch (ItemMarketplaceException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
        }
    }
}
