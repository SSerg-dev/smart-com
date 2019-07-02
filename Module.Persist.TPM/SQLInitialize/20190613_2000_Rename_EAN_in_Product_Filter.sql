UPDATE [TPM_Dev].[dbo].[ProductTree]
SET [Filter] = REPLACE([Filter], '"EAN"', '"EAN_Case"')
GO
