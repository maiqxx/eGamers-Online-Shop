using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGamersShop.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*Lastname is required.")]
        public string Lastname { get; set; }
        

        [Required(ErrorMessage = "*Firstname is required.")]
        public string Firstname { get; set; }
        

        [Required(ErrorMessage = "*Address is required.")]
        public string Address { get; set; }
        

        [Required(ErrorMessage = "*Birthdate is required.")]
        public string Birthdate { get; set; }
        

        [Required(ErrorMessage = "*Contact number is required.")]
        public string Contactnum { get; set; }
        

        [Required(ErrorMessage = "*Email is required.")]
        public string Email { get; set; }
        

        [Required(ErrorMessage = "*Username is required.")]
        public string Username { get; set; }
        

        [Required(ErrorMessage = "*Password is required.")]
        public string Password { get; set; }
        

        [Required(ErrorMessage = "*Role is required.")]
        public string Role { get; set; }
        
    }

    //public enum Role
    //{
    //    Admin, Customer
    //}
}