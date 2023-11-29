using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics.Metrics;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using LibraryDatabase.Objects;
using System.Security.Policy;


namespace LibraryDatabase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog= LibraryDB;Integrated Security=SSPI;";

        public App()
        {
            string sqlConnectionString = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog= LibraryDB;Integrated Security=SSPI;";
            string userName = Environment.UserName;
            FileInfo file = new FileInfo(@"C:\Users\"+userName+ @"\source\repos\LibraryDatabase\LibraryDatabase\Scripts\CreateLibraryDatabase.sql");
            string script = file.OpenText().ReadToEnd();
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            Microsoft.SqlServer.Management.Common.ServerConnection connection = new Microsoft.SqlServer.Management.Common.ServerConnection(conn);
            Server server = new Server(connection);

            server.ConnectionContext.ExecuteNonQuery(script);
            setupTables();
            int x = 1;
            InsertBook("Dummy", x, "1", "Test", "0001-01-01", "Nobody", x);

            using (SqlConnection connection2 = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection2.CreateCommand())
                {

                    command.CommandText = "INSERT INTO [LibraryDB].[Shelf](ShelfNumber, Section) VALUES('0', '0')";
                    connection2.Open();
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[BookCopy](BookTitleID, ShelfID, [PageCount], DamageLevel) VALUES('1', '1', '0', '999')";
                    command.ExecuteNonQuery();

                    connection2.Close();
                }
            }
            DamageControll();
            DamageControll2();
        }

        private void setupTables()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    #region The Genre generic Inserts
                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Action+"')";
                    connection.Open();
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Adventure+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Art+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.AutoBiography+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Biography+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Dystopian+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.Fantasy+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('"+GenreEnum.GraphicNovel+"')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.HistoricalFiction + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.History + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Horror + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Humor + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Mystery + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Religious + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Romance + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Science + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.ScienceFiction + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.SelfHelp + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Thriller + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.Travel + "')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Genre](GenreName) VALUES('" + GenreEnum.TrueCrime + "')";
                    command.ExecuteNonQuery();
                    #endregion

                    #region The Audience generic Inserts
                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Children + "', '0')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Teens + "', '0')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.YoungAdults + "', '0')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Adults + "', '1')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Men + "', '1')";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [LibraryDB].[Audience](AudienceName, KidsRead) VALUES('" + AudienceEnum.Women + "', '1')";
                    command.ExecuteNonQuery();
                    #endregion

                    command.CommandText = "INSERT INTO [LibraryDB].[Author](FullName) VALUES('Dummy')";
                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }

            
        }

        /// <summary>
        /// Don't Ever Touch
        /// </summary>
        private void DamageControll()
        {
            InsertPatron(957256325, "Caresse Perford", "344-939-8077", "634 Cody Center", "8/29/2022", true);
            InsertPatron(957256326, "Raimundo Simao", "946-184-5492", "9 Loftsgordon Alley", "2/14/2022", true);
            InsertPatron(957256327, "Darren Grassick", "845-187-2923", "07311 Waubesa Court", "8/5/2022", true);
            InsertPatron(957256328, "Cathrin Bestwick", "764-132-1408", "53644 International Parkway", "9/18/2022", false);
            InsertPatron(957256329, "Adelaida Vertigan", "798-195-0136", "81 Northwestern Plaza", "7/4/2023", true);
            InsertPatron(957256330, "Abigail Goldberg", "181-498-6004", "5 Helena Point", "11/24/2023", false);
            InsertPatron(957256331, "Maison Kenchington", "518-887-6792", "86 Warrior Hill", "10/22/2021", false);
            InsertPatron(957256332, "Sula Scoggans", "574-950-1335", "685 Fuller Center", "9/2/2021", false);
            InsertPatron(957256333, "Tonye Frensch", "133-588-7175", "1077 Golf View Place", "2/26/2022", false);
            InsertPatron(957256334, "Vernen Bullen", "760-731-3394", "6237 Gateway Point", "12/31/2022", true);
            InsertPatron(957256335, "Kristan Cabel", "970-952-0497", "7086 Hooker Circle", "1/8/2023", true);
            InsertPatron(957256336, "Coralie Akeherst", "301-588-3330", "009 High Crossing Drive", "11/1/2021", false);
            InsertPatron(957256337, "Halley Matthewes", "591-886-8530", "903 Hazelcrest Alley", "3/15/2023", true);
            InsertPatron(957256338, "Daffy Vant Hoff", "739-233-8105", "8 Pierstorff Crossing", "11/18/2023", true);
            InsertPatron(957256339, "Mohandis Scutter", "703-794-0021", "57001 Stang Place", "9/6/2022", false);
            InsertPatron(957256340, "Saunderson Pembery", "560-293-0240", "74 Donald Plaza", "5/2/2023", true);
            InsertPatron(957256341, "Donelle Stowte", "514-875-4793", "80 Mayfield Circle", "4/30/2022", false);
            InsertPatron(957256342, "Kipp Plews", "958-494-9205", "170 Stuart Terrace", "7/3/2022", false);
            InsertPatron(957256343, "Melloney Lindwasser", "150-770-5995", "32 Monterey Way", "6/22/2022", false);
            InsertPatron(957256344, "Benn Rosenfarb", "648-526-8403", "74 Fairfield Road", "10/21/2022", false);
            InsertPatron(957256345, "Cherida Munns", "280-859-5125", "181 Bashford Junction", "10/30/2023", false);
            InsertPatron(957256346, "Prisca Kilbee", "639-611-3324", "837 Comanche Lane", "7/13/2022", true);
            InsertPatron(957256347, "Jerrome Valentin", "529-630-5794", "1261 Buena Vista Circle", "8/22/2021", false);
            InsertPatron(957256348, "Ag Tallowin", "703-619-3286", "2355 2nd Hill", "8/29/2022", true);
            InsertPatron(957256349, "Tully Domniney", "951-470-9130", "90502 Moose Crossing", "7/16/2022", true);
            InsertPatron(957256350, "Wallis Cosans", "467-261-2792", "346 Sunfield Court", "12/18/2022", false);
            InsertPatron(957256351, "Rosabella Scoular", "246-648-9862", "57950 Meadow Ridge Place", "6/9/2023", true);
            InsertPatron(957256352, "Rosalind Panting", "450-591-5075", "55 1st Hill", "6/28/2022", true);
            InsertPatron(957256353, "Jandy Dils", "730-302-8390", "2 Rigney Drive", "12/27/2021", true);
            InsertPatron(957256354, "Ranice Gounot", "404-565-8566", "5 Hanson Alley", "4/19/2022", false);
            InsertPatron(957256355, "Elke Stango", "993-732-9492", "4206 Gina Circle", "8/30/2021", true);
            InsertPatron(957256356, "Theresita Larcher", "192-249-1758", "56824 Graceland Lane", "10/27/2021", true);
            InsertPatron(957256357, "Lance Burgin", "258-996-9112", "65 Acker Road", "10/3/2022", false);
            InsertPatron(957256358, "Rowan Klees", "588-658-7404", "93157 Fair Oaks Park", "8/15/2022", false);
            InsertPatron(957256359, "Markus Guiel", "624-546-8005", "12622 Fulton Way", "9/15/2022", true);
            InsertPatron(957256360, "Daryl Abramow", "893-192-7706", "0 Schurz Road", "10/3/2022", true);
            InsertPatron(957256361, "Camala Fansy", "876-186-3810", "47199 Kipling Court", "10/22/2023", true);
            InsertPatron(957256362, "Elfreda Askam", "270-301-2926", "49 Lunder Pass", "7/7/2022", true);
            InsertPatron(957256363, "Rakel Manilove", "869-183-6019", "764 Acker Plaza", "10/30/2023", true);
            InsertPatron(957256364, "Drusi Warden", "191-987-9495", "9419 Del Sol Place", "1/11/2022", false);
            InsertPatron(957256365, "Barry Leaf", "954-690-9250", "236 American Hill", "2/24/2023", true);
            InsertPatron(957256366, "Lauraine Lage", "756-419-4467", "1 Mccormick Hill", "5/30/2023", false);
            InsertPatron(957256367, "Ericha Spillane", "621-131-2267", "86 Mitchell Way", "11/11/2021", true);
            InsertPatron(957256368, "Theodore Pollington", "242-443-5852", "387 Monument Drive", "1/5/2022", false);
            InsertPatron(957256369, "Haze Silk", "278-733-2491", "10758 Brentwood Trail", "9/4/2023", true);
            InsertPatron(957256370, "Lynett Pease", "484-130-6971", "2 Glendale Drive", "12/1/2021", true);
            InsertPatron(957256371, "Ailey Enright", "253-895-9446", "25644 Carioca Lane", "9/22/2022", true);
            InsertPatron(957256372, "Dun Vlasenkov", "814-728-4734", "9243 Portage Place", "6/19/2022", true);
            InsertPatron(957256373, "Angelo Bussen", "186-359-3597", "7954 Northwestern Circle", "2/25/2022", true);
            InsertPatron(957256374, "Maurise Kirkland", "167-947-6485", "3499 Iowa Alley", "12/18/2022", false);
            InsertPatron(957256375, "Gillian Hunnaball", "179-154-7689", "0 Messerschmidt Park", "3/6/2022", true);
            InsertPatron(957256376, "Miller Kleinholz", "287-666-8984", "2 Myrtle Point", "1/3/2022", false);
            InsertPatron(957256377, "Sena McFee", "134-153-8200", "84481 Little Fleur Lane", "3/3/2022", false);
            InsertPatron(957256378, "Lewes Woolaghan", "711-971-7577", "53 Portage Lane", "5/6/2022", true);
            InsertPatron(957256379, "Julie Alesbrook", "370-198-0876", "55585 Saint Paul Avenue", "9/10/2022", false);
            InsertPatron(957256380, "Karney Mapledorum", "996-396-7956", "64 Barby Alley", "10/29/2022", false);
            InsertPatron(957256381, "Susanetta Takos", "795-161-2427", "236 Dayton Center", "1/7/2022", false);
            InsertPatron(957256382, "Diena Carling", "253-147-3495", "944 Lake View Place", "10/9/2023", true);
            InsertPatron(957256383, "Carlota Swafford", "114-508-9418", "90 Haas Way", "8/30/2023", true);
            InsertPatron(957256384, "Alexis Gartshore", "518-317-2705", "32665 Sachs Way", "3/17/2022", false);
            InsertPatron(957256385, "Gussie Grattan", "581-458-2020", "416 Arizona Street", "4/2/2023", false);
            InsertPatron(957256386, "Delia Rothschild", "203-520-8120", "6232 Lotheville Alley", "8/24/2022", true);
            InsertPatron(957256387, "Teodoor Euesden", "556-306-7859", "14 Fordem Way", "10/26/2023", true);
            InsertPatron(957256388, "Alf Commander", "864-796-3578", "7 Oakridge Drive", "1/12/2022", true);
            InsertPatron(957256389, "Codie Moulsdall", "202-940-7761", "20858 Sage Trail", "12/4/2022", true);
            InsertPatron(957256390, "Hebert Boland", "841-140-5419", "063 Dunning Parkway", "10/4/2021", false);
            InsertPatron(957256391, "Donelle Romanini", "442-181-1619", "00018 Vahlen Avenue", "1/20/2023", false);
            InsertPatron(957256392, "Laurella Rist", "972-239-7754", "054 Dapin Center", "8/5/2022", false);
            InsertPatron(957256393, "Fairfax Giddons", "849-713-9184", "5061 Karstens Circle", "9/22/2022", true);
            InsertPatron(957256394, "Jeanine Akitt", "210-806-9920", "129 1st Point", "6/16/2022", false);
            InsertPatron(957256395, "Orelie Willard", "517-384-1844", "21 Sunfield Road", "3/26/2023", true);
            InsertPatron(957256396, "Kayle Guerin", "822-806-0789", "976 Debs Crossing", "4/1/2023", false);
            InsertPatron(957256397, "Jaynell Baignard", "806-780-2707", "12 Everett Plaza", "12/30/2021", false);
            InsertPatron(957256398, "Blakelee Pendlington", "327-281-2470", "683 Dovetail Road", "11/25/2022", false);
            InsertPatron(957256399, "Brena Life", "770-796-7702", "6 Sheridan Circle", "8/10/2023", false);
            InsertPatron(957256400, "Melloney Tolson", "653-331-8341", "45 Londonderry Alley", "12/17/2022", false);
            InsertPatron(957256401, "Marley Napleton", "465-909-3985", "211 Fallview Crossing", "1/6/2022", false);
            InsertPatron(957256402, "Berkie Radin", "230-728-4185", "302 Maryland Way", "10/25/2021", true);
            InsertPatron(957256403, "Emmet Servis", "891-499-6102", "746 Acker Parkway", "6/21/2023", false);
            InsertPatron(957256404, "Titos Geldert", "463-773-0394", "8 Reindahl Plaza", "8/3/2022", true);
            InsertPatron(957256405, "Edythe Slocomb", "730-797-1344", "8 Center Avenue", "7/6/2023", false);
            InsertPatron(957256406, "Marysa Dadge", "186-269-8328", "31267 Nelson Lane", "6/19/2023", true);
            InsertPatron(957256407, "Oran Barrat", "401-478-6054", "285 Sunfield Hill", "7/28/2023", false);
            InsertPatron(957256408, "Ashton Josh", "922-772-8210", "88 Trailsway Point", "11/5/2022", false);
            InsertPatron(957256409, "Tommy Avrasin", "520-956-9787", "419 Ryan Plaza", "5/13/2023", true);
            InsertPatron(957256410, "Brook Agget", "436-573-3858", "87321 Cherokee Plaza", "10/31/2021", true);
            InsertPatron(957256411, "Anitra Kingsly", "805-611-2333", "355 Scott Court", "9/17/2021", true);
            InsertPatron(957256412, "Rahel Bonde", "655-242-6840", "7 Tennessee Drive", "4/9/2022", false);
            InsertPatron(957256413, "Emlyn Bernhardsson", "113-887-6382", "24637 Northridge Hill", "10/10/2021", false);
            InsertPatron(957256414, "Andrea Bidewel", "264-484-1019", "49859 Talisman Drive", "5/15/2023", true);
            InsertPatron(957256415, "Korney Lumb", "911-985-0834", "2935 Fremont Alley", "11/16/2022", false);
            InsertPatron(957256416, "Kara-lynn Geraghty", "107-652-4657", "58252 Atwood Circle", "4/2/2022", false);
            InsertPatron(957256417, "Eziechiele Mulbry", "920-770-2524", "31 Bayside Way", "3/21/2023", false);
            InsertPatron(957256418, "Alejandrina Bank", "692-923-2251", "3256 Heath Crossing", "4/14/2022", true);
            InsertPatron(957256419, "Dene Arendsen", "300-191-2117", "21 Center Terrace", "2/1/2022", true);
            InsertPatron(957256420, "Terri Eglese", "588-963-9513", "62 Sherman Circle", "4/4/2022", true);
            InsertPatron(957256421, "Neale Saphin", "732-748-1873", "813 Buena Vista Park", "4/28/2023", false);
            InsertPatron(957256422, "Xymenes Woloschinski", "495-752-2627", "2 Holy Cross Lane", "3/11/2023", false);
            InsertPatron(957256423, "Delores Vasilenko", "126-441-9357", "63582 Swallow Circle", "3/5/2023", true);
            InsertPatron(957256424, "Iain Hailes", "230-639-2802", "69 Warrior Drive", "7/7/2023", true);
            InsertPatron(957256425, "Normand Gipson", "820-639-3666", "77 Anhalt Junction", "5/24/2023", true);
            InsertPatron(957256426, "Ingaberg Croxon", "769-428-3097", "6678 Riverside Avenue", "2/17/2023", true);
            InsertPatron(957256427, "Diann Wroughton", "962-596-9942", "3 Montana Pass", "1/23/2023", true);
            InsertPatron(957256428, "Roseline Collister", "902-365-7043", "648 Algoma Plaza", "11/22/2023", true);
            InsertPatron(957256429, "Payton Parchment", "304-216-5939", "3117 Graceland Alley", "10/31/2022", true);
            InsertPatron(957256430, "La verne Barkes", "575-221-1349", "33502 Monica Circle", "9/11/2022", true);
            InsertPatron(957256431, "Dawna Hambribe", "455-260-2189", "21 Anderson Terrace", "4/26/2022", true);
            InsertPatron(957256432, "Farrah Edgeler", "284-753-8106", "4 Springview Drive", "12/18/2021", true);
            InsertPatron(957256433, "Tabitha Coaker", "786-205-6784", "56067 Debra Junction", "6/16/2023", true);
            InsertPatron(957256434, "Cara Ferandez", "185-613-1808", "57 Delladonna Avenue", "5/8/2022", false);
            InsertPatron(957256435, "Saxon Noriega", "664-359-9508", "857 Gateway Place", "2/3/2022", true);
            InsertPatron(957256436, "Odella Palmar", "421-369-5061", "0422 Debs Way", "10/12/2021", true);
            InsertPatron(957256437, "Denny Drugan", "692-312-6214", "888 Gulseth Terrace", "9/21/2021", true);
            InsertPatron(957256438, "Sampson Rohfsen", "138-925-7000", "67253 Washington Hill", "10/3/2022", true);
            InsertPatron(957256439, "Pattie Lowerson", "326-646-3870", "527 Cody Alley", "8/3/2023", true);
            InsertPatron(957256440, "Kip Challiss", "377-413-3280", "213 Dwight Hill", "8/3/2022", true);
            InsertPatron(957256441, "Ida Rosenbush", "840-717-9492", "5422 Monterey Trail", "12/19/2021", true);
            InsertPatron(957256442, "Burg Tribe", "201-436-1987", "7 Morrow Street", "6/14/2022", true);
            InsertPatron(957256443, "Burnaby Iannetti", "278-468-3384", "57152 Golden Leaf Street", "2/16/2023", true);
            InsertPatron(957256444, "Babara Pinilla", "978-272-5939", "84 Rutledge Place", "9/15/2023", true);
            InsertPatron(957256445, "Tabbie Macartney", "542-414-4251", "9 Goodland Alley", "10/23/2022", true);
            InsertPatron(957256446, "Ulrikaumeko Roy", "278-828-4568", "9639 Pepper Wood Point", "10/8/2023", false);
            InsertPatron(957256447, "Burt Kornalik", "871-125-8123", "271 Everett Center", "2/18/2023", false);
            InsertPatron(957256448, "Jule Fruin", "888-611-5992", "3 Cottonwood Point", "1/13/2023", false);
            InsertPatron(957256449, "Wilhelm Oliveras", "289-834-1161", "1 Linden Circle", "1/27/2023", true);
            InsertPatron(957256450, "Lebbie Pragnall", "350-461-1524", "0 Westridge Trail", "2/14/2023", true);
            InsertPatron(957256451, "Kent Chadwen", "258-603-0093", "99631 Clarendon Hill", "5/21/2022", false);
            InsertPatron(957256452, "Berni Epperson", "402-319-5586", "790 Hallows Trail", "5/8/2023", false);
            InsertPatron(957256453, "Saundra Packer", "649-894-2371", "1821 Northview Circle", "11/16/2021", true);
            InsertPatron(957256454, "Christabella Beardwood", "451-978-5159", "5597 Grim Hill", "11/21/2022", true);
            InsertPatron(957256455, "Bo Eyre", "483-363-2068", "185 Forster Junction", "7/16/2022", false);
            InsertPatron(957256456, "Grantley Sudron", "970-362-5728", "8568 Bayside Circle", "12/16/2022", true);
            InsertPatron(957256457, "Bianca Moisey", "298-682-0520", "38713 Goodland Road", "8/20/2021", false);
            InsertPatron(957256458, "Francoise Habbon", "290-318-4655", "0 Hoard Road", "5/7/2022", true);
            InsertPatron(957256459, "Flo Darrigone", "670-359-6300", "68821 Kim Point", "7/21/2022", false);
            InsertPatron(957256460, "Ellen Rawsthorn", "574-881-8769", "0 Michigan Junction", "6/9/2023", true);
            InsertPatron(957256461, "Holmes MacKereth", "473-799-6808", "5600 Scofield Drive", "12/20/2021", true);
            InsertPatron(957256462, "Drusie Kowal", "331-697-9697", "92565 Forest Dale Circle", "1/30/2023", false);
            InsertPatron(957256463, "Cass Milham", "300-350-4714", "33700 American Junction", "11/18/2023", true);
            InsertPatron(957256464, "Robinson Zavattari", "519-846-6566", "53044 Bartelt Crossing", "4/15/2023", false);
            InsertPatron(957256465, "Charmain Mapis", "254-696-1854", "3 Merry Drive", "7/25/2023", false);
            InsertPatron(957256466, "Reggie Peltz", "570-713-8604", "0199 Dunning Junction", "12/9/2021", true);
            InsertPatron(957256467, "Gael Apedaile", "704-395-7654", "884 Barnett Drive", "11/15/2023", true);
            InsertPatron(957256468, "Onofredo Milesop", "641-193-1070", "51770 Montana Road", "2/27/2023", false);
            InsertPatron(957256469, "Adams Mcettrick", "115-572-7873", "252 Ilene Plaza", "11/6/2023", true);
            InsertPatron(957256470, "Gussy Cassy", "163-628-2325", "023 High Crossing Street", "7/24/2022", false);
            InsertPatron(957256471, "Eleanor Gallamore", "912-356-1061", "59 Anthes Road", "12/3/2021", true);
            InsertPatron(957256472, "Jessie Hursey", "299-589-9555", "19 Milwaukee Crossing", "1/20/2023", true);
            InsertPatron(957256473, "Nathanil Logesdale", "302-326-9055", "26624 Service Trail", "12/1/2021", true);
            InsertPatron(957256474, "Felecia Schonfeld", "117-400-7886", "00247 School Trail", "4/26/2023", true);
            InsertPatron(957256475, "Alexandros Witherup", "741-298-2158", "8 Bashford Street", "6/5/2023", false);
            InsertPatron(957256476, "Babb Questier", "139-329-2594", "3094 Sheridan Pass", "5/29/2022", false);
            InsertPatron(957256477, "Janina Rolance", "799-305-3799", "4 Elmside Court", "7/21/2022", false);
            InsertPatron(957256478, "Erskine Mozzini", "755-551-6412", "99 Schiller Drive", "7/26/2023", false);
            InsertPatron(957256479, "Rick Towse", "229-909-0871", "404 Sutherland Trail", "9/28/2022", true);
            InsertPatron(957256480, "Dolf Bradane", "668-712-5610", "653 Cascade Avenue", "1/19/2022", false);
            InsertPatron(957256481, "Munroe Lewsy", "409-824-3435", "8547 Northridge Alley", "9/29/2023", true);
            InsertPatron(957256482, "Wanids Taggert", "399-589-6382", "1 Warner Pass", "9/15/2022", true);
            InsertPatron(957256483, "Hinze Edridge", "949-497-6342", "72542 Westerfield Trail", "11/6/2022", false);
            InsertPatron(957256484, "Obadiah Kording", "504-596-0002", "749 Mariners Cove Alley", "11/30/2022", true);
            InsertPatron(957256485, "Tallia Canavan", "988-348-0558", "59931 Ridgeway Avenue", "11/10/2023", true);
            InsertPatron(957256486, "Corette Oscroft", "937-137-2535", "36659 Knutson Pass", "3/1/2023", false);
            InsertPatron(957256487, "Addy Grise", "945-874-9101", "1 Anthes Alley", "6/23/2023", true);
            InsertPatron(957256488, "Thedrick Nellen", "798-851-8328", "848 Carpenter Circle", "11/24/2021", false);
            InsertPatron(957256489, "Susie Pellman", "910-687-1140", "19366 Glacier Hill Road", "10/14/2023", false);
            InsertPatron(957256490, "Lorene Emery", "747-346-0962", "4863 Comanche Center", "9/23/2022", true);
            InsertPatron(957256491, "Hermy Tregian", "155-646-6472", "39 Westend Street", "11/17/2021", false);
            InsertPatron(957256492, "Dawn Heinsen", "942-514-9644", "4 Elka Avenue", "5/16/2023", true);
            InsertPatron(957256493, "Binnie Jovanovic", "847-635-6537", "69446 Moland Lane", "9/28/2021", false);
            InsertPatron(957256494, "Olenka O Mara", "914-375-4417", "01573 Moose Crossing", "10/20/2023", true);
            InsertPatron(957256495, "Dana Jeannaud", "868-577-6015", "9 Main Point", "2/13/2023", true);
            InsertPatron(957256496, "Tobi Burth", "468-108-4718", "9610 Monica Street", "9/20/2022", false);
            InsertPatron(957256497, "Ravi Tuckie", "948-108-5754", "261 Badeau Terrace", "4/26/2023", false);
            InsertPatron(957256498, "Giselle Swain", "244-393-7097", "8493 Myrtle Plaza", "3/4/2023", false);
            InsertPatron(957256499, "Mellisa Saunter", "313-535-9511", "13 Monument Hill", "9/6/2022", true);
            InsertPatron(957256500, "Avram Alster", "977-690-1506", "386 8th Junction", "6/16/2023", false);
            InsertPatron(957256501, "Shelly Duthie", "917-143-5474", "413 Coleman Drive", "1/6/2023", false);
            InsertPatron(957256502, "Nalani Eskriett", "640-132-4814", "95 Rutledge Terrace", "1/24/2023", true);
            InsertPatron(957256503, "Ron Ray", "962-687-6311", "6 Nobel Park", "9/13/2021", true);
            InsertPatron(957256504, "Pearline Patington", "342-481-5374", "4 Russell Crossing", "9/12/2023", false);
            InsertPatron(957256505, "Ax Maunton", "424-201-8863", "74050 Rieder Center", "10/12/2022", true);
            InsertPatron(957256506, "Babette Toping", "813-202-9041", "61703 Vernon Crossing", "2/9/2022", true);
            InsertPatron(957256507, "Shelagh Tirone", "497-305-5139", "4 Bashford Junction", "5/4/2022", false);
            InsertPatron(957256508, "Carmella Gath", "401-861-2648", "211 Killdeer Terrace", "5/20/2023", true);
            InsertPatron(957256509, "Lanie Darthe", "741-599-6662", "1973 Holy Cross Parkway", "9/21/2022", true);
            InsertPatron(957256510, "Celie Worthing", "580-947-2767", "11433 Sommers Lane", "10/27/2021", true);
            InsertPatron(957256511, "Myron Chatell", "976-181-8077", "66 Texas Terrace", "10/4/2022", true);
            InsertPatron(957256512, "Constanta Dolden", "760-916-8007", "285 Green Ridge Hill", "9/12/2021", false);
            InsertPatron(957256513, "Irena Eddoes", "770-746-0904", "2006 Briar Crest Trail", "5/20/2023", false);
            InsertPatron(957256514, "Korie Nannetti", "656-446-8391", "01311 Milwaukee Plaza", "12/7/2022", true);
            InsertPatron(957256515, "Harmonia Merington", "408-883-0633", "8352 Ridgeway Street", "7/3/2022", false);
            InsertPatron(957256516, "Marcello Donhardt", "250-491-3002", "142 Lindbergh Crossing", "9/24/2023", false);
            InsertPatron(957256517, "Rosie Le Lievre", "325-151-5573", "9 Golf Course Alley", "2/28/2022", true);
            InsertPatron(957256518, "Eb Troke", "307-910-3242", "1 Londonderry Parkway", "3/8/2023", true);
            InsertPatron(957256519, "Gerianne Dowyer", "185-959-9007", "531 Dapin Pass", "8/11/2021", true);
            InsertPatron(957256520, "Gavra Hillatt", "282-968-9821", "791 Sycamore Court", "7/12/2023", false);
            InsertPatron(957256521, "Morna Whetson", "424-646-3415", "15964 Myrtle Parkway", "1/13/2022", true);
            InsertPatron(957256522, "Kiah Goburn", "372-909-6140", "516 Dawn Trail", "3/16/2022", true);
            InsertPatron(957256523, "Eva Bowry", "833-704-7591", "57513 Hoard Lane", "3/8/2023", true);
            InsertPatron(957256524, "Rhodie Knudsen", "837-645-4251", "8649 Fairfield Avenue", "10/9/2021", true);


        }

        /// <summary>
        /// Its bad
        /// </summary>
        private void DamageControll2()
        {
            InsertBook("Hyatt Ackenhead", 7, "760308804-6", "Peculiarities of the National Fishing (Osobennosti natsionalnoy rybalki)", "4/28/2023", "Greenfelder, White and Gutkowski", 5);
            InsertBook("Tore Kalvin", 10, "925211520-X", "Rumor Has It...", "2/8/2023", "Moen-Hackett", 4);
            InsertBook("Hailee Ridsdell", 9, "803263440-1", "Return to the 36th Chamber (Shao Lin da peng da shi) ", "9/14/2023", "Beahan-Stanton", 3);
            InsertBook("Salomone Kuhnel", 8, "052822301-1", "Seventh Continent, The (Der siebente Kontinent)", "1/8/2023", "Labadie-Haag", 5);
            InsertBook("Ansel Upston", 8, "510507170-0", "Sun on the Horizon", "10/28/2023", "Beer Inc", 4);
            InsertBook("Chester Chelley", 7, "327539208-5", "Midnight Chronicles", "3/30/2023", "Schmitt LLC", 5);
            InsertBook("Richie D orsay", 6, "695192446-5", "Generation, A (Pokolenie)", "6/26/2023", "Towne, Goodwin and Bosco", 0);
            InsertBook("Emmalee Sybbe", 14, "594674641-3", "Lawrence of Arabia", "12/2/2022", "D Amore-Sipes", 4);
            InsertBook("Edsel Teodorski", 7, "416313954-0", "Souper, Le (Supper, The)", "4/23/2023", "Heathcote Group", 2);
            InsertBook("Ortensia Bernardy", 5, "849042906-5", "Grill Point (Halbe Treppe)", "11/11/2023", "Kuhic, Trantow and Hyatt", 2);
            InsertBook("Culver Ravenscroft", 7, "898325252-9", "Spider Baby or, The Maddest Story Ever Told (Spider Baby)", "12/13/2022", "Lebsack-Lind", 2);
            InsertBook("Debi Dollar", 8, "450498501-2", "Pursuit of D.B. Cooper, The (a.k.a. Pursuit)", "12/17/2022", "Toy-Bogisich", 0);
            InsertBook("Daffie Greeding", 16, "905347757-8", "Minnie and Moskowitz", "1/23/2023", "Hills Group", 0);
            InsertBook("Mina Dumper", 19, "928423397-6", "Curious George", "1/8/2023", "Jaskolski-Pagac", 2);
            InsertBook("Joni Faustian", 16, "843461760-9", "You Belong to Me", "12/15/2022", "Olson, Anderson and Volkman", 3);
            InsertBook("Nita Botcherby", 19, "232101335-4", "German Doctor, The (Wakolda)", "4/30/2023", "Vandervort-Thiel", 5);
            InsertBook("Vergil Straun", 1, "896757150-X", "Every Which Way But Loose", "9/10/2023", "Lowe-Schumm", 2);
            InsertBook("Maye Zapater", 9, "948482223-1", "Ten Commandments, The", "9/23/2023", "Johnson LLC", 0);
            InsertBook("Feodora Larret", 8, "018876839-4", "Let the Right One In (Låt den rätte komma in)", "7/28/2023", "Swift Inc", 3);
            InsertBook("Adelaide Sumner", 6, "465050270-5", "Loneliest Planet, The", "10/3/2023", "Schmitt Group", 2);
            InsertBook("Toni Hugo", 2, "954411670-2", "Island of the Burning Damned (Night of the Big Heat)", "3/31/2023", "Murphy-Leffler", 0);
            InsertBook("Marleah Wyrill", 4, "083322886-2", "Rat Race, The (Garson Kanins The Rat Race)", "11/22/2023", "Bartoletti and Sons", 0);
            InsertBook("Fair Shovell", 6, "471105176-7", "Dracula: Prince of Darkness", "5/1/2023", "Powlowski, Kunze and White", 3);
            InsertBook("Della Matyushkin", 18, "404530329-4", "Private Lives of Pippa Lee, The", "5/10/2023", "Barton, Little and Klein", 4);
            InsertBook("Susette Faireclough", 1, "588792625-2", "Waltzes from Vienna", "5/18/2023", "Bosco-Pagac", 2);
            InsertBook("Maye Zapater", 12, "134101221-2", "Sangre de mi sangre (Padre Nuestro)", "7/9/2023", "Purdy-Renner", 3);
            InsertBook("Marleah Wyrill", 1, "133104032-9", "In the Heart of the Sea", "5/24/2023", "Rodriguez-Thiel", 2);
            InsertBook("Rollo Blewis", 2, "775014219-3", "Full Metal Jacket", "9/25/2023", "Stracke-Wuckert", 3);
            InsertBook("Xena Korejs", 14, "458534412-8", "Excuse Me for Living", "10/10/2023", "Fadel Group", 5);
            InsertBook("Salomone Kuhnel", 9, "598240816-6", "In the Blood", "3/19/2023", "Zboncak, Marquardt and Borer", 3);
            InsertBook("Nickolas Reedshaw", 14, "187961502-9", "Anchorman: The Legend of Ron Burgundy", "1/5/2023", "Schoen Group", 5);
            InsertBook("Lucila Byrkmyr", 12, "193611383-X", "Big One, The", "8/2/2023", "Steuber, Hessel and Macejkovic", 2);
            InsertBook("Deck Lukacs", 3, "346956753-0", "Burning Hot Summer, A (Un été brûlant)", "5/10/2023", "Blanda and Sons", 4);
            InsertBook("Terry Bockmaster", 14, "757937345-9", "99 and 44/100% Dead", "10/13/2023", "Schinner-Moore", 2);
            InsertBook("Hallie Tzar", 1, "416615036-7", "Carnival of Souls", "9/15/2023", "Kassulke and Sons", 0);
            InsertBook("Brok Carryer", 10, "916788395-8", "Fugitives (Fugitivas)", "7/15/2023", "Corwin-Davis", 1);
            InsertBook("Duncan Bernardoni", 15, "663878356-4", "Very Bad Things", "2/5/2023", "Hayes-Reichert", 1);
            InsertBook("Devinne Laffin", 6, "869908319-X", "Basara: Princess Goh", "7/25/2023", "Larson, Kub and Ebert", 4);
            InsertBook("Adrienne Kindon", 2, "399659261-2", "Trans", "1/6/2023", "Beahan and Sons", 3);
            InsertBook("Kelsy Loiterton", 0, "558849798-8", "Old Fashioned Way, The", "7/1/2023", "Mills and Sons", 2);
            InsertBook("Rafferty Conley", 3, "063960678-4", "Music in the Air", "8/3/2023", "Baumbach and Sons", 0);
            InsertBook("Essie Warwick", 15, "535109833-4", "Live Nude Girls", "9/1/2023", "Howell and Sons", 0);
            InsertBook("Niles O Dogherty", 19, "560342833-3", "Happy New Year", "10/7/2023", "Paucek-Heller", 5);
            InsertBook("Kit Creus", 4, "226810913-5", "Won Ton Ton: The Dog Who Saved Hollywood", "8/12/2023", "Block, Batz and Bauch", 5);
            InsertBook("Noreen Kingzet", 1, "207961326-X", "Afterglow", "6/1/2023", "Ortiz, Dooley and Kertzmann", 2);
            InsertBook("Roxana Knipe", 6, "472352097-X", "Mark of the Vampire", "7/3/2023", "Pacocha, Beahan and Carroll", 3);
            InsertBook("Shayna Riddiford", 4, "105151901-2", "Death of a Salesman", "2/4/2023", "Weber-Wehner", 2);
            InsertBook("Karel Chung", 17, "519608150-X", "Spriggan (Supurigan)", "7/12/2023", "Weimann Group", 0);
            InsertBook("Odie Garlick", 0, "056442979-1", "Ulzanas Raid", "5/28/2023", "Hills-Dickinson", 5);
            InsertBook("Neilla Beades", 10, "129347183-6", "Teen Wolf Too", "6/4/2023", "Corwin-D Amore", 5);
            InsertBook("Dalston Andrysek", 10, "121595198-1", "Heaven Is for Real", "11/29/2022", "Kuhic-Hyatt", 2);
            InsertBook("Ham Berndsen", 5, "375818277-8", "Upstream Color", "11/30/2022", "Klein-Wolff", 5);
            InsertBook("Gizela Divine", 19, "436918737-0", "Lost Battalion, The", "10/4/2023", "Crona, Wyman and Smitham", 1);
            InsertBook("Dyna Jeggo", 19, "857486322-X", "Rumor of Angels, A", "9/4/2023", "Kris Inc", 2);
            InsertBook("Karel Chung", 13, "742468965-7", "Seed", "10/1/2023", "Kuhn-Borer", 4);
            InsertBook("Rheta Gaitskill", 6, "489854225-5", "Monster-in-Law", "5/21/2023", "Schaefer-Frami", 5);
            InsertBook("Adelaide Sumner", 2, "255645542-X", "Millions", "11/8/2023", "Herzog-Schmeler", 2);
            InsertBook("Graeme Carruth", 11, "246842444-1", "Centre Stage: Turn It Up", "6/9/2023", "Sipes LLC", 5);
            InsertBook("Ange Swafield", 8, "548067821-3", "Card Subject To Change", "4/15/2023", "Lind-Auer", 5);
            InsertBook("Joyan Alders", 18, "706197084-7", "Boulevard", "5/12/2023", "Schamberger-Nikolaus", 0);
            InsertBook("Jeane Barneville", 8, "941797135-2", "Happythankyoumoreplease", "9/4/2023", "Robel-Hodkiewicz", 4);
            InsertBook("Idelle Rummer", 18, "303461638-4", "My Boy", "3/30/2023", "Leuschke, Wehner and Schaefer", 4);
            InsertBook("Antonino Gerraty", 2, "328402441-7", "Patriots, The (Patriotes, Les)", "2/12/2023", "Nitzsche-Blick", 3);
            InsertBook("Adelind Veldman", 3, "575559721-9", "I Was Born, But... (a.k.a. Children of Tokyo) (Otona no miru ehon - Umarete wa mita keredo)", "4/27/2023", "Halvorson-Mueller", 4);
            InsertBook("Chrisy Macellar", 11, "415254009-5", "Paris Is Burning", "3/2/2023", "Roob Group", 1);
            InsertBook("Gauthier Hounsome", 16, "253226767-4", "Trials of Darryl Hunt, The", "7/9/2023", "Oberbrunner, Prosacco and Jakubowski", 1);
            InsertBook("Alexandr Byrom", 7, "745189007-4", "Behind the Candelabra", "12/21/2022", "Terry Group", 1);
            InsertBook("Englebert Rice", 2, "069860730-9", "Going My Way", "11/3/2023", "Hayes-Jones", 0);
            InsertBook("Sheena Becken", 13, "406775424-8", "Sting, The", "1/7/2023", "Daniel-Kirlin", 0);
            InsertBook("Raymond Pennoni", 11, "779219773-5", "13", "10/12/2023", "Haley, Purdy and Sporer", 2);
            InsertBook("Ginelle Fussey", 5, "889126113-0", "Beautiful People", "7/18/2023", "Lemke, Collins and Quigley", 4);
            InsertBook("Andreana Franzolini", 14, "173733205-1", "Rawhead Rex", "4/21/2023", "O Kon LLC", 1);
            InsertBook("Payton Batchelour", 3, "344958467-7", "Monica Z", "3/29/2023", "Gottlieb-D Amore", 2);
            InsertBook("Kenna Grinsted", 6, "362582994-0", "Legend of Lizzie Borden, The", "11/9/2023", "Gislason-Haag", 1);
            InsertBook("Reid Schneider", 6, "698970491-8", "Gate, The", "8/15/2023", "Mann-Hoppe", 4);
            InsertBook("Derick Roggeman", 12, "448081964-9", "Red Corner", "8/7/2023", "Trantow-Zboncak", 2);
            InsertBook("Lamond Klaas", 1, "124522028-4", "I Love You Too (Ik ook Van Jou)", "11/10/2023", "Kulas and Sons", 2);
            InsertBook("Lorant Bailles", 10, "891817294-X", "Black Snake Moan", "5/7/2023", "Wiza, O Connell and Kuhic", 2);
            InsertBook("Jdavie McLinden", 8, "685597179-2", "Horse in the Gray Flannel Suit, The", "5/23/2023", "Stark Inc", 3);
            InsertBook("Luci Pridie", 20, "942161462-3", "Open Season", "6/30/2023", "Block-Runte", 3);
            InsertBook("Yvon Pendry", 19, "638706343-4", "Sabrina", "8/18/2023", "Wilderman, Yundt and McKenzie", 1);
            InsertBook("Kit Creus", 8, "725075570-6", "Love Exposure (Ai No Mukidashi)", "1/9/2023", "Walter LLC", 3);
            InsertBook("Emelina Epsly", 8, "573774111-7", "Terror Train", "8/6/2023", "Boehm, Haley and Brekke", 4);
            InsertBook("Jermaine Corwood", 16, "244429575-7", "Nightmare Castle (Amanti d oltretomba) (Lovers from Beyond the Tomb) (Faceless Monster, The)", "11/30/2022", "Bartell Group", 1);
            InsertBook("Xena Korejs", 2, "122037263-3", "Older Brother, Younger Sister (Ani imôto)", "11/12/2023", "Beatty-Leffler", 2);
            InsertBook("Brew Leversha", 14, "888484396-0", "Cartoon All-Stars to the Rescue", "8/28/2023", "Schinner, Green and Daniel", 4);
            InsertBook("Shurlocke Cluney", 10, "300001806-9", "Rambo: First Blood Part II", "9/12/2023", "Muller, O Reilly and Ritchie", 4);
            InsertBook("Glynn Lucia", 8, "627749893-2", "Commandments", "10/13/2023", "Hoeger and Sons", 0);
            InsertBook("Luci Pridie", 10, "944816916-7", "Battle of China, The (Why We Fight, 6)", "9/27/2023", "Green, Heidenreich and Renner", 0);
            InsertBook("Carver Dearlove", 0, "276865877-4", "Charlie St. Cloud", "5/13/2023", "Keeling-Nienow", 2);
            InsertBook("Kim Crunkhurn", 11, "040454546-7", "Bonheur, Le", "7/6/2023", "Ward-Stoltenberg", 2);
            InsertBook("Adel Iwanowski", 15, "283081002-3", "Little Nicholas (Le petit Nicolas)", "11/10/2023", "Funk-Hyatt", 5);
            InsertBook("Riley Carlaw", 13, "571576412-2", "Broadcast News", "1/18/2023", "Hickle and Sons", 0);
            InsertBook("Creigh McCard", 20, "930444986-3", "Con, The", "10/14/2023", "Kutch, Funk and Sawayn", 1);
            InsertBook("Sheena Matthias", 4, "891606564-X", "Slap Shot", "4/26/2023", "Satterfield, Moen and Brown", 4);
            InsertBook("Brnaby Simonetto", 5, "026224292-3", "Miracle in Milan (Miracolo a Milano)", "11/3/2023", "Daugherty-Mayer", 5);
            InsertBook("Demetri Matusson", 14, "799220758-0", "It Happened Here", "4/6/2023", "Reilly LLC", 3);
            InsertBook("Essie Warwick", 4, "326243528-7", "Lions For Lambs", "10/14/2023", "Cronin, Armstrong and Hackett", 5);
            InsertBook("Chase Gumery", 18, "503176150-4", "Believe Me", "6/17/2023", "Brown, Larson and Bogisich", 0);
            InsertBook("Myrle McPherson", 7, "911314778-1", "Elsewhere", "7/27/2023", "DuBuque-Wolff", 2);
            InsertBook("Gunter Gammett", 17, "657436895-2", "Advance to the Rear", "4/27/2023", "Mayert, Hand and Gibson", 3);
            InsertBook("Abelard Mellers", 17, "403459478-0", "Ironclad", "7/12/2023", "Schiller-Kuhlman", 3);
            InsertBook("Andreana Franzolini", 7, "099277977-4", "Hierro ", "8/20/2023", "Kling-Sporer", 2);
            InsertBook("Vladimir Kiezler", 13, "674823528-6", "Story of the Weeping Camel, The (Geschichte vom weinenden Kamel, Die)", "3/3/2023", "White, Zulauf and Hermiston", 4);
            InsertBook("Durand Quimby", 12, "091842489-5", "French Fried Vacation 2 (Les bronzés font du ski)", "2/23/2023", "Connelly and Sons", 1);
            InsertBook("Selle Worsom", 5, "492570934-8", "Elite Squad (Tropa de Elite)", "10/22/2023", "Hand-Durgan", 4);
            InsertBook("Carmon Fee", 14, "112874942-4", "Accattone", "4/27/2023", "Rice, Hettinger and Becker", 5);
            InsertBook("Terza Lightbown", 4, "471111179-4", "41-Year-Old Virgin Who Knocked Up Sarah Marshall and Felt Superbad About It, The", "4/26/2023", "Fritsch-Swaniawski", 0);
            InsertBook("Rinaldo Poxton", 20, "297450179-6", "Carry on Cruising", "1/15/2023", "Schmeler-Becker", 3);
            InsertBook("Sheena Becken", 15, "298183170-4", "Wall Street: Money Never Sleeps", "9/22/2023", "MacGyver, Goyette and Powlowski", 2);
            InsertBook("Lionello Pharro", 6, "213811963-7", "Scotland, Pa.", "7/3/2023", "Paucek Group", 2);
            InsertBook("Anestassia MacRannell", 19, "679994754-2", "Situation, The", "9/26/2023", "Hackett-Mraz", 3);
            InsertBook("Desi Sapauton", 18, "818379501-3", "Human Resources Manager, The", "10/27/2023", "Hackett, Grant and Okuneva", 0);
            InsertBook("Guss Jenk", 3, "329807513-2", "Sh! The Octopus", "10/24/2023", "Swaniawski, Romaguera and Mraz", 1);
            InsertBook("Cobby Palister", 16, "923360558-2", "Go West", "6/18/2023", "Koch Inc", 3);
            InsertBook("Niki Gregorowicz", 17, "964982531-2", "Moving", "9/1/2023", "Casper Group", 4);
            InsertBook("Chase Gumery", 10, "307981943-8", "Prison (Fängelse) ", "11/21/2023", "Cronin, Considine and Brown", 2);
            InsertBook("Tilly Colling", 8, "605548587-7", "One Shot", "1/6/2023", "Turner-Beatty", 1);
            InsertBook("Iris Ghilardi", 4, "243640274-4", "Adventures in Babysitting", "3/23/2023", "Rolfson-Wintheiser", 4);
            InsertBook("Timmi Dadson", 16, "105827498-8", "Back-up Plan, The", "11/14/2023", "Keebler, Feest and Abernathy", 2);
            InsertBook("Alard Gaitskell", 13, "881382887-X", "In the Army Now", "9/30/2023", "White-Bartoletti", 4);
            InsertBook("Michel Trudgion", 4, "341490664-3", "Too Many Husbands", "2/26/2023", "Swift Group", 2);
            InsertBook("Merna Halling", 0, "214036880-0", "And Then Came Lola", "1/30/2023", "Hegmann, Gibson and Littel", 5);
            InsertBook("Ortensia Bernardy", 8, "871568123-8", "El Dorado", "6/8/2023", "Schimmel-Cruickshank", 1);
            InsertBook("Skye Zambonini", 2, "208226368-1", "Beast from 20,000 Fathoms, The", "11/9/2023", "Walsh LLC", 4);
            InsertBook("Wilie Olczak", 9, "105686494-X", "Holy Matrimony", "9/6/2023", "Paucek-Dietrich", 3);
            InsertBook("Yvon Pendry", 15, "382618374-6", "My Best Enemy (Mi mejor enemigo)", "9/9/2023", "Borer, VonRueden and Langworth", 2);
            InsertBook("Laurie O Henehan", 8, "929361696-3", "Breaking Upwards", "12/5/2022", "Bashirian-Marks", 5);
            InsertBook("Cobby Palister", 8, "456870123-6", "Patriot, The", "12/22/2022", "Bashirian-Smitham", 1);
            InsertBook("Reid Schneider", 2, "889858915-8", "Faces of Death 4", "10/27/2023", "Orn-Robel", 2);
            InsertBook("Yolande Kitchenman", 12, "571322872-X", "Lodger: A Story of the London Fog, The", "10/14/2023", "Dare Inc", 5);
            InsertBook("Hildagarde Cadreman", 6, "252321610-8", "Beetle Queen Conquers Tokyo", "5/30/2023", "Gislason and Sons", 2);
            InsertBook("Fax Ceaser", 8, "579505110-1", "Fly Me to the Moon", "9/5/2023", "Ryan and Sons", 3);
            InsertBook("Tilly Colling", 12, "652552768-6", "From Within", "9/3/2023", "Hand, Pollich and Moore", 1);
            InsertBook("Nikolaus Preedy", 11, "174440705-3", "From Dusk Till Dawn", "9/3/2023", "Weimann-Rosenbaum", 4);
            InsertBook("Indira Stanway", 1, "897630394-6", "Notorious", "4/29/2023", "Runte, Ankunding and Shanahan", 4);
            InsertBook("Mabel Peniman", 9, "072217785-2", "Cinderella Man", "8/21/2023", "Heller Group", 5);
            InsertBook("Chase Gumery", 2, "444622285-6", "Lone Wolf and Cub: Baby Cart at the River Styx (Kozure Ôkami: Sanzu no kawa no ubaguruma)", "4/19/2023", "Kuhlman LLC", 1);
            InsertBook("Darill Ebbins", 8, "128858802-X", "Dont Eat the Pictures: Sesame Street at the Metropolitan Museum of Art", "4/16/2023", "Macejkovic-Emard", 3);
            InsertBook("Syd Pavlov", 18, "418454780-X", "Cosmic Journey", "3/9/2023", "Simonis-Macejkovic", 5);
            InsertBook("Seline Tuberfield", 1, "464832559-1", "Imago mortis", "4/12/2023", "Fisher LLC", 1);
            InsertBook("Kane Breeze", 4, "390056248-2", "An Empress and the Warriors", "12/18/2022", "Kris, Dietrich and Dibbert", 1);
            InsertBook("Mara Shuker", 19, "296315542-5", "Secret Agent", "5/12/2023", "Botsford, Satterfield and Huel", 4);
            InsertBook("Mina Dumper", 15, "742092962-9", "Genova (Summer in Genoa, A)", "2/1/2023", "Morissette, Rowe and Abshire", 4);
            InsertBook("Devinne Laffin", 19, "858653511-7", "Junk Mail (Budbringeren)", "4/25/2023", "Reilly, Spinka and Dach", 2);
            InsertBook("Sherie Sendall", 8, "457307195-4", "Glowing Stars", "10/24/2023", "Quitzon Group", 4);
            InsertBook("Carita Hardinge", 8, "452392726-X", "Pure Country", "1/27/2023", "Kunde-Boyer", 3);
            InsertBook("Mary Stapels", 12, "108952244-4", "Shanghai", "12/5/2022", "Vandervort, West and Kautzer", 1);
            InsertBook("Fax Ceaser", 19, "257504055-8", "La montaña rusa", "3/17/2023", "D Amore, Barrows and Jerde", 2);
            InsertBook("Dyna Jeggo", 17, "040203360-4", "Dark Alibi", "9/2/2023", "Bechtelar, Nader and Sporer", 3);
            InsertBook("Fax Ceaser", 14, "605624846-1", "Saimaa Gesture, The (Saimaa-ilmiö)", "10/15/2023", "Hickle Inc", 3);
            InsertBook("Linda Brommage", 9, "470718074-4", "Go-Between, The", "7/4/2023", "Koelpin and Sons", 1);
            InsertBook("Shepperd Trappe", 18, "856051625-5", "Flintstones in Viva Rock Vegas, The", "4/5/2023", "Gutmann Inc", 1);
            InsertBook("Gian Dixsee", 1, "690958780-4", "Grotesque (Gurotesuku)", "11/22/2023", "Glover-Pfannerstill", 0);
            InsertBook("Ardine Yearnes", 20, "027654203-7", "Gamera vs. Barugon", "9/15/2023", "Romaguera-Prosacco", 1);
            InsertBook("Jeno Grier", 19, "236060118-0", "Tom Sawyer", "1/15/2023", "Boyle Group", 4);
            InsertBook("Sheena Matthias", 7, "301696183-0", "Tainted", "1/29/2023", "Anderson, Steuber and Zieme", 4);
            InsertBook("Derick Roggeman", 11, "362858545-7", "Gypsy 83", "8/31/2023", "Graham, Connelly and Friesen", 3);
            InsertBook("Aloisia Kentish", 20, "304903399-1", "Trial, The (Procès, Le)", "1/28/2023", "Hand Group", 5);
            InsertBook("Una Cato", 3, "954992620-6", "Mirrors 2", "3/22/2023", "Bergnaum Inc", 1);
            InsertBook("Frederic Glanz", 12, "272876560-X", "Secret of NIMH, The", "3/1/2023", "Kunde-Pouros", 5);
            InsertBook("Adella Graal", 12, "633908634-9", "Black Coffee", "9/6/2023", "Spencer-Schiller", 3);
            InsertBook("Stu Caccavella", 0, "454074355-4", "Victory (a.k.a. Escape to Victory)", "3/31/2023", "Lueilwitz, Champlin and Kling", 3);
            InsertBook("Farrah O Halleghane", 6, "933357522-7", "White God (Fehér isten)", "8/16/2023", "Dibbert, Cole and Lueilwitz", 3);
            InsertBook("Erie Hubbucke", 20, "893169282-X", "India Song", "8/29/2023", "Ritchie-Denesik", 5);
            InsertBook("Matty Wile", 11, "231446194-0", "De-Lovely", "9/2/2023", "Hintz Group", 2);
            InsertBook("Ansel Upston", 18, "956713536-3", "Affair of the Heart, An", "3/17/2023", "Friesen-Douglas", 5);
            InsertBook("Dwight Solon", 2, "581818664-4", "Grass: A Nations Battle for Life", "8/25/2023", "Kuvalis, Pagac and Dare", 5);
            InsertBook("Darill Ebbins", 7, "056372047-6", "SOS - en segelsällskapsresa", "10/26/2023", "Terry and Sons", 4);
            InsertBook("Adel Iwanowski", 10, "894877700-9", "The Challenge", "9/8/2023", "Ullrich-Oberbrunner", 2);
            InsertBook("Milicent Doll", 15, "088264397-5", "Strongest Man in the World, The", "11/21/2023", "Legros Group", 2);
            InsertBook("Nealon Tayloe", 1, "336356535-6", "Bio Zombie (Sun faa sau si)", "8/3/2023", "Runolfsson and Sons", 5);
            InsertBook("Hermine Keslake", 2, "640898145-3", "You and Me (Ty i ya)", "10/23/2023", "Wuckert-Jenkins", 3);
            InsertBook("Faye Gooke", 16, "158625424-3", "Mentor", "11/18/2023", "Runolfsson-Witting", 4);
            InsertBook("Devinne Laffin", 11, "340657835-7", "Django the Bastard (Strangers Gundown, The) (Django il bastardo)", "7/8/2023", "Braun-Effertz", 2);
            InsertBook("Meghan O Hoey", 9, "912974303-6", "Colin Quinn: Long Story Short", "5/10/2023", "Boehm-Block", 4);
            InsertBook("Odie Garlick", 11, "450642692-4", "Junior Prom", "9/9/2023", "Quigley-Green", 2);
            InsertBook("Eleanora Dymoke", 0, "712601304-3", "Wonderful, Horrible Life of Leni Riefenstahl, The (Macht der Bilder: Leni Riefenstahl, Die)", "10/30/2023", "Osinski-Schowalter", 5);
            InsertBook("Valery MacCarrane", 15, "413761798-8", "Blood and Black Lace (Sei donne per l assassino)", "10/14/2023", "Skiles, Johns and Tromp", 1);
            InsertBook("Adolphe Rolinson", 20, "274137248-9", "Goldfish Memory", "8/9/2023", "Weber and Sons", 3);
            InsertBook("Duncan Bernardoni", 17, "310326350-3", "Maria, ihm schmeckt s nicht! (Maria, He Doesnt Like It)", "4/26/2023", "Schmidt, Feeney and Nikolaus", 1);
            InsertBook("Latia Wrightim", 9, "073482924-8", "Son of the White Mare", "7/8/2023", "Dickinson Inc", 1);
            InsertBook("Alvinia Iskowitz", 5, "180819648-1", "Mating Game, The", "7/2/2023", "Bradtke, Beer and Hills", 2);
            InsertBook("Gertrude Haton", 11, "088307434-6", "Shed No Tears (Känn ingen sorg)", "2/11/2023", "Gulgowski, O Reilly and Kovacek", 4);
            InsertBook("Carroll Killiam", 11, "320599719-0", "Jack and Jill", "11/6/2023", "Wiegand, Beatty and Jerde", 3);
            InsertBook("Somerset Storch", 8, "429457994-3", "Dont Make Waves", "12/31/2022", "Effertz and Sons", 5);
            InsertBook("Xaviera Walklate", 9, "629200823-1", "Death of a Nation - The Timor Conspiracy", "1/22/2023", "Cruickshank Group", 0);
            InsertBook("Corella Breddy", 19, "552226492-6", "Masked & Anonymous", "7/5/2023", "Conroy-Dickens", 1);
            InsertBook("Elladine Wheatcroft", 12, "524727478-4", "Nines, The", "9/27/2023", "Graham Group", 3);
            InsertBook("Jdavie McLinden", 7, "976934774-4", "Moderns, The", "5/20/2023", "Hoeger, Hintz and Beier", 1);
            InsertBook("Edd Meah", 11, "571841580-3", "Amen", "6/9/2023", "Runolfsdottir-Goyette", 0);
            InsertBook("Sherie Sendall", 5, "887562898-X", "Good Mother, The", "11/15/2023", "Macejkovic, Koelpin and King", 3);
            InsertBook("Berta Trorey", 18, "439745308-X", "Love, Marilyn", "9/16/2023", "Johnston, Flatley and Rodriguez", 2);
            InsertBook("Yvon Pendry", 12, "863241170-5", "Good Old Fashioned Orgy, A", "10/9/2023", "O Connell-Schultz", 2);
            InsertBook("Rinaldo Poxton", 19, "464630136-9", "Highway ", "12/18/2022", "VonRueden, Olson and Will", 3);
            InsertBook("Doralynn Wellbeloved", 17, "446991436-3", "Harry Potter and the Chamber of Secrets", "11/10/2023", "Leannon Group", 3);
            InsertBook("Marni McKimmie", 12, "679889316-3", "Promoter, The (Card, The)", "11/14/2023", "Johns, Hilpert and Brekke", 2);
            InsertBook("Vanna Scotchmore", 5, "625254368-3", "Shrek", "2/17/2023", "Walter-Spinka", 4);
            InsertBook("Jocelyn Matessian", 19, "623180261-2", "Wreckers", "8/16/2023", "D Amore-O Keefe", 5);
            InsertBook("Amie Cobbled", 11, "277544688-4", "Kin-Dza-Dza!", "6/10/2023", "Bayer-Cassin", 5);
            InsertBook("Chase Gumery", 10, "410683389-1", "Traitor", "5/30/2023", "DuBuque Inc", 5);
            InsertBook("Hermine Keslake", 9, "082903110-3", "Mayday at 40,000 Feet!", "8/21/2023", "Smith, Weimann and Auer", 0);
            InsertBook("Roxana Knipe", 19, "495584963-6", "Beethovens 5th", "10/4/2023", "Hansen, Okuneva and Heaney", 0);
            InsertBook("Della Matyushkin", 9, "936836254-8", "7 Seconds", "9/21/2023", "Miller-Dooley", 0);
            InsertBook("Stu Caccavella", 6, "402605294-X", "Caesar Must Die (Cesare deve morire)", "9/3/2023", "Muller Group", 4);
            InsertBook("Maure Robillart", 12, "097348495-0", "Mac and Me", "3/23/2023", "Windler-Schiller", 2);
            InsertBook("Jeannette Mulvany", 1, "199366431-9", "Chouga (Shuga)", "8/31/2023", "Huel-Bogisich", 5);
            InsertBook("Rikki Jalland", 17, "590831797-7", "Lake, A (Un lac)", "9/22/2023", "Bartoletti Inc", 5);
            InsertBook("Marshall Matveichev", 3, "140574208-9", "Final Conflict, The (a.k.a. Omen III: The Final Conflict)", "11/19/2023", "Legros, O Conner and Moen", 4);
            InsertBook("Annamaria Gorsse", 20, "380168870-4", "Sand Pebbles, The", "10/3/2023", "Auer, Walter and Ortiz", 5);
            InsertBook("Jeannette Mulvany", 5, "445469371-4", "Sukiyaki Western Django", "4/6/2023", "Raynor-Dickens", 0);
            InsertBook("Ammamaria Fanton", 17, "578229839-1", "Interrupters, The", "11/16/2023", "Yost-Will", 2);
            InsertBook("Vita Davidou", 19, "295736300-3", "Free Radicals:  A History of Experimental Film", "2/15/2023", "Balistreri Group", 2);
            InsertBook("Wadsworth Franke", 9, "487291901-7", "Riviera", "11/26/2023", "Raynor, Dietrich and Skiles", 0);
            InsertBook("Richie Dorsay", 15, "873486287-0", "Yes Or No", "2/15/2023", "Rowe-Kemmer", 0);
            InsertBook("Miltie Sutherley", 3, "874577376-9", "Rebel Without a Cause", "6/11/2023", "Prohaska, Waters and Rosenbaum", 3);
            InsertBook("Yolanthe Corse", 0, "770875216-7", "Red State", "11/27/2023", "Gottlieb and Sons", 3);
            InsertBook("Jermaine Corwood", 11, "710486827-5", "Nobody Knows Anything!", "7/7/2023", "Crona-Rippin", 4);
            InsertBook("Euphemia McNeill", 19, "218255469-6", "Wszystko, co kocham", "3/1/2023", "Hand-Cartwright", 3);
            InsertBook("Adolphe Rolinson", 3, "666678819-2", "Hail Columbia!", "8/13/2023", "Kub-Hackett", 0);
            InsertBook("Nickolas Reedshaw", 1, "293096996-2", "Only You", "9/22/2023", "Huel Group", 0);
            InsertBook("Annamaria Gorsse", 16, "113243822-5", "Angel", "4/2/2023", "Skiles-Boyle", 1);
            InsertBook("Ottilie Harral", 15, "993274844-7", "D.L. Hughley: Reset", "6/6/2023", "Mayer-Schiller", 1);
            InsertBook("Ginelle Fussey", 5, "046415243-7", "Assault, The (Aanslag, De)", "6/4/2023", "DuBuque, Rice and Auer", 4);
            InsertBook("Desirae Willshere", 19, "021419891-X", "Vanya on 42nd Street", "2/9/2023", "Hermann, Marvin and Runolfsson", 1);
            InsertBook("Maye Zapater", 6, "820191088-5", "Kiki", "4/12/2023", "Bins-Botsford", 0);
            InsertBook("Lib Krysztowczyk", 10, "755056890-1", "Night on the Galactic Railroad (Ginga-tetsudo no yoru)", "2/15/2023", "Herzog and Sons", 0);
            InsertBook("Jobye Giacomozzo", 3, "543984748-0", "Brown of Harvard", "6/22/2023", "Muller-Jerde", 2);
            InsertBook("Jamil Bearcroft", 17, "152514474-X", "Sweeney, The", "7/28/2023", "Baumbach-Cruickshank", 2);
            InsertBook("Rafferty Conley", 10, "533298718-8", "Club Fed", "7/16/2023", "Quigley-Friesen", 0);
            InsertBook("Ernestine Bourdas", 2, "778359510-3", "I Am Cuba (Soy Cuba/Ya Kuba)", "6/28/2023", "Jaskolski LLC", 3);
            InsertBook("Ortensia Bernardy", 0, "902402917-1", "Project Wild Thing", "12/11/2022", "Casper-Huels", 2);
            InsertBook("Oriana Demaid", 11, "321976631-5", "The Big Flame", "8/24/2023", "Waelchi LLC", 0);
            InsertBook("Edie Pontefract", 6, "705404007-4", "Cat People", "1/26/2023", "Swift-Schmidt", 5);
            InsertBook("Nilson Scoffins", 16, "263192638-2", "Rendition", "12/3/2022", "Zieme LLC", 0);
            InsertBook("Pattin Baskeyfield", 1, "874533820-5", "Rush", "7/26/2023", "Wuckert-Howell", 1);
            InsertBook("Antonino Gerraty", 1, "868325491-7", "Let the Fire Burn", "7/15/2023", "Reichert, Powlowski and Quitzon", 5);
            InsertBook("Gillan Aujouanet", 8, "193536338-7", "Treasure Island", "4/23/2023", "Johns-Zemlak", 2);
            InsertBook("Neila Farnell", 0, "030621826-7", "Tungsten", "3/31/2023", "Kuvalis-Denesik", 5);
            InsertBook("Willis McSorley", 19, "098483124-X", "Adventures of Picasso, The (Picassos äventyr)", "1/19/2023", "Bins Inc", 3);
            InsertBook("Chester Chelley", 20, "414421903-8", "The Amazing Spider-Man 2", "5/23/2023", "Mitchell-Schroeder", 4);
            InsertBook("Wadsworth Franke", 11, "682358983-3", "3 Women (Three Women)", "3/5/2023", "Herman-Fahey", 2);
            InsertBook("Marleah Wyrill", 13, "676038719-8", "HazMat", "7/2/2023", "Greenfelder, Rodriguez and Rau", 1);
            InsertBook("Lamond Klaas", 10, "320279066-8", "All Quiet on the Western Front", "8/20/2023", "Schimmel, Keeling and McKenzie", 3);
            InsertBook("Samuel Grinnov", 12, "682888682-8", "Takva: A Mans Fear of God", "6/9/2023", "Douglas LLC", 5);
            InsertBook("Kurt Cisec", 19, "452520315-3", "Carolina", "9/18/2023", "Green, Bashirian and Bins", 5);
            InsertBook("Miltie Sutherley", 7, "822662828-8", "Passage to Marseille", "4/28/2023", "Okuneva Group", 0);
            InsertBook("Buckie Veneur", 6, "750880000-1", "Dead Birds", "7/14/2023", "Emard, Auer and Mitchell", 4);
            InsertBook("Vikky Pigram", 10, "716565807-6", "From the Earth to the Moon", "3/25/2023", "Nikolaus Group", 0);
            InsertBook("Skye Zambonini", 1, "794151214-9", "Inheritance, The (Karami-ai)", "9/17/2023", "Lowe, Murray and Murphy", 4);
            InsertBook("Cobby Barradell", 6, "288774124-1", "Michael", "1/31/2023", "Medhurst Inc", 3);
            InsertBook("Hildagarde Cadreman", 16, "230357245-2", "Sex and the City 2", "2/25/2023", "Cummings Inc", 4);
            InsertBook("Valma D Emanuele", 14, "241507550-7", "California Split", "8/29/2023", "Stehr Group", 2);
            InsertBook("Shurlocke Cluney", 8, "892442099-2", "Three Musketeers, The", "7/14/2023", "Mosciski, Nicolas and Ward", 1);
            InsertBook("Oriana Demaid", 3, "457705711-5", "Tale of Winter, A (a.k.a. A Winters Tale) (Conte d hiver)", "1/8/2023", "Schmeler Inc", 0);
            InsertBook("Dora Chat", 14, "364668349-X", "Midnight Chronicles", "9/2/2023", "Kuphal-Kub", 4);
            InsertBook("Indira Stanway", 3, "270631843-0", "Wonderful Crook, The (Pas si méchant que ça)", "2/17/2023", "Schmitt-Cronin", 1);
            InsertBook("Ami Cussins", 4, "504747372-4", "Suriyothai (a.k.a. Legend of Suriyothai, The)", "4/23/2023", "Jakubowski-Torphy", 4);
            InsertBook("Terry Bockmaster", 15, "841121359-5", "Halloween 5: The Revenge of Michael Myers", "1/13/2023", "Nikolaus-Hammes", 4);
            InsertBook("Maxwell Whewill", 1, "739686119-5", "La Bande du drugstore", "12/25/2022", "Klocko Inc", 5);
            InsertBook("Skye Zambonini", 16, "231540092-9", "If I Were You", "10/2/2023", "Huels, Yost and Williamson", 0);
            InsertBook("Essie Warwick", 16, "919235910-8", "*batteries not included", "3/31/2023", "Koch LLC", 5);
            InsertBook("Ortensia Bernardy", 12, "177421778-3", "Mr. Motos Gamble", "1/1/2023", "Satterfield-Abshire", 3);
            InsertBook("Rozalin Farrant", 4, "941379566-5", "Operation Ganymed", "12/29/2022", "Mertz-Stiedemann", 5);
            InsertBook("Laurella Rudeyeard", 0, "427826934-X", "Long Way Down, A", "11/15/2023", "Mueller Group", 3);
            InsertBook("Erda Sneezem", 17, "668314968-2", "Seitsemän veljestä", "5/15/2023", "Ernser, Prosacco and Weimann", 1);
            InsertBook("Doralynn Wellbeloved", 17, "618253308-0", "Used Cars", "5/10/2023", "Rath, Hamill and Rohan", 4);
            InsertBook("Bax Dignan", 2, "958104239-3", "Leif", "1/12/2023", "Leuschke, Rohan and Denesik", 1);
            InsertBook("Brenda Queree", 17, "444958867-3", "Rock, The", "10/29/2023", "Sauer, Goyette and Jacobson", 5);
            InsertBook("Brok Carryer", 19, "735724959-X", "Phantom of the Opera, The", "6/7/2023", "Christiansen, Walter and Koch", 2);
            InsertBook("Toni Hugo", 0, "652515854-0", "Our Folks (Sami swoi)", "5/21/2023", "Nolan and Sons", 3);
            InsertBook("Lauretta Shuttell", 13, "396146272-0", "Bells from the Deep", "9/29/2023", "Walsh Group", 5);
            InsertBook("Elane Tomsett", 7, "474701876-X", "1990: The Bronx Warriors (1990: I guerrieri del Bronx)", "1/12/2023", "Beahan-Treutel", 3);
            InsertBook("Rodi Thews", 9, "207560521-1", "Blue Gardenia, The", "6/17/2023", "Rath, Keebler and Sawayn", 3);
            InsertBook("Noah Cowsby", 1, "010111660-8", "Clouds of Sils Maria", "9/14/2023", "Gerhold-Jacobson", 3);
            InsertBook("Cobby Barradell", 9, "428920708-1", "Appleseed (Appurushîdo)", "11/28/2022", "Kuhlman-Schowalter", 4);
            InsertBook("Gayla Fermer", 15, "531160105-1", "My Science Project", "8/21/2023", "Mayert-Hessel", 3);
            InsertBook("Rafferty Conley", 14, "646179172-8", "Bitter Creek", "9/14/2023", "Becker-Osinski", 4);
            InsertBook("Rufus Tramel", 8, "521393863-9", "God Is the Bigger Elvis", "8/28/2023", "Parisian, Mitchell and Blanda", 5);
            InsertBook("Carlos Grigor", 6, "845440896-7", "I Love You Too (Ik ook Van Jou)", "4/12/2023", "Dickinson, Spencer and Stark", 5);
            InsertBook("Toni Hugo", 16, "853538879-6", "Dream Machine", "9/24/2023", "Treutel, Kihn and Rosenbaum", 5);
            InsertBook("Fredelia Hayman", 8, "015815103-8", "Confidence", "10/3/2023", "Cronin, Davis and Berge", 5);
            InsertBook("Zaccaria Carlton", 5, "092722010-5", "Bachelor Weekend, The", "3/26/2023", "Kub-McCullough", 4);
            InsertBook("Lynea Terrington", 20, "590294989-0", "Friends at the Margherita Cafe, The (Gli amici del bar Margherita)", "3/27/2023", "Jerde and Sons", 0);
            InsertBook("Gavrielle Lamberton", 3, "942420216-4", "Year One", "2/25/2023", "Konopelski-Rohan", 2);
            InsertBook("Mae Emmens", 13, "790740318-9", "Sanjuro (Tsubaki Sanjûrô)", "8/25/2023", "Langworth-McLaughlin", 2);
            InsertBook("Shayna Riddiford", 9, "811794571-1", "Forbidden Zone", "2/1/2023", "O Conner-Rath", 2);
            InsertBook("Adel Iwanowski", 6, "107726230-2", "Face of a Fugitive", "12/16/2022", "Buckridge-Gulgowski", 2);
            InsertBook("Gian Dixsee", 11, "699152601-0", "Other Voices, Other Rooms", "7/4/2023", "Steuber, Gleason and Kuhlman", 0);
            InsertBook("Adolphe Rolinson", 10, "080479109-0", "Teen Spirit", "2/2/2023", "Fisher-Gorczany", 2);
            InsertBook("Noami Marioneau", 20, "615875940-6", "Isnt She Great?", "4/21/2023", "Ortiz and Sons", 2);
            InsertBook("Abbi Honatsch", 5, "159853229-4", "Design for Living", "8/29/2023", "Christiansen LLC", 3);
            InsertBook("Cristiano McCole", 15, "031052183-1", "Funny About Love", "6/26/2023", "O Kon-Frami", 4);
            InsertBook("Trista Izkovitz", 6, "919100096-3", "Two Days in April", "10/20/2023", "Farrell, Von and Wunsch", 2);
            InsertBook("Rikki Jalland", 17, "970894357-6", "Horse Rebellion, The (Pulakapina)", "5/1/2023", "Larkin, Marquardt and Keebler", 3);
            InsertBook("Sholom Blindt", 3, "517748939-6", "The Orkly Kid", "4/15/2023", "Frami LLC", 0);
            InsertBook("Flynn Gravet", 19, "343363565-X", "Monster That Challenged the World, The", "7/1/2023", "Corwin Group", 4);
            InsertBook("Desi Sapauton", 7, "833673427-9", "Womans Face, A (En kvinnas ansikte) ", "7/19/2023", "Dooley, Hudson and Heaney", 4);
            InsertBook("Meade Tarbert", 11, "590773546-5", "Scratch", "8/1/2023", "Pagac, Greenfelder and Turcotte", 1);
            InsertBook("Garret Reddish", 2, "330495467-8", "Serpico", "6/28/2023", "Rodriguez, Hackett and Mante", 5);
            InsertBook("Cynthy Raine", 10, "897688567-8", "Lucky Texan, The", "3/15/2023", "McGlynn Group", 0);
            InsertBook("Tani Brandreth", 12, "389728456-1", "Dolores Claiborne", "11/17/2023", "Oberbrunner-Lemke", 3);
            InsertBook("Somerset Storch", 5, "386964022-7", "Which Way Home", "6/27/2023", "Schulist LLC", 3);
            InsertBook("Leonelle O Heagertie", 9, "381433136-2", "35 and Ticking", "8/12/2023", "Mills LLC", 2);
            InsertBook("Shepperd Trappe", 13, "555417612-7", "Looking for Comedy in the Muslim World", "3/23/2023", "Schimmel, Durgan and McCullough", 1);
            InsertBook("Case Gherardelli", 7, "734925868-2", "Tales of Vesperia: The First Strike (Teiruzu obu vesuperia: The first strike)", "10/16/2023", "Watsica, Thompson and Corkery", 2);
            InsertBook("Caroljean Kenninghan", 18, "388776851-5", "Zoom", "6/3/2023", "Kerluke, Towne and Herman", 4);
            InsertBook("Eleanora Dymoke", 10, "661182192-9", "Sex in Chains (Geschlecht in Fesseln)", "10/15/2023", "Hodkiewicz LLC", 4);
            InsertBook("Kelsy Loiterton", 8, "887673983-1", "Ambush Trail", "12/8/2022", "Lowe and Sons", 1);
            InsertBook("Roxana Knipe", 11, "839669141-X", "Bells of St. Marys, The", "6/19/2023", "Considine Group", 3);
            InsertBook("Alvinia Iskowitz", 0, "058392932-X", "Police State", "7/19/2023", "Jast, Rice and Cassin", 2);
            InsertBook("Marnie Toopin", 8, "035651923-6", "Zombie Island Massacre", "11/25/2023", "Bradtke, Rippin and O Kon", 1);
            InsertBook("Daren Sigert", 19, "085104833-1", "Sidewalks of London (St. Martins Lane)", "12/30/2022", "Towne, Wiza and Simonis", 1);
            InsertBook("Jdavie McLinden", 15, "965153704-3", "Dangerous Method, A", "1/24/2023", "Botsford-Reichel", 3);
            InsertBook("Joline West", 10, "575488660-8", "One Mile Away", "6/15/2023", "Towne, Schultz and Lynch", 4);
            InsertBook("Gauthier Hounsome", 9, "422798317-7", "Lilo & Stitch", "4/28/2023", "Gottlieb, Reichert and Auer", 0);
            InsertBook("Brew Leversha", 11, "002819813-1", "Young and the Damned, The (Olvidados, Los)", "4/12/2023", "Hoeger Group", 4);
            InsertBook("Timmi Dadson", 19, "226830860-X", "C(r)ook (Basta - Rotwein Oder Totsein)", "10/2/2023", "Wehner-Shields", 3);
            InsertBook("Corrina MacPhail", 0, "510683909-2", "Opportunists, The", "3/28/2023", "Aufderhar Inc", 0);
            InsertBook("Vikky Pigram", 17, "168313735-3", "Gifted Hands: The Ben Carson Story", "3/6/2023", "Hills, Torp and Fay", 1);
            InsertBook("Gail Eastes", 16, "121225513-5", "Hole, The (Dong)", "12/18/2022", "Morissette, Ferry and Williamson", 0);
            InsertBook("Shayna Riddiford", 2, "614340921-8", "Solomon Northups Odyssey", "1/21/2023", "Johnston-Altenwerth", 1);
            InsertBook("Virgilio Grant", 12, "495468982-1", "A Cinderella Story: Once Upon a Song", "3/31/2023", "Smitham-Runolfsdottir", 3);
            InsertBook("Dora Chat", 8, "211897463-9", "Pieces (Mil gritos tiene la noche) (One Thousand Cries Has the Night)", "11/22/2023", "Dooley, Flatley and Hirthe", 4);
            InsertBook("Cristabel Howard", 11, "613893049-5", "Dont Go Breaking My Heart (Daan gyun naam yu)", "11/7/2023", "Cronin LLC", 5);
            InsertBook("Mae Emmens", 14, "153573066-8", "House Party", "12/16/2022", "Predovic Group", 3);
            InsertBook("Essie Warwick", 7, "259914870-7", "Little Murders", "4/26/2023", "Hamill-Wehner", 1);
            InsertBook("Ansel Upston", 6, "944652397-4", "Appleseed (Appurushîdo)", "3/5/2023", "Heaney Group", 4);
            InsertBook("Tiffany Borthe", 3, "262024543-5", "Magic Camp", "5/13/2023", "Schimmel, Blanda and Langworth", 4);
            InsertBook("Terri Ochterlony", 20, "076831306-6", "Its a Free World...", "8/8/2023", "Kilback Group", 2);
            InsertBook("Lib Krysztowczyk", 17, "649027349-7", "Arthur", "6/28/2023", "Koelpin Inc", 2);
            InsertBook("Terri-jo Hammelberg", 14, "789736262-0", "Exotica", "9/25/2023", "Roberts-Ward", 2);
            InsertBook("Wadsworth Franke", 20, "680557258-4", "Intimate Lighting (Intimni osvetleni)", "6/23/2023", "Purdy, Kunde and Von", 1);
            InsertBook("Elsinore Gutridge", 10, "370384085-4", "Man in the Wilderness", "10/3/2023", "Leffler-Okuneva", 5);
            InsertBook("Iorgos Priddie", 16, "564030522-3", "Very Bad Things", "2/4/2023", "D Amore, O Keefe and Legros", 2);
            InsertBook("Kim Crunkhurn", 6, "983815924-7", "Heidi Fleiss: Hollywood Madam", "10/18/2023", "Nikolaus, Moen and Zboncak", 5);
            InsertBook("Corabel Wordsworth", 1, "610001436-X", "Magadheera", "4/9/2023", "Cummings LLC", 5);
            InsertBook("Krystalle Guyver", 0, "170637218-3", "Third Star", "3/20/2023", "Howe-Powlowski", 2);
            InsertBook("Gardy Vaun", 3, "404189193-0", "Jour se lève, Le (Daybreak)", "1/1/2023", "Upton, O Conner and Mueller", 1);
            InsertBook("Elsinore Gutridge", 16, "256875873-2", "Knights of Bloodsteel", "2/9/2023", "Torphy LLC", 4);
            InsertBook("Lionello Cosyns", 7, "564057770-3", "Prom Queen: The Marc Hall Story", "8/29/2023", "Hagenes and Sons", 0);
            InsertBook("Cristiano McCole", 19, "678307013-1", "Accidental Spy, The (Dak miu mai shing)", "4/17/2023", "Schuppe, Harber and Kassulke", 5);
            InsertBook("Emery Sirmond", 7, "187784068-8", "Snowballs", "5/22/2023", "Lindgren-Wilderman", 5);
            InsertBook("Malchy Lindenbluth", 2, "380351399-5", "Il fiore dai petali d acciaio", "10/1/2023", "Lindgren-Paucek", 3);
            InsertBook("Dulsea Dunbobbin", 10, "585413306-7", "Moonraker", "2/22/2023", "Wyman-Kulas", 0);
            InsertBook("Bethany Chetham", 11, "777156546-8", "Casey Jones", "7/26/2023", "Terry, Hansen and Ryan", 3);
            InsertBook("Corabel Wordsworth", 13, "689661761-6", "Deep Blue Sea, The", "9/3/2023", "Pfeffer-Ondricka", 0);
            InsertBook("Aloisia Kentish", 10, "320432919-4", "Walter Defends Sarajevo (Valter brani Sarajevo)", "12/18/2022", "Emard Group", 2);
            InsertBook("Denny Roomes", 10, "771808773-5", "Love, Rosie", "4/23/2023", "Bayer-Hermiston", 1);
            InsertBook("Selle Worsom", 19, "398496144-8", "Where the Truth Lies", "5/23/2023", "DuBuque LLC", 2);
            InsertBook("Mariana Boc", 5, "119553097-8", "Werckmeister Harmonies (Werckmeister harmóniák)", "9/1/2023", "Pfannerstill, Upton and Yundt", 3);
            InsertBook("Durand Quimby", 18, "385274314-1", "Donovans Reef", "7/20/2023", "Dooley-Lemke", 0);
            InsertBook("Hallie Tzar", 20, "989701725-9", "Judge Priest", "3/13/2023", "Veum LLC", 5);
            InsertBook("Ange Swafield", 19, "485472870-1", "Mirage", "7/4/2023", "Nicolas-Predovic", 3);
            InsertBook("Englebert Rice", 12, "036890786-4", "Shiri (Swiri)", "8/28/2023", "Gusikowski and Sons", 1);
            InsertBook("Saudra Petrusch", 8, "309752189-5", "Knights of Badassdom", "2/14/2023", "Sauer, Bode and Berge", 2);
            InsertBook("Godfree Baldrick", 8, "580008780-6", "Bishop Murder Case, The", "4/24/2023", "Lemke LLC", 3);
            InsertBook("Lynea Terrington", 14, "997934418-0", "Dirty Filthy Love", "2/26/2023", "Abshire Group", 3);
            InsertBook("Iorgos Priddie", 1, "232287359-4", "Café Metropole", "1/9/2023", "Wiegand-Baumbach", 5);
            InsertBook("Avril Rableau", 13, "001123760-0", "Fireworks Wednesday (Chaharshanbe-soori)", "9/10/2023", "Green-Quigley", 0);
            InsertBook("Maye Zapater", 3, "196876246-9", "But Im a Cheerleader", "12/27/2022", "Bruen Group", 5);
            InsertBook("Hailee Ridsdell", 7, "035937091-8", "Young in Heart, The", "10/28/2023", "Von-Hoppe", 4);
            InsertBook("Roxana Knipe", 3, "585715613-0", "This is Martin Bonner", "3/14/2023", "Buckridge, Mann and Sawayn", 3);
            InsertBook("Neilla Beades", 7, "612398941-3", "Elementary Particles, The (Elementarteilchen)", "12/19/2022", "Hudson-Stoltenberg", 2);
            InsertBook("Iorgos Priddie", 8, "273337775-2", "Mummys Curse, The", "6/18/2023", "Schneider Group", 3);
            InsertBook("Sheelah Chastanet", 14, "049774702-2", "What Women Want", "2/24/2023", "Swaniawski-Bartell", 1);
            InsertBook("Davy Juliano", 19, "160053422-8", "Kevin Hart: Im a Grown Little Man", "12/21/2022", "Wehner-Shanahan", 3);
            InsertBook("Kane Breeze", 3, "472249796-6", "Brother Bear", "5/6/2023", "Dickinson-Legros", 5);
            InsertBook("Rollo Edgerley", 20, "899529497-3", "Safe Passage", "4/16/2023", "Howe LLC", 1);
            InsertBook("Flynn Gravet", 15, "261179151-1", "Shade", "8/15/2023", "Gibson, Kuphal and Emard", 3);
            InsertBook("Elissa Curman", 18, "900855579-4", "Paper Chase, The", "1/17/2023", "Gerlach Inc", 1);
            InsertBook("Orrin Kendred", 7, "389660963-7", "War and Peace (Jang Aur Aman)", "9/29/2023", "West, Yundt and Lind", 5);
            InsertBook("Emery Sirmond", 1, "166127332-7", "Spinning Boris", "5/9/2023", "Lang, Reichert and Frami", 5);
            InsertBook("Zaccaria Carlton", 19, "871983251-6", "Wanted", "7/27/2023", "Ortiz, Wiza and Ledner", 4);
            InsertBook("Romeo Haughton", 4, "336044370-5", "Silk Stockings", "6/10/2023", "Wolf-Vandervort", 5);
            InsertBook("Terza Lightbown", 4, "053937749-X", "Deadgirl", "3/10/2023", "Collins LLC", 2);
            InsertBook("Tadeo Jeaves", 11, "382240863-8", "Pavement: Slow Century", "2/6/2023", "Jerde, Schmitt and Leannon", 4);
            InsertBook("Koren Pearl", 7, "494492082-2", "Sanatorium", "5/24/2023", "Kling-Pouros", 0);
            InsertBook("Malissa Segebrecht", 7, "961786300-6", "Santitos", "9/28/2023", "Bradtke, Gislason and Smitham", 3);
            InsertBook("Derick Roggeman", 9, "578673948-1", "My Man and I", "12/30/2022", "Fisher Group", 5);
            InsertBook("Zahara Lohrensen", 12, "677760890-7", "Hunger Games: Catching Fire, The", "12/1/2022", "Terry and Sons", 0);
            InsertBook("Sheena Matthias", 15, "180310748-0", "Police Academy 5: Assignment: Miami Beach", "8/22/2023", "Schowalter Inc", 2);
            InsertBook("Lucila Byrkmyr", 19, "017809462-5", "Scooby-Doo! WrestleMania Mystery", "6/26/2023", "Feest, Schaefer and Hermann", 1);
            InsertBook("Euphemia McNeill", 2, "767370081-6", "Summer Palace (Yihe yuan)", "3/2/2023", "Windler Group", 3);
            InsertBook("Shelton Bourges", 9, "524665308-0", "Tuesdays with Morrie", "9/17/2023", "Schuppe LLC", 5);
            InsertBook("Nickolas Reedshaw", 19, "689840737-6", "Sommersby", "2/15/2023", "Russel-Romaguera", 4);
            InsertBook("Essie Warwick", 2, "024811856-0", "What the #$*! Do We Know!? (a.k.a. What the Bleep Do We Know!?)", "7/17/2023", "Bins, Sipes and Hirthe", 4);
            InsertBook("Jeno Grier", 14, "852274753-9", "In the Shadow of Doubt (Epäilyksen varjossa)", "8/22/2023", "Balistreri-Langworth", 2);
            InsertBook("Jeannette Mulvany", 20, "011143795-4", "Flyboys", "9/22/2023", "Hayes-Hermiston", 0);
            InsertBook("Mirilla Liston", 13, "395024971-0", "Whisper", "1/27/2023", "Powlowski, West and Will", 3);
            InsertBook("Mia Frantsev", 8, "431813152-1", "Flowers in the Attic", "12/11/2022", "Bernier-Schmitt", 1);
            InsertBook("Hailee Ridsdell", 4, "716592352-7", "After Tiller", "11/10/2023", "Murazik-Dickinson", 0);
            InsertBook("Stacee Beautyman", 9, "364908458-9", "Great King, The (Der große König)", "5/20/2023", "Hagenes-Raynor", 5);
            InsertBook("Mae Cotelard", 20, "107730553-2", "Merry Gentleman, The", "6/8/2023", "Stroman and Sons", 4);
            InsertBook("Shelby Klulicek", 3, "400386138-8", "One Small Hitch", "1/14/2023", "Gusikowski Inc", 0);
            InsertBook("Vasilis Gateland", 16, "397379822-2", "Heatstroke", "8/20/2023", "Walter, Dickens and Koch", 1);
            InsertBook("Sheena Matthias", 7, "297996393-3", "In the Army Now", "1/2/2023", "Ortiz-Will", 2);
            InsertBook("Madonna Elsby", 6, "486010799-3", "Fright Night Part II", "5/27/2023", "Johnston-Sawayn", 1);
            InsertBook("Deina Zanetti", 3, "201480921-6", "Planet of the Apes", "4/21/2023", "Rogahn and Sons", 2);
            InsertBook("Lotti Kloisner", 1, "422159289-3", "Pelican Brief, The", "1/5/2023", "Waters-Senger", 1);
            InsertBook("Odie Garlick", 17, "902018936-0", "Comedy, The", "2/25/2023", "Bashirian and Sons", 0);
            InsertBook("Adolphe Rolinson", 4, "876566852-9", "Chaos", "6/22/2023", "Waters-Connelly", 0);
            InsertBook("Gayler Zuppa", 8, "920660844-4", "The Package", "3/29/2023", "Graham-Waters", 0);
            InsertBook("Adrienne Kindon", 12, "579941517-5", "Englishman Who Went Up a Hill But Came Down a Mountain, The", "12/31/2022", "Nienow-Schuster", 2);
            InsertBook("Wadsworth Franke", 6, "920135015-5", "Glenn Killing på Berns", "8/25/2023", "Emard-Collier", 3);
            InsertBook("Gertrud Titlow", 3, "471881599-1", "Blast from the Past", "11/17/2023", "Kunde-Wolf", 2);
            InsertBook("Hershel Newlands", 2, "971411221-4", "Cell 2, The", "11/1/2023", "Lehner, Pagac and Greenholt", 3);
            InsertBook("Ange Swafield", 18, "685948423-3", "Unforgotten: Twenty-Five Years After Willowbrook", "3/7/2023", "Mitchell, Sporer and Yundt", 1);
            InsertBook("Jobye Giacomozzo", 10, "011484233-7", "Lost Reels of Pancho Villa, The (Los rollos perdidos de Pancho Villa)", "12/16/2022", "Casper, Dietrich and Bruen", 0);
            InsertBook("Felix Blaine", 9, "653277973-3", "Heartbeeps", "3/24/2023", "Goldner, Buckridge and Rath", 5);
            InsertBook("Alfonse Kevern", 12, "945662078-6", "One Potato, Two Potato", "2/13/2023", "Torphy and Sons", 2);
            InsertBook("Faye Gooke", 3, "244192174-6", "Samouraï, Le (Godson, The)", "3/5/2023", "Towne-Wiza", 3);
            InsertBook("Tiffany Borthe", 2, "028093616-8", "Sams Song", "5/26/2023", "Runolfsdottir, Wisoky and Luettgen", 4);
            InsertBook("Alanah Assinder", 11, "613510856-5", "Mahjong (Ma jiang)", "7/23/2023", "Metz, Jacobson and Legros", 3);
            InsertBook("Derrik Ruddell", 9, "590709760-4", "Transcendent Man", "3/26/2023", "Cummings, Bergnaum and Doyle", 3);
            InsertBook("Maxwell Whewill", 18, "450508867-7", "Tulsa", "5/29/2023", "Marquardt-Howe", 0);
            InsertBook("Hallsy McKerrow", 16, "314848569-6", "A Night at the Movies: The Horrors of Stephen King", "6/27/2023", "Champlin and Sons", 2);
            InsertBook("Stephi Britch", 1, "297780329-7", "Strange Love of Martha Ivers, The", "12/3/2022", "Ankunding and Sons", 1);
            InsertBook("Krystalle Guyver", 11, "853317261-3", "Call Me Crazy: A Five Film", "3/4/2023", "Reinger, Harber and Howe", 5);
            InsertBook("Bendix Ligerton", 2, "278517291-4", "Everything Must Go", "2/26/2023", "Bartoletti Inc", 0);
            InsertBook("Cal Endrici", 12, "922108770-0", "Brazil", "3/2/2023", "Lynch and Sons", 1);
            InsertBook("Gabrielle McCumskay", 14, "443593887-1", "Schlussmacher", "3/27/2023", "Schaden LLC", 4);
            InsertBook("Lionello Pharro", 14, "235803036-8", "Christopher Strong", "11/19/2023", "Streich LLC", 5);
            InsertBook("Imogene Viscovi", 17, "176882557-2", "Cardcaptor Sakura: The Sealed Card", "8/1/2023", "Ortiz, Sawayn and Crona", 3);
            InsertBook("Samuel Grinnov", 10, "563665463-4", "Billy Budd", "3/19/2023", "Bartell-Kuhlman", 1);
            InsertBook("Nilson Scoffins", 8, "920729303-X", "Charlotte Gray", "7/25/2023", "Sipes, Bruen and Harvey", 5);
            InsertBook("Jamil Bearcroft", 3, "178751121-9", "Firm, The", "7/16/2023", "MacGyver-Barton", 0);
            InsertBook("Mirilla Liston", 18, "835154031-5", "Wild, Wild Planet (I criminali della galassia)", "7/13/2023", "Schowalter-Dicki", 0);
            InsertBook("Pattin Baskeyfield", 2, "464324845-9", "Late Spring (Banshun)", "3/29/2023", "Marquardt and Sons", 2);
            InsertBook("Martelle Moulson", 8, "961390643-6", "Electric Dreams", "10/27/2023", "Orn LLC", 1);
            InsertBook("Desirae Zollner", 1, "529025619-9", "Band Baaja Baaraat", "11/18/2023", "Champlin-Bernier", 5);
            InsertBook("Flynn Gravet", 17, "569106349-9", "Sonatine (Sonachine)", "12/7/2022", "Hagenes and Sons", 3);
            InsertBook("Romeo Haughton", 18, "117609757-1", "Halo Legends", "12/24/2022", "Kessler and Sons", 4);
            InsertBook("Derek Argue", 5, "757656508-X", "Drawn Together Movie: The Movie!, The", "5/26/2023", "Schulist, Denesik and Ruecker", 0);
            InsertBook("Jackqueline Woodus", 15, "026664515-1", "Achilles and the Tortoise (Akiresu to kame)", "10/22/2023", "Legros, Blick and Hane", 4);
            InsertBook("Alaric Echallie", 13, "778422330-7", "String, The (Le fil)", "5/20/2023", "Schuster LLC", 4);
            InsertBook("Sinclair Paffley", 7, "014459319-X", "Staying Together", "3/6/2023", "Baumbach-Farrell", 3);
            InsertBook("Barret Klewi", 4, "302579478-X", "The Sky Dragon", "1/31/2023", "Legros and Sons", 3);
            InsertBook("Lefty Huyhton", 20, "844858349-3", "Back Street", "5/13/2023", "Cronin, Jakubowski and Johnson", 5);
            InsertBook("Mina Dumper", 20, "628171552-7", "Absent (Ausente)", "2/5/2023", "Bayer-Towne", 1);
            InsertBook("Rufus Tramel", 13, "436692091-3", "All the Brothers Were Valiant", "3/17/2023", "Pollich Group", 0);
            InsertBook("Erv Ellsworthe", 0, "539038849-6", "White Palace", "3/16/2023", "Haag-Morar", 2);
            InsertBook("Wilie Olczak", 2, "311122082-6", "Wild Parrots of Telegraph Hill, The", "7/19/2023", "Fay-Torp", 5);
            InsertBook("Madonna Elsby", 10, "597098365-9", "Wild Reeds (Les roseaux sauvages)", "2/5/2023", "Wiegand-Jaskolski", 3);
            InsertBook("Neila Farnell", 3, "309504093-8", "Prom Night in Mississippi", "3/30/2023", "Kertzmann and Sons", 2);
            InsertBook("Malissa Segebrecht", 20, "031151840-0", "Big Sur", "8/26/2023", "Hettinger-Kuhlman", 3);
            InsertBook("Erinna Domleo", 0, "050325408-8", "Steam Experiment, The", "8/5/2023", "Herzog Group", 2);
            InsertBook("Una Cato", 5, "551438723-2", "Ask Me Anything", "1/30/2023", "Boyer, Kunze and Roberts", 2);
            InsertBook("Chester Chelley", 1, "614460811-7", "Win a Date with Tad Hamilton!", "7/21/2023", "Becker Inc", 1);
            InsertBook("Josiah Anthonsen", 11, "247806250-X", "Leopard, The (Gattopardo, Il)", "4/26/2023", "Bechtelar-Cremin", 3);
            InsertBook("Kit Creus", 2, "305393753-0", "Mighty Aphrodite", "8/12/2023", "Nienow, Stoltenberg and Ondricka", 0);
            InsertBook("Garreth Jancar", 8, "612264493-5", "Manderlay", "5/13/2023", "Kozey LLC", 0);
            InsertBook("Odey Hyatt", 6, "830048916-9", "Grave Halloween", "11/7/2023", "Gutkowski LLC", 4);

        }

        /// <summary>
        /// Inserts a book into the database along with a new author and the books audiance
        /// </summary>
        /// <param name="authorsName">the authors name</param>
        /// <param name="genreName">the name of the genre</param>
        /// <param name="isbn">the ISBN Number</param>
        /// <param name="title">The books Title</param>
        /// <param name="publishDate">the day the book was published in year-month-day like "2015-01-24"</param>
        /// <param name="publisher">The name of the publisher</param>
        /// <param name="readerType">The books target Audience</param>
        private void InsertBook(string authorsName, int genreName, string isbn, string title, string publishDate, string publisher, int audienceType)
        {
            int authID = 0;
            bool exists = false;
            //Checks to see if authors name exists in database
            /*
             * If SqlConnection and SqlCommand are NOT Functioning then
             * 1) Right Click 'LibraryDatabase'
             * 2) Click on Manage NuGet Packages option
             * 3) In the NuGet Package Manager window, Select the Browser Tab. Search for System.Data.SqlClient
             * 4) System.Data.SqlClient by Microsoft and click install
             */
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT AuthorID, FullName FROM [LibraryDB].[Author] WHERE FullName = '" + authorsName + "'";
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["FullName"]);
                            if (form == authorsName)
                            {
                                exists = true;
                                form = String.Format("{0}", reader["AuthorID"]);
                                authID = int.Parse(form);
                            }

                        }
                    }
                    finally
                    {
                        reader.Close();
                    }

                    connection.Close();
                }
            }

            //if author doesn't exist then insert Author's name into LibraryDB.Author
            //Then gets the Authors AuthorID from LibraryDB.Author
            if (exists == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO [LibraryDB].[Author](FullName) VALUES('" + authorsName + "')";

                        connection.Open();
                        command.ExecuteNonQuery();

                        command.CommandText = "SELECT AuthorID, FullName FROM [LibraryDB].[Author] WHERE FullName = '" + authorsName + "'";
                        SqlDataReader reader = command.ExecuteReader();
                        try
                        {
                            while (reader.Read())
                            {
                                string form = String.Format("{0}", reader["AuthorID"]);
                                authID = int.Parse(form);
                            }
                        }
                        finally
                        {
                            reader.Close();
                        }
                        connection.Close();
                    }
                }
            }
            //inserts the book into LibraryDB.BookTitle but no AudienceID and No GenreID
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO [LibraryDB].[BookTitle] (AuthorID, AudienceID, GenreID, ISBN, Title, PublishDate, Publisher) " +
                        "VALUES('"+ authID + "', '" + audienceType + "', '" + genreName + "','" + isbn + "', '" + title + "', '" + publishDate + "', '" + publisher + "')";

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Inserts a patron into the database
        /// </summary>
        /// <param name="cardNum">The number on the card</param>
        /// <param name="name">the persons full name</param>
        /// <param name="phoneNum">the individuals phone number</param>
        /// <param name="address">the individuals address</param>
        /// <param name="dateofBirth">the individuals Date of birth</param>
        /// <param name="isKid">Whether or not the patron is a kid</param>
        private void InsertPatron(int cardNum, string name, string phoneNum, string address, string dateofBirth, bool isKid)
        {
            int histID = 0;



            //This retrieves the HistoryId number from the database
            //when retrieving patron history information ingore all results whose checked out date equals its checked in date 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO [LibraryDB].[History](BookCopyID, CheckedOutDate, CheckedInDate) VALUES('1', '2000-01-01 05:30:00', '2000-01-01 05:30:00')";
                    connection.Open();
                    command.ExecuteNonQuery();

                    command.CommandText = "SELECT HistoryID FROM [LibraryDB].[History]";
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            string form = String.Format("{0}", reader["HistoryID"]);
                            histID = int.Parse(form);
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                    connection.Close();
                }
            }



            //inserts the Patron into the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "";
                    if (isKid)
                    {
                        command.CommandText = "INSERT INTO [LibraryDB].[Patron] (HistoryID, CardNumber, [FullName], PhoneNumber, [Address], BirthDate, KidReader) " +
                        "VALUES('" + histID + "', '" + cardNum + "', '" + name + "','" + phoneNum + "', '" + address + "', '" + dateofBirth + "', '0')";
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO [LibraryDB].[Patron] (HistoryID, CardNumber, [FullName], PhoneNumber, [Address], BirthDate, KidReader) " +
                        "VALUES('" + histID + "', '" + cardNum + "', '" + name + "','" + phoneNum + "', '" + address + "', '" + dateofBirth + "', '1')";
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }




    }
}
