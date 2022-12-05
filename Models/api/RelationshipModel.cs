using System.ComponentModel.DataAnnotations;
using MDIP_Backend.Utils;

namespace MDIP_Backend.Models.api;

public class RelationshipModel
{
    [Required]
    public RelationshipType RelationshipType { get; set; }
    
    [Required]
    public string FirsthandOperator { get; set; }
    
    [Required]
    public string SecondhandOperator { get; set; }
    
    [Required]
    public string OperationType { get; set; }
}