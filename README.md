# IAV23-DelCastilloRubio

## AUTOR
- [Ignacio del Castillo](https://github.com/NachoDelCastillo)

## PROPUESTA

El objetivo de esta practica final consiste en crear una inteligencia artificial que sea capaz de controlar a un "Jefe Final" de un videojuego del género **Souls-like**.

</br>
<table>
  <tr>
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/EldenRing_01.jpg" /></td>
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/BreathOfTheWild_01.jpg" /></td>
  </tr>
    <tr>
    <th>(Elden Ring)</th>
    <th>(The Legend Of Zelda : Breath Of The Wild)</th>
  </tr>
</table>

</br>
El genero **Souls-like** es un subgénero de los videojuegos de rol de acción que se compone de juegos que toma mecánicas y elementos de la saga souls y lo adaptan a sí mismos.

En este famoso subgénero de videojuegos llaman la atencion los llamados "Jefes", secciones del juego los cuales se basan en combates uno contra uno (hay excepciones) en el que el objetivo del jugador se convierte en derrotar a un enemigo mas fuerte que los enemigos comunes que presenta un desafio.

Estos "Jefes" suelen presentar un reto bastante dificil el cual se basa en aprender el comportamiento del enemigo y adaprtarse a el, esquivando sus ataques y creando oportunidades para atacarle entre sus patrones de ataque.
Si el comportamiento de estos enemigos no es creado correctamente, la batalla podria sentirse injusta, impredecible, o repetitiva.

En esta practica mi objetivo es crear una batalla entre el jugador y un Jefe. Programando la inteligencia artificial que controla a dicho enemigo de forma que sea predecible, que presente un desafio y mantenga la pelea interesante evitando el tipico patron de "Acercarse al jugador" y "Atacar".


## DISEÑO
Para conseguir un comporamiento variado e interesante, hago uso de diferentes niveles de proximidad alrededor del Jefe, el cual tiene en cuenta a que distancia se encuentra del jugador, determinando el area de proximidad en el que se encuentra el jugador respecto al Jefe.
En cada una de estas areas de proximidad, el Jefe podra elegir su siguiente accion entre un grupo de acciones designadas a esta area de proximidad, cada accion determinada por una probabilidad (facilmente asignable desde el editor).
Una accion no tiene porque constituir un ataque, tambien incluye otras acciones como reposicionamiento (andar/correr hacia el jugador, retroceder, situarse en el medio del campo de batalla).

Dependiendo de en que area de proximidad este el jugador, correra mas riesgo o menos de perder vida. Mas riesgo mientras mas cerca este del enemigo, y menos riesgo mientras mas lejos este.
Pero como el ataque cuerpo a cuerpo es la unica opcion para el jugador, estara obligado a analizar los movimientos del enemigo y descubrir aperturas para meterse en el area de mayor riesgo y restarle asi vida hasta vencerlo.

<td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/TrabajoFinal_01.jpg" /></td>


## INFRAESTRUCTURA


## ACCIONES
- PRUEBA : Prueba.

| **Accion** | 
|:-:|:--|:-:|
| Roll/Esquiva | Hace una voltereta, durante la misma, es invulnerable a cualquier ataque |
| Sprint | Mientras se mantenga el boton presionado el jugador aumenta su velocidad |
| LockOn | Inabilita el control manual de la camara, la cual se centra en el enemigo hasta que se desactive. En este estado el jugador cambiara sus animaciones de movimiento para no dar la espalda al enemigo |
| Ataque ligero | Realiza una animacion de atacar con la espada rapida realizando poco daño |
| Ataque pesado | Realiza una animacion de atacar con la espada lenta realizando mucho daño |



**PRUEBA**
- PRUEBA : Prueba.

| A  |  B  |  C  |  
|:-:|:--|:-:|
| ✔ | prueba: | 123 |
| ✔ | prueba: | 123 |
| ✔ | prueba: | 123 |
