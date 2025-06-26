using DataAccessLayer.dbcontext;
using DataAccessLayer.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class PrescriptionDAO
    {
        private readonly ClinicDbContext _context;

        public PrescriptionDAO(ClinicDbContext context)
        {
            _context = context;
        }

        public List<Prescription> GetPrescriptions()
        {
            return _context.Prescriptions.Include(p => p.Medicine).ToList();
        }

        public Prescription GetPrescriptionById(int id)
        {
            return _context.Prescriptions.Include(p => p.Medicine).FirstOrDefault(p => p.PrescriptionId == id);
        }

        public Prescription SavePrescription(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();
            return prescription; // Trả về đối tượng đã lưu để đảm bảo PrescriptionId được gán
        }

        public void UpdatePrescription(Prescription prescription)
        {
            _context.Entry(prescription).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeletePrescription(int id)
        {
            var prescription = _context.Prescriptions.SingleOrDefault(c => c.PrescriptionId == id);
            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
                _context.SaveChanges();
            }
        }
    }
}