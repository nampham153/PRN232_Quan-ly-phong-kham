using BusinessAccessLayer.IService;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _repository;

        public PrescriptionService(IPrescriptionRepository repository)
        {
            _repository = repository;
        }

        public List<PrescriptionViewModel> GetAllPrescriptions()
        {
            var prescriptions = _repository.GetAll();
            return prescriptions.Select(p => new PrescriptionViewModel
            {
                PrescriptionId = p.PrescriptionId,
                RecordId = p.RecordId,
                MedicineId = p.MedicineId,
                MedicineName = p.Medicine != null ? p.Medicine.MedicineName : "Unknown",
                Quantity = p.Quantity,
                Dosage = p.Dosage
            }).ToList();
        }

        public PrescriptionViewModel GetPrescriptionById(int id)
        {
            var prescription = _repository.GetById(id);
            if (prescription == null) return null;
            return new PrescriptionViewModel
            {
                PrescriptionId = prescription.PrescriptionId,
                RecordId = prescription.RecordId,
                MedicineId = prescription.MedicineId,
                MedicineName = prescription.Medicine != null ? prescription.Medicine.MedicineName : "Unknown", // Sửa từ p.Medicine thành prescription.Medicine
                Quantity = prescription.Quantity,
                Dosage = prescription.Dosage
            };
        }

        public Prescription CreatePrescription(Prescription prescription)
        {
            if (prescription.Quantity <= 0) throw new ArgumentException("Quantity must be positive.");
            return _repository.Create(prescription);
        }

        public void UpdatePrescription(Prescription prescription)
        {
            if (prescription == null) throw new ArgumentNullException(nameof(prescription));
            _repository.Update(prescription);
        }

        public void DeletePrescription(int id)
        {
            _repository.Delete(id);
        }

        public List<PrescriptionViewModel> SearchPrescriptions(int? recordId = null, int? medicineId = null, int? quantity = null, string dosage = null)
        {
            var prescriptions = _repository.GetAll().AsQueryable();
            if (recordId.HasValue) prescriptions = prescriptions.Where(p => p.RecordId == recordId.Value);
            if (medicineId.HasValue) prescriptions = prescriptions.Where(p => p.MedicineId == medicineId.Value);
            if (quantity.HasValue) prescriptions = prescriptions.Where(p => p.Quantity == quantity.Value);
            if (!string.IsNullOrEmpty(dosage)) prescriptions = prescriptions.Where(p => p.Dosage == dosage);

            return prescriptions.Select(p => new PrescriptionViewModel
            {
                PrescriptionId = p.PrescriptionId,
                RecordId = p.RecordId,
                MedicineId = p.MedicineId,
                MedicineName = p.Medicine != null ? p.Medicine.MedicineName : "Unknown",
                Quantity = p.Quantity,
                Dosage = p.Dosage
            }).ToList();
        }

        public Prescription GetPrescriptionEntityById(int id)
        {
            var prescription = _repository.GetById(id);
            if (prescription == null) return null;
            return new Prescription
            {
                PrescriptionId = prescription.PrescriptionId,
                RecordId = prescription.RecordId,
                MedicineId = prescription.MedicineId,            
                Quantity = prescription.Quantity,
                Dosage = prescription.Dosage
            };
        }
    }
}