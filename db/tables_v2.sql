-- Person
CREATE TYPE SEX AS ENUM ('m', 'f');

CREATE TABLE event
(
   id             serial      NOT NULL,
   institute      text        NOT NULL,
   agegroup       text        NOT NULL,
   kidlimit       integer     NOT NULL,
   date           timestamp   NOT NULL,
   description    text        NOT NULL,
   volunteerid    integer     NOT NULL,
   createdat      timestamp   DEFAULT now(),
   status         integer     DEFAULT 0,
   justification  text
);

CREATE TABLE credentials
(
   email     text   NOT NULL,
   password  text   NOT NULL
);

CREATE TABLE person
(
   id         serial      NOT NULL,
   name       text        NOT NULL,
   birthdate  timestamp   NOT NULL,
   cpf        text        NOT NULL,
   sex        sex         NOT NULL
);

CREATE TABLE volunteer
(
   status          integer     NOT NULL,
   profession      text        NOT NULL,
   address         text        NOT NULL,
   phone           text        NOT NULL,
   photo           text        NOT NULL,
   criminalrecord  text        NOT NULL,
   credentials     text        NOT NULL,
   createdat       timestamp   DEFAULT now()
)
INHERITS (person);


CREATE TABLE administrator
(
   credentials  text        NOT NULL
)
INHERITS (person);


CREATE TABLE agegroup
(
   id     serial    NOT NULL,
   min    integer   NOT NULL,
   max    integer   NOT NULL,
   label  text      NOT NULL
);


