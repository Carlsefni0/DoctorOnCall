using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.DTOs;

public class DoctorDetailsDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Specialization { get; set; }
    public string WorkingDistrict { get; set; }
    public string Hospital { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }
    public int UserId { get; set; }
    
}