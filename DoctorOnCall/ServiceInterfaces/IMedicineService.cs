using DoctorOnCall.DTOs;
using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IMedicineService
{
    Task UpdateMedicinesForVisitRequest(int visitRequestId, Dictionary<int, int> updatedMedicines);
}