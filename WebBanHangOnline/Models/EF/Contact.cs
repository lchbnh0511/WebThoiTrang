using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Contact")]
    public class Contact : CommonAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không đượt vượt quá 150 kí tự")]
        public string Name { get; set; }
        [StringLength(150, ErrorMessage = "Không đượt vượt quá 150 kí tự")]
        public string Email { get; set; }
        [StringLength(4000)]
        public string Message { get; set; }
        public string IsRead { get; set; }

    }
}