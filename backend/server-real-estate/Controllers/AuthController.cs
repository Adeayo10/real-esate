// using System;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using server_real_estate.Model;
// using server_real_estate.Services;

// [ApiController]
// [Route("api/[controller]")]
// public class AuthController : ControllerBase
// {
//     private readonly UserManager<IdentityUser> _userManager;
//     private readonly SignInManager<IdentityUser> _signInManager;
//     private readonly ITokenService _tokenService;

//     public AuthController(
//         UserManager<IdentityUser> userManager,
//         SignInManager<IdentityUser> signInManager,
//         ITokenService tokenService)
//     {
//         _userManager = userManager;
//         _signInManager = signInManager;
//         _tokenService = tokenService;
//     }
    
    
//     [HttpPost("login")]
//     public async Task<IActionResult> Login([FromBody] Login model)
//     {
//         if (!ModelState.IsValid)
//             return BadRequest(ModelState);

//         var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

//         if (result.Succeeded)
//         {
//             var user = await _userManager.FindByEmailAsync(model.Email);
//             var tokenResult = _tokenService.CreateToken(user.Id, user.Email, "User"); // Set appropriate role

//             if (!tokenResult.Success)
//                 return BadRequest(new { Status = "Error", Message = tokenResult.Message });

//             return Ok(new
//             {
//                 Token = tokenResult.Data,
//                 Expiration = DateTime.Now.AddHours(24), // Match token expiration in TokenService
//                 User = new
//                 {
//                     Id = user.Id,
//                     Email = user.Email,
//                     UserName = user.UserName
//                 }
//             });
//         }

//         return Unauthorized(new { Status = "Error", Message = "Invalid login attempt" });
//     }
    
    
// }
