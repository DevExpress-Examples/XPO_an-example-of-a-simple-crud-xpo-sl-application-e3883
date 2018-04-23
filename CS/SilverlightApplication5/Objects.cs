using System;
using System.Net;
using DevExpress.Xpo;

namespace SilverlightApplication5 {
    public class Person : XPObject {
        string firstName;
        public string FirstName {
            get { return firstName; }
            set { SetPropertyValue<string>("FirstName", ref firstName, value); }
        }
        string lastName;
        public string LastName {
            get { return lastName; }
            set { SetPropertyValue<string>("LastName", ref lastName, value); }
        }
        DateTime birthdate;
        public DateTime Birthdate {
            get { return birthdate; }
            set { SetPropertyValue<DateTime>("Birthdate", ref birthdate, value); }
        }
        public Person(Session session)
            : base(session) {
        }
    }
}
