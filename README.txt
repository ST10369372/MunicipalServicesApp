# MUNICIPAL SERVICES APPLICATION — SOUTH AFRICA
Version 1.1
C# Windows Forms | .NET Framework 4.7.2

======================================================================
1. PROJECT OVERVIEW
======================================================================

The Municipal Services Application is a desktop application developed
to improve communication between South African residents and their
local municipality.

The application provides a central platform where residents can report
municipal problems, discover local events and announcements, and access
the Service Request Status section.

The system uses a graphical user interface developed with C# and
Windows Forms. It includes input validation, user feedback, navigation,
event searching, data structures, and a recommendation feature.

The application is designed to be simple to use, organised, and
accessible to residents who need to interact with municipal services.

PROJECT NAME:
MunicipalServicesApp

PROGRAMMING LANGUAGE:
C#

FRAMEWORK:
.NET Framework 4.7.2

APPLICATION TYPE:
Windows Forms Desktop Application

TARGET OPERATING SYSTEM:
Windows 10 and Windows 11

DATA STORAGE:
In-memory collections for the current application session.

======================================================================
2. PROJECT OBJECTIVES
======================================================================

The objectives of the application are to:

1. Allow residents to report municipal problems.
2. Validate submitted information and provide feedback.
3. Allow residents to browse and search local events.
4. Display municipal announcements in a rotating queue.
5. Maintain a history of recently viewed events.
6. Record search terms and analyse category preferences.
7. Recommend upcoming events based on user search patterns.
8. Demonstrate the use of stacks, queues, dictionaries, sorted
   dictionaries, and hash sets.
9. Provide a main menu with three organised service options.
10. Provide a foundation for future municipal service request tracking.

======================================================================
3. SYSTEM REQUIREMENTS
======================================================================

Hardware:
- Computer or laptop capable of running Windows.
- At least 512 MB of available RAM is recommended for the application.
- Approximately 50 MB of available disk space.

Software:
- Windows 10 or Windows 11.
- Visual Studio 2019 or Visual Studio 2022.
- .NET Framework 4.7.2 Developer Pack or compatible targeting pack.
- Windows Forms development workload in Visual Studio.

The .NET Framework runtime must be installed to execute the application.
The Developer Pack or targeting pack is required to build against
.NET Framework 4.7.2.

======================================================================
4. PROJECT STRUCTURE
======================================================================

The project may contain the following files:

MunicipalServicesApp/
|
|-- MunicipalServicesApp.csproj
|-- Program.cs
|-- MainMenuForm.cs
|-- MainMenuForm.Designer.cs
|-- ReportIssuesForm.cs
|-- ReportIssuesForm.Designer.cs
|-- Issue.cs
|-- AnimationHelper.cs
|-- EventManager.cs
|-- LocalEventsForm.cs
|-- LocalEventsForm.Designer.cs
|-- ServiceRequestStatusForm.cs
|-- ServiceRequestStatusForm.Designer.cs
|-- README.txt
|
|-- Properties/
|   |-- AssemblyInfo.cs
|   |-- Resources.resx
|   |-- Settings.settings
|
|-- bin/
|   |-- Debug/
|   |-- Release/
|
|-- obj/

Note:
The exact file structure may differ depending on the Visual Studio
project template. Files ending in .Designer.cs contain automatically
generated Windows Forms interface code.

The EventManager.cs, LocalEventsForm.cs, LocalEventsForm.Designer.cs,
ServiceRequestStatusForm.cs, and ServiceRequestStatusForm.Designer.cs
files are required for the described Part 2 implementation.

======================================================================
5. APPLICATION MODULES
======================================================================

5.1 MAIN MENU

File:
- MainMenuForm.cs
- MainMenuForm.Designer.cs

The main menu is the application's central navigation screen.

It provides three service options:

1. Report Issues
2. Local Events and Announcements
3. Service Request Status

The menu should provide clear labels, consistent formatting, and
working navigation to the appropriate forms.

The interface may use button hover effects, animations, a consistent
colour scheme, and an issue counter.

Expected behaviour:
- Selecting Report Issues opens the reporting form.
- Selecting Local Events and Announcements opens the events form.
- Selecting Service Request Status opens the status form.
- Closing a child form returns the user to the existing main menu.
- Returning to the menu should preserve the current session's issue
  data rather than creating unnecessary duplicate main menu forms.

5.2 REPORT ISSUES

Files:
- ReportIssuesForm.cs
- ReportIssuesForm.Designer.cs
- Issue.cs
- AnimationHelper.cs

The Report Issues module enables residents to submit municipal
problems for attention.

The form supports the following fields:

Location:
The street address, area, or location where the problem occurred.

Category:
The type of municipal problem being reported.

Description:
A detailed explanation of the issue.

Supporting Media:
An optional attachment containing supporting evidence.

Example categories:
- Sanitation
- Roads and Potholes
- Water and Utilities
- Electricity
- Waste Management
- Public Safety
- Parks and Recreation
- Other

Validation:
- Required fields must be completed.
- The description must meet the application's minimum length.
- Invalid submissions must display a helpful message.
- Successful submissions must display a unique reference code.

The interface may include a character counter, progress indicator,
animated feedback, and button hover effects.

Data handling:
Issues are stored in the IssueDataStore.Issues collection for the
duration of the current session.

Unless additional persistence is implemented, submitted issues are
not permanently saved to a database or file.

5.3 LOCAL EVENTS AND ANNOUNCEMENTS

Files:
- LocalEventsForm.cs
- LocalEventsForm.Designer.cs
- EventManager.cs

The Local Events module allows residents to discover community events
and view municipal announcements.

Expected features:

Event Listing:
Displays available local events with relevant information, such as
the event title, category, date, location, and description.

Search:
Allows residents to search for events using a keyword, category,
and date.

Announcements:
Displays municipal announcements and rotates through them using
a first-in, first-out queue.

Recently Viewed Events:
Records events opened by the resident using a stack.

Search History:
Records search keywords using a stack so that the latest search
can be accessed first.

Category Preferences:
Counts category searches to identify categories that may interest
the resident.

Recommended for You:
Displays suitable upcoming events based on category preferences.
Previously viewed events should be excluded from recommendations.
If no suitable category-based recommendations are available,
the system can fall back to high-priority featured events.

The module demonstrates practical applications of the required
data structures.

5.4 SERVICE REQUEST STATUS

Files:
- ServiceRequestStatusForm.cs
- ServiceRequestStatusForm.Designer.cs

The Service Request Status module provides the entry point for
checking municipal service requests.

The initial version may contain a placeholder screen explaining
that status tracking will be implemented in a later development phase.

A complete tracking feature would require a request identifier and
a data source containing request status information.

Unless such functionality has been implemented, the placeholder
must not be represented as a working connection to a municipal
database or live tracking service.

======================================================================
6. DATA STRUCTURES AND IMPLEMENTATION
======================================================================

Part 2 demonstrates several data structures from the
System.Collections.Generic namespace.

6.1 STACKS

A stack follows the Last In, First Out (LIFO) principle.

Stack<LocalEvent> _recentlyViewed

Purpose:
Stores recently viewed events.

Expected operation:
When a resident opens an event, that event is pushed onto the stack.

Stack<string> _searchTerms

Purpose:
Stores search keywords in the order they are entered.

Expected operation:
Each search keyword is pushed onto the stack, allowing the most
recent keyword to be accessed first.

6.2 QUEUES

A queue follows the First In, First Out (FIFO) principle.

Queue<string> _announcements

Purpose:
Stores municipal announcements in rotation order.

Expected operation:
The first announcement is removed using Dequeue(), displayed to the
resident, and added back to the end of the queue when rotation is
required.

This allows announcements to cycle through the available messages.

6.3 PRIORITY QUEUE

SortedDictionary<int, Queue<LocalEvent>> _featuredByPriority

Purpose:
Stores featured events according to priority.

Priority convention:
- Priority 1 represents the highest priority.
- Larger numbers represent lower priorities.

The SortedDictionary keeps priority keys sorted. Each key contains a
queue of events assigned to that priority.

When selecting featured events, the implementation must process the
lowest numerical priority first.

This structure is compatible with .NET Framework 4.7.2 and avoids
relying on the PriorityQueue<TElement, TPriority> API introduced in
later .NET versions.

6.4 DICTIONARIES

Dictionary<string, LocalEvent> _eventById

Purpose:
Provides fast event lookup using a unique event identifier.

Dictionary<string, int> _categorySearchCounts

Purpose:
Counts searches associated with event categories and supports the
recommendation feature.

Dictionaries provide average O(1) lookup by key under normal
hashing conditions.

6.5 SORTED DICTIONARY

SortedDictionary<DateTime, List<LocalEvent>> _eventsByDate

Purpose:
Groups events by date and keeps date keys in sorted order.

Expected operation:
Events can be retrieved in chronological order by iterating through
the sorted keys.

For date filtering, the implementation should use the relevant date
entries rather than scanning unrelated events wherever practical.

6.6 HASH SETS

HashSet<string> _categories

Purpose:
Stores unique event categories and supplies category options for
the category selection control.

HashSet<DateTime> _eventDates

Purpose:
Stores unique event dates and supports efficient date-existence checks.

HashSet<string> viewed

Purpose:
Tracks event identifiers that have already been viewed so that
recommendations can exclude those events.

HashSet collections prevent duplicate values and generally provide
average O(1) membership checks.

======================================================================
7. SEARCH FUNCTIONALITY
======================================================================

The EventManager.Search(keyword, category, date) method is responsible
for filtering the available events.

Supported criteria:

Keyword:
Matches relevant event information according to the implemented
search rules.

Category:
Restricts the results to the selected category.

Date:
Restricts the results to the selected date.

Combined filtering:
When multiple criteria are provided, the returned events must satisfy
all active filters.

The date index should be used to retrieve events associated with the
selected date efficiently.

Search behaviour should also update the relevant search history and
category preference information.

The implementation should handle empty search fields appropriately
and should not crash when no events match the selected criteria.

======================================================================
8. PERSONALIZED RECOMMENDATIONS
======================================================================

The recommendation feature suggests upcoming local events based
on the resident's search activity.

The recommendation process follows these steps:

1. Record each search keyword in the search-history stack.
2. Update the search count for the relevant category.
3. Identify categories with the highest search counts.
4. Find upcoming events belonging to preferred categories.
5. Exclude events already viewed by the resident using a HashSet.
6. Rank or order the suitable results according to the implemented
   recommendation rules.
7. If no suitable category-based events are found, use eligible
   high-priority featured events as a fallback.
8. Display the results in the Recommended for You section.

Recommendation results should be updated when search preferences
or viewing history changes.

The feature uses in-memory session data. Unless user profiles and
persistent storage are added, preferences are not retained after
the application closes.

======================================================================
9. BUILDING THE APPLICATION
======================================================================

METHOD 1: VISUAL STUDIO

1. Open Visual Studio 2019 or Visual Studio 2022.
2. Select File > Open > Project/Solution.
3. Locate the MunicipalServicesApp.csproj file.
4. Open the project.
5. Confirm that the project targets .NET Framework 4.7.2.
6. Confirm that all required .cs files are included in the project.
7. Check that the source files use the correct namespace:
   MunicipalServicesApp.
8. Select Build > Build Solution.
9. Alternatively, press Ctrl+Shift+B.
10. Check the Error List and Output window.
11. Resolve any compilation errors before running the application.

METHOD 2: ADD FILES TO AN EXISTING PROJECT

1. Open the existing MunicipalServicesApp project.
2. In Solution Explorer, right-click the project.
3. Select Add > Existing Item.
4. Add the required source files that are not already included.
5. For each new Windows Form, ensure its .cs and .Designer.cs files
   are correctly associated with the project.
6. Verify that Program.cs starts the intended main menu.
7. Build the solution.

Do not add duplicate copies of existing classes or forms. Duplicate
class definitions may produce compilation errors.

METHOD 3: COMMAND LINE

Open Developer Command Prompt for Visual Studio.

Navigate to the directory containing the project file:

cd path\to\MunicipalServicesApp

Build the project in Release configuration:

msbuild MunicipalServicesApp.csproj /p:Configuration=Release

The executable is normally generated under:

bin\Release\MunicipalServicesApp.exe

The exact output location depends on the project configuration.

======================================================================
10. RUNNING THE APPLICATION
======================================================================

Using Visual Studio:
- Press F5 to run with debugging.
- Press Ctrl+F5 to run without debugging.

Using File Explorer:
- Navigate to the project's bin\Debug or bin\Release directory.
- Double-click MunicipalServicesApp.exe.

Using the command line:
- Navigate to the relevant output directory.
- Run MunicipalServicesApp.exe.

The application should open the main menu.

======================================================================
11. HOW TO USE THE APPLICATION
======================================================================

11.1 OPENING THE MAIN MENU

Start the application.

Select one of the three available service options.

11.2 REPORTING A MUNICIPAL ISSUE

1. Select Report Issues.
2. Enter the location of the problem.
3. Select the appropriate category.
4. Enter a detailed description.
5. Attach supporting media if necessary.
6. Complete all required fields.
7. Select Submit Report.
8. Review the validation message or submission confirmation.
9. Record the generated reference code.
10. Return to the main menu.

11.3 SEARCHING FOR LOCAL EVENTS

1. Select Local Events and Announcements.
2. Enter a search keyword, if required.
3. Select a category, if required.
4. Choose a date, if required.
5. Start the search.
6. Review the matching events.
7. Select an event to view its details.
8. Check the Recommended for You section.
9. Review the current announcement.
10. Return to the main menu when finished.

11.4 VIEWING SERVICE REQUEST STATUS

1. Select Service Request Status.
2. Review the available screen and instructions.
3. If the module is still a placeholder, return to the main menu.

Actual request tracking requires a working implementation and a
reliable data source.

======================================================================
12. TESTING AND QUALITY ASSURANCE
======================================================================

The following checks should be performed before submission.

MAIN MENU:
- Confirm that all three options are visible.
- Confirm that each button opens the correct form.
- Confirm that closing a child form returns to the main menu.
- Confirm that the application does not create unnecessary duplicate
  main menu instances.

REPORT ISSUES:
- Attempt to submit an empty form.
- Test each required field.
- Test descriptions below and above the minimum length.
- Test valid and unsupported file extensions.
- Confirm that successful submissions generate reference codes.
- Confirm that submitted issues appear in the session collection.

LOCAL EVENTS:
- Confirm that the event list displays available events.
- Search using a valid keyword.
- Search using a category.
- Search using a date.
- Combine keyword, category, and date filters.
- Test a search that returns no results.
- Confirm that search terms are recorded.
- Confirm that category search counts are updated.
- Confirm that viewing an event updates viewing history.
- Confirm that announcements rotate correctly.
- Confirm that viewed events are excluded from recommendations.
- Confirm that fallback recommendations work when applicable.

DATA STRUCTURES:
- Verify that event identifiers are unique.
- Verify that categories are not duplicated in the category set.
- Verify that dates are stored and processed correctly.
- Verify that priority 1 events are considered before lower-priority
  events.
- Verify that chronological ordering is maintained.

SERVICE REQUEST STATUS:
- Confirm that the menu button opens the correct form.
- Confirm that the placeholder or implemented functionality behaves
  as described.

GENERAL:
- Build the application without compilation errors.
- Test the application at normal and different display scaling levels.
- Confirm that navigation does not unexpectedly terminate the
  application.
- Confirm that validation errors are understandable.

======================================================================
13. ERROR HANDLING AND TROUBLESHOOTING
======================================================================

PROBLEM: The project does not compile.

Possible causes:
- Missing source files.
- Incorrect namespace.
- Duplicate class definitions.
- Missing event handlers.
- Incorrect control names.
- Missing project references.
- Unsupported framework APIs.

Solution:
Check the first error in Visual Studio's Error List and correct it
before addressing subsequent errors.

PROBLEM: A form cannot be found.

Possible cause:
The form class has not been added to the project or its namespace
does not match the calling code.

Solution:
Confirm that the form's .cs file is included and that the namespace
is MunicipalServicesApp.

PROBLEM: A button does not respond.

Possible causes:
- The Click event is not connected.
- The event handler name is incorrect.
- The button is disabled.
- The handler contains an exception.

Solution:
Check the control's Click event in the Properties window and verify
the corresponding event-handler method.

PROBLEM: No events appear in search results.

Possible causes:
- The event collection is empty.
- The search criteria do not match any event.
- The date or category filter is incorrect.

Solution:
Confirm that sample events have been loaded and test the search
with fewer filtering criteria.

PROBLEM: Recommendations are empty.

Possible causes:
- No upcoming events are available.
- No category preferences have been recorded.
- All eligible events have already been viewed.
- Featured events have not been loaded.

Solution:
Add suitable upcoming sample events, perform category searches,
and verify the featured-event fallback.

PROBLEM: Announcements do not rotate.

Possible causes:
- The queue is empty.
- The rotation method is not being called.
- The queue item is removed but not re-enqueued.
- The timer or event handler is not configured correctly.

Solution:
Check the queue initialization, DequeueAnnouncement() logic, and
the event or timer that triggers announcement rotation.

PROBLEM: The application closes unexpectedly.

Possible cause:
A form-closing event calls Application.Exit() or another form's
closing logic terminates the application.

Solution:
Review form-closing handlers and ensure that closing a child form
does not unintentionally shut down the entire application.

PROBLEM: Data disappears after restarting.

Cause:
The current implementation uses in-memory storage.

Solution:
Implement persistent storage, such as a database or file-based
repository, if information must survive application restarts.

======================================================================
14. LIMITATIONS
======================================================================

The application has the following limitations unless additional
functionality is implemented:

1. In-memory data is lost when the application terminates.
2. Reported issues are not automatically sent to a municipality.
3. Event information may be sample data rather than live municipal data.
4. Recommendations are based on the current session's search patterns.
5. User accounts and cross-session preferences are not provided by
   the described in-memory implementation.
6. Service request tracking requires a data source and additional logic.
7. File attachments may be selected without necessarily being uploaded
   to a server or permanently stored.
8. No live municipal API integration is assumed.

These limitations should be considered when extending the system.

======================================================================
15. FUTURE ENHANCEMENTS
======================================================================

Possible improvements include:

- Database integration for permanent data storage.
- Resident registration and secure authentication.
- Administrator and municipal employee dashboards.
- Live municipal event and announcement feeds.
- Persistent issue reference numbers and status tracking.
- Email or SMS notifications.
- Maps for reporting issue locations.
- Image and document upload storage.
- Advanced recommendations using historical user preferences.
- Accessibility improvements and multilingual support.
- Audit logs and reporting dashboards.
- Automated unit tests and integration tests.

======================================================================
16. TECHNOLOGIES USED
======================================================================

Programming Language:
C#

User Interface:
Windows Forms

Framework:
.NET Framework 4.7.2

Collections and Data Structures:
- List<T>
- Stack<T>
- Queue<T>
- Dictionary<TKey, TValue>
- SortedDictionary<TKey, TValue>
- HashSet<T>

Development Environment:
Visual Studio 2019 or Visual Studio 2022

Operating System:
Windows 10 or Windows 11

======================================================================
17. CONCLUSION
======================================================================

The Municipal Services Application provides a desktop interface for
reporting municipal problems and accessing local community information.

Part 1 introduces the Report Issues module with form validation,
supporting media selection, reference codes, and user feedback.

Part 2 extends the application with local events, rotating
announcements, event search, viewing history, priority-based event
selection, and personalised recommendations.

The project demonstrates how C# Windows Forms and generic collection
classes can be applied to a practical municipal services scenario.

The application can be extended with persistent storage, authentication,
live municipal integrations, and a fully implemented service request
tracking module.

======================================================================
END OF README
======================================================================
