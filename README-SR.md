# RapidTDD - Rapid TDD aplikacija

> 🇬🇧 [English version / Engleska verzija](./README.md)

Kompajlira, izvršava C# kod u memoriji i izvršava testove sa code coverage opcijom - bez refaktorisanja, bez assert-ova, mock-ova ili testability layer-a.

**Source code:** https://github.com/Darko-Bondjerovic/RapidTDD

Više informacija na youtube RapidTDD kanalu:
https://www.youtube.com/@rapidtdd

Najnovija verzija RapidTDD je u releases:
https://github.com/Darko-Bondjerovic/RapidTDD/releases/

![alt text](RapidTDD.png?raw=true)

---

### Osnovna ideja

Sa `[TEST]` markerom u outputu teksta, zbog toga što se `Console.WriteLine` komande mogu pozivati bilo gde u kodu, RapidTDD može da testira i **privatne** i **void** metode (koje ne vraćaju rezultat) - bez refaktorisanja koda, bez assertova, testability layer-a, mock-ova itd. (što je neophodno u klasičnim alatima kao npr. NUnit, xUnit...)

**Aktuelni rezultat testa (actual result)** – aplikacija izdvaja tekst od prvog mesta gde se pojavljuje `[TEST]` marker, do sledećeg mesta gde se pojavljuje `[TEST]` marker.

**Očekivani rezultat (expected result)** se može kreirati na sledeće načine:
1. Korisnik u okviru aplikacije ručno unosi expected kao proizvoljan tekst (u text edit box-u)
2. U aplikaciji je moguće direktno kopirati aktuelni rezultat u očekivani
3. Korišćenjem `[EXPC]` markera u C# kodu i upisom vrednosti u sam kod (kao u NUnit-u)

> **Napomena:** Tekst definisan sa `[EXPC]` ima veći prioritet, te zamenjuje prethodno memorisan tekst.

### Primer

```csharp
FindPrimesTest(15);

static void FindPrimesTest(int input)
{
    Console.WriteLine($"[TEST] Find primes {input}");
    Console.WriteLine(FindPrimes(input).ToString());
    if (input == 15) Console.WriteLine("[EXPC]3\n5");
}
```

### Radni tok (workflow)

1. Napišemo `Console.WriteLine("[TEST] <naziv>")` na mestima gde želimo da vidimo šta se dogodilo, bez menjanja ostatka koda 
2. Pokrenemo program kroz RapidTDD - dobijamo actual rezultate za sve testove
3. Kliknemo 'Copy actual to expected' - za sve testove odjednom (ili pojedinačno)
4. Aplikacija sada ima sve expected vrednosti i poredi ih sa aktuelnim
5. Sada imamo zeleni baseline – all tests pass
6. Refaktorišemo kod – prilikom svakog pokretanja koda, RapidTDD automatski ponovo poredi i obeležava testove sa fail/pass.

### Prednosti u odnosu na klasične test framework-e

**Ne refaktorišemo kod odmah**
- Postojeći kod može ostati 100% kakav jeste
- Testiramo kod usred privatnih metoda - ne moramo da ih menjamo u public
- Testiramo međurezultate, metoda može biti void - samo ispišemo vrednosti
- Nije potreban testability layer, DI, interface, Mock, Assert

**Jednostavnost**
- Nazivi testova su proizvoljni - nisu nazivi metoda
- Ne moramo da definišemo expected unapred
- Možemo sortirati redosled izvršavanja direktno u kodu
- Kreiranje N testova u petlji:
```csharp
for(int i=0; i<100; i++) { 
    Console.WriteLine($"[TEST] test {i}"); 
    ExecuteSomeMethod(i); 
}
```

**Brzo izvršavanje**
- Build i izvršavanje se obavlja u RAM memoriji, bez upisa na disk
- Komplikovani ispisi (liste, matrice, stabla) se provere odmah - poredi se tekst

### Dodatne mogućnosti

- Interni Code Coverage (Glavni meni UI: Views → Coverage)
- Spisak testova, actual i expected vrednosti se mogu sačuvati/učitati preko Tests → Save test file / Load test file

Code Coverage unutar RapidTDD-a:

![alt text](CodeCover.png?raw=true)

---

### Kako doprineti

Ako želite da doprinesete, kreirajte issue ili pogledajte postojeće issue-e i pošaljite pull request. Kao i uvek: ako planirate veliku promenu, hajde prvo da se dogovorimo da izbegnemo nepotreban rad.

Moj email: rapidtdd@gmail.com

### For Developers
For technical stack and future migration plans, see [DEVELOPMENT.md](./DEVELOPMENT.md) and [ROADMAP.md](./ROADMAP.md).