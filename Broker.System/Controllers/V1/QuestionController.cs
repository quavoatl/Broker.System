// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Broker.System.Contracts.V1;
// using Broker.System.Domain;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Broker.System.Controllers.V1
// {
//     public class QuestionController : Controller
//     {
//         private readonly List<Question> _questions = null;
//
//         public QuestionController()
//         {
//             _questions = new List<Question>();
//             _questions.Add(new Question()
//                 {BrokerId = new Guid(), Name = "Question(1)", Description = "Question body description (1)"});
//             _questions.Add(new Question()
//                 {BrokerId = new Guid(), Name = "Question(2)", Description = "Question body description (2)"});
//             _questions.Add(new Question()
//                 {BrokerId = new Guid(), Name = "Question(3)", Description = "Question body description (3)"});
//             _questions.Add(new Question()
//                 {BrokerId = new Guid(), Name = "Question(4)", Description = "Question body description (4)"});
//             _questions.Add(new Question()
//                 {BrokerId = new Guid(), Name = "Question(5)", Description = "Question body description (5)"});
//         }
//
//         [HttpGet(ApiRoutes.Question.GetAll)]
//         public IActionResult GetAll(int id)
//         {
//             return Ok(_questions.Where(q => q.BrokerId.Equals(id)));
//         }
//     }
// }