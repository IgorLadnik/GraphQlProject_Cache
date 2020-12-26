using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQlProject.Models;

namespace GraphQlProject.Type
{
    public class ReservationType : ObjectGraphType<Reservation>
    {
        public ReservationType()
        {
            Field(r => r.Id);
            Field(r => r.Name);
            Field(r => r.Phone);
            Field(r => r.Date);
            Field(r => r.Time);
            Field(r => r.Email);
            Field(r => r.TotalPeople);
        }
    }
}
