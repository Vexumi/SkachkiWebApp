namespace SkachkiWebApp.Areas.profile.Models
{
    public class HownerViewModel
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public IEnumerable<HorseModel>? Horses { get; set; }
        public IEnumerable<CompetitionTicketModel>? Competitions { get; set; }
    }
}
