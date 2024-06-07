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
        private readonly LoginService _loginService;

        public ProblemController(ProblemService problemService, LoginService loginService)
        {
            _problemService = problemService;
            _loginService = loginService;
        }
        [HttpGet]
        public ActionResult<List<Problem>> getAllProblems() {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.ADMIN))
            {
                return Ok(_problemService.getAllProblems());
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost()]
        public ActionResult<bool> reportProblem(Problem problem)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST))
            {
                return Ok(_problemService.reportProblem(problem));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{problemId}")]
        public ActionResult<bool> getProblemById(int problemId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                var problem = getProblemById(problemId);
                if (problem == null)
                {
                    return BadRequest("Problem not found");
                }
                return Ok(problem);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("{problemId}/review")]
        public ActionResult<bool> reviewProblem(int problemId, [FromBody]  bool isValid)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.ADMIN))
            {
                var problem = _problemService.reviewProblem(problemId, isValid);
                if (!problem)
                {
                    return BadRequest("Problem not found");
                }
                return Ok(problem);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("{problemId}/solve")]
        public ActionResult<bool> solveProblem(int problemId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                var problem = _problemService.solveProblem(problemId);
                if (!problem)
                {
                    return BadRequest("Problem not found");
                }
                return Ok(problem);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("{problemId}/revision")]
        public ActionResult<bool> sendProblemToRevision(int problemId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.AUTHOR))
            {
                var problem = _problemService.sendProblemToRevision(problemId);
                if (!problem)
                {
                    return BadRequest("Problem not found");
                }
                return Ok(problem);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("/api/users/{userId}/problem")]
        public ActionResult<List<Problem>> getUserProblems(string userId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                return Ok(_problemService.getUserProblems(userId));
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{problemId}/status")]
        public ActionResult<EnumProblemStatus> getProblemStatus(int problemId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                var status = _problemService.getProblemStatus(problemId);
                if (status == EnumProblemStatus.ERROR)
                {
                    return BadRequest("Invalid status: " + status.ToString());
                }
                return Ok(status);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("{problemId}/history")]
        public ActionResult<EnumProblemStatus> getProblemStatusHistory(int problemId)
        {
            if (_loginService.authorize(HttpContext.Request.Headers["Authorization"], UserType.TOURIST, UserType.AUTHOR))
            {
                return Ok(_problemService.getProblemStatusHistory(problemId));
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
