using DoctorOnCall.DTOs;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Utils;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Services;

//TODO: Refactor this class later
public class MedicineService: IMedicineService
{
    private readonly IVisitRequestMedicineRepository _visitRequestMedicineRepository;

    public MedicineService(IVisitRequestMedicineRepository visitRequestMedicineRepository, DataContext db)
    {
        _visitRequestMedicineRepository = visitRequestMedicineRepository;
    }
    
    public async Task UpdateMedicinesForVisitRequest(int visitRequestId, Dictionary<int, int> updatedMedicines)
    {
        var currentMedicines = await _visitRequestMedicineRepository.GetMedicinesByVisitRequestId(visitRequestId);

        var currentMedicinesDict = currentMedicines.ToDictionary(m => m.MedicineId, m => m.Quantity);

        var medicinesToAdd = updatedMedicines
            .Where(m => !currentMedicinesDict.ContainsKey(m.Key))
            .Select(m => new VisitRequestMedicine
            {
                VisitRequestId = visitRequestId,
                MedicineId = m.Key,
                Quantity = m.Value
            })
            .ToList();

        var medicinesToRemove = currentMedicines
            .Where(m => !updatedMedicines.ContainsKey(m.MedicineId))
            .ToList();

        foreach (var medicine in currentMedicines)
        {
            if (updatedMedicines.TryGetValue(medicine.MedicineId, out var newQuantity))
            {
                medicine.Quantity = newQuantity;
            }
        }

        if (medicinesToAdd.Any())
        {
            await _visitRequestMedicineRepository.AddMedicinesAsync(medicinesToAdd);
        }

        if (medicinesToRemove.Any())
        {
            await _visitRequestMedicineRepository.RemoveMedicinesAsync(medicinesToRemove);
        }

       
    }

    
    
}