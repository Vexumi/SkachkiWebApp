using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
public class CompetitionModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
    public int? IppodromId { get; set; }
    [ForeignKey("IppodromId")]
    public IppodromModel? Ippodrom { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Description { get; set; }
    public bool Recruiting { get; set; }

}