﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceXBackend.Services.DTO
{
    public class AuthDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
