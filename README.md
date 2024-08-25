# RE4-PS4NS-PACK-TOOL
Extract and repack RE4 PS4/NS .pack files

**Translate from Portuguese Brazil**

Programa destinado a extrair e reempacotar arquivos .pack das versão de PS4 e NS;
<br> Ao extrair será gerado um arquivo de extenção .idxps4nspack, ele será usado para o repack.

**update: 1.0.5**
<br> Melhorias, agora, caso tenha um GNF e um DDS com mesmo ID, ele vai considerar o GNF em vez do DDS.

**update: 1.0.4**
<br> Melhorias no código. (versão de lançamento para essa tool)

**update: 1.0.3**
<br>Adicionado suporte ao arquivo de formato ".reference", dentro dele vai ter o ID de uma textura anterior referenciada, serve para colocar a mesma textura em mais de um ID, ocupando o espaço em disco de um único arquivo de textura, porém na memória do jogo, vai ocupar o espaço de duas texturas.

**update: 1.0.2**
<br>Arrumado o alinhamento dos arquivos, corrigidos bugs.

**update: 1.0.1**
<br>Agora, além dos arquivos .dds e .tga, ele aceita arquivos .empty que representa que aquela numeração está vazia, assim pode você pular a numeração sem ocupar mais espaço no arquivo.
<br>Nota: você não pode fazer referência a numerações de arquivos "empty" no tpl, pois realmente não existe imagem ali. No lugar, será exibida a textura de botões.

## Extract

Exemplo:
<br>RE4_PS4NS_PACK_TOOL.exe "01000000.pack"

* Ira gerar um arquivo de nome "01000000.pack.idxps4nspack"
* Ira criar uma pasta de nome "01000000"
* Na pasta vão conter as texturas, nomeadas numericamente com 4 dígitos. Ex: 0000.dds

## Repack

Exemplo:
<br>RE4_PS4NS_PACK_TOOL.exe "01000000.pack.idxps4nspack"

* Vai ler as imagens da pasta "01000000"
A quantidade é definida pela numeração, (então não deixe imagens faltando no meio).
* O nome do arquivo gerado é o mesmo nome do idxps4nspack, mas sem o .idxps4nspack;

## Avisos:
A versão de NS só aceita imagens no formato DDS (também aceita TGA, porém não é usado no jogo);
<br>A versão de PS4 só aceita imagens no formato GNF (e TGA, porém esse é pouco usado no jogo);

**At.te: JADERLINK**
<br>2024-08-25