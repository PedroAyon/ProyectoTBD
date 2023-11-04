DROP SCHEMA IF EXISTS hotelclean;
CREATE SCHEMA hotelclean;
USE hotelclean;

CREATE TABLE Employee
(
    ID          INT PRIMARY KEY AUTO_INCREMENT,
    Name        VARCHAR(50)                                NOT NULL,
    LastName    VARCHAR(50)                                NOT NULL,
    Status      ENUM ('Disponible', 'Ocupado', 'Inactivo') NOT NULL,
    Position    ENUM ('Administracion', 'Intendencia')     NOT NULL,
    Username    VARCHAR(20) UNIQUE                         NOT NULL,
    Password    VARCHAR(20)                                NOT NULL,
    PhoneNumber VARCHAR(10)
);

CREATE TABLE Location
(
    ID     INT PRIMARY KEY AUTO_INCREMENT,
    Name   VARCHAR(100),
    Number VARCHAR(2),
    Type   ENUM ('Room', 'Area') NOT NULL,
    Floor  INT                   NOT NULL,
    CONSTRAINT CK_Name_Number CHECK (
            (Name IS NOT NULL AND Number IS NULL)
            OR (Name IS NULL AND Number IS NOT NULL)
        )
);

CREATE TABLE Service
(
    ID         INT PRIMARY KEY AUTO_INCREMENT,
    LocationID INT                                                            NOT NULL,
    Type       ENUM ('Limpieza', 'Sanitizacion', 'General') DEFAULT 'General' NOT NULL,
    Status     ENUM ('Pendiente', 'En Curso', 'Terminado')                    NOT NULL,
    Date       DATE,
    StartTime  TIME,
    EndingTime TIME,
    CONSTRAINT FK_Service_Location FOREIGN KEY (LocationID) REFERENCES Location (ID),
    CONSTRAINT CK_Service_Date CHECK ((Status <> 'En Curso') OR (`Date` IS NOT NULL)),
    CONSTRAINT CK_Service_EndingTime CHECK ((Status <> 'Terminado') OR (EndingTime IS NOT NULL))
);

DROP TABLE IF EXISTS ServiceEmployeeAssignment;

CREATE TABLE ServiceEmployeeAssignment
(
    ServiceID  INT,
    EmployeeID INT,
    PRIMARY KEY (ServiceID, EmployeeID),
    FOREIGN KEY (ServiceID) REFERENCES Service (ID) ON DELETE CASCADE,
    FOREIGN KEY (EmployeeID) REFERENCES Employee (ID) ON DELETE CASCADE
);

call createEmployee('Pedro', 'Ayon', 'Administracion', null, 'admin', '123')
