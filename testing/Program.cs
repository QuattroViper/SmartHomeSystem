using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using ClassLibrary;
using System.Data;
using ClassLibrary.classes;
using ClassLibrary.classes.lazyLoad.client;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {

            //List<ClassLibrary.classes.System> systems = new List<ClassLibrary.classes.System>();
            //List<Address> addresses = new List<Address> { new Address(new Guid(), "Swartpiek cres 4", "Shawu", "Ninapark", "0152", "Pretoria", "South Africa") };

            //Account ac = new Account(new Guid(), "30 Day", 200m, new string[] { }, false, 300m, new DateTime(2018, 5, 20), new Card(new Guid(), "Absa", "Check", "1234 5678 0987 6543", new DateTime(2020, 5, 6), "Marno van Niekerk", "123"));
            //Login l = new Login(new Guid(), "marnobvn", "123456789", null, null);

            //Contact con = new Contact(new Guid(), "071 990 8072", "marnovn071@gmail.com", "0", "0", new int[] { 0, 0, 0, });

            //Client cl = new Client(new Guid(), systems, ac, addresses, l, con, "", "9702065134084", "Marno", "van Niekerk", new DateTime(1997, 2, 6), 1);

            //////List<ClassLibrary.classes.System> systems = new List<ClassLibrary.classes.System>();
            //////List<Address> addresses = new List<Address> { new Address(new Guid(), "HammerEmmer", " ", "Akaisa", "0184", "Pretoria", "South Africa") };

            //////Account ac = new Account(new Guid(), "1 Day", 200m, new string[] { }, false, 300m, new DateTime(2018, 5, 20), new Card(new Guid(), "FNB", "Savings", "9876 6541 2568 8745", new DateTime(2021, 5, 6), "Renier le Roux", "147"));
            //////Login l = new Login(new Guid(), "renersdf", "123wert5", null, null);

            //////Contact con = new Contact(new Guid(), "083 524 7845", "maatvanysterenplaat@gmail.com", "0", "0", new int[] { 0, 0, 0, });

            //////Client cl = new Client(new Guid(), systems, ac, addresses, l, con, "", "9709165789456", "Renier", "le Roux", new DateTime(1997, 9, 16), 1);

            //cl.insertClient();

            //List<Client> clients = new ClientList().getAllClients();

            //

            //string clientAddressQuery = @"SELECT A.address1,A.address2,A.city,A.country, A.[guid], A.postalCode, A.suburb FROM tblAddressClient AC 
            //                             INNER JOIN tblAddress A ON AC.address_guid = A.[guid]
            //                              WHERE AC.client_guid = CAST('FFFCB31D-348A-463E-A825-9B61C9100A1E' AS uniqueidentifier)";

            //ClassLibrary.classes.lazyLoad.client.Details sdf = new ClassLibrary.classes.lazyLoad.client.Details(Guid.NewGuid());

            //foreach (Address address in addresses)
            //{
            //    Console.WriteLine(string.Format("{0} {1}  -  {2}", address.Address1, address.Address2, address.Suburb));
            //}

            //Details dets = new Details().lazyLoadClientDetails(new Guid("FFFCB31D-348A-463E-A825-9B61C9100A1E"));

            //Console.WriteLine(ClassLibrary.functions.functions.var_dump(dets, 0));

            //Card c1 = new Card(Guid.NewGuid(), "Absa bank", "Check", "1234 1234 1234 1234", new DateTime(2020, 11, 02), "Manru van Niekerk", "999");
            //Card c2 = new Card(Guid.NewGuid(), "Absa bank", "Savings", "9999 444 5555 1111", new DateTime(2019, 10, 06), "Eldane Ferrari", "111");
            //Card c3 = new Card(Guid.NewGuid(), "Standard bank", "Check", "2525 6565 7878 9898", new DateTime(2021, 03, 15), "Renier le Roux", "546");
            //Card c4 = new Card(Guid.NewGuid(), "Absa bank", "Savings", "0000 0000 0000 0000", new DateTime(2016, 12, 05), "Tanya de Jager", "975");

            //Console.WriteLine(c1.ToString());
            //Console.WriteLine();
            //Console.WriteLine(c1.encryptObject());
            //Console.WriteLine();
            //Console.WriteLine(c2.encryptObject());
            //Console.WriteLine();
            //Console.WriteLine(c3.encryptObject());
            //Console.WriteLine();
            //Console.WriteLine(c4.encryptObject());
            //Console.WriteLine();


            //Login l1 = new Login(Guid.NewGuid(), "marnovn071", "123456", null, null);
            //Login l2 = new Login(Guid.NewGuid(), "El_dane", "el232.", null, null);
            //Login l3 = new Login(Guid.NewGuid(), "reneeeereer", "qwerty.", null, null);
            //Login l4 = new Login(Guid.NewGuid(), "TheLastDJ", "123456.", null, null);

            //Console.WriteLine(l1.ToString());
            //Console.WriteLine();
            //Console.WriteLine(l1.encryptString());
            //Console.WriteLine();
            //Console.WriteLine(l2.encryptString());
            //Console.WriteLine();
            //Console.WriteLine(l3.encryptString());
            //Console.WriteLine();
            //Console.WriteLine(l4.encryptString());

            Console.WriteLine(Guid.NewGuid().ToString());

            Console.ReadKey();

        }


    }
}
