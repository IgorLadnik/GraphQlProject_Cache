using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlProject.Data;
using GraphQlProject.Interfaces;
using GraphQlProject.Models;

namespace GraphQlProject.Services
{
    public class ReservationService : IReservation
    {
        private readonly GraphQLDbContext _dbContext;

        public ReservationService(GraphQLDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Reservation AddReservation(Reservation reservation)
        {
            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();
            return reservation;
        }

        public IEnumerable<Reservation> GetReservations() => _dbContext.Reservations;
    }
}
