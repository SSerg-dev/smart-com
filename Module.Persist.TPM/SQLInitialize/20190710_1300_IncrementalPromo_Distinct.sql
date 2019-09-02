WITH CTE AS(
   SELECT DeletedDate, PromoId, ProductId,
       RN = ROW_NUMBER()OVER(PARTITION BY DeletedDate, PromoId, ProductId ORDER BY DeletedDate)
   FROM dbo.IncrementalPromo
)
DELETE FROM CTE WHERE RN > 1