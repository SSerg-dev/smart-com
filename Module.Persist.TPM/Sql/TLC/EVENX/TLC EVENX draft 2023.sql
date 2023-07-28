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
	
		-------формируем имя промо и ищем выбранный узел в дереве продуктов -------
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
		print(N'Применено стандарное событие')
		SELECT @eventId = Id, @eventNameBD = [name] FROM Jupiter.[Event] Where LOWER([Name]) = 'standard promo' AND [Disabled] = 'FALSE';
	END

	IF(@eventId IS NULL) BEGIN
		print(N'Промо №' + CAST(@number AS NVARCHAR) + N' событие ' + @eventName + N' не найдено');
		print(N'Применено стандарное событие')
		SELECT @eventId = Id, @eventNameBD = [name] FROM Jupiter.[Event] Where LOWER([Name]) = 'standard promo' AND [Disabled] = 'FALSE';	
	END
	
		
		--Тип промо
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
		print(N'Промо №' + CAST(@number AS NVARCHAR) + N' механика ' + @planMarsMechanicName + N' не найдена');
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
		print(N'Присутствует ActualInStoreMarsMechanicType')
	END
	ELSE BEGIN
		SET @actualInStoreMechanicDiscount = @actualMarsMechanicDiscount;
		print(N'Отсутствует ActualInStoreMarsMechanicType')
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

Add_TLC '10001','0','Whiskas','Dry','5kg','Evenx','TPR','','15','TPR','','0','','','18.06.23','01.07.23','2023P7W1D1','2023P7W2D7','04.06.23','24.06.23','2023P6W3D1','2023P7W1D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10002','0','Whiskas','Dry','800g, 1,9kg','Evenx','TPR','','10','TPR','','0','','','18.06.23','01.07.23','2023P7W1D1','2023P7W2D7','04.06.23','24.06.23','2023P6W3D1','2023P7W1D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10003','0','Perfect Fit Cat','Pouch','0','Evenx','TPR','','15','TPR','','0','','','18.06.23','01.07.23','2023P7W1D1','2023P7W2D7','04.06.23','24.06.23','2023P6W3D1','2023P7W1D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10004','0','Pedigree','Pouch','0','Evenx','TPR','','15','TPR','','0','','','18.06.23','01.07.23','2023P7W1D1','2023P7W2D7','04.06.23','24.06.23','2023P6W3D1','2023P7W1D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10005','0','Pedigree','Dry','600g, 2,2kg','Evenx','TPR','','15','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10006','0','Whiskas','Pouch','0','Evenx','TPR','','15','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10007','0','Whiskas','Dry','800g, 1,9kg','Evenx','TPR','','30','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10008','0','Natures Table Cats','Pouch','0','Evenx','TPR','','30','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10009','0','Natures Table Cats','Dry','650g, 1,1kg','Evenx','TPR','','15','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10010','0','Pedigree','Dry','600g, 2,2kg','Evenx','TPR','','15','TPR','','0','','','18.06.23','01.07.23','2023P7W1D1','2023P7W2D7','04.06.23','24.06.23','2023P6W3D1','2023P7W1D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10011','0','Whiskas','Dry','800g, 1,9kg','Evenx','TPR','','10','TPR','','0','','','18.06.23','01.07.23','2023P7W1D1','2023P7W2D7','04.06.23','24.06.23','2023P6W3D1','2023P7W1D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10012','0','Perfect Fit Cat','Pouch','0','Evenx','TPR','','15','TPR','','0','','','18.06.23','01.07.23','2023P7W1D1','2023P7W2D7','04.06.23','24.06.23','2023P6W3D1','2023P7W1D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10013','0','Pedigree','Pouch','0','Evenx','TPR','','15','TPR','','0','','','18.06.23','01.07.23','2023P7W1D1','2023P7W2D7','04.06.23','24.06.23','2023P6W3D1','2023P7W1D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10014','0','Pedigree','Dry','600g, 2,2kg','Evenx','TPR','','15','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10015','0','Whiskas','Pouch','0','Evenx','TPR','','15','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10016','0','Whiskas','Dry','800g, 1,9kg','Evenx','TPR','','30','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10017','0','Natures Table Cats','Pouch','0','Evenx','TPR','','30','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10018','0','Natures Table Cats','Dry','650g, 1,1kg','Evenx','TPR','','15','TPR','','0','','','02.07.23','15.07.23','2023P7W3D1','2023P7W4D7','18.06.23','08.07.23','2023P7W1D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10019','0','Pedigree','Dry','600g, 2,2kg','Evenx','TPR','','15','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10020','0','Whiskas','Pouch','0','Evenx','TPR','','15','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10021','0','Whiskas','Pouch','0','Evenx','TPR','','30','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10022','0','Natures Table Cats','Pouch','0','Evenx','TPR','','30','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10023','0','Natures Table Cats','Dry','650g, 1,1kg','Evenx','TPR','','30','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10024','0','Natures Table Cats','Dry','650g, 1,1kg','Evenx','TPR','','15','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10025','0','Pedigree','Pouch','0','Evenx','TPR','','15','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10026','0','Pedigree','Pouch','0','Evenx','TPR','','10','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10027','0','Pedigree','Dry','13kg','Evenx','TPR','','10','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10028','0','Pedigree','Dry','13kg','Evenx','TPR','','15','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10029','0','Whiskas','Dry','800g, 1,9kg','Evenx','TPR','','15','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO
Add_TLC '10030','0','Whiskas','Dry','350g','Evenx','TPR','','15','TPR','','0','','','18.06.23','15.07.23','2023P7W1D1','2023P7W4D7','04.06.23','08.07.23','2023P6W3D1','2023P7W3D7',0,0,0,0,0,0,0,0,0,'5000163','492','draft','yuliya.aleksandrova@effem.com','1',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
GO

DROP PROCEDURE Add_TLC;
