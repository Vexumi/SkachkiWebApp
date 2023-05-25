using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class HorseModel
{
    public int Id { get; set; }
    public string? Nickname { get; set; }
    public string? Sex { get; set; }
    [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? DOB { get; set; } // date of birth
    public int HorseOwnerId { get; set; }
    [ForeignKey("HorseOwnerId")]
    public HorseOwnerModel? HorseOwner { get; set; }
}