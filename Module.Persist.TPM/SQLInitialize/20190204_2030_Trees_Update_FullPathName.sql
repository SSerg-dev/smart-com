--Замена символа '⮡ ' на '>' в иерархиях
UPDATE ClientTree SET FullPathName = REPLACE (FullPathName, N' ⮡  ', ' > ');
UPDATE ProductTree SET FullPathName = REPLACE (FullPathName, N' ⮡  ', ' > ');