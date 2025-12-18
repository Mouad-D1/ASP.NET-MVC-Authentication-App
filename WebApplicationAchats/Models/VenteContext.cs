using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationAchats.Models;

public partial class VenteContext : DbContext
{
    public VenteContext()
    {
    }

    public VenteContext(DbContextOptions<VenteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Clientadress> Clientadresses { get; set; }

    public virtual DbSet<Commande> Commandes { get; set; }

    public virtual DbSet<DetailCommande> DetailCommandes { get; set; }

    public virtual DbSet<Produit> Produits { get; set; }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<Marque> Marques { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-QP77BJV;Initial Catalog=BD_VENTE_MIG;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Numcli).HasName("PK__CLIENT__64C8A28346C65F99");

            entity.ToTable("CLIENT");

            entity.Property(e => e.Numcli)
                .ValueGeneratedNever()
                .HasColumnName("NUMCLI");
            entity.Property(e => e.Categorie)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEGORIE");
            entity.Property(e => e.Compte)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COMPTE");
            entity.Property(e => e.Nomcli)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOMCLI");
            entity.Property(e => e.Ville)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("VILLE");
        });

        modelBuilder.Entity<Clientadress>(entity =>
        {
            entity.HasKey(e => e.Numcli).HasName("pk_ClientAddr");

            entity.ToTable("CLIENTAdress");

            entity.Property(e => e.Numcli)
                .ValueGeneratedNever()
                .HasColumnName("NUMCLI");
            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ADDRESS1");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STATE");

            entity.HasOne(d => d.NumcliNavigation).WithOne(p => p.Clientadress)
                .HasForeignKey<Clientadress>(d => d.Numcli)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ClientAddr_Client");
        });

        modelBuilder.Entity<Commande>(entity =>
        {
            entity.HasKey(e => e.Numcom).HasName("pk_commande");

            entity.ToTable("COMMANDE");

            entity.Property(e => e.Numcom).HasColumnName("NUMCOM");
            entity.Property(e => e.Datecom).HasColumnName("DATECOM");
            entity.Property(e => e.Numcli).HasColumnName("NUMCLI");

            entity.HasOne(d => d.NumcliNavigation).WithMany(p => p.Commandes)
                .HasForeignKey(d => d.Numcli)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_commande_client");
        });

        modelBuilder.Entity<DetailCommande>(entity =>
        {
            entity.HasKey(e => e.IdDetail).HasName("pk_detail");

            entity.ToTable("DETAIL_COMMANDE");

            entity.Property(e => e.IdDetail).HasColumnName("ID_DETAIL");
            entity.Property(e => e.IdCommande).HasColumnName("ID_COMMANDE");
            entity.Property(e => e.IdProduit).HasColumnName("ID_PRODUIT");
            entity.Property(e => e.Quantite).HasColumnName("QUANTITE");

            entity.HasOne(d => d.IdCommandeNavigation).WithMany(p => p.DetailCommandes)
                .HasForeignKey(d => d.IdCommande)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detail_commande");

            entity.HasOne(d => d.IdProduitNavigation).WithMany(p => p.DetailCommandes)
                .HasForeignKey(d => d.IdProduit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detail_produit");
        });

        modelBuilder.Entity<Produit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__produits__3213E83F6174ADA6");

            entity.ToTable("produits");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateAjout)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("DATE_AJOUT");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Disponible)
                .HasDefaultValue(true)
                .HasColumnName("DISPONIBLE");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.Prix)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("prix");
            entity.Property(e => e.Quantite).HasColumnName("quantite");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
