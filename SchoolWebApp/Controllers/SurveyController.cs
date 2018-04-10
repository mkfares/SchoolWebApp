using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolWebApp.Controllers
{
    public class SurveyController : Controller
    {
        // GET: Survey
        public ActionResult Index()
        {
            // Create the list of possible answers for questions
            // Each questions may have a different number of answers
            // In this case all questions have 3 possible answers
            var possibleAnswers = new List<AnswerViewModel>
            {
                new AnswerViewModel { Id = 1, Text= "Fair"},
                new AnswerViewModel { Id = 2, Text= "Average"},
                new AnswerViewModel { Id = 3, Text= "Good"},
            };

            // Get the questions from the database
            // This is a data sample
            var questions = new List<QuestionViewModel>
            {
                new QuestionViewModel { Id = 1, Text = "Question 1", PossibleAnswers = possibleAnswers },
                new QuestionViewModel { Id = 2, Text = "Question 2", PossibleAnswers = possibleAnswers },
            };

            // Create a new view model
            var model = new SurveyViewModel();

            // Copy the questions from the data model into the view model
            foreach (var item in questions)
            {
                model.Questions.Add(item);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SurveyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save the questions with selected answers from the VM to your database
                foreach (var question in model.Questions)
                {
                    // question.Id; //to get the question id;
                    // question.SelectedAnswer; // to get the answer 1, 2, 3 if not set it is null
                }

                // Return to to your home
                return RedirectToAction("Index", "Home");
            }

            // Issue with the model
            return View(model);
        }
    }

    // Put the following classes in the ViewModel folder
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? SelectedAnswer { get; set; }
        public List<AnswerViewModel> PossibleAnswers { get; set; }
    }

    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class SurveyViewModel
    {
        public SurveyViewModel()
        {
            Questions = new List<QuestionViewModel>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}