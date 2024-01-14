CREATE TABLE users
(
    id         uuid PRIMARY KEY,
    name       varchar(255) NOT NULL,
    surname    varchar(255) NOT NULL,
    patronymic varchar(255) NOT NULL,
    age        int          NOT NULL,
    salary     int          NOT NULL,
    email      varchar(255) NOT NULL UNIQUE
);

CREATE TYPE typeOfCard AS ENUM ('basic', 'priority');

CREATE TYPE statusOfCard AS ENUM ('activate', 'deactivate');

CREATE TABLE cards
(
    id        uuid PRIMARY KEY,
    code      bigint          NOT NULL,
    cvv       int          NOT NULL,
    user_id   uuid         NOT NULL,
    type_card typeOfCard   NOT NULL,
    balance   int          NOT NULL,
    life_time DATE         NOT NULL,
    status    statusOfCard NOT NULL,
    CONSTRAINT fk_users FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE TABLE transactions
(
    id          uuid PRIMARY KEY,
    card_id     uuid NOT NULL,
    create_date DATE NOT NULL,
    sum         int  NOT NULL,
    to_user_id  uuid NOT NULL,
    CONSTRAINT fk_cards FOREIGN KEY (card_id) REFERENCES cards (id),
    CONSTRAINT fk_users FOREIGN KEY (to_user_id) REFERENCES users (id)
)