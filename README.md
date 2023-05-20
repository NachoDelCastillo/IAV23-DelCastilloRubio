# IAV23-DelCastilloRubio

## IMPORTANTE
En esta practica se empleara hasta el ultimo dia permitido para terminar y pulir la practica.

El estado actual del juego (viernes 19 de mayo de 2023) constiste en:
- El personaje que controla el jugador completamente terminado, con todas las acciones mencionadas mas abajo.
- La infraestructura y la interfaz que muestra la salud actual del jugador.
- Un enemigo con un funcionamiento basico de momento, en el que puede perseguir al jugador y atacarlo. Tambien puede recibir daño.

Aqui hay un breve video mostrando las funcionalidades basicas que hay por el momento (tarda un poco en cargarse)
<td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/EstadoProyecto.gif" /></td>


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
  </br>
    <tr>
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/Bloodborne_01.jpg" /></td>
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/Bloodborne_01.jpg" /></td>
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

</br></br>
## DISEÑO
Para conseguir un comporamiento variado e interesante, hago uso de diferentes niveles de proximidad alrededor del Jefe, el cual tiene en cuenta a que distancia se encuentra del jugador, determinando el area de proximidad en el que se encuentra el jugador respecto al Jefe.
En cada una de estas areas de proximidad, el Jefe podra elegir su siguiente accion entre un grupo de acciones designadas a esta area de proximidad, cada accion determinada por una probabilidad (facilmente asignable desde el editor).
Una accion no tiene porque constituir un ataque, tambien incluye otras acciones como reposicionamiento (andar/correr hacia el jugador, retroceder, situarse en el medio del campo de batalla).

Dependiendo de en que area de proximidad este el jugador, correra mas riesgo o menos de perder vida. Mas riesgo mientras mas cerca este del enemigo, y menos riesgo mientras mas lejos este.
Pero como el ataque cuerpo a cuerpo es la unica opcion para el jugador, estara obligado a analizar los movimientos del enemigo y descubrir aperturas para meterse en el area de mayor riesgo y restarle asi vida hasta vencerlo.

<td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/TrabajoFinal_01.jpg" /></td>

</br></br>
## INFRAESTRUCTURA

Para el correcto funcionamiento del gameplay en este juego, han sido necesarios muchos scripts extra que no tienen nada que ver con la Inteligencia Artificial del Enemigo.
Los unicos scripts relevantes oara la inteligencia artificial del enemigo son los siguientes (Assets/Scripts/A.I):

- ****EnemyManager :**** </br> Es el script principal del enemigo, en el se organiza el resto de funcionalidades repartidas en el resto de scripts. Tambien se ocupa de calcular el area de proximidad en el que se encuentra el jugador y dependiendo de la misma elegir nuevas acciones durante todo el combate.

- ****EnemyAction / EnemyAttackAction :**** </br>ScriptableObjects que definen las posibles acciones del Jefe, almacenando informacion (dependiendo del tipo de accion) como el angulo necesario entre el frente del enemigo y el jugador para poder realizar la accion, el area de proximidad a la que pertenece esta accion, la probabilidad de que esta accion se eliga sobre las demas o el tiempo de recuperacion (tiempo que el enemigo debe esperar antes de realizar otro ataque).

- ****EnemyLocomotion :**** </br>Se encarga de manejar el movimiento y rotacion del enemigo en todo momento manipulando el rigidbody, tambien activando y desactivando el NavMeshAgent cuando sea necesario, y calculando rotaciones con "Slerp" para un movimiento fluido

- ****EnemyAnimator :**** </br>Encapsula todo lo que tiene que ver con el manejo de las animaciones del enemigo.

- ****EnemyStats :**** </br>Almacena informacion acerca de la vida restante del enemigo y funciones relacionadas con la misma, como recibir daño del jugador. Tambien actualiza la interfaz para mostrarselo en todo momento al jugador por pantalla.

</br></br>
## ACCIONES

**JEFE**

(Aun por determinar)

**JUGADOR**
| **ACCION** | **TECLADO** | **MANDO** | **DESCRIPCION** |
|:-:|:-:|:-:|:-:|
| Roll/Esquiva | Left Shift | Left Shoulder | Realiza una voltereta, durante la misma, es invulnerable a cualquier ataque |
| Sprint | Left Control | Right Joystick Press | Mientras se mantenga el boton presionado el jugador aumenta su velocidad |
| LockOn/LockOff | F | Right Joystick Press| Inabilita el control manual de la camara, la cual se centra en el enemigo hasta que se desactive. En este estado el jugador cambiara sus animaciones de movimiento para no dar la espalda al enemigo |
| Ataque ligero | Click Izquierdo | Right Shoulder (R1) | Realiza una animacion de atacar con la espada rapida realizando poco daño |
| Ataque pesado | Click Derecho | Right Trigger (R2) | Realiza una animacion de atacar con la espada lenta realizando mucho daño |

**PRUEBA**
- PRUEBA : Prueba.

| A  |  B  |  C  |  
|:-:|:--|:-:|
| ✔ | prueba: | 123 |
| ✔ | prueba: | 123 |
| ✔ | prueba: | 123 |
