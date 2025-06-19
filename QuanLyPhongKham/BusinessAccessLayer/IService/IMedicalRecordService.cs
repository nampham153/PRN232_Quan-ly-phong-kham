using DataAccessLayer.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.IService
{
    public interface IMedicalRecordService
    {
        List<MedicalRecord> GetAll();
        MedicalRecord? GetById(int id);
    }

}
