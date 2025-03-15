using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PoliceDb.Models;
using PoliceDb.Services;
using PoliceDb.ViewModels;

namespace PoliceDb.Controllers
{

    public class PoliceController : Controller
    {

        private readonly PoliceServices _policeServices;
        public PoliceController(PoliceServices policeServices)
        {
            _policeServices = policeServices;
        }

        public async Task<IActionResult> Index()
        {
            var verbali = await _policeServices.GetAllReports();
            return View(verbali);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.Violazioni = await _policeServices.GetAllViolation();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSave(PoliceAddModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Qualcosa è andato storto";
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var result = await _policeServices.AddReport(model);
            if (!result)
            {
                TempData["Error"] = "Errore durante l'aggiunta del verbale al databse";
            }

            return RedirectToAction("Index");
        }

        [HttpGet("Filter/{category}")]
        public async Task<IActionResult> Filter(string category)
        {
            VerbaliListModel? data = null;
            string title = "";

            switch (category)
            {
                case "TotaleVerbali":
                    data = await _policeServices.GetTotalReport();
                    title = "Totale Verbali per Trasgressore";
                    break;

                case "TotalePunti":
                    data = await _policeServices.GetTotalePunti();
                    title = "Totale Punti Decurtati per Trasgressore";
                    break;

                case "Oltre10Punti":
                    data = await _policeServices.GetReportPointOver10();
                    title = "Verbali con oltre 10 Punti Decurtati";
                    break;

                case "Oltre400Euro":
                    data = await _policeServices.GetReportValueOver400();
                    title = "Verbali con Importo Maggiore di 400 Euro";
                    break;

                default:
                    return RedirectToAction("Index");
            }

            ViewBag.Title = title;
            ViewBag.Category = category;
            return View(data);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _policeServices.DeleteReport(id);
            if (!result)
            {
                TempData["Error"] = "Errore nell'eliminazione del libro dal database";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditReport(Guid id)
        {
            var verbale = await _policeServices.GetEditReport(id);

            if (verbale == null)
            {
                TempData["Error"] = "Verbale non trovato";
                return RedirectToAction("Index");
            }

            var model = new PoliceEditModel
            {
                IdVerbale = verbale.IdVerbale,
                Anagrafica = verbale.Anagrafica,
                DataViolazione = verbale.DataViolazione,
                NominativoAgente = verbale.NominativoAgente,
                DataTrascrizioneVerbale = verbale.DataTrascrizioneVerbale,
                IndirizzoViolazione = verbale.IndirizzoViolazione,
                Violazioni = verbale.ViolazioniVerbali.Select(v => new ViolazioniVerbali
                {
                    IdViolazione = v.IdViolazione,
                    Importo = v.Importo,
                    DecurtamentoPunti = v.DecurtamentoPunti
                }).ToList(),
                Importo = verbale.TotaleImporto,
                DecurtamentoPunti = verbale.TotaleDecurtamentoPunti
            };

            ViewBag.Violazioni = await _policeServices.GetAllViolation();
            return View(model);
        }

        [HttpPost("Police/Report/Edit/{IdVerbale:guid}")]
        public async Task<IActionResult> EditSave(Guid IdVerbale, PoliceEditModel model)
        {
            var report = await _policeServices.SaveEditReport(model);
            if (!report)
            {
                TempData["Error"] = "Errore durante la modifica del verbale al databse";
            }

            return RedirectToAction("Index");
        }
    }
}

