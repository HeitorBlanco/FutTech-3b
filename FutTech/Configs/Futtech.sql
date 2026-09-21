# Banco de Dados FutTech

create database bd_futtech;
use bd_futtech;

create table Escolinha(
id_esc int primary key auto_increment,
nome_esc varchar(300),
cnpj_esc varchar(100),
ativo_esc boolean
);

create table UsuarioSistema(
id_usu int primary key auto_increment,
nome_usu varchar(300),
email_usu varchar(300),
senha_usu varchar(300),
perfil_usu varchar(100),
cargo_usu varchar(300),
ativo_usu boolean,
perfil_descricao_usu varchar(300),
rota_inicial_usu varchar(300),
id_esc_fk int,
foreign key(id_esc_fk) references Escolinha(id_esc)
);

create table Treinador(
id_trei int primary key auto_increment,
nome_trei varchar(300),
cargo_trei varchar(300),
ativo_trei boolean,
id_esc_fk int,
id_usu_fk int,
foreign key(id_esc_fk) references Escolinha(id_esc),
foreign key(id_usu_fk) references UsuarioSistema(id_usu)
);

create table Turma(
id_tur int primary key auto_increment,
nome_tur varchar(300),
categoria_tur varchar(300),
dias_de_treino_tur varchar(300),
horario_tur time,
ativa_tur boolean,
id_trei_fk int,
id_esc_fk int,
foreign key(id_trei_fk) references Treinador(id_trei),
foreign key(id_esc_fk) references Escolinha(id_esc)
);

create table Aluno(
id_alu int primary key auto_increment,
nome_alu varchar(300),
responsavel_alu varchar(300),
data_nascimento_alu date,
id_tur_fk int,
ativo_alu boolean,
foreign key(id_tur_fk) references Turma(id_tur)
);

create table Mensalidade(
id_men int primary key auto_increment,
id_alu_fk int,
competencia_men date,
valor_men float,
vencimento_men date,
data_pagamento_men date,
status_men varchar(100),
foreign key(id_alu_fk) references Aluno(id_alu)
);

create table RegistroPresenca(
id_pre int primary key auto_increment,
id_alu_fk int,
id_tur_fk int,
data_pre date,
presente_pre boolean,
foreign key(id_alu_fk) references Aluno(id_alu),
foreign key(id_tur_fk) references Turma(id_tur)
);

create table AvaliacaoAluno(
id_ava int primary key auto_increment,
id_alu_fk int,
id_tur_fk int,
id_trei_fk int,
data_ava date,
nota_tecnica_ava int,
nota_fisica_ava int,
nota_tatica_ava int,
nota_comportamental_ava int,
observacoes_ava varchar(500),
media_ava float,
foreign key(id_alu_fk) references Aluno(id_alu),
foreign key(id_tur_fk) references Turma(id_tur),
foreign key(id_trei_fk) references Treinador(id_trei)
);

create table Comunicado(
id_com int primary key auto_increment,
titulo_com varchar(300),
conteudo_com varchar(500),
publicado_em_com date,
publicado_as_com time,
autor_com varchar(300),
categoria_com varchar(300),
destacado_com boolean,
ativo_com boolean,
id_esc_fk int,
id_usu_fk int,
foreign key(id_esc_fk) references Escolinha(id_esc),
foreign key(id_usu_fk) references UsuarioSistema(id_usu)
);

create table Financeiro(
id_fin int primary key auto_increment,
recebido_mes_atual_fin decimal(10,2),
pendentes_fin int,
total_a_receber_fin decimal(10,2),
total_pago_fin decimal(10,2)
);


insert into Escolinha values(null, 'FutTech Academy', '12.345.678/0001-90', true);
insert into Escolinha values(null, 'Arena Bola Kids', '98.765.432/0001-10', true);
insert into Escolinha values(null, 'Escolinha Pimentel', '11.111.111/0001-11', true);
insert into Escolinha values(null, 'Escolinha Blanco', '22.222.222/0001-22', true);
insert into Escolinha values(null, 'Escolinha Moreira', '33.333.333/0001-33', true);

insert into UsuarioSistema values(null, 'Administrador', 'admin@futtech.com', 'admin123', 'Administrador', 'Coordenador', true, 'Administrador', '/admin/dashboard', 1);
insert into UsuarioSistema values(null, 'Marcos Oliveira', 'treinador@futtech.com', 'treinador123', 'Treinador', 'Treinador', true, 'Treinador', '/treinador/dashboard', 1);
insert into UsuarioSistema values(null, 'Guilherme Pimentel', 'guilherme@futtech.com', 'guilherme123', 'Administrador', 'Coordenador', true, 'Administrador', '/admin/dashboard', 1);
insert into UsuarioSistema values(null, 'Heitor Blanco', 'heitor@futtech.com', 'heitor123', 'Treinador', 'Treinador', true, 'Treinador', '/treinador/dashboard', 1);
insert into UsuarioSistema values(null, 'Luís Gustavo', 'responsavel@futtech.com', 'resp123', 'Responsavel', 'Responsavel', true, 'Responsavel', '/responsavel/dashboard', 1);

insert into Treinador values(null, 'Marcos Oliveira', 'Treinador principal', true, 1, 2);
insert into Treinador values(null, 'Ana Martins', 'Auxiliar tecnica', true, 1, null);
insert into Treinador values(null, 'Guilherme Pimentel', 'Coordenador tecnico', true, 1, 3);
insert into Treinador values(null, 'Heitor Blanco', 'Treinador', true, 1, 4);
insert into Treinador values(null, 'Gustavo Moreira', 'Preparador fisico', true, 1, null);

insert into Turma values(null, 'Sub-09 Manha', 'Sub-09', 'Segunda e Quarta', '08:00:00', true, 1, 1);
insert into Turma values(null, 'Sub-11 Tarde', 'Sub-11', 'Terca e Quinta', '15:30:00', true, 2, 1);
insert into Turma values(null, 'Sub-13 Noite', 'Sub-13', 'Segunda e Sexta', '18:00:00', true, 3, 1);
insert into Turma values(null, 'Sub-15 Manha', 'Sub-15', 'Terca e Quinta', '09:30:00', true, 4, 1);
insert into Turma values(null, 'Sub-17 Tarde', 'Sub-17', 'Quarta e Sexta', '16:00:00', true, 5, 1);

insert into Aluno values(null, 'Lucas Silva', 'Paula Santos', '2016-05-12', 1, true);
insert into Aluno values(null, 'Pedro Santos', 'Paula Santos', '2014-10-21', 2, true);
insert into Aluno values(null, 'Gustavo Moreira', 'Guilherme Pimentel', '2012-03-08', 3, true);
insert into Aluno values(null, 'Samuel Butzke', 'Heitor Blanco', '2011-07-19', 4, true);
insert into Aluno values(null, 'Luís Gustavo', 'Guilherme Pimentel', '2010-11-25', 5, true);

insert into Mensalidade values(null, 1, '2026-08-01', 150.00, '2026-08-10', null, 'Pendente');
insert into Mensalidade values(null, 2, '2026-07-01', 150.00, '2026-07-10', '2026-07-09', 'Pago');
insert into Mensalidade values(null, 3, '2026-08-01', 160.00, '2026-08-10', null, 'Pendente');
insert into Mensalidade values(null, 4, '2026-08-01', 160.00, '2026-08-10', '2026-08-08', 'Pago');
insert into Mensalidade values(null, 5, '2026-08-01', 170.00, '2026-08-10', null, 'Atrasada');

insert into RegistroPresenca values(null, 1, 1, '2026-08-03', true);
insert into RegistroPresenca values(null, 2, 2, '2026-08-03', false);
insert into RegistroPresenca values(null, 3, 3, '2026-08-04', true);
insert into RegistroPresenca values(null, 4, 4, '2026-08-04', true);
insert into RegistroPresenca values(null, 5, 5, '2026-08-04', false);

insert into AvaliacaoAluno values(null, 1, 1, 1, '2026-08-01', 8, 9, 7, 9, 'Boa evolucao no passe e na disciplina.', 8.25);
insert into AvaliacaoAluno values(null, 2, 2, 2, '2026-08-01', 7, 8, 8, 8, 'Precisa reforcar finalizacao.', 7.75);
insert into AvaliacaoAluno values(null, 3, 3, 3, '2026-08-04', 9, 8, 8, 9, 'Gustavo Moreira mostrou boa lideranca em campo.', 8.50);
insert into AvaliacaoAluno values(null, 4, 4, 4, '2026-08-04', 8, 7, 9, 8, 'Samuel Butzke teve bom desempenho tatico.', 8.00);
insert into AvaliacaoAluno values(null, 5, 5, 5, '2026-08-04', 7, 8, 7, 9, 'Luís Gustavo participou bem das atividades.', 7.75);

insert into Comunicado values(null, 'Reuniao de responsaveis', 'Reuniao mensal marcada para sexta-feira as 19h.', '2026-08-02', '09:00:00', 'Administrador', 'Geral', true, true, 1, 1);
insert into Comunicado values(null, 'Treino especial', 'No sabado teremos treino especial de fundamentos.', '2026-08-03', '14:30:00', 'Marcos Oliveira', 'Treinos', false, true, 1, 2);
insert into Comunicado values(null, 'Aviso de Guilherme Pimentel', 'Guilherme Pimentel confirmou a reuniao tecnica da semana.', '2026-08-04', '08:30:00', 'Guilherme Pimentel', 'Geral', false, true, 1, 3);
insert into Comunicado values(null, 'Aviso de Heitor Blanco', 'Heitor Blanco informou treino de fundamentos para a turma Sub-15.', '2026-08-04', '10:00:00', 'Heitor Blanco', 'Treinos', true, true, 1, 4);
insert into Comunicado values(null, 'Aviso de Luís Gustavo', 'Luís Gustavo pediu atencao aos vencimentos das mensalidades.', '2026-08-04', '11:00:00', 'Luís Gustavo', 'Financeiro', false, true, 1, 5);

insert into Financeiro values(null, 310.00, 3, 480.00, 310.00);
insert into Financeiro values(null, 450.00, 2, 620.00, 450.00);
insert into Financeiro values(null, 600.00, 4, 850.00, 600.00);
insert into Financeiro values(null, 780.00, 3, 1020.00, 780.00);
insert into Financeiro values(null, 950.00, 2, 1150.00, 950.00);