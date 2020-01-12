create database saim;

create table societe(
	id_societe int NOT NULL AUTO_INCREMENT primary key,
	nom_societe varchar(25),
	nif varchar(11),
	stat varchar(17),
	adresse varchar(50),
	activite text,
	mail varchar(50),
	num_telephone varchar(20)
);

create table personnel(
	id_personnel int NOT NULL AUTO_INCREMENT primary key,
	id_societe int,
	nom varchar(50),
	prenoms varchar(50),
	num_telephone varchar(20),
	poste varchar(50),
	foreign key(id_societe) references societe(id_societe)
);


