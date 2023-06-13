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
			2023, --#71
			
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

Add_TLC '10001','0','Cesar','Pouch','0','Petshop','TPR','','20','TPR','','0','','','21.05.23','17.06.23','2023P6W1D1','2023P6W4D7','14.05.23','16.06.23','2023P5W4D1','2023P6W4D6',0,142233.550883299,118,671342.360169171,1240276.56370237,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','1',0,0,0,0,0,747664.760169171,0,0,747664.760169171,747664.760169171,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10002','0','Cesar','Pouch','0','Petshop','TPR','','20','TPR','','0','','','13.08.23','09.09.23','2023P9W1D1','2023P9W4D7','06.08.23','08.09.23','2023P8W4D1','2023P9W4D6',1,147768.123903422,118,697465.544824152,1288538.04043784,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','2',0,0,0,0,0,773787.944824152,0,0,773787.944824152,773787.944824152,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10003','0','Cesar','Pouch','0','Petshop','TPR','','20','TPR','','0','','','05.11.23','02.12.23','2023P12W1D1','2023P12W4D7','29.10.23','01.12.23','2023P11W4D1','2023P12W4D6',2,147768.123903422,118,697465.544824152,1288538.04043784,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','3',0,0,0,0,0,773787.944824152,0,0,773787.944824152,773787.944824152,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10004','0','Cesar','Pouch','0','Petshop','TPR','','25','TPR','','0','','','01.12.23','14.12.23','2023P12W4D6','2023P13W2D5','24.11.23','13.12.23','2023P12W3D6','2023P13W2D4',3,147768.123903422,186.9126,552394.484718215,847930.732525059,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','4',0,0,0,0,0,597627.333918215,0,0,597627.333918215,597627.333918215,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10005','0','Dreamies','C&T','0','Petshop','TPR','','10','TPR','','0','','','13.08.23','09.09.23','2023P9W1D1','2023P9W4D7','06.08.23','08.09.23','2023P8W4D1','2023P9W4D6',4,179408.049755316,39,279876.557618294,997508.756639559,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','5',0,0,0,0,0,305101.757618294,0,0,305101.757618294,305101.757618294,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10006','0','Dreamies','C&T','0','Petshop','TPR','','10','TPR','','0','','','08.10.23','04.11.23','2023P11W1D1','2023P11W4D7','01.10.23','03.11.23','2023P10W4D1','2023P11W4D6',5,179653.972463636,39,280260.197043272,998876.086897816,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','6',0,0,0,0,0,305485.397043272,0,0,305485.397043272,305485.397043272,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10007','0','Dreamies','C&T','0','Petshop','TPR','','15','TPR','','0','','','01.12.23','14.12.23','2023P12W4D6','2023P13W2D5','24.11.23','13.12.23','2023P12W3D6','2023P13W2D4',6,179653.972463636,83.6580525,300590.029203928,659897.9741312,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','7',0,0,0,0,0,331878.140838928,0,0,331878.140838928,331878.140838928,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10008','0','Dreamies','C&T','0','Petshop','TPR','','10','TPR','','0','','','03.12.23','30.12.23','2023P13W1D1','2023P13W4D7','26.11.23','29.12.23','2023P12W4D1','2023P13W4D6',7,179741.822647109,39,280397.243329489,999364.533917924,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','8',0,0,0,0,0,305622.443329489,0,0,305622.443329489,305622.443329489,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10009','0','Pedigree','Dry','13kg','Petshop','TPR','','10','TPR','','0','','','21.05.23','17.06.23','2023P6W1D1','2023P6W4D7','14.05.23','16.06.23','2023P5W4D1','2023P6W4D6',8,286309.568125976,70,801666.790752732,1946905.06325663,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','9',0,0,0,0,0,876048.790752732,0,0,876048.790752732,876048.790752732,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10010','0','Pedigree','Dry','13kg','Petshop','TPR','','10','TPR','','0','','','10.09.23','07.10.23','2023P10W1D1','2023P10W4D7','03.09.23','06.10.23','2023P9W4D1','2023P10W4D6',9,320835.236563432,70,898338.662377609,2181679.60863134,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','10',0,0,0,0,0,972720.662377609,0,0,972720.662377609,972720.662377609,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10011','0','Pedigree','Pouch','0','Petshop','TPR','','15','TPR','','0','','','21.05.23','17.06.23','2023P6W1D1','2023P6W4D7','14.05.23','16.06.23','2023P5W4D1','2023P6W4D6',10,44903.0998822649,43,77233.3317974956,256845.731326555,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','11',0,0,0,0,0,83789.1117974956,0,0,83789.1117974956,83789.1117974956,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10012','0','Pedigree','Pouch','0','Petshop','TPR','','15','TPR','','0','','','10.09.23','07.10.23','2023P10W1D1','2023P10W4D7','03.09.23','06.10.23','2023P9W4D1','2023P10W4D6',11,45507.9163612534,43,78273.6161413558,260305.281586369,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','12',0,0,0,0,0,84829.3961413558,0,0,84829.3961413558,84829.3961413558,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10013','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','10','TPR','','0','','','23.04.23','20.05.23','2023P5W1D1','2023P5W4D7','16.04.23','19.05.23','2023P4W4D1','2023P5W4D6',12,305950.56267271,59,722043.327907597,1945845.57859844,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','13',0,0,0,0,0,782433.426907597,0,0,782433.426907597,782433.426907597,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10014','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','20','TPR','','0','','','17.04.23','23.04.23','2023P4W4D2','2023P5W1D1','10.04.23','22.04.23','2023P4W3D2','2023P4W4D7',13,305950.56267271,159.356579411765,487552.35136628,793502.91403899,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','14',0,0,0,0,0,572991.540774477,0,0,572991.540774477,572991.540774477,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10015','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','10','TPR','','0','','','16.07.23','12.08.23','2023P8W1D1','2023P8W4D7','09.07.23','11.08.23','2023P7W4D1','2023P8W4D6',14,306394.134641674,59,723090.15775435,1948666.69632104,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','15',0,0,0,0,0,783480.25675435,0,0,783480.25675435,783480.25675435,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10016','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','10','TPR','','0','','','08.10.23','04.11.23','2023P11W1D1','2023P11W4D7','01.10.23','03.11.23','2023P10W4D1','2023P11W4D6',15,322291.343315264,59,760607.570224024,2049772.94348508,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','16',0,0,0,0,0,820997.669224024,0,0,820997.669224024,820997.669224024,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10017','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','20','TPR','','0','','','01.11.23','14.11.23','2023P11W4D4','2023P12W2D3','25.10.23','13.11.23','2023P11W3D4','2023P12W2D2',16,322291.343315264,159.356579411765,1027184.92089487,1671767.6075254,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','17',0,0,0,0,0,1112624.11030306,0,0,1112624.11030306,1112624.11030306,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10018','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','20','TPR','','0','','','01.12.23','14.12.23','2023P12W4D6','2023P13W2D5','24.11.23','13.12.23','2023P12W3D6','2023P13W2D4',17,322291.343315264,159.356579411765,1027184.92089487,1671767.6075254,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','18',0,0,0,0,0,1112624.11030306,0,0,1112624.11030306,1112624.11030306,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10019','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','15','TPR','','0','','','23.04.23','20.05.23','2023P5W1D1','2023P5W4D7','16.04.23','19.05.23','2023P4W4D1','2023P5W4D6',18,790910.545470169,59,1866548.8873096,5030191.06919027,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','19',0,0,0,0,0,2152619.9869484,0,0,2152619.9869484,2152619.9869484,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10020','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','15','TPR','','0','','','16.07.23','12.08.23','2023P8W1D1','2023P8W4D7','09.07.23','11.08.23','2023P7W4D1','2023P8W4D6',19,792057.220099113,59,1869255.03943391,5037483.91983036,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','20',0,0,0,0,0,2155326.1390727,0,0,2155326.1390727,2155326.1390727,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10021','0','Perfect Fit Cat','Dry','0','Petshop','TPR','','15','TPR','','0','','','08.10.23','04.11.23','2023P11W1D1','2023P11W4D7','01.10.23','03.11.23','2023P10W4D1','2023P11W4D6',20,833152.977118305,59,1966241.0259992,5298852.93447242,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','21',0,0,0,0,0,2252312.125638,0,0,2252312.125638,2252312.125638,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10022','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','10','TPR','','0','','','23.04.23','20.05.23','2023P5W1D1','2023P5W4D7','16.04.23','19.05.23','2023P4W4D1','2023P5W4D6',21,35426.5454162227,90,127535.563498402,269241.745163292,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','22',0,0,0,0,0,139181.275912241,0,0,139181.275912241,139181.275912241,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10023','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','25','TPR','','0','','','17.04.23','23.04.23','2023P4W4D2','2023P5W1D1','10.04.23','22.04.23','2023P4W3D2','2023P4W4D7',22,35426.5454162227,186.666666666667,66129.5514436158,101556.096859838,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','23',0,0,0,0,0,80502.8847769492,0,0,80502.8847769492,80502.8847769492,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10024','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','10','TPR','','0','','','16.07.23','12.08.23','2023P8W1D1','2023P8W4D7','09.07.23','11.08.23','2023P7W4D1','2023P8W4D6',23,35475.9412603125,90,127713.388537125,269617.153578375,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','24',0,0,0,0,0,139359.100950964,0,0,139359.100950964,139359.100950964,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10025','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','10','TPR','','0','','','08.10.23','04.11.23','2023P11W1D1','2023P11W4D7','01.10.23','03.11.23','2023P10W4D1','2023P11W4D6',24,39167.5174646178,90,141003.062872624,297673.132731095,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','25',0,0,0,0,0,152648.775286463,0,0,152648.775286463,152648.775286463,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10026','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','25','TPR','','0','','','01.11.23','14.11.23','2023P11W4D4','2023P12W2D3','25.10.23','13.11.23','2023P11W3D4','2023P12W2D2',25,39167.5174646178,186.666666666667,146225.398534573,224560.433463809,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','26',0,0,0,0,0,160598.731867907,0,0,160598.731867907,160598.731867907,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10027','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','25','TPR','','0','','','01.12.23','14.12.23','2023P12W4D6','2023P13W2D5','24.11.23','13.12.23','2023P12W3D6','2023P13W2D4',26,39167.5174646178,186.666666666667,146225.398534573,224560.433463809,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','27',0,0,0,0,0,160598.731867907,0,0,160598.731867907,160598.731867907,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10028','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','15','TPR','','0','','','23.04.23','20.05.23','2023P5W1D1','2023P5W4D7','16.04.23','19.05.23','2023P4W4D1','2023P5W4D6',27,36944.8259340608,90,133001.373362619,280780.677098862,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','28',0,0,0,0,0,144647.085776458,0,0,144647.085776458,144647.085776458,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10029','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','15','TPR','','0','','','16.07.23','12.08.23','2023P8W1D1','2023P8W4D7','09.07.23','11.08.23','2023P7W4D1','2023P8W4D6',28,36996.3387428973,90,133186.81947443,281172.17444602,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','29',0,0,0,0,0,144832.53188827,0,0,144832.53188827,144832.53188827,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10030','0','Perfect Fit Dog','Dry','0','Petshop','TPR','','15','TPR','','0','','','08.10.23','04.11.23','2023P11W1D1','2023P11W4D7','01.10.23','03.11.23','2023P10W4D1','2023P11W4D6',29,40846.1253559586,90,147046.051281451,310430.552705285,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','30',0,0,0,0,0,158691.76369529,0,0,158691.76369529,158691.76369529,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10031','0','Perfect Fit Cat','Pouch','0','Petshop','TPR','','20','TPR','','0','','','01.06.23','14.06.23','2023P6W2D5','2023P6W4D4','25.05.23','13.06.23','2023P6W1D5','2023P6W4D3',30,646160.01795217,193.963470445,2506628.79089613,3798948.82680047,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','31',0,0,0,0,0,2749858.98283416,0,0,2749858.98283416,2749858.98283416,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10032','0','Perfect Fit Cat','Pouch','0','Petshop','TPR','','20','TPR','','0','','','01.11.23','14.11.23','2023P11W4D4','2023P12W2D3','25.10.23','13.11.23','2023P11W3D4','2023P12W2D2',31,677160.120964281,193.963470445,2626886.54218376,3981206.78411232,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','32',0,0,0,0,0,2870116.73412179,0,0,2870116.73412179,2870116.73412179,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10033','0','Perfect Fit Cat','Pouch','0','Petshop','TPR','','20','TPR','','0','','','01.12.23','14.12.23','2023P12W4D6','2023P13W2D5','24.11.23','13.12.23','2023P12W3D6','2023P13W2D4',32,677160.120964281,193.963470445,2626886.54218376,3981206.78411232,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','33',0,0,0,0,0,2870116.73412179,0,0,2870116.73412179,2870116.73412179,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10034','0','Whiskas','Dry','5kg, 13kg','Petshop','TPR','','10','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','11.06.23','14.07.23','2023P6W4D1','2023P7W4D6',33,219508.319900066,140,1229246.59144037,2107279.87104063,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','34',0,0,0,0,0,1274522.59144037,0,0,1274522.59144037,1274522.59144037,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10035','0','Whiskas','Dry','5kg, 13kg','Petshop','TPR','','10','TPR','','0','','','10.09.23','07.10.23','2023P10W1D1','2023P10W4D7','03.09.23','06.10.23','2023P9W4D1','2023P10W4D6',34,227833.606434727,140,1275868.19603447,2187202.62177338,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','35',0,0,0,0,0,1321144.19603447,0,0,1321144.19603447,1321144.19603447,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10036','0','Whiskas','Dry','5kg, 13kg','Petshop','TPR','','10','TPR','','0','','','03.12.23','30.12.23','2023P13W1D1','2023P13W4D7','26.11.23','29.12.23','2023P12W4D1','2023P13W4D6',35,227134.423073288,140,1271952.76921041,2180490.46150356,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','36',0,0,0,0,0,1317228.76921041,0,0,1317228.76921041,1317228.76921041,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10037','0','Whiskas','Dry','5kg, 13kg','Petshop','TPR','','15','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','11.06.23','14.07.23','2023P6W4D1','2023P7W4D6',36,254167.52830534,140,1423338.1585099,2440008.27173126,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','37',0,0,0,0,0,1598711.5105099,0,0,1598711.5105099,1598711.5105099,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10038','0','Whiskas','Dry','5kg, 13kg','Petshop','TPR','','15','TPR','','0','','','10.09.23','07.10.23','2023P10W1D1','2023P10W4D7','03.09.23','06.10.23','2023P9W4D1','2023P10W4D6',37,263807.333766526,140,1477321.06909255,2532550.40415865,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','38',0,0,0,0,0,1652694.42109255,0,0,1652694.42109255,1652694.42109255,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10039','0','Whiskas','Dry','5kg, 13kg','Petshop','TPR','','15','TPR','','0','','','03.12.23','30.12.23','2023P13W1D1','2023P13W4D7','26.11.23','29.12.23','2023P12W4D1','2023P13W4D6',38,262997.753032228,140,1472787.41698048,2524778.42910939,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','39',0,0,0,0,0,1648160.76898048,0,0,1648160.76898048,1648160.76898048,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10040','0','Whiskas','Pouch','Core','Petshop','TPR','','20','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','11.06.23','14.07.23','2023P6W4D1','2023P7W4D6',39,443668.897658144,89,1579461.27566299,3354136.86629557,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','40',0,0,0,0,0,1727486.07566299,0,0,1727486.07566299,1727486.07566299,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10041','0','Whiskas','Pouch','Core','Petshop','TPR','','20','TPR','','0','','','10.09.23','07.10.23','2023P10W1D1','2023P10W4D7','03.09.23','06.10.23','2023P9W4D1','2023P10W4D6',40,450186.779596618,89,1602664.93536396,3403412.05375043,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','41',0,0,0,0,0,1750689.73536396,0,0,1750689.73536396,1750689.73536396,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10042','0','Whiskas','Pouch','Core','Petshop','TPR','','20','TPR','','0','','','03.12.23','30.12.23','2023P13W1D1','2023P13W4D7','26.11.23','29.12.23','2023P12W4D1','2023P13W4D6',41,450186.779596618,89,1602664.93536396,3403412.05375043,0,0,0,0,'5000153','472','draft ','ilya.chernoskutov@effem.com','42',0,0,0,0,0,1750689.73536396,0,0,1750689.73536396,1750689.73536396,0,0,0,0,0,0,0,0,0,0
GO

DROP PROCEDURE Add_TLC;
