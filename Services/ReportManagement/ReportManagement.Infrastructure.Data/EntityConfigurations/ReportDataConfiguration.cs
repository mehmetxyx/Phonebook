using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportManagement.Infrastructure.Data.Entities;

namespace ContactManagement.Infrastructure.Data.EntityConfigurations;

public class ReportDataConfiguration : IEntityTypeConfiguration<ReportDataEntity>
{
    public void Configure(EntityTypeBuilder<ReportDataEntity> builder)
    {
        builder.ToTable("report_data");

        builder.HasKey(cd => cd.Id);
       
        builder.HasIndex(cd => cd.Id)
            .IsUnique();

        builder.HasIndex(cd => new {cd.ReportId, cd.Id})
            .IsUnique();

        builder.Property(cd => cd.Id)
            .IsRequired();

        builder.Property(cd => cd.ReportId)
            .IsRequired();

        builder.Property(cd => cd.Location)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(cd => cd.ContactCount)
            .IsRequired();

        builder.Property(cd => cd.PhoneNumberCount)
            .IsRequired();
    }
}