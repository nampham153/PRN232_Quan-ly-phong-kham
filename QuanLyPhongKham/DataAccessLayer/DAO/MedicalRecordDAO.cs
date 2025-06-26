
using DataAccessLayer.dbcontext;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DAO
{
    public class MedicalRecordDAO
    {
        private readonly ClinicDbContext _context;

        public MedicalRecordDAO(ClinicDbContext context)
        {
            _context = context;
        }

        public List<MedicalRecord> GetAll()
        {
            return _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.User)
                .Include(m => m.TestResults).ThenInclude(tr => tr.Test)
                .Include(m => m.Prescriptions).ThenInclude(p => p.Medicine)
                .ToList();
        }

        public MedicalRecord? GetById(int id)
        {
            return _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.User)
                .Include(m => m.TestResults).ThenInclude(tr => tr.Test)
                .Include(m => m.Prescriptions).ThenInclude(p => p.Medicine)
                .FirstOrDefault(m => m.RecordId == id);
        }

        public void Add(MedicalRecord record)
        {
            _context.MedicalRecords.Add(record);
            _context.SaveChanges();
        }

        public void Update(MedicalRecord record)
        {
            _context.MedicalRecords.Update(record);
            _context.SaveChanges();
        }

        public void Delete(MedicalRecord record)
        {
            _context.MedicalRecords.Remove(record);
            _context.SaveChanges();
        }
        public IQueryable<MedicalRecord> QueryAll()
        {
            return _context.MedicalRecords
                .Include(m => m.Patient)
                .Include(m => m.User)
                .Include(m => m.TestResults).ThenInclude(tr => tr.Test)
                .Include(m => m.Prescriptions).ThenInclude(p => p.Medicine);
        }
        public bool PatientHasRecord(int patientId)
        {
            return _context.MedicalRecords
                .Any(r => r.PatientId == patientId);
        }
        public bool DoctorHasRecord(int doctorId)
        {
            return _context.MedicalRecords.Any(r => r.UserId == doctorId);
        }

    }
}
