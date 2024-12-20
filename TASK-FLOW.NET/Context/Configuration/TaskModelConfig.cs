using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TASK_FLOW.NET.Task.Enums;
using TASK_FLOW.NET.Task.Model;
using TASK_FLOW.NET.Utils.Helpers;

namespace TASK_FLOW.NET.Context.Configuration
{
    public class TaskModelConfig : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.HasKey(row => row.Id);
            builder.Property(row => row.Id).UseIdentityColumn().ValueGeneratedOnAdd();

            builder.Property(row => row.Name);
            builder.Property(row => row.Descritpion);

            builder.Property(row => row.Status).HasConversion(
                EnumConversionHelper.EnumConversion<STATUSTASK>()
                ).IsUnicode(false);

            builder.Property(row => row.ResponsibleName);

            builder.ToTable("Task");
        }
    }
}
