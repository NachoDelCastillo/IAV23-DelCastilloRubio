# IAV23-DelCastilloRubio

## FLAME SOULS
<td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/Portada.PNG" /></td>

## IMPORTANTE
En esta practica se empleara hasta el ultimo dia permitido para terminar y pulir la practica.

Estado del juego en el dia viernes 25 de mayo de 2023 / Video explicatorio:


Estado del juego en el dia viernes 19 de mayo de 2023:
- El personaje que controla el jugador completamente terminado, con todas las acciones mencionadas mas abajo.
- La infraestructura y la interfaz que muestra la salud actual del jugador.
- Un enemigo con un funcionamiento basico de momento, en el que puede perseguir al jugador y atacarlo. Tambien puede recibir daño.
Aqui hay un breve video mostrando las funcionalidades basicas que hay por el momento (tarda un poco en cargarse)
<td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/EstadoProyecto.gif" /></td>


## AUTOR
- [Ignacio del Castillo](https://github.com/NachoDelCastillo)

## PROPUESTA

El objetivo de esta practica final consiste en crear una inteligencia artificial que sea capaz de controlar a un "Jefe Final" de un videojuego del género **Souls-like**.

- [Objetivo (obviamente no tan laborioso ni con tan buenos graficos)](https://www.youtube.com/watch?v=jQhfP79H09g&ab_channel=Shirrako)

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
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/DarkSouls3_01.jpg" /></td>
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/Bloodborne_01.jpg" /></td>
  </tr>
    <tr>
    <th>(Dark Souls 3)</th>
    <th>(Bloodborne)</th>
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
</br>
****SCRIPTS DEL JEFE****</br>

Para el funcionamiento del juego presentado, han sido necesarios muchos scripts extra que no tienen nada que ver con la Inteligencia Artificial del Enemigo. Por lo que no se mencionaran en la documentación.
Los scripts relevantes para la inteligencia artificial del enemigo son los siguientes: (Assets/Scripts/A.I):

- ****EnemyManager :**** </br> 
Es el script principal del enemigo, en el se organiza el resto de funcionalidades repartidas en el resto de scripts.
Tambien es el encargado de gestionar el funcionamiento y transicion de estados de la maquina de estados, la cual se emplea para determinar las acciones del Jefe en todo momento

- ****EnemyAnimator :**** </br>Encapsula todo lo que tiene que ver con el manejo de las animaciones del enemigo y su comunicacion con el resto de scripts

- ****EnemyStats :**** </br>Almacena informacion acerca de la vida restante del enemigo y funciones relacionadas con la misma, como recibir daño del jugador. Tambien actualiza la interfaz para mostrarselo en todo momento al jugador por pantalla.

- ****EnemyAction / EnemyAttackAction :**** </br>ScriptableObjects que definen las posibles acciones del Jefe, almacenando informacion (dependiendo del tipo de accion) como el angulo necesario entre el frente del enemigo y el jugador para poder realizar la accion, el area de proximidad a la que pertenece esta accion, la probabilidad de que esta accion se eliga sobre las demas o el tiempo de recuperacion (tiempo que el enemigo debe esperar antes de realizar otro ataque).

</br></br>
****ESTADOS****</br>

Para gestionar los estados, estos mismos heredan de la clase **State**, la cual solo tiene el metodo Tick, el cual se llamara desde la maquina de estados del enemyManager dependiendo del estado actual del Jefe.</br>
En este metodo "Tick" se añade la funcionalidad particular de cada estado, en la cual se puede cambiar de estado facilmente usando el "return" con un estado distinto, o usando "return this" si se quiere permanecer en el mismo estado.
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NX
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler);
    }
}
```

Para la maquina de estados del enemigo se utilizan los siguientes estados:

- ****SleepState :**** </br>
Estado en el que comienza el enemigo
En este estado, el enemigo se esta quieto y repite en loop la animacion asignada
con el parametro "sleepAnimation", cuando el jugador se acerca a menos de "detectionRadius"
de distancia, el estado cambia al estado "PursueTargetState" en el que se perseguira al jugador.

```
public class SleepState : State
{
    public PursueState pursueTargetState;

    public bool isSleeping;
    public float detectionRadius = 2;
    public string sleepAnimation;
    public string wakeAnimation;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        // Si esta durmiendo, poner la animacion de dormir
        if (isSleeping && !enemyManager.isInteracting)
            enemyAnimatorHandler.PlayTargetAnimation(sleepAnimation, true);

        // Comprueba si el jugador esta a menos de la distancia parametrizada
        #region Detectar al jugador

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerStats playerStats = colliders[i].transform.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                Vector3 targetDirection = playerStats.transform.position - enemyManager.transform.position;

                //enemyManager.currentTarget = playerStats;
                isSleeping = false;
                enemyAnimatorHandler.PlayTargetAnimation(wakeAnimation, true);

                return pursueTargetState;
            }
        }

        #endregion

        // Si no se ha detectado al jugador, seguir en este estado
        return this;
    }
}
```
    
</br></br>
- ****PursueState :**** </br>
En este estado, la IA hace uso del NavMesh para acercarse al jugador
Una vez que el enemigo se ha acercado lo suficiente al jugador teniendo en cuenta
el parametro "enemyManager.maximumAggroRadius", pasa al estado de combate.

```
public class PursueState : State
{
    public CombatState combatStanceState;
    public RotateTowardsTargetState rotateTowardsTargetState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

        // Se encarga de mover al enemigo hacia el jugador, ya sea usando Navmesh o rotacion manual usando matematicas
        HandleRotateTowardsTarget(enemyManager);

        // Si no esta mirando al jugador, pasar al estado de Rotate para tenerle de frente
        if (viewableAngle > 65 || viewableAngle < -65)
            return rotateTowardsTargetState;


        if (enemyManager.isPerformingAction)
        {
            enemyAnimatorHandler.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

            // Si se esta en medio de un ataque y se permite rotar en el ataque
            if (enemyManager.canRotate)
                CombatState.HandleRotateTowardsTarget(enemyManager);

            return this;
        }


        if (distanceFromTarget > enemyManager.maximumAggroRadius)
            enemyAnimatorHandler.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);

        enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
        enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;


        // Si esta en rango de combate, cambiar a estado de combate
        if (distanceFromTarget <= enemyManager.maximumAggroRadius)
            return combatStanceState;
        // Si no esta en rango de combate, seguir persiguiendo
        else
            return this;
    }

    // Rotar usando navmesh, o manualmente
    public void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        // Seguir al jugador usando navmesh
        Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
        Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;
        enemyManager.navMeshAgent.enabled = true;
        enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        enemyManager.enemyRigidbody.velocity = targetVelocity;
        enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
    }
}
```


- ****CombatState :**** </br>
Comprueba si el jugador ha muerto, en dicho caso, asigna la animacion correspondiente y pasa al estado Idle
Se encarga de tener al jugador de frente constantemente, calculando y modificando su siguiente rotacion

ELECCION DE ATAQUES </br>
Tambien se encarga de calcular que ataque deberia ejecutarse en cada momento
Cada ataque tiene una variable que determina la probabilidad de que sea elegido
sobre el resto (AttackScore), se suman todos los numeros y se elige uno aleatorio.
Cada ataque tiene su propio tiempo de recovery, que es el numero de segundos que tiene que pasar antes de 
realizar otro ataque

FORMA EN LA QUE ACERCARSE AL JUGADOR </br>
En este estado, el enemigo decide de que forma acercarse al jugador dependiendo
de la variable "combatWalkingTypes", facilmente modificable.


```
public class CombatState : State
{
    public IdleState idleState;
    public AttackState attackState;
    public PursueState pursueTargetState;

    public EnemyAttackAction[] enemyAttacks;

    float rotationSpeed;

    // Velocidad a la que rota normalmente
    float normalRotationSpeed = 5;
    // Velocidad a la que rota mientras ataca
    float attackingRotationSpeed = 3;

    // Caminar de una de las formas designadas en el modo combate
    bool randomDestinationSet = false;
    float verticalMovementValue = 0;
    float horizontalMovementValue = 0;

    float randomMovementTimer = 0;
    float maxMovementTime = 6;
    float minMovementTime = 3;

    string lastAttackName = "";

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        // Comprobar si el jugador ha muerto, en dicho caso, asigna la animacion correspondiente y pasa al estado Idle
        if (enemyManager.currentTarget.GetComponent<PlayerStats>().isDead)
        {
            enemyAnimatorHandler.PlayTargetAnimation("EnemyVictory", true);
            return idleState;
        }

        // Distancia hasta el jugador
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        // Mover el enemigo mediante los parametros de las animaciones
        enemyAnimatorHandler.anim.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
        enemyAnimatorHandler.anim.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);

        // Si se esta de vuelta en este estado, significa que el ataque que se estaba ejecutando ha terminado
        attackState.hasPerformedAttack = false;

        // Si se esta realizando alguna accion, no permitir moverse
        if (enemyManager.isInteracting)
        {
            enemyAnimatorHandler.anim.SetFloat("Vertical", 0);
            enemyAnimatorHandler.anim.SetFloat("Horizontal", 0);
            return this;
        }

        // Si el jugador se va fuera del rango de combate, cambiar al estado de persecucion
        else if (distanceFromTarget > enemyManager.maximumAggroRadius)
            return pursueTargetState;


        // Acercarse al enemigo permutando entre diferentes formas de caminar
        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            // Decicir la forma de acercarse al enemigo aleatoriamente
            DecideCirclingAction(enemyAnimatorHandler);

            // Decidir dentro de cuanto tiempo se cambiara este estilo de andar
            randomMovementTimer = Random.Range(minMovementTime, maxMovementTime);
        }
        else
        {
            // Si se tiene una destination asignada cambiarla dentro de X segundos
            randomMovementTimer -= Time.deltaTime;

            if (randomMovementTimer <= 0)
            {
                randomMovementTimer = 0;
                // Desvincular el movimiento anterior para que se decida uno nuevo
                randomDestinationSet = false;
            }
        }

        // Mirar constantemente al jugador
        HandleRotateTowardsTarget(enemyManager);

        // Si se ha elegido un ataque para realizar, pasar al estado de ataque
        // Si se esta en un cool down despues de un ataque, seguir en este modo
        if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
        {
            randomDestinationSet = false;
            return attackState;
        }
        else
            // Elegir un nuevo ataque
            GetNewAttack(enemyManager);

        return this;
    }

    // Gira hacia el jugador de forma suave
    static public void HandleRotateTowardsTarget(EnemyManager enemyManager)
    {
        Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        direction.y = 0;
        direction.Normalize();

        if (direction == Vector3.zero)
            direction = enemyManager.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemyManager.transform.rotation = Quaternion.Slerp
            (enemyManager.transform.rotation, targetRotation, 3 * Time.deltaTime);
    }

    // Diferentes tipos de acercarse al jugador
    struct CombatWalkingType
    {
        public float verticalMovementValue;
        public float horizontalMovementValue;

        public CombatWalkingType(float _verticalMovementValue, float _horizontalMovementValue)
        {
            verticalMovementValue = _verticalMovementValue;
            horizontalMovementValue = _horizontalMovementValue;
        }
    }

    CombatWalkingType[] combatWalkingTypes;
    int currentWalkTypeIndex = 0;

    private void Awake()
    {
        // Se configuran los 3 tipos diferentes de acercarse al jugador
        combatWalkingTypes = new CombatWalkingType[3];

        // Andar hacia la derecha
        combatWalkingTypes[0] = new CombatWalkingType(0, .5f);
        // Andar hacia la izquierda
        combatWalkingTypes[1] = new CombatWalkingType(0, -.5f);
        // Andar hacia el frente
        combatWalkingTypes[2] = new CombatWalkingType(.6f, 0);
    }

    // Decide la forma de acercarse al enemigo aleatoriamente
    private void DecideCirclingAction(EnemyAnimatorHandler enemyAnimatorHandler)
    {
        // No elegir el mismo tipo de andar dos veces seguidas
        int randomNewWalkType = currentWalkTypeIndex;
        while (randomNewWalkType == currentWalkTypeIndex)
            randomNewWalkType = Random.Range(0, combatWalkingTypes.Length);
        currentWalkTypeIndex = randomNewWalkType;

        // Settear los valores que determinan el movimiento en el enemigo
        CombatWalkingType randomWalkType = combatWalkingTypes[currentWalkTypeIndex];
        verticalMovementValue = randomWalkType.verticalMovementValue;
        horizontalMovementValue = randomWalkType.horizontalMovementValue;
    }

    // Elegir un nuevo ataque, que este dentro del rango, en un angulo permitido de forma aleatoria
    private void GetNewAttack(EnemyManager enemyManager)
    {
        Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        int maxScore = 0;

        // Todos los ataques permitidos suman su probabilidad de salir elegidos
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            // Si el ataque seleccionado no puede usarse (angulo no apropiado o distancia)
            // Si es valido, parar el movimiento y atacar el target (jugador)
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximunAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (lastAttackName == enemyAttackAction.name)
                        continue;

                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int temporaryScore = 0;

        // Se elige un ataque de esos teniendo en cuenta la probabilidad de cada uno
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maximunAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                {
                    if (lastAttackName == enemyAttackAction.name)
                        continue;

                    if (attackState.currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        attackState.currentAttack = enemyAttackAction;
                        lastAttackName = enemyAttackAction.name;
                    }
                }
            }
        }
    }
}
```

- ****AttackState :**** </br>

Este estado se encarga de ejecutar el ataque elegido en el estado de combate, una vez que se ha ejecutado el ataque
exitosamente, se pasa al estado de RotateTowardsTarget
Tambien se encarga de gestionar y resetear las variables necesarias despues de realizar un ataque
como el tiempo de recovery y el ataque actual.

```
public class AttackState : State
{
    public CombatState combatStanceState;
    public RotateTowardsTargetState rotateTowardsTargetState;
    public PursueState pursueTargetState;
    public EnemyAttackAction currentAttack;


    bool willDoComboNextAttack = false;
    public bool hasPerformedAttack = false;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        // Elegir un ataque

        // Volver al estado de combate
        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

        // Cambia al estado de persecucion si el jugador se aleja demasiado
        if (distanceFromTarget > enemyManager.maximumAggroRadius)
            return pursueTargetState;

        // Si todavia no se a realizado el ataque elegido en el estado de combate, ejecutarlo
        if (!hasPerformedAttack)
            AttackTarget(enemyAnimatorHandler, enemyManager);


        // Cuando se haya realizado el ataque, pasar al estado de rotacion hacia el jugador
        return rotateTowardsTargetState;
    }

    // Se encarga de ejecutar las animaciones y actualizar los valores de ataque
    private void AttackTarget(EnemyAnimatorHandler enemyAnimatorHandler, EnemyManager enemyManager)
    {
        enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
        // Activar el recovery timer para dejar al jugador una oportunidad de atacar despues del ataque
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        // Actualizar las variables
        hasPerformedAttack = true;
        currentAttack = null; //////
    }
}
```       

- ****RotateTowardsTargetState :**** </br>

Este estado sirve para rotar al jefe de forma suave usando animaciones predefinidas.
Este metodo se llama justo despues de terminar un ataque, en el que el jefe normalmente
habra acabado en una rotacion en la que el jugador no esta delante suyo.

Cuando se entra en este estado, se calcula el angulo entre el frente del jefe y el jugador,
dependiendo del valor, se elegira una de las tres animaciones de girar: 90 grados hacia izquierda/derecha o 180 grados.
Cuando se ha ejecutado la animacion correcta, se pasara al estado de combate.

```
 public class RotateTowardsTargetState : State
{
    public CombatState combatStanceState;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        // Inmovilizar a este enemigo al entrar en este estado
        enemyAnimatorHandler.anim.SetFloat("Vertical", 0);
        enemyAnimatorHandler.anim.SetFloat("Horizontal", 0);

        Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

        // Cuando entremos en este estado, el ultimo ataque seguira en marcha, por lo que tenemos
        // que esperar a que termine su animacion para poder empezar a girar
        if (enemyManager.isInteracting)
            return this;

        // Elegir la animacion mas aproximada para estar mirando directamente al jugador
        if (!enemyManager.isInteracting)
        {
            if (viewableAngle >= 100 && viewableAngle <= 180)
            {
                enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("BackTurn", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -101 && viewableAngle >= -180)
            {
                enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("BackTurn", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -45 && viewableAngle >= -100)
            {
                enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("RightTurn", true);
                return combatStanceState;
            }
            else if (viewableAngle >= 45 && viewableAngle <= 100)
            {
                enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("LeftTurn", true);
                return combatStanceState;
            }
        }

        return combatStanceState;
    }
}
```

- ****IdleState :**** </br>
Estado cuyo unico proposito es que el enemigo no se mueva,
Este estado se llama cuando el enemigo muere, junto a la animacion de muerte del mismo.
Este estado tambien se llama cuando el jugador muere, donde el enemigo realiza en bucle
la animacion de victoria hasta que se reinicie la escena.

```
public class IdleState : State
{
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
    {
        return this;
    }
}
```

</br></br>
## ACCIONES

**JEFE**

Para un uso mas sencillo y organizado de la IA, he clasificado todas las posibles acciones como ScriptableObjects facilmente modificables.
<img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/Acciones.PNG" />
En estas acciones se pueden observar diferentes parametros, los cuales se tienen en cuenta desde el estado de combate dependiendo del contexto:
**ActionAnimation : ** Nombre de la animacion que realiza la accion
**AttackScore : ** Probabilidad de que un ataque sea elegido sobre el resto, para elegir un ataque, todos los ataques posibles en cada frame se suman a una misma variable, cada uno sumando su respectiva Score, despues se elige una aleatoria, mientras mas Score tenga la accion, mas probabilidad de ejecutarse sobre las demas.  
**Maximun/Minimum Attack Angle : ** Rango de vision del jefe a la hora de realizar este ataque, por ejemplo si es 35, -35 y el jugador se situa en la espalda del enemigo, la accion nunca sera elegida ya que solo tendra una oportunidad de ejecutarse si el angulo entre el jugador y el frente del enemigo es menor que 35 grados.
**Maximun/Minimum Attack Distance : ** Igual que en el parametro anterior, solo que delimitando unas distancias maximas y minimas para que la IA sea mucho mas variada y precisa en sus decisiones.

</br>
<table>
  <tr>
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/Animation01.PNG" /></td>
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/Animation02.PNG" /></td>
  </tr>
    <tr>
    <th>Animaciones de acciones</th>
    <th>BlendTree para combinar las animaciones de caminar/correr</th>
  </tr>
  </br>
    <tr>
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/Animation03.PNG" /></td>
    <td><img src="https://github.com/NachoDelCastillo/IAV23-DelCastilloRubio/blob/main/ReadmeImages/Animation04.PNG" /></td>
  </tr>
    <tr>
    <th>Ejemplo de animacion con sus respectivos eventos</th>
    <th>Todas las animaciones implementadas</th>
  </tr>
</table>

**JUGADOR**
| **ACCION** | **TECLADO** | **MANDO** | **DESCRIPCION** |
|:-:|:-:|:-:|:-:|
| Roll/Esquiva | Left Shift | Left Shoulder | Realiza una voltereta, durante la misma, es invulnerable a cualquier ataque |
| Sprint | Left Control | Right Joystick Press | Mientras se mantenga el boton presionado el jugador aumenta su velocidad |
| LockOn/LockOff | F | Right Joystick Press| Inabilita el control manual de la camara, la cual se centra en el enemigo hasta que se desactive. En este estado el jugador cambiara sus animaciones de movimiento para no dar la espalda al enemigo |
| Ataque ligero | Click Izquierdo | Right Shoulder (R1) | Realiza una animacion de atacar con la espada rapida realizando poco daño |
| Ataque pesado | Click Derecho | Right Trigger (R2) | Realiza una animacion de atacar con la espada lenta realizando mucho daño |

<!--
 **PRUEBA**
- PRUEBA : Prueba.

| A  |  B  |  C  |  
|:-:|:--|:-:|
| ✔ | prueba: | 123 |
| ✔ | prueba: | 123 |
| ✔ | prueba: | 123 |
-->
