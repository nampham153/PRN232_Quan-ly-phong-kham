﻿using DataAccessLayer.models;
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
    }

}
