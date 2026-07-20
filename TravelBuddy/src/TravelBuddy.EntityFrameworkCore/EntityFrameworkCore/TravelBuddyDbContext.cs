using Microsoft.EntityFrameworkCore;
using TravelBuddy.Calificaciones;
using TravelBuddy.Ciudades;
using TravelBuddy.Destinos;
using TravelBuddy.Experiencias;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using TravelBuddy.Favoritos;
using TravelBuddy.Notificaciones;
using TravelBuddy.EntityFrameworkCore;
using TravelBuddy.Comentarios;
using TravelBuddy.Seguimientos;
using TravelBuddy.ApisExternas;


namespace TravelBuddy.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName("Default")]
public class TravelBuddyDbContext :
    AbpDbContext<TravelBuddyDbContext>,
    IIdentityDbContext
{
 
    public DbSet<Destino> Destinos { get; set; }
    public DbSet<Calificacion> Calificaciones { get; set; }
    public DbSet<Ciudad> Ciudades { get; set; } 
    public DbSet<Experiencia> Experiencias { get; set; }

    public DbSet<Favorito> Favoritos { get; set; }
    public DbSet<Notificacion> Notificaciones { get; set; }

    public DbSet<PreferenciaNotificacion> PreferenciasNotificaciones { get; set; }
    public DbSet<DestinoSeguido> DestinosSeguidos { get; set; }
    public DbSet<RegistroLlamadaApi> RegistrosLlamadasApi { get; set; }
    #region Entities from the modules

    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    #endregion
    public DbSet<Comentario> Comentarios { get; set; }  
    public TravelBuddyDbContext(DbContextOptions<TravelBuddyDbContext> options)
        : base(options)
    {
    }

    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureBlobStoring();

        builder.Entity<Destino>(b =>
        {
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "Destinos", TravelBuddyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Pais).IsRequired().HasMaxLength(100);
            b.Property(x => x.Ciudad).IsRequired().HasMaxLength(100);
            b.Property(x => x.Coordenadas).IsRequired().HasMaxLength(256);
            b.Property(x => x.Foto).IsRequired().HasMaxLength(512);
            b.Property(x => x.Poblacion).IsRequired();
        });

        
        builder.Entity<Calificacion>(b =>
        {
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "Calificaciones", TravelBuddyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Puntaje).IsRequired().HasMaxLength(100);
            b.Property(x => x.Comentario).HasMaxLength(256);
        });

        
        builder.Entity<Ciudad>(b =>
        {
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "Ciudades", TravelBuddyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            b.Property(x => x.Pais).IsRequired().HasMaxLength(100);
            b.Property(x => x.Poblacion).IsRequired();
        });

        
        builder.Entity<Experiencia>(b =>
        {
           
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "Experiencias", TravelBuddyConsts.DbSchema);

            
            b.ConfigureByConvention();

        });

        builder.Entity<Favorito>(b =>
        {
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "Favoritos", TravelBuddyConsts.DbSchema);
            b.ConfigureByConvention();
                
            b.HasIndex(x => new { x.UserId, x.DestinoId }).IsUnique();
        });

        builder.Entity<Notificacion>(b =>
        {
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "Notificaciones", TravelBuddyConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<PreferenciaNotificacion>(b =>
        {
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "PreferenciasNotificaciones", TravelBuddyConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Comentario>(b =>
        {
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "Comentarios", TravelBuddyConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<RegistroLlamadaApi>(b =>
        {
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "RegistrosLlamadasApi", TravelBuddyConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.NombreApi).IsRequired().HasMaxLength(100);
            b.Property(x => x.Endpoint).IsRequired().HasMaxLength(200);
        });

        builder.Entity<DestinoSeguido>(b =>
        {
            b.ToTable(TravelBuddyConsts.DbTablePrefix + "DestinosSeguidos", TravelBuddyConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasIndex(x => new { x.UserId, x.DestinoId }).IsUnique();

            b.HasOne<Destino>()
                .WithMany()
                .HasForeignKey(x => x.DestinoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
