using MDIP_Backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MDIP_Backend.Models;

public class MMOContext : DbContext
{
    public MMOContext(DbContextOptions<MMOContext> options)
        : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var mmoTypeConverter = new EnumToStringConverter<MMOType>();
        var relationshipTypeConverter = new EnumToStringConverter<RelationshipType>();
        
        modelBuilder
            .Entity<MMO>()
            .Property(e => e.MMOType)
            .HasConversion(mmoTypeConverter);
        
        modelBuilder
            .Entity<Relationship>()
            .Property(e => e.RelationshipType)
            .HasConversion(relationshipTypeConverter);
        
        modelBuilder.Entity<Relationship>()
            .HasOne(s => s.MMO)
            .WithMany(s => s.Relationships)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Semantic>()
            .HasOne(s => s.MMO)
            .WithOne(s => s.Semantic)
            .HasForeignKey<Semantic>(s => s.MMOId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Context>()
            .HasOne(s => s.MMO)
            .WithOne(s => s.Context)
            .HasForeignKey<Context>(c => c.MMOId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public DbSet<MMO> MMOS { get; set; }
    public DbSet<Context> Contexts { get; set; }
    public DbSet<Semantic> Semantics { get; set; }
    public DbSet<Relationship> Relationships { get; set; }
}