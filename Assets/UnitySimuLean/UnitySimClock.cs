using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using simProcess;
using System.Security.Cryptography;
using System.Text;

public class UnitySimClock : MonoBehaviour {

    static public UnitySimClock instance;

    public double timeScale = 1;

    public bool simOn = false;
    bool simStarted = false;
    public bool pause;

    public bool simRestarted = false;
    public static float simInitTime = 0.0f;

    public SimClock clock = new SimClock();

    public Text timeCounter;
    public Text earningCounter;

    public List<SElement> elements = new List<SElement>();
    public List<UnityMultiLink> mLinks = new List<UnityMultiLink>();

    //UI
    public GameObject initialPanel;
    public GameObject controlPanel;

    public GameObject endSim;

    public float MAXTIme = 1800;

    double timetoBegin=0;

    bool endSimul = true;

    public Text userName;
    string userNameText;

    void saveResult()
    {
        string clave = "AilinZHIlanstqla3su.lozswGT428Gb";

        Rijndael algAES;
        CryptoStream cStrem;
        StreamWriter sResultado;

        Debug.Log("Current path: " + Directory.GetCurrentDirectory());

        Byte[] iniciacion = Encoding.ASCII.GetBytes("akwHZtz57!_gZ.19");

        //Inicializar stream para escribir
        FileStream fStream = new FileStream("RESULTADO_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + ".gii", FileMode.Create);
        algAES = Rijndael.Create();
        cStrem = new CryptoStream(fStream, algAES.CreateEncryptor(Encoding.ASCII.GetBytes(clave), iniciacion), CryptoStreamMode.Write);
        sResultado = new StreamWriter(cStrem);

        sResultado.Write("FECHA " + DateTime.Now.ToLongTimeString() + "\n" + Environment.UserName + "\nBeneficio   " + SimCosts.getEarnings().ToString());
        
        sResultado.Write("\nNOMBRE: " + userNameText);
        sResultado.Write("\n" + SimCosts.totalCost.ToString());
        sResultado.Write("\n" + SimCosts.totalRevenue.ToString());
        sResultado.Flush();
        sResultado.Close();
        fStream.Close();

        
    }

    void Awake()
    {
        UnitySimClock.instance = this;

        simOn = false;
        initialPanel.SetActive(true);
        controlPanel.SetActive(false);
        endSim.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - simInitTime > MAXTIme)
        {
            simOn = false;
            endSim.SetActive(true);
            controlPanel.SetActive(false);

            if (endSimul)
            {
                saveResult();
                endSimul = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            closeSim();
        }

        if (simOn && !simRestarted)
        {
            if (simStarted)
            {
                clock.advanceClock((Time.time - simInitTime + Time.deltaTime) * timeScale);

                if (timeCounter != null)
                {
					timeCounter.text = Math.Round(clock.getSimulationTime(), 1).ToString();
                }
                if (earningCounter != null)
                {
					earningCounter.text = Math.Round(SimCosts.getEarnings()).ToString();
                }
            }
            else
            {
                // INICIO DE LA SIMULACIÓN



                UpdateTime tUpdate = new UpdateTime();

                foreach (SElement theElem in elements)
                {
                    theElem.initializeSim(); //Es necesario darle nombres fijos a cada uno. Los assembler y Multiserver se identifican por él
                }
                foreach (SElement theElem in elements)
                {
                    theElem.connectSim();
                }
                foreach (UnityMultiLink umLink in mLinks)
                {
                    umLink.connectSim();

                }
                foreach (SElement theElem in elements)
                {
                    theElem.startSim();
                }

                tUpdate.start(clock);

                clock.advanceClock((Time.time - simInitTime + Time.deltaTime) * timeScale);
                simStarted = true;

                
            }

        }

    }


    public void paused()
    {

        pause = !pause;

        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void configureScenario()
    {
        if (userName.text == "")
        {
            
            return;
        }

        userNameText = userName.text;

        simOn = !simOn;
        
        SimCosts.restartEarnings();

        initialPanel.SetActive(false);
        controlPanel.SetActive(true);

        simInitTime = Time.time;

        

        if (simRestarted == true)
        {
            foreach (SElement theElem in elements)
            {
                theElem.restartSim();
            }

            simInitTime = Time.time;
            simRestarted = false;
        }
    }

    public void closeSim()
    {
        Application.Quit();
    }

    public void restartSim()
    {
        simRestarted = true;
        simOn = false;
       
        initialPanel.SetActive(true);
        controlPanel.SetActive(false);

        earningCounter.text = "0";
        timeCounter.text = "0";

        clock.reset();   
    }
}
