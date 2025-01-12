**Prerequisite**
1. .NET 8 SDK
2. npm version 18.16.0 or newer (optional for front end)

**HOW TO RUN**
1. Open **PlayStudiosCodingChallenge.sln** using Visual Studio
2. Right-click on solution in Solution Explorer -> **Restore NuGet Packages**
3. Rebuild solution
4. Do either step 5 or 6 based on your preference
5. (OPTIONAL) To run both front end and backend projects: Right-click on solution in Solution Explorer -> **Properties** -> Make sure **Multiple startup projects** is selected -> Choose **Start** for **PlayStudiosCodingChallenge.Server** & **PlayStudiosCodingChallenge.client**
6. (OPTIONAL if you already did step 5) Right-click on **PlayStudiosCodingChallenge.Server** in Solution Explorer -> Click **Set as startup project**
7. Click **Start**

**Quest Configuration JSON Documentation**
- **RateFromBet**: The rate from bet
- **LevelBonusRate**: The rate for player level bonus
- **TotalQuestPointsForCompletion**: The total quest points required for completion or 100%
- **QuestMilestones**: Amount of milestones in a quest,
- **ChipsAwardedForMilestoneCompletion**: Chips reward for each milestone completion

**Sequence Diagram & Database Schema**
Sees **Diagrams** folder in **PlayStudiosCodingChallenge.Server**
