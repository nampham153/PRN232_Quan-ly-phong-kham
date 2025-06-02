using DataAccessLayer.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient GetPatientById(int id);
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeletePatient(int id);
        bool IsPhoneExists(string phone, int? excludePatientId = null);
        List<Patient> SearchPatients(string fullName, string phone, string email, string address, string gender, DateTime? dobFrom, DateTime? dobTo);
    }
}
