using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using localMarketingSystem.DAL.Entities;

namespace localMarketingSystem.DAL;

public partial class localMarketingSystemDBContext : DbContext
{
    public localMarketingSystemDBContext()
    {
    }

    public localMarketingSystemDBContext(DbContextOptions<localMarketingSystemDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MCategoryList> MCategoryLists { get; set; }

    public virtual DbSet<MProductList> MProductLists { get; set; }

    public virtual DbSet<MSubCategoryList> MSubCategoryLists { get; set; }

    public virtual DbSet<MUser> MUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DBConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MCategoryList>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("category_list_pkey");

            entity.Property(e => e.CategoryId).ValueGeneratedNever();
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('admin.category_list_id_seq'::regclass)");

            entity.HasOne(d => d.Product).WithMany(p => p.MCategoryLists).HasConstraintName("category_list_product_id_fkey");
        });

        modelBuilder.Entity<MProductList>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("m_product_list_pkey");

            entity.Property(e => e.ProductId).ValueGeneratedNever();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<MSubCategoryList>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId).HasName("sub_category_list_pkey");

            entity.Property(e => e.SubCategoryId).ValueGeneratedNever();
            entity.Property(e => e.Id).HasDefaultValueSql("nextval('admin.sub_category_list_id_seq'::regclass)");

            entity.HasOne(d => d.Category).WithMany(p => p.MSubCategoryLists)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sub_category_list_category_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.MSubCategoryLists).HasConstraintName("sub_category_list_product_id_fkey");
        });

        modelBuilder.Entity<MUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("m_users_pkey");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MobileNo).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
