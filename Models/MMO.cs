using MDIP_Backend.Models.api;
using MDIP_Backend.Utils;

namespace MDIP_Backend.Models;

public class MMO
{
    public MMO()
    {
        Relationships = new List<Relationship>();
    }

    public MMO(MMOModel model)
    {
        MMOId = KeyGenerator.getRandomKey();
        MMOType = model.MmoType;
        Extension = Path.GetExtension(model.MmoFile.FileName);
        
        MemoryStream ms = new MemoryStream();
        model.MmoFile.CopyTo(ms);
        
        MMOData = Convert.ToBase64String(ms.ToArray());
    }
    
    public string MMOId { get; set; }
    public MMOType MMOType { get; set; }
    
    public String Extension { get; set; }
    
    public String MMOData { get; set; }
    
    public Context Context { get; set; }
    public Semantic Semantic { get; set; }
    
    public virtual ICollection<Relationship> Relationships { get; set; }
}