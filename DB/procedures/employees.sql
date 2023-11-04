USE hotelclean;

-- EMPLEADOS

DROP PROCEDURE IF EXISTS createEmployee;

DELIMITER //
CREATE PROCEDURE createEmployee(IN p_name VARCHAR(50), IN p_lastName VARCHAR(50),
                                IN p_position ENUM ('Administración', 'Intendencia'), IN p_phoneNumber VARCHAR(10),
                                IN p_userName VARCHAR(20), IN p_password VARCHAR(20)
)
BEGIN
    INSERT INTO Employee(Name, LastName, Status, Position, Username, Password, PhoneNumber)
    VALUES (p_name, p_lastName, 'Disponible', p_position, p_userName, p_password, p_phoneNumber);
END//
DELIMITER ;

DROP PROCEDURE IF EXISTS getEmployeesD;

DELIMITER //
CREATE PROCEDURE getEmployees()
BEGIN
    SELECT *
    FROM Employee;
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
CREATE PROCEDURE searchEmployeeByName(IN _name VARCHAR(150))
BEGIN
    SELECT * FROM Employee WHERE CONCAT(Name, ' ', LastName) LIKE CONCAT('%', _name, '%');
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS deleteEmployee;

DELIMITER //

CREATE PROCEDURE deleteEmployee(IN employeeID INT)
BEGIN
    DELETE FROM Employee WHERE ID = employeeID;
END //

DELIMITER ;

DROP PROCEDURE IF EXISTS getTopPerformingEmployees;

DELIMITER //
CREATE PROCEDURE getTopPerformingEmployees(
    IN startDate DATE,
    IN endDate DATE
)
BEGIN
    SELECT e.ID, e.Name, e.LastName, e.Status, e.PhoneNumber, COUNT(*) AS ServiceCount
    FROM Employee e
             JOIN ServiceEmployeeAssignment sea ON e.ID = sea.EmployeeID
             JOIN Service s ON sea.ServiceID = s.ID
    WHERE (startDate IS NULL OR s.Date BETWEEN startDate AND endDate)
      AND e.Position = 'Intendencia'
    GROUP BY e.ID
    ORDER BY ServiceCount DESC;
END//
DELIMITER ;

DROP PROCEDURE IF EXISTS getLeastPerformingEmployees;

DELIMITER //
CREATE PROCEDURE getLeastPerformingEmployees(
    IN startDate DATE,
    IN endDate DATE
)
BEGIN
    SELECT e.ID, e.Name, e.LastName, e.Status, COUNT(*) AS ServiceCount
    FROM Employee e
             JOIN ServiceEmployeeAssignment sea ON e.ID = sea.EmployeeID
             JOIN Service s ON sea.ServiceID = s.ID
    WHERE (startDate IS NULL OR s.Date BETWEEN startDate AND endDate)
      AND e.Position = 'Intendencia'
    GROUP BY e.ID
    ORDER BY ServiceCount;
END//
DELIMITER ;

DROP PROCEDURE IF EXISTS serviceEmployeeList;

DELIMITER //
CREATE PROCEDURE serviceEmployeeList(IN _serviceID INT)
BEGIN
    SELECT e.*
    FROM Employee e
             INNER JOIN ServiceEmployeeAssignment se ON e.ID = se.EmployeeID
    WHERE se.ServiceID = _serviceID;
END;
//