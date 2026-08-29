================================================================================
MUNICIPAL SERVICES APPLICATION — SOUTH AFRICA
Version 1.1  |  C# .NET Framework 4.7.2  |  Windows Forms
================================================================================

PROJECT OVERVIEW
--------------------------------------------------------------------------------
This application was developed for a South African municipality to streamline
citizen engagement with municipal services.  Part 1 implements the "Report
Issues" module, enabling residents to report local problems, attach supporting
media, and receive immediate feedback through a polished, animated interface.

Architecture:  flat single-namespace Windows Forms application.
Data structure: List<Issue> (IssueDataStore) — all issues are stored in memory
for the duration of the session.

FILES INCLUDED
--------------------------------------------------------------------------------
  MunicipalServicesApp.csproj   — Visual Studio project file (.NET 4.7.2)
  Program.cs                    — Application entry point
  Issue.cs                      — Issue model + IssueDataStore (List<Issue>)
  AnimationHelper.cs            — Reusable animation utilities (fade, slide, bar)
  MainMenuForm.cs               — Main menu with 3 service options
  ReportIssuesForm.cs           — Report Issues form with all required fields
  README.txt                    — This file

SYSTEM REQUIREMENTS
--------------------------------------------------------------------------------
  Operating System  : Windows 10 or 11 (64-bit recommended)
  .NET Framework    : 4.7.2 or higher (pre-installed on Windows 10/11)
  IDE               : Visual Studio 2019 or 2022 (Community Edition is free)
  RAM               : 512 MB minimum
  Disk Space        : 50 MB

HOW TO COMPILE
--------------------------------------------------------------------------------
Method 1 — Visual Studio (recommended)
  1. Open Visual Studio.
  2. Click  File > Open > Project/Solution.
  3. Browse to the project folder and open  MunicipalServicesApp.csproj.
  4. Press  Ctrl + Shift + B  to build.
  5. The Output window should report: "Build succeeded."

Method 2 — Add files to an existing project
  1. In Visual Studio create a new Windows Forms App (.NET Framework) project.
     Name it exactly: MunicipalServicesApp
  2. Right-click the project in Solution Explorer → Add > Existing Item…
  3. Add:  Issue.cs, AnimationHelper.cs, MainMenuForm.cs, ReportIssuesForm.cs
  4. Replace the generated Program.cs with the provided Program.cs.
  5. Press  F6  (Build > Build Solution) to compile.

Method 3 — MSBuild (command line)
  Open a Developer Command Prompt for Visual Studio and run:

    cd path\to\MunicipalServicesApp
    msbuild MunicipalServicesApp.csproj /p:Configuration=Release

  The executable is created at:  bin\Release\MunicipalServicesApp.exe

HOW TO RUN
--------------------------------------------------------------------------------
  • From Visual Studio  :  press F5 (with debugger) or Ctrl+F5 (without).
  • From File Explorer  :  double-click bin\Debug\MunicipalServicesApp.exe
  • From command line   :  bin\Release\MunicipalServicesApp.exe

HOW TO USE THE SOFTWARE
--------------------------------------------------------------------------------
MAIN MENU
  When the application starts you will see three options:

    📋  Report Issues                     ← ACTIVE — click to report an issue
    📅  Local Events & Announcements      ← disabled (Part 2)
    🔧  Service Request Status            ← disabled (Part 2)

  The buttons slide in from the left when the form loads.
  The issue count at the bottom updates every time you return from reporting.

REPORT ISSUES FORM
  1. Location   : type the street address or area of the problem.
                  A grey placeholder hint is shown until you start typing.
  2. Category   : choose from the dropdown:
                  Sanitation / Roads and Potholes / Water and Utilities /
                  Electricity / Waste Management / Public Safety /
                  Parks and Recreation / Other
  3. Description: write a detailed description (minimum 10 characters).
                  A character counter updates as you type.
  4. Attach Media (optional): click "Browse for File…" to open a file picker.
                  Supported: .jpg .jpeg .png .bmp .gif .pdf .doc .docx
  5. Engagement strip: watch the progress bar fill as you complete fields.
                  Encouraging messages rotate every 4 seconds.
  6. Submit     : click "✔ Submit Report".
                  All fields are validated first. On success a MessageBox
                  confirms the submission with a unique Reference Code.
                  The form then resets for the next report.
  7. Back       : click "← Back to Main Menu" to return.

DATA STORAGE
--------------------------------------------------------------------------------
Issues are stored in IssueDataStore.Issues (a List<Issue>) for the duration
of the application session.  Data is NOT written to disk in Part 1.

ANIMATIONS (Engagement Strategy)
--------------------------------------------------------------------------------
  • Form fade-in      — both forms appear smoothly (opacity 0 → 1).
  • Button slide-in   — main-menu buttons glide in from the left with stagger.
  • Progress bar      — fills smoothly as the user completes form fields.
  • Message rotation  — encouraging messages fade in/out every 4 seconds.
  • Submit flash      — button flashes green on success, red on error.
  • Hover effects     — buttons change shade when the mouse hovers over them.

DESIGN CONSIDERATIONS
--------------------------------------------------------------------------------
  Consistency    — uniform colour scheme (blue header, AliceBlue background,
                   Arial font) across both forms.
  Clarity        — bold labels, plain language, inline character counter.
  User Feedback  — MessageBox for success / validation errors; progress bar.
  Responsiveness — Anchor properties on wide controls; minimum form size set.

BUG FIXES (vs. original student code)
--------------------------------------------------------------------------------
  ❌  Original: FormClosing called Application.Exit() — closed entire app
               when user pressed the window X button on ReportIssuesForm.
  ✅  Fixed   : FormClosing just cleans up timers and lets the form close
               normally; MainMenuForm re-shows itself via its FormClosed handler.

  ❌  Original: BtnBack_Click created a brand-new MainMenuForm() — memory leak
               and loss of session state (issue count reset to 0).
  ✅  Fixed   : BtnBack_Click simply calls this.Close(); the existing
               MainMenuForm instance handles its own FormClosed subscription.

TROUBLESHOOTING
--------------------------------------------------------------------------------
  Problem                         Solution
  ─────────────────────────────── ──────────────────────────────────────────────
  Build errors / missing types    Ensure all 5 .cs files are in the project and
                                  the namespace is MunicipalServicesApp.
  OpenFileDialog does not appear  Run the .exe as a user (not as System).
  Animations look choppy          Close other heavy applications; animations run
                                  at 16 ms intervals (~60 fps) via WinForms Timer.
  Form is very small at startup   Ensure Windows display scaling is ≤ 150 %.
  Progress bar does not move      Fill in Location and Description fields — these
                                  are the two required fields for progress.

CONTACT
--------------------------------------------------------------------------------
  For technical support, contact the ICT Development Team.
================================================================================
