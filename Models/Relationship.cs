using MDIP_Backend.Models.api;
using MDIP_Backend.Utils;

namespace MDIP_Backend.Models;

public class Relationship
{

    public Relationship()
    {
        
    }

    public Relationship(RelationshipModel relationshipModel, string generatedMMOId)
    {
        RelationshipId = KeyGenerator.getRandomKey();
        
        RelationshipType = relationshipModel.RelationshipType;
        FirsthandOperator = relationshipModel.FirsthandOperator;
        SecondhandOperator = relationshipModel.SecondhandOperator;
        OperationType = relationshipModel.OperationType;

        MMOId = generatedMMOId;
    }
    
    public string RelationshipId { get; set; }
    public RelationshipType RelationshipType { get; set; }
    
    public string FirsthandOperator { get; set; }
    public string SecondhandOperator { get; set; }
    public string OperationType { get; set; }
    
    public string MMOId { get; set; }
    public virtual MMO MMO { get; set; }
}