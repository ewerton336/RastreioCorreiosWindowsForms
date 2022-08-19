-- isangue_ewertondev.correios_rastreamento definition

CREATE TABLE `correios_rastreamento` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID DO PACOTE',
  `CODIGO_RASTREIO` varchar(20) NOT NULL,
  `DESCRICAO_GERAL` varchar(400) DEFAULT NULL,
  `ULTIMO_PROCESSAMENTO` datetime DEFAULT NULL,
  `CONTEUDO_PACOTE` varchar(100) DEFAULT NULL,
  `PACOTE_DOS_CLIENTES` tinyint(1) DEFAULT NULL,
  `ENTREGUE` tinyint(1) DEFAULT NULL,
  `DATA_ENCERRAMENTO` datetime DEFAULT NULL COMMENT 'Data de encerramento do pacote',
  PRIMARY KEY (`ID`)
) 
