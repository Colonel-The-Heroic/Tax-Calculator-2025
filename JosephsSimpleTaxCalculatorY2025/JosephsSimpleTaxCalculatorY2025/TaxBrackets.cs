using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using static JosephsSimpleTaxCalculatorY2025.Form1;

namespace JosephsSimpleTaxCalculatorY2025
{
    public class TaxBrackets//gets the necessary tax brackets. I need the list of brackets to be able to factor in marginal brackets
    {
        public List<StateTaxBracket> GetStateTaxBrackets(int stateId, int filingStatusId)//gathers a list of brackets by state and filing status
        {
            using IDbConnection db = new SqlConnection(DbHelper.Conn);

            string query = @"
                SELECT *
                FROM StateTaxBrackets
                WHERE StateId = @StateId
                AND FilingStatusId = @FilingStatusId
                ORDER BY MinValue";//the query string
            
            return db.Query<StateTaxBracket>(query, new
            {
                StateId = stateId,
                FilingStatusId = filingStatusId
            }).ToList();
        }

        public List<FederalTaxBracket> GetFederalTaxBrackets(int filingStatusId)
        {
            using IDbConnection db = new SqlConnection(DbHelper.Conn);

            string query = @"
                SELECT *
                FROM FederalTaxBrackets
                WHERE FilingStatusId = @FilingStatusId
                ORDER BY MinValue";

            return db.Query<FederalTaxBracket>(query, new
            {
                FilingStatusId = filingStatusId
            }).ToList();
        }
    }
}
