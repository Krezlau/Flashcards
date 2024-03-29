﻿using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public interface IAuthenticationService
    {
        Task<bool> CreateAccountAsync(string username, string email, string password);

        Task<User> LoginUserAsync(string username, string password);

        Task<bool> ChangeUserPasswordAsync(string newPassword, string oldPassword, string username);

        Task<bool> IfPasswordCorrect(string password, string username);
    }
}
