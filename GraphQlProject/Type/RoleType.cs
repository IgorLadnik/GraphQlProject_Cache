using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Types;
using GraphQlProject.Models;

namespace GraphQlProject.Type
{
    public class RoleType : ObjectGraphType<Role>
            {
        public RoleType()
        {
            Field(r => r.Id);
            Field(r => r.StrId);
            Field(r => r.Name);
            Field(r => r.Description);
        }
    }
}


