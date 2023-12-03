-- ---------- MIGRONDI:UP:1701589598955 --------------
-- Write your Up migrations here
CREATE TABLE User(
    Id INTEGER PRIMARY KEY,
    Name Text
)
-- ---------- MIGRONDI:DOWN:1701589598955 --------------
-- Write how to revert the migration here
DROP TABLE User
