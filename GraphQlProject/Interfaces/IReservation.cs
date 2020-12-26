using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlProject.Models;

namespace GraphQlProject.Interfaces
{
    public interface IReservation
    {
        IEnumerable<Reservation> GetReservations();
        Reservation AddReservation(Reservation reservation);
    }
}
