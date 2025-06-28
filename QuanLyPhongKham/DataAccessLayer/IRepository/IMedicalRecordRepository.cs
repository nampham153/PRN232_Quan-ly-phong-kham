
using DataAccessLayer.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository
{
    public interface IMedicalRecordRepository
    {
        List<MedicalRecord> GetAll();
        MedicalRecord? GetById(int id);
        void Add(MedicalRecord record);
        void Update(MedicalRecord record);
        void Delete(MedicalRecord record);
        IQueryable<MedicalRecord> QueryAll();
        bool PatientHasRecord(int patientId);
        bool DoctorHasRecord(int doctorId);
        bool PatientExists(int patientId);
        bool DoctorExists(int doctorId);

    }
}
