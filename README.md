# Bloody Kitchen
Juego siguiendo un tutorial, pero lo voy a modificar para que sea un juego gore y pierda toda la inocencia, además de agregarle otras features.

Consiste en un juego de cocina, donde uno prepara platos y los entrega a los comensales.

## Kitchen Chaos
Nombre del juego original. Juego inocente de cocina. Para que tenga mi toque, le fui agregando/modificando ciertas cosas

#### Objetivo
El objetivo del juego es ganar la mayor cantidad de plata preparando platos durante el tiempo de juego.
El juego original consistia en solamente preparar platos.

#### Platos e ingredientes 
Los platos a preparas son hamburguesas / ensaladas y los ingredientes disponibles son:
- Carne
- Pan
- Lechuga
- Tomate
- Queso

En el juego original, solamente se preparan hamburguesas y esta limitado a cierto menu debido al diseño de ese requerimiento. Como elemento visual de todas las hamburguesas posibles, en el tutorial el man utilizó
el modelo visual de una hamburguesa completa (pan, carne, queso lechuga y tomate) para representar a todas las hamburguesas posibles. Las demás opciones del menú salían de activar o desactivar ingredientes de este elemento visual.
Yo lo modifique para que se la combinacion de ingredientes sea infinita. El modelo visual de la hamburguesa va cambiando comforme uno va agregando nuevos ingredientes.

#### Mesadas
Hay diferentes tipos de mesadas con los que se pueden interactuar
- Clear Counter: mesada sin nada, sirve para apoyar algo y liberarte las manos para poder hacer otras tareas. Bueno tambien sirve para apoyar los platos y armarlos.
- Stove Counter: Es el hornito con la hornalla y la sartén, donde podes apoyar algo y se pone a cocinar.
- Container Counter: Es un spawn de cualquiera de los ingredientes
- Thrash Counter: tachito de basura para tirar cosas.
- Plate Counter: mesada que spawnea platos

Le agregué un efecto visual al horno cuando se te quema la comida para que le salga humito y se escuche una alarma de incendios.

#### Pedidos
A diferencia del juego original, cada pedido tiene un precio y un tiempo asociado. Si no terminas el plato en ese tiempo, perdes la venta. Además, al estar cerca de alcanzar este tiempo de entrega, se dispara un efecto visual (titila el elemento OrdenUI) y un efecto de sonido.

#### Niveles de dificultad
- Facil: tiempo de juego: 120 seg; 2 recetas activas simultaneamente; Pedidos sin tiempo asociado
- Dificil: tiempo de juego: 100 seg; 4 recetas activas simultaneamente; Pedidos con tiempo asociado

# Proximamente
Cuando termine de hacer las modificaciones pendientes, lo voy a ir migrando a algo mas gore.
