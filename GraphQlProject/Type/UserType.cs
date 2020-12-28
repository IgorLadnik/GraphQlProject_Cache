using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Types;
using GraphQlProject.Models;

namespace GraphQlProject.Type
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Field(u => u.Id);
            Field(u => u.StrId);
            Field(u => u.UserName);
            Field(u => u.Password);
            Field(u => u.Permissions);
        }
    }
}
