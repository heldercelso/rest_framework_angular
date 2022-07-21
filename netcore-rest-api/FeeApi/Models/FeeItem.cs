
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FeeApi.Models
{
    public class FeeItem
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Preencha o campo Cpf.")]
        public string client { get; set; }
        
        [ForeignKey("client")]
        public Client clientF { get; set; }
        
        [Required(ErrorMessage = "Preencha o campo Valor.")]
        public double source_currency_amount { get; set; }
        public double conversion_result { get; set; }
        public double fee { get; set; }
        public string formula { get; set; }
        public FeeItem()
        { 
            formula = "Resultado_BRL = (Valor_EUR * Taxa_Cambio) * (1 + Taxa_Segmento)";
        }
    }
    public class Client
    {
        public int Id { get; set; }
        [Key]
        [Required(ErrorMessage = "Preencha o campo Cpf.")]
        public string cpf { get; set; }
        
        [Required(ErrorMessage = "Preencha o campo Nome.")]
        public string name { get; set; }
        
        [Required(ErrorMessage = "Preencha o campo segmento. Se não há opções disponíveis, registre um segmento primeiro.")]
        public string segment { get; set; }
        
        [ForeignKey("segment")]
        public Segment segmentF { get; set; }
    }

    public class Segment
    {
        public int Id { get; set; }
        [Key]
        [Required(ErrorMessage = "Preencha o campo Nome.")]
        public string name { get; set; } = null!;
        
        [Required(ErrorMessage = "Preencha o campo Taxa.")]
        public double fee { get; set; }
    }

}