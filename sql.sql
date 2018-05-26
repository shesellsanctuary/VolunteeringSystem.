DROP SCHEMA public CASCADE;
CREATE SCHEMA public;

CREATE TYPE SEX AS ENUM ('M', 'F');

CREATE TABLE activity (
  id           SERIAL PRIMARY KEY NOT NULL UNIQUE,
  descriptiton TEXT               NOT NULL UNIQUE
);

CREATE TABLE age_group
(
  id    SERIAL PRIMARY KEY NOT NULL UNIQUE,
  min   INT                NOT NULL UNIQUE,
  max   INT                NOT NULL UNIQUE,
  label TEXT               NOT NULL UNIQUE,
  CHECK (max >= min)
);

INSERT INTO age_group (label, min, max) VALUES
  ('Crianças', 4, 5),
  ('Ensino Fundamental Inicial', 6, 10),
  ('Ensino Fundamental Final', 11, 14),
  ('Ensino Médio', 15, 17);

CREATE TABLE event
(
  id            SERIAL    NOT NULL,
  institute     TEXT      NOT NULL,
  age_group_id  INT       NOT NULL REFERENCES age_group,
  kid_limit     INT       NOT NULL,
  date          TIMESTAMP NOT NULL,
  description   TEXT      NOT NULL,
  volunteer_id  INT       NOT NULL,
  creation_date TIMESTAMP DEFAULT now(),
  status        INT       DEFAULT 0,
  justification TEXT
);

CREATE TABLE credential
(
  email    TEXT NOT NULL UNIQUE PRIMARY KEY,
  password TEXT NOT NULL
);

CREATE TABLE person
(
  id        SERIAL PRIMARY KEY NOT NULL,
  name      TEXT               NOT NULL,
  birthdate TIMESTAMP,
  cpf       CHAR(14) UNIQUE,
  sex       SEX                NOT NULL
);

CREATE TABLE administrator
(
  email TEXT NOT NULL UNIQUE REFERENCES credential,
  PRIMARY KEY (id)
)
  INHERITS (person);

CREATE TABLE volunteer
(
  email           TEXT NOT NULL UNIQUE REFERENCES credential,
  status          INT  NOT NULL,
  profession      TEXT NOT NULL,
  address         TEXT NOT NULL,
  phone           TEXT NOT NULL,
  photo           TEXT NOT NULL UNIQUE,
  criminal_record TEXT NOT NULL UNIQUE,
  creation_date   TIMESTAMP DEFAULT now(),
  PRIMARY KEY (id)
)
  INHERITS (person);

CREATE TABLE kid (
  availability INT NOT NULL,
  PRIMARY KEY (id)
)
  INHERITS (person);

CREATE TABLE kid_opinion (
  kid_id      INT     NOT NULL REFERENCES kid,
  activity_id INT     NOT NULL REFERENCES activity,
  likes       BOOLEAN NOT NULL
);

INSERT INTO kid (name, birthdate, sex, availability) VALUES
  ('José Dias', cast('2008-06-06' AS TIMESTAMP), 'M', 0),
  ('Janaína Castro', cast('2009-06-06' AS TIMESTAMP), 'F', 0),
  ('Eduardo Ferreira', cast('2010-06-06' AS TIMESTAMP), 'M', 1),
  ('Jacó Dias', cast('2011-06-06' AS TIMESTAMP), 'M', 1),
  ('Mariana Fontoura', cast('2011-10-10' AS TIMESTAMP), 'F', 1),
  ('Otávio Delazeri', cast('2011-10-04' AS TIMESTAMP), 'M', 1),
  ('Pedro Dantas', cast('2012-06-06' AS TIMESTAMP), 'M', 1),
  ('Gabriel Jacobi', cast('2013-08-02' AS TIMESTAMP), 'M', 1);

INSERT INTO credential (email, password) VALUES
  ('bernardo@sulzbach.com', 'yes'),
  ('felipe@bertoldo.com', 'yes'),
  ('emily@pereira.com', 'yes'),
  ('guilherme@delazeri.com', 'yes'),
  ('otavio@jacobi.com', 'yes'),
  ('pietra@castro.com', 'no'),
  ('eduardo@costa.com', 'no'),
  ('pedro@cardoso.com', 'no');

INSERT INTO administrator (name, birthdate, cpf, sex, email)
VALUES
  ('Bernardo Sulzbach', cast('1997-07-22' AS TIMESTAMP), '060.317.474-48', 'M', 'bernardo@sulzbach.com'),
  ('Felipe Bertoldo', cast('1998-08-08' AS TIMESTAMP), '808.847.295-40', 'M', 'felipe@bertoldo.com'),
  ('Emily Pereira', cast('1995-01-06' AS TIMESTAMP), '571.162.642-64', 'F', 'emily@pereira.com'),
  ('Guilherme Delazeri', cast('1997-09-23' AS TIMESTAMP), '304.320.843-98', 'M', 'guilherme@delazeri.com'),
  ('Otávio Jacobi', cast('1996-12-23' AS TIMESTAMP), '947.754.837-57', 'M', 'otavio@jacobi.com');

INSERT INTO volunteer (name, birthdate, cpf, sex, email, status, profession, address, phone, photo, criminal_record)
VALUES
  ('Pietra Castro', cast('1993-06-06' AS TIMESTAMP), '094.227.889-51', 'F', 'pietra@castro.com', 0, 'Jornalista', 'Avenida da Rua, 800', '+55-51-98846-5555', '1.jpg', '1.pdf'),
  ('Eduardo Costa', cast('1992-05-05' AS TIMESTAMP), '509.324.075-36', 'M', 'eduardo@costa.com', 1, 'Professor', 'Avenida da Rua, 900', '+55-51-98846-5555', '2.jpg', '2.pdf'),
  ('Pedro Cardoso', cast('1991-04-04' AS TIMESTAMP), '509.324.075-36', 'M', 'pedro@cardoso.com', 2, 'Ladrão', 'Avenida da Rua, 1000', '+55-51-98846-5555', '3.jpg', '3.pdf');
