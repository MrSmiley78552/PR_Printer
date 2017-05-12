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

namespace PR_Printer
{
    public partial class Form1 : Form
    {
        private OleDbConnection connection = new OleDbConnection();
        
        public Form1()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\TFTM3Data\Test3.mdb";
        }
        
        private void accessDatabaseButton_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                accessDatabaseLabel.Text = "Success";
                connection.Close();
            }
            catch(Exception e2)
            {
                accessDatabaseLabel.Text = "Error";
            }
        }

        private void retrieveDataButton_Click(object sender, EventArgs e)
        {
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM Athlete";

            OleDbDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                var testVariable = reader.GetString(1);
            }

            connection.Close();
        }
        ///<summary>Copies the Template.pdf and
        ///edits the copy to contain the athlete's
        ///name and Rory's signature.
        ///
        /// Have this code execute for each new PR to create a custom PR card.
        ///</summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            string newPath = Directory.GetCurrentDirectory().Replace("bin\\Debug", "PersonalRecords\\");
            string fileNameTemplate = @"" + newPath + "Template.pdf";

            int fileCount_Ath = Directory.GetFiles(newPath + "\\AthleteNameAdded", "*", SearchOption.TopDirectoryOnly).Length;
            string fileNameAthleteAdded = @"" + newPath + "\\AthleteNameAdded\\testAthleteAddedCopy" + fileCount_Ath + ".pdf";

            int fileCount_Sig_Ath = Directory.GetFiles(newPath + "\\Final_SignatureAndAthlete", "*", SearchOption.TopDirectoryOnly).Length;
            string fileNameSignatureAndAthleteAdded = @"" + newPath + "\\Final_SignatureAndAthlete\\testFinal_PR_Card" + fileCount_Sig_Ath + ".pdf";


            using (var reader = new PdfReader(@"" + fileNameTemplate))
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

                //text to be added
                var multiLineString = "Mitchell Kollodge";
                //adds in the text
                int x = 200;
                foreach (var line in multiLineString)
                {
                    contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, line.ToString(), x, 500, 0);
                    x = x + 10;
                }
                contentByte.EndText();
                contentByte.AddTemplate(importedPage, 0, 0);
                

                document.Close();
                writer.Close();

                //add signature here
                using (Stream inputPdfStream = new FileStream(@"" + fileNameAthleteAdded, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Stream inputImageStream = new FileStream("../../Resources/Signature.jpg", FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Stream outputPdfStream = new FileStream(@"" + fileNameSignatureAndAthleteAdded, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var reader2 = new PdfReader(inputPdfStream);
                    var stamper = new PdfStamper(reader2, outputPdfStream);
                    var pdfContentByte = stamper.GetOverContent(1);

                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
                    image.SetAbsolutePosition(100, 100);
                    pdfContentByte.AddImage(image);
                    stamper.Close();
                }
            }

            PrintPDFs(@"" + fileNameSignatureAndAthleteAdded);

            //PrintDialog printDlg = new PrintDialog();
            //PrintDocument printDoc = new PrintDocument();
            //printDoc.DocumentName = "Personal Records";
            //printDlg.Document = printDoc;
            //printDlg.AllowSelection = true;
            //printDlg.AllowSomePages = true;
            //if (printDlg.ShowDialog() == DialogResult.OK)
            //    printDoc.Print();
        }

        public static Boolean PrintPDFs(string pdfFileName)
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.StartInfo.Verb = "print";

                //Define location of adobe reader/command line
                //switches to launch adobe in "print" mode
                proc.StartInfo.FileName =
                  @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe";
                proc.StartInfo.Arguments = String.Format(@"/p /h {0}", pdfFileName);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;

                proc.Start();
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (proc.HasExited == false)
                {
                    proc.WaitForExit(10000);
                }

                proc.EnableRaisingEvents = true;

                proc.Close();
                //For whatever reason, sometimes adobe likes to be a stage 5 clinger.
                //So here we kill it with fire.
                KillAdobe("AcroRd32");
                return true;
            }
            catch
            {
                return false;
            }
        }


        private static bool KillAdobe(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses().Where(
                         clsProcess => clsProcess.ProcessName.StartsWith(name)))
            {
                clsProcess.Kill();
                return true;
            }
            return false;
        }
    }
}


