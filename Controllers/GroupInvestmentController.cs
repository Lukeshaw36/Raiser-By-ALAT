using GROUP2.Dtos;
using GROUP2.Models;
using GROUP2.Services.GroupInvestment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GROUP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupInvestmentController : ControllerBase
    {
        private readonly IInterestRepository _interest;
        private readonly IInvetmentRepo _investmentRepository;

        public GroupInvestmentController(IInterestRepository interest, IInvetmentRepo investmentRepository)
        {
            _interest = interest;
            _investmentRepository = investmentRepository;
          //  _investmentService = investmentService;
        }
        [HttpGet]
        [Route("investmentinterest/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetInvestmentInterestById(int id)
        {
            var investmentInterest = _investmentRepository.GetById(id);
            if (investmentInterest == null)
            {
                return NotFound();
            }
            return Ok(investmentInterest);
        }

        [HttpGet]
        [Route("investmentinterest-get-all")]
        public IActionResult GetAllInvestmentInterests()
        {
            var investmentInterests = _investmentRepository.GetAll();
            return Ok(investmentInterests);
        }

        [HttpPost]
        [Route("investmentinterest-create")]
        [Authorize(Roles ="Admin")]
        public IActionResult AddInvestmentInterest([FromBody] InvestmentDtos investmentInterest)
        {
            _investmentRepository.Add(investmentInterest);
            return CreatedAtAction(nameof(GetInvestmentInterestById), new { id = investmentInterest.Id }, investmentInterest);
        }

        [HttpPut]
        [Route("investmentinterest/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateInvestmentInterest(int id, [FromBody] InvestmentDtos investmentInterest)
        {
            if (id != investmentInterest.Id)
            {
                return BadRequest("Id mismatch between route parameter and request body");
            }

            _investmentRepository.Update(investmentInterest);
            return Ok(investmentInterest);
        }

        [HttpDelete]
        [Route("investmentinterest/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteInvestmentInterest(int id)
        {
            _investmentRepository.Delete(id);
            return NoContent();
        }

        // CRUD operations for Interests

        [HttpGet]
        [Route("interest/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetInterestById(long id)
        {
            var interest = _interest.GetById(id);
            if (interest == null)
            {
                return NotFound();
            }
            return Ok(interest);
        }

        [HttpGet]
        [Route("interest-get-all")]
        public IActionResult GetAllInterests()
        {
            var interests = _interest.GetAll();
            return Ok(interests);
        }

        [HttpPost]
        [Route("interest-create")]
       // [Authorize(Roles = "Admin")]
        public IActionResult AddInterest([FromBody] InterestDtos interest)
        {
            _interest.Add(interest);
            return CreatedAtAction(nameof(GetInterestById), new { id = interest.Id }, interest);
        }

        [HttpPut]
        [Route("interest/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateInterest(long id, [FromBody] InterestDtos interest)
        {
            if (id != interest.Id)
            {
                return BadRequest("Id mismatch between route parameter and request body");
            }

            _interest.Update(interest);
            return Ok(interest);
        }

        [HttpDelete]
        [Route("interest/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteInterest(long id)
        {
            _interest.Delete(id); 
            return NoContent();
        }

        [HttpPost]
        [Route("{interestId}/addInvestment")]
       // [Authorize(Roles = "Admin")]
        public IActionResult AddInvestmentToInterest(long interestId, [FromBody] InvestmentDtos investmentInterest)
        {
            try
            {
                _interest.AddInvestmentToInterest(interestId, investmentInterest);
                return Ok("Investment added to the interest successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it based on your application's requirements.
                return BadRequest($"Failed to add investment to the interest. Error: {ex.Message}");
            }
        }

        //[HttpPost]
        //[Route("invest")]
        //public IActionResult Invest([FromBody] InvestmentRequest request)
        //{
        //    // Validate request

        //    _investmentService.Invest(request.UserId, request.InterestId, request.InvestmentInterestId, (int)request.Amount, request.AccountId);

        //    return Ok("Investment successful"); // You may return a more meaningful response
        //}
    }
}
