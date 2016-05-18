using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace AngularMVCBoundDataExample.Models
{
    public class WelcomeLetterModel
    {
        private readonly string connectionString = "Data Source=10.0.0.35;Initial Catalog=iMap;integrated security=SSPI;";
        public string GetQuery()
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT A.Validation_File_ID");
            sb.AppendLine(",isNULL(VF.Filename,A.File_Name) as File_Name");
            sb.AppendLine(",Provider");
            sb.AppendLine(",TPA");
            sb.AppendLine(",S.Sponsor");
            sb.AppendLine(",(CASE WHEN S.Sponsor = 'Various' THEN");
            sb.AppendLine("(Select DISTINCT([Sponsor]) + ', ' AS 'data()' ");
            sb.AppendLine("from dbo.ARP_DataFiles df");
            sb.AppendLine("where df.Validation_File_ID = A.Validation_File_ID");
            sb.AppendLine("FOR XML PATH('')) ELSE S.Sponsor END) As SponsorList");
            sb.AppendLine(",Convert(varchar(10),MAX(Funded_Date),101) as Funded_Date");
            sb.AppendLine(",Convert(varchar(10),MAX(Funded_Date),112) as Funded_Date_YMD");
            sb.AppendLine(",MAX(Funded_Date) AS DateFunded");
            sb.AppendLine(",count(*) as Accounts");
            sb.AppendLine(",sum(convert(money,Cash_Amount)) as Funded_Amount");
            sb.AppendLine(",WK.ARP_DataFiles_Welcome_Kits_Batch_ID as BatchId");
            sb.AppendLine("FROM dbo.ARP_DataFiles_Welcome_Kits as WK");
            sb.AppendLine("inner join dbo.ARP_DataFiles as A on A.ID=WK.ARP_DataFiles_ID");
            sb.AppendLine("inner join dbo.Validation_File as VF on A.Validation_File_ID=VF.Validation_File_ID");
            sb.AppendLine("inner join dbo.PlanSponsor as S on S.Validation_File_ID=A.Validation_File_ID");
            sb.AppendLine("WHERE WK.Status=(Select Type_ID From dbo.Types Where Table_Name='WelcomeKitStatus' and Type_Name='New Account')");
            sb.AppendLine("and A.Funding_Status=(Select Type_ID From dbo.Types Where Table_Name='ARPFundingStatus' and Type_Name='Funded')");
            sb.AppendLine("and Kit_Source_Type=(Select Type_ID From dbo.Types Where Table_Name='WelcomeKitSource' and Type_Name='File')");
            sb.AppendLine("GROUP BY A.Validation_File_ID");
            sb.AppendLine(", Convert(VARCHAR(10), Funded_Date, 101)");
            sb.AppendLine(", WK.ARP_DataFiles_Welcome_Kits_Batch_ID");
            sb.AppendLine(",isNULL(VF.Filename,A.File_Name)");
            sb.AppendLine(",Provider");
            sb.AppendLine(",TPA");
            sb.AppendLine(",S.Sponsor");
            sb.AppendLine("ORDER BY Convert(VARCHAR(10), Funded_Date, 101) ASC, TPA ASC");

            return sb.ToString();
        }

        public List<WelcomeLetter> GetWelcomeLetters()
        {
            var list = new List<WelcomeLetter>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(GetQuery(), connection))
                    {
                        SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {

                            var wk = new WelcomeLetter();
                            wk.ValidationFileId = reader.GetInt32(reader.GetOrdinal("Validation_File_ID"));
                            wk.FileName = reader.GetString(reader.GetOrdinal("File_Name"));
                            wk.Provider = reader.GetString(reader.GetOrdinal("Provider"));
                            wk.TPA = reader.GetString(reader.GetOrdinal("TPA"));
                            wk.Sponsor = reader.GetString(reader.GetOrdinal("Sponsor"));
                            wk.SponsorList = reader.GetString(reader.GetOrdinal("SponsorList"));
                            wk.FundedDate = reader.GetString(reader.GetOrdinal("Funded_Date"));
                            wk.FundedDateYMD = reader.GetString(reader.GetOrdinal("Funded_Date_YMD"));
                            wk.Accounts = reader.GetInt32(reader.GetOrdinal("Accounts"));
                            wk.FundedAmount = reader.GetDecimal(reader.GetOrdinal("Funded_Amount"));

                            list.Add(wk);



                            //list.Add(
                            //    new WelcomeLetter
                            //    {
                            //        ValidationFileId = reader.GetInt32(reader.GetOrdinal("Validation_File_ID")),
                            //        FileName = reader.GetString(reader.GetOrdinal("File_Name")),
                            //        Provider = reader.GetString(reader.GetOrdinal("Provider")),
                            //        TPA = reader.GetString(reader.GetOrdinal("TPA")),
                            //        Sponsor = reader.GetString(reader.GetOrdinal("Sponsor")),
                            //        SponsorList = reader.GetString(reader.GetOrdinal("SponsorList")),
                            //        FundedDate = reader.GetDateTime(reader.GetOrdinal("Funded_Date")),
                            //        FundedDateYMD = reader.GetString(reader.GetOrdinal("Funded_Date_YMD")),
                            //        Accounts = reader.GetInt32(reader.GetOrdinal("Accounts")),
                            //        FundedAmount = reader.GetDecimal(reader.GetOrdinal("Funded_Amount"))
                            //    });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return list;
            
        }
    }

    public class WelcomeLetter
    {
        public int ValidationFileId { get; set; }
        public string FileName { get; set; }
        public string Provider { get; set; }
        public string TPA { get; set; }
        public string Sponsor { get; set; }
        public string SponsorList { get; set; }
        public string FundedDate { get; set; }
        public string FundedDateYMD { get; set; }
        public int Accounts { get; set; }
        public decimal FundedAmount { get; set; }


    }
}