using System.ComponentModel;

namespace SkachkiWebApp.Areas.profile.Models
{
    public class JokeyViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [DisplayName("Date of birth")]
        public DateTime? DOB { get; set; } // date of birth 
        public int Rating { get; set; }
        public IEnumerable<CompetitionTicketModel>? Competitions { get; set; }
    }
}
