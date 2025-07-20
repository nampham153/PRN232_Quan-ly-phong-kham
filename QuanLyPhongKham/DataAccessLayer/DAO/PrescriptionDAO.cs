using DataAccessLayer.dbcontext;
using DataAccessLayer.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.DAO
{
    public class PrescriptionDAO
    {
        private readonly ClinicDbContext _context;

        public PrescriptionDAO(ClinicDbContext context)
        {
            _context = context;
        }

        public List<Prescription> GetPrescriptions()
        {
            return _context.Prescriptions
                           .Include(p => p.Medicine)
                           .AsNoTracking()
                           .ToList();
        }

        public Prescription GetPrescriptionById(int id)
        {
            return _context.Prescriptions
                           .Include(p => p.Medicine)
                           .AsNoTracking()
                           .FirstOrDefault(p => p.PrescriptionId == id);
        }

        public Prescription SavePrescription(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();
            return prescription;
        }

        public void UpdatePrescription(Prescription updatedPrescription)
        {
            var existing = _context.Prescriptions
                                   .FirstOrDefault(p => p.PrescriptionId == updatedPrescription.PrescriptionId);

            if (existing != null)
            {
                existing.RecordId = updatedPrescription.RecordId;
                existing.MedicineId = updatedPrescription.MedicineId;
                
                existing.Dosage = updatedPrescription.Dosage;

                _context.SaveChanges();
            }
        }

        public void DeletePrescription(int id)
        {
            var prescription = _context.Prescriptions
                                       .FirstOrDefault(p => p.PrescriptionId == id);

            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
                _context.SaveChanges();
            }
        }
    }
}
