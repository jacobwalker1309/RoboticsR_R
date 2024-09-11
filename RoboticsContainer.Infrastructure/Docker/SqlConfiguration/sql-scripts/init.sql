-- Create the database if it doesn't exist
IF DB_ID('TestDB') IS NULL
BEGIN
    CREATE DATABASE TestDB;
END
GO

-- Switch to the TestDB database context
USE TestDB;
GO

-- Create the RoboticsContainerTest table if it doesn't exist
IF OBJECT_ID('RoboticsContainerTest', 'U') IS NULL
BEGIN
    CREATE TABLE RoboticsContainerTest (
        ID INT IDENTITY PRIMARY KEY,          -- Primary key
        Temperature FLOAT,                    -- Temperature as FLOAT
        [Current] FLOAT,                      -- Current as FLOAT (wrapped in brackets)
        Voltage FLOAT,                        -- Voltage as FLOAT
        StateOfCharge FLOAT,                  -- State of charge as FLOAT
        ContainerID INT,                      -- ContainerID as INT
        DateInserted DATETIME DEFAULT GETDATE()  -- DateInserted with default current date/time
    );
END
GO

-- Insert sample data only if the table is empty
IF NOT EXISTS (SELECT 1 FROM RoboticsContainerTest)
BEGIN
    INSERT INTO RoboticsContainerTest (Temperature, [Current], Voltage, StateOfCharge, ContainerID, DateInserted)
    VALUES (25.5, 0.75, 12.5, 85.0, 1, GETDATE());
END
GO
