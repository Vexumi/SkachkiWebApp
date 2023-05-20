using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class JokeyModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? DOB { get; set; } // date of birth 
    public int Rating { get; set; }

}