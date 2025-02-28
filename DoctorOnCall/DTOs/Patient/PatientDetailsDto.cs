using System.Text.Json.Serialization;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.DTOs;

public class PatientDetailsDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
    public string District { get; set; }
    public string Disease { get; set; }
    public int UserId { get; set; } 
    
}