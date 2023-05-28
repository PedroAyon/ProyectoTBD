USE hotelclean;

-- Procesos de creacion
DROP PROCEDURE IF EXISTS createHotel;

DELIMITER //
CREATE PROCEDURE createHotel(IN _name VARCHAR(50))
BEGIN
    INSERT INTO Hotel(Name)
    VALUES (_name);
END //
DELIMITER ;


-- EMPLEADOS
DROP PROCEDURE IF EXISTS createEmployee;

DELIMITER //
CREATE PROCEDURE createEmployee(IN p_hotel INT, IN p_name VARCHAR(30), IN p_lastName VARCHAR(30),
                                IN p_position ENUM ('Administración', 'Intendencia'), IN p_phoneNumber VARCHAR(10),
                                IN p_userName VARCHAR(20), IN p_password VARCHAR(20)
)
BEGIN
    INSERT INTO Employee(HotelID, Name, LastName, Status, Position, Username, Password, PhoneNumber)
    VALUES (p_hotel, p_name, p_lastName, 'Disponible', p_position, p_userName, p_password, p_phoneNumber);
END//
DELIMITER ;

DROP PROCEDURE IF EXISTS getEmployeesByHotelID;

DELIMITER //
CREATE PROCEDURE getEmployeesByHotelID(IN p_hotelID INT)
BEGIN
    SELECT *
    FROM Employee e
    WHERE e.HotelID = p_hotelID;
END//
DELIMITER ;

DROP PROCEDURE IF EXISTS getEmployeeByID;

DELIMITER //
CREATE PROCEDURE getEmployeeByID(IN employeeID INT)
BEGIN
    SELECT * FROM Employee WHERE ID = employeeID;
END //
DELIMITER ;


DROP PROCEDURE IF EXISTS changeEmployeeCredentials;

DELIMITER //
CREATE PROCEDURE changeEmployeeCredentials(IN userID INT, IN newUser VARCHAR(20), IN newPassword VARCHAR(20))
BEGIN
    IF newUser IS NOT NULL THEN
        UPDATE Employee
        SET Username = newUser
        WHERE ID = userID;
    END IF;

    IF newPassword IS NOT NULL THEN
        UPDATE Employee
        SET Password = newPassword
        WHERE ID = userID;
    END IF;
END//
DELIMITER ;


DROP PROCEDURE IF EXISTS searchEmployeeByName;

DELIMITER //
CREATE PROCEDURE searchEmployeeByName(IN lastName VARCHAR(50), IN name VARCHAR(50))
BEGIN
    SELECT * FROM Employee WHERE LastName like lastName AND Name like name;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS getEmployeesSorted;

DELIMITER //
CREATE PROCEDURE getEmployeesSorted()
BEGIN
    SELECT * FROM Employee order by Name;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS deleteEmployee;

DELIMITER //

CREATE PROCEDURE deleteEmployee(IN employeeID INT)
BEGIN
    DELETE FROM Employee WHERE ID = employeeID;
END //

DELIMITER ;


-- Locaciones

DROP PROCEDURE IF EXISTS createLocation;

DELIMITER //
CREATE PROCEDURE createLocation(IN _hotelID INT, IN _name VARCHAR(100), IN _number VARCHAR(2),
                                IN _type ENUM ('Room', 'Area'), IN _floor INT)
BEGIN
    INSERT INTO Location(HotelID, Name, Number, Type, Floor)
    VALUES (_hotelID, _name, _number, _type, _floor);
END;
//

DROP PROCEDURE IF EXISTS registerService;

DELIMITER //
CREATE PROCEDURE registerService(IN _hotelID INT, IN _locationID INT,
                                 IN _type ENUM ('Limpieza', 'Sanitizacion', 'General'))
BEGIN
    INSERT INTO Service(HotelID, LocationID, Type, Status, Date)
    VALUES (_hotelID, _locationID, _type, 'Pendiente', curdate());
END;
//

DROP PROCEDURE IF EXISTS newCleaningResource;

DELIMITER //
CREATE PROCEDURE newCleaningResource(IN _name VARCHAR(60), IN _category VARCHAR(40), IN _stock INT)
BEGIN
    DECLARE defaultStock INT DEFAULT (0);
    IF (NOT (_stock IS NULL OR _stock < 0))
    THEN
        SET defaultStock = _stock;
    END IF;
    INSERT INTO CleaningResource(Name, Category, Stock) VALUES (_name, _category, _stock);
END;
//

DROP PROCEDURE IF EXISTS createRecurringService;

DELIMITER //
CREATE PROCEDURE createRecurringService(IN _hotelID INT, IN _locationID INT,
                                        IN _type ENUM ('Limpieza', 'Sanitizacion', 'General'),
                                        IN _frequency ENUM ('Daily', 'Custom'),
                                        IN _cDays SET ('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday'),
                                        IN _sTime TIME)
BEGIN
    INSERT INTO RecurringService(HotelID, LocationID, Type, Frequency, CustomDays, StartTime)
    VALUES (_hotelID, _locationID, _type, _frequency, _cDays, _sTime);
END;
//


-- Procesos recurrentes

DROP PROCEDURE IF EXISTS assignEmployeeToService;

DELIMITER //
CREATE PROCEDURE assignEmployeeToService(IN _employeeID INT, IN _serviceID INT)
BEGIN
    INSERT INTO ServiceEmployeeAssignment(ServiceID, EmployeeID)
    VALUES (_serviceID, _employeeID);
END;
//

DROP PROCEDURE IF EXISTS startService;

DELIMITER //
CREATE PROCEDURE startService(IN _serviceID INT)
BEGIN
    UPDATE Service SET Status = 'En Curso', StartTime = curtime() WHERE ID = _serviceID;
    UPDATE Employee
    SET Status = 'Ocupado'
    WHERE ID IN (SELECT EmployeeID FROM ServiceEmployeeAssignment WHERE ServiceID = _serviceID);
END;
//

DROP PROCEDURE IF EXISTS registerServiceAsFinished;

DELIMITER //
CREATE PROCEDURE registerServiceAsFinished(IN _serviceID INT)
BEGIN
    UPDATE Service SET Status = 'Terminado', EndingTime = curtime() WHERE ID = _serviceID;
    UPDATE Employee
    SET Status = 'Disponible'
    WHERE ID IN (SELECT EmployeeID FROM ServiceEmployeeAssignment WHERE ServiceID = _serviceID);
END;
//

DROP PROCEDURE IF EXISTS updateStock;

DELIMITER //
CREATE PROCEDURE updateStock(IN _id INT, IN _stock INT)
BEGIN
    DECLARE actualStock INT DEFAULT (SELECT Stock FROM CleaningResource WHERE ID = _id);
    UPDATE CleaningResource SET Stock = (actualStock + _stock) WHERE ID = _id;
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

-- REPORTES

DROP PROCEDURE IF EXISTS getHotelsInOrderAlphabetic;

DELIMITER //
CREATE PROCEDURE getHotelsInOrderAlphabetic()
BEGIN
    SELECT * FROM Hotel order by Name;
END;
//

DROP PROCEDURE IF EXISTS getLocations;

DELIMITER //
CREATE PROCEDURE getLocations()
BEGIN
    SELECT * FROM Location order by Type, Number, Name;
END;
//

DROP PROCEDURE IF EXISTS serviceEmployeeList;

DELIMITER //
CREATE PROCEDURE serviceEmployeeList(IN _serviceID INT)
BEGIN
    SELECT e.Name, e.LastName, e.Status, e.Position
    FROM Employee e
             INNER JOIN ServiceEmployeeAssignment se ON e.ID = se.EmployeeID
    WHERE se.ServiceID = _serviceID;
END;
//