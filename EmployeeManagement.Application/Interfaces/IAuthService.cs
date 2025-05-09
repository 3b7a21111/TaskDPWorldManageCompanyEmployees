﻿using EmployeeManagement.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(UserLoginDTO loginDto);
    }
}
