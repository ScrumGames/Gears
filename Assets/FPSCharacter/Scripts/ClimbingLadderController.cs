using UnityEngine; 
using System.Collections;

public class ClimbingLadderController : MonoBehaviour {

 /*   [SerializeField] private UnityEngine.UI.Text msgText;

    // Player
    private FirstPersonController Player;

    // Colliders da escada.
    private Collider ladderCollider;
    private Collider downstairsCollider;
    private Collider upstairsCollider;
    private Collider roofCollider;

    // variaveis de controle dos Colliders. Para verificar se esta ou nao tocando o jogador.
    private bool isDownstairsColliding;
    private bool isUpstairsColliding;
    private bool isRoofColliding;
    private bool isTerrainColliding;

    // variavel para controle se o botao de interacao ja foi pressionado.
    // Serve para manter a msg de pressionar botao apagada enquanto o jogadoe é movido para a posicao da escada
    public bool pressedButton;

    // Verificar se o jogador esta interagindo com a escada
    public bool isClimbing;

    void Start () {
        isDownstairsColliding = false;
        isUpstairsColliding = false;
        isRoofColliding = false;
        isClimbing = false;
        pressedButton = false;
        Player = GetComponent<FirstPersonController>();
    }
	
	void Update () {

        // Jogador entrando na area de interacao com a escada na parte inferior.
	    if(isDownstairsColliding && !isClimbing)
        {
            if (!pressedButton)
            {
                msgText.text = "Pressione G para subir";
            }
            
            if (Input.GetButtonDown("Gear") && !pressedButton)
            {
                msgText.text = "";
                pressedButton = true;
                Player.movePlayerToLadderDownstairs(downstairsCollider.gameObject.transform.GetChild(0));
            }
        }

        //Jogador entrando na area de interacao com a escada na parte superior.
        if (isUpstairsColliding && !isClimbing)
        {
            if (!pressedButton)
            {
                msgText.text = "Pressione G para descer";
            }

            if (Input.GetButtonDown("Gear") && !pressedButton)
            {
                msgText.text = "";
                pressedButton = true;
                Player.movePlayerToLadderUpstairs(upstairsCollider.gameObject.transform.GetChild(0));
            }
        }

        // Jogador chega ao limite inferior maximo da escada e para de interagir com ela.
        if (isTerrainColliding && isClimbing)
        {
            //***sai da escada afastando o jogador um pouco dela e mantem a camera olhando para a escada
            //***seta isClimbing para false apos o jogador estar na posicao afastada da escada
        }

        // Jogador chega ao limite superior maximo da escada e para de interagir com ela.
        if (isRoofColliding && isClimbing)
        {
            //***sai da escada afastando o jogador um pouco dela e mantem a camera olhando para frente
            //***seta isClimbing para false apos o jogador estar na posicao afastada da escada
        }

        // Jogador fora da area de interacao com a escada, a mensagem desaparece.
        if (!isDownstairsColliding && !isUpstairsColliding)
        {
            msgText.text = "";
        }
        

    }

    // Verifica qual trigger da escada comecou tocar o jogador e coloca sua respectiva variavel em (true).
    // Captura o collider que foi tocado.
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "DownstairsTrigger":
                isDownstairsColliding = true;
                downstairsCollider = other;
                break;
            case "UpstairsTrigger":
                isUpstairsColliding = true;
                upstairsCollider = other;
                break;
            case "RoofTrigger":
                isRoofColliding = true;
                roofCollider = other;
                break;
            case "Terrain":
                isTerrainColliding = true;
                break;
        }
    }

    // Verifica qual trigger da escada parou de tocar o jogador e coloca sua respectiva variavel em (false).
    void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "DownstairsTrigger":
                isDownstairsColliding = false;
                break;
            case "UpstairsTrigger":
                isUpstairsColliding = false;
                break;
            case "RoofTrigger":
                isRoofColliding = false;
                break;
        }
    }*/
}
