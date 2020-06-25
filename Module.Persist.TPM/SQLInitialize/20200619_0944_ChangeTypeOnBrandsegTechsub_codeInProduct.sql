ALTER TABLE [dbo].[Product] ADD BSTS_TEMP NVARCHAR(255)
GO

UPDATE [dbo].[Product] SET BSTS_TEMP = BrandsegTechsub_code
ALTER TABLE [dbo].[Product] DROP COLUMN BrandsegTechsub_code
GO

EXEC sys.sp_rename @objname = N'[dbo].[Product].[BSTS_TEMP]', @newname = 'BrandsegTechsub_code', @objtype = 'COLUMN'