using ContactManagement.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactManagement.Infrastructure.Data.EntityConfigurations;
public class ContactConfiguration: IEntityTypeConfiguration<ContactEntity>
{
    public void Configure(EntityTypeBuilder<ContactEntity> builder)
    {
        builder.ToTable("contacts");

        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.Id)
            .IsUnique();

        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Surname)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Company)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(c => c.ContactDetails)
            .WithOne(c => c.Contact)
            .HasForeignKey(cd => cd.ContactId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}