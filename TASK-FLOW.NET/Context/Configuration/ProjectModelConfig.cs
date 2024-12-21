using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TASK_FLOW.NET.Project.Model;

namespace TASK_FLOW.NET.Context.Configuration
{
    public class ProjectModelConfig : IEntityTypeConfiguration<ProjectModel>
    {
        public void Configure(EntityTypeBuilder<ProjectModel> builder)
        {
            builder.HasKey(row => row.Id);
            builder.Property(row => row.Id).UseIdentityColumn().ValueGeneratedOnAdd();

            builder.Property(r => r.Name);
            builder.Property(r => r.Description);

            builder.HasMany(e => e.UsersIncludes)
            .WithOne(p => p.Project)
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Tasks)
            .WithOne(p => p.Project)
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Project");
        }
    }
}
