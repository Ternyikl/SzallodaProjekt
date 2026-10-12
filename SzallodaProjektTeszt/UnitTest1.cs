using FsCheck.Experimental;
using NUnit.Framework;
using System.IO;

namespace SzallodaProjekt.Teszt
{
    [TestFixture]
    public class SzallodaElemzoTests
    {
        private string vFile;
        private string sFile;
        private string fFile;
        private SzallodaElemzo elemzo;

        [SetUp]
        public void Setup()
        {
            vFile = Path.GetTempFileName();
            sFile = Path.GetTempFileName();
            fFile = Path.GetTempFileName();

            // Teszt adatok készítése
            File.WriteAllText(vFile, "VendegId;Nev;Varos\n1;Kovács Anna;Budapest\n2;Nagy Béla;Debrecen\n3;Tóth Eszter;Budapest");
            File.WriteAllText(sFile, "SzobaId;Szobaszam;Kategoria;ArPerEjszaka\n101;10;Standard;15000\n102;20;Deluxe;30000\n103;30;Lakosztály;60000");
            File.WriteAllText(fFile, "FoglalasId;VendegId;SzobaId;EjszakakSzama;FoglalasDatuma\n1;1;101;3;2026-06-01\n2;2;102;5;2026-06-05\n3;1;103;2;2026-07-10\n4;2;101;1;2026-08-01");

            this.elemzo = new SzallodaElemzo(vFile, sFile, fFile);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(this.vFile)) File.Delete(this.vFile);
            if (File.Exists(this.sFile)) File.Delete(this.sFile);
            if (File.Exists(this.fFile)) File.Delete(this.fFile);
        }

        [Test]
        public void Test_BudapestiVendegekNeve()
        {
            var eredmeny = this.elemzo.BudapestiVendegekNeve();
            Assert.That(eredmeny, Is.EquivalentTo(new[] { "Kovács Anna", "Tóth Eszter" }));
        }

        [Test]
        public void Test_DeluxeSzobakArSzerint()
        {
            var eredmeny = this.elemzo.DeluxeSzobakArSzerint();
            Assert.That(eredmeny.Count, Is.EqualTo(1));
            Assert.That(eredmeny[0].Kategoria, Is.EqualTo("Deluxe"));
        }

        [Test]
        public void Test_FoglalasokSzoveggel()
        {
            var eredmeny = this.elemzo.FoglalasokSzoveggel();
            Assert.That(eredmeny, Does.Contain("Kovács Anna - 3 éjszaka"));
        }

        [Test]
        public void Test_ReszletesFoglalasok()
        {
            var eredmeny = this.elemzo.ReszletesFoglalasok();
            Assert.That(eredmeny, Is.Not.Null);
        }

        [Test]
        public void Test_FoglalasokTeljesAra()
        {
            var eredmeny = this.elemzo.FoglalasokTeljesAra();
            Assert.That(eredmeny, Does.Contain("Kovács Anna - 45000 Ft"));
        }

        [Test]
        public void Test_AtlagosArKategoriakSzerint()
        {
            var eredmeny = this.elemzo.AtlagosArKategoriakSzerint();
            Assert.That(eredmeny, Is.Not.Null);
            Assert.That(eredmeny["Standard"], Is.EqualTo(15000));
        }

        [Test]
        public void Test_LakosztalytFoglaloVendegek()
        {
            var eredmeny = this.elemzo.LakosztalytFoglaloVendegek();
            Assert.That(eredmeny, Is.EquivalentTo(new[] { "Kovács Anna" }));
        }

        [Test]
        public void Test_LeghosszabbFoglalasVendegNeve()
        {
            var eredmeny = this.elemzo.LeghosszabbFoglalasVendegNeve();
            Assert.That(eredmeny, Is.EqualTo("Nagy Béla"));
        }

        [Test]
        public void Test_MindenFoglalasEjszakaPozitiv()
        {
            var eredmeny = this.elemzo.MindenFoglalasEjszakaPozitiv();
            Assert.That(eredmeny, Is.True);
        }

        [Test]
        public void Test_LegtobbetKoltoVendegNeve()
        {
            var eredmeny = this.elemzo.LegtobbetKoltoVendegNeve();
            Assert.That(eredmeny, Is.EqualTo("Kovács Anna"));
        }

        [Test]
        public void Test_LegnezettebbSzobaKategoria()
        {
            var eredmeny = this.elemzo.LegnezettebbSzobaKategoria();
            Assert.That(eredmeny, Is.EqualTo("Standard"));
        }

        [Test]
        public void Test_HtöbbszörFoglaloVendegekNeve()
        {
            var eredmeny = this.elemzo.HtöbbszörFoglaloVendegekNeve();
            Assert.That(eredmeny, Is.EquivalentTo(new[] { "Kovács Anna", "Nagy Béla" }));
        }

        [Test]
        public void Test_BevetelVarosonkent()
        {
            var eredmeny = this.elemzo.BevetelVarosonkent();
            Assert.That(eredmeny["Budapest"], Is.EqualTo(165000));
            Assert.That(eredmeny["Debrecen"], Is.EqualTo(165000));
        }

        [Test]
        public void Test_MaxEjszakakKategoriakSzerint()
        {
            var eredmeny = this.elemzo.MaxEjszakakKategoriakSzerint();
            Assert.That(eredmeny["Standard"], Is.EqualTo(3));
            Assert.That(eredmeny["Deluxe"], Is.EqualTo(5));
        }

        [Test]
        public void Test_DeluxeFoglalasokSzazaleka()
        {
            var eredmeny = this.elemzo.DeluxeFoglalasokSzazaleka();
            // 4-ből 1 deluxe -> 25.0%
            Assert.That(eredmeny, Is.EqualTo(25.0).Within(0.01));
        }

        [Test]
        public void Test_VendegKoltsekSzotar()
        {
            var eredmeny = this.elemzo.VendegKoltsekSzotar();
            Assert.That(eredmeny["Kovács Anna"], Is.EqualTo(165000));
            Assert.That(eredmeny["Nagy Béla"], Is.EqualTo(165000));
        }
    }
}