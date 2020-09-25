﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models
{
    public class DashboardChartModel
    {
        public Int32 TotalStudents { get; set; }
        public int TotalBoys { get; set; }
        public int TotalGirls    { get; set; }
        public int TotalStudentPresent { get; set; }
        public int TotalStudentAbsent { get; set; }
        public int TotalStudentLeave { get; set; }

        public int TotalEmployee { get; set; }
        public int TotalEmpMale { get; set; }
        public int TotalEmpFemale { get; set; }
        public int TotalEmpPresent { get; set; }
        public int TotalEmpLeave { get; set; }
        public int TotalEmpAbsent { get; set; }

        public int TotalWithdrawled { get; set; }
        public int TotalNewEnrolled { get; set; }

        public int TotalFeeGenerated { get; set; }
        public int TotalFeePaid { get; set; }
        public int TotalExpenses { get; set; }
        public int TotalLiabilities { get; set; }
        public int TotalSalaryGenerated { get; set; }
        public int TotalSalaryPaid { get; set; }

        public int TotalCashInHand { get; set; }
        public int TotalCashInBank { get; set; }
    }
}
