-- 1. Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Yukimushi')
BEGIN
    CREATE DATABASE Yukimushi;
END
GO

USE Yukimushi;
GO

-- 2. Tables
