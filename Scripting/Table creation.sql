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
CREATE TABLE [dbo].[MESSAGES]( 
CULTURE NVARCHAR(100) NOT NULL, 
MESSAGENAME NVARCHAR(255) NOT NULL,
VALUE NVARCHAR(2000) NULL,
COMMENT_EN_US NVARCHAR(2000) NULL,
INUSE INTEGER NULL); 

CREATE TABLE [dbo].[FORMITEMS]( 
APP_FORM NVARCHAR(MAX) NOT NULL, 
ITEM NVARCHAR(255) NOT NULL, 
CULTURE NVARCHAR(100) NOT NULL, 
PROPERTYNAME NVARCHAR(255) NOT NULL, 
VALUETYPE NVARCHAR(100) NOT NULL, 
VALUE NVARCHAR(2000) NULL, 
INUSE INTEGER NULL); 

CREATE TABLE [dbo].[CULTURES]( 
CULTURE NVARCHAR(100) NOT NULL, 
NATIVENAME NVARCHAR(200) NULL, 
LCID INTEGER NULL); 


-- MySQL server ------------------------------------------
CREATE TABLE IF NOT EXISTS MESSAGES( 
CULTURE VARCHAR(100) NOT NULL,
MESSAGENAME VARCHAR(255) NOT NULL,
VALUE VARCHAR(2000) NULL, 
COMMENT_EN_US VARCHAR(2000) NULL, 
INUSE INTEGER NULL); 

CREATE TABLE IF NOT EXISTS FORMITEMS( 
APP_FORM TEXT NOT NULL, 
ITEM VARCHAR(255) NOT NULL, 
CULTURE VARCHAR(100) NOT NULL, 
PROPERTYNAME VARCHAR(255) NOT NULL, 
VALUETYPE VARCHAR(100) NOT NULL, 
VALUE VARCHAR(2000) NULL, 
INUSE INTEGER NULL); 

CREATE TABLE IF NOT EXISTS CULTURES( 
CULTURE VARCHAR(100) NOT NULL, 
NATIVENAME VARCHAR(200) NULL,
LCID INTEGER NULL); 

-- PostgreSQL server -------------------------------------
CREATE TABLE public.MESSAGES( 
CULTURE VARCHAR(100) NOT NULL, 
MESSAGENAME VARCHAR(255) NOT NULL, 
VALUE VARCHAR(2000) NULL, 
COMMENT_EN_US VARCHAR(2000) NULL, 
INUSE INTEGER NULL); 

CREATE TABLE public.FORMITEMS( 
APP_FORM TEXT NOT NULL, 
ITEM VARCHAR(255) NOT NULL, 
CULTURE VARCHAR(100) NOT NULL, 
PROPERTYNAME VARCHAR(255) NOT NULL, 
VALUETYPE VARCHAR(100) NOT NULL, 
VALUE VARCHAR(2000) NULL, 
INUSE INTEGER NULL); 

CREATE TABLE public.CULTURES( 
CULTURE VARCHAR(100) NOT NULL, 
NATIVENAME VARCHAR(200) NULL, 
LCID INTEGER NULL); 

-- SQLite database ---------------------------------------
CREATE TABLE IF NOT EXISTS MESSAGES( 
CULTURE TEXT NOT NULL, 
MESSAGENAME TEXT NOT NULL, 
VALUE TEXT NULL, 
COMMENT_EN_US TEXT NULL, 
INUSE INTEGER NULL); 

CREATE TABLE IF NOT EXISTS FORMITEMS( 
APP_FORM TEXT NOT NULL, 
ITEM TEXT NOT NULL, 
CULTURE TEXT NOT NULL, 
PROPERTYNAME TEXT NOT NULL, 
VALUETYPE TEXT NOT NULL, 
VALUE TEXT NULL, 
INUSE INTEGER NULL); 

CREATE TABLE IF NOT EXISTS CULTURES( 
CULTURE TEXT NOT NULL, 
NATIVENAME TEXT NULL, 
LCID INTEGER NULL); 
