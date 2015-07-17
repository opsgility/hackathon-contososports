using System.Threading;
using System.Threading.Tasks;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Contoso.Models
{

    public interface IContosoWebContext : IDisposable
    {
        IDbSet<Player> Players { get; }
        IDbSet<Team> Teams { get; }
        IDbSet<GameStat> GameStats { get; }
        IDbSet<Match> Matches { get; }

        IDbSet<CartItem> CartItems { get; }
        IDbSet<Category> Categories { get; }
        IDbSet<OrderDetail> OrderDetails { get; }
        IDbSet<Order> Orders { get; }
        IDbSet<Product> Products { get; }
        IDbSet<ApplicationUser> Users { get; }
        IDbSet<Raincheck> RainChecks { get; }
        IDbSet<Store> Stores { get; }

        Task<int> SaveChangesAsync(CancellationToken requestAborted);
        DbEntityEntry Entry(object entity);
    }
}
