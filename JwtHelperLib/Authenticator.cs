﻿using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using JwtHelperLib.Data;
using RepoInterfaceLib;

namespace JwtHelperLib
{
    public class Authenticator
    {
        private IRepo<UserDbContext> _repo;
        private AuthenticationService _authService;

        public bool IsOK { get; private set; } = false;

        public Authenticator(IRepo<UserDbContext> repo, AuthenticationService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
            var result = "User not found.";
            try
            {
                var user = await _repo.FetchAsync(dbContext => dbContext.Users
                            .Where(u => u.UserName == userName && u.Password == password)
                            .FirstOrDefault());
                if (user != null)
                {
                    result = await _authService.Login(user.UserName, user.Type);
                    IsOK = true;
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }
    }
}
