using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SzallodaProjekt
{
    public class SzallodaElemzo
    {
        private List<Vendeg> vendegek;
        private List<Szoba> szobak;
        private List<Foglalas> foglalasok;

        public List<Vendeg> Vendegek
        {
            get => this.vendegek;
            set => this.vendegek = value;
        }

        public List<Szoba> Szobak
        {
            get => this.szobak;
            set => this.szobak = value;
        }

        public List<Foglalas> Foglalasok
        {
            get => this.foglalasok;
            set => this.foglalasok = value;
        }

        public SzallodaElemzo(string vendegekUtvonal, string szobakUtvonal, string foglalasokUtvonal)
        {
            this.vendegek = File.ReadAllLines(vendegekUtvonal)
                .Skip(1)
                .Select(sor => sor.Split(';'))
                .Select(adatok => new Vendeg(int.Parse(adatok[0]), adatok[1], adatok[2]))
                .ToList();

            this.szobak = File.ReadAllLines(szobakUtvonal)
                .Skip(1)
                .Select(sor => sor.Split(';'))
                .Select(adatok => new Szoba(int.Parse(adatok[0]), int.Parse(adatok[1]), adatok[2], int.Parse(adatok[3])))
                .ToList();

            this.foglalasok = File.ReadAllLines(foglalasokUtvonal)
                .Skip(1)
                .Select(sor => sor.Split(';'))
                .Select(adatok => new Foglalas(int.Parse(adatok[0]), int.Parse(adatok[1]), int.Parse(adatok[2]), int.Parse(adatok[3]), DateTime.Parse(adatok[4])))
                .ToList();
        }

        // --- Alapfeladatok (1 - 5) ---

        // 1. Add vissza egy listában azoknak a vendégeknek a nevét, akik Budapestről származnak.
        public List<string> BudapestiVendegekNeve()
        {
            List<string> budapestiVendegekNeve = vendegek.Where(v => v.Varos == "Budapest").Select(v => v.Nev).ToList();
            return budapestiVendegekNeve;
        }

        // 2. Add vissza a "Deluxe" kategóriájú szobákat árak szerint növekvő sorrendben.
        public List<Szoba> DeluxeSzobakArSzerint()
        {
            List<Szoba> deluxeSzobakArSzerint = szobak.Where(s => s.Kategoria == "Deluxe").OrderBy(s => s.ArPerEjszaka).ToList();
            return deluxeSzobakArSzerint;
        }

        // 3. Készíts listát, amely formázott szövegként tartalmazza a foglalásokat az alábbi formában: "[VendegNeve] - [EjszakakSzama] éjszaka".
        public List<string> FoglalasokSzoveggel()
        {
            List<string> foglalasokSzoveggel = foglalasok.Join(vendegek, f => f.VendegId, v => v.VendegId, (f, v) => new { v.Nev, f.EjszakakSzama }).Select(x => $"{x.Nev} - {x.EjszakakSzama} éjszaka").ToList();
            return foglalasokSzoveggel;
        }

        // 4. Írj lekérdezést, amely összeköti a foglalásokat a vendégekkel és a szobákkal (dupla JOIN), és egy olyan listát ad vissza, amelyben szerepel a vendég neve, a szoba kategóriája és az éjszakák száma.
        public List<string> ReszletesFoglalasok()
        {
            List<string> reszletesFoglalasok = foglalasok.Join(vendegek, f => f.VendegId, v => v.VendegId, (f, v) => new { f, v })
                .Join(szobak, fv => fv.f.SzobaId, s => s.SzobaId, (fv, s) => new { fv.v.Nev, s.Kategoria, fv.f.EjszakakSzama })
                .Select(x => $"{x.Nev} - {x.Kategoria} - {x.EjszakakSzama} éjszaka")
                .ToList();
            return reszletesFoglalasok;
        }

        // 5. Számítsd ki és add vissza egy listában az egyes foglalások teljes árait (EjszakakSzama * ArPerEjszaka), kiegészítve a vendég nevével.
        public List<string> FoglalasokTeljesAra()
        {
            List<string> foglalasokTeljesAra = foglalasok.Join(vendegek, f => f.VendegId, v => v.VendegId, (f, v) => new { f, v })
                .Join(szobak, fv => fv.f.SzobaId, s => s.SzobaId, (fv, s) => new { fv.v.Nev, TeljesAr = fv.f.EjszakakSzama * s.ArPerEjszaka })
                .Select(x => $"{x.Nev} - {x.TeljesAr} Ft")
                .ToList();
            return foglalasokTeljesAra;
        }

        // --- Közép- és haladó feladatok (6 - 10) ---

        // 6. Csoportosítsd a szobákat kategória szerint, és határozd meg az egyes kategóriákhoz tartozó átlagos árat egy Dictionary-ben (Dictionary<string, double>).
        public Dictionary<string, double> AtlagosArKategoriakSzerint()
        {
            Dictionary<string, double> atlagosArKategoriakSzerint = szobak.GroupBy(s => s.Kategoria)
                .ToDictionary(g => g.Key, g => g.Average(s => s.ArPerEjszaka));
            return atlagosArKategoriakSzerint;
        }

        // 7. Add meg azoknak a vendégeknek a nevét egyedi (Distinct) listában, akik "Lakosztály" kategóriájú szobát foglaltak.
        public List<string> LakosztalytFoglaloVendegek()
        {
            List<string> lakosztalytFoglaloVendegek = foglalasok.Join(vendegek, f => f.VendegId, v => v.VendegId, (f, v) => new { f, v })
                .Join(szobak, fv => fv.f.SzobaId, s => s.SzobaId, (fv, s) => new { fv.v.Nev, s.Kategoria })
                .Where(x => x.Kategoria == "Lakosztály")
                .Select(x => x.Nev)
                .Distinct()
                .ToList();
            return lakosztalytFoglaloVendegek;
        }

        // 8. Ki foglalta le a legtöbb éjszakát egyetlen alkalommal? Add meg a leghosszabb foglalás mögött álló vendég nevét.
        public string LeghosszabbFoglalasVendegNeve()
        {
            string leghosszabbFoglalasVendegNeve = foglalasok.Join(vendegek, f => f.VendegId, v => v.VendegId, (f, v) => new { v.Nev, f.EjszakakSzama })
                .OrderByDescending(x => x.EjszakakSzama)
                .Select(x => x.Nev)
                .First();
            return leghosszabbFoglalasVendegNeve;
        }

        // 9. Ellenőrizd, hogy minden egyes foglalásban az éjszakák száma nagyobb-e, mint 0 (All metódussal).
        public bool MindenFoglalasEjszakaPozitiv()
        {
            bool mindenFoglalasEjszakaPozitiv = foglalasok.All(f => f.EjszakakSzama > 0);
            return mindenFoglalasEjszakaPozitiv;
        }

        // 10. Melyik vendég költötte a legtöbbet összesen a szállodában? Számítsd ki minden vendégnél a foglalásai összértékét, rendezd őket csökkenőbe, és add vissza a nevét.
        public string LegtobbetKoltoVendegNeve()
        {
            string legtobbetKoltoVendegNeve = foglalasok.Join(vendegek, f => f.VendegId, v => v.VendegId, (f, v) => new { f, v })
                .Join(szobak, fv => fv.f.SzobaId, s => s.SzobaId, (fv, s) => new { fv.v.Nev, TeljesAr = fv.f.EjszakakSzama * s.ArPerEjszaka })
                .GroupBy(x => x.Nev)
                .Select(g => new { Nev = g.Key, OsszesenKoltes = g.Sum(x => x.TeljesAr) })
                .OrderByDescending(x => x.OsszesenKoltes)
                .Select(x => x.Nev)
                .First();
            return legtobbetKoltoVendegNeve;
        }

        // --- Új, extra nehéz feladatok az óra második felére és házinak (11 - 16) ---

        // 11. Melyik szobakategóriát foglalták le a legtöbbször? Add vissza a kategória nevét!
        public string LegnezettebbSzobaKategoria()
        {
            string legnezettebbSzobaKategoria = foglalasok.Join(szobak, f => f.SzobaId, s => s.SzobaId, (f, s) => s.Kategoria)
                .GroupBy(k => k)
                .MaxBy(g => g.Count())!.Key;
            return legnezettebbSzobaKategoria;
        }

        // 12. Add vissza azoknak a vendégeknek a nevét, akik egynél többször (többszörös foglalás) szerepelnek a rendszerben!
        public List<string> HtöbbszörFoglaloVendegekNeve()
        {
            List<string> htöbbszörFoglaloVendegekNeve = foglalasok.GroupBy(f => f.VendegId)
                .Where(g => g.Count() > 1)
                .Join(vendegek, g => g.Key, v => v.VendegId, (g, v) => v.Nev)
                .ToList();
            return htöbbszörFoglaloVendegekNeve;
        }

        // 13. Számítsd ki a szálloda összbevételét városonként (a vendégek lakóhelye szerint), és adj vissza egy Dictionary<string, int>-et!
        public Dictionary<string, int> BevetelVarosonkent()
        {
            Dictionary<string, int> bevetelVarosonkent = foglalasok.Join(vendegek, f => f.VendegId, v => v.VendegId, (f, v) => new { f, v })
                .Join(szobak, fv => fv.f.SzobaId, s => s.SzobaId, (fv, s) => new { fv.v.Varos, TeljesAr = fv.f.EjszakakSzama * s.ArPerEjszaka })
                .GroupBy(x => x.Varos)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.TeljesAr));
            return bevetelVarosonkent;
        }

        // 14. Készíts egy szótárt, amely kategóriánként megadja, hogy az adott kategóriában mi volt a maximális éjszakák száma egyetlen foglalásban!
        public Dictionary<string, int> MaxEjszakakKategoriakSzerint()
        {
            Dictionary<string, int> maxEjszakakKategoriakSzerint = foglalasok.Join(szobak, f => f.SzobaId, s => s.SzobaId, (f, s) => new { s.Kategoria, f.EjszakakSzama })
                .GroupBy(x => x.Kategoria)
                .ToDictionary(g => g.Key, g => g.Max(x => x.EjszakakSzama));
            return maxEjszakakKategoriakSzerint;
        }

        // 15. Számítsd ki, hogy a "Deluxe" kategóriájú foglalások hány százalékát teszik ki az összes foglalásnak (double értékként)!
        public double DeluxeFoglalasokSzazaleka()
        {
            double deluxeFoglalasokSzazaleka = (double)foglalasok.Join(szobak, f => f.SzobaId, s => s.SzobaId, (f, s) => s.Kategoria)
                .Count(k => k == "Deluxe") / foglalasok.Count * 100;
            return deluxeFoglalasokSzazaleka;
        }

        // 16. Készíts egy teljes összegző szótárt (Dictionary<string, int>), amely minden vendéghez (név) hozzárendeli az általa összesen kifizetett összeget!
        public Dictionary<string, int> VendegKoltsekSzotar()
        {
            Dictionary<string, int> vendegKoltsekSzotar = foglalasok.Join(vendegek, f => f.VendegId, v => v.VendegId, (f, v) => new { f, v })
                .Join(szobak, fv => fv.f.SzobaId, s => s.SzobaId, (fv, s) => new { fv.v.Nev, TeljesAr = fv.f.EjszakakSzama * s.ArPerEjszaka })
                .GroupBy(x => x.Nev).ToDictionary(g => g.Key, g => g.Sum(x => x.TeljesAr));
            return vendegKoltsekSzotar;
        }

        //Saját feladatok:

        // 17. Készíts egy szótárt, amely kategóriánként megadja, hogy az adott kategóriában mennyi bevétel keletkezett összesen!
        public Dictionary<string, int> BevetelKategoriankent()
        {
            Dictionary<string, int> bevetelKategoriankent = foglalasok.Join(szobak, f => f.SzobaId, s => s.SzobaId, (f, s) => new { s.Kategoria, TeljesAr = f.EjszakakSzama * s.ArPerEjszaka })
                .GroupBy(x => x.Kategoria)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.TeljesAr));
            return bevetelKategoriankent;
        }

        // 18. Készíts egy szótárt, amely szobánként megadja, hogy az adott szobában összesen hány éjszakát töltöttek el a vendégek!
        public Dictionary<int, int> SzobakFoglaltsaga()
        {
            Dictionary<int, int> szobakFoglaltsaga = foglalasok.GroupBy(f => f.SzobaId)
                .ToDictionary(g => g.Key, g => g.Sum(f => f.EjszakakSzama));
            return szobakFoglaltsaga;
        }
    }
}