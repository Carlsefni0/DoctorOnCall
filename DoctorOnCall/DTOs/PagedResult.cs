using System.Collections;

namespace DoctorOnCall.DTOs.ResponseDto;

public class PagedResult<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public ICollection<T> Items { get; set; }
}