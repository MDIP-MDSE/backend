using System.ComponentModel.DataAnnotations;
using MDIP_Backend.Utils;

namespace MDIP_Backend.Models.api;

public class MMOModel
{

    public MMOModel()
    {
        Relationships = new List<RelationshipModel>();
    }
    
    [Required]
    public MMOType MmoType { get; set; }
    
    [Required] 
    public IFormFile MmoFile { get; set; }
    
    [Required] 
    public string Context { get; set; }
    
    public SemanticModel? Semantic { get; set; }
    
    public List<RelationshipModel>? Relationships { get; set; }
}