﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PTMS.Domain.Entities;
using System;
using System.Collections.Generic;

namespace PTMS.BusinessLogic.Helpers
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(
           IUserStore<AppUser> store,
           IOptions<IdentityOptions> optionsAccessor,
           IPasswordHasher<AppUser> passwordHasher,
           IEnumerable<IUserValidator<AppUser>> userValidators,
           IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
           ILookupNormalizer keyNormalizer,
           IdentityErrorDescriber errors,
           IServiceProvider services,
           ILogger<UserManager<AppUser>> logger)
           : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
