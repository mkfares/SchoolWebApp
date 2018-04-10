using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolWebApp.Controllers
{
    public class Survey3Controller : Controller
    {
        // GET: Survey
        public ActionResult Index()
        {
            // Create the list of possible answers for questions
            // Each questions may have a different number of answers
            // In this case all questions have 3 possible answers
            var possibleAnswers = new List<AnswerViewModel3>
            {
                new AnswerViewModel3 { Id = 1, Text= "Fair"},
                new AnswerViewModel3 { Id = 2, Text= "Average"},
                new AnswerViewModel3 { Id = 3, Text= "Good"},
            };

            // Get the questions from the database
            // This is a data sample
            var questions = new List<QuestionViewModel3>
            {
                // Rdio button input
                new QuestionViewModel3
                {
                    Id = 1,
                    Text = "Question 1",
                    QuestionInputType = QuestionInputType.RadioButton,
                    PossibleAnswers = possibleAnswers,
                },

                // Textbox input
                new QuestionViewModel3
                {
                    Id = 2,
                    Text = "Question 2",
                    QuestionInputType = QuestionInputType.TextBox,
                },

                // Text Area input
                new QuestionViewModel3
                {
                    Id = 3,
                    Text = "Question 3",
                    QuestionInputType = QuestionInputType.TextArea,
                }
            };

            // Create a new view model
            var model = new SurveyViewModel3();

            // Copy the questions from the data model into the view model
            foreach (var item in questions)
            {
                model.Questions.Add(item);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SurveyViewModel3 model)
        {
            if (ModelState.IsValid)
            {
                // Save the questions with selected answers from the VM to your database
                foreach (var question in model.Questions)
                {
                    // question.Id; //to get the question id;
                    // question.SelectedAnswer; // to get radio box answer
                    // question.input; // to get the textbox value; if number use Parse or TryParse
                    // questions.QuestionInputType; //to get the type input: radio, textbox, textarea
                }

                // Return to to your home
                return RedirectToAction("Index", "Home");
            }

            // Issue with the model
            return View(model);
        }
    }

    // Put the following classes in the ViewModel folder
    public class QuestionViewModel3
    {
        public int Id { get; set; }

        // Question Text
        public string Text { get; set; }

        // Holds values of radio buttons
        public int? SelectedAnswer { get; set; }

        // Holds values of textboxes and textarea
        public string Input { get; set; }

        // Holds radio buttons answers
        public List<AnswerViewModel3> PossibleAnswers { get; set; }

        // Types of inputs as enum: RadioButton, TextBoxNumber, TextBoxString, TextArea
        public QuestionInputType QuestionInputType { get; set; } // enum type of input
    }

    public enum QuestionInputType
    {
        RadioButton, TextBox, TextArea
    }

    public class AnswerViewModel3
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class SurveyViewModel3
    {
        public SurveyViewModel3()
        {
            Questions = new List<QuestionViewModel3>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuestionViewModel3> Questions { get; set; }
    }
}