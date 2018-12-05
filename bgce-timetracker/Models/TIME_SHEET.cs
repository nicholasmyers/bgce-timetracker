//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bgce_timetracker.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    public class TimeSheetSearch
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TIME_SHEET format { get; set; }
        public IEnumerable<bgce_timetracker.Models.TIME_SHEET> TSList { get; set; }
    }

    public partial class TIME_SHEET
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIME_SHEET()
        {
            this.TIME_SHEET_ENTRY = new HashSet<TIME_SHEET_ENTRY>();
        }

        [DisplayName("Timesheet ID")]
        public int timesheetID { get; set; }
        [DisplayName("Employee")]
        public int employee { get; set; }
        [DisplayName("Submitted on")]
        public Nullable<System.DateTime> submitted_on { get; set; }
        [DisplayName("Approved on")]
        public Nullable<System.DateTime> approved_on { get; set; }
        [DisplayName("Approved by")]
        public Nullable<int> approved_by { get; set; }
        [DisplayName("Submitted")]
        public bool submitted { get; set; }
        [DisplayName("Approved")]
        public bool approved { get; set; }
        [DisplayName("Comments")]
        public string comments { get; set; }
        [DisplayName("Active")]
        public bool active { get; set; }
        [DisplayName("Missing punches")]
        public bool is_missing_punches { get; set; }
        [DisplayName("Total entries")]
        public int total_entries { get; set; }
        [DisplayName("Total hours worked")]
        public Nullable<double> total_hours_worked { get; set; }
        [DisplayName("Total overtime worked")]
        public Nullable<double> total_overtime_worked { get; set; }
        [DisplayName("Pay period")]
        public int pay_period { get; set; }
        [DisplayName("Total PTO used")]
        public Nullable<double> total_pto_used { get; set; }
        [DisplayName("Total unpaid time")]
        public Nullable<double> total_unpaid_time { get; set; }
        [DisplayName("Created on")]
        public System.DateTime created_on { get; set; }
        [DisplayName("Total PTO earned")]
        public double total_pto_earned { get; set; }
        [DisplayName("Total pay earned ($)")]
        public double pay_earned { get; set; }

        public virtual PAY_PERIOD PAY_PERIOD1 { get; set; }
        public virtual USER USER { get; set; }
        public virtual USER USER1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIME_SHEET_ENTRY> TIME_SHEET_ENTRY { get; set; }
    }
}
