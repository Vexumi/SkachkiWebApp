using System.ComponentModel.DataAnnotations.Schema;

public class Horse
{
    public int Id { get; set; }
    public string? Nickname { get; set; }
    public string? Sex { get; set; }
    public string? DOB { get; set; } // date of birth
    public int? HorseOwnerId { get; set; }
    [ForeignKey("HorseOwnerId")]
    public HorseOwner? HorseOwner { get; set; }
}