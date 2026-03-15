using System;
using System.Collections.Generic;
using GenshinDataApp.Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenshinDataApp.Backend.Data;

public partial class GenshinDbContext : DbContext
{
    public GenshinDbContext(DbContextOptions<GenshinDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ArtifactPiece> ArtifactPieces { get; set; }

    public virtual DbSet<ArtifactPieceHistory> ArtifactPieceHistories { get; set; }

    public virtual DbSet<ArtifactPieceTranslation> ArtifactPieceTranslations { get; set; }

    public virtual DbSet<ArtifactPieceTranslationHistory> ArtifactPieceTranslationHistories { get; set; }

    public virtual DbSet<ArtifactSet> ArtifactSets { get; set; }

    public virtual DbSet<ArtifactSetHistory> ArtifactSetHistories { get; set; }

    public virtual DbSet<ArtifactSetTranslation> ArtifactSetTranslations { get; set; }

    public virtual DbSet<ArtifactSetTranslationHistory> ArtifactSetTranslationHistories { get; set; }

    public virtual DbSet<BugReport> BugReports { get; set; }

    public virtual DbSet<BugReportHistory> BugReportHistories { get; set; }

    public virtual DbSet<BugReportImage> BugReportImages { get; set; }

    public virtual DbSet<BugReportImageHistory> BugReportImageHistories { get; set; }

    public virtual DbSet<Build> Builds { get; set; }

    public virtual DbSet<BuildHistory> BuildHistories { get; set; }

    public virtual DbSet<BuildSnapshot> BuildSnapshots { get; set; }

    public virtual DbSet<BuildSnapshotHistory> BuildSnapshotHistories { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<CharacterConstellation> CharacterConstellations { get; set; }

    public virtual DbSet<CharacterConstellationHistory> CharacterConstellationHistories { get; set; }

    public virtual DbSet<CharacterConstellationTranslation> CharacterConstellationTranslations { get; set; }

    public virtual DbSet<CharacterConstellationTranslationHistory> CharacterConstellationTranslationHistories { get; set; }

    public virtual DbSet<CharacterHistory> CharacterHistories { get; set; }

    public virtual DbSet<CharacterTalent> CharacterTalents { get; set; }

    public virtual DbSet<CharacterTalentHistory> CharacterTalentHistories { get; set; }

    public virtual DbSet<CharacterTalentTranslation> CharacterTalentTranslations { get; set; }

    public virtual DbSet<CharacterTalentTranslationHistory> CharacterTalentTranslationHistories { get; set; }

    public virtual DbSet<CharacterTranslation> CharacterTranslations { get; set; }

    public virtual DbSet<CharacterTranslationHistory> CharacterTranslationHistories { get; set; }

    public virtual DbSet<Element> Elements { get; set; }

    public virtual DbSet<ElementHistory> ElementHistories { get; set; }

    public virtual DbSet<ElementTranslation> ElementTranslations { get; set; }

    public virtual DbSet<ElementTranslationHistory> ElementTranslationHistories { get; set; }

    public virtual DbSet<EmailRateLimit> EmailRateLimits { get; set; }

    public virtual DbSet<ImportLog> ImportLogs { get; set; }

    public virtual DbSet<ImportLogHistory> ImportLogHistories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LanguageHistory> LanguageHistories { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<RefreshTokenHistory> RefreshTokenHistories { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<RegionHistory> RegionHistories { get; set; }

    public virtual DbSet<RegionTranslation> RegionTranslations { get; set; }

    public virtual DbSet<RegionTranslationHistory> RegionTranslationHistories { get; set; }

    public virtual DbSet<StatType> StatTypes { get; set; }

    public virtual DbSet<StatTypeHistory> StatTypeHistories { get; set; }

    public virtual DbSet<StatTypeTranslation> StatTypeTranslations { get; set; }

    public virtual DbSet<StatTypeTranslationHistory> StatTypeTranslationHistories { get; set; }

    public virtual DbSet<TechnicalPermission> TechnicalPermissions { get; set; }

    public virtual DbSet<TechnicalPermissionHistory> TechnicalPermissionHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserArtifact> UserArtifacts { get; set; }

    public virtual DbSet<UserArtifactHistory> UserArtifactHistories { get; set; }

    public virtual DbSet<UserCharacter> UserCharacters { get; set; }

    public virtual DbSet<UserCharacterHistory> UserCharacterHistories { get; set; }

    public virtual DbSet<UserHistory> UserHistories { get; set; }

    public virtual DbSet<UserWeapon> UserWeapons { get; set; }

    public virtual DbSet<UserWeaponHistory> UserWeaponHistories { get; set; }

    public virtual DbSet<VwUserArtifactStat> VwUserArtifactStats { get; set; }

    public virtual DbSet<Weapon> Weapons { get; set; }

    public virtual DbSet<WeaponHistory> WeaponHistories { get; set; }

    public virtual DbSet<WeaponRefinement> WeaponRefinements { get; set; }

    public virtual DbSet<WeaponRefinementHistory> WeaponRefinementHistories { get; set; }

    public virtual DbSet<WeaponRefinementTranslation> WeaponRefinementTranslations { get; set; }

    public virtual DbSet<WeaponRefinementTranslationHistory> WeaponRefinementTranslationHistories { get; set; }

    public virtual DbSet<WeaponTranslation> WeaponTranslations { get; set; }

    public virtual DbSet<WeaponTranslationHistory> WeaponTranslationHistories { get; set; }

    public virtual DbSet<WeaponType> WeaponTypes { get; set; }

    public virtual DbSet<WeaponTypeHistory> WeaponTypeHistories { get; set; }

    public virtual DbSet<WeaponTypeTranslation> WeaponTypeTranslations { get; set; }

    public virtual DbSet<WeaponTypeTranslationHistory> WeaponTypeTranslationHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArtifactPiece>(entity =>
        {
            entity.ToTable("ArtifactPiece", tb => tb.HasTrigger("TR_ArtifactPiece_History"));

            entity.HasIndex(e => e.PublicId, "IX_ArtifactPiece_PublicId").IsUnique();

            entity.HasIndex(e => e.ArtifactSetId, "IX_ArtifactPiece_Set").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.SlotType).HasMaxLength(20);

            entity.HasOne(d => d.ArtifactSet).WithMany(p => p.ArtifactPieces)
                .HasForeignKey(d => d.ArtifactSetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArtifactPiece_Set");
        });

        modelBuilder.Entity<ArtifactPieceHistory>(entity =>
        {
            entity.ToTable("ArtifactPiece_History");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.SlotType).HasMaxLength(20);
        });

        modelBuilder.Entity<ArtifactPieceTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_APT");

            entity.ToTable("ArtifactPieceTranslation", tb => tb.HasTrigger("TR_APT_History"));

            entity.HasIndex(e => e.PublicId, "IX_APT_PublicId").IsUnique();

            entity.HasIndex(e => new { e.ArtifactPieceId, e.LanguageId }, "UX_APT_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.ArtifactPiece).WithMany(p => p.ArtifactPieceTranslations)
                .HasForeignKey(d => d.ArtifactPieceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_APT_Piece");

            entity.HasOne(d => d.Language).WithMany(p => p.ArtifactPieceTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_APT_Language");
        });

        modelBuilder.Entity<ArtifactPieceTranslationHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_APT_History");

            entity.ToTable("ArtifactPieceTranslation_History");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<ArtifactSet>(entity =>
        {
            entity.ToTable("ArtifactSet", tb => tb.HasTrigger("TR_ArtifactSet_History"));

            entity.HasIndex(e => e.PublicId, "IX_ArtifactSet_PublicId").IsUnique();

            entity.HasIndex(e => e.Name, "UX_ArtifactSet_Name")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.FourPieceBonus).HasMaxLength(500);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.TwoPieceBonus).HasMaxLength(500);
        });

        modelBuilder.Entity<ArtifactSetHistory>(entity =>
        {
            entity.ToTable("ArtifactSet_History");

            entity.Property(e => e.FourPieceBonus).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.TwoPieceBonus).HasMaxLength(500);
        });

        modelBuilder.Entity<ArtifactSetTranslation>(entity =>
        {
            entity.ToTable("ArtifactSetTranslation", tb => tb.HasTrigger("TR_AST_History"));

            entity.HasIndex(e => e.PublicId, "IX_AST_PublicId").IsUnique();

            entity.HasIndex(e => new { e.ArtifactSetId, e.LanguageId }, "UX_AST_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.FourPieceBonus).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.TwoPieceBonus).HasMaxLength(500);

            entity.HasOne(d => d.ArtifactSet).WithMany(p => p.ArtifactSetTranslations)
                .HasForeignKey(d => d.ArtifactSetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AST_Set");

            entity.HasOne(d => d.Language).WithMany(p => p.ArtifactSetTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AST_Language");
        });

        modelBuilder.Entity<ArtifactSetTranslationHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AST_History");

            entity.ToTable("ArtifactSetTranslation_History");

            entity.Property(e => e.FourPieceBonus).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.TwoPieceBonus).HasMaxLength(500);
        });

        modelBuilder.Entity<BugReport>(entity =>
        {
            entity.ToTable("BugReport", tb => tb.HasTrigger("TR_BugReport_History"));

            entity.HasIndex(e => new { e.Status, e.CreatedAt }, "IX_BugReport_Public")
                .IsDescending(false, true)
                .HasFilter("([IsActive]=(1) AND ([Status] IN ('Confirmed', 'InProgress', 'Fixed', 'Closed')))");

            entity.HasIndex(e => e.PublicId, "IX_BugReport_PublicId").IsUnique();

            entity.HasIndex(e => e.Status, "IX_BugReport_Status").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.UserId, "IX_BugReport_UserId").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Priority).HasMaxLength(10);
            entity.Property(e => e.PublicDescription).HasMaxLength(1000);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.PublicTitle).HasMaxLength(150);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Reported");
            entity.Property(e => e.Title).HasMaxLength(150);

            entity.HasOne(d => d.AssignedToUser).WithMany(p => p.BugReportAssignedToUsers)
                .HasForeignKey(d => d.AssignedToUserId)
                .HasConstraintName("FK_BugReport_AssignedTo");

            entity.HasOne(d => d.User).WithMany(p => p.BugReportUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BugReport_User");
        });

        modelBuilder.Entity<BugReportHistory>(entity =>
        {
            entity.ToTable("BugReport_History");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.Priority).HasMaxLength(10);
            entity.Property(e => e.PublicDescription).HasMaxLength(1000);
            entity.Property(e => e.PublicTitle).HasMaxLength(150);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(150);
        });

        modelBuilder.Entity<BugReportImage>(entity =>
        {
            entity.ToTable("BugReportImage", tb => tb.HasTrigger("TR_BugReportImage_History"));

            entity.HasIndex(e => e.PublicId, "IX_BugReportImage_PublicId").IsUnique();

            entity.HasIndex(e => e.BugReportId, "IX_BugReportImage_Report").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.BugReport).WithMany(p => p.BugReportImages)
                .HasForeignKey(d => d.BugReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BugReportImage_Report");
        });

        modelBuilder.Entity<BugReportImageHistory>(entity =>
        {
            entity.ToTable("BugReportImage_History");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<Build>(entity =>
        {
            entity.ToTable("Build", tb => tb.HasTrigger("TR_Build_History"));

            entity.HasIndex(e => e.PublicId, "IX_Build_PublicId").IsUnique();

            entity.HasIndex(e => e.IsShared, "IX_Build_Shared").HasFilter("([IsActive]=(1) AND [IsShared]=(1))");

            entity.HasIndex(e => e.UserId, "IX_Build_UserId").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Version).HasDefaultValue(1);

            entity.HasOne(d => d.ArtifactCirclet).WithMany(p => p.BuildArtifactCirclets)
                .HasForeignKey(d => d.ArtifactCircletId)
                .HasConstraintName("FK_Build_Circlet");

            entity.HasOne(d => d.ArtifactFlower).WithMany(p => p.BuildArtifactFlowers)
                .HasForeignKey(d => d.ArtifactFlowerId)
                .HasConstraintName("FK_Build_Flower");

            entity.HasOne(d => d.ArtifactGoblet).WithMany(p => p.BuildArtifactGoblets)
                .HasForeignKey(d => d.ArtifactGobletId)
                .HasConstraintName("FK_Build_Goblet");

            entity.HasOne(d => d.ArtifactPlume).WithMany(p => p.BuildArtifactPlumes)
                .HasForeignKey(d => d.ArtifactPlumeId)
                .HasConstraintName("FK_Build_Plume");

            entity.HasOne(d => d.ArtifactSands).WithMany(p => p.BuildArtifactSands)
                .HasForeignKey(d => d.ArtifactSandsId)
                .HasConstraintName("FK_Build_Sands");

            entity.HasOne(d => d.Character).WithMany(p => p.Builds)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Build_Character");

            entity.HasOne(d => d.User).WithMany(p => p.Builds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Build_User");

            entity.HasOne(d => d.Weapon).WithMany(p => p.Builds)
                .HasForeignKey(d => d.WeaponId)
                .HasConstraintName("FK_Build_Weapon");
        });

        modelBuilder.Entity<BuildHistory>(entity =>
        {
            entity.ToTable("Build_History");

            entity.HasIndex(e => new { e.BuildId, e.OperationDate }, "IX_Build_History_Lookup").IsDescending(false, true);

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<BuildSnapshot>(entity =>
        {
            entity.ToTable("BuildSnapshot", tb => tb.HasTrigger("TR_BuildSnapshot_History"));

            entity.HasIndex(e => new { e.BuildId, e.Version }, "IX_BuildSnapshot_Build")
                .IsDescending(false, true)
                .HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.PublicId, "IX_BuildSnapshot_PublicId").IsUnique();

            entity.Property(e => e.ArchivedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.Build).WithMany(p => p.BuildSnapshots)
                .HasForeignKey(d => d.BuildId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BuildSnapshot_Build");
        });

        modelBuilder.Entity<BuildSnapshotHistory>(entity =>
        {
            entity.ToTable("BuildSnapshot_History");

            entity.Property(e => e.ArchivedAt).HasColumnType("datetime");
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<Character>(entity =>
        {
            entity.ToTable("Character", tb => tb.HasTrigger("TR_Character_History"));

            entity.HasIndex(e => e.ElementId, "IX_Character_Element").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.PublicId, "IX_Character_PublicId").IsUnique();

            entity.HasIndex(e => e.Rarity, "IX_Character_Rarity").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.RegionId, "IX_Character_Region").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.WeaponTypeId, "IX_Character_WeaponType").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.Name, "UX_Character_Name")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.BaseAtk).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BaseDef).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BaseHp).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IconPath).HasMaxLength(500);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ThumbnailPath).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.AscensionStat).WithMany(p => p.Characters)
                .HasForeignKey(d => d.AscensionStatId)
                .HasConstraintName("FK_Character_AscensionStat");

            entity.HasOne(d => d.Element).WithMany(p => p.Characters)
                .HasForeignKey(d => d.ElementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Character_Element");

            entity.HasOne(d => d.Region).WithMany(p => p.Characters)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Character_Region");

            entity.HasOne(d => d.WeaponType).WithMany(p => p.Characters)
                .HasForeignKey(d => d.WeaponTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Character_WeaponType");
        });

        modelBuilder.Entity<CharacterConstellation>(entity =>
        {
            entity.ToTable("CharacterConstellation", tb => tb.HasTrigger("TR_CharacterConstellation_History"));

            entity.HasIndex(e => e.PublicId, "IX_CharacterConstellation_PublicId").IsUnique();

            entity.HasIndex(e => new { e.CharacterId, e.Level }, "UX_CharacterConstellation_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IconPath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterConstellations)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharacterConstellation_Character");
        });

        modelBuilder.Entity<CharacterConstellationHistory>(entity =>
        {
            entity.ToTable("CharacterConstellation_History");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<CharacterConstellationTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CCT");

            entity.ToTable("CharacterConstellationTranslation", tb => tb.HasTrigger("TR_CCT_History"));

            entity.HasIndex(e => e.PublicId, "IX_CCT_PublicId").IsUnique();

            entity.HasIndex(e => new { e.CharacterConstellationId, e.LanguageId }, "UX_CCT_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.CharacterConstellation).WithMany(p => p.CharacterConstellationTranslations)
                .HasForeignKey(d => d.CharacterConstellationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CCT_Constellation");

            entity.HasOne(d => d.Language).WithMany(p => p.CharacterConstellationTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CCT_Language");
        });

        modelBuilder.Entity<CharacterConstellationTranslationHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CCT_History");

            entity.ToTable("CharacterConstellationTranslation_History");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<CharacterHistory>(entity =>
        {
            entity.ToTable("Character_History");

            entity.Property(e => e.BaseAtk).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BaseDef).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BaseHp).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<CharacterTalent>(entity =>
        {
            entity.ToTable("CharacterTalent", tb => tb.HasTrigger("TR_CharacterTalent_History"));

            entity.HasIndex(e => e.PublicId, "IX_CharacterTalent_PublicId").IsUnique();

            entity.HasIndex(e => new { e.CharacterId, e.TalentType }, "UX_CharacterTalent_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IconPath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.TalentType).HasMaxLength(20);

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterTalents)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharacterTalent_Character");
        });

        modelBuilder.Entity<CharacterTalentHistory>(entity =>
        {
            entity.ToTable("CharacterTalent_History");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.TalentType).HasMaxLength(20);
        });

        modelBuilder.Entity<CharacterTalentTranslation>(entity =>
        {
            entity.ToTable("CharacterTalentTranslation", tb => tb.HasTrigger("TR_CTT_History"));

            entity.HasIndex(e => e.PublicId, "IX_CTT_PublicId").IsUnique();

            entity.HasIndex(e => new { e.CharacterTalentId, e.LanguageId }, "UX_CTT_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.CharacterTalent).WithMany(p => p.CharacterTalentTranslations)
                .HasForeignKey(d => d.CharacterTalentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTT_Talent");

            entity.HasOne(d => d.Language).WithMany(p => p.CharacterTalentTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTT_Language");
        });

        modelBuilder.Entity<CharacterTalentTranslationHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CTT_History");

            entity.ToTable("CharacterTalentTranslation_History");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<CharacterTranslation>(entity =>
        {
            entity.ToTable("CharacterTranslation", tb => tb.HasTrigger("TR_CharacterTranslation_History"));

            entity.HasIndex(e => e.PublicId, "IX_CharacterTranslation_PublicId").IsUnique();

            entity.HasIndex(e => new { e.CharacterId, e.LanguageId }, "UX_CharacterTranslation_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Character).WithMany(p => p.CharacterTranslations)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharacterTranslation_Character");

            entity.HasOne(d => d.Language).WithMany(p => p.CharacterTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CharacterTranslation_Language");
        });

        modelBuilder.Entity<CharacterTranslationHistory>(entity =>
        {
            entity.ToTable("CharacterTranslation_History");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Element>(entity =>
        {
            entity.ToTable("Element", tb => tb.HasTrigger("TR_Element_History"));

            entity.HasIndex(e => e.PublicId, "IX_Element_PublicId").IsUnique();

            entity.HasIndex(e => e.Name, "UX_Element_Name")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IconPath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<ElementHistory>(entity =>
        {
            entity.ToTable("Element_History");

            entity.Property(e => e.IconPath).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<ElementTranslation>(entity =>
        {
            entity.ToTable("ElementTranslation", tb => tb.HasTrigger("TR_ElementTranslation_History"));

            entity.HasIndex(e => e.PublicId, "IX_ElementTranslation_PublicId").IsUnique();

            entity.HasIndex(e => new { e.ElementId, e.LanguageId }, "UX_ElementTranslation_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Element).WithMany(p => p.ElementTranslations)
                .HasForeignKey(d => d.ElementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ElementTranslation_Element");

            entity.HasOne(d => d.Language).WithMany(p => p.ElementTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ElementTranslation_Language");
        });

        modelBuilder.Entity<ElementTranslationHistory>(entity =>
        {
            entity.ToTable("ElementTranslation_History");

            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<EmailRateLimit>(entity =>
        {
            entity.ToTable("EmailRateLimit");

            entity.HasIndex(e => new { e.Email, e.ActionType, e.WindowStart }, "IX_EmailRateLimit_Lookup");

            entity.Property(e => e.ActionType).HasMaxLength(30);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.LastRequest)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RequestCount).HasDefaultValue(1);
            entity.Property(e => e.WindowStart)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ImportLog>(entity =>
        {
            entity.ToTable("ImportLog", tb => tb.HasTrigger("TR_ImportLog_History"));

            entity.HasIndex(e => e.PublicId, "IX_ImportLog_PublicId").IsUnique();

            entity.HasIndex(e => new { e.Status, e.StartedAt }, "IX_ImportLog_Status").IsDescending(false, true);

            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Started");

            entity.HasOne(d => d.ExecutedByUser).WithMany(p => p.ImportLogs)
                .HasForeignKey(d => d.ExecutedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportLog_User");
        });

        modelBuilder.Entity<ImportLogHistory>(entity =>
        {
            entity.ToTable("ImportLog_History");

            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.StartedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("Language", tb => tb.HasTrigger("TR_Language_History"));

            entity.HasIndex(e => e.PublicId, "IX_Language_PublicId").IsUnique();

            entity.HasIndex(e => e.Code, "UX_Language_Code")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NativeName).HasMaxLength(50);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<LanguageHistory>(entity =>
        {
            entity.ToTable("Language_History");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NativeName).HasMaxLength(50);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshToken", tb => tb.HasTrigger("TR_RefreshToken_History"));

            entity.HasIndex(e => e.PublicId, "IX_RefreshToken_PublicId").IsUnique();

            entity.HasIndex(e => e.Token, "IX_RefreshToken_Token").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.UserId, "IX_RefreshToken_UserId").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeviceFingerprint).HasMaxLength(512);
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Token).HasMaxLength(512);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefreshToken_User");
        });

        modelBuilder.Entity<RefreshTokenHistory>(entity =>
        {
            entity.ToTable("RefreshToken_History");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.Token).HasMaxLength(512);
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("Region", tb => tb.HasTrigger("TR_Region_History"));

            entity.HasIndex(e => e.PublicId, "IX_Region_PublicId").IsUnique();

            entity.HasIndex(e => e.Name, "UX_Region_Name")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IconPath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<RegionHistory>(entity =>
        {
            entity.ToTable("Region_History");

            entity.Property(e => e.IconPath).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<RegionTranslation>(entity =>
        {
            entity.ToTable("RegionTranslation", tb => tb.HasTrigger("TR_RegionTranslation_History"));

            entity.HasIndex(e => e.PublicId, "IX_RegionTranslation_PublicId").IsUnique();

            entity.HasIndex(e => new { e.RegionId, e.LanguageId }, "UX_RegionTranslation_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Language).WithMany(p => p.RegionTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RegionTranslation_Language");

            entity.HasOne(d => d.Region).WithMany(p => p.RegionTranslations)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RegionTranslation_Region");
        });

        modelBuilder.Entity<RegionTranslationHistory>(entity =>
        {
            entity.ToTable("RegionTranslation_History");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<StatType>(entity =>
        {
            entity.ToTable("StatType", tb => tb.HasTrigger("TR_StatType_History"));

            entity.HasIndex(e => e.PublicId, "IX_StatType_PublicId").IsUnique();

            entity.HasIndex(e => e.Name, "UX_StatType_Name")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Category).HasMaxLength(20);
            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<StatTypeHistory>(entity =>
        {
            entity.ToTable("StatType_History");

            entity.Property(e => e.Category).HasMaxLength(20);
            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<StatTypeTranslation>(entity =>
        {
            entity.ToTable("StatTypeTranslation", tb => tb.HasTrigger("TR_StatTypeTranslation_History"));

            entity.HasIndex(e => e.PublicId, "IX_StatTypeTranslation_PublicId").IsUnique();

            entity.HasIndex(e => new { e.StatTypeId, e.LanguageId }, "UX_StatTypeTranslation_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Language).WithMany(p => p.StatTypeTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatTypeTranslation_Language");

            entity.HasOne(d => d.StatType).WithMany(p => p.StatTypeTranslations)
                .HasForeignKey(d => d.StatTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatTypeTranslation_StatType");
        });

        modelBuilder.Entity<StatTypeTranslationHistory>(entity =>
        {
            entity.ToTable("StatTypeTranslation_History");

            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<TechnicalPermission>(entity =>
        {
            entity.ToTable("TechnicalPermission", tb => tb.HasTrigger("TR_TechnicalPermission_History"));

            entity.HasIndex(e => e.PublicId, "IX_TechnicalPermission_PublicId").IsUnique();

            entity.HasIndex(e => new { e.UserId, e.Permission }, "UX_TechPerm_UserPermission")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.GrantedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Permission).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.GrantedByUser).WithMany(p => p.TechnicalPermissionGrantedByUsers)
                .HasForeignKey(d => d.GrantedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TechPerm_GrantedBy");

            entity.HasOne(d => d.User).WithMany(p => p.TechnicalPermissionUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TechPerm_User");
        });

        modelBuilder.Entity<TechnicalPermissionHistory>(entity =>
        {
            entity.ToTable("TechnicalPermission_History");

            entity.Property(e => e.GrantedAt).HasColumnType("datetime");
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.Permission).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", tb => tb.HasTrigger("TR_User_History"));

            entity.HasIndex(e => new { e.AuthProvider, e.AuthProviderId }, "IX_User_AuthProvider").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.EmailVerificationToken, "IX_User_EmailVerificationToken").HasFilter("([EmailVerificationToken] IS NOT NULL AND [IsActive]=(1))");

            entity.HasIndex(e => e.PasswordResetToken, "IX_User_PasswordResetToken").HasFilter("([PasswordResetToken] IS NOT NULL AND [IsActive]=(1))");

            entity.HasIndex(e => e.PublicId, "IX_User_PublicId").IsUnique();

            entity.HasIndex(e => e.Email, "UQ_User_Email").IsUnique();

            entity.HasIndex(e => new { e.Username, e.UserCode }, "UX_User_UsernameCode")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.AuthProvider).HasMaxLength(20);
            entity.Property(e => e.AuthProviderId).HasMaxLength(256);
            entity.Property(e => e.AvatarPath).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.EmailVerificationToken).HasMaxLength(256);
            entity.Property(e => e.EmailVerificationTokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordResetToken).HasMaxLength(256);
            entity.Property(e => e.PasswordResetTokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Client");
            entity.Property(e => e.UserCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Username).HasMaxLength(20);
            entity.Property(e => e.UsernameLastChangedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserArtifact>(entity =>
        {
            entity.ToTable("UserArtifact", tb => tb.HasTrigger("TR_UserArtifact_History"));

            entity.HasIndex(e => new { e.UserId, e.MainStatId }, "IX_UserArtifact_MainStat").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.PublicId, "IX_UserArtifact_PublicId").IsUnique();

            entity.HasIndex(e => e.UserId, "IX_UserArtifact_UserId").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => new { e.UserId, e.ArtifactPieceId }, "IX_UserArtifact_UserPiece").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MainStatValue).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.SubStat1Value).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubStat2Value).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubStat3Value).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubStat4Value).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.ArtifactPiece).WithMany(p => p.UserArtifacts)
                .HasForeignKey(d => d.ArtifactPieceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserArtifact_Piece");

            entity.HasOne(d => d.MainStat).WithMany(p => p.UserArtifactMainStats)
                .HasForeignKey(d => d.MainStatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserArtifact_MainStat");

            entity.HasOne(d => d.SubStat1).WithMany(p => p.UserArtifactSubStat1s)
                .HasForeignKey(d => d.SubStat1Id)
                .HasConstraintName("FK_UserArtifact_Sub1");

            entity.HasOne(d => d.SubStat2).WithMany(p => p.UserArtifactSubStat2s)
                .HasForeignKey(d => d.SubStat2Id)
                .HasConstraintName("FK_UserArtifact_Sub2");

            entity.HasOne(d => d.SubStat3).WithMany(p => p.UserArtifactSubStat3s)
                .HasForeignKey(d => d.SubStat3Id)
                .HasConstraintName("FK_UserArtifact_Sub3");

            entity.HasOne(d => d.SubStat4).WithMany(p => p.UserArtifactSubStat4s)
                .HasForeignKey(d => d.SubStat4Id)
                .HasConstraintName("FK_UserArtifact_Sub4");

            entity.HasOne(d => d.User).WithMany(p => p.UserArtifacts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserArtifact_User");
        });

        modelBuilder.Entity<UserArtifactHistory>(entity =>
        {
            entity.ToTable("UserArtifact_History");

            entity.HasIndex(e => new { e.UserArtifactId, e.OperationDate }, "IX_UserArtifact_History_Lookup").IsDescending(false, true);

            entity.Property(e => e.MainStatValue).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.SubStat1Value).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubStat2Value).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubStat3Value).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubStat4Value).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<UserCharacter>(entity =>
        {
            entity.ToTable("UserCharacter", tb => tb.HasTrigger("TR_UserCharacter_History"));

            entity.HasIndex(e => e.PublicId, "IX_UserCharacter_PublicId").IsUnique();

            entity.HasIndex(e => e.UserId, "IX_UserCharacter_UserId").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => new { e.UserId, e.CharacterId }, "UX_UserCharacter_UserChar")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Level).HasDefaultValue(1);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.TalentBurst).HasDefaultValue(1);
            entity.Property(e => e.TalentNormalAtk).HasDefaultValue(1);
            entity.Property(e => e.TalentSkill).HasDefaultValue(1);

            entity.HasOne(d => d.Character).WithMany(p => p.UserCharacters)
                .HasForeignKey(d => d.CharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCharacter_Character");

            entity.HasOne(d => d.User).WithMany(p => p.UserCharacters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCharacter_User");
        });

        modelBuilder.Entity<UserCharacterHistory>(entity =>
        {
            entity.ToTable("UserCharacter_History");

            entity.HasIndex(e => new { e.UserCharacterId, e.OperationDate }, "IX_UserCharacter_History_Lookup").IsDescending(false, true);

            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<UserHistory>(entity =>
        {
            entity.ToTable("User_History");

            entity.HasIndex(e => new { e.UserId, e.OperationDate }, "IX_User_History_UserId").IsDescending(false, true);

            entity.Property(e => e.AuthProvider).HasMaxLength(20);
            entity.Property(e => e.AuthProviderId).HasMaxLength(256);
            entity.Property(e => e.AvatarPath).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UserCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Username).HasMaxLength(20);
        });

        modelBuilder.Entity<UserWeapon>(entity =>
        {
            entity.ToTable("UserWeapon", tb => tb.HasTrigger("TR_UserWeapon_History"));

            entity.HasIndex(e => e.PublicId, "IX_UserWeapon_PublicId").IsUnique();

            entity.HasIndex(e => e.UserId, "IX_UserWeapon_UserId").HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Level).HasDefaultValue(1);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Refinement).HasDefaultValue(1);

            entity.HasOne(d => d.User).WithMany(p => p.UserWeapons)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserWeapon_User");

            entity.HasOne(d => d.Weapon).WithMany(p => p.UserWeapons)
                .HasForeignKey(d => d.WeaponId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserWeapon_Weapon");
        });

        modelBuilder.Entity<UserWeaponHistory>(entity =>
        {
            entity.ToTable("UserWeapon_History");

            entity.HasIndex(e => new { e.UserWeaponId, e.OperationDate }, "IX_UserWeapon_History_Lookup").IsDescending(false, true);

            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<VwUserArtifactStat>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_UserArtifactStats");

            entity.Property(e => e.MainStatValue).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StatValue).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Weapon>(entity =>
        {
            entity.ToTable("Weapon", tb => tb.HasTrigger("TR_Weapon_History"));

            entity.HasIndex(e => e.PublicId, "IX_Weapon_PublicId").IsUnique();

            entity.HasIndex(e => e.Rarity, "IX_Weapon_Rarity").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.WeaponTypeId, "IX_Weapon_Type").HasFilter("([IsActive]=(1))");

            entity.HasIndex(e => e.Name, "UX_Weapon_Name")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.BaseAtk).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PassiveName).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.SecondaryStatValue).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ThumbnailPath).HasMaxLength(500);

            entity.HasOne(d => d.SecondaryStat).WithMany(p => p.Weapons)
                .HasForeignKey(d => d.SecondaryStatId)
                .HasConstraintName("FK_Weapon_SecondaryStat");

            entity.HasOne(d => d.WeaponType).WithMany(p => p.Weapons)
                .HasForeignKey(d => d.WeaponTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Weapon_WeaponType");
        });

        modelBuilder.Entity<WeaponHistory>(entity =>
        {
            entity.ToTable("Weapon_History");

            entity.Property(e => e.BaseAtk).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.PassiveName).HasMaxLength(100);
            entity.Property(e => e.SecondaryStatValue).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<WeaponRefinement>(entity =>
        {
            entity.ToTable("WeaponRefinement", tb => tb.HasTrigger("TR_WeaponRefinement_History"));

            entity.HasIndex(e => e.PublicId, "IX_WeaponRefinement_PublicId").IsUnique();

            entity.HasIndex(e => new { e.WeaponId, e.RefinementLevel }, "UX_WeaponRefinement_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Weapon).WithMany(p => p.WeaponRefinements)
                .HasForeignKey(d => d.WeaponId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeaponRefinement_Weapon");
        });

        modelBuilder.Entity<WeaponRefinementHistory>(entity =>
        {
            entity.ToTable("WeaponRefinement_History");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<WeaponRefinementTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_WRT");

            entity.ToTable("WeaponRefinementTranslation", tb => tb.HasTrigger("TR_WRT_History"));

            entity.HasIndex(e => e.PublicId, "IX_WRT_PublicId").IsUnique();

            entity.HasIndex(e => new { e.WeaponRefinementId, e.LanguageId }, "UX_WRT_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Language).WithMany(p => p.WeaponRefinementTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WRT_Language");

            entity.HasOne(d => d.WeaponRefinement).WithMany(p => p.WeaponRefinementTranslations)
                .HasForeignKey(d => d.WeaponRefinementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WRT_Refinement");
        });

        modelBuilder.Entity<WeaponRefinementTranslationHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_WRT_History");

            entity.ToTable("WeaponRefinementTranslation_History");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<WeaponTranslation>(entity =>
        {
            entity.ToTable("WeaponTranslation", tb => tb.HasTrigger("TR_WeaponTranslation_History"));

            entity.HasIndex(e => e.PublicId, "IX_WeaponTranslation_PublicId").IsUnique();

            entity.HasIndex(e => new { e.WeaponId, e.LanguageId }, "UX_WeaponTranslation_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PassiveName).HasMaxLength(100);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Language).WithMany(p => p.WeaponTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeaponTranslation_Language");

            entity.HasOne(d => d.Weapon).WithMany(p => p.WeaponTranslations)
                .HasForeignKey(d => d.WeaponId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeaponTranslation_Weapon");
        });

        modelBuilder.Entity<WeaponTranslationHistory>(entity =>
        {
            entity.ToTable("WeaponTranslation_History");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.PassiveName).HasMaxLength(100);
        });

        modelBuilder.Entity<WeaponType>(entity =>
        {
            entity.ToTable("WeaponType", tb => tb.HasTrigger("TR_WeaponType_History"));

            entity.HasIndex(e => e.PublicId, "IX_WeaponType_PublicId").IsUnique();

            entity.HasIndex(e => e.Name, "UX_WeaponType_Name")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IconPath).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<WeaponTypeHistory>(entity =>
        {
            entity.ToTable("WeaponType_History");

            entity.Property(e => e.IconPath).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<WeaponTypeTranslation>(entity =>
        {
            entity.ToTable("WeaponTypeTranslation", tb => tb.HasTrigger("TR_WeaponTypeTranslation_History"));

            entity.HasIndex(e => e.PublicId, "IX_WeaponTypeTranslation_PublicId").IsUnique();

            entity.HasIndex(e => new { e.WeaponTypeId, e.LanguageId }, "UX_WeaponTypeTranslation_Composite")
                .IsUnique()
                .HasFilter("([IsActive]=(1))");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.PublicId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Language).WithMany(p => p.WeaponTypeTranslations)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeaponTypeTranslation_Language");

            entity.HasOne(d => d.WeaponType).WithMany(p => p.WeaponTypeTranslations)
                .HasForeignKey(d => d.WeaponTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeaponTypeTranslation_WeaponType");
        });

        modelBuilder.Entity<WeaponTypeTranslationHistory>(entity =>
        {
            entity.ToTable("WeaponTypeTranslation_History");

            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType).HasMaxLength(10);
            entity.Property(e => e.OperationUser)
                .HasMaxLength(256)
                .HasDefaultValueSql("(suser_sname())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
