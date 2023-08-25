using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Singleton, ya que es SinglePLayer
 * Cuando lo haga Multiplayer, ahi sale refactor
 */
public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float rotationSpeed = 10f;
        
    
    private bool isWalking;
    private Vector3 lastMovementDirection;
    private BaseCounter selectedCounter;
    [SerializeField]
    private KitchenObject kitchenObject;
    [SerializeField]
    private Transform spawnPoint;


    private const float RADIUS = .7f; // el scale de la esfera del cuerpo dividido 2.
    private const float PLAYER_HEIGHT = 2f; // lo mande a ojo, es lo de menos igual
    private const float INTERACT_DISTANCE = 2f;
    [SerializeField]
    private GameInput gameInput;

    /* La idea es la siguiente: cada vez que cambia la selección de una mesada, se dispara el evento. TODOS los ClearCounters van a estar escuchando por el evento
     * y van a fijarse si se corresponde con alguno de ellos y si es asi, van a actualizar la parte visual.
     * La contra? que es viable en un contexto asi, mapa pequeño, no nos importa el consumo de memoria en este caso.
     * Si necesito pasar datos cuando disparo el evento, lo hago con el generic ese y con la clase que tiene los datos asociados.
    */
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }


    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 instance of a Player");
        }
        Instance = this;
    }


    /*
       1) La multiplicación del Time.deltaTime se hace para que la velocidad ya no dependa de la cantidad de fps. Recordemos que Update se ejecuta en cada frame
         Por lo tanto si tengo muchos mas fps, naturalmente la velocidad va a ser mayor. Pero si lo multiplico por un infinitesimal de tiempo, ese numero se mantiene
        ya que si aumento los fps, el delta es aun más pequeño
       
        2) Antes de moverme directamente (asignacion a transform.position) debo verificar si estoy colisionando con algo, sino lo voy a traspasar (Collisions)
     Physics.Raycast, es como un laser que apunta en cierta direccion y te avisa si colisionas con otro objeto. Sale desde el centro del objeto origen, asi que si en la
    dirección del centro no detecta colisión, el jugador camina, no importa si con el brazo o algo asi termine chocando con algo.
    Para evitar este problema, hay otros tipos de lasers que se pueden castear, con diferentes formas, como BoxCast, CapsuleCast o SphereCast.

    Physics.CapsuleCast(capsuleBottom, CapsuleTop, radius, 
    capsuleBottom es la base del jugador, esta en el suelo.
    capsule Top es la altura maxima que seria la cabeza

     */


    /*
     * ROTACIÓN
     * Para manejar la rotación hacia la dirección donde me muevo, hay varias maneras
     * 1 - transform.rotation
     * 2 - eulerAngles
     * 3 - LookAt: rota pero dando la espalda. Si me muevo de forma circular, estaría siempre mirando hacia el centro y no hacia la dirección tangente
               transform.LookAt(rotationSpeed * Time.deltaTime * move);
     * 4 - transform.forward
           transform.forward = move; si hago la rotación así, se logra pero de forma directa, no hay un suave movimiento hacia la rotación objetivo
     */
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction; //Listener del evento
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter)
        {
            selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementNormalizedVector();

        //modifico la posición del jugador. Como el jugador se mueve en un mapa de 3 dimensiones, es necesario hacer el casteo. Asi tengo separado el vector input, del movimiento en sí.
        Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);


        float moveDistance = Time.deltaTime * movementSpeed;

        bool collisionDetected = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, RADIUS, move, moveDistance);

        // Si me muevo en diagonal al chocar contra un obstaculo, me fijo si es posible moverme en la dirección paralela.
        if (collisionDetected)
        {
            //intento moverme en X 
            Vector3 moveX = new Vector3(move.x, 0, 0).normalized; // para evitar moverme a una velocidad diferente. Siempre debe ser un vector unitario
            collisionDetected = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, RADIUS, moveX, moveDistance);
            if (!collisionDetected)
            {
                move = moveX;
            }
            else
            {
                //ahora intento moverme en el plano z
                Vector3 moveZ = new Vector3(0, 0, move.z).normalized;
                collisionDetected = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, RADIUS, moveZ, moveDistance);
                if (!collisionDetected)
                {
                    move = moveZ;
                }
            }
        }

        if (!collisionDetected)
        {
            transform.position += (moveDistance * move);
        }
        isWalking = move != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, move, Time.deltaTime * rotationSpeed);
    }

    private void HandleInteractions()
    {
        /* 1)
         * Con un raycast alcanza ya que no estoy pensando en no chocarme con cosas, simplemente saber si tengo algo adelante para interactuar, y qué objeto
         * Hay muchas firmas de Raycast, algunas devuelven un booleano, que sirve para saber si hay algo adelante o no.
         * Como en este caso me interesa interactuar, necesito obtener la referencia del objeto. Para ello, utilizo la firma con el parametro "out RaycastHit info".
         * Esta variable referencia al objeto con el que colisiona el jugador. (side effect)
         * 
         * 2) Si yo me quedo parado, no me estoy moviendo (duh). Eso quiere decir que el vector move será nulo y por lo tanto no voy a mandar un Raycast a ningun lugar, por lo que
         * por más que tenga un objeto en frente, no voy a detectarlo.
         * Para solucionar esto, cuando guardarme una referencia de la ultima dirección a la que me moví. De ahi nace lastMovementDirection
         * 
         * 3) En este juego solamente voy a interactuar con clearCounters, pero en caso de tener un monton de clases distintas para interactuar, como abrir puertar,
           interactuar con npcs, apretar botones y demás, hay otro enfoque mucho mejor que una lluvia de ifs
        */
        Vector2 inputVector = gameInput.GetMovementNormalizedVector();
        Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);

        if (move != Vector3.zero)
        {
            lastMovementDirection = move;
        }

        if (Physics.Raycast(transform.position, lastMovementDirection, out RaycastHit hit, INTERACT_DISTANCE)) // Hay algo adelante?
        {
            if (hit.transform.TryGetComponent<BaseCounter>(out BaseCounter counter))
            {
                if(selectedCounter != counter) { 
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = this.selectedCounter
        });
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    public Transform GetSpawnPoint() { return spawnPoint; }
    public void SetKitchenObject(KitchenObject ko) { kitchenObject = ko; }
    public KitchenObject GetKitchenObject() { return kitchenObject; }
    public void ClearKitchenObject() { kitchenObject = null; }
    public bool HasKitchenObject() { return kitchenObject != null; }
}
