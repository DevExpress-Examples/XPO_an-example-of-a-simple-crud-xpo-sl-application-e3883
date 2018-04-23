using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DevExpress.Xpo.DB;
using DevExpress.Xpo;

namespace SilverlightApplication5.Web {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Service1 : DataStoreService {
        static IDataStore dataStore;
        static Service1() {
            string cs = MSSqlConnectionProvider.GetConnectionString("localhost", "XpoSLExample");
            dataStore = XpoDefault.GetConnectionProvider(cs, AutoCreateOption.SchemaAlreadyExists);
        }
        public Service1()
            : base(dataStore) {
        }
    }
}
