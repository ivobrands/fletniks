using System;
using fletnix.Models;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace fletnix.Controllers.Api
{
    public class ApiCustomerFeedbackController: Controller
    {
        private ICustomerFeedbackRepository _repsitory;


        public ApiCustomerFeedbackController(ICustomerFeedbackRepository repository)
        {
            _repsitory = repository;
        }

        [HttpPost("api/customerFeedback/add")]
        public JsonResult Add([FromBody]CustomerFeedbackModel customerFeedbackModel)
        {
            
            _repsitory.SaveCustomerFeedback(new CustomerFeedback 
            {
                MovieId = customerFeedbackModel.MovieId,
                CustomerMailAddress = User.Identity.Name,
                Comment = customerFeedbackModel.Comment,
                Rating = customerFeedbackModel.Rating,
                FeedbackDate = DateTime.Now
             });
 
            return Json(customerFeedbackModel);
        }
        

       [HttpPost("api/customerFeedback/Edit")]
        public JsonResult Edit([FromBody]CustomerFeedbackModel customerFeedbackModel)
        {
            
            _repsitory.EditCustomerFeedback(new CustomerFeedback 
            {
                MovieId = customerFeedbackModel.MovieId,
                CustomerMailAddress = User.Identity.Name,
                Comment = customerFeedbackModel.Comment,
                Rating = customerFeedbackModel.Rating,
                FeedbackDate = DateTime.Now
            });
 
            return Json(customerFeedbackModel);
        }
    
        [HttpPost("api/customerFeedback/delete")]
        public JsonResult Delete([FromBody]CustomerFeedbackModel customerFeedbackModel)
        {
            
            _repsitory.DeleteCustomerFeedback(new CustomerFeedback 
            {
                MovieId = customerFeedbackModel.MovieId,
                CustomerMailAddress = User.Identity.Name,
                Comment = customerFeedbackModel.Comment,
                Rating = customerFeedbackModel.Rating,
                FeedbackDate = DateTime.Now
            });
 
            return Json(customerFeedbackModel);
        }  
    }
}