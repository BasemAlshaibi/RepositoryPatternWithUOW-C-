using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Author
    {
         public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }
    }
}