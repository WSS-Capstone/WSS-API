﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WSS.API.Data.Models;

public partial class WssContext : DbContext
{
    public WssContext()
    {
    }

    public WssContext(DbContextOptions<WssContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<ComboService> ComboServices { get; set; }

    public virtual DbSet<Commission> Commissions { get; set; }

    public virtual DbSet<CurrentPrice> CurrentPrices { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<PartnerPaymentHistory> PartnerPaymentHistories { get; set; }

    public virtual DbSet<PartnerService> PartnerServices { get; set; }

    public virtual DbSet<PaymentHistory> PaymentHistories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceImage> ServiceImages { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<WeddingInformation> WeddingInformations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=20.189.117.242;Database=WSS;User Id=sa;Password=29327Cab@456789;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RefId).IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Service).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_Cart_Service");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.InverseCategoryNavigation)
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
        });

        modelBuilder.Entity<Commission>(entity =>
        {
            entity.ToTable("Commission");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateOfApply).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Commissions)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Commission_Category");
        });

        modelBuilder.Entity<CurrentPrice>(entity =>
        {
            entity.ToTable("CurrentPrice");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateOfApply).HasColumnType("date");

            entity.HasOne(d => d.Service).WithMany(p => p.CurrentPrices)
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

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_Account");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.ToTable("Feedback");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
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

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Order_Customer");

            entity.HasOne(d => d.Owner).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_Order_Owner");

            entity.HasOne(d => d.WeddingInformation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.WeddingInformationId)
                .HasConstraintName("FK_Order_WeddingInformation");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.ToTable("Owner");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DataOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Owner)
                .HasForeignKey<Owner>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Owner_Account");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.ToTable("Partner");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DataOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Fullname).IsUnicode(false);
            entity.Property(e => e.Phone).IsUnicode(false);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Partner)
                .HasForeignKey<Partner>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Partner_Account");

            entity.HasOne(d => d.Role).WithMany(p => p.Partners)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Partner_Role");
        });

        modelBuilder.Entity<PartnerPaymentHistory>(entity =>
        {
            entity.ToTable("PartnerPaymentHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Partner).WithMany(p => p.PartnerPaymentHistories)
                .HasForeignKey(d => d.PartnerId)
                .HasConstraintName("FK_PartnerPaymentHistory_Partner");
        });

        modelBuilder.Entity<PartnerService>(entity =>
        {
            entity.ToTable("PartnerService");

            entity.HasIndex(e => e.PartnerId, "IX_PartnerService").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Partner).WithOne(p => p.PartnerService)
                .HasForeignKey<PartnerService>(d => d.PartnerId)
                .HasConstraintName("FK_PartnerService_Partner");

            entity.HasOne(d => d.Service).WithMany(p => p.PartnerServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_PartnerService_Service");
        });

        modelBuilder.Entity<PaymentHistory>(entity =>
        {
            entity.ToTable("PaymentHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
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

            entity.HasOne(d => d.Category).WithMany(p => p.Services)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("FK_Service_Category");

            entity.HasOne(d => d.Owner).WithMany(p => p.Services)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_Service_Owner");
        });

        modelBuilder.Entity<ServiceImage>(entity =>
        {
            entity.ToTable("ServiceImageRepo");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceImages)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_ServiceImage_Service");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.RoleId });

            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).IsUnicode(false);
            entity.Property(e => e.Phone).IsUnicode(false);

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_Account");

            entity.HasOne(d => d.Id1).WithMany(p => p.Staff)
                .HasPrincipalKey(p => p.StaffId)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_Task");

            entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staff_Role");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("Task");

            entity.HasIndex(e => e.StaffId, "IX_Task").IsUnique();

            entity.HasIndex(e => e.PartnerId, "IX_Task_1").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StaffId).IsRequired();
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CreateBy)
                .HasConstraintName("FK_Task_Owner");

            entity.HasOne(d => d.Partner).WithOne(p => p.Task)
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
        });

        modelBuilder.Entity<WeddingInformation>(entity =>
        {
            entity.ToTable("WeddingInformation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.WeddingDay).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
