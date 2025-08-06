using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportManagement.Infrastructure.Data.Entities;

namespace ContactManagement.Infrastructure.Data.EntityConfigurations;

public class ContactConfiguration: IEntityTypeConfiguration<ReportEntity>
{
    public void Configure(EntityTypeBuilder<ReportEntity> builder)
    {
        builder.ToTable("reports");

        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.Id)
            .IsUnique();

        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.RequestDate)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasMany(r => r.ReportData)
            .WithOne(rd => rd.Report)
            .HasForeignKey(rd => rd.ReportId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}