using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TASK_FLOW.NET.Utils.Helpers;
using TASK_FLOW.NET.UserProject.Model;
using TASK_FLOW.NET.UserProject.Enums;

namespace TASK_FLOW.NET.Context.Configuration
{
    public class UserProjectModelConfig : IEntityTypeConfiguration<UserProjectModel>
    {
        public void Configure(EntityTypeBuilder<UserProjectModel> builder)
        {
            builder.HasKey(row => row.Id);
            builder.Property(row => row.Id).UseIdentityColumn().ValueGeneratedOnAdd();

            builder.Property(row => row.AccessLevel).HasConversion(
                EnumConversionHelper.EnumConversion<ACCESSLEVEL>()
                ).IsUnicode(false);

            builder.ToTable("UserProject");
        }
    }
}
