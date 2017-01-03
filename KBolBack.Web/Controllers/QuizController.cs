using KBolBack.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using MongoDB.Driver;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KBolBack.Web.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private static string mongoDBConnection = CloudConfigurationManager.GetSetting("MongoDBConnection");
        private static string quizDatabaseName = CloudConfigurationManager.GetSetting("QuizDatabaseName");

        public ActionResult Index()
        {
            var client = new MongoClient(mongoDBConnection);
            var db = client.GetDatabase(quizDatabaseName);
            var collection = db.GetCollection<Quiz>("Quiz");
             
            var filter = Builders<Quiz>.Filter.Empty;
            var quizzes = collection.Find(filter).SortByDescending(q => q.CreatedDate).ToList();

            return View(quizzes);
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
        public async Task<ActionResult> Create(Quiz quiz)
        {
            if (quiz != null && quiz.Type == QuestionType.ShortAnswer)
            {
                ModelState.Remove("AnswerChoices[0].No");
                ModelState.Remove("AnswerChoices[0].Answer");
                ModelState.Remove("AnswerChoices[1].No");
                ModelState.Remove("AnswerChoices[1].Answer");
                ModelState.Remove("AnswerChoices[2].No");
                ModelState.Remove("AnswerChoices[2].Answer");
                ModelState.Remove("AnswerChoices[3].No");
                ModelState.Remove("AnswerChoices[3].Answer");
            }

            if (ModelState.IsValid)
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

                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    quiz.ImageUrl = await UploadImageAsync(Request.Files[0]);
                }

                collection.InsertOne(quiz);

                return RedirectToAction("Index");
            }

            return View(quiz);
        }

        private async Task<string> UploadImageAsync(HttpPostedFileBase file)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(CloudConfigurationManager.GetSetting("BlobContainerName"));

            var blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(file.FileName));
            await blob.UploadFromStreamAsync(file.InputStream);

            return blob.Uri.AbsoluteUri;
        }

        private static string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }
    }
}