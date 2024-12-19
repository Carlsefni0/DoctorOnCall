﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorOnCall.Models;

public class Patient
{
    public int Id { get; set; }
    
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public string District { get; set; }
    
    [Required]
    public string Disease { get; set; }
    
    [ForeignKey("UserId")]
    public int UserId { get; set; } 
    public virtual AppUser User { get; set; }
    
}