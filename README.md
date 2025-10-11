# Devnoter.dk

Devnoter.dk er en noteplatform udviklet til udviklere, der ønsker at holde styr på deres kode-snippets, tekniske noter og små projekter.
Formålet med platformen er at skabe et enkelt, hurtigt og overskueligt værktøj, så du kan fokusere på selve kodningen – uden at miste vigtige idéer eller løsninger undervejs.

For at køre projektet lokalt skal du oprette din egen database og indsætte forbindelsesstrengen i:
"DefaultConnection": ""
Dette gøres i filen appsettings.Development.json.

Kør derefter projektet i Development-miljøet i Visual Studio.
Bemærk: Databasen, der bruges til udvikling, er ikke inkluderet i projektet, så du skal bruge din egen.
Du kan eventuelt benytte Entity Framework til at oprette og opdatere databasen.


### Entity Framework kommandoer
Når databasen er sat op, kan du bruge følgende kommandoer i Package Manager Console:
Add-Migration <NavnPåMigration>
Update-Database



## Nødvendige pakker / Extensions
Følgende NuGet-pakker er installeret i projektet (du behøver muligvis ikke dem alle):
* Microsoft.AspNetCore.Identity.EntityFrameworkCore
* Microsoft.AspNetCore.Identity.UI
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Sqlite
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools
* Microsoft.VisualStudio.Web.CodeGeneration.Design

### Installation

1. Clone repo: `git clone <repo-link>`
2. Åbn i Visual Studio
3. Opret lokal database
4. Indsæt forbindelsesstreng i `appsettings.Development.json`
5. Kør EF-migrationer:
6. Kør projektet i Development-miljø

## Opdateringer
Projektet vedligeholdes løbende, og hjemmesiden forventes opdateret cirka én gang om ugen med forbedringer og nye funktioner.

## Bidrag
Har du forslag eller finder fejl?
* Åbn et issue
* Eller send en pull request
Alle bidrag er velkomne!

### .NET version:
Jeg bruger version 8.0 til denne aplikation

#### License
MIT © Jens Hincke Friis
