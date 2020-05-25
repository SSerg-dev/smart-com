namespace Module.Persist.TPM.Migrations
{
    using global::Persist;
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;

    public partial class Create_DayIncrementalQTYRecalculation_Job : DbMigration
    {
        public override void Up()
        {
            string database;
            string userID;

            using (DatabaseContext context = new DatabaseContext())
            {
                string connectionString = context.Database.Connection.ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                database = builder.InitialCatalog;
                userID = builder.UserID;
            }

            Sql($@"
                DECLARE @jobId BINARY(16)
                SELECT @jobId = job_id FROM msdb.dbo.sysjobs WHERE (name = N'DayIncrementalQTYRecalculation_{database}')

                IF (@jobId IS NULL)
                BEGIN
				    BEGIN TRANSACTION
				    DECLARE @ReturnCode INT
				    SELECT @ReturnCode = 0

				    IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
				    BEGIN
				        EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
				        IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
				    END


				    EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'DayIncrementalQTYRecalculation_{database}', 
						    @enabled=1, 
						    @notify_level_eventlog=0, 
						    @notify_level_email=0, 
						    @notify_level_netsend=0, 
						    @notify_level_page=0, 
						    @delete_level=0, 
						    @description=N'No description available.', 
						    @category_name=N'[Uncategorized (Local)]', 
						    @owner_login_name=N'{userID}', @job_id = @jobId OUTPUT
				    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
				    EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Fill CurrentDayIncremental', 
						    @step_id=1, 
						    @cmdexec_success_code=0, 
						    @on_success_action=3, 
						    @on_success_step_id=0, 
						    @on_fail_action=2, 
						    @on_fail_step_id=0, 
						    @retry_attempts=0, 
						    @retry_interval=0, 
						    @os_run_priority=0, @subsystem=N'TSQL', 
						    @command=N'
							    BEGIN TRY
								    EXEC [{database}].[dbo].[FillCurrentDayIncremental] 
							    END TRY 
                                BEGIN CATCH 
                                    INSERT INTO [{database}].[dbo].[JobErrorLog] ( [StepName] ,[ErrorMessage] ,[AdditionalInfo] ) 
	                                VALUES ( ERROR_PROCEDURE(), ERROR_MESSAGE(), null )
                                    ;THROW
                                END CATCH',
						    @database_name=N'master', 
						    @flags=0
				    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
				    EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Fill PromoProductDifference', 
						    @step_id=2, 
						    @cmdexec_success_code=0, 
						    @on_success_action=3, 
						    @on_success_step_id=0, 
						    @on_fail_action=2, 
						    @on_fail_step_id=0, 
						    @retry_attempts=0, 
						    @retry_interval=0, 
						    @os_run_priority=0, @subsystem=N'TSQL', 
						    @command=N'
							    BEGIN TRY
								    EXEC [{database}].[dbo].[FillPromoProductDifference]
							    END TRY 
                                BEGIN CATCH 
                                    INSERT INTO [{database}].[dbo].[JobErrorLog] ( [StepName] ,[ErrorMessage] ,[AdditionalInfo] ) 
	                                VALUES ( ERROR_PROCEDURE(), ERROR_MESSAGE(), null )
                                    ;THROW
                                END CATCH', 
						    @database_name=N'master', 
						    @flags=0
					IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
				    EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Fill PreviousDayIncremental', 
						    @step_id=3, 
						    @cmdexec_success_code=0, 
						    @on_success_action=3, 
						    @on_success_step_id=0, 
						    @on_fail_action=2, 
						    @on_fail_step_id=0, 
						    @retry_attempts=0, 
						    @retry_interval=0, 
						    @os_run_priority=0, @subsystem=N'TSQL', 
						    @command=N'
							    BEGIN TRY
								    EXEC [{database}].[dbo].[FillPreviousDayIncremental];
							    END TRY 
                                BEGIN CATCH 
                                    INSERT INTO [{database}].[dbo].[JobErrorLog] ( [StepName] ,[ErrorMessage] ,[AdditionalInfo] ) 
	                                VALUES ( ERROR_PROCEDURE(), ERROR_MESSAGE(), null )
                                    ;THROW
                                END CATCH', 
						    @database_name=N'master', 
						    @flags=0
				    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
                    EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SetJobFlag', 
		                    @step_id=4, 
		                    @cmdexec_success_code=0, 
		                    @on_success_action=1, 
		                    @on_success_step_id=0, 
		                    @on_fail_action=2, 
		                    @on_fail_step_id=0, 
		                    @retry_attempts=0, 
		                    @retry_interval=0, 
		                    @os_run_priority=0, @subsystem=N'TSQL', 
		                    @command=N'
                                BEGIN TRY 
                                    UPDATE [{database}].[dbo].[JobFlag] SET [Value] = 0 WHERE [Prefix] = N''DayIncrementalQTYRecalculation''; 
                                END TRY 
                                BEGIN CATCH 
                                    INSERT INTO [{database}].[dbo].[JobErrorLog] ( [StepName] ,[ErrorMessage] ,[AdditionalInfo] ) 
	                                VALUES ( ERROR_PROCEDURE(), ERROR_MESSAGE(), null )
                                    ;THROW
                                END CATCH', 
		                    @database_name=N'master', 
		                    @flags=0
				    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
				    EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
				    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
				    EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
				    IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
				    COMMIT TRANSACTION
				    GOTO EndSave
				    QuitWithRollback:
					    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
				    EndSave:
                END 
				GO");
        }
        
        public override void Down()
        {
			string database;

			using (DatabaseContext context = new DatabaseContext())
			{
				string connectionString = context.Database.Connection.ConnectionString;
				SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
				database = builder.InitialCatalog;
			}

			Sql($@"EXEC msdb.dbo.sp_delete_job @job_name = N'DayIncrementalQTYRecalculation_{database}'");
		}
    }
}
