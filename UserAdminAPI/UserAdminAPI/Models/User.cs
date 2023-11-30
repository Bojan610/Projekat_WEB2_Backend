﻿using System;

namespace UserAdminAPI.Models
{
    public abstract class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
        public string Address { get; set; }
        public string UserKind { get; set; }

        public string Password { get; set; }
        public string Image { get; set; }
    }
}
