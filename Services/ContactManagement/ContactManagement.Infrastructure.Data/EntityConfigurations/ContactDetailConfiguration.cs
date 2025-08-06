using ContactManagement.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactManagement.Infrastructure.Data.EntityConfigurations;

public class ContactDetailConfiguration : IEntityTypeConfiguration<ContactDetailEntity>
{
    public void Configure(EntityTypeBuilder<ContactDetailEntity> builder)
    {
        builder.ToTable("contact_details");

        builder.HasKey(cd => cd.Id);
       
        builder.HasIndex(cd => cd.Id)
            .IsUnique();

        builder.HasIndex(cd => new {cd.ContactId, cd.Id})
            .IsUnique();

        builder.Property(cd => cd.Id)
            .IsRequired();

        builder.Property(cd => cd.ContactId)
            .IsRequired();

        builder.Property(cd => cd.Type)
            .HasMaxLength(50)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(cd => cd.Value)
            .HasMaxLength(100)
            .IsRequired();
    }
}