using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIEfirma.Models;

public partial class EfirmaDbContext : DbContext
{
    public EfirmaDbContext()
    {
    }

    public EfirmaDbContext(DbContextOptions<EfirmaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Documento> Documentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=efirma;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.DocId).HasName("documentos_pkey");

            entity.ToTable("documentos");

            entity.Property(e => e.DocId).HasColumnName("doc_id");
            entity.Property(e => e.DocFirma).HasColumnName("doc_firma");
            entity.Property(e => e.DocHashcode).HasColumnName("doc_hashcode");
            entity.Property(e => e.DocNombredocumento)
                .HasMaxLength(255)
                .HasColumnName("doc_nombredocumento");
            entity.Property(e => e.DocRfc)
                .HasMaxLength(255)
                .HasColumnName("doc_rfc");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
