#Migrations
First set src/Infrastructure as default project nin package manager console

Add-Migration InitialApp -OutputDir Data/Migrations -Context Infrastructure.Data.AppDbContext -StartupProject Web

Update-Database -Context Infrastructure.Data.AppDbContext


Add-Migration InitialIdentity -OutputDir Identity/Migrations -Context Infrastructure.Identity.AppIdentityDbContext -StartupProject Web
Update-Database -Context Infrastructure.Identity.AppIdentityDbContext

Add-Migration OrderBasketAded -OutputDir Data/Migrations -Context Infrastructure.Data.AppDbContext -StartupProject Web
update-database -context AppDbContext

remove-migration -context AppDbContext

#
