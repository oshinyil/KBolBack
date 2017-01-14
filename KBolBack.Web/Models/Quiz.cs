using MongoDB.Bson;
using System;
using System.ComponentModel.DataAnnotations;

namespace KBolBack.Web.Models
{
    public class Quiz
    {
        public ObjectId Id { get; set; }

        [Required]
        public QuestionType Type { get; set; }

        [Required]
        public string Question { get; set; }

        public string ImageUrl { get; set; }

        public AnswerChoice[] AnswerChoices { get; set; }

        [Required]
        public string Answer { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}