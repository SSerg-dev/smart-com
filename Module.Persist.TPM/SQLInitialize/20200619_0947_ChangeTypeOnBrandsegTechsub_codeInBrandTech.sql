ALTER TABLE [BrandTech] ADD BSTS_TEMP NVARCHAR(255)
GO

UPDATE [BrandTech] SET BSTS_TEMP = BrandsegTechsub_code
ALTER TABLE [BrandTech] DROP COLUMN BrandsegTechsub_code
GO

EXEC sys.sp_rename @objname = N'[BrandTech].[BSTS_TEMP]', @newname = 'BrandsegTechsub_code', @objtype = 'COLUMN'
