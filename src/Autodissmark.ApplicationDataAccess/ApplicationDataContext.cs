using Autodissmark.ApplicationDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace Autodissmark.ApplicationDataAccess;

public class ApplicationDataContext : DbContext
{
    public DbSet<AcapellaEntity> Acapellas { get; set; }
    public DbSet<ArtistEntity> Artists { get; set; }
    public DbSet<AuthorEntity> Authors { get; set; }
    public DbSet<BeatEntity> Beats { get; set; }
    public DbSet<DissAcapellaEntity> DissAcapellas { get; set; }
    public DbSet<DissEntity> Disses { get; set; }
    public DbSet<TextEntity> Texts { get; set; }
    public DbSet<VoiceEntity> Voices { get; set; }

    public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}