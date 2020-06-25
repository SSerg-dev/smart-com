ALTER TABLE [dbo].[BrandTech] ADD BSTS_TEMP NVARCHAR(255)
GO

UPDATE [dbo].[BrandTech] SET BSTS_TEMP = BrandsegTechsub_code
ALTER TABLE [dbo].[BrandTech] DROP COLUMN BrandsegTechsub_code
GO

EXEC sys.sp_rename @objname = N'[dbo].[BrandTech].[BSTS_TEMP]', @newname = 'BrandsegTechsub_code', @objtype = 'COLUMN'