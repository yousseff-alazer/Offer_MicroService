using System;
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
        public virtual DbSet<OfferTranslate> OfferTranslates { get; set; }
        public virtual DbSet<OfferType> OfferTypes { get; set; }
        public virtual DbSet<OfferUser> OfferUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=OfferDb;User Id=admin;Password=admin1234;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.ToTable("offer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActionType)
                    .HasMaxLength(250)
                    .HasColumnName("action_type")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.ActionTypeId)
                    .HasMaxLength(250)
                    .HasColumnName("action_type_id")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.ConstantType)
                    .HasMaxLength(250)
                    .HasColumnName("constant_type")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(250)
                    .HasColumnName("created_by");

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

                entity.Property(e => e.LanguageId)
                    .HasColumnType("character varying")
                    .HasColumnName("language_id");

                entity.Property(e => e.MaxValue)
                    .HasMaxLength(250)
                    .HasColumnName("max_value")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Maxusagecount)
                    .HasColumnName("maxusagecount")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.MinValue)
                    .HasMaxLength(250)
                    .HasColumnName("min_value")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Modificationdate).HasColumnName("modificationdate");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(250)
                    .HasColumnName("modified_by");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.ObjectId)
                    .HasMaxLength(250)
                    .HasColumnName("object_id")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.ObjectTypeId)
                    .HasMaxLength(250)
                    .HasColumnName("object_type_id")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.ObjectUrl)
                    .HasMaxLength(250)
                    .HasColumnName("object_url")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.OfferTypeId).HasColumnName("offer_type_id");

                entity.Property(e => e.Purpose)
                    .HasMaxLength(250)
                    .HasColumnName("purpose")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Usedcount)
                    .HasColumnName("usedcount")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Validfrom).HasColumnName("validfrom");

                entity.Property(e => e.Validto).HasColumnName("validto");

                entity.HasOne(d => d.OfferType)
                    .WithMany(p => p.Offers)
                    .HasForeignKey(d => d.OfferTypeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_offer_type");
            });

            modelBuilder.Entity<OfferTranslate>(entity =>
            {
                entity.ToTable("offer_translate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(250)
                    .HasColumnName("created_by");

                entity.Property(e => e.Creationdate)
                    .HasColumnName("creationdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.LanguageId)
                    .HasMaxLength(250)
                    .HasColumnName("language_id")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Modificationdate).HasColumnName("modificationdate");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(250)
                    .HasColumnName("modified_by");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.OfferId).HasColumnName("offer_id");

                entity.Property(e => e.Purpose)
                    .HasMaxLength(250)
                    .HasColumnName("purpose")
                    .HasDefaultValueSql("NULL::character varying");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.OfferTranslates)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_offer");
            });

            modelBuilder.Entity<OfferType>(entity =>
            {
                entity.ToTable("offer_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(250)
                    .HasColumnName("created_by");

                entity.Property(e => e.Creationdate)
                    .HasColumnName("creationdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description")
                    .HasDefaultValueSql("NULL::character varying");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Modificationdate).HasColumnName("modificationdate");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(250)
                    .HasColumnName("modified_by");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name")
                    .HasDefaultValueSql("NULL::character varying");
            });

            modelBuilder.Entity<OfferUser>(entity =>
            {
                entity.ToTable("offer_user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(250)
                    .HasColumnName("created_by");

                entity.Property(e => e.Creationdate)
                    .HasColumnName("creationdate")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Modificationdate).HasColumnName("modificationdate");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(250)
                    .HasColumnName("modified_by");

                entity.Property(e => e.Offerid).HasColumnName("offerid");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("user_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
