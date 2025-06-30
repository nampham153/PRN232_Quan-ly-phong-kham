﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels.Authen
{
    public class ChangePasswordViewModel
    {
        public int AccountId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
