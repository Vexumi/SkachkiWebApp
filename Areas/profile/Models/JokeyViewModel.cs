using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkachkiWebApp.Areas.profile.Models
{
    public class JokeyViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; } // date of birth
        public string Email { get; set; }
        public int Rating { get; set; }
        public IEnumerable<CompetitionTicketModel>? Competitions { get; set; }
    }
}
