using CRUDENTITY.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDENTITY.DataContext
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<WebLinkTempEntity> WebLinkTemp { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<RoleWisePagePermission> RoleWisePagePermissions { get; set; }
        public DbSet<RolePagePermission> RolePagePermission { get; set; }
        public DbSet<PageModuleAction> PageModuleAction { get; set; }
        public DbSet<StudentAppointment> StudentAppointment { get; set; }
        public DbSet<StudentAppointmentUploadLog> StudentAppointmentUploadLog { get; set; }
        public DbSet<OtpVerification> OtpVerification { get; set; }


        //public DbSet<State> State { get; set; }
        //public DbSet<District> District { get; set; }
        //public DbSet<SubDistrict> SubDistrict { get; set; }
        //public DbSet<Block> Block { get; set; }
        //public DbSet<Village> Village { get; set; }

        public DbSet<WebsiteVisitorLog> WebsiteVisitorLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entity mappings here
            modelBuilder.Entity<RoleWisePagePermission>().HasNoKey();


            //// Primary key
            //modelBuilder.Entity<State>()
            //    .HasKey(s => s.Id);

            //// Primary key
            //modelBuilder.Entity<District>()
            //    .HasKey(d => d.ID);
            //modelBuilder.Entity<State>()
            //            .HasAlternateKey(s => s.State_Code);

            //modelBuilder.Entity<District>()
            //            .HasAlternateKey(s => s.District_Code);

            //// Configure FK relationship on State_Code (non-PK)
            //modelBuilder.Entity<IFRMonthlyImpleRPT>()
            //    .HasOne(rpt => rpt.State)
            //    .WithMany()
            //    .HasForeignKey(rpt => rpt.State_Code)
            //    .HasPrincipalKey(s => s.State_Code);


            //// Configure FK relationship on State_Code (non-PK)
            //modelBuilder.Entity<IFRMonthlyImpleRPT>()
            //    .HasOne(rpt => rpt.District)
            //    .WithMany()
            //    .HasForeignKey(rpt => rpt.District_Code)
            //    .HasPrincipalKey(s => s.District_Code);

            modelBuilder.Entity<NotificationDataEntity>().HasNoKey();
        }
    }
}
