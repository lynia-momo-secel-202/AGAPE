using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public enum ExistType
    {
        Create,
        Update
    }
}
