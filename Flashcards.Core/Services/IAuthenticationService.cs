﻿using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public interface IAuthenticationService
    {
        Task<bool> CreateAccountAsync(string username, string email, string password, string confirmPassword);

        Task<User> LoginUserAsync(string username, string password);
    }
}
