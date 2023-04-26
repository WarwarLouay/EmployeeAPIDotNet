using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Mobile { get; set; }
        public string UserType { get; set; }
        public List<Post> Posts { get; set; } = new();
    }
}
