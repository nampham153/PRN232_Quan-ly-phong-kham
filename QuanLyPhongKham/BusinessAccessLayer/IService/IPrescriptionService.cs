using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.IService
{
    public interface IPrescriptionService
    {
        List<PrescriptionViewModel> GetAllPrescriptions();
        PrescriptionViewModel GetPrescriptionById(int id);
        Prescription GetPrescriptionEntityById(int id); // thêm hàm này để lấy entity gốc

        Prescription CreatePrescription(Prescription prescription); // Thay void bằng Prescription
        void UpdatePrescription(Prescription prescription);
        void DeletePrescription(int id);
        List<PrescriptionViewModel> SearchPrescriptions(int? recordId = null, int? medicineId = null, string dosage = null);
    }
}
