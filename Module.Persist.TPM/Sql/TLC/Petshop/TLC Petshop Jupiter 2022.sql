CREATE PROCEDURE Add_TLC
(
	@number int, --A
	@GA bit, --B
	@brandName NVARCHAR(255),	--C
	@techName NVARCHAR(255),	--D
	@subrangeName NVARCHAR(255),	--E
	@clientName NVARCHAR(255),	--F

	@planMarsMechanicName NVARCHAR(255),	--G
	@planMarsMechanicType NVARCHAR(255),	--H
	@planMarsMechanicDiscount INT,	--I

	@actualMarsMechanicName NVARCHAR(255),	--J
	@actualMarsMechanicType NVARCHAR(255),	--K
	@actualMarsMechanicDiscount INT,	--L

	@eventName NVARCHAR(255),	--M
	@mechanicComment NVARCHAR(255),	--N

	@startDate NVARCHAR(255),	--O
	@endDate NVARCHAR(255),	--P
	@startDateMars NVARCHAR(255),	--Q	
	@endDateMars NVARCHAR(255),	--R
	
	@dispatchesStart NVARCHAR(255),	--S
	@dispatchesEnd NVARCHAR(255),	--T
	@dispatchesStartMars NVARCHAR(255),	--U	
	@dispatchesEndMars NVARCHAR(255),	--V

	@TPRVPCompensation FLOAT,	--W

	@planPromoBaselineLSV FLOAT,	--X
	@planPromoUpliftPercent FLOAT,	--Y
	@planPromoIncrementalLSV FLOAT,	--Z
	@planPromoLSV FLOAT,	--AA

	@actualPromoBaselineLSV FLOAT,	--AB
	@actualPromoUpliftPercent FLOAT,	--AC
	@actualPromoIncrementalLSV FLOAT,	--AD
	@actualPromoLSV FLOAT,	--AE

	@baseClientsObjIds NVARCHAR(255),	--AF
	@baseClientId INT,	--AG
	@statusName NVARCHAR(255),	--AH
	@creatorName NVARCHAR(255),	--AI
	@needRecountUplift BIT,	--AJ
	
	@planRoi FLOAT, --AK
	@actualRoi FLOAT, --AL
	
	@planBaseTI FLOAT, --AM
	@planIncBaseTI FLOAT, --AN
	@planShopperTI FLOAT, --AP
	@planIncNSV FLOAT, --AQ
	@planCOGS FLOAT, --AR
	@planIncCOGS FLOAT, --AS
	@planIncMAC FLOAT, --AT
	@planIncEarnings FLOAT, --AU
	@planPromoCost FLOAT, --AV

	@actualBaseTI FLOAT, --AW
	@actualIncBaseTI FLOAT, --AX
	@actualShopperTI FLOAT, --AY
	@actualIncNSV FLOAT, --AZ
	@actualCOGS FLOAT, --BA
	@actualIncCOGS FLOAT, --BB
	@actualIncMAC FLOAT, --BC
	@actualIncEarnings FLOAT, --BD
	@actualPromoCost FLOAT --BE
)
AS BEGIN
BEGIN TRANSACTION
	DECLARE @promoId UNIQUEIDENTIFIER;
	SET @promoId = NEWID();

	DECLARE @brandId UNIQUEIDENTIFIER;
	SELECT @brandId = Id FROM Jupiter.Brand WHERE LOWER([Name]) = LOWER(@brandName) AND [Disabled] = 'FALSE';
	
	DECLARE @techId UNIQUEIDENTIFIER;
	IF (@techName = 'Premium Pouch') BEGIN
		SELECT @techId = Id FROM Jupiter.Technology WHERE LOWER([Name]) = 'Pouch' AND [Disabled] = 'FALSE' AND SubBrand = 'Prem';
	END
	ELSE IF (@techName = 'Pouch' AND @brandName = 'Whiskas') BEGIN
		SELECT @techId = Id FROM Jupiter.Technology WHERE LOWER([Name]) = 'Pouch' AND [Disabled] = 'FALSE' AND SubBrand = 'Core';
	END
	ELSE BEGIN
		SELECT @techId = Id FROM Jupiter.Technology WHERE LOWER([Name]) = LOWER(@techName) AND [Disabled] = 'FALSE' AND (SubBrand IS NULL OR SubBrand = '');
	END

	DECLARE @brandTechId UNIQUEIDENTIFIER;
	SELECT @brandTechId = Id FROM Jupiter.BrandTech WHERE BrandId = @brandId AND TechnologyId = @techId AND [Disabled] = 'FALSE';
	
	DECLARE @promoStatusId UNIQUEIDENTIFIER;
	SELECT @promoStatusId = Id FROM Jupiter.PromoStatus WHERE LOWER(SystemName) = LOWER(@statusName) AND [Disabled] = 'FALSE';

	---------- поиск клиента и запись базовых ----------
	DECLARE @baseClientTreeId INT;
	DECLARE @clientTreeObjectId INT;
	DECLARE @clietnFullPath NVARCHAR(255);
	DECLARE @isBaseClient BIT;
	DECLARE @clientTreeKeyId INT;
	SELECT @clientTreeKeyId=Id, @baseClientTreeId = ObjectId, @clientTreeObjectId = ObjectId, @clietnFullPath = FullPathName, @isBaseClient = IsBaseClient
		FROM Jupiter.ClientTree WHERE LOWER([Name]) = LOWER(@clientName) AND [EndDate] IS NULL;

	IF (@isBaseClient = 0) BEGIN
		SET @baseClientTreeId = NULL;
	END
	print(@isBaseClient)
	
		-------формируем им€ промо и ищем выбранный узел в дереве продуктов -------
	DECLARE @promoName NVARCHAR(255);
	DECLARE @productFullPath NVARCHAR(255);
	DECLARE @productObjectId int;

	SELECT @promoName = Abbreviation, @productObjectId = ObjectId, @productFullPath  = FullPathName FROM Jupiter.ProductTree 
		WHERE BrandId = @brandId AND [EndDate] IS NULL AND CHARINDEX('test', Abbreviation, 1) = 0;

	IF(@techId IS NOT NULL) BEGIN 
		SELECT @promoName = CONCAT(@promoName, ' ', Abbreviation), @productFullPath = FullPathName, @productObjectId = ObjectId FROM Jupiter.ProductTree 
			WHERE TechnologyId = @techId AND parentId = @productObjectId AND [EndDate] IS NULL;
	END

	-----------ищем цвет----------------
	DECLARE @colorId UNIQUEIDENTIFIER;
	SELECT @colorId = Id FROM Jupiter.Color Where BrandTechId = @brandTechId AND [Disabled] = 'FALSE';

	-----------ищем событие-------------
	DECLARE @eventId UNIQUEIDENTIFIER;
	DECLARE @eventNameBD NVARCHAR(255);
	
	IF (LEN(@eventName) > 1) BEGIN
		SELECT @eventId = Id, @eventNameBD = [name] FROM Jupiter.[Event] Where LOWER([Name]) = LOWER(@eventName) AND [Disabled] = 'FALSE';	
	END
	ELSE BEGIN
		print(N'ѕрименено стандарное событие')
		SELECT @eventId = Id, @eventNameBD = [name] FROM Jupiter.[Event] Where LOWER([Name]) = 'standard promo' AND [Disabled] = 'FALSE';
	END

	IF(@eventId IS NULL) BEGIN
		print(N'ѕромо є' + CAST(@number AS NVARCHAR) + N' событие ' + @eventName + N' не найдено');
		print(N'ѕрименено стандарное событие')
		SELECT @eventId = Id, @eventNameBD = [name] FROM Jupiter.[Event] Where LOWER([Name]) = 'standard promo' AND [Disabled] = 'FALSE';	
	END
	
		
		--“ип промо
	DECLARE @promoTypeId UNIQUEIDENTIFIER;
	Select TOP (1) @promoTypeId = id FROM Jupiter.[PromoTypes] WHERE SystemName = 'Regular';

	-----------ищем механики-------------
	DECLARE @mechanicId UNIQUEIDENTIFIER;
	DECLARE @mechanicName NVARCHAR(255);
	DECLARE @promoMechanicName NVARCHAR(255);
	
	SELECT @mechanicId = Id, @mechanicName = [SystemName] FROM Jupiter.[Mechanic] 
		Where LOWER([SystemName]) = LOWER(@planMarsMechanicName) AND [Disabled] = 'FALSE' AND PromoTypesId = @promoTypeId;	
	
	DECLARE @marsMechanicTypeId UNIQUEIDENTIFIER;
	DECLARE @marsMechanicTypeName NVARCHAR(255);
	IF (LEN(@planMarsMechanicType) > 1) BEGIN
		SELECT @marsMechanicTypeId = Id, @planMarsMechanicDiscount = Discount, @marsMechanicTypeName=[Name] FROM Jupiter.MechanicType 
			WHERE LOWER(NAME) = LOWER(@planMarsMechanicType) AND [Disabled] = 'FALSE';
	END

	IF(@mechanicId IS NULL) BEGIN
		print(N'ѕромо є' + CAST(@number AS NVARCHAR) + N' механика ' + @planMarsMechanicName + N' не найдена');
	END
	ELSE BEGIN
		SET @promoName = CONCAT(@promoName, ' ', @mechanicName, ' ', @planMarsMechanicDiscount, '%');
		SET @promoMechanicName = CONCAT(IIF(@mechanicName IS NOT NULL, CONCAT(@mechanicName, ' '), ''), 
										  IIF(@planMarsMechanicType IS NOT NULL, CONCAT(@planMarsMechanicType, ' '), ''),
										  IIF(@planMarsMechanicDiscount IS NOT NULL, CONCAT(@planMarsMechanicDiscount, '%'), ''));
	END
	
	--заполнение Actual Discount In Store
	DECLARE @actualInstoreMechanicId UNIQUEIDENTIFIER;
	DECLARE @actualInstoreMechanicTypeId UNIQUEIDENTIFIER;
	DECLARE @actualInStoreMechanicDiscount INT;
	IF(LEN(@actualMarsMechanicName) > 1) BEGIN
		SELECT @actualInstoreMechanicId = Id FROM Jupiter.[Mechanic] 
			Where LOWER([SystemName]) = LOWER(@actualMarsMechanicName) AND [Disabled] = 'FALSE' AND PromoTypesId = @promoTypeId;
	END
	IF(LEN(@actualMarsMechanicType) > 1) BEGIN
		SELECT @actualInstoreMechanicTypeId = Id, @actualInStoreMechanicDiscount = Discount FROM Jupiter.MechanicType 
			WHERE LOWER(NAME) = LOWER(@actualMarsMechanicType) AND [Disabled] = 'FALSE';
		print(N'ѕрисутствует ActualInStoreMarsMechanicType')
	END
	ELSE BEGIN
		SET @actualInStoreMechanicDiscount = @actualMarsMechanicDiscount;
		print(N'ќтсутствует ActualInStoreMarsMechanicType')
	END
	
	--------корректировка baseline--------
	DECLARE @correctPlanPromoBaselineLSV FLOAT = @planPromoBaselineLSV * ((DATEDIFF (DAY, convert(datetime, @startDate, 5), convert(datetime, @endDate, 5)) + 1) / 7.0);
	DECLARE @correctActualPromoBaselineLSV FLOAT = @actualPromoBaselineLSV * ((DATEDIFF (DAY, convert(datetime, @startDate, 5), convert(datetime, @endDate, 5)) + 1) / 7.0);

		------кто-то должен быть создателем-----
	DECLARE @creatorId UNIQUEIDENTIFIER;
	Select TOP (1) @creatorId = [User].Id FROM Jupiter.[User] Where LOWER([User].Name) = LOWER(@creatorName) AND [User].Disabled = 'FALSE';



		INSERT INTO [Jupiter].[Promo] (
			[Id] --#1
			,[Disabled] --#2
			,[DeletedDate] --#3
			,[BrandId] --#4
			,[BrandTechId] --#5
			,[PromoStatusId] --#6
			,[Name] --#7
			,[StartDate] --#8
			,[EndDate] --#9
			,[DispatchesStart] --#10
			,[DispatchesEnd] --#11
			,[ColorId] --#12
			,[EventId] --#13
			,[MarsMechanicId] --#14
			,[MarsMechanicTypeId] --#15
			,[MarsMechanicDiscount] --#16
			,[ClientHierarchy] --#17
			,[ProductHierarchy] --#18
			,[CreatorId] --#19
			,[MarsStartDate] --#20
			,[MarsEndDate] --#21
			,[MarsDispatchesStart] --#22
			,[MarsDispatchesEnd] --#23
			,[ClientTreeId] --#24
			,[ClientTreeKeyId] --#24.2
			,[BaseClientTreeId] --#25
			,[Mechanic] --#26
			,[MechanicIA] --#27
			,[BaseClientTreeIds] --#28        
			,[TechnologyId] --#29
			,[NeedRecountUplift] --#30    
			,[PlanPromoCostProdXSites] --#31
			,[PlanPromoCostProdCatalogue] --#32    
			,[CalendarPriority]	--#33
			,[PlanPromoTIShopper] --#34
			,[ActualInStoreShelfPrice] --#41
			,[ActualPromoIncrementalLSV] --#42
			,[ActualPromoLSV] --#43
			,[PlanPromoBaselineLSV] --#44
			,[PlanPromoLSV] --#45
			,[PlanPromoUpliftPercent] --#46
			,[PlanPromoIncrementalLSV] --#47
			,[ActualInStoreDiscount] --#48
			,[ActualPromoUpliftPercent] --#49
			,[ActualPromoNetUpliftPercent] --#50
			,[ActualPromoBaselineLSV] --#52
			,[LoadFromTLC] --#57
			
			,[PromoDuration] --#61
			,[DispatchDuration] --#62
			,[EventName] --#63
			,[ProductSubrangesList] --#64
			,[InOut] --#65
			,[ActualInstoreMechanicId] --#66
			,[ActualInstoreMechanicTypeId] --#67
			,[IsGrowthAcceleration] --#68
			,[MechanicComment] --#69
			,[PromoTypesId] --#70
			,[BudgetYear] --#71

			,[PlanPromoROIPercent] --#72
			,[ActualPromoROIPercent] --#73

			,[PlanPromoBaseTI] --#74
			,[PlanPromoIncrementalBaseTI] --#75
			,[PlanPromoNSV] --#77
			,[PlanCOGSPercent] --#78
			,[PlanPromoIncrementalCOGS] --#79
			,[PlanPromoIncrementalMAC] --#80
			,[PlanPromoIncrementalEarnings] --#81
			,[PlanPromoCost] --#82

			,[ActualPromoBaseTI] --#83
			,[ActualPromoIncrementalBaseTI] --#84
			,[ActualPromoTIShopper] --#85
			,[ActualPromoNSV] --#86
			,[ActualCOGSPercent] --#87
			,[ActualPromoIncrementalCOGS] --#88
			,[ActualPromoIncrementalMAC] --#89
			,[ActualPromoIncrementalEarnings] --#90
			,[ActualPromoCost] --#91
			,[SumInvoice] --#92
			,[ManualInputSumInvoice] --#93
		   )
         
     VALUES (
			@promoId, --#1
			'FALSE', --#2
			NULL, --#3
			@brandId, --#4
			@brandTechId, --#5
			@promoStatusId, --#6
			@promoName, --#7
            convert(datetime, @startDate, 5), --#8
            convert(datetime, @endDate, 5), --#9
            convert(datetime, @dispatchesStart, 5), --#10
            convert(datetime, @dispatchesEnd, 5), --#11
            @colorId, --#12
            @eventId, --#13
            @mechanicId, --#14
			@marsMechanicTypeId, --#15
            @planMarsMechanicDiscount, --#16
            @clietnFullPath, --#17
            @productFullPath, --#18
            @creatorId, --#19
            @startDateMars, --#20
            @endDateMars, --#21
            @dispatchesStartMars, --#22
            @dispatchesEndMars, --#23
            @clientTreeObjectId, --#24
			@clientTreeKeyId, --#24.2
            @clientTreeKeyId, --#25
            @promoMechanicName, --#26
            '', --#27
            @baseClientTreeId, --#28
            @techId, --#29
            @needRecountUplift, --#30
            NULL, --#31
            NULL, --#32    
			3, --#33
			@planShopperTI, --34
			0, --#41
			@actualPromoIncrementalLSV, --#42
			@actualPromoLSV, --#43
			@planPromoBaselineLSV * ((DATEDIFF (DAY, convert(datetime, @startDate, 5), convert(datetime, @endDate, 5)) + 1) / 7.0), --#44
			@planPromoLSV, --#45
			@planPromoUpliftPercent, --#46
			@planPromoIncrementalLSV, --#47
			@actualInstoreMechanicDiscount, --#48
			@actualPromoUpliftPercent, --#49
			@actualPromoUpliftPercent, --#50
			@actualPromoBaselineLSV * ((DATEDIFF (DAY, convert(datetime, @startDate, 5), convert(datetime, @endDate, 5)) + 1) / 7.0), --#52
			1, --#57
			
			DATEDIFF(DAY, convert(datetime, @startDate, 5), convert(datetime, @endDate, 5)) + 1, --#61
			DATEDIFF(DAY, convert(datetime, @dispatchesStart, 5), convert(datetime, @dispatchesEnd, 5)) + 1, --#62
			@eventNameBD, --#63
			@subrangeName, --#64
			0, --#65
			@actualInstoreMechanicId, --#66
			@actualInstoreMechanicTypeId, --#67
			@GA, --#68
			@mechanicComment, --#69
			@promoTypeId, --#70
			2022, --#71
			
			@planRoi, --72
			@actualRoi, --73
	
			@planBaseTI, --74
			@planIncBaseTI, --75
			@planIncNSV, --77
			@planCOGS, --78
			@planIncCOGS, --79
			@planIncMAC, --80
			@planIncEarnings, --81
			@planPromoCost, --82

			@actualBaseTI, --83
			@actualIncBaseTI, --84
			@TPRVPCompensation, --85
			@actualIncNSV, --86
			@actualCOGS, --87
			@actualIncCOGS, --88
			@actualIncMAC, --89
			@actualIncEarnings, --90
			@actualPromoCost, --91
			@TPRVPCompensation, --#92
			1 --#93
		)

			------выбираем узел в дереве продуктов-----
		------список Subrange через ; без пробелов!!-------
	IF(CHARINDEX(';', @subrangeName) > 0) BEGIN
		DECLARE @subrange NVARCHAR(255)
		DECLARE @pos INT
		DECLARE @parentProductObjectId INT = @productObjectId
		WHILE CHARINDEX(';', @subrangeName) > 0 BEGIN
			SELECT @pos  = CHARINDEX(';', @subrangeName)  
			SELECT @subrange = SUBSTRING(@subrangeName, 1, @pos-1)
			SELECT @subrangeName = SUBSTRING(@subrangeName, @pos+1, LEN(@subrangeName)-@pos)

			SELECT @productObjectId = IIF(ObjectId IS NOT NULL, ObjectId, @productObjectId), @productFullPath = IIF(ObjectId IS NOT NULL, FullPathName, @productFullPath) FROM Jupiter.ProductTree 
			WHERE [Name] = @subrange AND parentId = @parentProductObjectId AND [EndDate] IS NULL;
			INSERT INTO Jupiter.PromoProductTree ([Disabled], [DeletedDate], [PromoId], [ProductTreeObjectId]) VALUES ('FALSE', NULL, @promoId, @productObjectId);
		END
	END ELSE BEGIN
		SELECT @productObjectId = IIF(ObjectId IS NOT NULL, ObjectId, @productObjectId), @productFullPath = IIF(ObjectId IS NOT NULL, FullPathName, @productFullPath) FROM Jupiter.ProductTree 
			WHERE [Name] = @subrangeName AND parentId = @productObjectId AND [EndDate] IS NULL;
		INSERT INTO Jupiter.PromoProductTree ([Disabled], [DeletedDate], [PromoId], [ProductTreeObjectId]) VALUES ('FALSE', NULL, @promoId, @productObjectId);
	END

	IF (@@error <> 0 OR (@mechanicId IS NULL) OR (@eventId IS NULL)) BEGIN
		print('ROLLBACK ' + CAST(@number AS NVARCHAR))
        ROLLBACK
	END
	ELSE BEGIN
		print('COMMIT ' + CAST(@number AS NVARCHAR))
		COMMIT
	END
END
GO

Add_TLC '10001','0','Whiskas','Pouch','Core','Petshop','TPR','','20','TPR','','0','','','30.12.21','30.01.22','2021P13W4D5','2022P2W1D1','16.12.21','29.01.22','2021P13W2D5','2022P1W4D7',0,360000,25.7434031093204,423662.862599102,2069377.14831339,274784.308368279,43.226676646727,542994.854648235,1799151.69290323,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,470265.777485003,0,0,470265.777485003,470265.777485003,0,0,0,0,604353.273223486,0,0,604353.273223486,604353.273223486,0
GO
Add_TLC '10002','0','Natures Table Cats','Pouch','0','Petshop','TPR','','15','TPR','','0','','','30.12.21','30.01.22','2021P13W4D5','2022P2W1D1','16.12.21','29.01.22','2021P13W2D5','2022P1W4D7',0,60000,40.3940737634413,110795.173751153,385080.888036867,65908.8232085253,37.0007093540409,111482.203949966,412779.681474654,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,122982.64286378,0,0,122982.64286378,122982.64286378,0,0,0,0,124079.692996313,0,0,124079.692996313,124079.692996313,0
GO
Add_TLC '10003','0','Cesar','Pouch','0','Petshop','TPR','','20','TPR','','0','','','30.12.21','30.01.22','2021P13W4D5','2022P2W1D1','16.12.21','29.01.22','2021P13W2D5','2022P1W4D7',0,140000,14.9869427188937,95916.4334009197,735916.43340092,68000,24.8593966487756,77277.2101539083,388134.353011051,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,106467.241075021,0,0,106467.241075021,106467.241075021,0,0,0,0,86009.5349012999,0,0,86009.5349012999,86009.5349012999,0
GO
Add_TLC '10004','0','Pedigree','Dry','13kg','Petshop','TPR','','15','TPR','','0','','','30.12.21','30.01.22','2021P13W4D5','2022P2W1D1','16.12.21','29.01.22','2021P13W2D5','2022P1W4D7',0,230000,19.4831528050491,204851.435207373,1256280.00663594,166517.70539913,26.9006104611648,204773.848129325,965997.644239631,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,227385.093080184,0,0,227385.093080184,227385.093080184,0,0,0,0,227913.292967938,0,0,227913.292967938,227913.292967938,0
GO
Add_TLC '10005','0','Pedigree','C&T','0','Petshop','TPR','','15','TPR','','0','','','30.12.21','30.01.22','2021P13W4D5','2022P2W1D1','16.12.21','29.01.22','2021P13W2D5','2022P1W4D7',0,100000,31.5608322903227,144278.090470047,601420.947612904,138959.297112903,25.6112224812276,162693.370248849,797935.871336406,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,160148.680421752,0,0,160148.680421752,160148.680421752,0,0,0,0,181077.721086969,0,0,181077.721086969,181077.721086969,0
GO
Add_TLC '10006','0','Dreamies','C&T','0','Petshop','TPR','','15','TPR','','0','','','31.01.22','27.02.22','2022P2W1D2','2022P3W1D1','17.01.22','26.02.22','2022P1W3D2','2022P2W4D7',0,140000,38.5616481278801,215945.229516129,775945.229516129,154779.634274193,25.3306407452701,156826.692419355,775945.229516127,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,239699.204762903,0,0,239699.204762903,239699.204762903,0,0,0,0,174548.108662742,0,0,174548.108662742,174548.108662742,0
GO
Add_TLC '10007','0','Perfect Fit Cat','Pouch','0','Petshop','TPR','','20','TPR','','0','','','31.01.22','27.02.22','2022P2W1D2','2022P3W1D1','17.01.22','26.02.22','2022P1W3D2','2022P2W4D7',0,400000,30.8746779809908,493994.847695853,2093994.84769585,378742.064758284,55.8554464770299,846192.285068245,2361160.54410138,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,548334.280942397,0,0,548334.280942397,548334.280942397,0,0,0,0,941812.013280957,0,0,941812.013280957,941812.013280957,0
GO
Add_TLC '10008','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','20','TPR','','0','','','31.01.22','27.02.22','2022P2W1D2','2022P3W1D1','17.01.22','26.02.22','2022P1W3D2','2022P2W4D7',0,850000,24.2033557319056,822914.09488479,4222914.09488479,741624.900163979,42.3534354749643,1256414.49422888,4222914.09488479,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,913434.645322117,0,0,913434.645322117,913434.645322117,0,0,0,0,1398389.33207674,0,0,1398389.33207674,1398389.33207674,0
GO
Add_TLC '10009','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','20','TPR','','0','','','31.01.22','27.02.22','2022P2W1D2','2022P3W1D1','17.01.22','26.02.22','2022P1W3D2','2022P2W4D7',0,70000,45.4538542864821,0,280000,46755.8343951613,45.4538542864821,85009.315345622,272032.652926267,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,94615.3679796773,0,0,94615.3679796773,94615.3679796773,0
GO
Add_TLC '10010','0','Sheba','Pouch','Core','Petshop','TPR','','25','TPR','','0','','','31.01.22','27.02.22','2022P2W1D2','2022P3W1D1','17.01.22','26.02.22','2022P1W3D2','2022P2W4D7',0,500000,35.8014975311061,716029.950622121,2716029.95062212,469972.61025336,55.1932475900557,1037572.58553034,2917463.02654378,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,794793.245190555,0,0,794793.245190555,794793.245190555,0,0,0,0,1154818.28769527,0,0,1154818.28769527,1154818.28769527,0
GO
Add_TLC '10011','0','Natures Table Cats','Pouch','0','Petshop','TPR','','15','TPR','','0','','','28.02.22','07.03.22','2022P3W1D2','2022P3W2D2','14.02.22','06.03.22','2022P2W3D2','2022P3W2D1',0,60000,62.8152741167424,43073.330822909,111644.759394338,65908.8232085253,48.2186446585045,36320.3900131666,111644.759394338,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,47811.397213429,0,0,47811.397213429,47811.397213429,0,0,0,0,40424.5940846545,0,0,40424.5940846545,40424.5940846545,0
GO
Add_TLC '10012','0','Natures Table Cats','Dry','0','Petshop','TPR','','15','TPR','','0','','','28.02.22','07.03.22','2022P3W1D2','2022P3W2D2','14.02.22','06.03.22','2022P2W3D2','2022P3W2D1',0,20000,63.0655,14414.9714285714,37272.1142857143,12801.1406394009,174.528755890159,25533.3388545096,40163.2138709678,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,16000.6182857143,0,0,16000.6182857143,16000.6182857143,0,0,0,0,28418.6061450692,0,0,28418.6061450692,28418.6061450692,0
GO
Add_TLC '10013','0','Natures Table Dog','Dry','0','Petshop','TPR','','15','TPR','','0','','','28.02.22','07.03.22','2022P3W1D2','2022P3W2D2','14.02.22','06.03.22','2022P2W3D2','2022P3W2D1',0,20000,42.039866359447,9609.11231073074,32466.2551678736,8434.96185099846,180.657002107313,17415.2562387535,27055.2126398946,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,10666.1146649111,0,0,10666.1146649111,10666.1146649111,0,0,0,0,19383.1801937327,0,0,19383.1801937327,19383.1801937327,0
GO
Add_TLC '10014','0','Cesar','Dry','0','Petshop','TPR','','20','TPR','','0','','','28.02.22','07.03.22','2022P3W1D2','2022P3W2D2','14.02.22','06.03.22','2022P2W3D2','2022P3W2D1',0,140000,221.3408,354145.28,514145.28,68000,104.747148437864,81403.498214569,159117.783928855,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,393101.2608,0,0,393101.2608,393101.2608,0,0,0,0,90602.0935128152,0,0,90602.0935128152,90602.0935128152,0
GO
Add_TLC '10015','0','Sheba','Pouch','0','Petshop','TPR','','15','TPR','','0','','','28.02.22','07.03.22','2022P3W1D2','2022P3W2D2','14.02.22','06.03.22','2022P2W3D2','2022P3W2D1',0,600000,112.687733456221,772715.886556944,1458430.17227123,600000,112.687733456221,772715.886556944,1458430.17227123,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,857714.634078208,0,0,857714.634078208,857714.634078208,0,0,0,0,860032.781737878,0,0,860032.781737878,860032.781737878,0
GO
Add_TLC '10016','0','Whiskas','Dry','5kg, 13kg','Petshop','TPR','','15','TPR','','0','','','28.02.22','07.03.22','2022P3W1D2','2022P3W2D2','14.02.22','06.03.22','2022P2W3D2','2022P3W2D1',0,300000,234.286704820277,803268.702240948,1146125.84509809,271967.830999424,83.2875289175126,258874.612445688,569694.990730744,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,891628.259487452,0,0,891628.259487452,891628.259487452,0,0,0,0,288127.443652051,0,0,288127.443652051,288127.443652051,0
GO
Add_TLC '10017','0','Perfect Fit Cat','Pouch','0','Petshop','TPR','','15','TPR','','0','','','25.04.22','22.05.22','2022P5W1D2','2022P6W1D1','11.04.22','21.05.22','2022P4W3D2','2022P5W4D7',0,400000,28.722589719086,459561.435505376,2059561.43550538,378742.064758284,34.7159736514338,525935.981633529,2040904.24066667,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,510113.193410967,0,0,510113.193410967,510113.193410967,0,0,0,0,585366.747558118,0,0,585366.747558118,585366.747558118,0
GO
Add_TLC '10018','0','Sheba','Pouch','Core','Petshop','TPR','','15','TPR','','0','','','23.05.22','19.06.22','2022P6W1D2','2022P7W1D1','09.05.22','18.06.22','2022P5W3D2','2022P6W4D7',0,500000,30.6143548935481,612287.097870962,2612287.09787096,571104.464516129,21.8356457598312,498817.39116129,2783235.24922581,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,679638.678636768,0,0,679638.678636768,679638.678636768,0,0,0,0,555183.756362516,0,0,555183.756362516,555183.756362516,0
GO
Add_TLC '10019','0','Whiskas','Pouch','0','Petshop','TPR','','20','TPR','','0','','','20.06.22','17.07.22','2022P7W1D2','2022P8W1D1','06.06.22','16.07.22','2022P6W3D2','2022P7W4D7',0,360000,88.9380901433694,1280708.49806452,2720708.49806452,496528.890089286,39.3092954944263,780728.034481569,2766843.59483871,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,1421586.43285162,0,0,1421586.43285162,1421586.43285162,0,0,0,0,868950.302377987,0,0,868950.302377987,868950.302377987,0
GO
Add_TLC '10020','0','Whiskas','Dry','5kg, 13kg','Petshop','TPR','','15','TPR','','0','','','20.06.22','17.07.22','2022P7W1D2','2022P8W1D1','06.06.22','16.07.22','2022P6W3D2','2022P7W4D7',0,300000,51.4835651505376,617802.781806452,1817802.78180645,156027.285120968,56.9148825913673,355210.984548387,979320.125032258,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,685761.087805161,0,0,685761.087805161,685761.087805161,0,0,0,0,395349.825802355,0,0,395349.825802355,395349.825802355,0
GO
Add_TLC '10021','0','Pedigree','Pouch','0','Petshop','TPR','','15','TPR','','0','','','20.06.22','17.07.22','2022P7W1D2','2022P8W1D1','06.06.22','16.07.22','2022P6W3D2','2022P7W4D7',0,33000,7.25051661779082,9570.68193548388,141570.681935484,30950.0327096774,16.1591187812326,20005.0101935485,143805.141032258,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,10623.4569483871,0,0,10623.4569483871,10623.4569483871,0,0,0,0,22265.5763454195,0,0,22265.5763454195,22265.5763454195,0
GO
Add_TLC '10022','0','Dreamies','C&T','0','Petshop','TPR','','15','TPR','','0','','','20.06.22','17.07.22','2022P7W1D2','2022P8W1D1','06.06.22','16.07.22','2022P6W3D2','2022P7W4D7',0,140000,17.0103247465445,95257.8185806491,655257.818580649,154779.634274193,34.5141923718284,213683.76290323,832802.300000002,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,105736.17862452,0,0,105736.17862452,105736.17862452,0,0,0,0,237830.028111295,0,0,237830.028111295,237830.028111295,0
GO
Add_TLC '10023','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','15','TPR','','0','','','18.07.22','14.08.22','2022P8W1D2','2022P9W1D1','04.07.22','13.08.22','2022P7W3D2','2022P8W4D7',0,400000,166.8069775,2668911.64,4268911.64,530232.767833333,18.1673335100456,385316.621247309,2506247.69258064,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,2962491.9204,0,0,2962491.9204,2962491.9204,0,0,0,0,428857.399448255,0,0,428857.399448255,428857.399448255,0
GO
Add_TLC '10024','0','Natures Table Cats','Pouch','0','Petshop','TPR','','15','TPR','','0','','','18.07.22','14.08.22','2022P8W1D2','2022P9W1D1','04.07.22','13.08.22','2022P7W3D2','2022P8W4D7',0,60000,116.6264,279903.36,519903.36,71226.9657258064,25.2695729731522,71995.0003225792,356902.863225805,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,310692.7296,0,0,310692.7296,310692.7296,0,0,0,0,80130.4353590306,0,0,80130.4353590306,80130.4353590306,0
GO
Add_TLC '10025','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','15','TPR','','0','','','18.07.22','14.08.22','2022P8W1D2','2022P9W1D1','04.07.22','13.08.22','2022P7W3D2','2022P8W4D7',0,850000,50.2849223529412,1709687.36,5109687.36,683093.998296083,17.7814104334787,485854.989933947,3218230.98311828,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,1897752.9696,0,0,1897752.9696,1897752.9696,0,0,0,0,540756.603796483,0,0,540756.603796483,540756.603796483,0
GO
Add_TLC '10026','0','Natures Table Cats','Dry','0','Petshop','TPR','','15','TPR','','0','','','18.07.22','14.08.22','2022P8W1D2','2022P9W1D1','04.07.22','13.08.22','2022P7W3D2','2022P8W4D7',0,20000,7.224975,5779.98,85779.98,12801.1406394009,11.0358676702293,5650.86776497695,56855.4303225806,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,6415.7778,0,0,6415.7778,6415.7778,0,0,0,0,6289.41582241934,0,0,6289.41582241934,6289.41582241934,0
GO
Add_TLC '10027','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','15','TPR','','0','','','18.07.22','14.08.22','2022P8W1D2','2022P9W1D1','04.07.22','13.08.22','2022P7W3D2','2022P8W4D7',0,70000,18.7016301906166,0,280000,40236.6462903226,18.7016301906166,30099.6351612904,191046.220322581,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,33500.8939345162,0,0,33500.8939345162,33500.8939345162,0
GO
Add_TLC '10028','0','Natures Table Dog','Dry','0','Petshop','TPR','','15','TPR','','0','','','18.07.22','14.08.22','2022P8W1D2','2022P9W1D1','04.07.22','13.08.22','2022P7W3D2','2022P8W4D7',0,20000,32.8034,26242.72,106242.72,11147.6405833333,32.9392468720279,14687.7954086021,59278.3577419355,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,29129.4192,0,0,29129.4192,29129.4192,0,0,0,0,16347.5162897742,0,0,16347.5162897742,16347.5162897742,0
GO
Add_TLC '10029','0','Sheba','Pouch','Core','Petshop','TPR','','15','TPR','','0','','','15.08.22','11.09.22','2022P9W1D2','2022P10W1D1','01.08.22','10.09.22','2022P8W3D2','2022P9W4D7',0,761078.065828062,68.9677245799053,2099592.89711347,5143905.16042572,716525.919806452,34.764566461104,996388.518408602,3862492.19763441,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,2330548.11579596,0,0,2330548.11579596,2330548.11579596,0,0,0,0,1108980.42098877,0,0,1108980.42098877,1108980.42098877,0
GO
Add_TLC '10030','0','Cesar','Pouch','0','Petshop','TPR','','20','TPR','','0','','','15.08.22','11.09.22','2022P9W1D2','2022P10W1D1','01.08.22','10.09.22','2022P8W3D2','2022P9W4D7',0,92498.4910610526,114.928072625698,425226.931937286,795220.896181497,48673.5523387096,24.873700163679,48427.6538709679,243121.863225806,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,472001.894450388,0,0,472001.894450388,472001.894450388,0,0,0,0,53899.9787583872,0,0,53899.9787583872,53899.9787583872,0
GO
Add_TLC '10031','0','Dreamies','C&T','0','Petshop','TPR','','15','TPR','','0','','','15.08.22','11.09.22','2022P9W1D2','2022P10W1D1','01.08.22','10.09.22','2022P8W3D2','2022P9W4D7',0,105191.90694,203.196123702077,854983.509401503,1275751.1371615,206126.506527778,24.2654585459571,200070.16797491,1024576.19408602,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,949031.695435668,0,0,949031.695435668,949031.695435668,0,0,0,0,222678.096956075,0,0,222678.096956075,222678.096956075,0
GO
Add_TLC '10032','0','Perfect Fit Cat','Pouch','0','Petshop','TPR','','15','TPR','','0','','','12.09.22','09.10.22','2022P10W1D2','2022P11W1D1','29.08.22','08.10.22','2022P9W3D2','2022P10W4D7',0,94230.6811440011,102.890378133558,387817.216587561,764739.941163565,530232.767833333,18.0370620511184,382553.653397844,2503484.72473118,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,430477.110412192,0,0,430477.110412192,430477.110412192,0,0,0,0,425782.2162318,0,0,425782.2162318,425782.2162318,0
GO
Add_TLC '10033','0','Natures Table Cats','Pouch','0','Petshop','TPR','','15','TPR','','0','','','12.09.22','09.10.22','2022P10W1D2','2022P11W1D1','29.08.22','08.10.22','2022P9W3D2','2022P10W4D7',0,162381.395729441,73.0227272727273,474301.294980632,1123826.8778984,93021.6710967742,36.3204479284772,135143.550451613,507230.23483871,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,526474.437428501,0,0,526474.437428501,526474.437428501,0,0,0,0,150414.771652645,0,0,150414.771652645,150414.771652645,0
GO
Add_TLC '10034','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','15','TPR','','0','','','12.09.22','09.10.22','2022P10W1D2','2022P11W1D1','29.08.22','08.10.22','2022P9W3D2','2022P10W4D7',0,1049494.09215202,67.4958386762176,2833459.35742145,7031435.72602952,683093.998296083,13.3962077775289,366034.765310292,3098410.75849463,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,3145139.88673781,0,0,3145139.88673781,3145139.88673781,0,0,0,0,407396.693790355,0,0,407396.693790355,407396.693790355,0
GO
Add_TLC '10035','0','Natures Table Cats','Dry','0','Petshop','TPR','','15','TPR','','0','','','12.09.22','09.10.22','2022P10W1D2','2022P11W1D1','29.08.22','08.10.22','2022P9W3D2','2022P10W4D7',0,29225.3909185198,43.6107954545455,50981.701817068,167883.265491147,15942.6303360215,24.4032971829436,15562.1098387098,79332.6311827959,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,56589.6890169454,0,0,56589.6890169454,56589.6890169454,0,0,0,0,17320.628250484,0,0,17320.628250484,17320.628250484,0
GO
Add_TLC '10036','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','15','TPR','','0','','','12.09.22','09.10.22','2022P10W1D2','2022P11W1D1','29.08.22','08.10.22','2022P9W3D2','2022P10W4D7',0,56015.9327264993,126.000808355574,282322.112173214,506385.843079212,40236.6462903226,23.4196041379232,37693.0531182796,198639.63827957,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,313377.544512268,0,0,313377.544512268,313377.544512268,0,0,0,0,41952.3681206452,0,0,41952.3681206452,41952.3681206452,0
GO
Add_TLC '10037','0','Natures Table Dog','Dry','0','Petshop','TPR','','15','TPR','','0','','','12.09.22','09.10.22','2022P10W1D2','2022P11W1D1','29.08.22','08.10.22','2022P9W3D2','2022P10W4D7',0,21690.6250549451,73.0227272727273,63356.3439104895,150118.84413027,13010.3045188172,16.4523212426746,8561.98837634409,60603.2064516129,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,70325.5417406434,0,0,70325.5417406434,70325.5417406434,0,0,0,0,9529.49306287097,0,0,9529.49306287097,9529.49306287097,0
GO
Add_TLC '10038','0','Whiskas','Pouch','0','Petshop','TPR','','20','TPR','','0','','','10.10.22','06.11.22','2022P11W1D2','2022P12W1D1','26.09.22','05.11.22','2022P10W3D2','2022P11W4D7',0,124505.798282913,116.376871636683,579583.812191731,1077607.00532338,298432.605376344,69.2611526855312,826791.449892475,2020521.87139785,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,643338.031532821,0,0,643338.031532821,643338.031532821,0,0,0,0,920218.883730324,0,0,920218.883730324,920218.883730324,0
GO
Add_TLC '10039','0','Pedigree','Pouch','0','Petshop','TPR','','15','TPR','','0','','','10.10.22','06.11.22','2022P11W1D2','2022P12W1D1','26.09.22','05.11.22','2022P10W3D2','2022P11W4D7',0,15143.1750683544,73.0227272727273,44231.8377223844,104804.537995802,36051.0360887097,18.5979778588866,26819.0548387097,171023.199193548,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,49097.3398718466,0,0,49097.3398718466,49097.3398718466,0,0,0,0,29849.6080354839,0,0,29849.6080354839,29849.6080354839,0
GO
Add_TLC '10040','0','Dreamies','C&T','0','Petshop','TPR','','15','TPR','','0','','','10.10.22','06.11.22','2022P11W1D2','2022P12W1D1','26.09.22','05.11.22','2022P10W3D2','2022P11W4D7',0,105191.90694,203.196123702077,854983.509401503,1275751.1371615,206126.506527778,13.5260516739903,111523.111146953,936029.137258064,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,949031.695435668,0,0,949031.695435668,949031.695435668,0,0,0,0,124125.222706558,0,0,124125.222706558,124125.222706558,0
GO
Add_TLC '10041','0','Perfect Fit Cat','Pouch','0','Petshop','TPR','','20','TPR','','0','','','31.10.22','13.11.22','2022P11W4D2','2022P12W2D1','17.10.22','12.11.22','2022P11W2D2','2022P12W1D7',0,400000,436.74685,3493974.8,4293974.8,530232.767833333,19.1173125887598,202732.511349462,1263198.04701613,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,3878312.028,0,0,3878312.028,3878312.028,0,0,0,0,225641.285131951,0,0,225641.285131951,225641.285131951,0
GO
Add_TLC '10042','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','20','TPR','','0','','','31.10.22','13.11.22','2022P11W4D2','2022P12W2D1','17.10.22','12.11.22','2022P11W2D2','2022P12W1D7',0,850000,151.356579411765,2573061.85,4273061.85,683093.998296083,31.2773617713781,427307.562171275,1793495.55876344,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,2856098.6535,0,0,2856098.6535,2856098.6535,0,0,0,0,475593.316696629,0,0,475593.316696629,475593.316696629,0
GO
Add_TLC '10043','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','25','TPR','','0','','','31.10.22','13.11.22','2022P11W4D2','2022P12W2D1','17.10.22','12.11.22','2022P11W2D2','2022P12W1D7',0,70000,166.666666666667,233333.333333333,373333.333333333,40236.6462903226,37.4348655706613,30125.0688978495,110598.361478495,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,259000,0,0,259000,259000,0,0,0,0,33529.2016833065,0,0,33529.2016833065,33529.2016833065,0
GO
Add_TLC '10044','0','Sheba','Pouch','Core','Petshop','TPR','','15','TPR','','0','','','07.11.22','04.12.22','2022P12W1D2','2022P13W1D1','24.10.22','03.12.22','2022P11W3D2','2022P12W4D7',0,761078.065828062,68.9677245799053,2099592.89711347,5143905.16042572,896500.85008535,40.2297805965586,1442641.30014247,5028644.70048387,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,2330548.11579596,0,0,2330548.11579596,2330548.11579596,0,0,0,0,1605659.76705857,0,0,1605659.76705857,1605659.76705857,0
GO
Add_TLC '10045','0','Natures Table Cats','Pouch','0','Petshop','TPR','','15','TPR','','0','','','07.11.22','04.12.22','2022P12W1D2','2022P13W1D1','24.10.22','03.12.22','2022P11W3D2','2022P12W4D7',0,162381.395729441,73.0227272727273,474301.294980632,1123826.8778984,158971.981478495,60.7667784954338,386408.607419355,1022296.53333333,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,526474.437428501,0,0,526474.437428501,526474.437428501,0,0,0,0,430072.780057742,0,0,430072.780057742,430072.780057742,0
GO
Add_TLC '10046','0','Natures Table Cats','Dry','0','Petshop','TPR','','15','TPR','','0','','','07.11.22','04.12.22','2022P12W1D2','2022P13W1D1','24.10.22','03.12.22','2022P11W3D2','2022P12W4D7',0,29225.3909185198,43.6107954545455,50981.701817068,167883.265491147,17671.4030913979,130.596111485296,92312.6611290322,162998.273494624,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,56589.6890169454,0,0,56589.6890169454,56589.6890169454,0,0,0,0,102743.991836613,0,0,102743.991836613,102743.991836613,0
GO
Add_TLC '10047','0','Cesar','Pouch','0','Petshop','TPR','','20','TPR','','0','','','07.11.22','04.12.22','2022P12W1D2','2022P13W1D1','24.10.22','03.12.22','2022P11W3D2','2022P12W4D7',0,92498.4910610526,114.928072625698,425226.931937286,795220.896181497,48673.5523387096,40.2784254320378,78419.7619354841,273113.971290322,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,472001.894450388,0,0,472001.894450388,472001.894450388,0,0,0,0,87281.1950341937,0,0,87281.1950341937,87281.1950341937,0
GO
Add_TLC '10048','0','Natures Table Dog','Dry','0','Petshop','TPR','','15','TPR','','0','','','07.11.22','04.12.22','2022P12W1D2','2022P13W1D1','24.10.22','03.12.22','2022P11W3D2','2022P12W4D7',0,21690.6250549451,73.0227272727273,63356.3439104895,150118.84413027,13010.3045188172,47.8080382041987,24879.8854193549,76921.1034946237,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,70325.5417406434,0,0,70325.5417406434,70325.5417406434,0,0,0,0,27691.312471742,0,0,27691.312471742,27691.312471742,0
GO
Add_TLC '10049','0','Whiskas','Pouch','0','Petshop','TPR','','20','TPR','','0','','','05.12.22','01.01.23','2022P13W1D2','2023P1W1D1','21.11.22','31.12.22','2022P12W3D2','2022P13W4D7',0,124505.798282913,116.376871636683,579583.812191731,1077607.00532338,298432.605376344,20.7912288389976,248191.223655914,1441921.64516129,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,643338.031532821,0,0,643338.031532821,643338.031532821,0,0,0,0,276236.831929032,0,0,276236.831929032,276236.831929032,0
GO
Add_TLC '10050','0','Dreamies','C&T','0','Petshop','TPR','','15','TPR','','0','','','05.12.22','01.01.23','2022P13W1D2','2023P1W1D1','21.11.22','31.12.22','2022P12W3D2','2022P13W4D7',0,105191.90694,203.196123702077,854983.509401503,1275751.1371615,206126.506527778,12.1044138514734,99801.621630826,924307.647741938,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,949031.695435668,0,0,949031.695435668,949031.695435668,0,0,0,0,111079.204875109,0,0,111079.204875109,111079.204875109,0
GO
Add_TLC '10051','0','Cesar','Pouch','0','Petshop','TPR','','25','TPR','','0','','','12.12.22','25.12.22','2022P13W2D2','2022P13W4D1','28.11.22','24.12.22','2022P12W4D2','2022P13W3D7',0,70000,146.9126,205677.64,345677.64,48673.5523387096,23.2253385987591,22609.1946774191,119956.299354838,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,228302.1804,0,0,228302.1804,228302.1804,0,0,0,0,25164.0336759674,0,0,25164.0336759674,25164.0336759674,0
GO
Add_TLC '10052','0','Perfect Fit Cat','Pouch','0','Petshop','TPR','','20','TPR','','0','','','12.12.22','25.12.22','2022P13W2D2','2022P13W4D1','28.11.22','24.12.22','2022P12W4D2','2022P13W3D7',0,400000,436.74685,3493974.8,4293974.8,530232.767833333,20.9032554441148,221671.819817204,1282137.35548387,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,3878312.028,0,0,3878312.028,3878312.028,0,0,0,0,246720.735456548,0,0,246720.735456548,246720.735456548,0
GO
Add_TLC '10053','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','20','TPR','','0','','','12.12.22','25.12.22','2022P13W2D2','2022P13W4D1','28.11.22','24.12.22','2022P12W4D2','2022P13W3D7',0,850000,151.356579411765,2573061.85,4273061.85,683093.998296083,6.9662664690701,95172.2963110606,1461360.29290323,'5000138','361','closed','ilya.chernoskutov@effem.com','1',0,0,0,0,0,2856098.6535,0,0,2856098.6535,2856098.6535,0,0,0,0,105926.76579421,0,0,105926.76579421,105926.76579421,0
GO
DROP PROCEDURE Add_TLC;
