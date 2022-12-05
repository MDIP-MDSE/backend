using MDIP_Backend.Utils;

namespace MDIP_Backend.Models.api;

public class MMOSearchModel
{
    public MMOType? MmoType { get; set; }
    public String? Extension { get; set; }
    
    public String? Context { get; set; }
    
    public SemanticSearchModel? SemanticSearchModel { get; set; }
    public ICollection<RelationshipSearchModel>? RelationshipSearchModels { get; set; }

    public bool isObjectNullable()
    {
        return GetType().GetProperties().All(prop => prop.GetValue(this) == null);
    }
}