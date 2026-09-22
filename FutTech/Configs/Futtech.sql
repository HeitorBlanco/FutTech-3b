CREATE DATABASE bd_futtech;
USE bd_futtech;

CREATE TABLE Escolinha (
    id_esc INT PRIMARY KEY AUTO_INCREMENT,
    nome_esc VARCHAR(300),
    cnpj_esc VARCHAR(100),
    ativo_esc BOOLEAN
);

CREATE TABLE UsuarioSistema (
    id_usu INT PRIMARY KEY AUTO_INCREMENT,
    nome_usu VARCHAR(300),
    email_usu VARCHAR(300),
    senha_usu VARCHAR(300),
    perfil_usu VARCHAR(100),
    cargo_usu VARCHAR(300),
    ativo_usu BOOLEAN,
    perfil_descricao_usu VARCHAR(300),
    rota_inicial_usu VARCHAR(300),
    id_esc_fk INT,
    FOREIGN KEY (id_esc_fk) REFERENCES Escolinha(id_esc)
);

CREATE TABLE Treinador (
    id_trei INT PRIMARY KEY AUTO_INCREMENT,
    nome_trei VARCHAR(300),
    cargo_trei VARCHAR(300),
    ativo_trei BOOLEAN,
    id_esc_fk INT,
    id_usu_fk INT,
    FOREIGN KEY (id_esc_fk) REFERENCES Escolinha(id_esc),
    FOREIGN KEY (id_usu_fk) REFERENCES UsuarioSistema(id_usu)
);

CREATE TABLE Turma (
    id_tur INT PRIMARY KEY AUTO_INCREMENT,
    nome_tur VARCHAR(300),
    categoria_tur VARCHAR(300),
    dias_de_treino_tur VARCHAR(300),
    horario_tur TIME,
    ativa_tur BOOLEAN,
    id_trei_fk INT,
    id_esc_fk INT,
    FOREIGN KEY (id_trei_fk) REFERENCES Treinador(id_trei),
    FOREIGN KEY (id_esc_fk) REFERENCES Escolinha(id_esc)
);

CREATE TABLE Aluno (
    id_alu INT PRIMARY KEY AUTO_INCREMENT,
    nome_alu VARCHAR(300),
    responsavel_alu VARCHAR(300),
    data_nascimento_alu DATE,
    id_tur_fk INT,
    ativo_alu BOOLEAN,
    FOREIGN KEY (id_tur_fk) REFERENCES Turma(id_tur)
);

CREATE TABLE ResponsavelAluno (
    id_res_alu INT PRIMARY KEY AUTO_INCREMENT,
    id_usu_fk INT,
    id_alu_fk INT,
    parentesco_res_alu VARCHAR(100),
    principal_res_alu BOOLEAN,
    FOREIGN KEY (id_usu_fk) REFERENCES UsuarioSistema(id_usu),
    FOREIGN KEY (id_alu_fk) REFERENCES Aluno(id_alu)
);

CREATE TABLE Mensalidade (
    id_men INT PRIMARY KEY AUTO_INCREMENT,
    id_alu_fk INT,
    competencia_men DATE,
    valor_men DECIMAL(10,2),
    vencimento_men DATE,
    data_pagamento_men DATE,
    status_men VARCHAR(100),
    FOREIGN KEY (id_alu_fk) REFERENCES Aluno(id_alu)
);

CREATE TABLE RegistroPresenca (
    id_pre INT PRIMARY KEY AUTO_INCREMENT,
    id_alu_fk INT,
    id_tur_fk INT,
    data_pre DATE,
    presente_pre BOOLEAN,
    FOREIGN KEY (id_alu_fk) REFERENCES Aluno(id_alu),
    FOREIGN KEY (id_tur_fk) REFERENCES Turma(id_tur)
);

CREATE TABLE AvaliacaoAluno (
    id_ava INT PRIMARY KEY AUTO_INCREMENT,
    id_alu_fk INT,
    id_tur_fk INT,
    id_trei_fk INT,
    data_ava DATE,
    nota_tecnica_ava INT,
    nota_fisica_ava INT,
    nota_tatica_ava INT,
    nota_comportamental_ava INT,
    observacoes_ava VARCHAR(500),
    media_ava DECIMAL(4,2),
    FOREIGN KEY (id_alu_fk) REFERENCES Aluno(id_alu),
    FOREIGN KEY (id_tur_fk) REFERENCES Turma(id_tur),
    FOREIGN KEY (id_trei_fk) REFERENCES Treinador(id_trei)
);

CREATE TABLE Comunicado (
    id_com INT PRIMARY KEY AUTO_INCREMENT,
    titulo_com VARCHAR(300),
    conteudo_com VARCHAR(500),
    publicado_em_com DATE,
    publicado_as_com TIME,
    autor_com VARCHAR(300),
    categoria_com VARCHAR(300),
    destacado_com BOOLEAN,
    ativo_com BOOLEAN,
    id_esc_fk INT,
    id_usu_fk INT,
    FOREIGN KEY (id_esc_fk) REFERENCES Escolinha(id_esc),
    FOREIGN KEY (id_usu_fk) REFERENCES UsuarioSistema(id_usu)
);

CREATE TABLE Financeiro (
    id_fin INT PRIMARY KEY AUTO_INCREMENT,
    recebido_mes_atual_fin DECIMAL(10,2),
    pendentes_fin INT,
    total_a_receber_fin DECIMAL(10,2),
    total_pago_fin DECIMAL(10,2)
);

INSERT INTO Escolinha VALUES
(NULL, 'FutTech Academy', '12.345.678/0001-90', TRUE),
(NULL, 'Arena Bola Kids', '98.765.432/0001-10', TRUE),
(NULL, 'Escolinha Pimentel', '11.111.111/0001-11', TRUE),
(NULL, 'Escolinha Blanco', '22.222.222/0001-22', TRUE),
(NULL, 'Escolinha Moreira', '33.333.333/0001-33', TRUE);

INSERT INTO UsuarioSistema VALUES
(NULL, 'Administrador', 'admin@futtech.com', 'admin123', 'Administrador', 'Coordenador', TRUE, 'Administrador', '/admin/dashboard', 1),
(NULL, 'Marcos Oliveira', 'treinador@futtech.com', 'treinador123', 'Treinador', 'Treinador', TRUE, 'Treinador', '/treinador/dashboard', 1),
(NULL, 'Guilherme Pimentel', 'guilherme@futtech.com', 'guilherme123', 'Administrador', 'Coordenador', TRUE, 'Administrador', '/admin/dashboard', 1),
(NULL, 'Heitor Blanco', 'heitor@futtech.com', 'heitor123', 'Treinador', 'Treinador', TRUE, 'Treinador', '/treinador/dashboard', 1),
(NULL, 'Luís Gustavo', 'responsavel@futtech.com', 'resp123', 'Responsavel', 'Responsavel', TRUE, 'Responsavel', '/responsavel/dashboard', 1);

INSERT INTO Treinador VALUES
(NULL, 'Marcos Oliveira', 'Treinador principal', TRUE, 1, 2),
(NULL, 'Ana Martins', 'Auxiliar técnica', TRUE, 1, NULL),
(NULL, 'Guilherme Pimentel', 'Coordenador técnico', TRUE, 1, 3),
(NULL, 'Heitor Blanco', 'Treinador', TRUE, 1, 4),
(NULL, 'Gustavo Moreira', 'Preparador físico', TRUE, 1, NULL);

INSERT INTO Turma VALUES
(NULL, 'Sub-09 Manhã', 'Sub-09', 'Segunda e Quarta', '08:00:00', TRUE, 1, 1),
(NULL, 'Sub-11 Tarde', 'Sub-11', 'Terça e Quinta', '15:30:00', TRUE, 2, 1),
(NULL, 'Sub-13 Noite', 'Sub-13', 'Segunda e Sexta', '18:00:00', TRUE, 3, 1),
(NULL, 'Sub-15 Manhã', 'Sub-15', 'Terça e Quinta', '09:30:00', TRUE, 4, 1),
(NULL, 'Sub-17 Tarde', 'Sub-17', 'Quarta e Sexta', '16:00:00', TRUE, 5, 1);

INSERT INTO Aluno VALUES
(NULL, 'Lucas Silva', 'Paula Santos', '2016-05-12', 1, TRUE),
(NULL, 'Pedro Santos', 'Paula Santos', '2014-10-21', 2, TRUE),
(NULL, 'Gustavo Moreira', 'Guilherme Pimentel', '2012-03-08', 3, TRUE),
(NULL, 'Samuel Butzke', 'Heitor Blanco', '2011-07-19', 4, TRUE),
(NULL, 'Luís Gustavo', 'Guilherme Pimentel', '2010-11-25', 5, TRUE);

INSERT INTO ResponsavelAluno VALUES
(NULL, 5, 1, 'Responsável', TRUE),
(NULL, 5, 2, 'Responsável', TRUE),
(NULL, 5, 3, 'Responsável', TRUE),
(NULL, 5, 4, 'Responsável', TRUE),
(NULL, 5, 5, 'Responsável', TRUE);

INSERT INTO Mensalidade VALUES
(NULL, 1, '2026-08-01', 150.00, '2026-08-10', NULL, 'Pendente'),
(NULL, 2, '2026-07-01', 150.00, '2026-07-10', '2026-07-09', 'Pago'),
(NULL, 3, '2026-08-01', 160.00, '2026-08-10', NULL, 'Pendente'),
(NULL, 4, '2026-08-01', 160.00, '2026-08-10', '2026-08-08', 'Pago'),
(NULL, 5, '2026-08-01', 170.00, '2026-08-10', NULL, 'Atrasada');

INSERT INTO RegistroPresenca VALUES
(NULL, 1, 1, '2026-08-03', TRUE),
(NULL, 2, 2, '2026-08-03', FALSE),
(NULL, 3, 3, '2026-08-04', TRUE),
(NULL, 4, 4, '2026-08-04', TRUE),
(NULL, 5, 5, '2026-08-04', FALSE);

INSERT INTO AvaliacaoAluno VALUES
(NULL, 1, 1, 1, '2026-08-01', 8, 9, 7, 9, 'Boa evolução no passe e na disciplina.', 8.25),
(NULL, 2, 2, 2, '2026-08-01', 7, 8, 8, 8, 'Precisa reforçar finalização.', 7.75),
(NULL, 3, 3, 3, '2026-08-04', 9, 8, 8, 9, 'Gustavo Moreira mostrou boa liderança em campo.', 8.50),
(NULL, 4, 4, 4, '2026-08-04', 8, 7, 9, 8, 'Samuel Butzke teve bom desempenho tático.', 8.00),
(NULL, 5, 5, 5, '2026-08-04', 7, 8, 7, 9, 'Luís Gustavo participou bem das atividades.', 7.75);

INSERT INTO Comunicado VALUES
(NULL, 'Reunião de responsáveis', 'Reunião mensal marcada para sexta-feira às 19h.', '2026-08-02', '09:00:00', 'Administrador', 'Geral', TRUE, TRUE, 1, 1),
(NULL, 'Treino especial', 'No sábado teremos treino especial de fundamentos.', '2026-08-03', '14:30:00', 'Marcos Oliveira', 'Treinos', FALSE, TRUE, 1, 2),
(NULL, 'Aviso de Guilherme Pimentel', 'Guilherme Pimentel confirmou a reunião técnica da semana.', '2026-08-04', '08:30:00', 'Guilherme Pimentel', 'Geral', FALSE, TRUE, 1, 3),
(NULL, 'Aviso de Heitor Blanco', 'Heitor Blanco informou treino de fundamentos para a turma Sub-15.', '2026-08-04', '10:00:00', 'Heitor Blanco', 'Treinos', TRUE, TRUE, 1, 4),
(NULL, 'Aviso de Luís Gustavo', 'Luís Gustavo pediu atenção aos vencimentos das mensalidades.', '2026-08-04', '11:00:00', 'Luís Gustavo', 'Financeiro', FALSE, TRUE, 1, 5);

INSERT INTO Financeiro VALUES
(NULL, 310.00, 3, 480.00, 310.00),
(NULL, 450.00, 2, 620.00, 450.00),
(NULL, 600.00, 4, 850.00, 600.00),
(NULL, 780.00, 3, 1020.00, 780.00),
(NULL, 950.00, 2, 1150.00, 950.00);