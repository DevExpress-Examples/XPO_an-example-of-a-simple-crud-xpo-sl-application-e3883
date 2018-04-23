using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo.DB;
using DevExpress.Xpo;
using SilverlightApplication5;

namespace ApplicationUpdater {
    class Program {
        static void Main(string[] args) {
            string cs = MSSqlConnectionProvider.GetConnectionString("localhost", "XpoSLExample");
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(cs, AutoCreateOption.DatabaseAndSchema);
            XpoDefault.Session = null;
            try {
                using (UnitOfWork uow = new UnitOfWork()) {
                    uow.UpdateSchema(typeof(Program).Assembly);
                    uow.CreateObjectTypeRecords(typeof(Program).Assembly);
                    Person p = uow.FindObject<Person>(null);
                    if (p == null) {
                        new Person(uow) { FirstName = "Jake", LastName = "Smith", Birthdate = new DateTime(1990, 10, 1) };
                        new Person(uow) { FirstName = "Bill", LastName = "Simpson", Birthdate = new DateTime(1982, 2, 20) };
                        new Person(uow) { FirstName = "Harry", LastName = "Jonathan", Birthdate = new DateTime(1972, 5, 17) };
                        uow.CommitChanges();
                    }
                    Console.WriteLine("Update successful.");
                }
            } catch (Exception) {
                Console.WriteLine("Update failed.");
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
