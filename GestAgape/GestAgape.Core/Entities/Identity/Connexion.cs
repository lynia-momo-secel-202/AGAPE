using GestAgape.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities
{
    public class Connexion: BaseEntity
    {
        public string? IPAddress { get; set; }
        public string? Place { get; set; }
        public string? UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public ApplicationUsers? User { get; set; }
    }
}
