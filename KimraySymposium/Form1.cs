using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimraySymposium
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }   /* Constructor() */

        private void PrintCSV_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;               // hourglass cursor...
            string _title = "\"Kimray Symposium Signup List\" \n\"Thurs., May 9th, 2019 (8am-8pm)\"\n";
            string _attendees = "Year,SignupDate,Name,LoginID,IPAddress,CompanyName,Email,Phone,isAttendingEvening,isNeedingTransportation";
            string _filename = @"C:\Book1.csv";
            StringBuilder SB = new StringBuilder();

            using (SqlConnection connection = new SqlConnection("Server=okc-sql-02;Database=KimrayDotCom;Uid=WebAppUser;Pwd=4edc%TGV; "))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Symposium WHERE (Year = 2019); ", connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        File.Delete(_filename);     // delete the output file...

                        using (StreamWriter writetext = new StreamWriter(_filename))
                        {
                            if (reader.HasRows)             // Check is the reader has any rows at all before starting to read.
                            {
                                writetext.WriteLine(_title);
                                writetext.WriteLine(_attendees);

                                while (reader.Read())       // Read advances to the next row.
                                {
                                    SB.Length = 0; SB.Capacity = 0;         // empty the StringBuilder...
                                    SB.Append($@"{reader.GetInt32(1)},");   // get "id" and "Year"...

                                    for (int ii = 2; ii < 11; ++ii)
                                    {
                                        string tmpstr = "";

                                        if ( !reader.IsDBNull(ii) )
                                            tmpstr = reader.GetString(ii);  // construct the record string...

                                        SB.Append($"\"{tmpstr}\",");        // Add...
                                    }   /* for ii */

                                    if(SB.Length > 0)
                                        --SB.Length;        // remove final comma ",".....

                                    writetext.WriteLine(SB);
                                }   /* while reader.Read */
                            }   /* if reader.HasRows */
                            else
                                writetext.WriteLine("* * * There are no attendees on the list yet! * * *");
                        }   /* using writetext */
                    }   /* using reader */

                    connection.Close();             // close the connection...
                }   /* using SqlCommand */
            }   /* using SqlConnection */

            System.Diagnostics.Process.Start(_filename);
            this.Cursor = Cursors.Default;          // arrow-shaped cursor...
    }   /* PrintCSV_Click() */

}   /* class Form1 */
}   /* namespace KimraySymposium */
