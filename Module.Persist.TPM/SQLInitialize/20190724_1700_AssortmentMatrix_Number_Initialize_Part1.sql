DROP TRIGGER AssortmentMatrix_ChangesIncident_Insert_Update_Trigger;

UPDATE am
SET am.Number = am.NewNumber
FROM (SELECT Number, ROW_NUMBER() OVER(ORDER BY Number) AS NewNumber FROM AssortmentMatrix) am;
