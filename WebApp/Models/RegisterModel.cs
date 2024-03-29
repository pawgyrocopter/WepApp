﻿using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "Не указан Email")]
    public string Email { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
 
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароль введен неверно")]
    public string ConfirmPassword { get; set; }
}