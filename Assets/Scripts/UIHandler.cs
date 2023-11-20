

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour, ITuningScreen
{
    [SerializeField] private GameObject tuningGameObject;
    [SerializeField] private GameObject stylingGameObject;
    [SerializeField] private GameObject stylingScrollObject;
    [SerializeField] private SuperDuperService testServer;
    [SerializeField] private ContentSlider wheelsSlider;
    [SerializeField] private ContentSlider vinylSlider;
    [Space]
    [Header("Кнопки")]
    [SerializeField] private Button tuningButton;
    [SerializeField] private List<Button> tuningScroll;
    [SerializeField] private Button stylingButton;
    [SerializeField] private List<Button> stylingScroll;
    [SerializeField] private List<Button> bodyScroll;
    [SerializeField] private List<Button> discsCustomScroll;
    [SerializeField] private List<Button> tintScroll;
    [Space]
    [Header("Подтверждение покупки")]
    [SerializeField]
    private Button buyButton;
    [SerializeField] private Button buyDonateButton;
    [SerializeField] private GameObject buyAcceptObject;
    [SerializeField] private TextMeshProUGUI buyUpgradeName;
    [SerializeField] private TextMeshProUGUI buyPriceAcceptText;
    [SerializeField] private Button buyAcceptButton;
    [SerializeField] private Button buyCancelButton;
    [Space]
    [SerializeField]
    private List<TuningButton> tuningButtons;
    [SerializeField] private List<Slider> tuningCurrentSlider;
    [SerializeField] private List<TextMeshProUGUI> tuningCurrentText;
    [SerializeField] private List<Slider> tuningFutureSlider;
    [SerializeField] private List<TextMeshProUGUI> tuningFutureText;
    [SerializeField] private List<TextMeshProUGUI> tuningDifferenceText;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI buyButtonText;
    [SerializeField] private GameObject buyGameObject;
    [SerializeField] private GameObject buyDonateButtonGameObject;
    [SerializeField] private RectTransform buyTransform;
    [SerializeField] private GameObject bodyScrollObject;
    [SerializeField] private GameObject bodyContentObject;
    [SerializeField] private Button bodyArrowNext;
    [SerializeField] private Button bodyArrowPrevious;
    [SerializeField] private TextMeshProUGUI bodyPartNameText;
    [SerializeField] private GameObject colorWheel;
    [SerializeField] private GameObject transparencySlider;
    [SerializeField] private GameObject bodyObject;
    [SerializeField] private GameObject discsScroll;
    [SerializeField] private GameObject discsCustom;
    [SerializeField] private GameObject vinylScroll;
    [SerializeField] private GameObject tintObject;
    [SerializeField] private GameObject hydraulicsObject;
    [SerializeField] private TextMeshProUGUI hydraulicsText;
    [SerializeField] private Sprite activeButtonSprite;
    [SerializeField] private Sprite inactiveButtonSprite;
    [SerializeField] private Slider stylingSliderValue;
    [SerializeField] private GameObject stylingSliderObject;
    [SerializeField] private TextMeshProUGUI stylingSliderNameText;
    [SerializeField] private TextMeshProUGUI stylingSliderValueText;
    [SerializeField] private ColorCircle colorCircle;

    private int _balance = 0;
    private int _price = 0;
    private Action _onMoneySelect;

    private Dictionary<TuningDetails, (int level, int price, int donatePrice)> _tuningLevels;
    private readonly int[][] _tuningUpgradeValues = new int[(int)TuningDetails.Max][]
    {
        new [] {10, 5, 0, -5},
        new [] {5, 5, 0, -5},
        new [] {-5, 0, 5, 5},
        new [] {0, 0, 5, 5},
        new [] {5, 0, 5, 0},
        new [] {0, 10, 0, 0},
        new [] {0, 0, 0, 10},
    };
    private TuningDetails _currentTuningDetail;
    private int[] _tuningValues = new int[4];
    private bool _isInitialTuning = true;

    private readonly Dictionary<TuningDetails, string> _tuningUpgradeName = new Dictionary<TuningDetails, string>
    {
        { TuningDetails.Brakes, "Тормоза " },
        { TuningDetails.ChipTuning, "Чип-тюнинг " },
        { TuningDetails.Engine, "Двигатель " },
        { TuningDetails.Suspension, "Подвеска " },
        { TuningDetails.Tires, "Шины " },
        { TuningDetails.Transmission, "Трансмиссия " },
        { TuningDetails.Turbocharging, "Турбонаддув " }
    };

    private readonly Dictionary<VehicleTuningType, string> _stylingUpgradeName = new Dictionary<VehicleTuningType, string>
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

    private readonly Dictionary<ColorsType, string> _stylingColorName = new Dictionary<ColorsType, string>
    {
        { ColorsType.TonerFront, "Тонировка передних стекол" },
        { ColorsType.TonerSide, "Тонировка боковых стекол" },
        { ColorsType.TonerRear, "Тонировка задних стекол" },
        { ColorsType.Wheels, "Перекраска дисков" },
        { ColorsType.Body, "Перекраска кузова" },
        { ColorsType.Headlights, "Смена цвета фар" },
        { ColorsType.Neon, "Смена цвета неона" }
    };

    private readonly Dictionary<WheelEditType, string> _stylingDiscsName = new Dictionary<WheelEditType, string>
    {
        { WheelEditType.FrontOffset, "Вылет передних" },
        { WheelEditType.BackOffset, "Вылет задних" },
        { WheelEditType.FrontAlignment, "Развал передних" },
        { WheelEditType.BackAlignment, "Развал задних" },
        { WheelEditType.Width, "Ширина колес" }
    };
    private WheelEditType _currentEditWheelType;
    private float _currentEditWheelMultiplier;

    private readonly Vector2 _tuningBuyPos = new Vector2(-363, 265);
    private readonly Vector2 _stylingBuyPos = new Vector2(-363, 195);

    private Image _activeButtonImage;
    private Image _activeSecondaryButtonImage;
    private GameObject _activeContent;

    private int _bodyContentShownNum = -1;
    private VehicleTuningType _currentBodyDetailType;
    private List<(int id, int price)> _bodyDetails = new();
    private List<(int id, int price)> _bodyDetailsTemp = new();
    private readonly Color _yellowColor = new Color(1f, 213 / 255f, 84 / 255f);
    private readonly Color _purpleColor = new Color(154 / 255f, 118 / 255f, 1);
    private bool _selectedColorTransparency;
    private ColorsType _selectedColorType;
    private int[] _colorsPrices = new int[8];
    private ColorsType[] _transparentColors = new[] {ColorsType.TonerFront, ColorsType.TonerSide, ColorsType.TonerRear};

    private Dictionary<WheelEditType, int> _editWheelsPrices;

    private int _hydraulicsInstallPrice = 0;
    private int _hydraulicsRemovePrice = 0;
    private bool _isInstalledHydraulics;

    private int _nitroLevel = 0;
    private List<int> _nitroPrices;

    private int _suspensionPrice = 0;

    private readonly Color _inactiveColor = new Color(39 / 255f, 37 / 255f, 55 / 255f, 0.4f);
    private readonly Color _activeColor = new Color(39 / 255f, 37 / 255f, 55 / 255f, 0.8f);


    private void Awake()
    {
        _tuningLevels = new();
        _nitroPrices = new List<int>();
        _editWheelsPrices = new();

        _activeButtonImage = tuningScroll[0].GetComponent<Image>();

        tuningButton.onClick.AddListener(() => OpenTuning());
        stylingButton.onClick.AddListener(() => OpenStyling());

        buyButton.onClick.AddListener(() => ShowConfirmation());
        buyDonateButton.onClick.AddListener(() => ShowDonateConfirmation());
        buyCancelButton.onClick.AddListener(() => CloseConfirmation());

        bodyScroll[0].onClick.AddListener(() => OpenStyling());
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
            tintScroll[temp + 1].onClick.AddListener(() => StylingColorPress((ColorsType)temp, tintScroll[temp + 1]));
        }
        tintScroll[0].onClick.AddListener(() => OpenStyling());

        stylingScroll[0].onClick.AddListener(() => StylingBodyPressed());
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
    }

    private void OpenTuning()
    {
        for (int i = 0; i <= 3; i++)
        {
            tuningFutureSlider[i].value = 0;
            tuningFutureText[i].text = "";
            tuningDifferenceText[i].text = "";
        }
        buyTransform.anchoredPosition = _tuningBuyPos;
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
        buyTransform.anchoredPosition = _stylingBuyPos;
        buyDonateButtonGameObject.SetActive(false);
        discsCustom.SetActive(false);
        tintObject.SetActive(false);
        bodyObject.SetActive(false);
        stylingScrollObject.SetActive(true);
        SetActiveContent(null);
    }

    private void SetActiveButtonImage(Image image, Sprite sprite = null)
    {
        if(_activeButtonImage) _activeButtonImage.sprite = inactiveButtonSprite;
        if (image)
        {
            _activeButtonImage = image;
            _activeButtonImage.sprite = sprite ? sprite: activeButtonSprite;
        }
    }
    private void SetActiveSecondaryButtonImage(Image image)
    {
        if (_activeSecondaryButtonImage) _activeSecondaryButtonImage.color = _inactiveColor;
        if (image)
        {
            _activeSecondaryButtonImage = image;
            _activeSecondaryButtonImage.color = _activeColor;
        }
    }

    private void SetActiveContent(GameObject go)
    {
        if (_activeContent) _activeContent.SetActive(false);
        if (go)
        {
            _activeContent = go;
            _activeContent.SetActive(true);
        }
    }

    private void StylingButtonPress(Button button, GameObject content)
    {
        SetActiveButtonImage(button ? button.GetComponent<Image>() : null);
        SetActiveContent(content);
    }
    private void SelectItem(int price, string upgradeName, Action action, string buyButtonText = "КУПИТЬ")
    {
        if (action == null)
        {
            return;
        }
        string tempPriceText = NumberSplit(price);
        this.buyButtonText.text = buyButtonText;
        priceText.text = tempPriceText;
        buyGameObject.SetActive(true);
        this._price = price;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(ShowConfirmation);
        _onMoneySelect = () => {
            buyUpgradeName.text = upgradeName;
            buyPriceAcceptText.text = $"${tempPriceText}";
            buyPriceAcceptText.color = _yellowColor;
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
        if (_tuningLevels[_currentTuningDetail].donatePrice > 0 && _tuningLevels[_currentTuningDetail].level < 5)
        {
            _onMoneySelect?.Invoke();
            buyAcceptObject.SetActive(true);
        }
        buyPriceAcceptText.text = $"${0}";
        buyPriceAcceptText.color = _purpleColor;
        buyAcceptButton.onClick.RemoveAllListeners();
        buyAcceptButton.onClick.AddListener(() => 
        {
            testServer.BuyPerformanceDetail(_currentTuningDetail, BuyType.Donate);
            _tuningLevels[_currentTuningDetail] = (_tuningLevels[_currentTuningDetail].level, _tuningLevels[_currentTuningDetail].price, _tuningLevels[_currentTuningDetail].donatePrice - 1);
            buyAcceptObject.SetActive(false);
        });
        TuningScrollPress(_currentTuningDetail);
    }

    private void ShowConfirmation()
    {
        if(_price <= _balance)
        {
            _onMoneySelect?.Invoke();
            buyAcceptObject.SetActive(true);
        }
    }

    private void CloseConfirmation()
    {
        buyAcceptObject.SetActive(false);
    }

    private void TuningScrollPress(TuningDetails type)
    {
        string upgradeName = $"{_tuningUpgradeName[type]}{_tuningLevels[type].level + 1} уровня";
        int price = _tuningLevels[type].price;

        if (_tuningLevels[type].level < 5)
        {
            SelectItem(price, upgradeName, () => BuyTuningDetail(type));
            for (int i = 0; i < 4; i++)
            {
                int upgradeValue = _tuningUpgradeValues[(int)type][i];
                float futureValue = _tuningValues[i] + upgradeValue;
                tuningFutureSlider[i].value = futureValue;
                tuningFutureText[i].text = (futureValue).ToString();
                tuningDifferenceText[i].text = upgradeValue >= 0 ? $"+{upgradeValue}" : upgradeValue.ToString();
            }
        }
        else
        {
            buyGameObject.SetActive(false);
            SelectItem(price, upgradeName, null);
            for (int i = 0; i < 4; i++)
            {
                int upgradeValue = _tuningUpgradeValues[(int)type][i];
                float futureValue = upgradeValue;
                tuningFutureSlider[i].value = futureValue;
                tuningFutureText[i].text = "";
                tuningDifferenceText[i].text = "";
            }
        } 

        _currentTuningDetail = type;

        SetActiveButtonImage(tuningScroll[(int)type].GetComponent<Image>());

    }

    private void BuyTuningDetail(TuningDetails type)
    {
        testServer.BuyPerformanceDetail(type, BuyType.Money);
        TuningScrollPress(type);
    }

    private void StylingBodyPressed()
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
        _bodyContentShownNum = -1;
        bodyPartNameText.text = "СТАНДАРТНЫЙ";
        _currentBodyDetailType = type;
        testServer.GetDetails(type);
    }

    private void BodyContentArrowPressed(bool isForward)
    {
        _bodyContentShownNum += isForward ? 1 : -1;
        if (_bodyContentShownNum < -1) _bodyContentShownNum = _bodyDetails.Count - 1;
        else if (_bodyContentShownNum >= _bodyDetails.Count) _bodyContentShownNum = -1;
        if(_bodyContentShownNum != -1)
        {
            int id = _bodyDetails[_bodyContentShownNum].id;
            bodyPartNameText.text = $"№ {id}";
            SelectItem(_bodyDetails[_bodyContentShownNum].price, $"{_stylingUpgradeName[_currentBodyDetailType]}{id}", () => testServer.BuyBodyDetail(id, _currentBodyDetailType));
        }
        else
        {
            bodyPartNameText.text = "СТАНДАРТНЫЙ";
            RemoveSelectItem();
        }
    }

    private void StylingColorPress(ColorsType type, Button button)
    {
        RemoveSelectItem();
        
        _selectedColorType = type;

        SelectItem(_colorsPrices[(int)type], _stylingColorName[_selectedColorType], () => BuyColor());

        _selectedColorTransparency = _transparentColors.Contains(type);
        StylingButtonPress(button, colorWheel);
        transparencySlider.SetActive(_transparentColors.Contains(type));
    }

    private void BuyColor()
    {
        Vector4 temp = colorCircle.GetColor();
        temp *= 255;
        temp.w = _selectedColorTransparency ? temp.w : 255;
        testServer.BuyColor(_selectedColorType, (int)temp.x, (int)temp.y, (int)temp.z, (int)temp.w);
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
        SelectItem(price, $"{_stylingUpgradeName[VehicleTuningType.Wheels]}{name}", () => testServer.BuyWheels(name));
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
        _currentEditWheelType = type;
        switch (type)
        {
            case (WheelEditType.FrontOffset):
                ChangeSliderValues(0, 15, 0, StylingSliderPositiveDiscs);
                _currentEditWheelMultiplier = 0.01f;
                break;

            case (WheelEditType.BackOffset):
                ChangeSliderValues(0, 15, 0, StylingSliderPositiveDiscs);
                _currentEditWheelMultiplier = 0.01f;
                break;

            case (WheelEditType.FrontAlignment):
                ChangeSliderValues(0, 20, 0, StylingSliderDegrees);
                _currentEditWheelMultiplier = 1;
                break;

            case (WheelEditType.BackAlignment):
                ChangeSliderValues(0, 20, 0, StylingSliderDegrees);
                _currentEditWheelMultiplier = 1;
                break;

            case (WheelEditType.Width):
                ChangeSliderValues(5, 15, 10, StylingSliderMultiplication);
                _currentEditWheelMultiplier = 0.1f;
                break;
        }
        stylingScrollObject.SetActive(false);
        SelectItem(_editWheelsPrices[type], $"{_stylingDiscsName[type]} {_currentEditWheelMultiplier * stylingSliderValue.value}", () => BuyDiscsCustom());
        buyButton.onClick.AddListener(() => BuyDiscsCustomPress());

        buyGameObject.SetActive(true);
    }

    private void BuyDiscsCustomPress()
    {
        buyUpgradeName.text = $"{_stylingDiscsName[_currentEditWheelType]} {_currentEditWheelMultiplier * stylingSliderValue.value}";
    }
    private void BuyDiscsCustom()
    {
        testServer.BuyEditWheels(_currentEditWheelType, _currentEditWheelMultiplier * stylingSliderValue.value);
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
        SelectItem(_suspensionPrice, $"{_tuningUpgradeName[TuningDetails.Suspension]} -{stylingSliderValue.value}", () => BuyDiscsCustom());
        buyButton.onClick.AddListener(BuySuspensionPress);
    }

    private void BuySuspensionPress()
    {
        buyUpgradeName.text = $"{_tuningUpgradeName[TuningDetails.Suspension]} -{stylingSliderValue.value}";
    }


    private void StylingNitroPress(Button button)
    {
        StylingButtonPress(button, stylingSliderObject);
        stylingSliderNameText.text = "УРОВЕНЬ";
        ChangeSliderValues(0, 3, 0, StylingSliderNitro);
        SelectItem(_nitroPrices[_nitroLevel], $"Нитро уровень {stylingSliderValue.value}", () => BuyDiscsCustom());
        buyButton.onClick.AddListener(BuyNitroPress);
    }

    private void BuyNitroPress()
    {
        buyUpgradeName.text = $"Нитро уровень {stylingSliderValue.value}";
        buyPriceAcceptText.text = $"${_nitroPrices[(int)stylingSliderValue.value]}";
    }

    private void StylingHydraulicsPress(Button button)
    {
        if(button) StylingButtonPress(button, hydraulicsObject);
        if (_isInstalledHydraulics)
        {
            hydraulicsText.text = "УСТАНОВЛЕНА";
            if (!_isInitialTuning)
            {
                SelectItem(_hydraulicsRemovePrice, "Снятие гидравлики", () => testServer.BuyHydraulic(true), "СНЯТЬ");
            }
        }
        else
        {
            hydraulicsText.text = "ОТСУТСТВУЕТ";
            if (!_isInitialTuning)
            {
                SelectItem(_hydraulicsInstallPrice, "Установка гидравлики", () => testServer.BuyHydraulic(false), "УСТАНОВИТЬ");
            }
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
        priceText.text = _nitroPrices[(int)value].ToString();
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
        _isInitialTuning = true;
        SetBalance(balance);
        testServer.GetSuspension();
        testServer.GetNitroData();
        testServer.GetHydraulicsData();
        testServer.GetEditWheels();
        testServer.GetWheels();
        testServer.GetVinyls();
        _selectedColorType = ColorsType.TonerFront;
        testServer.GetColorsPrice(_selectedColorType);
        testServer.GetPerformanceData();
    }

    public void SetBalance(int balance)
    {
        balanceText.text = NumberSplit(balance);
        this._balance = balance;
    }

    public void AddPerformanceDetail(TuningDetails type, int currentLevel, int price, int donateUpgrades)
    {
        _tuningLevels[type] = (currentLevel, price, donateUpgrades);
        if(type < TuningDetails.Max)
        {
            tuningButtons[(int)type].SetLevel(currentLevel);
            for (int i = 0; i < 4; i++)
            {
                int temp = 0;
                tuningCurrentSlider[i].value = 0;
                _tuningValues[i] = 0;
                for(TuningDetails x = TuningDetails.Engine; x < TuningDetails.Max; x++)
                {
                    temp += _tuningUpgradeValues[(int)x][i] * _tuningLevels[x].level;
                }
                tuningCurrentSlider[i].value += temp;
                _tuningValues[i] += temp;
                tuningCurrentText[i].text = _tuningValues[i].ToString();
            }
        }

        if (!_isInitialTuning)
        {
            TuningScrollPress(type);
        }
    }

    public void PerformanceDetailComplete()
    {
        _isInitialTuning = false;
    }

    public void AddBodyDetail(int id, int price)
    {
        _bodyDetailsTemp.Add((id, price));
    }

    public void BodyDetailComplete()
    {
        _bodyDetails = _bodyDetailsTemp;
        _bodyDetailsTemp = new();
        SetActiveContent(bodyContentObject);
    }

    public void SetColorPrice(int price)
    {
        _colorsPrices[(int)_selectedColorType] = price;
        if (_selectedColorType < ColorsType.None)
        {
            _selectedColorType++;
            testServer.GetColorsPrice(_selectedColorType);
        }
    }

    public void SetEditWheels(Dictionary<WheelEditType, int> prices)
    {
        _editWheelsPrices = prices;
    }

    public void SetHydraulicsData(int installPrice, int removePrice, bool isInstalled)
    {
        _hydraulicsInstallPrice = installPrice;
        _hydraulicsRemovePrice = removePrice;
        _isInstalledHydraulics = isInstalled;
        StylingHydraulicsPress(null);
    }

    public void SetNitroData(int currentLevel, int removePrice, int lvl1Price, int lvl2Price, int lvl3Price)
    {
        _nitroPrices.Clear();
        _nitroLevel = currentLevel;
        _nitroPrices.Add(removePrice);
        _nitroPrices.Add(lvl1Price);
        _nitroPrices.Add(lvl2Price);
        _nitroPrices.Add(lvl3Price);
    }

    public void SetSuspensionPrice(int price)
    {
        _suspensionPrice = price;
    }

    public void SetVinyls(Dictionary<string, int> vinyls)
    {
        foreach(var i in vinyls)
        {
            Button temp = vinylSlider.AddDetail();
            temp.onClick.AddListener(() => StylingVinylContentPress(temp, i.Key, i.Value));
        }
    }

    public void SetWheels(Dictionary<int, int> wheels)
    {
        foreach (KeyValuePair<int, int> i in wheels)
        {
            Button temp = wheelsSlider.AddDetail();
            temp.onClick.AddListener(() => StylingDiscsContentPressed(temp, i.Key, i.Value));
        }
    }
}
