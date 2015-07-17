using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Contoso.Models;
using ContosoWeb.ViewModels;

namespace ContosoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContosoWebContext _db;
        public int roco_count = 1000;

        public HomeController(IContosoWebContext context)
        {
            _db = context;
        }

        public ActionResult Index()
        {
            var viewModel = GetHomeViewModelMatches();

            // Get most popular products
            viewModel.TopSellingProducts = QueryTopSellingProducts().Take(4).ToList();
            viewModel.NewProducts = QueryNewProducts().Take(4).ToList();

            return View(viewModel);
        }

        public ActionResult Xbox()
        {
            var viewModel = GetHomeViewModelMatches();

            return View("Xbox", "_XboxLayout", viewModel);
        }

        private HomeViewModel GetHomeViewModelMatches()
        {
            var query = QueryMatches();

            var nextMatch = query.Where(x => x.Progress == MatchProgress.Pending)
                                 .OrderBy(x => x.MatchDate)
                                 .FirstOrDefault();

            var currentMatch = query.Where(x => x.Progress == MatchProgress.InProgress)
                                    .OrderBy(x => x.MatchDate)
                                    .FirstOrDefault();

            var previousMatches = query.Where(x => x.Progress == MatchProgress.Completed)
                                       .ToList();

            return new HomeViewModel {
                NextMatch = nextMatch,
                CurrentMatch = currentMatch,
                PreviousMatches = previousMatches
            };
        }


        //stubbing in a recomendations action
        public ActionResult Recomendations()
        {
            ViewBag.Message = "Your application description page.";
            //See file /home/Recomendations.cshtml for initial rendering

            // Group the order details by product and return
            // the products the top recomendations for the recomendations page

            int count = 0;
            while (count < roco_count)
            {
                _db.Products
                    .OrderByDescending(a => a.OrderDetails.Count())
                    .Take(count++)
                    .ToList();
            }

            return View();
        }

        private IQueryable<Match> QueryMatches()
        {
            return _db
                .Matches
                .Include(x => x.HomeTeam)
                .Include(x => x.HomeTeam.Players)
                .Include(x => x.HomeTeamStats)
                .Include(x => x.AwayTeam)
                .Include(x => x.AwayTeam.Players)
                .Include(x => x.AwayTeamStats)
                .AsQueryable();
        }

        private IQueryable<Product> QueryTopSellingProducts()
        {
            // Group the order details by product and return
            // the products with the highest count

            // TODO [EF] We don't query related data as yet, so the OrderByDescending isn't doing anything
            return _db.Products
                      .OrderByDescending(a => a.OrderDetails.Count())
                      .AsQueryable();
        }

        private IQueryable<Product> QueryNewProducts()
        {
            return _db.Products
                      .OrderByDescending(a => a.Created)
                      .AsQueryable();
        }
    }
}
