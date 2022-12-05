using System.Text.Json;
using MDIP_Backend.Models;
using MDIP_Backend.Models.api;
using MDIP_Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace MDIP_Backend.Controllers;

[ApiController]
[Route("mmo")]
public class MMOController : ControllerBase
{
    private readonly ILogger<MMOController> _logger;
    private MMOContext _context;
    
    public MMOController(ILogger<MMOController> logger, MMOContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost("upload")]
    public async Task<ActionResult> upload([FromForm] MMOModel model)
    {
        var dbMMO = await _context.MMOS.AddAsync(new MMO(model));
        var dbContext = await _context.Contexts.AddAsync(new Context(model, dbMMO.Entity.MMOId));
        
        if(model.Semantic != null)
            await _context.Semantics.AddAsync(new Semantic(model.Semantic, dbMMO.Entity.MMOId));

        if(model.Relationships != null)
            foreach (var modelRelationship in model.Relationships)
                await _context.Relationships.AddAsync(new Relationship(modelRelationship, dbMMO.Entity.MMOId));
        
        await _context.SaveChangesAsync();

        var updatedMMO = await _context.MMOS.Include(mmo => mmo.Context).Include(mmo => mmo.Semantic)
            .Include(mmo => mmo.Relationships)
            .FirstAsync(mmo => mmo.MMOId == dbMMO.Entity.MMOId);
        
        return Ok(new
        { 
            message = "Successfully inserted the MMO to the database.",
            code = Ok().StatusCode,
            data = new {mmo = updatedMMO}
        });
    }
    
    [HttpGet("query/data")]
    public async Task<ActionResult> getQueryData()
    {
        var dbContexts = await _context.Contexts.Select(context => context.Value).Distinct().ToListAsync();
        
        var dbSemantics = await _context.Semantics.ToListAsync();
        
        var entity = dbSemantics.Select(semantic => semantic.Entity).Where(entity => entity != null).Distinct().ToList();
        var location = dbSemantics.Select(semantic => semantic.Location).Where(location => location != null).Distinct().ToList();
        var objectSemantic = dbSemantics.Select(semantic => semantic.Object).Where(objectSem => objectSem != null).Distinct().ToList();
        var eventSemantic = dbSemantics.Select(semantic => semantic.Event).Where(eventSem => eventSem != null).Distinct().ToList();

        var operationTypes = await _context.Relationships.Select(rel => rel.OperationType).Distinct().ToListAsync();
        
        return Ok(new
        { 
            message = "Successfully fetched query data.",
            code = Ok().StatusCode,
            data = new {contexts = dbContexts, entities = entity, locations = location, objects = objectSemantic, events = eventSemantic, operations = operationTypes}
        });
    }
    
    [HttpPost("search")]
    public async Task<ActionResult> search([FromBody] MMOSearchModel model)
    {
        IQueryable<MMO> queryableMMO = _context.MMOS.Include(mmo => mmo.Context).Include(mmo => mmo.Semantic)
            .Include(mmo => mmo.Relationships);

        if (model.MmoType != null)
            queryableMMO = queryableMMO.Where(mmo => mmo.MMOType == model.MmoType);

        if (!String.IsNullOrEmpty(model.Extension))
            queryableMMO = queryableMMO.Where(mmo => mmo.Extension == model.Extension);
        
        if (!String.IsNullOrEmpty(model.Context))
            queryableMMO = queryableMMO.Where(mmo => mmo.Context.Value == model.Context);

        if (model.SemanticSearchModel != null)
        {
            if (!String.IsNullOrEmpty(model.SemanticSearchModel.Date))
                queryableMMO = queryableMMO.Where(mmo => mmo.Semantic.Date == model.SemanticSearchModel.Date);
            
            if (!String.IsNullOrEmpty(model.SemanticSearchModel.Entity))
                queryableMMO = queryableMMO.Where(mmo => mmo.Semantic.Entity == model.SemanticSearchModel.Entity);
            
            if (!String.IsNullOrEmpty(model.SemanticSearchModel.Event))
                queryableMMO = queryableMMO.Where(mmo => mmo.Semantic.Event == model.SemanticSearchModel.Event);
            
            if (!String.IsNullOrEmpty(model.SemanticSearchModel.Location))
                queryableMMO = queryableMMO.Where(mmo => mmo.Semantic.Location == model.SemanticSearchModel.Location);
            
            if (!String.IsNullOrEmpty(model.SemanticSearchModel.Object))
                queryableMMO = queryableMMO.Where(mmo => mmo.Semantic.Object == model.SemanticSearchModel.Object);
            
            if (!String.IsNullOrEmpty(model.SemanticSearchModel.Time))
                queryableMMO = queryableMMO.Where(mmo => mmo.Semantic.Time == model.SemanticSearchModel.Time);
        }

        if (model.RelationshipSearchModels != null && model.RelationshipSearchModels.Count != 0)
        {
            foreach (var modelRelationshipSearchModel in model.RelationshipSearchModels)
            {
                queryableMMO = queryableMMO.Where(mmo => mmo.Relationships.Any(dbRelationship =>
                    dbRelationship.FirsthandOperator == modelRelationshipSearchModel.FirsthandOperator && dbRelationship.SecondhandOperator == modelRelationshipSearchModel.SecondhandOperator
                    && dbRelationship.OperationType == modelRelationshipSearchModel.OperationType && dbRelationship.RelationshipType == modelRelationshipSearchModel.RelationshipType));
            }
        }

        var dbMMOs = await queryableMMO.ToListAsync();
        
        return Ok(new
        { 
            message = "Successfully fetched the requested query.",
            code = Ok().StatusCode,
            data = dbMMOs
        });
    }

}