-- Create the database if it doesn't exist
CREATE DATABASE IF NOT EXISTS TestDB;

-- Switch to the TestDB database context
USE TestDB;

-- Create the RoboticsContainerTest table if it doesn't exist
CREATE TABLE IF NOT EXISTS RoboticsContainerTest (
    ID INT AUTO_INCREMENT PRIMARY KEY,      -- Primary key with AUTO_INCREMENT for MySQL
    Temperature FLOAT,                      -- Temperature as FLOAT
    `Current` FLOAT,                        -- Current as FLOAT (use backticks for reserved keywords)
    Voltage FLOAT,                          -- Voltage as FLOAT
    StateOfCharge FLOAT,                    -- State of charge as FLOAT
    ContainerID INT,                        -- ContainerID as INT
    DateInserted DATETIME DEFAULT CURRENT_TIMESTAMP  -- DateInserted with default current timestamp
);

-- Insert sample data only if the table is empty
INSERT INTO RoboticsContainerTest (Temperature, `Current`, Voltage, StateOfCharge, ContainerID, DateInserted)
SELECT 25.5, 0.75, 12.5, 85.0, 1, CURRENT_TIMESTAMP
WHERE NOT EXISTS (SELECT 1 FROM RoboticsContainerTest);
