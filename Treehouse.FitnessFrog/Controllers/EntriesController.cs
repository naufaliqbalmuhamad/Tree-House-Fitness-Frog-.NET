using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Data;
using Treehouse.FitnessFrog.Models;
using static Treehouse.FitnessFrog.Models.Entry;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository();
        }

        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries();

            // Calculate the total activity.
            double totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            ViewBag.TotalActivity = totalActivity;
            ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

            return View(entries);
        }

        [ActionName("Add"), HttpPost] //fungsi untuk menambah data/add post
        public ActionResult AddPost(Entry entry)
        {
            if (ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);

                return RedirectToAction("Index");
            }
            return View(entry);
        }

        public ActionResult Add()
        {
            ViewBag.DropDownItem = new SelectList(Data.Data.Activities, "Id", "Name");

            var Entry = new Entry()
            {
                Date = DateTime.Today
            };
            return View(Entry);
        }

        [ActionName("Edit"), HttpPost] //fungsi untuk menambah data/add post
        public ActionResult EditPost(Entry entry)
        {
            if (ModelState.IsValid)
            {
                _entriesRepository.UpdateEntry(entry);

                return RedirectToAction("Index");
            }
            return View(entry);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.DropDownItem = new SelectList(Data.Data.Activities, "Id", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Entry Entry = _entriesRepository.GetEntry((int) id);

            return View(Entry);
        }

        [ActionName("Delete"), HttpPost]
        public ActionResult Delete(int id)
        {
            _entriesRepository.DeleteEntry(id);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Entry Entry = _entriesRepository.GetEntry((int) id);

            return View(Entry);
        }

    }
}