USE hotelclean;

-- Procesos de creacion
DROP PROCEDURE IF EXISTS createHotel;

DELIMITER //
CREATE PROCEDURE createHotel(IN _name VARCHAR(50))
BEGIN
    INSERT INTO Hotel(Name)
    VALUES (_name);
END;
//

DROP PROCEDURE IF EXISTS createEmployee;

DELIMITER //
CREATE PROCEDURE createEmployee(IN _hotel INT, IN _name VARCHAR(30), IN _lastName VARCHAR(30),
                                IN _position ENUM ('Administración', 'Intendencia'), IN _phone INT
)
BEGIN
    DECLARE _autoUser VARCHAR(20) DEFAULT (SELECT max(ID) FROM Employee);
    DECLARE _autoPassword VARCHAR(20) DEFAULT '';
    DECLARE a INT Default 0;

    IF ((SELECT max(ID) FROM Employee) IS NULL)
    THEN
        SET _autoUser = '0';
    END IF;

    simple_loop:
    LOOP
        SET a = a + 1;
        SET _autoUser = hex(_autoUser + a);
        IF ((SELECT Username FROM Employee) IS NULL) THEN
            LEAVE simple_loop;
        END IF;
    END LOOP simple_loop;

    SET _autoPassword = convert(rand(), char(8));

    INSERT INTO Employee(HotelID, Name, LastName, Status, Position, Username, Password, PhoneNumber)
    VALUES (_hotel, _name, _lastName, 'Disponible', _position, _autoUser, _autoPassword, _phone);
END;
//

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
    INSERT INTO Resource(Name, Category, Stock) VALUES (_name, _category, _stock);
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

    UPDATE Service SET Status = 'En Curso' WHERE ID = _serviceID;
    UPDATE Employee SET Status = 'Ocupado' WHERE ID = _employeeID;

    IF ((SELECT (StartTime) FROM Service WHERE ID = _serviceID) IS NOT NULL)
    THEN
        UPDATE Service SET StartTime = current_time() WHERE ID = _serviceID;
    END IF;
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

DELIMITER //
CREATE PROCEDURE registerServiceAsFinished(IN _serviceID INT)
BEGIN
    UPDATE Service SET Status = 'Terminado', EndingTime = curtime() WHERE ID = _serviceID;
    UPDATE Employee
    SET Status = 'Disponible'
    WHERE ID IN (SELECT EmployeeID FROM ServiceEmployeeAssignment WHERE ServiceID = _serviceID);
END;
//

DROP PROCEDURE IF EXISTS changeUserPassword;

DELIMITER //
CREATE PROCEDURE changeUserPassword(IN userID INT, IN newUser VARCHAR(20), IN newPassword VARCHAR(20))
BEGIN
    UPDATE Employee
    SET Username = newUser,
        Password = newPassword
    WHERE ID = userID;
END;
//
DROP PROCEDURE IF EXISTS closeService;

DELIMITER //
CREATE PROCEDURE closeService(IN serviceID INT)
BEGIN
    UPDATE Service SET Status = 'Terminado', EndingTime = current_time() WHERE ID = serviceID;
    UPDATE Employee
    SET Status = 'Disponible'
    WHERE ID IN (SELECT e.ID
                 FROM Service s
                          INNER JOIN ServiceEmployeeAssignment sea ON s.ID = sea.ServiceID
                          INNER JOIN Employee e ON e.ID = sea.EmployeeID);
END;
//

DROP PROCEDURE IF EXISTS updateStock;

DELIMITER //
CREATE PROCEDURE updateStock(IN _id INT, IN _stock INT)
BEGIN
    DECLARE actualStock INT DEFAULT (SELECT Stock FROM Resource WHERE ID = _id);
    UPDATE Resource SET Stock = (actualStock + _stock) WHERE ID = _id;
END;
//

-- BUSQUEDA

DROP PROCEDURE IF EXISTS searchEmployeeByID;

DELIMITER //
CREATE PROCEDURE searchEmployeeByID(IN employeeID INT)
BEGIN
    SELECT * FROM Employee WHERE ID = employeeID;
END;
//

DROP PROCEDURE IF EXISTS searchEmployeeByName;

DELIMITER //
CREATE PROCEDURE searchEmployeeByName(IN lastName VARCHAR(50), IN name VARCHAR(50))
BEGIN
    SELECT * FROM Employee WHERE LastName like lastName AND Name like name;
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

DROP PROCEDURE IF EXISTS getEmployeeInOrderAlphabetic;

DELIMITER //
CREATE PROCEDURE getEmployeeInOrderAlphabetic()
BEGIN
    SELECT * FROM Employee order by Name;
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