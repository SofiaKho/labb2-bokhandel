using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BokhandelApp.Models; // ← ändra om din context ligger i annat namespace

namespace BokhandelApp
{
    class Program
    {
        static void Main()
        {
            using var context = new BokhandelContext();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("📚 BokhandelApp");
                Console.WriteLine("1. Visa lagersaldo");
                Console.WriteLine("2. Lägg till bok i lager");
                Console.WriteLine("3. Ta bort bok från lager");
                Console.WriteLine("4. Avsluta");
                Console.Write("Välj: ");
                string val = Console.ReadLine() ?? "";

                switch (val)
                {
                    case "1":
                        VisaLagersaldo(context);
                        break;
                    case "2":
                        LäggTillBok(context);
                        break;
                    case "3":
                        TaBortBok(context);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val");
                        break;
                }

                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }

        static void VisaLagersaldo(BokhandelContext context)
        {
            var saldo = context.LagerSaldos
                .Include(l => l.Butik)
                .Include(l => l.IsbnNavigation)
                .ToList()
                .Select(ls => new
                {
                    Butik = ls.Butik.Butiksnamn,
                    Bok = ls.IsbnNavigation.Titel,
                    Antal = ls.Antal
                });

            Console.WriteLine("\n📦 Lagersaldo:\n");

            foreach (var post in saldo)
            {
                Console.WriteLine($"{post.Butik,-25} | {post.Bok,-30} | Antal: {post.Antal}");
            }
        }

        static void LäggTillBok(BokhandelContext context)
        {
            Console.WriteLine("\n📍 Tillgängliga butiker:");
            var butiker = context.Butikers.ToList();
            foreach (var butik in butiker)
            {
                Console.WriteLine($"{butik.Id}. {butik.Butiksnamn}");
            }

            Console.Write("Välj Butik ID: ");
            int butikId = int.Parse(Console.ReadLine() ?? "0");

            Console.WriteLine("\n📚 Tillgängliga böcker:");
            var bocker = context.Böckers.ToList();
            foreach (var bok in bocker)
            {
                Console.WriteLine($"{bok.Isbn} – {bok.Titel}");
            }

            Console.Write("Ange ISBN för boken du vill lägga till: ");
            string isbn = Console.ReadLine() ?? "";

            Console.Write("Hur många exemplar vill du lägga till? ");
            int antal = int.Parse(Console.ReadLine() ?? "0");

            var lager = context.LagerSaldos
                .FirstOrDefault(l => l.ButikId == butikId && l.Isbn == isbn);

            if (lager != null)
            {
                lager.Antal += antal;
                Console.WriteLine("✅ Uppdaterade befintligt lagersaldo.");
            }
            else
            {
                context.LagerSaldos.Add(new LagerSaldo
                {
                    ButikId = butikId,
                    Isbn = isbn,
                    Antal = antal
                });
                Console.WriteLine("✅ Lade till ny bok i lagret.");
            }

            context.SaveChanges();
        }

        static void TaBortBok(BokhandelContext context)
        {
            Console.WriteLine("\n📍 Tillgängliga butiker:");
            var butiker = context.Butikers.ToList();
            foreach (var butik in butiker)
            {
                Console.WriteLine($"{butik.Id}. {butik.Butiksnamn}");
            }

            Console.Write("Välj Butik ID: ");
            int butikId = int.Parse(Console.ReadLine() ?? "0");

            var lagerLista = context.LagerSaldos
                .Where(l => l.ButikId == butikId)
                .Include(l => l.IsbnNavigation)
                .ToList();

            if (!lagerLista.Any())
            {
                Console.WriteLine("❌ Denna butik har inga böcker i lager.");
                return;
            }

            Console.WriteLine("\n📚 Böcker i butikens lager:");
            foreach (var l in lagerLista)
            {
                Console.WriteLine($"{l.Isbn} – {l.IsbnNavigation?.Titel} – Antal: {l.Antal}");
            }

            Console.Write("Ange ISBN för boken du vill ta bort: ");
            string isbn = Console.ReadLine() ?? "";

            var lager = context.LagerSaldos
                .FirstOrDefault(l => l.ButikId == butikId && l.Isbn == isbn);

            if (lager != null)
            {
                context.LagerSaldos.Remove(lager);
                context.SaveChanges();
                Console.WriteLine("✅ Boken togs bort från butikens lager.");
            }
            else
            {
                Console.WriteLine("❌ Boken hittades inte i denna butik.");
            }
        }
    }
}


