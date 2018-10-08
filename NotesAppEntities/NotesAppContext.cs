using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Nerdable.NotesApi.NotesAppEntities
{
    public partial class NotesAppContext : DbContext
    {
        public NotesAppContext()
        {
        }

        public NotesAppContext(DbContextOptions<NotesAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<TagAlwaysIncludeRelationship> TagAlwaysIncludeRelationship { get; set; }
        public virtual DbSet<TagNoteRelationship> TagNoteRelationship { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=HEATHERA-PC3;Database=NotesApp;Trusted_Connection=True;MultipleActiveResultSets=true");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notes>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.HasIndex(e => e.CreatedByUserId);

                entity.Property(e => e.LastUpdated).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(d => d.CreatedByUserId);
            });

            modelBuilder.Entity<TagAlwaysIncludeRelationship>(entity =>
            {
                entity.HasKey(e => new { e.AlwaysIncludeTagId, e.ChildTagId });

                entity.HasOne(d => d.AlwaysIncludeTag)
                    .WithMany(p => p.TagAlwaysIncludeRelationshipAlwaysIncludeTag)
                    .HasForeignKey(d => d.AlwaysIncludeTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagAlwaysIncludesRelationship_Tags");

                entity.HasOne(d => d.ChildTag)
                    .WithMany(p => p.TagAlwaysIncludeRelationshipChildTag)
                    .HasForeignKey(d => d.ChildTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagAlwaysIncludesRelationship_Tags_Child");
            });

            modelBuilder.Entity<TagNoteRelationship>(entity =>
            {
                entity.HasKey(e => new { e.TagId, e.NoteId });

                entity.HasIndex(e => e.NoteId);

                entity.HasOne(d => d.Note)
                    .WithMany(p => p.TagNoteRelationship)
                    .HasForeignKey(d => d.NoteId);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagNoteRelationship)
                    .HasForeignKey(d => d.TagId);
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.HasKey(e => e.TagId);

                entity.HasIndex(e => e.CreatedByUserId);

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.Tags)
                    .HasForeignKey(d => d.CreatedByUserId);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });
        }
    }
}
