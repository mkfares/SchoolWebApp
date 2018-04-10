using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolWebApp.Controllers
{
    public class Survey2Controller : Controller
    {
        // GET: Survey
        public ActionResult Index()
        {
            // Get the questions from the database
            // This is a data sample
            var questions = new List<QuestionViewModel2>
            {
                new QuestionViewModel2 { Id = 1, Text = "Question 1" },
                new QuestionViewModel2 { Id = 2, Text = "Question 2" },
            };

            // Create a new view model
            var model = new SurveyViewModel2();

            // Copy the questions from the data model into the view model
            foreach (var item in questions)
            {
                model.Questions.Add(item);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SurveyViewModel2 model)
        {
            if (ModelState.IsValid)
            {
                // Save the questions with selected answers from the VM to your database
                foreach (var question in model.Questions)
                {
                    // question.Id; //to get the question id;
                    // question.Score; // to get the answer 1, 2, 3; if not set it is null
                }

                // Return to to your home
                return RedirectToAction("Index", "Home");
            }

            // Issue with the model
            return View(model);
        }
    }

    // Put the following classes in the ViewModel folder
    public class QuestionViewModel2
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? Score { get; set; }
    }

    public class SurveyViewModel2
    {
        public SurveyViewModel2()
        {
            Questions = new List<QuestionViewModel2>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestionViewModel2> Questions { get; set; }
    }
}