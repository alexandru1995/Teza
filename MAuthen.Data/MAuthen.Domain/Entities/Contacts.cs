﻿using System;

namespace MAuthen.Domain.Entities
{
    public class Contacts
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
