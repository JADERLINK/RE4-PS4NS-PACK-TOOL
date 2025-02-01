## Change Log

**Update: 1.0.8**
<br> Melhorias no código.

**Update: 1.0.7**
<br>Corrigido bug ao extrair arquivos de imagens com 0 de tamanho.
<br>Agora, ao arrastar arquivos sobre o programa, ele vai ficar aberto após extrair/reempacotar.
<br>Os arquivos bat funcionam iguais a antes, mas agora adicionei mais um parâmetro neles.

**Update: 1.0.6**
<br>Agora, a tool aceita vários arquivos como parâmetro, assim, podendo extrair ou recompactar vários arquivos .pack;

**Update: 1.0.5**
<br> Melhorias, agora, caso tenha um GNF e um DDS com mesmo ID, ele vai considerar o GNF em vez do DDS.

**Update: 1.0.4**
<br> Melhorias no código. (versão de lançamento para essa tool)

**Update: 1.0.3**
<br>Adicionado suporte ao arquivo de formato ".reference", dentro dele vai ter o ID de uma textura anterior referenciada, serve para colocar a mesma textura em mais de um ID, ocupando o espaço em disco de um único arquivo de textura, porém na memória do jogo, vai ocupar o espaço de duas texturas.

**Update: 1.0.2**
<br>Arrumado o alinhamento dos arquivos, corrigidos bugs.

**Update: 1.0.1**
<br>Agora, além dos arquivos .dds e .tga, ele aceita arquivos .empty que representa que aquela numeração está vazia, assim pode você pular a numeração sem ocupar mais espaço no arquivo.
<br>Nota: você não pode fazer referência a numerações de arquivos "empty" no tpl, pois realmente não existe imagem ali. No lugar, será exibida a textura de botões.