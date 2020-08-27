
IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ROLLING_VOLUMES_FDM' AND XTYPE = 'U')        
DROP TABLE [ROLLING_VOLUMES_FDM]                                                              
                IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME = 'ROLLING_VOLUMES_FDM' AND XTYPE = 'U')   
                   CREATE TABLE [dbo].[ROLLING_VOLUMES_FDM](				                                    
                   	[ZREP] [nvarchar](100) NULL,				                                            
                   	[DMDGROUP] [nvarchar](max) NULL,				                                        
                   	[WeekStartDate] [datetime] NULL,				                                        
                   	[PlanProductIncrementalQty] [float] NULL,			                                    
                   	[ActualsQty] [float] NULL,				                                                
                   	[OpenOrdersQty] [float] NULL,				                                            
                   	[ActualOpenOrdersQty] [float] NULL,				                                        
                   	[BaselineQty] [float] NULL,				                                                
                   	[ActualIncrementalQty] [float] NULL,				                                    
                   	[PreliminaryRollingVolumesQty] [float] NULL,			                                
                   	[PreviousRollingVolumesQty] [float] NULL,			                                    
                   	[PromoDifferenceQty] [float] NULL,             		                                    
                   	[RollingVolumesQty] [float] NULL,	
					[FullWeekDiffQty] [float] NULL
                   ) ON [PRIMARY]                           

