using System.Data.OleDb;

namespace PLPMonitoria
{
    class connection
    {
        string uri_db;
        public OleDbConnection con;

        public connection()
        {
            this.uri_db = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\mateus\Documents\Apollo15\Apollo15.mdb";
            this.con = new OleDbConnection(this.uri_db);
        }
    }
}
