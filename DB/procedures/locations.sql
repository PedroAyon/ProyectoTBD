USE hotelclean;

-- LOCACIONES
DROP PROCEDURE IF EXISTS getLocations;

DELIMITER //
CREATE PROCEDURE getLocations()
BEGIN
    SELECT *
    FROM Location
    order by Type, Number, Name;
END;
//