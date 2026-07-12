using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFLTeams.Models;

namespace NFLTeams.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {
        private TeamContext context;

        public TeamsController(TeamContext ctx)
        {
            context = ctx;
        }

        public IActionResult Index()
        {
            var teams = context.Teams
                .Include(t => t.Conference)
                .Include(t => t.Division)
                .OrderBy(t => t.Name)
                .ToList();
            return View(teams);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Conferences = context.Conferences.ToList();
            ViewBag.Divisions = context.Divisions.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Team team, string ConferenceID, string DivisionID)
        {
            if (string.IsNullOrWhiteSpace(team.TeamID))
            {
                ModelState.AddModelError("TeamID", "Team ID is required.");
                ViewBag.Conferences = context.Conferences.ToList();
                ViewBag.Divisions = context.Divisions.ToList();
                return View(team);
            }

            team.Conference = context.Conferences.Find(ConferenceID);
            team.Division = context.Divisions.Find(DivisionID);

            context.Teams.Add(team);
            context.SaveChanges();
            TempData["message"] = $"{team.Name} was added successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var team = context.Teams
                .Include(t => t.Conference)
                .Include(t => t.Division)
                .FirstOrDefault(t => t.TeamID == id);

            if (team == null)
                return RedirectToAction("Index");

            ViewBag.Conferences = context.Conferences.ToList();
            ViewBag.Divisions = context.Divisions.ToList();
            return View(team);
        }

        [HttpPost]
        public IActionResult Edit(Team team, string ConferenceID, string DivisionID)
        {
            team.Conference = context.Conferences.Find(ConferenceID);
            team.Division = context.Divisions.Find(DivisionID);

            context.Teams.Update(team);
            context.SaveChanges();
            TempData["message"] = $"{team.Name} was updated successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var team = context.Teams
                .Include(t => t.Conference)
                .Include(t => t.Division)
                .FirstOrDefault(t => t.TeamID == id);

            if (team == null)
                return RedirectToAction("Index");

            return View(team);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(string TeamID)
        {
            var team = context.Teams.Find(TeamID);
            if (team != null)
            {
                context.Teams.Remove(team);
                context.SaveChanges();
                TempData["message"] = $"{team.Name} was deleted successfully";
            }
            return RedirectToAction("Index");
        }
    }
}