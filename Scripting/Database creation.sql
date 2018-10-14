/*
A program and library for application localization.
Copyright (C) 2015  VPKSoft

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

-- Microsoft SQL Server script --------------------------
USE [master]
GO

CREATE DATABASE [LANG]
GO

CREATE LOGIN [langlib_user] WITH PASSWORD=N'VPKSoft', DEFAULT_DATABASE=[LANG], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [LANG]
GO
CREATE USER [langlib_user] FOR LOGIN [langlib_user]
GO

EXEC sp_addrolemember N'db_datareader', N'langlib_user'
GO
EXEC sp_addrolemember N'db_datawriter', N'langlib_user'
GO

GRANT CREATE TABLE TO [langlib_user]
GO

GRANT ALTER ON Schema :: [dbo] TO [langlib_user]
GO

-- MySQL server ------------------------------------------
CREATE DATABASE lang CHARACTER SET utf8;
GRANT ALL PRIVILEGES ON lang.* TO 'lang_lib'@'localhost' IDENTIFIED BY 'VPKSoft';

-- PostgreSQL server -------------------------------------
CREATE ROLE langlib_user LOGIN ENCRYPTED PASSWORD 'VPKSoft' NOINHERIT VALID UNTIL 'infinity';
CREATE DATABASE lang WITH ENCODING='UTF8' OWNER=langlib_user;

-- SQLite database ---------------------------------------
----------------------------------------------------------
-- No creation needed