using DataAccessLayer.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository{
    public interface IPrescriptionRepository
    {
        List<Prescription> GetAll();
        Prescription GetById(int id);
        Prescription Create(Prescription prescription); // Thay void bằng Prescription
        void Update(Prescription prescription);
        void Delete(int id);
    }
}
