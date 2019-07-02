UPDATE pt
 SET pt.TechnologyId = bt.TechnologyId
 FROM ProductTree AS pt INNER JOIN BrandTech AS bt ON pt.TechnologyId = bt.Id
GO   

