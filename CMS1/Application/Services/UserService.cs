using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Application.DTO.EmployeeDto;
using UserManagement.Application.DTO.PermissionDto;
using UserManagement.Application.DTO.UserDto;
using UserManagement.Application.DTO.UserRoleDto;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure;

namespace UserManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UMSDbContext _dbContext;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public UserService(UMSDbContext dbContext, IGenericRepository<User> userRepo, IMapper mapper, IConfiguration config, IEmailService emailService)
        {
            _dbContext = dbContext;
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
            _emailService = emailService;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserDto dto)
        {
            bool userExist = await _dbContext.Users.AnyAsync(x => x.Email == dto.Email);
            if (userExist)
                throw new ArgumentException("User record exists.");

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            Task.Run(() => _emailService.SendUserRegistrationEmail(user.Email));

            return _mapper.Map<CreateUserResponse>(user);
        }

        public async Task<UserLoginResponse> Login(UserLoginDto loginmodel)
        {
            var user = await _userRepo.GetByConditionAsync(u => u.Email == loginmodel.Email);
            if (user == null)
                throw new Exception("Invalid email.");

            // verify the password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginmodel.Password, user.PasswordHash);
            if (!isPasswordValid)
                throw new Exception("Incorrect credential");


            var employee = await _dbContext.Employees
                .Include(e => e.Role)
                    .ThenInclude(r => r.Permissions)
                .Include(e => e.JobTitle)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.UserId == user.Id);


            var token = GenerateJwt(user, employee);
            var response = _mapper.Map<UserLoginResponse>(user);
            response.Token = token;
            response.Message = employee == null
                ? "Login successful, but you must complete your employee registration to access system features."
                : "Login successful.";
            response.IsEmployee = employee != null;

            if (employee != null)
            {
                response.Employee = _mapper.Map<EmployeeResponse>(employee);
                response.Permissions = _mapper.Map<IEnumerable<PermissionResponse>>(employee.Role.Permissions);
            }

            return response;
        }

        private string GenerateJwt(User user, Employee employee = null)
        {
            var claims = new List <Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            //new Claim(ClaimTypes.GivenName, user.FirstName),
            //new Claim(ClaimTypes.Surname, user.LastName),
            //new Claim(ClaimTypes.Role, user.RoleName)
            };

            //Add role if available
            if (employee?.Role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, employee.Role.RoleName));
                claims.Add(new Claim("EmployeeId", employee.Id.ToString()));

                // Add permissions as individual claims
                foreach (var permission in employee.Role.Permissions)
                {
                    claims.Add(new Claim("Permission", permission.SubModuleName));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JWTSettings:Issuer"],
                audience: _config["JWTSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<CreateUserResponse> UpdateUser(int id, UpdateUserDto dto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Email = dto.Email ?? user.Email;
            user.RoleName = dto.RoleName ?? user.RoleName;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CreateUserResponse>(dto);
        }
    }
}

