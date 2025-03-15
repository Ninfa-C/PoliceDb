using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PoliceDb.Data;
using PoliceDb.Models;
using PoliceDb.ViewModels;

namespace PoliceDb.Services
{
    public class PoliceServices
    {
        private readonly PoliceDbContext _db;
        public PoliceServices(PoliceDbContext db)
        {
            _db = db;
        }

        private async Task<bool> SaveAChanges()
        {
            try
            {
                var rowsAffected = await _db.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<TipoViolazione>> GetAllViolation()
        {
            try
            {
                var violazioniList = new List<TipoViolazione>();

                violazioniList = await _db.TipiViolazione.ToListAsync();
                return violazioniList;
            }
            catch
            {
                return new List<TipoViolazione>();
            }
        }

        public async Task<VerbaliListModel> GetAllReports()
        {

            try
            {
                var model = new VerbaliListModel();
                model.VerbaliList = await _db.Verbali
                    .Include(v => v.Anagrafica)
                    .Include(v => v.ViolazioniVerbali)
                    .ThenInclude(vv => vv.TipoViolazione)
                    .ToListAsync();
                return model;
            }
            catch
            {
                return new VerbaliListModel() { VerbaliList = new List<Verbali>() };
            }
        }

        public async Task<bool> DeleteReport(Guid id)
        {
            var report = await _db.Verbali.FindAsync(id);
            if (report == null)
            {
                return false;
            }
            _db.Verbali.Remove(report);
            return await SaveAChanges();
        }

        private async Task<Anagrafica> FindOrCreate(Anagrafica anagrafica)
        {
            var person = await _db.Anagrafiche.FirstOrDefaultAsync(u => u.CF == anagrafica.CF);
            if (person == null)
            {
                person = new Anagrafica
                {
                    IdAnagrafica = Guid.NewGuid(),
                    Cognome = anagrafica.Cognome,
                    Nome = anagrafica.Nome,
                    Indirizzo = anagrafica.Indirizzo,
                    Città = anagrafica.Città,
                    CAP = anagrafica.CAP,
                    CF = anagrafica.CF
                };
                _db.Anagrafiche.Add(person);
                await SaveAChanges();
            }
            return person;
        }


        public async Task<bool> AddReport(PoliceAddModel model)
        {
            Guid guid = Guid.NewGuid();

            var person = await FindOrCreate(model.Anagrafica);

            var report = new Verbali()
            {
                IdVerbale = guid,
                IdAnagrafica = person.IdAnagrafica,
                DataViolazione = model.DataViolazione,
                DataTrascrizioneVerbale = model.DataTrascrizioneVerbale,
                NominativoAgente = model.NominativoAgente,
                IndirizzoViolazione = model.IndirizzoViolazione,
                TotaleImporto = model.Importo,
                TotaleDecurtamentoPunti = model.DecurtamentoPunti,
                ScadenzaContestazione = model.DataViolazione.AddDays(60),
                ViolazioniVerbali = model.Violazioni.Select(i => new ViolazioniVerbali
                {
                    Id = Guid.NewGuid(),
                    IdVerbale = guid,
                    IdViolazione = i.IdViolazione,
                    Importo = i.Importo,
                    DecurtamentoPunti = i.DecurtamentoPunti
                }).ToList()
            };

            _db.Verbali.Add(report);
            return await SaveAChanges();
        }

        public async Task<Verbali?> GetEditReport(Guid id)
        {
            var report = await _db.Verbali
                .Include(v => v.Anagrafica)
                .Include(v => v.ViolazioniVerbali)
                 .FirstOrDefaultAsync(l => l.IdVerbale == id);
            if (report == null)
            {
                return null;
            }
            return report;
        }


        public async Task<bool> SaveEditReport(PoliceEditModel model)
        {
            var report = await _db.Verbali
                .Include(r => r.ViolazioniVerbali)
                .FirstOrDefaultAsync(r => r.IdVerbale == model.IdVerbale);

            if (report == null)
            {
                Console.WriteLine($"Report con IdVerbale {model.IdVerbale} non trovato.");
                return false;
            }
            foreach (var violazioneEsistente in report.ViolazioniVerbali)
            {
                await _db.Entry(violazioneEsistente).ReloadAsync();
            }
            report.DataViolazione = model.DataViolazione;
            report.DataTrascrizioneVerbale = model.DataTrascrizioneVerbale;
            report.NominativoAgente = model.NominativoAgente;
            report.IndirizzoViolazione = model.IndirizzoViolazione;
            report.TotaleImporto = model.Importo;
            report.TotaleDecurtamentoPunti = model.DecurtamentoPunti;
            report.ScadenzaContestazione = model.DataViolazione.AddDays(60);

            var person = await FindOrCreate(model.Anagrafica);
            report.IdAnagrafica = person.IdAnagrafica;

            foreach (var violazioneModel in model.Violazioni)
            {
                var violazioneEsistente = report.ViolazioniVerbali
                    .FirstOrDefault(v => v.IdViolazione == violazioneModel.IdViolazione);
                decimal importo = decimal.Parse(violazioneModel.Importo.ToString(), CultureInfo.InvariantCulture);
                if (violazioneEsistente != null)
                {
                    violazioneEsistente.Importo = importo;
                    violazioneEsistente.DecurtamentoPunti = violazioneModel.DecurtamentoPunti;

                    _db.Entry(violazioneEsistente).State = EntityState.Modified;
                }
                else
                {
                    var nuovaViolazione = new ViolazioniVerbali
                    {
                        Id = Guid.NewGuid(),
                        IdVerbale = model.IdVerbale,
                        IdViolazione = violazioneModel.IdViolazione,
                        Importo = importo,
                        DecurtamentoPunti = violazioneModel.DecurtamentoPunti
                    };

                    report.ViolazioniVerbali.Add(nuovaViolazione);
                    _db.Entry(nuovaViolazione).State = EntityState.Added;
                }
            }
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
               
                Console.WriteLine($"Errore di concorrenza: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {               
                Console.WriteLine($"Errore durante il salvataggio: {ex.Message}");
                return false;
            }
        }



        public async Task<VerbaliListModel> GetTotalReport()
        {

            try
            {
                var result = new VerbaliListModel();
                result.VerbaliList = await _db.Verbali
                              .Include(v => v.Anagrafica)
                              .GroupBy(v => v.Anagrafica!.CF)
                              .Select(g => new Verbali
                              {
                                  Anagrafica = g.First().Anagrafica,
                                  Total = g.Count(),
                              })
                              .ToListAsync();
                return result;
            }
            catch
            {
                return new VerbaliListModel() { VerbaliList = new List<Verbali>() };
            }
        }

        public async Task<VerbaliListModel> GetTotalePunti()
        {

            try
            {
                var result = new VerbaliListModel();
                result.VerbaliList = await _db.Verbali
                              .Include(v => v.Anagrafica)
                              .GroupBy(v => v.Anagrafica!.CF)
                              .Select(g => new Verbali
                              {
                                  Anagrafica = g.First().Anagrafica,
                                  Total = g.Sum(v => v.TotaleDecurtamentoPunti),
                              })
                              .ToListAsync();
                return result;
            }
            catch
            {
                return new VerbaliListModel() { VerbaliList = new List<Verbali>() };
            }
        }

        public async Task<VerbaliListModel> GetReportPointOver10()
        {
            try
            {
                var result = new VerbaliListModel();
                result.VerbaliList = await _db.Verbali
                        .Include(v => v.Anagrafica)
                        .Where(v => v.TotaleDecurtamentoPunti > 10)
                        .Select(g => new Verbali
                        {
                            IdVerbale = g.IdVerbale,
                            DataViolazione = g.DataViolazione,
                            Anagrafica = g.Anagrafica,
                            Total = g.TotaleDecurtamentoPunti,
                        })
                        .ToListAsync();
                return result;
            }
            catch
            {
                return new VerbaliListModel() { VerbaliList = new List<Verbali>() };
            }
        }

        public async Task<VerbaliListModel> GetReportValueOver400()
        {

            try
            {
                var result = new VerbaliListModel();
                result.VerbaliList = await _db.Verbali
                        .Include(v => v.Anagrafica)
                        .Where(v => v.TotaleImporto > 400)
                        .Select(g => new Verbali
                        {
                            IdVerbale = g.IdVerbale,
                            DataViolazione = g.DataViolazione,
                            Anagrafica = g.Anagrafica,
                            Total2 = g.TotaleImporto,
                        })
                        .ToListAsync();
                return result;
            }
            catch
            {
                return new VerbaliListModel() { VerbaliList = new List<Verbali>() };
            }
        }


    }
}
