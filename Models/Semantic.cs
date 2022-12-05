using MDIP_Backend.Models.api;
using MDIP_Backend.Utils;

namespace MDIP_Backend.Models;

public class Semantic
{

    public Semantic()
    {
        
    }

    public Semantic(SemanticModel model, string generatedMMOId)
    {
        SemanticId = KeyGenerator.getRandomKey();
        
        Entity = model.Entity;
        Location = model.Location;
        Date = model.Date;
        Object = model.Object;
        Time = model.Time;
        Event = model.Event;
        
        MMOId = generatedMMOId;
    }
    
    public string SemanticId { get; set; }
    public string? Entity { get; set; }
    public string? Location { get; set; }
    public string? Date { get; set; }
    public string? Object { get; set; }
    public string? Time { get; set; }
    public string? Event { get; set; }
    
    public string MMOId { get; set; }
    public virtual MMO MMO { get; set; }
}