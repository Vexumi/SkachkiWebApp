using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

public class HorseModel
{
    public int Id { get; set; }
    public string? Nickname { get; set; }
    public string? Sex { get; set; }
    [DisplayName("Date Of Birth")]
    public DateOnly? DOB { get; set; } // date of birth
    public int? HorseOwnerId { get; set; }
    [ForeignKey("HorseOwnerId")]
    public HorseOwnerModel? HorseOwner { get; set; }
}