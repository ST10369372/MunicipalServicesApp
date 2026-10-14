using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp
{
    // ------------------------------------------------------------------
    // LocalEvent model
    // ------------------------------------------------------------------
    public class LocalEvent
    {
        public string EventId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }   // 1 (highest) - 5 (lowest), drives the priority queue

        public override string ToString()
        {
            return string.Format("{0,-22} {1,-18} {2:dd MMM yyyy}  {3}",
                Title, "[" + Category + "]", Date, Venue);
        }
    }

    // ------------------------------------------------------------------
    // EventManager - single source of truth for all event data.
    // Advanced data structures used (each maps to a rubric requirement):
    //
    //   STACKS            _recentlyViewed        - history of events the user opened
    //                    _searchTerms           - stack of recent search keywords (undo-style)
    //   QUEUES           _announcements         - FIFO rotation of announcements
    //   PRIORITY QUEUE   _featuredByPriority    - SortedDictionary<int, Queue<LocalEvent>>
    //   HASH TABLE       _eventById             - Dictionary<string, LocalEvent> O(1) lookup
    //   SORTED DICTIONARY _eventsByDate         - events indexed in date order
    //   SETS             _categories            - HashSet<string> unique categories
    //                    _eventDates            - HashSet<DateTime> unique dates
    //   SEARCH PATTERNS  _categorySearchCounts  - Dictionary<string,int> preference profile
    // ------------------------------------------------------------------
    public static class EventManager
    {
        // Storage of all events
        private static readonly List<LocalEvent> _allEvents = new List<LocalEvent>();

        // Hash table: event id -> event (fast direct lookup)
        private static readonly Dictionary<string, LocalEvent> _eventById = new Dictionary<string, LocalEvent>();

        // Sorted dictionary: date -> events on that date, always kept in chronological order
        private static readonly SortedDictionary<DateTime, List<LocalEvent>> _eventsByDate = new SortedDictionary<DateTime, List<LocalEvent>>();

        // Sets: unique categories and unique event dates
        private static readonly HashSet<string> _categories = new HashSet<string>();
        private static readonly HashSet<DateTime> _eventDates = new HashSet<DateTime>();

        // Stacks: browsing history and search history
        private static readonly Stack<LocalEvent> _recentlyViewed = new Stack<LocalEvent>();
        private static readonly Stack<string> _searchTerms = new Stack<string>();

        // Queue: announcements shown in FIFO order
        private static readonly Queue<string> _announcements = new Queue<string>();

        // Priority queue (implemented with SortedDictionary so it works on .NET Framework):
        // priority (1=highest) -> queue of events with that priority
        private static readonly SortedDictionary<int, Queue<LocalEvent>> _featuredByPriority = new SortedDictionary<int, Queue<LocalEvent>>();

        // Search-pattern analysis for the recommendation feature
        private static readonly Dictionary<string, int> _categorySearchCounts = new Dictionary<string, int>();
        private static int _totalSearches = 0;

        static EventManager()
        {
            SeedEvents();
        }

        // ----------------------------------------------------------------
        // Seeding
        // ----------------------------------------------------------------
        private static void SeedEvents()
        {
            AddEvent(new LocalEvent { EventId = "E01", Title = "Community Cleanup Day", Category = "Environment", Date = NextDate(3), Venue = "Century City Park", Description = "Join neighbours to clean up local parks and waterways. Gloves and bags provided.", Priority = 1 });
            AddEvent(new LocalEvent { EventId = "E02", Title = "Load Shedding Info Session", Category = "Utilities", Date = NextDate(2), Venue = "Town Hall", Description = "Municipal engineers explain the new load shedding schedule and answer questions.", Priority = 1 });
            AddEvent(new LocalEvent { EventId = "E03", Title = "Youth Soccer Tournament", Category = "Sports", Date = NextDate(6), Venue = "Greenpoint Stadium", Description = "Inter-ward youth soccer finals. Free entry, food stalls on site.", Priority = 2 });
            AddEvent(new LocalEvent { EventId = "E04", Title = "Free Health Screening", Category = "Health", Date = NextDate(9), Venue = "Community Clinic, Khayelitsha", Description = "Free blood pressure, glucose and HIV screening. No appointment needed.", Priority = 2 });
            AddEvent(new LocalEvent { EventId = "E05", Title = "Heritage Day Festival", Category = "Arts & Culture", Date = NextDate(12), Venue = "Company's Garden", Description = "Celebrate South African heritage with music, dance and traditional food.", Priority = 3 });
            AddEvent(new LocalEvent { EventId = "E06", Title = "Water Restrictions Briefing", Category = "Utilities", Date = NextDate(4), Venue = "Council Chambers", Description = "Briefing on Level 2 water restrictions and borehole registration.", Priority = 1 });
            AddEvent(new LocalEvent { EventId = "E07", Title = "Street Library Launch", Category = "Community", Date = NextDate(8), Venue = "Mowbray Library", Description = "Grand opening of a new 24-hour street book exchange. Bring a book, take a book.", Priority = 4 });
            AddEvent(new LocalEvent { EventId = "E08", Title = "Crime Prevention Workshop", Category = "Public Safety", Date = NextDate(5), Venue = "SAPS Community Hall", Description = "Neighbourhood watch training and home safety tips from SAPS.", Priority = 2 });
            AddEvent(new LocalEvent { EventId = "E09", Title = "Parkrun Launch", Category = "Sports", Date = NextDate(7), Venue = "Rondebosch Common", Description = "Weekly free 5km timed run - all fitness levels welcome.", Priority = 4 });
            AddEvent(new LocalEvent { EventId = "E10", Title = "Art in the Park", Category = "Arts & Culture", Date = NextDate(15), Venue = "De Waal Park", Description = "Local artists exhibit and sell work. Live music from 12:00.", Priority = 5 });
            AddEvent(new LocalEvent { EventId = "E11", Title = "Recycling Drive", Category = "Environment", Date = NextDate(10), Venue = "Milnerton Civic Centre", Description = "Drop off e-waste, paper, glass and tins. Certificates for schools.", Priority = 3 });
            AddEvent(new LocalEvent { EventId = "E12", Title = "Free Eye Testing", Category = "Health", Date = NextDate(11), Venue = "Library Auditorium", Description = "Optometry students offer free vision screening for pensioners and children.", Priority = 4 });

            _announcements.Enqueue("Water supply maintenance in Ward 4 this Thursday 09:00-14:00.");
            _announcements.Enqueue("New refuse collection calendar effective 1 October - check your ward.");
            _announcements.Enqueue("Registration for the municipal bursary fund closes end of month.");
            _announcements.Enqueue("Street renaming public participation meetings start next week.");
        }

        // Helper (avoids repeating the Date math); small shim class removed below in AddEvent.
        private static DateTime NextDate(int days) => DateTime.Today.AddDays(days);

        // ----------------------------------------------------------------
        // Core add / index into every structure
        // ----------------------------------------------------------------
        public static void AddEvent(LocalEvent ev)
        {
            _allEvents.Add(ev);
            _eventById[ev.EventId] = ev;                       // hash table

            if (!_eventsByDate.ContainsKey(ev.Date.Date))
                _eventsByDate[ev.Date.Date] = new List<LocalEvent>();
            _eventsByDate[ev.Date.Date].Add(ev);               // sorted dictionary

            _categories.Add(ev.Category);                      // set (unique categories)
            _eventDates.Add(ev.Date.Date);                     // set (unique dates)

            if (!_featuredByPriority.ContainsKey(ev.Priority))
                _featuredByPriority[ev.Priority] = new Queue<LocalEvent>();
            _featuredByPriority[ev.Priority].Enqueue(ev);      // priority queue
        }

        // ----------------------------------------------------------------
        // Sets
        // ----------------------------------------------------------------
        public static IEnumerable<string> GetCategories() => _categories.OrderBy(c => c);
        public static HashSet<DateTime> GetEventDates() => new HashSet<DateTime>(_eventDates);

        // ----------------------------------------------------------------
        // Upcoming events: read straight from the sorted dictionary => always chronological
        // ----------------------------------------------------------------
        public static List<LocalEvent> GetUpcomingEvents()
        {
            List<LocalEvent> result = new List<LocalEvent>();
            foreach (var kvp in _eventsByDate)                 // keys ascending = chronological
            {
                if (kvp.Key < DateTime.Today) continue;
                result.AddRange(kvp.Value.OrderBy(e => e.Priority));
            }
            return result;
        }

        // ----------------------------------------------------------------
        // Search (also records search patterns for the recommender)
        // ----------------------------------------------------------------
        public static List<LocalEvent> Search(string keyword, string category, DateTime? date)
        {
            // --- record search pattern ---
            _totalSearches++;
            if (!string.IsNullOrEmpty(category))
            {
                if (!_categorySearchCounts.ContainsKey(category))
                    _categorySearchCounts[category] = 0;
                _categorySearchCounts[category]++;
            }
            if (!string.IsNullOrWhiteSpace(keyword))
                _searchTerms.Push(keyword.Trim());

            // --- efficient date-first search using the sorted dictionary ---
            IEnumerable<LocalEvent> pool;
            if (date.HasValue)
            {
                pool = _eventsByDate.ContainsKey(date.Value.Date)
                    ? _eventsByDate[date.Value.Date]
                    : new List<LocalEvent>();
            }
            else
            {
                pool = _allEvents;
            }

            IEnumerable<LocalEvent> query = pool;
            if (!string.IsNullOrEmpty(category))
                query = query.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(e =>
                    e.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Description.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Venue.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);

            return query.OrderBy(e => e.Date).ToList();
        }

        // ----------------------------------------------------------------
        // Stacks
        // ----------------------------------------------------------------
        public static void PushViewed(LocalEvent ev) => _recentlyViewed.Push(ev);
        public static LocalEvent PopViewed() => _recentlyViewed.Count > 0 ? _recentlyViewed.Pop() : null;
        public static List<LocalEvent> GetViewingHistory() => _recentlyViewed.ToList();

        // ----------------------------------------------------------------
        // Queue: announcements
        // ----------------------------------------------------------------
        public static bool HasAnnouncements => _announcements.Count > 0;
        public static string DequeueAnnouncement()
        {
            if (_announcements.Count == 0) return string.Empty;
            string msg = _announcements.Dequeue();
            _announcements.Enqueue(msg);  // re-enqueue so announcements cycle continuously
            return msg;
        }

        // ----------------------------------------------------------------
        // Priority queue: highest-priority featured events first
        // ----------------------------------------------------------------
        public static List<LocalEvent> GetFeaturedEvents(int count)
        {
            List<LocalEvent> featured = new List<LocalEvent>();
            foreach (var kvp in _featuredByPriority)          // 1 (highest) first
            {
                foreach (LocalEvent ev in kvp.Value)
                {
                    if (ev.Date.Date >= DateTime.Today)
                        featured.Add(ev);
                    if (featured.Count >= count) return featured;
                }
            }
            return featured;
        }

        // ----------------------------------------------------------------
        // Recommendation feature
        // 1. Analyses _categorySearchCounts to find the user's favourite categories
        // 2. Suggests upcoming events in those categories the user has NOT opened yet
        // 3. Falls back to high-priority featured events before any searches exist
        // ----------------------------------------------------------------
        public static List<LocalEvent> GetRecommendations(int max = 4)
        {
            HashSet<string> viewed = new HashSet<string>(
                _recentlyViewed.Select(v => v.EventId));      // set for O(1) "already seen" checks

            List<LocalEvent> picks = new List<LocalEvent>();

            if (_categorySearchCounts.Count > 0 && _totalSearches >= 2)
            {
                // Rank categories by how often the user has searched them.
                List<string> favouriteCategories = _categorySearchCounts
                    .OrderByDescending(kvp => kvp.Value)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (string cat in favouriteCategories)
                {
                    foreach (LocalEvent ev in GetUpcomingEvents())
                    {
                        if (picks.Count >= max) break;
                        if (ev.Category == cat && !viewed.Contains(ev.EventId) && !picks.Contains(ev))
                            picks.Add(ev);
                    }
                }
            }

            // Fallback: high-priority featured events if nothing personalised yet
            if (picks.Count < 2)
            {
                foreach (LocalEvent ev in GetFeaturedEvents(10))
                {
                    if (picks.Count >= max) break;
                    if (!viewed.Contains(ev.EventId) && !picks.Contains(ev))
                        picks.Add(ev);
                }
            }
            return picks.OrderBy(e => e.Date).Take(max).ToList();
        }
    }
}
