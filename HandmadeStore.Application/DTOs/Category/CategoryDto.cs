using System;
using System.Collections.Generic;
using System.Text;

namespace HandmadeStore.Application.DTOs.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } 
    }
}
