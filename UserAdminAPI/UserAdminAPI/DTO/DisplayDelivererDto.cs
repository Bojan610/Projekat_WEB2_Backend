﻿using System;

namespace UserAdminAPI.DTO
{
    public class DisplayDelivererDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
        public string Address { get; set; }
        public string UserKind { get; set; }
        public string Verified { get; set; }
    }
}