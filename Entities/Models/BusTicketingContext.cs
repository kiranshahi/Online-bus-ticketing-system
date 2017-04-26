using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Entities.Models.Mapping;

namespace Entities.Models
{
    public partial class BusTicketingContext : DbContext
    {
        static BusTicketingContext()
        {
            Database.SetInitializer<BusTicketingContext>(null);
        }

        public BusTicketingContext()
            : base("Name=BusTicketingContext")
        {
        }

        public DbSet<BookSeat> BookSeats { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Travel> Travels { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BookSeatMap());
            modelBuilder.Configurations.Add(new BusMap());
            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new RouteMap());
            modelBuilder.Configurations.Add(new ScheduleMap());
            modelBuilder.Configurations.Add(new TravelMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
