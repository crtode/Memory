using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject[] cartas;//guardo las cartas en un array
    private int[] cartas_boca_arriba = new int[2];//guardo las cartas que el jugador ha levantado
    public string estado = "inicial";//estado de la carta 
    public TextMeshProUGUI textoIntentos;//donde muestro los intentos
    private int intentos = 0;//el contador de este
    public TextMeshProUGUI textoMarcador;//donde muestro las parejas encontradas al jugador
    private int parejasEncontradas = 0;//el contador de este

    void Start()
    {
        cartas = GameObject.FindGameObjectsWithTag("Carta");//busca las cartas que haya 

        //para reiniciar las posiciones en el array
        cartas_boca_arriba[0] = -1;
        cartas_boca_arriba[1] = -1;

        BarajarCartas();//mezclo cartas

       //recorro las cartas
        for (int i = 0; i < cartas.Length; i++)
        {
            cartas[i].GetComponent<Carta>().indice = i;
        }

        //actualizo el texto que tengo para intentos
        if (textoIntentos != null)
            textoIntentos.text = "Intentos: 0";
    }

    //mezcla las cartas
    public void BarajarCartas()
    {
        //recorro las cartas
        for (int i = 0; i < cartas.Length; i++)
        {
            int randomIndex = Random.Range(0, cartas.Length);//carta aleatoria

            Vector3 tempPos = cartas[i].transform.position;//la guardo pero solo temporalmente

            cartas[i].transform.position = cartas[randomIndex].transform.position;//cambio posiciones
            cartas[randomIndex].transform.position = tempPos;
        }
    }

    //cambio el estado de la carta
    public void CambiarEstado(int indiceRecibido)
    {
        if (estado == "inicial")
        {
            cartas_boca_arriba[0] = indiceRecibido;//guardo lña primera carta
            estado = "una_carta";//y le cambio el estado
        }
        else if (estado == "una_carta")//si ya hay una carta levantada
        {
            cartas_boca_arriba[1] = indiceRecibido;//guardo la segunda carta
            estado = "bloqueado";//bloqueo el juego temporalmente

            intentos++; //sumo los intentos que me ha costado

            if (textoIntentos != null)//actualizo el texto de intentos
                textoIntentos.text = "Intentos: " + intentos;

            StartCoroutine(ComprobarPareja());//compruebo la pareja
        }
    }

    IEnumerator ComprobarPareja()//compruebo si son pareja
    {
        Carta c1 = cartas[cartas_boca_arriba[0]].GetComponent<Carta>();//cojo el script de carta levantada para este proceso
        Carta c2 = cartas[cartas_boca_arriba[1]].GetComponent<Carta>();

        yield return new WaitForSeconds(1f);//espero unos segundos antes de comprobar para que sea interesante como dijo Pascual

        if (c1.valor == c2.valor)//si las cartas tienen el mismo valor
        {
            parejasEncontradas++;//las sumo al contador de parejas

            if (textoMarcador != null)
                textoMarcador.text = "Parejas: " + parejasEncontradas;//actualizo el marcador
        }
        else
        {
            //si no son parejas ambas se vuelven a dar la vuelta 
            c1.Voltear();
            c2.Voltear();
        }
        //reinicio las cartas levantadas
        cartas_boca_arriba[0] = -1;
        cartas_boca_arriba[1] = -1;
        //y vuelven al estado incial
        estado = "inicial";
    }

    // para reiniciar el juego
    public void ReiniciarJuego()
    {
        //carga de nuevo la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //para barajar pero sin que reinicie el juego
    public void BotonBarajar()
    {
        if (estado == "bloqueado") return;//si el juego está bloqueado no les dejo barajar

        BarajarCartas();//mezclo las cartas
    }

    //para salir primero puse solo Application.Quit(); pero no funcionaba en el editor y tuve que usar UnityEditor.EditorApplication.isPlaying
    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego");

#if UNITY_EDITOR//si estoy en el editor de unity 
        UnityEditor.EditorApplication.isPlaying = false;//detengo el juego
#else
            Application.Quit();//si el juego está en un exe como pide Pascual también se puede cerrar la app 
#endif
    }
}