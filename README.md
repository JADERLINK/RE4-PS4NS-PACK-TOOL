# RE4-PS4NS-PACK-TOOL
Extract and repack RE4 PS4/NS .pack files

**Translate from Portuguese Brazil**

Programa destinado a extrair e reempacotar arquivos .pack das versão de PS4 e NS;
<br> Ao extrair será gerado um arquivo de extenção .idxps4nspack, ele será usado para o repack.

**Last Update: V.1.0.8**

## Extract

Exemplo:
<br>*RE4_PS4NS_PACK_TOOL.exe "01000000.pack"*

* Vai gerar um arquivo de nome "01000000.pack.idxps4nspack";
* Vai criar uma pasta de nome "01000000";
* Na pasta vão conter as texturas, nomeadas numericamente com 4 dígitos. Ex: 0000.dds;

## Repack

Exemplo:
<br>*RE4_PS4NS_PACK_TOOL.exe "01000000.pack.idxps4nspack"*

* Vai ler as imagens da pasta "01000000";
* A quantidade é definida pela numeração (então não deixe imagens faltando no meio);
* O nome do arquivo gerado é o mesmo nome do idxps4nspack, mas sem o .idxps4nspack;

## Avisos:
A versão de NS só aceita imagens no formato DDS (também aceita TGA [sem compresão], porém não é usado no jogo);
<br>A versão de PS4 só aceita imagens no formato GNF (e TGA [sem compresão], porém esse é pouco usado no jogo);

## Empty and Reference

Para pular numeração, uso um arquivo com o formato .empty (não vai ter imagem nessa numeração, então não a referencie no TPL) Ex: 0001.empty
<br>Para referenciar uma imagem anterior (repeti-la), use o arquivo .reference e dentro escreva o ID da imagem. Ex: 0002.reference, e o conteúdo do arquivo vai ser: "0000" , para referenciar a textura de ID 0;

**At.te: JADERLINK**
<br>2025-02-01