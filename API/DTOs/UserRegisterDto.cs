using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserRegisterDto
    {
        [Required] public string Username { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string NickName { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string Password { get; set; }
    }
}