﻿using AutenticacionJwtIdenty.Context;
using AutenticacionJwtIdenty.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutenticacionJwtIdenty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BdContext _bdContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager, BdContext bdContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _bdContext = bdContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModels registerViewModels)
        {
            var user = new IdentityUser { UserName = registerViewModels.Email, Email = registerViewModels.Email };
            var result = await _userManager.CreateAsync(user, registerViewModels.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, false, false);
            if (!result.Succeeded)
            {
                return BadRequest("Usuario o contraseña invalidos");
            }
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //asigno los claims roles del usuario
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var userRoles = _bdContext.UserRoles
           .Where(ur => ur.UserId == user.Id)
           .Select(ur => ur.RoleId)
           .ToList();

            var permisos = await _bdContext.Permissions.Where(p => p.RolePermissions.Any(r => userRoles.Contains(r.RoleId))).ToListAsync();

            foreach (var permiso in permisos)
            {
                claims.Add(new Claim("Permission", permiso.Nombre));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
