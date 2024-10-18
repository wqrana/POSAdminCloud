using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.ViewModels
{
    public class GraduateSeniorsViewModel
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public Nullable<bool> Graduate { get; set; }
        public double? MBalance { get; set; }
        public double? ABalance { get; set; }
        public double? TotalBalance { get; set; }
        public string Grade { get; set; }
        public string SchoolName { get; set; }
    }
    public class GraduateSeniorsFilters
    {
        public string SearchString { get; set; }
        public string SchoolFilter { get; set; }
        public string GradeFilter { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }

    public class GraduateSeniorsDistinctGradesViewModel
    {
        public long? GradeId { get; set; }
        public string GradeName { get; set; }
    }

    public class GraduateSeniorsDistinctSchoolsViewModel
    {
        public long? SchoolId { get; set; }
        public string SchoolName { get; set; }
    }
}
