PART 2 - WHERE EACH RUBRIC REQUIREMENT IS MET
=============================================

Main Menu (30 marks)
--------------------
  MainMenuForm.cs        -> three organised menu options, all working, error-free navigation.

Stacks, Queues, Priority Queues (15 marks)
------------------------------------------
  EventManager.cs
    - Stack<LocalEvent>  _recentlyViewed   : pushed every time a citizen opens an event
    - Stack<string>      _searchTerms      : history of search keywords (undo-style)
    - Queue<string>      _announcements    : FIFO rotation of council announcements
    - SortedDictionary<int, Queue<LocalEvent>> _featuredByPriority : priority queue
      (priority 1 = highest; works on .NET Framework, unlike System.Collections.Generic.PriorityQueue)
  LocalEventsForm.cs
    - announcements auto-cycle using DequeueAnnouncement()
    - clicking an event pushes it onto the viewing-history stack

Hash Tables, Dictionaries, Sorted Dictionaries (15 marks)
---------------------------------------------------------
  Dictionary<string, LocalEvent>            _eventById      : O(1) event lookup by ID
  Dictionary<string, int>                   _categorySearchCounts : search preference profile
  SortedDictionary<DateTime, List<LocalEvent>> _eventsByDate : events always kept in date order

Sets (10 marks)
---------------
  HashSet<string>    _categories : unique categories (feeds the category combo box)
  HashSet<DateTime>  _eventDates : unique event dates (fast date-existence checks)
  HashSet<string>    viewed      : used by the recommender to exclude already-seen events

Search (Task 2.1.b)
-------------------
  EventManager.Search(keyword, category, date) filters by all three criteria; date filtering
  goes straight through the SortedDictionary for efficiency.

Additional Recommendation Feature (30 marks)
--------------------------------------------
  1. Analyses search patterns: every search increments _categorySearchCounts and pushes
     the keyword onto _searchTerms.
  2. GetRecommendations() ranks the user's favourite categories and suggests upcoming
     events in those categories that they have not opened yet (checked with a HashSet),
     falling back to high-priority featured events.
  3. Recommendations are displayed in a dedicated "Recommended for you" list on the form.

NEW FILES TO ADD
----------------
  MainMenuForm.cs          (code-behind for your existing MainMenuForm.Designer.cs)
  LocalEventsForm.cs       (new)
  LocalEventsForm.Designer.cs (new)
  EventManager.cs          (new)
  ServiceRequestStatusForm.cs / .Designer.cs (placeholder for Task 3)

Build target: classic .NET Framework WinForms is fully supported (no .NET 6+ APIs used).
