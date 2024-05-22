using PSW_Dusan_Markovic.resources.model.problem;
using PSW_Dusan_Markovic.resources.model;
using Microsoft.Extensions.Logging;

namespace PSW_Dusan_Markovic.resources.service
{
    public class ProblemService
    {
        private readonly YourDbContext _context;
        private readonly Action<Problem, EnumProblemStatus> _statusChangedHandler;

        public ProblemService(YourDbContext context)
        {
            _context = context;
            _statusChangedHandler = (problem, newStatus) =>
            {
                //trigger notifications & fill event log
                _context.ProblemStateChanges.Add(new ProblemStatusChangedEvent(problem.ProblemId, newStatus));
                _context.SaveChanges();
            };
        }

        public List<Problem> getAllProblems() {
            return _context.Problems.ToList();
        }

        public bool reportProblem(Problem problem)
        {
            //subscribe to StatusChanged
            problem.StatusChanged += (sender, e) => _statusChangedHandler(problem, e.NewStatus);

            _context.Problems.Add(problem);
            _context.SaveChanges();
            problem.changeStatus(EnumProblemStatus.ON_HOLD);
            return true;
        }

        public bool reviewProblem(int problemId, bool isValid)
        {
            Problem problem = getProblemById(problemId);
            if (problem == null)
            {
                return false;
            }
            //subscribe to StatusChanged
            problem.StatusChanged += (sender, e) => _statusChangedHandler(problem, e.NewStatus);

            if (isValid)
            {
                problem.changeStatus(EnumProblemStatus.ON_HOLD);
            } else
            {
                problem.changeStatus(EnumProblemStatus.DISMISSED);
            }
            return true;
        }

        public bool solveProblem(int problemId)
        {
            Problem problem = getProblemById(problemId);
            if (problem == null)
            {
                return false;
            }
            //subscribe to StatusChanged
            problem.StatusChanged += (sender, e) => _statusChangedHandler(problem, e.NewStatus);

            problem.changeStatus(EnumProblemStatus.SOLVED);
            return true;
        }

        public bool sendProblemToRevision(int problemId)
        {
            Problem problem = getProblemById(problemId);
            if (problem == null)
            {
                return false;
            }
            //subscribe to StatusChanged
            problem.StatusChanged += (sender, e) => _statusChangedHandler(problem, e.NewStatus);

            problem.changeStatus(EnumProblemStatus.ON_REVISION);
            return true;
        }

        public Problem getProblemById(int problemId)
        {
            var problem = _context.Problems.FirstOrDefault(p=> p.ProblemId == problemId);

            return problem;
        }

        public List<Problem> getUserProblems(string userId)
        {
            var user = _context.Users.FirstOrDefault(u=> u.Id == userId);
            if(user == null)
            {
                return new List<Problem>();
            }
            switch(user.UserType)
            {
                case UserType.TOURIST:
                    return getTouristProblems(userId);
                case UserType.ADMIN:
                    return getAdminProblems();
                default:
                    return getAuthorProblems(userId);
            }
        }

        public List<Problem> getTouristProblems(string userId)
        {
            return _context.Problems.Where(p => p.TouristId == userId).ToList();
        }

        public List<Problem> getAdminProblems()
        {
            var allProblems = getAllProblems();
            var adminProblems = new List<Problem>();
            foreach(var p in allProblems)
            {
                if(getProblemStatus(p.ProblemId) == EnumProblemStatus.ON_REVISION)
                {
                    adminProblems.Add(p);
                }
            }
            return adminProblems;
        }

        public List<Problem> getAuthorProblems(string userId)
        {
            return (from  p in _context.Problems
                    where _context.Tours.Any(t=> t.AuthorId == userId && t.TourId == p.TourId) 
                    select p).ToList();
        }

        public EnumProblemStatus getProblemStatus(int problemId)
        {
            var problem = _context.Problems.Find(problemId);
            if(problem == null)
            {
                return EnumProblemStatus.ERROR;
            }
            var events = _context.ProblemStateChanges
                    .Where(psc => psc.ProblemId == problemId)
                    .OrderBy(psc => psc.Timestamp)
                    .ToList();
            foreach(var e in events)
            {
                problem.changeStatus(e.NewStatus);
            }
            return problem.Status;
        }

        public List<ProblemStatusChangedEvent> getProblemStatusHistory(int problemId)
        {
            return _context.ProblemStateChanges
                    .Where(psc => psc.ProblemId == problemId)
                    .OrderBy(psc => psc.Timestamp)
                    .ToList();
        }
    }
}
