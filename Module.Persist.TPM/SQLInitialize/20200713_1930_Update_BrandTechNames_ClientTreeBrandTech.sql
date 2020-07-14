UPDATE [dbo].[ClientTreeBrandTech]
   SET 
      [CurrentBrandTechName] = (Select BrandTech.BrandsegTechsub from BrandTech where BrandTech.Id = BrandTechId)
	WHERE [CurrentBrandTechName] != (Select BrandTech.BrandsegTechsub from BrandTech where BrandTech.Id = BrandTechId)
GO