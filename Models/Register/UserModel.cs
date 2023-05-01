using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkachkiWebApp.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public RoleModel Role { get; set; }
        public int? UserId { get; set; }
    }

}
