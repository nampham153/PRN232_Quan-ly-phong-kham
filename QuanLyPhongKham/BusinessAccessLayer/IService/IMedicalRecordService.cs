using DataAccessLayer.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.ViewModels;

namespace BusinessAccessLayer.IService
{
    public interface IMedicalRecordService
    {
        List<MedicalRecord> GetAll();
        MedicalRecord? GetById(int id);
        void Add(MedicalRecord record);
        void Update(MedicalRecord record);
        void Delete(MedicalRecord record);
        bool PatientHasRecord(int patientId);

        IQueryable<MedicalRecord> QueryAll();

    }

}
