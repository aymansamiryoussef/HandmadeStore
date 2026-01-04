using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.DTOs.UserDtos
{
    public class AppUserDto
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IList<string> Roles { get; set; } = new List<string>();
        public string? Token { get; set; }
    }
}
