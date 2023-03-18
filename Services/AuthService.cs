using ChatApp_Api.DTOS.Auth;
using ChatApp_Api.Helpers;
using ChatApp_Api.Interfaces;
using ChatApp_Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp_Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWT _jwt;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;


        public AuthService(SignInManager<User> signInManager, UserManager<User> userManager ,IOptions<JWT> jwt)
        {
            _jwt = jwt.Value;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<UserReturnDto> UserRegister(UserRegisterDto userDto)
        {

            //check if email is exist
            if (await _userManager.FindByEmailAsync(userDto.Email) is not null)
                return new UserReturnDto { Massage = "Email is alerdy register!" };

            //check if userName is exist
            if (await _userManager.FindByNameAsync(userDto.UserName) is not null)
                return new UserReturnDto { Massage = "UserName is alerdy register!" };

            var user = new User
            {
                Name = userDto.UserName,
                Email = userDto.Email,
                UserName = userDto.UserName,
            };
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new UserReturnDto { Massage = errors };
            }
            // if creation is success assign role to doctor
            await _userManager.AddToRoleAsync(user, RoleNames.UserRole);

            var jwtSecurityToken = await CreateJwtToken(user);

            return new UserReturnDto
            {
                Id = user.Id,
                Email = user.Email,
                IsAuth = true,
                Roles = new List<string> { RoleNames.UserRole },
                UserName = user.UserName,
                Massage = "User Created Successfully",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpiresOn = jwtSecurityToken.ValidTo

            };
        }

        public async Task<UserReturnDto> UserLogin(UserLoginDto dto)
        {
            var userReturnDto = new UserReturnDto();
            var user = await _userManager.FindByEmailAsync(dto.Email);
            //check if email is exist
            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                userReturnDto.Massage = "Email or Password is incorrect";
            }
            else
            {
                var jwtSecurityToken = await CreateJwtToken(user);
                var roles = await _userManager.GetRolesAsync(user);

                userReturnDto.Id = user.Id;
                userReturnDto.IsAuth = true;
                userReturnDto.Email = user.Email;
                userReturnDto.UserName = user.UserName;
                userReturnDto.Roles = roles.ToList();
                userReturnDto.Massage = "Login Successfully";
                userReturnDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                userReturnDto.ExpiresOn = jwtSecurityToken.ValidTo;
                await _signInManager.PasswordSignInAsync(user, dto.Password, true, false);
            }


            
            return userReturnDto;
        }




        
    }
}
