DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource] = 'PromoProductsViews' and [Action] = 'FixateTempPromoProductsCorrections')
DELETE FROM AccessPoint WHERE [Resource] = 'PromoProductsViews' and [Action] = 'FixateTempPromoProductsCorrections'
