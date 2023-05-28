DROP SCHEMA IF EXISTS hotelclean;
CREATE SCHEMA hotelclean;
USE hotelclean;


CREATE TABLE Hotel
(
    ID   INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Employee
(
    ID          INT PRIMARY KEY AUTO_INCREMENT,
    HotelID     INT                                        NOT NULL,
    Name        VARCHAR(50)                                NOT NULL,
    LastName    VARCHAR(50)                                NOT NULL,
    Status      ENUM ('Disponible', 'Ocupado', 'Inactivo') NOT NULL,
    Position    ENUM ('Administración', 'Intendencia')     NOT NULL,
    Username    VARCHAR(20)                                NOT NULL,
    Password    VARCHAR(20)                                NOT NULL,
    PhoneNumber VARCHAR(10),
    CONSTRAINT FK_Employee_Hotel FOREIGN KEY (HotelID) REFERENCES Hotel (ID)
);

CREATE TABLE Location
(
    ID      INT PRIMARY KEY AUTO_INCREMENT,
    HotelID INT                   NOT NULL,
    Name    VARCHAR(100),
    Number  VARCHAR(2),
    Type    ENUM ('Room', 'Area') NOT NULL,
    Floor   INT                   NOT NULL,
    CONSTRAINT FK_Location_Hotel FOREIGN KEY (HotelID) REFERENCES Hotel (ID),
    CONSTRAINT CK_Name_Number CHECK (
            (Name IS NOT NULL AND Number IS NULL)
            OR (Name IS NULL AND Number IS NOT NULL)
        )
);

CREATE TABLE Service
(
    ID         INT PRIMARY KEY AUTO_INCREMENT,
    HotelID    INT                                                            NOT NULL,
    LocationID INT                                                            NOT NULL,
    Type       ENUM ('Limpieza', 'Sanitizacion', 'General') DEFAULT 'General' NOT NULL,
    Status     ENUM ('Pendiente', 'En Curso', 'Terminado')                    NOT NULL,
    Date       DATE,
    StartTime  TIME,
    EndingTime TIME,
    CONSTRAINT FK_Service_Hotel FOREIGN KEY (HotelID) REFERENCES Hotel (ID),
    CONSTRAINT FK_Service_Location FOREIGN KEY (LocationID) REFERENCES Location (ID),
    CONSTRAINT CK_Service_Date CHECK ((Status <> 'En Curso') OR (`Date` IS NOT NULL)),
    CONSTRAINT CK_Service_EndingTime CHECK ((Status <> 'Terminado') OR (EndingTime IS NOT NULL))
);

CREATE TABLE RecurringService
(
    ID         INT PRIMARY KEY AUTO_INCREMENT,
    HotelID    INT                                                            NOT NULL,
    LocationID INT                                                            NOT NULL,
    Type       ENUM ('Limpieza', 'Sanitizacion', 'General') DEFAULT 'General' NOT NULL,
    Frequency  ENUM ('Daily', 'Custom')                                       NOT NULL,
    CustomDays SET ('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday'),
    StartTime  TIME,
    CONSTRAINT FK_RecurringService_Hotel FOREIGN KEY (HotelID) REFERENCES Hotel (ID),
    CONSTRAINT FK_RecurringService_Location FOREIGN KEY (LocationID) REFERENCES Location (ID),
    CONSTRAINT CK_RecurringService_CustomDays CHECK (Frequency <> 'Custom' OR CustomDays IS NOT NULL)
);

CREATE TABLE ServiceEmployeeAssignment
(
    ServiceID  INT,
    EmployeeID INT,
    PRIMARY KEY (ServiceID, EmployeeID),
    FOREIGN KEY (ServiceID) REFERENCES Service (ID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee (ID)
);

CREATE TABLE CleaningResource
(
    ID       INT PRIMARY KEY AUTO_INCREMENT,
    Name     VARCHAR(60) NOT NULL,
    Category VARCHAR(40),
    Stock    INT         NOT NULL
);

CREATE TABLE ConsumedResources
(
    ServiceID  INT,
    ResourceID INT,
    SpentStock INT NOT NULL,
    PRIMARY KEY (ServiceID, ResourceID),
    FOREIGN KEY (ServiceID) REFERENCES Service (ID),
    FOREIGN KEY (ResourceID) REFERENCES CleaningResource (ID)
);