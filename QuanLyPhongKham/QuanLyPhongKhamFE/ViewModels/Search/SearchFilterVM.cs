﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels.Search
{
    public class SearchFilterVM
    {
        public string SearchTerm { get; set; } = "";

        [RegularExpression("^(TestName|Description|TestId)$",
            ErrorMessage = "SortBy must be TestName, Description, or TestId")]
        public string SortBy { get; set; } = "TestName";

        public bool SortDescending { get; set; } = false;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;
    }
}
