USE hotelclean;

-- SERVICIOS

DROP PROCEDURE IF EXISTS getService;

DELIMITER //
CREATE PROCEDURE getService(IN _serviceID INT)
BEGIN
    SELECT * FROM Service WHERE ID = _serviceID;
END//
DELIMITER ;

DROP PROCEDURE IF EXISTS registerService;

DELIMITER //
CREATE PROCEDURE registerService(IN _locationID INT,
                                 IN _type ENUM ('Limpieza', 'Sanitizacion', 'General'))
BEGIN
    INSERT INTO Service(LocationID, Type, Status)
    VALUES (_locationID, _type, 'Pendiente');
END;
//

DROP PROCEDURE IF EXISTS unregisterService;
DELIMITER //
CREATE PROCEDURE unregisterService(IN _serviceID INT)
BEGIN
    DELETE FROM Service WHERE ID = _serviceID;
END;
//

DROP PROCEDURE IF EXISTS assignEmployeeToService;

DELIMITER //
CREATE PROCEDURE assignEmployeeToService(IN _serviceID INT, IN _employeeID INT)
BEGIN
    DELETE FROM ServiceEmployeeAssignment s WHERE s.ServiceID = _serviceID AND EmployeeID = _employeeID;
    INSERT INTO ServiceEmployeeAssignment(ServiceID, EmployeeID)
    VALUES (_serviceID, _employeeID);
END;
//

DROP PROCEDURE IF EXISTS unregisterEmployeeFromService;

DELIMITER //
CREATE PROCEDURE unregisterEmployeeFromService(IN _serviceID INT, IN _employeeID INT)
BEGIN
    DELETE FROM ServiceEmployeeAssignment s WHERE s.ServiceID = _serviceID AND EmployeeID = _employeeID;
END;
//

DROP PROCEDURE IF EXISTS startService;

DELIMITER //
CREATE PROCEDURE startService(IN _serviceID INT)
BEGIN
    UPDATE Service SET Status = 'En Curso', Date = curdate() ,StartTime = curtime() WHERE ID = _serviceID;
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

DROP PROCEDURE IF EXISTS getEmployeeServiceHistory;

DELIMITER //
CREATE PROCEDURE getEmployeeServiceHistory(
    IN employeeID INT,
    IN startDate DATE,
    IN endDate DATE
)
BEGIN
    SELECT s.*
    FROM Service s
             JOIN ServiceEmployeeAssignment sea ON s.ID = sea.ServiceID
    WHERE sea.EmployeeID = employeeID
      AND (startDate IS NULL OR s.Date BETWEEN startDate AND endDate)
    ORDER BY s.Date;
END//
DELIMITER ;

DROP PROCEDURE IF EXISTS getServiceHistory;

DELIMITER //
CREATE PROCEDURE getServiceHistory(
    IN startDate DATE,
    IN endDate DATE
)
BEGIN
    SELECT *
    FROM Service s
    WHERE (startDate IS NULL OR s.Date BETWEEN startDate AND endDate)
    ORDER BY s.Date;
END//
DELIMITER ;

DROP PROCEDURE IF EXISTS getServicesByStatus;

DELIMITER //
CREATE PROCEDURE getServicesByStatus(
    IN _status ENUM ('Pendiente', 'En Curso', 'Terminado')
)
BEGIN
    SELECT * FROM Service s WHERE s.Status = _status;
END//
DELIMITER ;

DROP PROCEDURE IF EXISTS getServicesByLocation;

DELIMITER //
CREATE PROCEDURE getServicesByLocation(
    IN locationID INT,
    IN startDate DATE,
    IN endDate DATE
)
BEGIN
    SELECT *
    FROM Service s
    WHERE s.LocationID = locationID
      AND (startDate IS NULL OR s.Date BETWEEN startDate AND endDate)
    ORDER BY s.Date;
END//
DELIMITER ;

call getServicesByLocation(1, null, null);