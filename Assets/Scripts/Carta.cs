using UnityEngine;
using UnityEngine.InputSystem;

public class Carta : MonoBehaviour
{
    public Sprite anverso;//parte de arriba de la carta
    public Sprite reverso;//parte de abajo de la carta

    public int valor;//valor que tiene la carta para que asi pueda comprobarlas
    public int indice;//indice de mi carta dentro del array

    private bool bocaArriba = false;//para saber si mi carta esta boca arriba o no

    private SpriteRenderer sr;//componente para ponerle imagen a mi carta
    private GameManager gameManager;//referencia al gameManager

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();//cojo el strite de esta carta
        sr.sprite = reverso;//al empezar el juego muestro el reverso

        GameObject go = GameObject.FindWithTag("GameManager");//busco el objeto que tenga el gameManager
        if (go != null)//si existe lo guardo
        {
            gameManager = go.GetComponent<GameManager>();
        }
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)//compruebo si se ha hecho click izq
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());//convierto la posicion del raton en corrdenadas 

            Collider2D col = Physics2D.OverlapPoint(mousePos); //compruebo si el ratón está tocando algún collider

            if (col != null && col.gameObject == gameObject)//si el collider es esta carta 
            {
                ClickCarta();//ejecuto el click de la carta
            }
        }
    }

    void ClickCarta()
    {
        if (bocaArriba) return;//si ya esta boca arriba que no haga nada
        if (gameManager == null) return; //si no existe GameManager no hago nada
        if (gameManager.estado == "bloqueado") return;//si el juego esta bloqueado tampoco hago nada

        Voltear();//volteo la carta

        gameManager.CambiarEstado(indice);//le digo al manager que carta se ha pulsado
    }

    public void Voltear()
    {
        bocaArriba = !bocaArriba;//cambio el estado de la carta 

        if (bocaArriba)//si la carta esta boca arriba 
            sr.sprite = anverso;//enseño el anverso
        else//si es al reves 
            sr.sprite = reverso;//enseño lo contrario jajaj el reverso 
    }
}