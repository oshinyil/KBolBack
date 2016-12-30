using System.ComponentModel.DataAnnotations;

namespace KBolBack.Web.Models
{
    public class AnswerChoice
    {
        [Required]
        public string No { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}