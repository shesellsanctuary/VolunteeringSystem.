-- Event
CREATE TABLE event (
  id SERIAL PRIMARY KEY NOT NULL,
  institute TEXT NOT NULL,
  ageGroup TEXT NOT NULL,
  kidLimit INT NOT NULL,
  date TIMESTAMP NOT NULL,
  description TEXT NOT NULL
);

INSERT INTO event (institute, ageGroup, kidLimit, date, description) 
VALUES('Dummy institute', 'Dummy group', 42, TIMESTAMP '2011-05-16 15:36:38', 'Dummy description');


-- Credentials
CREATE TABLE credentials (
  email TEXT PRIMARY KEY NOT NULL,
  password TEXT NOT NULL
);

INSERT INTO credentials VALUES ('dummy@user.com', 'dummypwd');
INSERT INTO credentials VALUES ('dummy@admin.com', 'dummypwd');

-- Person
CREATE TYPE SEX AS ENUM ('m', 'f');

CREATE TABLE person (
  id SERIAL PRIMARY KEY NOT NULL,
  name TEXT NOT NULL,
  birthDate TIMESTAMP NOT NULL,
  CPF TEXT NOT NULL,
  sex SEX NOT NULL
);

-- Volunteer
CREATE TABLE volunteer (
  status INT NOT NULL,
  profession TEXT NOT NULL,
  address TEXT NOT NULL,
  phone TEXT NOT NULL,
  photo TEXT NOT NULL,
  criminalRecord TEXT NOT NULL,
  credentials TEXT REFERENCES credentials (email) NOT NULL
) INHERITS (person);

INSERT INTO volunteer (name, birthDate, CPF, sex, status, profession, address, phone, photo, criminalRecord, credentials) 
VALUES('Dummy Volunteer', TIMESTAMP '1993-05-16 15:36:38', '030.030.030-2', 'm', 0, 'Dummy profession', 'Dummy address', 'Dummy phone', 'www.dummy_url_photo.com/dummy.jpg', 'dummy', 'dummy@user.com');


-- Admin
CREATE TABLE administrator (
  credentials TEXT REFERENCES credentials (email) NOT NULL
) INHERITS (person);

INSERT INTO administrator (name, birthDate, CPF, sex, credentials) 
VALUES('Dummy Admin', TIMESTAMP '1993-05-16 15:36:38', '030.030.030-2', 'm', 'dummy@admin.com');


-- Utils
-- Connection String: "jdbc:postgresql://volunteeringsystem-postgres-sp.cr0sahgirswg.sa-east-1.rds.amazonaws.com/orphanage?user=vol_sys_postgres_db_admin&password=valpwd4242"
SELECT * FROM information_schema.tables 
WHERE table_schema = 'public';

SELECT * FROM event;
SELECT * FROM credentials;
SELECT * FROM volunteer;
SELECT * FROM administrator;
SELECT * FROM person;


--DROP TABLE event;
--DROP TABLE volunteer;
--DROP TABLE administrator;
--DROP TABLE person;
--DROP TABLE credentials;
--DROP TYPE SEX;


-- TODOS
-- Model volunteer  evaluations 
-- Model Kid entity
-- should person cpf be UNIQUE ?
