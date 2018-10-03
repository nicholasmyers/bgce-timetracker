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
    
    public partial class TIME_SHEET
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIME_SHEET()
        {
            this.TIME_SHEET_ENTRY = new HashSet<TIME_SHEET_ENTRY>();
        }
    
        public int timesheetID { get; set; }
        public int employee { get; set; }
        public Nullable<System.DateTime> submitted_on { get; set; }
        public Nullable<System.DateTime> approved_on { get; set; }
        public Nullable<int> approved_by { get; set; }
        public byte[] submitted { get; set; }
        public byte[] approved { get; set; }
        public string comments { get; set; }
        public byte[] active { get; set; }
        public byte[] is_missing_punches { get; set; }
        public int total_entries { get; set; }
        public Nullable<double> total_hours_worked { get; set; }
        public Nullable<double> total_overtime_worked { get; set; }
        public int pay_period { get; set; }
        public Nullable<double> total_pto_used { get; set; }
        public Nullable<double> total_unpaid_time { get; set; }
        public System.DateTime created_on { get; set; }
    
        public virtual PAY_PERIOD PAY_PERIOD1 { get; set; }
        public virtual USER USER { get; set; }
        public virtual USER USER1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIME_SHEET_ENTRY> TIME_SHEET_ENTRY { get; set; }
    }
}