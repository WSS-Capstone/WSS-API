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
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Combo> Combos { get; set; } = null!;
        public virtual DbSet<ComboService> ComboServices { get; set; } = null!;
        public virtual DbSet<Commission> Commissions { get; set; } = null!;
        public virtual DbSet<CurrentPrice> CurrentPrices { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Owner> Owners { get; set; } = null!;
        public virtual DbSet<Partner> Partners { get; set; } = null!;
        public virtual DbSet<PartnerPaymentHistory> PartnerPaymentHistories { get; set; } = null!;
        public virtual DbSet<PartnerService> PartnerServices { get; set; } = null!;
        public virtual DbSet<PaymentHistory> PaymentHistories { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServiceImage> ServiceImages { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;
        public virtual DbSet<WeddingInformation> WeddingInformations { get; set; } = null!;
        public virtual DbSet<staff> staff { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=20.189.117.242;Database=WSS;User Id=sa;Password=29327Cab@456789;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RefId).IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_Cart_Service");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Cart_Customer");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.InverseCategoryNavigation)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<Combo>(entity =>
            {
                entity.ToTable("Combo");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<ComboService>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Combo)
                    .WithMany(p => p.ComboServices)
                    .HasForeignKey(d => d.ComboId)
                    .HasConstraintName("FK_ComboServices_Combo");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ComboServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_ComboServices_Service");
            });

            modelBuilder.Entity<Commission>(entity =>
            {
                entity.ToTable("Commission");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateOfApply).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Commissions)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Commission_Category");
            });

            modelBuilder.Entity<CurrentPrice>(entity =>
            {
                entity.ToTable("CurrentPrice");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateOfApply).HasColumnType("date");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CurrentPrices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_CurrentPrice_Service");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Fullname).IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Account");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.OrderDetailId)
                    .HasConstraintName("FK_Feedback_OrderDetail");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Feedback_Customer");
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

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Combo)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ComboId)
                    .HasConstraintName("FK_Order_Combo");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_Order_Owner");

                entity.HasOne(d => d.WeddingInformation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.WeddingInformationId)
                    .HasConstraintName("FK_Order_WeddingInformation");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.Id).ValueGeneratedNever();

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

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.ToTable("Owner");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Owner)
                    .HasForeignKey<Owner>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Owner_Account");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("Partner");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Fullname).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Partner)
                    .HasForeignKey<Partner>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Partner_Account");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Partners)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Partner_Role");
            });

            modelBuilder.Entity<PartnerPaymentHistory>(entity =>
            {
                entity.ToTable("PartnerPaymentHistory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PartnerPaymentHistories)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_PartnerPaymentHistory_Order");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerPaymentHistories)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_PartnerPaymentHistory_Partner");
            });

            modelBuilder.Entity<PartnerService>(entity =>
            {
                entity.ToTable("PartnerService");

                entity.HasIndex(e => e.PartnerId, "IX_PartnerService")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Partner)
                    .WithOne(p => p.PartnerService)
                    .HasForeignKey<PartnerService>(d => d.PartnerId)
                    .HasConstraintName("FK_PartnerService_Partner");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.PartnerServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_PartnerService_Service");
            });

            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.ToTable("PaymentHistory");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PaymentHistories)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_PaymentHistory_Order");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(255);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.Categoryid)
                    .HasConstraintName("FK_Service_Category");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK_Service_Owner");
            });

            modelBuilder.Entity<ServiceImage>(entity =>
            {
                entity.ToTable("ServiceImage");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceImages)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_ServiceImage_Service");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.HasIndex(e => e.StaffId, "IX_Task")
                    .IsUnique();

                entity.HasIndex(e => e.PartnerId, "IX_Task_1")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StaffId).IsRequired();

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Task_Owner");

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.OrderDetailId)
                    .HasConstraintName("FK_Task_OrderDetail");

                entity.HasOne(d => d.Partner)
                    .WithOne(p => p.Task)
                    .HasForeignKey<Task>(d => d.PartnerId)
                    .HasConstraintName("FK_Task_Partner");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Voucher");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_Voucher_Owner");
            });

            modelBuilder.Entity<WeddingInformation>(entity =>
            {
                entity.ToTable("WeddingInformation");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.WeddingDay).HasColumnType("datetime");
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.RoleId });

                entity.ToTable("Staff");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Account");

                entity.HasOne(d => d.Id1)
                    .WithMany(p => p.staff)
                    .HasPrincipalKey(p => p.StaffId)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Task");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
