using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfSharp.Pdf.IO;
using System.Collections;
using System.Data.SqlClient;

namespace PR_Printer
{
    public partial class Form1 : Form
    {
        //Globals
        private string myConnectionString = "";
        public Form1()
        {
            InitializeComponent();
            this.ActiveControl = meetIDTextBox;
            this.AcceptButton = accessDatabaseButton;
            currentDataSourceLabel.Text = getCurrentDataSource();
            myConnectionString = "";
        }
        
        /// <summary>
        /// Gets a list of attributes of the records that were added from
        /// the user defined meet.
        /// </summary>
        /// <returns>string[] newlyEnteredResults</returns>
        public string[] getNewlyEnteredResults()
        {
            List<string> newlyEnteredResults = new List<string>();

            try
            {
                // Open OleDb Connection
                OleDbConnection myConnection = new OleDbConnection();
                myConnection.ConnectionString = myConnectionString;
                myConnection.Open();

                //// Execute Queries
                OleDbCommand cmd = myConnection.CreateCommand();
                int meetID = Convert.ToInt32(meetIDTextBox.Text);

                //gets all the new results from the meet------------------------------------------------------------
                cmd.CommandText = "select RESULT.ATHLETE, RESULT.I_R, RESULT.SCORE, RESULT.DISTANCE, RESULT.EVENT, MTEVENT.SEX, MTEVENT.AthsInRelay from RESULT inner join MTEVENT on RESULT.MTEVENT = MTEVENT.MTEVENT where RESULT.MEET=@1";
                cmd.Parameters.AddWithValue("@1", meetID);
                OleDbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (Convert.ToInt32(reader[0]) != 0)
                    {
                        newlyEnteredResults.Add(Convert.ToString(reader[0]));
                        newlyEnteredResults.Add(Convert.ToString(reader[1]));
                        newlyEnteredResults.Add(Convert.ToString(reader[2]));
                        newlyEnteredResults.Add(Convert.ToString(reader[3]));
                        newlyEnteredResults.Add(Convert.ToString(reader[4]));
                        newlyEnteredResults.Add(Convert.ToString(reader[5]));
                        newlyEnteredResults.Add(Convert.ToString(reader[6]));
                    }
                }
                reader.Close();
                cmd.Dispose();
                myConnection.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Error connecting to database:  " + e.Message);
            }
            return newlyEnteredResults.ToArray();
        }

        private int getAthleteResultForEvent(int aId, string i_r, int distance, int eventTrack, string sex, int athsInRelay)
        {
            int minTime = 99999;
            // Open OleDb Connection
            OleDbConnection myConnection = new OleDbConnection();
            myConnection.ConnectionString = myConnectionString;
            myConnection.Open();

            //// Execute Queries
            OleDbCommand cmd = myConnection.CreateCommand();
            int meetID = Convert.ToInt32(meetIDTextBox.Text);

            //gets all the new results from the meet------------------------------------------------------------
            cmd.CommandText = "select MIN(RESULT.SCORE) from RESULT inner join MTEVENT on RESULT.MTEVENT = MTEVENT.MTEVENT where RESULT.ATHLETE=@1 and RESULT.I_R=@2 and RESULT.DISTANCE=@3 and RESULT.EVENT=@4 AND MTEVENT.SEX=@5 AND MTEVENT.AthsInRelay=@6";
            cmd.Parameters.AddWithValue("@1", aId);
            cmd.Parameters.AddWithValue("@2", i_r);
            cmd.Parameters.AddWithValue("@3", distance);
            cmd.Parameters.AddWithValue("@4", eventTrack);
            cmd.Parameters.AddWithValue("@5", sex);
            cmd.Parameters.AddWithValue("@6", athsInRelay);

            OleDbDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                minTime = Convert.ToInt32(reader[0]);
            }
            reader.Close();
            cmd.Dispose();
            myConnection.Close();
            return minTime;
        }

        private void accessDatabaseButton_Click(object sender, EventArgs e)
        {
            myConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                      "Data Source="+ getCurrentDataSource() + ";" +
                      "Persist Security Info=True;" +
                      "Jet OLEDB:Database Password=5hY-tek;";

            List<string> individualPRs = new List<string>();
            List<string> relayStateQualifiers = new List<string>();

            string[] newlyEnteredResults = getNewlyEnteredResults();
            for (int i = 0; i < newlyEnteredResults.Length; i++)
            {
                if (i % 7 == 0)
                {
                    int minTime = getAthleteResultForEvent(Convert.ToInt32(newlyEnteredResults[i]), newlyEnteredResults[i + 1], Convert.ToInt32(newlyEnteredResults[i + 3]),
                                    Convert.ToInt32(newlyEnteredResults[i + 4]), newlyEnteredResults[i + 5], Convert.ToInt32(newlyEnteredResults[i + 6]));
                    if ((Convert.ToInt32(newlyEnteredResults[i + 2]) <= minTime || minTime == 99999) && newlyEnteredResults[i + 1].Equals("I")) //second half of first statement is true if no previous record is found
                    {
                        //new PR
                        string[] athleteName = getAthleteName(Convert.ToInt32(newlyEnteredResults[i]));
                        individualPRs.Add(athleteName[0]);
                        individualPRs.Add(athleteName[1]);
                        individualPRs.Add(getRace(newlyEnteredResults[i + 1], Convert.ToInt32(newlyEnteredResults[i + 3]), Convert.ToInt32(newlyEnteredResults[i + 4]), Convert.ToInt32(newlyEnteredResults[i + 6])));
                        individualPRs.Add(Convert.ToString(Math.Min(Convert.ToInt32(newlyEnteredResults[i + 2]), minTime)));
                    }
                    else if (newlyEnteredResults[i + 1].Equals("R"))
                    {
                        //dont have to check for a new PR becuase the people are changing... 
                        //check for new school record or qualifying for state
                        if (Convert.ToInt32(newlyEnteredResults[i + 4]) == 19)
                        {
                            int[] stateQualifyingRelayTimes = { 5284, 11154, 25624, 61024 };
                            if (((Convert.ToInt32(newlyEnteredResults[i + 3]) == 400 && Convert.ToInt32(newlyEnteredResults[i + 2]) < stateQualifyingRelayTimes[0]) ||
                            (Convert.ToInt32(newlyEnteredResults[i + 3]) == 800 && Convert.ToInt32(newlyEnteredResults[i + 2]) < stateQualifyingRelayTimes[1]) ||
                            (Convert.ToInt32(newlyEnteredResults[i + 3]) == 1600 && Convert.ToInt32(newlyEnteredResults[i + 2]) < stateQualifyingRelayTimes[2]) ||
                            (Convert.ToInt32(newlyEnteredResults[i + 3]) == 3200 && Convert.ToInt32(newlyEnteredResults[i + 2]) < stateQualifyingRelayTimes[3])) 
                            && Convert.ToInt32(newlyEnteredResults[i + 6]) == 4 && Convert.ToInt32(newlyEnteredResults[i + 4]) == 19)
                            {
                                for(int j = 0; j < 4; j++)
                                {
                                    relayStateQualifiers.Add("");
                                    relayStateQualifiers.Add("");
                                    relayStateQualifiers.Add(newlyEnteredResults[i + 3] + "m " + getRace(newlyEnteredResults[i + 1], Convert.ToInt32(newlyEnteredResults[i + 3]), Convert.ToInt32(newlyEnteredResults[i + 4]), Convert.ToInt32(newlyEnteredResults[i + 6])));
                                    relayStateQualifiers.Add(Convert.ToString(newlyEnteredResults[i + 2]));
                                }
                            }
                        }
                    }
                }
            }
            List<string> modIndividualPRs = deleteDuplicates(individualPRs);
            printPreview(modIndividualPRs.Concat(relayStateQualifiers).ToList());
        }


        private List<string> deleteDuplicates(List<string> list)
        {
            for(int i = 0; i < list.Count-4; i++)
            {
                if(i % 4 == 0)
                {
                    for (int j = i + 4; j < list.Count - 4; j++)
                    {
                        if (list[j].Equals(list[i]) && list[j + 1].Equals(list[i + 1]) && list[j + 2].Equals(list[i + 2]) && list[j + 3].Equals(list[i + 3]))
                        {
                            list.RemoveAt(i + 3);
                            list.RemoveAt(i + 2);
                            list.RemoveAt(i + 1);
                            list.RemoveAt(i);
                        }
                    }
                }
            }
            return list;
        }


        private string getRace(string i_r, int distance, int eventTrack, int athsInRelay)
        {
            if(i_r.Equals("I"))
            {
                if(distance != 0)
                {
                    if (eventTrack == 1) //then it is a dash
                        return distance + "m Dash";
                    else if (eventTrack == 2) //then it is a run
                        return distance + "m Run";
                    else if (eventTrack == 5) //then it is hurdles
                        return distance + "m Hurdles";
                    else
                        return distance + "m Run";
                }
                else
                {
                    if (eventTrack == 9) //then it is high jump
                        return "High Jump";
                    else if (eventTrack == 10) //then it is pole vault
                        return "Pole Vault";
                    else if (eventTrack == 11) //then it is long jump
                        return "Long Jump";
                    else if (eventTrack == 12) //then it is triple jump
                        return "Triple Jump";
                    else if (eventTrack == 13) //then it is shot put
                        return "Shot Put";
                    else if (eventTrack == 14) //then it is discuss
                        return "Discuss";
                    else if (eventTrack == 16) //then it is javelin
                        return "Javelin";
                    else
                        return "Throw / Jump";
                }
            }
            else if(i_r.Equals("R"))
            {
                return "Relay";
            }
            return "";
        }
        private string[] getAthleteName(int aId)
        {
            string[] names = new string[2];
            // Open OleDb Connection
            OleDbConnection myConnection = new OleDbConnection();
            myConnection.ConnectionString = myConnectionString;
            myConnection.Open();

            //// Execute Queries
            OleDbCommand cmd = myConnection.CreateCommand();
            int meetID = Convert.ToInt32(meetIDTextBox.Text);

            //gets all the new results from the meet------------------------------------------------------------
            cmd.CommandText = "select First, Last from ATHLETE where ATHLETE=@1";
            cmd.Parameters.AddWithValue("@1", aId);
            OleDbDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                names[0] = Convert.ToString(reader[0]);
                names[1] = Convert.ToString(reader[1]);
            }
            reader.Close();
            cmd.Dispose();
            myConnection.Close();
            return names;
        }

        private string[] getMeetInfo()
        {
            string[] meetInfo = new string[2];
            OleDbConnection myConnection = new OleDbConnection();

            bool invalidDB = true;
            while(invalidDB)
            {
                try
                {
                    myConnection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                      "Data Source=" + getCurrentDataSource() + ";" +
                      "Persist Security Info=True;" +
                      "Jet OLEDB:Database Password=5hY-tek;";
                    myConnection.Open();
                    invalidDB = false;
                }
                catch(Exception e5)
                {
                    MessageBox.Show("Choose a valid Database");
                    setNewDataSource();
                }
            }
            

            //// Execute Queries
            OleDbCommand cmd = myConnection.CreateCommand();
            int meetID = validateMeetID();

            //gets all the new results from the meet------------------------------------------------------------
            cmd.CommandText = "select MNAME, START from MEET where MEET=@1";
            cmd.Parameters.AddWithValue("@1", meetID);
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                meetInfo[0] = Convert.ToString(reader[0]);
                meetInfo[1] = Convert.ToString(reader[1]);
            }
            reader.Close();
            cmd.Dispose();
            myConnection.Close();
            return meetInfo;
        }

        private int validateMeetID()
        {
            int meetID = -1;
            try
            {
                meetID = Convert.ToInt32(meetIDTextBox.Text);
            }
            catch (Exception e6)
            {
                MessageBox.Show("Enter a Meet ID");
            }
            return meetID;
        }

        private void printPreview(List<string> prInfo)
        {
            string newPath = Directory.GetCurrentDirectory().Replace("bin\\Debug", "PersonalRecords\\");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Array.ForEach(Directory.GetFiles(@"" + newPath + "\\AthleteNameAdded\\"), File.Delete);
            string fileNameTemplate = @"" + newPath + "Template.pdf";
            string[] meetInfo = getMeetInfo();
            int textSpacing = 7;
            int textAlignment = PdfContentByte.ALIGN_CENTER;

            string[] prInfoArray = prInfo.ToArray();
            for(int i = 0; i < prInfoArray.Length - 1; i++)
            {
                if(i % 8 == 0)
                {
                    int fileCount_Ath = Directory.GetFiles(newPath + "\\AthleteNameAdded", "*", SearchOption.TopDirectoryOnly).Length;
                    string fileNameAthleteAdded = @"" + newPath + "\\AthleteNameAdded\\testAthleteAddedCopy" + fileCount_Ath + ".pdf";

                    using (var reader = new iTextSharp.text.pdf.PdfReader(@"" + fileNameTemplate))
                    using (var fileStream = new FileStream(@"" + fileNameAthleteAdded, FileMode.Create, FileAccess.Write))
                    {
                        var document = new Document(reader.GetPageSizeWithRotation(1));
                        var writer = PdfWriter.GetInstance(document, fileStream);

                        document.Open();

                        document.NewPage();

                        var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var importedPage = writer.GetImportedPage(reader, 1);

                        var contentByte = writer.DirectContent;
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(baseFont, 12);
                        string meetDate = meetInfo[1].Split(' ')[0];

                        //text to be added---The /NAME/
                        var athleteName = prInfoArray[i] + " " + prInfoArray[i + 1];
                        //adds in the name
                        int x = 225;
                        foreach (var line in athleteName)
                        {
                            contentByte.ShowTextAligned(textAlignment, line.ToString(), x, 593, 0);
                            x = x + textSpacing;
                        }

                        var trackEvent = prInfoArray[i + 2];

                        //adds in the event
                        int x2 = 157;
                        foreach (var line in trackEvent)
                        {
                            contentByte.ShowTextAligned(textAlignment, line.ToString(), x2, 537, 0);
                            x2 = x2 + textSpacing;
                        }

                        //adds in the time/distance
                        contentByte.ShowTextAligned(textAlignment, formatMark(prInfoArray, i, 3), x2 + 30, 537, 0);

                        //adds in the meet name
                        int x5 = 157;
                        foreach (var line in meetInfo[0])
                        {
                            contentByte.ShowTextAligned(textAlignment, line.ToString(), x5, 510, 0);
                            x5 = x5 + textSpacing;
                        }

                        //adds in the meet date
                        int x6 = 240;
                        foreach (var line in meetDate)
                        {
                            contentByte.ShowTextAligned(textAlignment, line.ToString(), x6, 483, 0);
                            x6 = x6 + textSpacing;
                        }


                        //-----------------------second pr-----------------------------------------------------
                        try
                        {
                            var athleteName2 = prInfoArray[i + 4] + " " + prInfoArray[i + 5];
                            //adds in the athlete name
                            int x3 = 225;
                            foreach (var line in athleteName2)
                            {
                                contentByte.ShowTextAligned(textAlignment, line.ToString(), x3, 205, 0);
                                x3 = x3 + textSpacing;
                            }

                            var trackEvent2 = prInfoArray[i + 6];
                            //adds in the event
                            int x4 = 157;
                            foreach (var line in trackEvent2)
                            {
                                contentByte.ShowTextAligned(textAlignment, line.ToString(), x4, 149, 0);
                                x4 = x4 + textSpacing;
                            }

                            //adds in the time/distance
                            contentByte.ShowTextAligned(textAlignment, formatMark(prInfoArray, i, 7), x4 + 30, 149, 0);

                            //adds in the meet name
                            int x7 = 157;
                            foreach (var line in meetInfo[0])
                            {
                                contentByte.ShowTextAligned(textAlignment, line.ToString(), x7, 122, 0);
                                x7 = x7 + textSpacing;
                            }

                            //adds in the meet date
                            int x8 = 240;
                            foreach (var line in meetDate)
                            {
                                contentByte.ShowTextAligned(textAlignment, line.ToString(), x8, 95, 0);
                                x8 = x8 + textSpacing;
                            }

                            contentByte.EndText();
                            contentByte.AddTemplate(importedPage, 0, 0);


                            document.Close();
                            writer.Close();
                        }
                        catch(Exception e)
                        {
                            
                        }
                    }
                }
            }


            string[] files = GetFiles(@"" + newPath + "\\AthleteNameAdded");
            PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument();

            foreach (string file in files)
            {
                PdfSharp.Pdf.PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(file, PdfDocumentOpenMode.Import);
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    // Get the page from the external document...
                    PdfSharp.Pdf.PdfPage page = inputDocument.Pages[idx];
                    // ...and add it to the output document.
                    outputDocument.AddPage(page);
                }
            }

            // Save the document...
            string concatenatedFilePath = @"" + desktopPath + "\\PR_Cards\\";
            if (!System.IO.Directory.Exists(concatenatedFilePath))
                System.IO.Directory.CreateDirectory(concatenatedFilePath);
            string concatenatedFileName = concatenatedFilePath + "PR_Cards_For_" + meetInfo[0].Replace(' ', '_') + ".pdf";
            try
            {
                outputDocument.Save(concatenatedFileName);
                // ...and start a viewer.
                Process.Start(concatenatedFileName);
            }
            catch (Exception e3)
            {
                MessageBox.Show("Please close PDF documents.");
            }

            //empty the temporary folder
            Array.ForEach(Directory.GetFiles(@"" + newPath + "\\AthleteNameAdded\\"), File.Delete);
        }

        private string formatMark(string[] prInfoArray, int blockPosition, int positionOfMark)
        {
            string shortRawMark = prInfoArray[blockPosition + positionOfMark].Substring(0, prInfoArray[blockPosition + positionOfMark].Length - 2);
            string fractionOfMark = prInfoArray[blockPosition + positionOfMark].Substring(prInfoArray[blockPosition + positionOfMark].Length - 2);

            if (Convert.ToInt32(prInfoArray[blockPosition + positionOfMark]) < 0)
            {
                int feet = Math.Abs(Convert.ToInt32(shortRawMark)) / 12;
                int inches = Math.Abs(Convert.ToInt32(shortRawMark)) % 12;
                return feet + "' " + inches + "." + fractionOfMark + "''";
            }
            else
            {
                int minutes = Convert.ToInt32(shortRawMark) / 60;
                int seconds = Convert.ToInt32(shortRawMark) % 60;

                if (minutes != 0)
                    return minutes + ":" + formatSeconds(seconds) + "." + fractionOfMark;
                else
                    return seconds + "." + fractionOfMark;
            }
        }

        private string formatSeconds(int seconds)
        {
            switch(seconds)
            {
                case 1:
                    return "01";
                case 2:
                    return "02";
                case 3:
                    return "03";
                case 4:
                    return "04";
                case 5:
                    return "05";
                case 6:
                    return "06";
                case 7:
                    return "07";
                case 8:
                    return "08";
                case 9:
                    return "09";
                case 0:
                    return "00";
            }
            return seconds + "";                    
        }        

        static string[] GetFiles(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = dirInfo.GetFiles("*.pdf");
            ArrayList list = new ArrayList();
            foreach (FileInfo info in fileInfos)
            {
                // HACK: Just skip the protected samples file...
                if (info.Name.IndexOf("protected") == -1)
                    list.Add(info.FullName);
            }
            return (string[])list.ToArray(typeof(string));
        }

        private void changeDataSource_Click(object sender, EventArgs e)
        {
            setNewDataSource();
            currentDataSourceLabel.Text = getCurrentDataSource();
        }

        private void setNewDataSource()
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                using (StreamWriter wr = new StreamWriter("Data_Source.txt"))
                {
                    wr.WriteLine(file);
                }
            }
        }
        
        private string getCurrentDataSource()
        {
            string currentDataSource = "";
            using (StreamReader sr = new StreamReader("Data_Source.txt"))
            {
                currentDataSource = sr.ReadLine();
            }
            if (currentDataSource == null)
                return "NONE";
            else
                return currentDataSource;
        }
    }
}


