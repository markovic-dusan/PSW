using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSW_Dusan_Markovic.resources.model.problem;

namespace PSW_Dusan_Markovic.resources.controller
{
    [Route("api/problem")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly ProblemService _problemService;

        public ProblemController(ProblemService problemService)
        {
            _problemService = problemService;
        }
        [HttpGet]
        public ActionResult<List<Problem>> getAllProblems() {
            return Ok(_problemService.getAllProblems());
        }

        [HttpPost()]
        public ActionResult<bool> reportProblem(Problem problem)
        {
            return Ok(_problemService.reportProblem(problem));
        }

        [HttpGet("{problemId}")]
        public ActionResult<bool> getProblemById(int problemId)
        {
            var problem = getProblemById(problemId);
            if(problem == null)
            {
                return BadRequest("Problem not found");
            }
            return Ok(problem);
        }

        [HttpPost("{problemId}/review")]
        public ActionResult<bool> reviewProblem(int problemId, [FromBody]  bool isValid)
        {
            var problem = _problemService.reviewProblem(problemId, isValid);
            if (!problem)
            {
                return BadRequest("Problem not found");
            }
            return Ok(problem);
        }

        [HttpPost("{problemId}/solve")]
        public ActionResult<bool> solveProblem(int problemId)
        {
            var problem = _problemService.solveProblem(problemId);
            if (!problem)
            {
                return BadRequest("Problem not found");
            }
            return Ok(problem);
        }

        [HttpPost("{problemId}/revision")]
        public ActionResult<bool> sendProblemToRevision(int problemId)
        {
            var problem = _problemService.sendProblemToRevision(problemId);
            if (!problem)
            {
                return BadRequest("Problem not found");
            }
            return Ok(problem);
        }

        [HttpGet("/api/users/{userId}/problem")]
        public ActionResult<List<Problem>> getUserProblems(string userId)
        {
            return Ok(_problemService.getUserProblems(userId));
        }

        [HttpGet("{problemId}/status")]
        public ActionResult<EnumProblemStatus> getProblemStatus(int problemId)
        {
            var status = _problemService.getProblemStatus(problemId);
            if(status == EnumProblemStatus.ERROR)
            {
                return BadRequest("Invalid status: "+ status.ToString());
            }
            return Ok(status);
        }

        [HttpGet("{problemId}/history")]
        public ActionResult<EnumProblemStatus> getProblemStatusHistory(int problemId)
        {
            return Ok(_problemService.getProblemStatusHistory(problemId));  
        }
    }
}
