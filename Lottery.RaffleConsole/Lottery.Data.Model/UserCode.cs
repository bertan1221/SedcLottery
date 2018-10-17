using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lottery.Data.Model
{
    [Table("UserCodes")]
    public class UserCode : IEntity
    {
        [Key]
        [Column("UserCodeID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Column("CodeID")]
        public int CodeId { get; set; }
        public Code Code { get; set; }
        public DateTime SentAt { get; set; }
    }
}
