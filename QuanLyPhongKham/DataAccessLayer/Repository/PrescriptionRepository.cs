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
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly PrescriptionDAO _dao;

        public PrescriptionRepository(PrescriptionDAO dao)
        {
            _dao = dao;
        }

        public List<Prescription> GetAll()
        {
            return _dao.GetPrescriptions();
        }

        public Prescription GetById(int id)
        {
            return _dao.GetPrescriptionById(id);
        }

        public Prescription Create(Prescription prescription)
        {
            return _dao.SavePrescription(prescription); // Trả về đối tượng đã lưu
        }

        public void Update(Prescription prescription)
        {
            _dao.UpdatePrescription(prescription);
        }

        public void Delete(int id)
        {
            _dao.DeletePrescription(id);
        }
    }
}
