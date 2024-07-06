using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Autodissmark.ApplicationDataAccess.Entities;
using Autodissmark.Domain.Enums;

namespace Autodissmark.ApplicationDataAccess.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
{
    public void Configure(EntityTypeBuilder<AuthorEntity> builder)
    {
        builder.Property(e => e.Role)
            .HasConversion(new EnumToStringConverter<Role>());
    }
}