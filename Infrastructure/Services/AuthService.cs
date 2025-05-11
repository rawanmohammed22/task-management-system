using Core.Entities;

using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Core.DTOs;
using Microsoft.Extensions.Logging;
using Core.Interfaces;
namespace Infrastructure.Services;






public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtTokenGenerator _tokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        JwtTokenGenerator tokenGenerator,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
        _logger = logger;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterDto dto)
    {
        try
        {
            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                _logger.LogWarning("Registration failed for {Email}: {Errors}", dto.Email, string.Join(", ", errors));
                throw new ApplicationException($"Registration failed: {string.Join(", ", errors)}");
            }

            _logger.LogInformation("User {Email} registered successfully", dto.Email);
            return _tokenGenerator.GenerateToken(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during registration for {Email}", dto.Email);
            throw;
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginDto dto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                _logger.LogWarning("Login attempt for non-existent email: {Email}", dto.Email);
                throw new ApplicationException("Invalid credentials");
            }

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                _logger.LogWarning("Invalid password attempt for {Email}", dto.Email);
                throw new ApplicationException("Invalid credentials");
            }

            _logger.LogInformation("User {Email} logged in successfully", dto.Email);
            return _tokenGenerator.GenerateToken(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for {Email}", dto.Email);
            throw;
        }
    }
}