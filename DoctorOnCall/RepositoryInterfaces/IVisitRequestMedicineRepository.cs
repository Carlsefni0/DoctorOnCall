using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IVisitRequestMedicineRepository
{
    Task AddMedicinesAsync(ICollection<VisitRequestMedicine> medicines);
    
    Task RemoveMedicinesAsync(ICollection<VisitRequestMedicine> medicines);
    
    Task<ICollection<VisitRequestMedicine>> GetMedicinesByVisitRequestId(int visitRequestId);
}