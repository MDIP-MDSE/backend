using MDIP_Backend.Models.api;
using MDIP_Backend.Utils;

namespace MDIP_Backend.Models;

public class Context
{

    public Context()
    {
        
    }

    public Context(MMOModel model, string generatedMMOId)
    {
        ContextId = KeyGenerator.getRandomKey();
        Value = model.Context;
        MMOId = generatedMMOId;
    }
    
    public string ContextId { get; set; }
    public string Value { get; set; }
    
    public string MMOId { get; set; }
    public virtual MMO MMO { get; set; }
}