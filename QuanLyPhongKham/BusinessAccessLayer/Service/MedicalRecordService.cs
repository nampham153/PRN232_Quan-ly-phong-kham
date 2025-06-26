
﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessAccessLayer.IService;
using DataAccessLayer.dbcontext;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BusinessAccessLayer.Service
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly ClinicDbContext _context;

        public MedicalRecordService(IMedicalRecordRepository repository, ClinicDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public List<MedicalRecord> GetAll() => _repository.GetAll();

        public MedicalRecord? GetById(int id) => _repository.GetById(id);

        public void Add(MedicalRecord record) => _repository.Add(record);

        public void Update(MedicalRecord record) => _repository.Update(record);

        public void Delete(MedicalRecord record) => _repository.Delete(record);
        public IQueryable<MedicalRecord> QueryAll() => _repository.QueryAll();
        public bool PatientHasRecord(int patientId)
        {
            return _repository.PatientHasRecord(patientId);
        }

    }
}
