using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TASK_FLOW.NET.User.Model;
using TASK_FLOW.NET.User.Enums;
using TASK_FLOW.NET.Utils.Helpers;


namespace TASK_FLOW.NET.Context.Configuration
{
    public class UserModelConfig : IEntityTypeConfiguration<UsersModel>
    {
        public void Configure(EntityTypeBuilder<UsersModel> builder)
        {
            builder.HasKey(row => row.Id);
            builder.Property(row => row.Id).UseIdentityColumn().ValueGeneratedOnAdd();

            builder.Property(row => row.First_name).HasMaxLength(100);
            builder.Property(row => row.Last_name).HasMaxLength(100);
            builder.Property(row => row.Age).HasMaxLength(100);

            builder.Property(row => row.Username).HasMaxLength(100).IsUnicode();
            builder.Property(row => row.Email).HasMaxLength(100).IsUnicode();
            builder.Property(row => row.Password);

            builder.Property(row => row.Roles).HasConversion(
                EnumConversionHelper.EnumConversion<ROLES>()
                ).HasMaxLength(20).IsUnicode(false);

            builder.Property(row => row.RefreshToken).IsRequired(false);

            builder.HasMany(e => e.ProjectIncludes)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("User");
        }
    }
}
