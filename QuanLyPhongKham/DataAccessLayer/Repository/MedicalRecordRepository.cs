
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessLayer.DAO;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;

namespace DataAccessLayer.Repository
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly MedicalRecordDAO _dao;

        public MedicalRecordRepository(MedicalRecordDAO dao)
        {
            _dao = dao;
        }

        public List<MedicalRecord> GetAll() => _dao.GetAll();

        public MedicalRecord? GetById(int id) => _dao.GetById(id);

        public void Add(MedicalRecord record) => _dao.Add(record);

        public void Update(MedicalRecord record) => _dao.Update(record);

        public void Delete(MedicalRecord record) => _dao.Delete(record);
        public IQueryable<MedicalRecord> QueryAll() => _dao.QueryAll();
        public bool PatientHasRecord(int patientId)
        {
            return _dao.PatientHasRecord(patientId);
        }
        public bool DoctorHasRecord(int doctorId)
        {
            return _dao.DoctorHasRecord(doctorId);
        }
        public bool DoctorExists(int doctorId)
        {
            return _dao.DoctorExists(doctorId);
        }

        public bool PatientExists(int patientId)
        {
            return _dao.PatientExists(patientId);
        }
        public User? GetDoctorById(int doctorId)
        {
            return _dao.GetDoctorById(doctorId);
        }

    }
}
