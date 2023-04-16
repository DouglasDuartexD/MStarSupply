using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MStarSupply.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Produto")]
        public string Nome { get; set; }
        [Required]
        public string Fabricante { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public bool Ativo { get; set; }

    }
}
