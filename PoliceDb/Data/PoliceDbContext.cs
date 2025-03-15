using Microsoft.EntityFrameworkCore;
using PoliceDb.Models;

namespace PoliceDb.Data
{
    public class PoliceDbContext : DbContext
    {
        public PoliceDbContext(DbContextOptions<PoliceDbContext> options) : base(options) { }

        public DbSet<Anagrafica> Anagrafiche { get; set; }
        public DbSet<TipoViolazione> TipiViolazione { get; set; }
        public DbSet<Verbali> Verbali { get; set; }
        public DbSet<ViolazioniVerbali> ViolazioniVerbali { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ViolazioniVerbali>()
                .HasIndex(vv => new { vv.IdVerbale, vv.IdViolazione })
                .IsUnique();
            modelBuilder.Entity<Anagrafica>()
                .HasIndex(a => a.CF)
                .IsUnique();

            modelBuilder.Entity<ViolazioniVerbali>()
                .HasOne(vv => vv.Verbale)
                .WithMany(v => v.ViolazioniVerbali)
                .HasForeignKey(vv => vv.IdVerbale)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TipoViolazione>().HasData(
                new TipoViolazione { Id = new Guid("d2a2c31f-ea47-4c69-a4c8-d9f3f8c2a2f3"), Descrizione = "Eccesso di velocità" },
                new TipoViolazione { Id = new Guid("BD3D24C4-C47A-4772-8BF4-13FECAC97CB4"), Descrizione = "Guida in stato di ebbrezza" },
                new TipoViolazione { Id = new Guid("b51a10bc-e722-45a5-9357-b81b75f2d387"), Descrizione = "Guida senza cintura di sicurezza" },
                new TipoViolazione { Id = new Guid("f6a06f68-ea4c-4ad7-9b10-8ef0ed2c93d7"), Descrizione = "Utilizzo del cellulare alla guida" },
                new TipoViolazione { Id = new Guid("c6d1d5b0-7bfa-4c2b-bd92-7bbed7e6f001"), Descrizione = "Passaggio con il semaforo rosso" }
            );

            modelBuilder.Entity<Anagrafica>().HasData(
                new Anagrafica { IdAnagrafica = new Guid("c1fcded1-0f9d-45c6-8c78-64f84600fa1a"), Nome = "Mario", Cognome = "Rossi", Indirizzo = "Via Roma 10", Città = "Milano", CAP = "20121", CF = "RSSMRA80A01H501Z" },
                new Anagrafica { IdAnagrafica = new Guid("e8d5c12f-b41f-4748-9d3d-d6f2387582c4"), Nome = "Luca", Cognome = "Bianchi", Indirizzo = "Corso Venezia 5", Città = "Torino", CAP = "10121", CF = "BNCPLC85B01F205X" },
                new Anagrafica { IdAnagrafica = new Guid("b95d59ac-702d-4b32-bc59-e0f25f5f576b"), Nome = "Giulia", Cognome = "Verdi", Indirizzo = "Piazza Duomo 3", Città = "Firenze", CAP = "50122", CF = "VRDGLI90C41D612K" },
                new Anagrafica { IdAnagrafica = new Guid("b7bcfd07-7f8b-47e7-b636-8c6b351a0d0f"), Nome = "Anna", Cognome = "Neri", Indirizzo = "Via Garibaldi 8", Città = "Bologna", CAP = "40123", CF = "NRAANN75D61G312T" },
                new Anagrafica { IdAnagrafica = new Guid("a3208b53-dba6-4857-95b4-264cc4209d3b"), Nome = "Francesco", Cognome = "Gialli", Indirizzo = "Viale Libertà 15", Città = "Napoli", CAP = "80121", CF = "GLLFRN95E01C123U" }
            );

            //     modelBuilder.Entity<TipoViolazione>().HasData(
            //    new TipoViolazione { Id = Guid.NewGuid(), Descrizione = "Eccesso di velocità" },
            //    new TipoViolazione { Id = Guid.NewGuid(), Descrizione = "Guida in stato di ebbrezza" },
            //    new TipoViolazione { Id = Guid.NewGuid(), Descrizione = "Guida senza cintura di sicurezza" },
            //    new TipoViolazione { Id = Guid.NewGuid(), Descrizione = "Utilizzo del cellulare alla guida" },
            //    new TipoViolazione { Id = Guid.NewGuid(), Descrizione = "Passaggio con il semaforo rosso" }
            //);
            //     modelBuilder.Entity<Anagrafica>().HasData(
            //         new Anagrafica { IdAnagrafica = Guid.NewGuid(), Nome = "Mario", Cognome = "Rossi", Indirizzo = "Via Roma 10", Città = "Milano", CAP = "20121", CF = "RSSMRA80A01H501Z" },
            //         new Anagrafica { IdAnagrafica = Guid.NewGuid(), Nome = "Luca", Cognome = "Bianchi", Indirizzo = "Corso Venezia 5", Città = "Torino", CAP = "10121", CF = "BNCPLC85B01F205X" },
            //         new Anagrafica { IdAnagrafica = Guid.NewGuid(), Nome = "Giulia", Cognome = "Verdi", Indirizzo = "Piazza Duomo 3", Città = "Firenze", CAP = "50122", CF = "VRDGLI90C41D612K" },
            //         new Anagrafica { IdAnagrafica = Guid.NewGuid(), Nome = "Anna", Cognome = "Neri", Indirizzo = "Via Garibaldi 8", Città = "Bologna", CAP = "40123", CF = "NRAANN75D61G312T" },
            //         new Anagrafica { IdAnagrafica = Guid.NewGuid(), Nome = "Francesco", Cognome = "Gialli", Indirizzo = "Viale Libertà 15", Città = "Napoli", CAP = "80121", CF = "GLLFRN95E01C123U" }
            //     );
        }
    }
}
