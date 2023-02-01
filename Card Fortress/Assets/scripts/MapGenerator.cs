using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Text moneyText;
    [SerializeField] Text incomeText;
    [SerializeField] Image timeBar;
    Animator moneyAnimator;
    public int money;
    public int income; 

    [SerializeField]  public Color color;
    [SerializeField] GameObject rangeImage;
    public static MapGenerator mapGenerator;

    public GameObject effect;
    public Material normal;
    public Material white;
    
    [SerializeField] GameObject cell;
    [SerializeField] public int worldSize;
    public int buildingZone;
    [SerializeField] GameObject text1;
    [SerializeField] GameObject text2;

    public Cell[] cells;






    [SerializeField] GameObject kingTower;
    private void Start()
    {
        moneyAnimator = moneyText.transform.GetComponentInParent<Animator>();
        AddMoney(100);
        AddIncome(10);
        timer = setTime;
    }

    float timer;
    const int setTime = 10;
    private void Update()
    {
        if(timer > 0)
        {
            timer = timer - Time.deltaTime;
            timeBar.fillAmount = 1 - timer / setTime;
        }
        else
        {
            AddMoney(income);
            timer = setTime;
        }
    }

    private void Awake()
    {
        if (mapGenerator == null)
        {
            mapGenerator = this;
        }
        else
        {
            Destroy(this);
        }

        FindObjectOfType<CameraMovement>().border = worldSize / 2;
        cells = new Cell[worldSize * 2 + 1];
        GameObject cellObj = Instantiate(cell, new Vector3(0, -5, 0), Quaternion.identity, transform);
        GameObject tower = Instantiate(kingTower, new Vector3(0, -0.9f, 0), Quaternion.identity, transform);
        cells[0] = new Cell(tower,cellObj);

        for (int i = 1; i < worldSize + 15; i++)
        {
            GameObject obj1 = Instantiate(cell, new Vector3(0.5f * i, -5, 0), Quaternion.identity, transform);
            GameObject obj2 = Instantiate(cell, new Vector3(0.5f * (-i), -5, 0), Quaternion.identity, transform);
            if (i < worldSize + 1)
            {
                cells[i] = new Cell(null,obj1);
                cells[worldSize + i] = new Cell(null,obj2);
            }
            else
            {
                obj1.GetComponent<SpriteRenderer>().color = Color.gray;
                obj2.GetComponent<SpriteRenderer>().color = Color.gray;

            }
        }
    }

    public int GetIndex(float positionX)
    {
        int index = Mathf.RoundToInt(positionX / 0.5f);
        
        if (index >= 0 && index < worldSize + 1)
        {
            return index;
        }
        else if(index > -(worldSize + 1) && index < 0)
        {
            return worldSize + Mathf.Abs(index);
        }
        else
        {
            return 0;
        }
    }
    
    public bool CheckBuilding(int index)
    {
        if (index >= 0 && index < worldSize  + 1)
        {
            if(cells[index].building == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (worldSize + Mathf.Abs(index) < worldSize * 2  + 1)
        {

            if(cells[worldSize + Mathf.Abs(index)].building == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public void Hit(int damage, int id)
    {
        if (id < 0)
        {
            id = worldSize + Mathf.Abs(id);
        }
        
        if (id >= 0 && id < worldSize * 2 + 1 && cells[id].building != null)
        {
            GameObject building = cells[id].building;
            GameObject textObj;
            if (Random.Range(0,2) == 1)
            textObj = Instantiate(text1, building.transform.position + new Vector3(0,0.2f,0), Quaternion.identity, transform);
            else
            textObj = Instantiate(text2, building.transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity, transform);

            textObj.GetComponent<TextMesh>().text = damage.ToString();
            Destroy(textObj, 1);
            building.GetComponent<Tower>().Hit(damage);
        }

    }

    public void SetText(Vector3 position,int damage)
    {
        GameObject textObj;
        if (Random.Range(0, 2) == 1)
            textObj = Instantiate(text1,position , Quaternion.identity, transform);
        else
            textObj = Instantiate(text2,position , Quaternion.identity, transform);

        textObj.GetComponent<TextMesh>().text = damage.ToString();
        Destroy(textObj, 1);
    }

    int range;
    GameObject building;
    int buildingIndex = 9999;
    bool isMap;
    GameObject rangeObj;
    
    public void BuildingMode(float x, GameObject gameObject)
    {
        range = gameObject.GetComponent<Tower>().range;
        int index = Mathf.RoundToInt(x / 0.5f);
        isMap = false;

        if (index >= 0 && index < worldSize + 1)
        {
            isMap = true;
        }
        else if (index > -(worldSize + 1) && index < 0)
        {
            isMap = true;
             index = worldSize + Mathf.Abs(index);
        }
        else
        {
            isMap = false;
        }
        if (buildingIndex != index)
        {
            if(isMap)
            {
                if (building == null)
                {
                    building = Instantiate(gameObject, new Vector3(cells[index].cell.transform.position.x, -0.9f, 0), Quaternion.identity, transform);
                    rangeObj = Instantiate(rangeImage, new Vector3(cells[index].cell.transform.position.x, -0.9f, 0), Quaternion.identity, transform);
                    rangeObj.transform.localScale = new Vector3(2 + 4 * range, 5, 1);

                    if (cells[index].building == null)
                    {
                        building.GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    else
                    {
                        building.GetComponent<SpriteRenderer>().color = Color.red;
                    }

                    building.GetComponent<Tower>().enabled = false;
                    building.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    building.transform.GetChild(0).transform.gameObject.SetActive(false);
                }
                else
                {
                    rangeObj.transform.position = new Vector3(cells[index].cell.transform.position.x, -0.9f, 0); 
                    building.transform.position = new Vector3(cells[index].cell.transform.position.x, -0.9f, 0);
                    if (cells[index].building == null)
                    {
                        building.GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    else
                    {
                        building.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
            }
            else
            {
                if (building == null)
                {
                    rangeObj = Instantiate(rangeImage, new Vector3(0.5f * index, -0.9f, 0), Quaternion.identity, transform);
                    rangeObj.transform.localScale = new Vector3(2 + 4 * range, 5, 1);
                    building = Instantiate(gameObject, new Vector3(0.5f * index, -0.9f, 0), Quaternion.identity, transform);
                    building.GetComponent<SpriteRenderer>().color = Color.red;
                    building.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    building.GetComponent<Tower>().enabled = false;
                    building.transform.GetChild(0).transform.gameObject.SetActive(false);
                }
                else
                {
                    rangeObj.transform.position = new Vector3(0.5f * index, -0.9f, 0);
                    building.transform.position = new Vector3(0.5f * index, -0.9f, 0);
                    building.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }

            buildingIndex = index;
        }


    }

    public void BuildingModeOff()
    {
        Destroy(rangeObj);
        Destroy(building);
        buildingIndex = 9999;
        range = 0;
    }


    public bool Build()
    {
        if (isMap && cells[buildingIndex].building == null)
        {
            building.GetComponent<SpriteRenderer>().sortingOrder = -2;
            building.GetComponent<Tower>().enabled = true;
            building.transform.GetChild(0).transform.gameObject.SetActive(true);
            building.GetComponent<SpriteRenderer>().color = Color.white;
            cells[buildingIndex].building = building;
            building = null;
            Destroy(rangeObj);
            buildingIndex = 9999;
            range = 0;
            return true;
        }else
        {
            Destroy(rangeObj);
            Destroy(building);
            building = null;
            buildingIndex = 9999;
            range = 0;
            return false;
        }  
    }

    public void AddMoney(int Value)
    {
        money = money + Value;
        moneyText.text = money.ToString();
        moneyAnimator.SetTrigger("added");
    }

    public void SubtractMoney(int value)
    {
        money = Mathf.Clamp(money - value,0, int.MaxValue);
        moneyText.text = money.ToString();
        moneyAnimator.SetTrigger("subtract");
    }

    public void AddIncome(int Value)
    {
        income = income + Value;
        incomeText.text = "+" + income.ToString();
        moneyAnimator.SetTrigger("added");
    }

}
