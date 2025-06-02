using DataAccessLayer.Repository;
using DataAccessLayer.models;
using System;
using BusinessAccessLayer.IService;
using DataAccessLayer.IRepository;

namespace BusinessAccessLayer.Service
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public List<DataAccessLayer.models.Patient> GetAllPatients()
        {
            return _patientRepository.GetAllPatients();
        }

        public DataAccessLayer.models.Patient GetPatientById(int id)
        {
            return _patientRepository.GetPatientById(id);
        }

        public void AddPatient(DataAccessLayer.models.Patient patient)
        {
            // Kiểm tra số điện thoại đã tồn tại
            if (_patientRepository.GetAllPatients().Any(p => p.Phone == patient.Phone))
            {
                throw new ArgumentException("Số điện thoại đã tồn tại trong hệ thống.");
            }

            // Validation cơ bản (có thể bỏ nếu đã validate ở ViewModel)
            if (string.IsNullOrEmpty(patient.FullName))
                throw new ArgumentException("Họ tên không được để trống");

            if (patient.DOB == default)
                throw new ArgumentException("Ngày sinh không được để trống");

            if (string.IsNullOrEmpty(patient.Gender))
                throw new ArgumentException("Giới tính không được để trống");

            if (string.IsNullOrEmpty(patient.Phone) || !System.Text.RegularExpressions.Regex.IsMatch(patient.Phone, @"^0\d{9}$"))
                throw new ArgumentException("Số điện thoại phải bắt đầu bằng 0 và có đúng 10 chữ số");

            if (!string.IsNullOrEmpty(patient.Email) && !IsValidEmail(patient.Email))
                throw new ArgumentException("Email không hợp lệ");

            if (!string.IsNullOrEmpty(patient.Address) && patient.Address.Length > 200)
                throw new ArgumentException("Địa chỉ không được vượt quá 200 ký tự");

            _patientRepository.AddPatient(patient);
        }

        public void UpdatePatient(DataAccessLayer.models.Patient patient)
        {
            // Kiểm tra số điện thoại đã tồn tại, nhưng bỏ qua nếu không thay đổi
            var existingPatient = _patientRepository.GetPatientById(patient.PatientId);
            if (existingPatient != null && existingPatient.Phone != patient.Phone)
            {
                if (_patientRepository.GetAllPatients().Any(p => p.Phone == patient.Phone && p.PatientId != patient.PatientId))
                {
                    throw new ArgumentException("Số điện thoại đã tồn tại trong hệ thống.");
                }
            }

            // Validation cơ bản
            if (string.IsNullOrEmpty(patient.FullName))
                throw new ArgumentException("Họ tên không được để trống");

            if (patient.DOB == default)
                throw new ArgumentException("Ngày sinh không được để trống");

            if (string.IsNullOrEmpty(patient.Gender))
                throw new ArgumentException("Giới tính không được để trống");

            if (string.IsNullOrEmpty(patient.Phone) || !System.Text.RegularExpressions.Regex.IsMatch(patient.Phone, @"^0\d{9}$"))
                throw new ArgumentException("Số điện thoại phải bắt đầu bằng 0 và có đúng 10 chữ số");

            if (!string.IsNullOrEmpty(patient.Email) && !IsValidEmail(patient.Email))
                throw new ArgumentException("Email không hợp lệ");

            if (!string.IsNullOrEmpty(patient.Address) && patient.Address.Length > 200)
                throw new ArgumentException("Địa chỉ không được vượt quá 200 ký tự");

            _patientRepository.UpdatePatient(patient);
        }

        public void DeletePatient(int id)
        {
            _patientRepository.DeletePatient(id);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}