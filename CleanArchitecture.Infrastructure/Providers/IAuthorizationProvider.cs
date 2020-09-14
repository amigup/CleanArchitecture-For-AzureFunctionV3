﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Providers
{
    public interface IAuthorizationProvider
    {
        Task Authorize();
    }
}
