using Microsoft.AspNetCore.Server.IIS.Core;


namespace PSW_Dusan_Markovic.resources.service
{
    public class ReportService
    {
        private readonly YourDbContext _context;
        private int PeriodInDays;

        public ReportService(YourDbContext context, int periodInDays = 1)
        {
            _context = context;
            PeriodInDays = periodInDays;
        }
        private List<Tour> getSoldToursSinceDate(string authorId, DateTime dateLimit)
        {
            List<Tour> soldTours = (from tp in _context.TourPurchases
                                    where tp.DateOfPurchase > dateLimit
                                    join t in _context.Tours on tp.TourId equals t.TourId
                                    where t.AuthorId == authorId
                                    select t).ToList();
            return soldTours;
        }
        public SellingReport generateReport(string authorId)
        {
            var dateLimit = DateTime.Today.AddDays(-PeriodInDays).Date;

            var soldTours = getSoldToursSinceDate(authorId, dateLimit);

            var numberOfSoldTours = soldTours.Count;

            decimal profit = 0;
            
            foreach(var st in soldTours)
            {
                profit += st.Price;
            }

            var lastReport = _context.Reports.FirstOrDefault(r=> r.Date == dateLimit && r.AuthorId == authorId);

            decimal deltaProfit = 0;

            if(lastReport != null)
            {
                deltaProfit = (profit - lastReport.Profit) / lastReport.Profit * 100m;
            }
            var report = new SellingReport(authorId, DateTime.Today, numberOfSoldTours, profit, deltaProfit);
            _context.Reports.Add(report);
            _context.SaveChanges();

            updateFailureMonitor(DateTime.Today.Date, authorId, soldTours);

            return report;
        }

        public void updateFailureMonitor(DateTime reportDate, string authorId, List<Tour> soldTours) {
            //unsold tours
            var unsoldTours = getUnsoldTours(reportDate, authorId);
            foreach (var ut in unsoldTours)
            {
                var monitor = _context.FailureMonitors.FirstOrDefault(fm => fm.TourId == ut.TourId);
                if(monitor != null)
                {
                    monitor.TimesFailed++;
                    _context.SaveChanges();
                }
                else
                {
                    _context.FailureMonitors.Add(new TourFailureMonitor(ut.TourId, 1));
                    _context.SaveChanges();
                }
            }
            //sold tours
            foreach (var st in soldTours)
            {
                var monitor = _context.FailureMonitors.FirstOrDefault(fm => fm.TourId == st.TourId);
                if(monitor !=null)
                {
                    monitor.TimesFailed = 0;
                    _context.SaveChanges();
                }
            }

        }

        public bool generateReportForEachAuthor()
        {
            var authors = _context.Users.Where(u => u.UserType == UserType.AUTHOR).ToList();
            foreach (var a in authors)
            {
                generateReport(a.Id);       
            }
            awardAuthors();
            return true;
        }

        public bool awardAuthors()
        {
            var awardedUsers = new List<string>();
            var reportsWithMaxSoldTours = _context.Reports
                        .Where(r => r.Date == DateTime.Today.Date && r.NoOfSoldTours == _context.Reports.Where(r => r.Date == DateTime.Today.Date).Max(r => r.NoOfSoldTours))
                        .ToList();
            foreach(var r in reportsWithMaxSoldTours)
            {
                awardedUsers.Add(r.AuthorId);
            }

            if(awardedUsers.Count > 0)
            {
                foreach (var a in awardedUsers)
                {
                    var award = _context.AuthorAwards.FirstOrDefault(aa => aa.AuthorId == a);
                    if (award == null)
                    {
                        _context.AuthorAwards.Add(new AuthorAward(a, 1));
                        _context.SaveChanges();
                    }
                    else
                    {
                        award.NumberOfAwards++;
                        _context.SaveChanges();
                    }
                }
            }            
            return true;
        }

        public List<Tour> getUnsoldTours(DateTime date, string authorId)
        {
            var dateLimit = date.AddDays(-PeriodInDays);
            return (from t in _context.Tours
                        where !_context.TourPurchases.Any(tp => tp.DateOfPurchase.Date > dateLimit && tp.DateOfPurchase.Date <= date && tp.TourId == t.TourId)
                            && t.AuthorId == authorId && t.IsPublished
                        select t).ToList();
        }

        public List<Tour> getBestSellers(DateTime date, string authorId)
        {
            var dateLimit = date.AddDays(-PeriodInDays);
            List<Tour> soldTours = (from tp in _context.TourPurchases
                             where tp.DateOfPurchase.Date > dateLimit && tp.DateOfPurchase.Date <= date
                             join t in _context.Tours on tp.TourId equals t.TourId
                             where t.AuthorId == authorId
                             select t).ToList();

            var bestSellers = soldTours
                                        .GroupBy(t => t.TourId)
                                        .OrderByDescending(g => g.Count())
                                        .Take(2)
                                        .Select(g => g.FirstOrDefault())
                                        .ToList();
            return bestSellers;
        }

        public List<Tour> getFailingTours(string authorId)
        {
            return (from t in _context.Tours
                    where _context.FailureMonitors.Any(fm=> fm.TimesFailed >= 3 && fm.TourId == t.TourId) 
                        && t.AuthorId == authorId
                    select t).ToList();
        }

        public List<SellingReport> getReports(string authorId)
        {
            var reports = _context.Reports.Where(r=> r.AuthorId == authorId).ToList();
            foreach(var report in reports)
            {
                report.BestSellers = getBestSellers(report.Date, authorId);
                report.NotSoldOnce = getUnsoldTours(report.Date, authorId);
            }
            return reports;
        }
        
    }
}
