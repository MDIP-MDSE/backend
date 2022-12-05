using MDIP_Backend.Utils;

namespace MDIP_Backend.Models.api;

public class RelationshipSearchModel
{
    public RelationshipType RelationshipType { get; set; }
    public string FirsthandOperator { get; set; }
    public string SecondhandOperator { get; set; }
    public string OperationType { get; set; }
}