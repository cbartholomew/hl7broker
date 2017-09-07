using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using HL7Importer.Model;

namespace HL7Importer.DAO
{
    public class SelOruDAO
    {
        public const string ACCESSION_NO = "ACCESSIONNO";

        // PROGRAM NOT IMPORTANT ENOUGH TO ADD CONFIG FILE
        public const string CONNNECTION_STRING = "Data Source=SHCTRANSSQL;Initial Catalog=TRANSCRIPTS;User Id=ReadingRadPooledUser;Password=ReadingRadPooledUser";
       
        /// <summary>
        /// IMPORTANT - THE QUERY MUST BE IN THIS FORMAT - THIS HAS TO BE THE ACCESSION NO FROM WHERE EVER YOU ARE PULLING FROM:
        /// SELECT <COLUMN> AS ACCESSIONNO FROM <TABLE> WHERE <COLUMN> <CONDITION> <EXP>
        //  SELECT ID AS ACCESSIONNO FROM SCANS WHERE CREATED DATE BETWEEN 2001-01-01 TO 2005-01-01 
        //  SELECT ID AS ACCESSIONNO FROM SCANS WHERE ID = '1010200101.1' -- CAN BE THE ACTUAL ACCESSION FOR INDIVIDUAL EXAMS
        /// </summary>
        /// <param name="Query"></param>
        /// <returns>
        /// List of SELORU Objects
        /// </returns>
        public static List<SelOru> GetAllAccessionsFromSELORU(string Query)
        {
            if (String.IsNullOrEmpty(Query))
                throw new ArgumentNullException("No query was given. Please provide a query in this format: SELECT <COLUMN> AS ACCESSIONNO FROM <TABLE> WHERE <COLUMN> <CONDITION> <EXP>");

            List<SelOru> selOruList = new List<SelOru>();
            SqlCommand sqlCommand = new SqlCommand();

            using (SqlConnection sqlConnection = new SqlConnection(CONNNECTION_STRING)) 
            {
                try
                {
                    // open 
                    sqlConnection.Open();
                    // set command attributes
                    sqlCommand.CommandText = Query;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Connection = sqlConnection;

                    // run query
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // add a new sel oru object to the list
                            selOruList.Add(new SelOru(reader));
                        }
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e, "GetAllAccessionsFromSELORU()");

                }                          
            }
            return selOruList;
        }
        
    }
}
                    