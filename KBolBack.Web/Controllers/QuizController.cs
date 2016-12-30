using KBolBack.Web.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace KBolBack.Web.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private static string mongoDBConnection = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;
        private static string quizDatabaseName = ConfigurationManager.AppSettings["QuizDatabaseName"];

        // GET: Question
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var quiz = new Quiz
            {
                AnswerChoices = new AnswerChoice[4]
                {
                    new AnswerChoice { No = "A" },
                    new AnswerChoice { No = "B" },
                    new AnswerChoice { No = "C" },
                    new AnswerChoice { No = "D" },
                }
            };
            return View(quiz);
        }

        [HttpPost]
        public ActionResult Create(Quiz quiz)
        {
            if (quiz !=  null)
            {
                var client = new MongoClient(mongoDBConnection);
                var db = client.GetDatabase(quizDatabaseName);
                var collection = db.GetCollection<Quiz>("Quiz");

                if (quiz.Type == QuestionType.ShortAnswer)
                {
                    quiz.AnswerChoices = null;
                }

                quiz.IsActive = true;
                quiz.IsDeleted = false;
                quiz.CreatedBy = User.Identity.GetUserName();
                quiz.CreatedDate = DateTime.Now;

                collection.InsertOne(quiz);

                RedirectToAction("Index");
            }

            return View(quiz);
        }
    }
}