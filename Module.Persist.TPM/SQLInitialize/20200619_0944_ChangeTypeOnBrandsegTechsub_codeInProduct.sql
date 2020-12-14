ALTER TABLE [Product] ADD BSTS_TEMP NVARCHAR(255)
GO

UPDATE [Product] SET BSTS_TEMP = BrandsegTechsub_code
ALTER TABLE [Product] DROP COLUMN BrandsegTechsub_code
GO

EXEC sys.sp_rename @objname = N'[Product].[BSTS_TEMP]', @newname = 'BrandsegTechsub_code', @objtype = 'COLUMN'
