﻿using TaskManagerAPI.Models.Domain;

namespace TaskManagerAPI.Models.DTO
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string JwtToken { get; set; }
        public string Role { get; set; }



    }
}
