using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.Utils.Helpers;

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
