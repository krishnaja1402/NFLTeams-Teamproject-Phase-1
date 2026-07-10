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
    }
}