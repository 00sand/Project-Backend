using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models.Domain;
using TaskManagerAPI.Models.DTO;
using TaskManagerAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private UserManager<IdentityUser> userManager;
        private ITokenRepository tokenRepository;

        public UsersController(IMapper mapper, IUserRepository userRepository, UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetAll()
        {
            var usersDomain = await userRepository.GetAllAsync();
            return Ok(mapper.Map<List<UserDTO>>(usersDomain));
        }

        [HttpGet("{id:Guid}")]
        
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var userDomain = await userRepository.GetByIdAsync(id);

            if (userDomain == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<UserDTO>(userDomain));
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AddUserDTO addUserDTO)
        {
            var userDomainModel = mapper.Map<User>(addUserDTO);

            var identityUser = new IdentityUser
            {
                UserName = userDomainModel.UserName,
                Email = userDomainModel.Email
            };

            // Attempt to create the identity user
            var identityResult = await userManager.CreateAsync(identityUser, addUserDTO.Password);

            if (identityResult.Succeeded)
            {
                // Use the ID from the identity store for the main application database
                userDomainModel.UserId = Guid.Parse(identityUser.Id);

                var createdUser = await userRepository.CreateAsync(userDomainModel);

                if (createdUser == null || createdUser.UserId.ToString() != identityUser.Id)
                {
                    // Delete the user from Identity store to maintain consistency
                    await userManager.DeleteAsync(identityUser);
                    return BadRequest("Failed to create user in main database.");
                }

                // Assign the role to the user
                await userManager.AddToRoleAsync(identityUser, userDomainModel.Role);

                // If user creation is successful, generate JWT token
                var roles = await userManager.GetRolesAsync(identityUser);
                var jwtToken = tokenRepository.CreateJWTToken(identityUser, roles.ToList());

                // Include the JWT token in the response
                var createdUserDTO = mapper.Map<UserDTO>(createdUser);
                createdUserDTO.JwtToken = jwtToken;

                createdUserDTO.UserId = identityUser.Id;

                return CreatedAtAction(nameof(GetById), new { id = createdUserDTO.UserId }, createdUserDTO);
            }
            else
            {
                var errors = identityResult.Errors.Select(e => e.Description);
                return BadRequest(new { message = "User registration failed", errors = errors });
            }
        }


        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserDTO updateUserDTO)
        {
            // 1. Update main database
            var userDomainModel = mapper.Map<User>(updateUserDTO);
            userDomainModel = await userRepository.UpdateAsync(id, userDomainModel);

            if (userDomainModel == null)
            {
                return NotFound();
            }

            // 2. Update Identity database (auth database)
            var identityUser = await userManager.FindByIdAsync(id.ToString());

            if (identityUser == null)
            {
                return NotFound();
            }

            // Update fields in the IdentityUser object as needed
            identityUser.UserName = userDomainModel.UserName;
            identityUser.Email = userDomainModel.Email;

            var identityResult = await userManager.UpdateAsync(identityUser);
            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description);
                return BadRequest(new { message = "User update in auth database failed", errors = errors });
            }

            // Get user roles (assuming you have a method for that)
            var userRoles = await userManager.GetRolesAsync(identityUser);

            // Create a new token using the updated data
            var newToken = tokenRepository.CreateJWTToken(identityUser, userRoles.ToList());

            return Ok(new
            {
                token = newToken,
                user = mapper.Map<UpdateUserDTO>(userDomainModel)
            });
        }


        [HttpDelete("{id:Guid}")]
        
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deletedUser = await userRepository.DeleteAsync(id);

            if (deletedUser == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<UserDTO>(deletedUser));
        }
    }
}
