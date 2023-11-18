using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIHandler : MonoBehaviour, ITuningScreen
{
    private enum TuningCharacteristics
    {
        Level,
        Price
    }

    [SerializeField] GameObject tuningGameObject;
    [SerializeField] GameObject stylingGameObject;
    [SerializeField] GameObject stylingScrollObject;
    [SerializeField] SuperDuperService testServer;
    [SerializeField] ContentSlider wheelsSlider;
    [SerializeField] ContentSlider vinylSlider;
    [Space]
    [Header("Кнопки")]
    [SerializeField] Button tuningButton;
    [SerializeField] List<Button> tuningScroll;
    [SerializeField] Button stylingButton;
    [SerializeField] List<Button> stylingScroll;
    [SerializeField] List<Button> bodyScroll;
    [SerializeField] List<Button> discsCustomScroll;
    [SerializeField] List<Button> tintScroll;
    [Space]
    [Header("Подтверждение покупки")]
    [SerializeField] Button buyButton;
    [SerializeField] Button buyDonateButton;
    [SerializeField] GameObject buyAcceptObject;
    [SerializeField] TextMeshProUGUI buyUpgradeName;
    [SerializeField] TextMeshProUGUI buyPriceAcceptText;
    [SerializeField] Button buyAcceptButton;
    [SerializeField] Button buyCancelButton;
    [Space]
    [SerializeField] List<TuningButton> tuningButtons;
    [SerializeField] List<Slider> tuningCurrentSlider;
    [SerializeField] List<TextMeshProUGUI> tuningCurrentText;
    [SerializeField] List<Slider> tuningFutureSlider;
    [SerializeField] List<TextMeshProUGUI> tuningFutureText;
    [SerializeField] List<TextMeshProUGUI> tuningDifferenceText;
    [SerializeField] TextMeshProUGUI balanceText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI buyButtonText;
    [SerializeField] GameObject buyGameObject;
    [SerializeField] GameObject buyDonateButtonGameObject;
    [SerializeField] RectTransform buyTransform;
    [SerializeField] GameObject bodyScrollObject;
    [SerializeField] GameObject bodyContentObject;
    [SerializeField] Button bodyArrowNext;
    [SerializeField] Button bodyArrowPrevious;
    [SerializeField] TextMeshProUGUI bodyPartNameText;
    [SerializeField] GameObject colorWheel;
    [SerializeField] GameObject transparencySlider;
    [SerializeField] GameObject bodyObject;
    [SerializeField] GameObject discsScroll;
    [SerializeField] List<Button> discsScrollContent;
    [SerializeField] GameObject discsCustom;
    [SerializeField] GameObject vinylScroll;
    [SerializeField] List<Button> vinylScrollContent;
    [SerializeField] GameObject tintObject;
    [SerializeField] GameObject hydraulicsObject;
    [SerializeField] TextMeshProUGUI hydraulicsText;
    [SerializeField] Sprite activeButtonSprite;
    [SerializeField] Sprite inactiveButtonSprite;
    [SerializeField] Slider stylingSliderValue;
    [SerializeField] GameObject stylingSliderObject;
    [SerializeField] TextMeshProUGUI stylingSliderNameText;
    [SerializeField] TextMeshProUGUI stylingSliderValueText;
    [SerializeField] ColorCircle colorCircle;

    private int balance = 0;
    private int donateBalance = 10000;
    private int price = 0;
    private int donatePrice = 0;
    private Action onMoneySelect;
    private Action onDonateSelect;

    private Dictionary<TuningDetails, (int level, int price, int donatePrice)> tuningLevels;
    private int[][] tuningUpgradeValues = new int[(int)TuningDetails.Max][]
    {
        new [] {10, 5, 0, -5},
        new [] {5, 5, 0, -5},
        new [] {-5, 0, 5, 5},
        new [] {0, 0, 5, 5},
        new [] {5, 0, 5, 0},
        new [] {0, 10, 0, 0},
        new [] {0, 0, 0, 10},
    };
    private TuningDetails currentTuningDetail;

    private Dictionary<TuningDetails, string> tuningUpgradeName = new Dictionary<TuningDetails, string>
    {
        { TuningDetails.Brakes, "Тормоза " },
        { TuningDetails.ChipTuning, "Чип-тюнинг " },
        { TuningDetails.Engine, "Двигатель " },
        { TuningDetails.Suspension, "Подвеска " },
        { TuningDetails.Tires, "Шины " },
        { TuningDetails.Transmission, "Трансмиссия " },
        { TuningDetails.Turbocharging, "Турбонаддув " }
    };

    private Dictionary<VehicleTuningType, string> stylingUpgradeName = new Dictionary<VehicleTuningType, string>
    {
        { VehicleTuningType.Exhaust, "Выхлоп № " },
        { VehicleTuningType.FrontBumper, "Передний бампер № " },
        { VehicleTuningType.Hood, "Капот № " },
        { VehicleTuningType.Hydraulics, "Гидравлику " },            //Поставить или снять
        { VehicleTuningType.Nitro, "Нитро " },
        { VehicleTuningType.RearBumper, "Задний бампер № " },
        { VehicleTuningType.Roof, "Крыша № " },
        { VehicleTuningType.SideSkirt, "Боковая юбка № " },
        { VehicleTuningType.Spoiler, "Спойлер № " },
        { VehicleTuningType.Wheels, "Диски № " }
    };

    private Dictionary<ColorsType, string> stylingColorName = new Dictionary<ColorsType, string>
    {
        { ColorsType.TonerFront, "Тонировка передних стекол" },
        { ColorsType.TonerSide, "Тонировка боковых стекол" },
        { ColorsType.TonerRear, "Тонировка задних стекол" },
        { ColorsType.Wheels, "Перекраска дисков" },
        { ColorsType.Body, "Перекраска кузова" },
        { ColorsType.Headlights, "Смена цвета фар" },
        { ColorsType.Neon, "Смена цвета неона" }
    };

    private Dictionary<WheelEditType, string> stylingDiscsName = new Dictionary<WheelEditType, string>
    {
        { WheelEditType.FrontOffset, "Вылет передних" },
        { WheelEditType.BackOffset, "Вылет задних" },
        { WheelEditType.FrontAlignment, "Развал передних" },
        { WheelEditType.BackAlignment, "Развал задних" },
        { WheelEditType.Width, "Ширина колес" }
    };
    private WheelEditType currentEditWheelType;
    private float currentEditWheelMultiplier;

    private Vector2 tuningBuyPos = new Vector2(-363, 265);
    private Vector2 stylingBuyPos = new Vector2(-363, 195);

    private Image activeButtonImage;
    private Image activeSecondaryButtonImage;
    private GameObject activeContent;

    private int bodyContentShownNum = -1;
    private VehicleTuningType currentBodyDetailType;
    private List<(int id, int price)> bodyDetails = new();
    private List<(int id, int price)> bodyDetailsTemp = new();

    private Color yellowColor = new Color(1f, 213 / 255f, 84 / 255f);
    private Color purpleColor = new Color(154 / 255f, 118 / 255f, 1);
    private int colorPrice = 0;
    private bool selectedColorTransparency;
    private ColorsType selectedColorType;
    private ColorsType[] transparentColors = new[] {ColorsType.TonerFront, ColorsType.TonerSide, ColorsType.TonerRear};

    private Dictionary<WheelEditType, int> editWheelsPrices;

    private int hydraulicsInstallPrice = 0;
    private int hydraulicsRemovePrice = 0;
    private bool isInstalledHydraulics;

    private int nitroLevel = 0;
    private List<int> nitroPrices;

    private int suspensionPrice = 0;

    private Color inactiveColor = new Color(39 / 255f, 37 / 255f, 55 / 255f, 0.4f);
    private Color activeColor = new Color(39 / 255f, 37 / 255f, 55 / 255f, 0.8f);

    private Dictionary<string, int> vinylPrices;

    private Dictionary<int, int> wheelsPrices;

    void Start()
    {
        tuningLevels = new ();
        nitroPrices = new List<int>();
        editWheelsPrices = new ();

        activeButtonImage = tuningScroll[0].GetComponent<Image>();

        tuningButton.onClick.AddListener(() => OpenTuning());
        stylingButton.onClick.AddListener(() => OpenStyling());

        buyButton.onClick.AddListener(() => ShowConfirmation());
        buyDonateButton.onClick.AddListener(() => ShowDonateConfirmation());
        buyCancelButton.onClick.AddListener(() => CloseConfirmation());

        bodyScroll[0].onClick.AddListener(() => OpenStyling());
        bodyScroll[1].onClick.AddListener(() => BodyContentPressed(VehicleTuningType.RearBumper, bodyScroll[1]));
        bodyScroll[2].onClick.AddListener(() => BodyContentPressed(VehicleTuningType.FrontBumper, bodyScroll[2]));
        bodyScroll[3].onClick.AddListener(() => BodyContentPressed(VehicleTuningType.Hood, bodyScroll[3]));
        bodyScroll[4].onClick.AddListener(() => BodyContentPressed(VehicleTuningType.Exhaust, bodyScroll[4]));
        bodyScroll[5].onClick.AddListener(() => BodyContentPressed(VehicleTuningType.Roof, bodyScroll[5]));
        bodyScroll[6].onClick.AddListener(() => BodyContentPressed(VehicleTuningType.SideSkirt, bodyScroll[6]));
        bodyScroll[7].onClick.AddListener(() => BodyContentPressed(VehicleTuningType.Spoiler, bodyScroll[7]));

        bodyArrowNext.onClick.AddListener(() => BodyContentArrowPressed(true));
        bodyArrowPrevious.onClick.AddListener(() => BodyContentArrowPressed(false));

        for (int i = 0; i < tuningScroll.Count; i++)
        {
            int temp = i;
            tuningScroll[i].onClick.AddListener(() => TuningScrollPress((TuningDetails)temp));
        }

        for (int i = 1; i < discsCustomScroll.Count; i++)
        {
            int temp = i - 1;
            discsCustomScroll[temp + 1].onClick.AddListener(() => StylingDiscsCustomContentPress((WheelEditType)temp, discsCustomScroll[temp + 1]));
        }
        discsCustomScroll[0].onClick.AddListener(() => OpenStyling());

        for (int i = 1; i < tintScroll.Count; i++)
        {
            int temp = i - 1;
            tintScroll[temp + 1].onClick.AddListener(() => StylingColorPress((ColorsType) temp, tintScroll[temp + 1]));
        }
        tintScroll[0].onClick.AddListener(() => OpenStyling());

        stylingScroll[0].onClick.AddListener(() => StylingBodyPressed(stylingScroll[0]));
        stylingScroll[1].onClick.AddListener(() => StylingColorPress(ColorsType.Body, stylingScroll[1]));
        stylingScroll[2].onClick.AddListener(() => StylingDiscsPress(stylingScroll[2]));
        stylingScroll[3].onClick.AddListener(() => StylingDiscsCustomPress());
        stylingScroll[4].onClick.AddListener(() => StylingColorPress(ColorsType.Wheels, stylingScroll[4]));
        stylingScroll[5].onClick.AddListener(() => StylingColorPress(ColorsType.Neon, stylingScroll[5]));
        stylingScroll[6].onClick.AddListener(() => StylingVinylPressed(stylingScroll[6]));
        stylingScroll[7].onClick.AddListener(() => StylingTintPress());
        stylingScroll[8].onClick.AddListener(() => StylingColorPress(ColorsType.Headlights, stylingScroll[8]));
        stylingScroll[9].onClick.AddListener(() => StylingSuspensionPress(stylingScroll[9]));
        stylingScroll[10].onClick.AddListener(() => StylingNitroPress(stylingScroll[10]));
        stylingScroll[11].onClick.AddListener(() => StylingHydraulicsPress(stylingScroll[11]));

        testServer.GetSuspension();
        testServer.GetNitroData();
        testServer.GetHydraulicsData();
        testServer.GetEditWheels();
        testServer.GetWheels();
        testServer.GetVinyls();
    }

    private void Awake()
    {
        
    }

    private void OpenTuning()
    {
        for (int i = 0; i <= 3; i++)
        {
            tuningFutureSlider[i].value = 0;
            tuningFutureText[i].text = "";
            tuningDifferenceText[i].text = "";
        }
        buyTransform.anchoredPosition = tuningBuyPos;
        buyDonateButtonGameObject.SetActive(true);
        SetActiveContent(null);
        SetActiveButtonImage(null);
        tuningGameObject.SetActive(true);
        stylingGameObject.SetActive(false);
        buyGameObject.SetActive(false);
        stylingScrollObject.SetActive(true);
        SetActiveContent(null);
    }

    private void OpenStyling()
    {
        SetActiveButtonImage(null);
        tuningGameObject.SetActive(false);
        stylingGameObject.SetActive(true);
        buyGameObject.SetActive(false);
        buyTransform.anchoredPosition = stylingBuyPos;
        buyDonateButtonGameObject.SetActive(false);
        discsCustom.SetActive(false);
        tintObject.SetActive(false);
        bodyObject.SetActive(false);
        stylingScrollObject.SetActive(true);
        SetActiveContent(null);
    }

    private void SetActiveButtonImage(Image image, Sprite sprite = null)
    {
        if(activeButtonImage) activeButtonImage.sprite = inactiveButtonSprite;
        if (image)
        {
            activeButtonImage = image;
            activeButtonImage.sprite = sprite ? sprite: activeButtonSprite;
        }
    }
    private void SetActiveSecondaryButtonImage(Image image)
    {
        if (activeSecondaryButtonImage) activeSecondaryButtonImage.color = inactiveColor;
        if (image)
        {
            activeSecondaryButtonImage = image;
            activeSecondaryButtonImage.color = activeColor;
        }
    }

    private void SetActiveContent(GameObject go)
    {
        if (activeContent) activeContent.SetActive(false);
        if (go)
        {
            activeContent = go;
            activeContent.SetActive(true);
        }
    }

    private void StylingButtonPress(Button button, GameObject content)
    {
        SetActiveButtonImage(button ? button.GetComponent<Image>() : null);
        SetActiveContent(content);
    }
    private void SelectItem(int price, string upgradeName, Action action, string buyButtonText = "КУПИТЬ")
    {
        string tempPriceText = NumberSplit(price);
        this.buyButtonText.text = buyButtonText;
        priceText.text = tempPriceText;
        buyGameObject.SetActive(true);
        this.price = price;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(ShowConfirmation);
        onMoneySelect = () => {
            buyUpgradeName.text = upgradeName;
            buyPriceAcceptText.text = $"${tempPriceText}";
            buyPriceAcceptText.color = yellowColor;
            buyAcceptButton.onClick.RemoveAllListeners();
            buyAcceptButton.onClick.AddListener(new UnityEngine.Events.UnityAction(action));
            buyAcceptButton.onClick.AddListener(() => { buyAcceptObject.SetActive(false); });
        };
    }

    private void RemoveSelectItem()
    {
        buyGameObject.SetActive(false);
    }

    private void ShowDonateConfirmation()
    {
        if (tuningLevels[currentTuningDetail].donatePrice > 0)       //donatePrice <= donateBalance
        {
            onMoneySelect?.Invoke();
            buyAcceptObject.SetActive(true);
        }
        buyPriceAcceptText.text = $"${0}";
        buyPriceAcceptText.color = purpleColor;
        buyAcceptButton.onClick.RemoveAllListeners();
        buyAcceptButton.onClick.AddListener(() => 
        {
            testServer.BuyPerformanceDetail(currentTuningDetail, BuyType.Donate);
            tuningLevels[currentTuningDetail] = (tuningLevels[currentTuningDetail].level, tuningLevels[currentTuningDetail].price, tuningLevels[currentTuningDetail].donatePrice - 1);
            buyAcceptObject.SetActive(false);
        });
        TuningScrollPress(currentTuningDetail);
    }

    private void ShowConfirmation()
    {
        if(price <= balance)
        {
            onMoneySelect?.Invoke();
            buyAcceptObject.SetActive(true);
        }
    }

    private void CloseConfirmation()
    {
        buyAcceptObject.SetActive(false);
    }

    private void BuyRightContentSliderPressed(string name, int price)
    {
        buyUpgradeName.text = name;
        priceText.text = "$" + price;
    }

    private void TuningScrollPress(TuningDetails type)
    {
        SelectItem(tuningLevels[type].price, $"{tuningUpgradeName[type]}{tuningLevels[type].level + 1} уровня", () => BuyTuningDetail(type));

        donatePrice = tuningLevels[type].donatePrice;
        currentTuningDetail = type;

        SetActiveButtonImage(tuningScroll[(int)type].GetComponent<Image>());

        for (int i = 0; i < 4; i++)
        {
            int upgradeValue = tuningUpgradeValues[(int)type][i];
            float futureValue = tuningCurrentSlider[i].value + upgradeValue;
            tuningFutureSlider[i].value = futureValue;
            tuningFutureText[i].text = (futureValue).ToString();
            tuningDifferenceText[i].text = upgradeValue >= 0 ? $"+{upgradeValue}" : upgradeValue.ToString();
        }
    }

    private void BuyTuningDetail(TuningDetails type)
    {
        testServer.BuyPerformanceDetail(type, BuyType.Money);
        TuningScrollPress(type);
    }

    private void StylingBodyPressed(Button button)
    {
        SetActiveButtonImage(null);
        SetActiveContent(null);
        stylingScrollObject.SetActive(false);
        bodyScrollObject.SetActive(true);
        buyGameObject.SetActive(false);
    }

    private void BodyContentPressed(VehicleTuningType type, Button button)
    {
        SetActiveButtonImage(button.GetComponent<Image>());
        buyGameObject.SetActive(false);
        bodyContentShownNum = -1;
        bodyPartNameText.text = "СТАНДАРТНЫЙ";
        currentBodyDetailType = type;
        testServer.GetDetails(type);
    }

    private void BodyContentArrowPressed(bool isForward)
    {
        bodyContentShownNum += isForward ? 1 : -1;
        if (bodyContentShownNum < -1) bodyContentShownNum = bodyDetails.Count - 1;
        else if (bodyContentShownNum >= bodyDetails.Count) bodyContentShownNum = -1;
        if(bodyContentShownNum != -1)
        {
            int id = bodyDetails[bodyContentShownNum].id;
            bodyPartNameText.text = $"№ {id}";
            SelectItem(bodyDetails[bodyContentShownNum].price, $"{stylingUpgradeName[currentBodyDetailType]}{id}", () => testServer.BuyBodyDetail(id, currentBodyDetailType));
        }
        else
        {
            bodyPartNameText.text = "СТАНДАРТНЫЙ";
            RemoveSelectItem();
        }
    }

    private void StylingColorPress(ColorsType type, Button button)
    {
        testServer.GetColorsPrice(type);

        selectedColorTransparency = transparentColors.Contains(type);
        selectedColorType = type;
        StylingButtonPress(button, colorWheel);
        transparencySlider.SetActive(transparentColors.Contains(type));
    }

    private void BuyColor()
    {
        Vector4 temp = colorCircle.GetColor();
        temp *= 255;
        temp.w = selectedColorTransparency ? temp.w : 255;
        testServer.BuyColor(selectedColorType, (int)temp.x, (int)temp.y, (int)temp.z, (int)temp.w);
    }

    private void StylingDiscsPress(Button button)
    {
        StylingButtonPress(button, discsScroll);
        buyGameObject.SetActive(false);
    }

    private void StylingDiscsContentPressed(Button button, int name, int price)
    {
        buyGameObject.SetActive(true);
        SetActiveSecondaryButtonImage(button.GetComponent<Image>());
        SelectItem(price, $"{stylingUpgradeName[VehicleTuningType.Wheels]}{name}", () => testServer.BuyWheels(name));
    }


    private void StylingDiscsCustomPress()
    {
        SetActiveButtonImage(null);
        SetActiveContent(null);
        stylingScrollObject.SetActive(false);
        discsCustom.SetActive(true);
        buyGameObject.SetActive(false);
    }

    private void ChangeSliderValues(int minValue, int maxValue, int currentValue, Action<float> action)
    {
        stylingSliderValue.minValue = minValue;
        stylingSliderValue.maxValue = maxValue;
        stylingSliderValue.value = currentValue;
        action?.Invoke(currentValue);
        stylingSliderValue.onValueChanged.RemoveAllListeners();
        stylingSliderValue.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(action));
    }

    private void StylingDiscsCustomContentPress(WheelEditType type, Button button)
    {
        StylingButtonPress(button, stylingSliderObject);
        currentEditWheelType = type;
        switch (type)
        {
            case (WheelEditType.FrontOffset):
                ChangeSliderValues(0, 15, 0, StylingSliderPositiveDiscs);
                currentEditWheelMultiplier = 0.01f;
                break;

            case (WheelEditType.BackOffset):
                ChangeSliderValues(0, 15, 0, StylingSliderPositiveDiscs);
                currentEditWheelMultiplier = 0.01f;
                break;

            case (WheelEditType.FrontAlignment):
                ChangeSliderValues(0, 20, 0, StylingSliderDegrees);
                currentEditWheelMultiplier = 1;
                break;

            case (WheelEditType.BackAlignment):
                ChangeSliderValues(0, 20, 0, StylingSliderDegrees);
                currentEditWheelMultiplier = 1;
                break;

            case (WheelEditType.Width):
                ChangeSliderValues(5, 15, 10, StylingSliderMultiplication);
                currentEditWheelMultiplier = 0.1f;
                break;
        }
        stylingScrollObject.SetActive(false);
        SelectItem(editWheelsPrices[type], $"{stylingDiscsName[type]} {currentEditWheelMultiplier * stylingSliderValue.value}", () => BuyDiscsCustom());
        buyButton.onClick.AddListener(() => BuyDiscsCustomPress());

        buyGameObject.SetActive(true);
    }

    private void BuyDiscsCustomPress()
    {
        buyUpgradeName.text = $"{stylingDiscsName[currentEditWheelType]} {currentEditWheelMultiplier * stylingSliderValue.value}";
    }
    private void BuyDiscsCustom()
    {
        testServer.BuyEditWheels(currentEditWheelType, currentEditWheelMultiplier * stylingSliderValue.value);
    }

    private void StylingVinylPressed(Button button)
    {
        StylingButtonPress(button, vinylScroll);
        buyGameObject.SetActive(false);
    }

    private void StylingVinylContentPress(Button button, string name, int price)
    {
        buyGameObject.SetActive(true);
        SetActiveSecondaryButtonImage(button.GetComponent<Image>());
        SelectItem(price, "Винил № " + name.Replace("Vinyl ", ""), () => testServer.BuyVinyls(name));
    }

    private void StylingTintPress()
    {
        SetActiveButtonImage(null);
        SetActiveContent(null);
        stylingScrollObject.SetActive(false);
        tintObject.SetActive(true);
        RemoveSelectItem();
    }


    private void StylingSuspensionPress(Button button)
    {
        StylingButtonPress(button, stylingSliderObject);
        stylingSliderNameText.text = "ВЫСОТА";
        ChangeSliderValues(0, 9, 0, StylingSliderNegative);
        SelectItem(suspensionPrice, $"{tuningUpgradeName[TuningDetails.Suspension]} -{stylingSliderValue.value}", () => BuyDiscsCustom());
        buyButton.onClick.AddListener(BuySuspensionPress);
    }

    private void BuySuspensionPress()
    {
        buyUpgradeName.text = $"{tuningUpgradeName[TuningDetails.Suspension]} -{stylingSliderValue.value}";
    }
    private void BuySuspension()
    {
        testServer.BuySuspension(stylingSliderValue.value);
    }


    private void StylingNitroPress(Button button)
    {
        StylingButtonPress(button, stylingSliderObject);
        stylingSliderNameText.text = "УРОВЕНЬ";
        ChangeSliderValues(0, 3, 0, StylingSliderNitro);
        SelectItem(nitroPrices[nitroLevel], $"Нитро уровень {stylingSliderValue.value}", () => BuyDiscsCustom());
        buyButton.onClick.AddListener(BuyNitroPress);
    }

    private void BuyNitroPress()
    {
        buyUpgradeName.text = $"Нитро уровень {stylingSliderValue.value}";
        buyPriceAcceptText.text = $"${nitroPrices[(int)stylingSliderValue.value]}";
    }
    private void BuyNitro()
    {
        testServer.BuyNitro((int)stylingSliderValue.value);
    }

    private void StylingHydraulicsPress(Button button)
    {
        if(button) StylingButtonPress(button, hydraulicsObject);
        if (isInstalledHydraulics)
        {
            SelectItem(hydraulicsRemovePrice, "Снятие гидравлики", () => testServer.BuyHydraulic(true), "СНЯТЬ");
            hydraulicsText.text = "УСТАНОВЛЕНА";
        }
        else
        {
            SelectItem(hydraulicsInstallPrice, "Установка гидравлики", () => testServer.BuyHydraulic(false), "УСТАНОВИТЬ");
            hydraulicsText.text = "ОТСУТСТВУЕТ";
        }
    }

    private void StylingSliderNegative(float value)
    {
        stylingSliderValueText.text = "-" + value;
    }

    private void StylingSliderPositiveDiscs(float value)
    {
        stylingSliderValueText.text = "+" + value / 100;
    }

    private void StylingSliderDegrees(float value)
    {
        stylingSliderValueText.text = value + "°";
    }

    private void StylingSliderMultiplication(float value)
    {
        stylingSliderValueText.text = "x" + value / 10;
    }

    private void StylingSliderNitro(float value)
    {
        if(value <= 0) stylingSliderValueText.text = "СТАНДАРТ";
        else stylingSliderValueText.text = value.ToString();
        priceText.text = nitroPrices[(int)value].ToString();
    }

    private string NumberSplit(int number)
    {
        string answer = "";
        for (int i = 0; number > 0; i++)
        {
            if (i % 3 <= 0)
            {
                answer = " " + answer;
            }
            answer = (number % 10).ToString() + answer;
            number = number / 10;
        }
        return answer;
    }

    public void Open(int balance)
    {
        SetBalance(balance);
        testServer.GetPerformanceData();
    }

    public void SetBalance(int balance)
    {
        balanceText.text = NumberSplit(balance);
        this.balance = balance;
    }

    public void AddPerformanceDetail(TuningDetails type, int currentLevel, int price, int donateUpgrades)
    {
        tuningLevels[type] = (currentLevel, price, donateUpgrades);
        if(type < TuningDetails.Max)
        {
            tuningButtons[(int)type].SetLevel(currentLevel);
            for (int i = 0; i < 4; i++)
            {
                int temp = 0;
                tuningCurrentSlider[i].value = 0;
                for(TuningDetails x = TuningDetails.Engine; x < TuningDetails.Max; x++)
                {
                    Debug.Log($"{x} {(int)x} {i}");
                    temp += tuningUpgradeValues[(int)x][i] * tuningLevels[x].level;
                }
                tuningCurrentSlider[i].value += temp;
                tuningCurrentText[i].text = tuningCurrentSlider[i].value.ToString();
            }
        }
    }

    public void PerformanceDetailComplete()
    {
        // OpenTuning();
    }

    public void AddBodyDetail(int id, int price)
    {
        bodyDetailsTemp.Add((id, price));
    }

    public void BodyDetailComplete()
    {
        bodyDetails = bodyDetailsTemp;
        bodyDetailsTemp = new();
        SetActiveContent(bodyContentObject);
    }

    public void SetColorPrice(int price)
    {
        colorPrice = price;
        SelectItem(colorPrice, stylingColorName[selectedColorType], () => BuyColor());
        buyGameObject.SetActive(true);
    }

    public void SetEditWheels(Dictionary<WheelEditType, int> prices)
    {
        editWheelsPrices = prices;
    }

    public void SetHydraulicsData(int installPrice, int removePrice, bool isInstalled)
    {
        hydraulicsInstallPrice = installPrice;
        hydraulicsRemovePrice = removePrice;
        isInstalledHydraulics = isInstalled;
        StylingHydraulicsPress(null);
    }

    public void SetNitroData(int currentLevel, int removePrice, int lvl1Price, int lvl2Price, int lvl3Price)
    {
        nitroPrices.Clear();
        nitroLevel = currentLevel;
        nitroPrices.Add(removePrice);
        nitroPrices.Add(lvl1Price);
        nitroPrices.Add(lvl2Price);
        nitroPrices.Add(lvl3Price);
    }

    public void SetSuspensionPrice(int price)
    {
        suspensionPrice = price;
    }

    public void SetVinyls(Dictionary<string, int> vinyls)
    {
        foreach(var i in vinyls)
        {
            Button temp = vinylSlider.addDetail();
            temp.onClick.AddListener(() => StylingVinylContentPress(temp, i.Key, i.Value));
        }
        vinylPrices = vinyls;
    }

    public void SetWheels(Dictionary<int, int> wheels)
    {
        foreach (KeyValuePair<int, int> i in wheels)
        {
            Button temp = wheelsSlider.addDetail();
            temp.onClick.AddListener(() => StylingDiscsContentPressed(temp, i.Key, i.Value));
        }
        wheelsPrices = wheels;
    }
}
