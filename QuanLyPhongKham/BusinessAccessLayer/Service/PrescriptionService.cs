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
            return prescriptions.AsEnumerable() // Chuyển sang IEnumerable trước khi ánh xạ
                .Select(p => new PrescriptionViewModel
                {
                    PrescriptionId = p.PrescriptionId,
                    RecordId = p.RecordId,
                    MedicineId = p.MedicineId,
                    MedicineName = p.Medicine != null ? p.Medicine.MedicineName : "Unknown",
                    Dosage = p.Dosage,
                    Date = p.Date,
                    Diagnosis = p.MedicalRecord?.Diagnosis ?? "Unknown", // Xử lý null
                    DoctorName = p.MedicalRecord?.User?.FullName ?? "Unknown" // Xử lý null
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
                MedicineName = prescription.Medicine != null ? prescription.Medicine.MedicineName : "Unknown",
                Dosage = prescription.Dosage,
                Date = prescription.Date,
                Diagnosis = prescription.MedicalRecord?.Diagnosis ?? "Unknown", // Xử lý null
                DoctorName = prescription.MedicalRecord?.User?.FullName ?? "Unknown" // Xử lý null
            };
        }

        public Prescription CreatePrescription(Prescription prescription)
        {
            prescription.Date = DateTime.Now; // Gán thời gian tạo mặc định
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

        public List<PrescriptionViewModel> SearchPrescriptions(int? recordId = null, int? medicineId = null, string dosage = null, DateTime? date = null, string diagnosis = null, string doctorName = null)
        {
            var prescriptions = _repository.GetAll().AsQueryable();
            if (recordId.HasValue) prescriptions = prescriptions.Where(p => p.RecordId == recordId.Value);
            if (medicineId.HasValue) prescriptions = prescriptions.Where(p => p.MedicineId == medicineId.Value);
            if (!string.IsNullOrEmpty(dosage)) prescriptions = prescriptions.Where(p => p.Dosage == dosage);
            if (date.HasValue) prescriptions = prescriptions.Where(p => p.Date.Date == date.Value.Date);
            if (!string.IsNullOrEmpty(diagnosis)) prescriptions = prescriptions.Where(p => p.MedicalRecord.Diagnosis == diagnosis);
            if (!string.IsNullOrEmpty(doctorName)) prescriptions = prescriptions.Where(p => p.MedicalRecord.User.FullName == doctorName);

            return prescriptions.AsEnumerable() // Chuyển sang IEnumerable trước khi ánh xạ
                .Select(p => new PrescriptionViewModel
                {
                    PrescriptionId = p.PrescriptionId,
                    RecordId = p.RecordId,
                    MedicineId = p.MedicineId,
                    MedicineName = p.Medicine != null ? p.Medicine.MedicineName : "Unknown",
                    Dosage = p.Dosage,
                    Date = p.Date,
                    Diagnosis = p.MedicalRecord?.Diagnosis ?? "Unknown", // Xử lý null
                    DoctorName = p.MedicalRecord?.User?.FullName ?? "Unknown" // Xử lý null
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
                Dosage = prescription.Dosage,
                Date = prescription.Date
            };
        }
    }
}