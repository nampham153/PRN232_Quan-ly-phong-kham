using DataAccessLayer.DAO;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
