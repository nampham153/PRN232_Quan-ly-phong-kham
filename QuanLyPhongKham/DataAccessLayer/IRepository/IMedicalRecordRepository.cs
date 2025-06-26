using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayer.models;

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

    }
}
