-- Create the database
\connect postgres
CREATE DATABASE preparationtask;

-- Connect to the new database and create the schema
\connect preparationtask
CREATE SCHEMA IF NOT EXISTS preptask;
CREATE extension postgis;

CREATE TABLE preptask."STREETS" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "Capacity" INTEGER NOT NULL,
    "Geometry" GEOMETRY(LineString, 4326) NOT NULL
);
