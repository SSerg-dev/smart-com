﻿/* Добавление кодов из Brand, Technology, Segmen, BrandTech из таблицы MARS_UNIVERSAL_PETCARE_MATERIALS */
/* Brand: Brand_code, Segmen_code */

UPDATE Brand SET Brand_code = '425', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('CRAVE')
UPDATE Brand SET Brand_code = '339', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('DREAMIES')
UPDATE Brand SET Brand_code = '073', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('SHEBA')
UPDATE Brand SET Brand_code = '014', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('CATSAN')
UPDATE Brand SET Brand_code = '264', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('ROYAL CANIN')
UPDATE Brand SET Brand_code = '137', Segmen_code = '07' WHERE LOWER([Name]) = LOWER('CHAPPI')
UPDATE Brand SET Brand_code = '474', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('NATURES TABLE CAT')
UPDATE Brand SET Brand_code = '474', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('NATURES TABLE CATS')
UPDATE Brand SET Brand_code = '474', Segmen_code = '07' WHERE LOWER([Name]) = LOWER('NATURES TABLE DOG')
UPDATE Brand SET Brand_code = '042', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('KITEKAT')
UPDATE Brand SET Brand_code = '063', Segmen_code = '07' WHERE LOWER([Name]) = LOWER('PEDIGREE')
UPDATE Brand SET Brand_code = '278', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('PERFECT FIT CAT')
UPDATE Brand SET Brand_code = '278', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('PERFECT FIT CATS')
UPDATE Brand SET Brand_code = '278', Segmen_code = '07' WHERE LOWER([Name]) = LOWER('PERFECT FIT DOG')
UPDATE Brand SET Brand_code = '094', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('WHISKAS')
UPDATE Brand SET Brand_code = '446', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('EUKANUBA CAT')
UPDATE Brand SET Brand_code = '446', Segmen_code = '07' WHERE LOWER([Name]) = LOWER('EUKANUBA DOG')
UPDATE Brand SET Brand_code = '016', Segmen_code = '07' WHERE LOWER([Name]) = LOWER('CESAR')
UPDATE Brand SET Brand_code = '099', Segmen_code = '04' WHERE LOWER([Name]) = LOWER('Mixed CAT')
UPDATE Brand SET Brand_code = '099', Segmen_code = '07' WHERE LOWER([Name]) = LOWER('Mixed DOG')

/* Technology: Tech_code */

UPDATE Technology SET Tech_code = '007' WHERE LOWER([Name]) = LOWER('Pouch')
UPDATE Technology SET Tech_code = '007' WHERE LOWER([Name]) = LOWER('Pouch App.Mix')
UPDATE Technology SET Tech_code = '003' WHERE LOWER([Name]) = LOWER('Dry')
UPDATE Technology SET Tech_code = '004' WHERE LOWER([Name]) = LOWER('Tray')
UPDATE Technology SET Tech_code = '999' WHERE LOWER([Name]) = LOWER('Mixed')
UPDATE Technology SET Tech_code = '000' WHERE LOWER([Name]) = LOWER('Not Applicable')
UPDATE Technology SET Tech_code = '005' WHERE LOWER([Name]) = LOWER('Litter')
UPDATE Technology SET Tech_code = '002' WHERE LOWER([Name]) = LOWER('C&T')
UPDATE Technology SET Tech_code = '001' WHERE LOWER([Name]) = LOWER('Can')
