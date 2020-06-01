--UPDATE поля в Baseline позволит создать(по триггеру) инциденты, по которым
--на пересчет должны отобраться промо, заведенные на on-invoice клиентов
UPDATE BaseLine SET Type = 1
WHERE DemandCode IN ('5KA_05_0125', 'X5_05_0125', 'KIB_05_0125', 'AUCHAN_05_0125', 'ATAK_05_0125') 
AND Disabled = 0