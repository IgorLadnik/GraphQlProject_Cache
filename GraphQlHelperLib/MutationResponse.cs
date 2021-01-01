using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQlHelperLib
{
    public class MutationResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
