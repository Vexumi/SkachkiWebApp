using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
public class CompetitionTicket

{
    public int TiketId { get; set; }
    public int CompetitionId { get; set; }
    [ForeignKey("CompetitionId")]
    public Competition? Competition { get; set; }
    public int HorseId { get; set; }
    [ForeignKey("HorseId")]
    public Horse? Horse { get; set; }
    public int JokeyId { get; set; }
    [ForeignKey("JokeyId")]
    public Jokey? Jokey { get; set; }
    public string? Result { get; set; }
}