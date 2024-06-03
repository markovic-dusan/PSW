using PSW_Dusan_Markovic.resources.model.problem;

namespace PSW_Dusan_Markovic.resources.service
{
    public class AdministrationService
    {
        private readonly YourDbContext _context;

        public AdministrationService(YourDbContext context)
        {
            _context = context;
        }

        public bool noticeMaliciousBehavior(string userId)
        {
            var tracker = _context.MaliciousTrackers.Where(mt => mt.UserId == userId).FirstOrDefault();
            if (tracker != null)
            {
                tracker.NumberOfStrikes++;
                _context.SaveChanges();
            } else
            {
                var newTracker = new MaliciousBehaviorTracker(userId);
                newTracker.NumberOfStrikes++;
                _context.MaliciousTrackers.Add(newTracker);
                _context.SaveChanges();
            }
            return true;
        }

        public bool blockUser(string userId)
        {
            var user = _context.Users.Where(u => u.UserName == userId).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            user.IsBlocked = true;
            _context.SaveChanges();
            return true;
        }

        public bool unblockUser(string userId)
        {
            var user = _context.Users.Where(u => u.UserName == userId).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            user.IsBlocked = false;
            _context.SaveChanges();
            return true;

        }

        public List<User> getMaliciousUsers()
        {
            return (from u in _context.Users
                    where _context.MaliciousTrackers.Any(mt => mt.UserId == u.Id && mt.NumberOfStrikes >= 10) && !u.IsBlocked
                    select u).ToList();
        }

        public List<User> getBlockedUsers()
        {
            return _context.Users.Where(u=> u.IsBlocked).ToList();
        }
    }
}
