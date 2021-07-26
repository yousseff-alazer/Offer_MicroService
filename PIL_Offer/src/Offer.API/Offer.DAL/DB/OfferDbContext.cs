﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Offer.API.Offer.DAL.DB
{
    public partial class OfferDbContext : DbContext
    {
        public OfferDbContext()
        {
        }

        public OfferDbContext(DbContextOptions<OfferDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<OfferUser> OfferUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=OfferDb;Username=admin;Password=admin1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.ToTable("offer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdby).HasColumnName("createdby");

                entity.Property(e => e.Creationdate)
                    .HasColumnName("creationdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Discount)
                    .HasMaxLength(250)
                    .HasColumnName("discount")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Imageurl)
                    .HasMaxLength(250)
                    .HasColumnName("imageurl")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Languageid)
                    .HasMaxLength(250)
                    .HasColumnName("languageid");

                entity.Property(e => e.Maxusagecount).HasColumnName("maxusagecount");

                entity.Property(e => e.Modificationdate).HasColumnName("modificationdate");

                entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Purpose)
                    .HasMaxLength(250)
                    .HasColumnName("purpose")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Usedcount).HasColumnName("usedcount");

                entity.Property(e => e.Validfrom).HasColumnName("validfrom");

                entity.Property(e => e.Validto).HasColumnName("validto");
            });

            modelBuilder.Entity<OfferUser>(entity =>
            {
                entity.ToTable("offer_user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdby).HasColumnName("createdby");

                entity.Property(e => e.Creationdate)
                    .HasColumnName("creationdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Modificationdate).HasColumnName("modificationdate");

                entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");

                entity.Property(e => e.Offerid).HasColumnName("offerid");

                entity.Property(e => e.Userid)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("userid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}