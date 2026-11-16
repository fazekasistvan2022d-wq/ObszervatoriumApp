using ObszervatoriumApp;
List<Tavcso> csovek = new List<Tavcso>();
int hibasSorok = 0;
foreach (var item in File.ReadAllLines("tavcsovek.txt").Skip(1))
{
    string[] adatok = item.Split("|");
    if(adatok.Count() != 5)
    {
        hibasSorok++;
        continue;
    }
    if (adatok[0] == "O")
    {
        csovek.Add(new Tavcso(adatok[1], Convert.ToInt32(adatok[2]), Convert.ToInt32(adatok[3])));
    }
    else if (adatok[0] == "R")
    {
        csovek.Add(new Radiotavcso(adatok[1], Convert.ToInt32(adatok[2]), Convert.ToInt32(adatok[3]), Convert.ToInt32(adatok[4])));
    }
    
}

Console.WriteLine($"Sikeresen beolvasott távcsövek: {csovek.Count()} db");
Console.WriteLine($"Kiszűrt hibás sorok száma: {hibasSorok} db");
Console.WriteLine();
Console.WriteLine("Beolvasott távcsövek listája:");
foreach (var item in csovek)
{
    Console.WriteLine(item);
}
Console.WriteLine();
Console.WriteLine("LINQ Statisztikák:");
Console.WriteLine($"Összesített éves fenntartási költség: {csovek.Sum(cso => cso.FenntartasiKoltseg())} EUR");
Console.WriteLine($"Átlagos üzemóra: {csovek.Average(cso => cso.Uzemora):F1} óra");
Console.WriteLine($"Legnagyobb átmérőjű távcső: {csovek.MaxBy(cso => cso.AtmeroCm).Azonosito} ({csovek.MaxBy(cso => cso.AtmeroCm).AtmeroCm} cm)");
Console.WriteLine($"Legalacsonyabb költségű távcső: {csovek.MinBy(cso => cso.FenntartasiKoltseg()).Azonosito} ({csovek.MinBy(cso => cso.FenntartasiKoltseg()).FenntartasiKoltseg()} EUR)");
Console.WriteLine($"Legalább 1000 órát üzemelt távcsövek: {csovek.Where(cso => cso.Uzemora >= 1000).Count()} db");
bool van_e = csovek.Any(cso => cso.FenntartasiKoltseg() > 40_000);
if (van_e)
{
    Console.WriteLine("Van 40 000 EUR feletti költségű távcső: Van");
}
else
{
    Console.WriteLine("Van 40 000 EUR feletti költségű távcső: Nincs");
}
Console.WriteLine();
Console.WriteLine($"Rádiótávcsövek átlagos frekvenciája: {csovek.OfType<Radiotavcso>().Average(r => r.FrekvenciaMHz)} MHz");
Console.WriteLine("Legalább 300 cm átmérőjű távcsövek (csökkenő sorrendben):");
foreach (var item in csovek)
{
    if(item.AtmeroCm >= 300)
    {
        Console.WriteLine($"- {item.Azonosito} ({item.AtmeroCm} cm)");
    }
}
Console.WriteLine();
var kimenet = csovek.Where(cso => cso.Uzemora > 500).OrderBy(cso => cso.Uzemora).Select(cso => $"{cso.Azonosito};{cso.AtmeroCm};{cso.Uzemora};{cso.FenntartasiKoltseg()}");
File.WriteAllLines("aktiv_tavcsovek.txt", kimenet);
Console.WriteLine("Az aktiv_tavcsovek.txt fájl sikeresen elkészült.");
Console.WriteLine($"Távcső példányok száma (static): {Tavcso.OsszesTavcso}");