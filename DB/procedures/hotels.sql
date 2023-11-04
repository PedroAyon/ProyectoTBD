USE hotelclean;

-- HOTELES
DROP PROCEDURE IF EXISTS createHotel;

DELIMITER //
CREATE PROCEDURE createHotel(IN _name VARCHAR(50))
BEGIN
    INSERT INTO Hotel(Name)
    VALUES (_name);
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS getHotelsSorted;

DELIMITER //
CREATE PROCEDURE getHotelsSorted()
BEGIN
    SELECT * FROM Hotel order by Name;
END;
//

DROP PROCEDURE IF EXISTS searchHotelByID;

DELIMITER //
CREATE PROCEDURE searchHotelByID(IN hotelID INT)
BEGIN
    SELECT * FROM Hotel WHERE ID = hotelID;
END;
//

DROP PROCEDURE IF EXISTS searchHotelByName;

DELIMITER //
CREATE PROCEDURE searchHotelByName(IN name VARCHAR(50))
BEGIN
    SELECT * FROM Hotel WHERE Name like name;
END;
//