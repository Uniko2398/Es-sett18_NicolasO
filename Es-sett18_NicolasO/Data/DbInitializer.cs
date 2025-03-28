using Es_sett18_NicolasO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Es_sett18_NicolasO.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            // Creazione del database se non esiste
            await context.Database.EnsureCreatedAsync();

            // Creazione dei ruoli
            if (!await roleManager.RoleExistsAsync("Amministratore"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Amministratore" });
            }

            if (!await roleManager.RoleExistsAsync("Utente"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Utente" });
            }

            // Creazione dell'utente amministratore
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Amministratore");
                }
            }

            // Aggiunta di dati di esempio
            if (!context.Artisti.Any())
            {
                var artisti = new List<Artista>
            {
                new Artista { Nome = "Coldplay", Genere = "Rock", Biografia = "Band britannica formata nel 1996" },
                new Artista { Nome = "Ed Sheeran", Genere = "Pop", Biografia = "Cantautore britannico" },
                new Artista { Nome = "Beyoncé", Genere = "R&B", Biografia = "Cantante americana" }
            };

                await context.Artisti.AddRangeAsync(artisti);
                await context.SaveChangesAsync();
            }

            if (!context.Eventi.Any())
            {
                var artisti = await context.Artisti.ToListAsync();

                var eventi = new List<Evento>
            {
                new Evento {
                    Titolo = "Coldplay World Tour",
                    Data = DateTime.Now.AddMonths(1),
                    Luogo = "Stadio Olimpico, Roma",
                    ArtistaId = artisti[0].ArtistaId
                },
                new Evento {
                    Titolo = "Ed Sheeran Live",
                    Data = DateTime.Now.AddMonths(2),
                    Luogo = "Arena di Verona",
                    ArtistaId = artisti[1].ArtistaId
                },
                new Evento {
                    Titolo = "Beyoncé Renaissance Tour",
                    Data = DateTime.Now.AddMonths(3),
                    Luogo = "San Siro, Milano",
                    ArtistaId = artisti[2].ArtistaId
                }
            };

                await context.Eventi.AddRangeAsync(eventi);
                await context.SaveChangesAsync();
            }
        }
    }
}
