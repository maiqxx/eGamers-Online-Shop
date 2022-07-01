using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace eGamersShop.Models
{
    public class UserModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter Lastname.")]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Please enter your Firstname.")]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Display(Name = "Middle name")]
        public string Middlename { get; set; }
        
        [Required(ErrorMessage = "Please enter your Address.")]
        [Display(Name = "Address")]
        public string Address { get; set; }



        public RoleType SelectRoleType { get; set; }
    }

    public enum RoleType
    {
        Admin, Customer
    }

   
}