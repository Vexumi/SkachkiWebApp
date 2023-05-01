using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
public class CompetitionTicketModel

{
    public int TiketId { get; set; }
    public int CompetitionId { get; set; }
    [ForeignKey("CompetitionId")]
    public CompetitionModel? Competition { get; set; }
    public int HorseId { get; set; }
    [ForeignKey("HorseId")]
    public HorseModel? Horse { get; set; }
    public int JokeyId { get; set; }
    [ForeignKey("JokeyId")]
    public JokeyModel? Jokey { get; set; }
    public string? Result { get; set; }
}