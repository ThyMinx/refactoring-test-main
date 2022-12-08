using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppLibrary
{
    public class Referral
    {
        public Guid Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Service Service { get; set; }
        public Region Region { get; internal set; }
    }
}
