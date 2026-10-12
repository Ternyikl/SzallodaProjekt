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
            throw new NotImplementedException();
        }

        // 4. Írj lekérdezést, amely összeköti a foglalásokat a vendégekkel és a szobákkal (dupla JOIN), és egy olyan listát ad vissza, amelyben szerepel a vendég neve, a szoba kategóriája és az éjszakák száma.
        public List<string> ReszletesFoglalasok()
        {
            throw new NotImplementedException();
        }

        // 5. Számítsd ki és add vissza egy listában az egyes foglalások teljes árait (EjszakakSzama * ArPerEjszaka), kiegészítve a vendég nevével.
        public List<string> FoglalasokTeljesAra()
        {
            throw new NotImplementedException();
        }

        // --- Közép- és haladó feladatok (6 - 10) ---

        // 6. Csoportosítsd a szobákat kategória szerint, és határozd meg az egyes kategóriákhoz tartozó átlagos árat egy Dictionary-ben (Dictionary<string, double>).
        public Dictionary<string, double> AtlagosArKategoriakSzerint()
        {
            throw new NotImplementedException();
        }

        // 7. Add meg azoknak a vendégeknek a nevét egyedi (Distinct) listában, akik "Lakosztály" kategóriájú szobát foglaltak.
        public List<string> LakosztalytFoglaloVendegek()
        {
            throw new NotImplementedException();
        }

        // 8. Ki foglalta le a legtöbb éjszakát egyetlen alkalommal? Add meg a leghosszabb foglalás mögött álló vendég nevét.
        public string LeghosszabbFoglalasVendegNeve()
        {
            throw new NotImplementedException();
        }

        // 9. Ellenőrizd, hogy minden egyes foglalásban az éjszakák száma nagyobb-e, mint 0 (All metódussal).
        public bool MindenFoglalasEjszakaPozitiv()
        {
            throw new NotImplementedException();
        }

        // 10. Melyik vendég költötte a legtöbbet összesen a szállodában? Számítsd ki minden vendégnél a foglalásai összértékét, rendezd őket csökkenőbe, és add vissza a nevét.
        public string LegtobbetKoltoVendegNeve()
        {
            throw new NotImplementedException();
        }

        // --- Új, extra nehéz feladatok az óra második felére és házinak (11 - 16) ---

        // 11. Melyik szobakategóriát foglalták le a legtöbbször? Add vissza a kategória nevét!
        public string LegnezettebbSzobaKategoria()
        {
            throw new NotImplementedException();
        }

        // 12. Add vissza azoknak a vendégeknek a nevét, akik egynél többször (többszörös foglalás) szerepelnek a rendszerben!
        public List<string> HtöbbszörFoglaloVendegekNeve()
        {
            throw new NotImplementedException();
        }

        // 13. Számítsd ki a szálloda összbevételét városonként (a vendégek lakóhelye szerint), és adj vissza egy Dictionary<string, int>-et!
        public Dictionary<string, int> BevetelVarosonkent()
        {
            throw new NotImplementedException();
        }

        // 14. Készíts egy szótárt, amely kategóriánként megadja, hogy az adott kategóriában mi volt a maximális éjszakák száma egyetlen foglalásban!
        public Dictionary<string, int> MaxEjszakakKategoriakSzerint()
        {
            throw new NotImplementedException();
        }

        // 15. Számítsd ki, hogy a "Deluxe" kategóriájú foglalások hány százalékát teszik ki az összes foglalásnak (double értékként)!
        public double DeluxeFoglalasokSzazaleka()
        {
            throw new NotImplementedException();
        }

        // 16. Készíts egy teljes összegző szótárt (Dictionary<string, int>), amely minden vendéghez (név) hozzárendeli az általa összesen kifizetett összeget!
        public Dictionary<string, int> VendegKoltsekSzotar()
        {
            throw new NotImplementedException();
        }
    }
}