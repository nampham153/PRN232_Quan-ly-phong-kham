using DataAccessLayer.DAO;
using DataAccessLayer.dbcontext;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDAO _patientDAO;

        public PatientRepository(PatientDAO patientDAO)
        {
            _patientDAO = patientDAO;
        }

        public List<Patient> GetAllPatients()
        {
            return _patientDAO.GetPatients();
        }

        public Patient GetPatientById(int id)
        {
            return _patientDAO.GetPatientById(id);
        }

        public void AddPatient(Patient patient)
        {
            _patientDAO.SavePatient(patient);
        }

        public void UpdatePatient(Patient patient)
        {
            _patientDAO.UpdatePatient(patient);
        }

        public void DeletePatient(int id)
        {
            var patient = _patientDAO.GetPatientById(id);
            if (patient != null)
            {
                _patientDAO.DeletePatient(patient);
            }
        }

        public bool IsPhoneExists(string phone, int? excludePatientId = null)
        {
            var patients = _patientDAO.GetPatients();
            return patients.Any(p => p.Phone == phone && (excludePatientId == null || p.PatientId != excludePatientId));
        }

        public List<Patient> SearchPatients(string fullName, string phone, string email, string address, string gender, DateTime? dobFrom, DateTime? dobTo)
        {
            var query = _patientDAO.GetPatients().AsQueryable();

            if (!string.IsNullOrEmpty(fullName))
            {
                query = query.Where(p => p.FullName.Contains(fullName));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(p => p.Phone.Contains(phone));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(p => p.Email != null && p.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(p => p.Address != null && p.Address.Contains(address));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(p => p.Gender != null &&
                    p.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase));
            }

            if (dobFrom.HasValue)
            {
                query = query.Where(p => p.DOB >= dobFrom.Value);
            }

            if (dobTo.HasValue)
            {
                query = query.Where(p => p.DOB <= dobTo.Value);
            }

            return query.ToList();
        }

        public bool IsPhoneExists(string phone, int excludePatientId)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                    return false;

                var patients = _patientDAO.GetPatients();
                return patients.Any(p => p.Phone == phone && p.PatientId != excludePatientId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra số điện thoại: {ex.Message}");
            }
        }
    }
}