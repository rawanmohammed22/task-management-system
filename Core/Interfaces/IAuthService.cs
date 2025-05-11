using System.Threading.Tasks;
using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterDto dto);
    Task<AuthResponse> LoginAsync(LoginDto dto);
}