UPDATE [TPM_Dev].[ProductTree]
SET [Filter] = REPLACE([Filter], '"EAN"', '"EAN_Case"')
GO
