using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MStarSupply.Models
{
    public class Movimentacoes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Produto")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione o produto corretamente.")]
        public int Produto_id { get; set; }

        [Required]
        [Display(Name = "Data e hora")]
        public DateTime DataHora { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }

        [Required]
        [Display(Name = "Tipo de movimentação")]
        public bool TipoMovimentacao { get; set; }

        [Required]
        public string Local { get; set; }
        // Propriedade de navegação para o produto relacionado
        [ForeignKey("Produto_id")]
        public virtual Produto? Produto { get; set; }
    }
}
