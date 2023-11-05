using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WSS.API.Data.Models
{
    public partial class WSSContext : DbContext
    {
        public WSSContext()
        {
        }

        public WSSContext(DbContextOptions<WSSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Combo> Combos { get; set; } = null!;
        public virtual DbSet<ComboService> ComboServices { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Commission> Commissions { get; set; } = null!;
        public virtual DbSet<Config> Configs { get; set; } = null!;
        public virtual DbSet<CurrentPrice> CurrentPrices { get; set; } = null!;
        public virtual DbSet<DayOff> DayOffs { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<PartnerPaymentHistory> PartnerPaymentHistories { get; set; } = null!;
        public virtual DbSet<PaymentHistory> PaymentHistories { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServiceImage> ServiceImages { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<TaskOrderDetail> TaskOrderDetails { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;
        public virtual DbSet<WeddingInformation> WeddingInformations { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=20.189.117.242;Database=WSS;User Id=sa;Password=29327Cab@456789;TrustServerCertificate=True;Connect Timeout=120");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.RefId).IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.IsOrderLimit).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Commision)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.CommisionId)
                    .HasConstraintName("FK_Category_Commission");
            });

            modelBuilder.Entity<Combo>(entity =>
            {
                entity.ToTable("Combo");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ComboService>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Combo)
                    .WithMany(p => p.ComboServices)
                    .HasForeignKey(d => d.ComboId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ComboServices_Combo");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ComboServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_ComboServices_Service");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Comment_User");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_Comment_Task");
            });

            modelBuilder.Entity<Commission>(entity =>
            {
                entity.ToTable("Commission");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfApply).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("Config_pk");

                entity.ToTable("Config");

                entity.Property(e => e.Key).HasMaxLength(50);
            });

            modelBuilder.Entity<CurrentPrice>(entity =>
            {
                entity.ToTable("CurrentPrice");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfApply).HasColumnType("date");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CurrentPrices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("CurrentPrice_Service_Id_fk");
            });

            modelBuilder.Entity<DayOff>(entity =>
            {
                entity.ToTable("DayOff");

                entity.HasIndex(e => e.PartnerId, "IX_PartnerService");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Day).HasColumnType("datetime");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.DayOffs)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_DayOff_User");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.DayOffs)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_DayOff_Service");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Feedback_User");

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.OrderDetailId)
                    .HasConstraintName("FK_Feedback_OrderDetail");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Combo)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ComboId)
                    .HasConstraintName("FK_Order_Combo");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Order_User");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.VoucherId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Order_Voucher1");

                entity.HasOne(d => d.WeddingInformation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.WeddingInformationId)
                    .HasConstraintName("FK_Order_WeddingInformation");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_OrderDetail_Service");
            });

            modelBuilder.Entity<PartnerPaymentHistory>(entity =>
            {
                entity.ToTable("PartnerPaymentHistory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PartnerPaymentHistories)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_PartnerPaymentHistory_Order");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerPaymentHistories)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_PartnerPaymentHistory_User");
            });

            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.ToTable("PaymentHistory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PaymentHistories)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_PaymentHistory_Order");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Service_Category");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("Service_Account_Id_fk");
            });

            modelBuilder.Entity<ServiceImage>(entity =>
            {
                entity.ToTable("ServiceImage");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceImages)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ServiceImage_Service");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.HasIndex(e => e.PartnerId, "IX_Task");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TaskCreateByNavigations)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Task_Owner");

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.OrderDetailId)
                    .HasConstraintName("FK_Task_OrderDetail");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.TaskPartners)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_Task_User1");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.TaskStaffs)
                    .HasForeignKey(d => d.StaffId)
                    .HasConstraintName("FK_Task_User");
            });

            modelBuilder.Entity<TaskOrderDetail>(entity =>
            {
                entity.ToTable("TaskOrderDetail");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.TaskOrderDetails)
                    .HasForeignKey(d => d.OrderDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TaskOrderDetail_OrderDetail_Id_fk");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TaskOrderDetails)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TaskOrderDetail_Task_Id_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_User_Category");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Owner_Account");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Voucher");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Voucher_Owner");
            });

            modelBuilder.Entity<WeddingInformation>(entity =>
            {
                entity.ToTable("WeddingInformation");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.WeddingDay).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
