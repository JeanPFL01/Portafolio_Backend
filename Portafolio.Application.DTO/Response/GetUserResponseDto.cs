using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Application.DTO.Response
{
    public class GetUserResponseDto
    {
        public int UserId { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string Name { get; set; } = null!;

        public string? Position { get; set; }

        public DateTime? HireDate { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }
    }
}
