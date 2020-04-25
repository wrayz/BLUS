
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BluSenseWorker.Models;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BluSenseWorker.DataAccess
{
    public class MeasurementDataAccess
    {
        private readonly IConfiguration _configuration;

        public MeasurementDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Save(List<Measurement> measurements)
        {
            var builder = new SqlConnectionStringBuilder(this._configuration.GetConnectionString("DefaultConnection"));

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        InsertMeasurements(measurements, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        private void InsertMeasurements(List<Measurement> data, SqlConnection connection, SqlTransaction transaction)
        {
            var insertQuery = "INSERT INTO dbo.Measurements (Test_Item ,Lot_Number ,Patient_ID ,Note ,Sample_Type ,Date ,Time ,Test_1_Type ,Test_1_Result ,Test_1_Value, [Test_1_Lower_CutOff_Value],[Test_1_Higher_CutOff_Value],Test_1_QC_Passed ,Test_1_QC_Code ,Test_2_Type ,Test_2_Result ,Test_2_Value , [Test_2_Lower_CutOff_Value], [Test_2_Higher_CutOff_Value] ,Test_2_QC_Passed ,Test_2_QC_Code ,Test_3_Type ,Test_3_Result ,Test_3_Value ,[Test_3_Lower_CutOff_Value] ,[Test_3_Higher_CutOff_Value] ,Test_3_QC_Passed ,Test_3_QC_Code ,Test_4_Type ,Test_4_Result ,Test_4_Value ,Test_4_Lower_CutOff_Value ,Test_4_Higher_CutOff_Value ,Test_4_QC_Passed ,Test_4_QC_Code ,Internal_QC_Passed ,Camera_Check_Passed ,Electronics_Check_Passed ,Mechanics_Check_Passed ,Optics_Check_Passed ,Optics_Check_Value ,Error_Code ,SN ,Model ,Cartridge_SN ,Test_Item_ID ,SW_Version) " +
                                                    "VALUES (@TestItem ,@LotNumber ,@PatientID ,@Note ,@SampleType ,@Date ,@Time ,@Test1Type ,@Test1Result ,@Test1Value ,@Test1LowerCutoffValue ,@Test1HigherCutoffValue ,@Test1QCPassed ,@Test1QCCode ,@Test2Type ,@Test2Result ,@Test2Value ,@Test2LowerCutoffValue ,@Test2HigherCutoffValue ,@Test2QCPassed ,@Test2QCCode ,@Test3Type ,@Test3Result ,@Test3Value ,@Test3LowerCutoffValue ,@Test3HigherCutoffValue ,@Test3QCPassed ,@Test3QCCode ,@Test4Type ,@Test4Result ,@Test4Value ,@Test4LowerCutoffValue ,@Test4HigherCutoffValue ,@Test4QCPassed ,@Test4QCCode ,@InternalQCPassed ,@CameraCheckPassed ,@ElectronicsCheckPassed ,@MechanicsCheckPassed ,@OpticsCheckPassed ,@OpticsCheckValue ,@ErrorCode ,@SN ,@Model ,@CartridgeSN ,@TestItemID ,@SWVersion)";
            connection.Execute(insertQuery, data, transaction: transaction);
            foreach (var item in data)
            {
                InsertMeasurementTests(item.MeasurementTests, connection, transaction);
            }
        }

        private void InsertMeasurementTests(List<MeasurementTest> data, SqlConnection connection, SqlTransaction transaction)
        {
            var insertQuery = "INSERT INTO dbo.MeasurementTests (Patient_ID, Model, SN, No, Test_Type ,Test_Result ,Test_Value ,Test_Lower_CutOff_Value ,Test_Higher_CutOff_Value ,Test_QC_Passed ,Test_QC_Code, TestDate) " +
                                "VALUES ( @PatientID, @Model, @SN, @No, @TestType ,@TestResult ,@TestValue ,@TestLowerCutoffValue ,@TestHigherCutoffValue ,@TestQCPassed ,@TestQCCode, @TestDate)";
            connection.Execute(insertQuery, data, transaction: transaction);
        }
    }
}