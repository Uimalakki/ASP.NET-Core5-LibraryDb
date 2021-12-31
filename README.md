## Hieman pohdintaa ja perusteluja valitsemalleni toteutustavalle:

Koska julkaisija (Publisher) ja valmistaja (PrintingHouse) -tauluilla oli identtiset kentät, loin kolmannen taulun Company, jonka kumpikin taulu perii.

Pitää hieman avata noiden DtoIn-luokkien runsautta. Sovellus sisältää siis kaksi eri DtoIn luokkaa per malli/taulu. Tämä siksi, että kun migraatio oli tehty, en pystynyt lisäämään mitään sinne jos lisättävä tieto sisälsi sen Id-tunnisteen. Ratkaisuksi tähän tein sitten glögipäissäni toisen DtoIn-luokan, josta puuttui Id. Yritin luntata fiksumman ratkaisun perässä katsomalla siitä sinun MovieApi mallista, miten sinä olit tuon ratkaissut, mutta se kaatui ihan samalla tavalla Id:n takia. Virhe ilmoitus oli siis: Cannot insert explicit value for identity column in table when IDENTITY_INSERT is set to OFF. Olinko sitten ladannut väärän migrationin siitä sinun mallista vai onko tuo joku minun Visual Studion bugi? Tuon identity_insertin olisi voinut laittaa myös päälle, mutta halusin pysyä linjalla, missä tietokanta saa sitten hoitaa itse sen numeroimisen.

En tiedä oliko joku bugi, mutta en löytänyt paikkaa mihin Console.Writeline() tulostaa parametrinsa (ei ainakaan Visual Studion konsoli eikä Swaggerin näyttävän selaimen konsoli), joten jotta pystyin itse varmistamaan tulostukset, kaikki konsolin tulostukset tehdäänkin kahteen kertaan. Ensin tuolla tehtävänannon mukaisella komennolla ja sitten System.Diagnostics.Debug.Print()

Jätin sinun testailuja varten kaikkien sovellusten tietojen hakuun omat kontrollerit, joten tehtävänannosta poiketen tauluista löytyy haut  Id:llä ja perus HttpGetillä. 

Tehtävänannon mukainen kirjojen haku valinnaisilla ja yhdistettävillä hakuehdoilla löytyy BooksControllerin endpoint GetBooksWithOptionalParameters.

Olisi pitänyt luoda tuo migraatio paljon aikaisemmin eikä vasta viimeisellä viikolla, sillä eihän kaikki ominaisuudet toimineetkaan ”oikeassa tietokannassa” ihan samalla tavalla kuin selaimenmuistiin luodussa tietokannassa. Meinasi tulla vähän kiire korjailujen kanssa, vaikka kaikki sovelluksen toiminnot olivatkin olleet toiminnassa jo viikkokausia siellä selaimenmuistissa toimivassa versiossa. Tulipahan kerrattua hieman lisää kurssin asioita.

En tiedä oliko tässä lopulta mitään järkeä, mutta sisällytin LoanChecker-luokkaan kaikki lainan tarkistukseen tarvittavat metodit (bonus 3 ja LoansControllerin käyttämät). Nehän olisi voineet myös laittaa suoraan sinne kontrollerin alle mikä niitä kutsuu. Lähtökohtaisesti ajattelin, että se olisi selkeää, että kaikki on samassa paikassa, jos useammat kontrollerit niitä kutsuvat, mutta nythän niin ei siis olekkaan.

Tietokantaan luodaan alustava data Program.cs luokassa olevilla metodeilla (AddPeopleToDatabase, AddPublishersToDatabase,…, CreateTestDataToDatabase). Siinä luodaan muun muassa kolme kirjaa, kaksi kirjailijaa, kolme asiakasta.


## Bonus 1:
Kirjojen saldojen hoitaminen on eritetty omaksi luokaksi/taulukseen BookCollection, koska halusin pitää kirjan tiedot ja kirjaston saldot omina kokonaisuuksiina. Taulu sisältää myös tehtävänannosta poikkeavan ”ylimääräisen” attribuutin ShelfNumber eli hyllynumeron, sillä ajattelin sen tietämisestä olevan hieman hyötyä kirjaston pyörittämisessä 😊

Eli kun tietokantaan lisää uuden kirjan, ennen kuin niitä voi lainata pitää myös lisätä uusi BookCollection, joka sisältää tiedon kuinka monta kappaletta on saldoilla. Tästä olisi voinut tehdä helposti automaation, että BookControllerin Post-metodissa luodaan myös uusi BookCollection taulu, mutta halusin pitää nuo erillään toisistaan. Olisi ehkä ollut käyttäjäystävällisempääkin tehdä niin, mutta halusin säilyttää tämän ”separation of duties” -mentaliteetin.

Kirjan lainaaminen tapahtuu LoansControllerin Post Loan pyynnöllä, joka tarkastaa samalla onko asiakkaalla erääntyneitä lainoja ja että lainattavia kirjoja on saatavilla kirjaston saldoilla LoanChecker-luokan metodien avulla. 

Lainan palautus tapahtuu saman kontrollerin ReturnLoan endpointilla, joka Put-pyynnön avulla muuttaa lainan tilan palautetuksi. 


## Bonus 2:
CustomersController sisältää metodin ChangeCustomersPermitToLoan(), jonka avulla voidaan muuttaa asiakkaan tila niin, että lainoja ei voi tehdä. Se on suojattu autentikoinnilla. Luokka LibrarianAuthorization.cs hoitaa tämän. Autentikoinnin tiedot:
avain: AuthorizationKey
arvo: ByThePowerOfGreyskull 

Myös saman kontrollerin delete-metodi on suojattu samalla autentikoinnilla.

Autentikaatioluokka on käytettävissä vain metoditasolla, koska sillä suojataan vain nuo kaksi metodia. Toki sillä olisi voinut suojata vähintäänkin sovelluksen kaikki post- ja put-endpointit, mikä varmasti olisi ollutkin järkevää oikeassa maailmassa, mutta tehtävänannon ja testaamisen suoraviivaisuuden takia suojasin vain nuo kaksi.


## Bonus 3:
LoanChecker-luokka sisältää metodin CheckDueLoans, joka tarkistaa tietokannan kaikki lainat, joita ei ole palautettu ja suorittaa tehtävänannon mukaiset toimet: jos laina umpeutuu viikon kuluessa, metodi lähettää sitä kutsuttaessa yhden viestin per päivä ja jos laina on vanhentunut siitä muistutetaan vain kerran.

