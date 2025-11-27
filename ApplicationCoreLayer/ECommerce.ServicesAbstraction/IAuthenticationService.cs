using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
    }
}
