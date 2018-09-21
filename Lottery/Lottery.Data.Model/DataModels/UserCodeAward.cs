using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lottery.Data.Model.DataModels
{
    [Table("dbo.UserCodeAwards")]
    public class UserCodeAward : IEntity
    {
        [Key]
        [Column("UserCodeAwardID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("UserCodeID")]
        public int UserCodeId { get; set; }
        public UserCode UserCode { get; set; }
        [Column("AwardID")]
        public int AwardId { get; set; }
        public Award Award { get; set; }
        public DateTime WonAt { get; set; }

        public UserCodeAward() { }

        public UserCodeAward(Award Award, UserCode UserCode, DateTime WonAt)
        {
            this.Award = Award;
            this.UserCode = UserCode;
            this.WonAt = WonAt;
        }
    }
}