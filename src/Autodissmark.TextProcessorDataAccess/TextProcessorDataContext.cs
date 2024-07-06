using Autodissmark.TextProcessorDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Autodissmark.TextProcessorDataAccess;

public class TextProcessorDataContext : DbContext
{
    public DbSet<ArtistEntity> Artists { get; set; }
    public DbSet<ArtistTextEntity> ArtistTexts { get; set; }
    public DbSet<DictionaryEntity> Dictionaries { get; set; }
    public DbSet<DictionaryWordEntity> DictionaryWords { get; set; }
    public DbSet<TextBaseArtistEntity> TextBaseArtists { get; set; }
    public DbSet<TextBaseDictionaryEntity> TextBaseDicitonaries{ get; set; }
    public DbSet<TextBaseEntity> TextBases { get; set; }

    public TextProcessorDataContext(DbContextOptions<TextProcessorDataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
