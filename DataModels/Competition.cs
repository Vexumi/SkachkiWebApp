using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
public class Competition
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Date { get; set; }
    public int? IppodromId { get; set; }
    [ForeignKey("IppodromId")]
    public Ippodrom? Ippodrom { get; set; }

}