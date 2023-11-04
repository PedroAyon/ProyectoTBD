USE hotelclean;

-- CLEANING RESOURCES

DROP PROCEDURE IF EXISTS newCleaningResource;

DELIMITER //

CREATE PROCEDURE newCleaningResource(IN _hotelID INT, IN _name VARCHAR(60), IN _stock INT)
BEGIN
    DECLARE defaultStock INT DEFAULT 0;
    IF _stock IS NOT NULL AND _stock >= 0 THEN
        SET defaultStock = _stock;
    END IF;
    INSERT INTO CleaningResource (HotelID, Name, Stock) VALUES (_hotelID, _name, _stock);
END;
//

DROP PROCEDURE IF EXISTS updateStock;

DELIMITER //
CREATE PROCEDURE updateStock(IN _resourceID INT, IN _amount_spent INT)
BEGIN
    DECLARE actualStock INT DEFAULT (SELECT Stock FROM CleaningResource WHERE ID = _resourceID);
    UPDATE CleaningResource SET Stock = (actualStock + _amount_spent) WHERE ID = _resourceID;
END;
//

DROP PROCEDURE IF EXISTS getHotelResources;

DELIMITER //
CREATE PROCEDURE getHotelResources(IN _hotelID INT)
BEGIN
    SELECT *
    FROM CleaningResource
    WHERE HotelID = _hotelID;
END;
//
DELIMITER ;

