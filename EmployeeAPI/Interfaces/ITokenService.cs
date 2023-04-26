using EmployeeAPI.Models;

namespace EmployeeAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
