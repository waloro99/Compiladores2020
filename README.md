# Compiladores2020
Proyecto... Walter Orozco/Ricardo Chian


## Integrantes del grupo:
> Walter Orozco - 1170917
> Ricardo Chian - 1103916

## Funcionamiento del programa:
- El funcionamiento del programa se divide en 3 fases las cuales se describiran a continuación:
  *Cargar el archivo*
    -Para comenzar el programa por medio de un boton que dejara buscar al usuario un archivo con cualquier extensión para que este sea analizado por el programa.
    -Luego en un textBox se encontrara la ubicación del archivo que se escogió y el usuario podra verificar o modificarlo antes de su lectura.
    -Por último el usuario debe de presionar el boton "ACEPTAR", lo cual activara que se bloquee el textBox y el boton para que el programa analice el archivo.
  *Analisis del archivo*
    -Primero se realiza una lectura de todo el archivo que se cargo, y lo divide por lineas en un arreglo para su análisis posteriormente.
    -Luego se estara analizando línea por línea lo que sucede en el archivo, entonces el porgrama agarra una línea y busca si hay un comentario o string.
    -Si no hay ningún commentario o string, el programa pasara a análizar por medio del primer filtro.
    -En el primer filtro se análiza quitando todos los espacios en blanco, y colocando cada frase en un arreglo.
    -Despues este texto separado entra en el primer filtro que contiene los casos más básicos y si cumple con algún match de las ER, se ingresa a una lista de tokens.
    -Si este texto no hace ningún match pasa a un segundo filtro, el cual realiza una verificacion parte por parte para saber que tipo de token es.
    -Ahora bien si el archivo en la linea encontro un string o comentario entra a un filtro y separa lo que viene antes o despues de estos.
    -Luego estas cadenas que se encontraron antes o despues van hacia el primer filtro y lo que se encontro se analiza por separado.
    -Finalmente todos estos llegan a un match y si no es porque posiblemente se encontro un error que se estara manejando e indicando al usuario.
  *Salida del archivo*
    -Para la salida del archivo, luego de todo el análisis del mismo se envia hacia la misma dirección un archivo de salida con extension .out.
    -Este archivo guardado en esa ubicación muestra los tokens identificados y los errores que se encontraron durante el análisis.
    -Y finalmente el programa le muestra al usuario unicamente los errores encontrados en el archivo o si no hay ninguno se muestra que se lee correctamente.
    
## Manejo de Errores:
  *Errores*
    -El manejo de errores es robusto debido a que siempre existe un match posible, en el cual entre lo que se analice y si no cumple con alguna norma del analisis lexico entonces es reportado como error, los errores se envian conforme aparecen y estos no cancelan el analisis del resto del programa, sino solo los deja guardados para posteriormente mostrarlos al usuario por medio de un mensaje en pantalla y escritos en el archivo de salida .out.
    
## Por lo tanto consideramos de que con este análisis el programa es robusto y tendra correcto análisis lexico de el archivo.

