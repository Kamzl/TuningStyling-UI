using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum TuningDetails
    {
        Engine,
        Turbocharging,
        Suspension,
        Tires,
        Transmission,
        ChipTuning,
        Brakes,
        Max
    }

    public enum BuyType
    {
        Money,
        Donate
    }

    public enum WheelEditType
    {
        FrontOffset, // Вылет передних колес  0 - 0.15 шаг 0.01
        BackOffset, // Вылет задних колес 0 - 0.15 шаг 0.01

        FrontAlignment, // Развал передних колес 0 - 20 шаг 1
        BackAlignment, // Развал задних колес 0 - 20 шаг 1

        Width, // Ширина 0.5 - 1.5 шаг 0.1

        None
    }

    public enum VehicleTuningType
    {
        Spoiler,
        Hood,
        Roof,
        SideSkirt,
        Lamps,
        Nitro,
        Exhaust,
        Wheels,
        Stereo,
        Hydraulics,
        FrontBumper,
        RearBumper,
        VentRight,
        VentLeft,

        None
    }

    public enum ColorsType
    {
        TonerFront, // Тонер передних стекол
        TonerSide, // Тонер боковых стекол
        TonerRear, // Тонер задних стекол
        Wheels, // Колеса
        Body, // Кузов
        Headlights, // Фары
        Neon,
        None,
    }
    
    public class SuperDuperService : MonoBehaviour, ITuningService
    {
        public static ITuningService Instance { get; private set; }

        private ITuningScreen _tuningScreen;

        private List<(DateTime, Action)> _delayedActions = new List<(DateTime, Action)>();
        
        private void Awake()
        {
            Instance = this;
            _tuningScreen = FindObjectsOfType<MonoBehaviour>(true).OfType<ITuningScreen>().FirstOrDefault();

            foreach (var type in Enum.GetValues(typeof(TuningDetails)).Cast<TuningDetails>())
            {
                _performanceData.Add(type, Random.Range(0, 3));
            }
            
            _wheelsSettings[WheelEditType.FrontOffset] = 0f;
            _wheelsSettings[WheelEditType.BackOffset] = 0f;
            _wheelsSettings[WheelEditType.FrontAlignment] = 0f;
            _wheelsSettings[WheelEditType.BackAlignment] = 0f;
            _wheelsSettings[WheelEditType.Width] = 1f;

            
            var detailId = 1;
            for (var dt = 0; dt < (int)VehicleTuningType.None; dt++)
            {
                var detailType = (VehicleTuningType)dt;

                var basePrice = (dt + 1) * 1000;
                var count = Random.Range(2, 7);
                for (var i = 0; i < count; i++)
                    _bodyDetails.Add((detailType, detailId++, basePrice + i * 100));
            }
            
            var vinylPrice = 1000;
            for (var i = 1; i <= 25; i++)
            {
                _vinyls.Add("Vinyl " + i, vinylPrice);
                vinylPrice += 100;
            }

            var wheelPrice = 1000;
            for (var i = 1; i <= 30; i++)
            {
                _wheels.Add(i, wheelPrice);
                wheelPrice += 100;
            }
            
        }

        private void Start()
        {
            DelayAction(3000,() =>
            {
                _tuningScreen?.Open(balance);
            });
        }

        private void DelayAction(int ms, Action act)
        {
            _delayedActions.Add((DateTime.Now.AddMilliseconds(ms),act));
        }
        
        private void DelayAction(Action act) => DelayAction(DefaultDelay, act);
        
        private const int DefaultDelay = 1000;

        private void Update()
        {
            var actions = _delayedActions.ToArray();
            foreach (var (time, act) in actions)
            {
                if (time <= DateTime.Now)
                {
                    try
                    {
                        act();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                    _delayedActions.Remove((time, act));
                }
            }
        }


        private Dictionary<TuningDetails, int> _performanceData = new Dictionary<TuningDetails, int>();
        private Dictionary<WheelEditType, float> _wheelsSettings = new Dictionary<WheelEditType, float>();
        private List<(VehicleTuningType type, int id, int price)> _bodyDetails = new List<(VehicleTuningType type, int id, int price)>();
        private Dictionary<string, int> _vinyls = new Dictionary<string, int>();
        private Dictionary<int, int> _wheels = new Dictionary<int, int>();
        private readonly int[] _prices = { 100, 200, 300, 400, 500, 600, 700, 800, 900, 10000 };
        [SerializeField] private int balance = 1000;
        [SerializeField] private int donate = 1000;
        private bool _isHydraulicsInstalled = false;    
        private int _nitroLevel = 0;

        private bool SpendMoney(int money)
        {
            if(balance < money) return false;
            balance -= money;
            _tuningScreen?.SetBalance(balance);
            return true;
        }
        
        private void SpendDonate(int donate)
        {
            this.donate -= donate;
        }
        
        private int GetColorPriceInternal(ColorsType type)
        {
            return type switch
            {
                ColorsType.TonerFront or ColorsType.TonerSide or ColorsType.TonerRear => 100,
                ColorsType.Wheels => 500,
                ColorsType.Body => 1000,
                ColorsType.Headlights => 200,
                ColorsType.Neon => 1500,
                _ => 321
            };
        }
        
        private int GetEditWheelPriceInternal(WheelEditType type)
        {
            return type switch
            {
                WheelEditType.FrontOffset or WheelEditType.BackOffset => 100,
                WheelEditType.FrontAlignment or WheelEditType.BackAlignment => 500,
                WheelEditType.Width => 1000,
                _ => 321
            };
        }

        public void GetPerformanceData() => DelayAction(() =>
        {
            void SendData(TuningDetails type, int ms) => DelayAction(ms, () =>
            {
                var lvl = _performanceData[type];
                
                _tuningScreen?.AddPerformanceDetail(type, lvl, _prices[lvl], ((int) type & 1) + 2);
            });

            int delay = 150;
            foreach (var type in Enum.GetValues(typeof(TuningDetails)).Cast<TuningDetails>())
            {
                SendData(type, delay);
                delay += 40;
            }
            
            DelayAction(delay, () => _tuningScreen?.PerformanceDetailComplete());
        });
        
        public void BuyPerformanceDetail(TuningDetails type, BuyType buyType) 
        {
            Assert.IsTrue(SpendMoney(_prices[_performanceData[type]]));
            _performanceData[type]++;
            _tuningScreen?.AddPerformanceDetail(type, _performanceData[type], _prices[_performanceData[type]], ((int) type & 1) + 2);
            _tuningScreen?.PerformanceDetailComplete();
            Debug.Log($"BuyPerformanceDetail {type} {buyType}");
        }
        
        public void GetDetails(VehicleTuningType type) 
        {
            DelayAction(() =>
            {
                var details = _bodyDetails.Where(x => x.type == type).ToArray();
                
                var delay = 100;
                foreach (var (detailType, id, price) in details)
                {
                    DelayAction(delay, () => _tuningScreen?.AddBodyDetail(id, price));
                    delay += 40;
                }
                
                DelayAction(delay, () => _tuningScreen?.BodyDetailComplete());
            });
        }
        
        public void BuyBodyDetail(int id, VehicleTuningType type) 
        {
            var detail = _bodyDetails.FirstOrDefault(x => x.id == id);
            Assert.IsTrue(detail != default);
            Assert.IsTrue(SpendMoney(detail.price));
            Debug.Log($"BuyBodyDetail {id} {type}");
        }

        public void GetColorsPrice(ColorsType type) => DelayAction(() =>
        {
            var price = GetColorPriceInternal(type);
            
            _tuningScreen?.SetColorPrice(price);
        });
        
        public void BuyColor(ColorsType type, int red, int green, int blue, int alpha) 
        {
            var price = GetColorPriceInternal(type);
            Assert.IsTrue(SpendMoney(price));
            Debug.Log($"BuyColor {type} {red} {green} {blue} {alpha}");
        }
        
        public void GetEditWheels() 
        {
            DelayAction(() =>
            {
                _tuningScreen?.SetEditWheels(new Dictionary<WheelEditType, int>
                {
                    { WheelEditType.FrontOffset, GetEditWheelPriceInternal(WheelEditType.FrontOffset) },
                    { WheelEditType.BackOffset, GetEditWheelPriceInternal(WheelEditType.BackOffset) },
                    { WheelEditType.FrontAlignment, GetEditWheelPriceInternal(WheelEditType.FrontAlignment) },
                    { WheelEditType.BackAlignment, GetEditWheelPriceInternal(WheelEditType.BackAlignment) },
                    { WheelEditType.Width, GetEditWheelPriceInternal(WheelEditType.Width) },
                });
            });
        }
        
        public void BuyEditWheels(WheelEditType type, float value) 
        {
            var price = GetEditWheelPriceInternal(type);
            Assert.IsTrue(SpendMoney(price));
            _wheelsSettings[type] = value;
        }

        public void GetHydraulicsData() => DelayAction(() =>
        {
            if (_isHydraulicsInstalled)
            {
                _tuningScreen?.SetHydraulicsData(100, 5000, _isHydraulicsInstalled);
            }
            else
            {
                _tuningScreen?.SetHydraulicsData(1000, 500, _isHydraulicsInstalled);
            }
        });
        
        public void BuyHydraulic(bool isRemove) 
        {
            Debug.Log($"{isRemove} {_isHydraulicsInstalled}");
            if(isRemove != _isHydraulicsInstalled) return;
            if(isRemove)
                Assert.IsTrue(SpendMoney(5000));
            else
                Assert.IsTrue(SpendMoney(1000));
            _isHydraulicsInstalled = !_isHydraulicsInstalled;
            
            GetHydraulicsData();
        }
        
        public void GetNitroData() => DelayAction(() =>
        {
            _tuningScreen?.SetNitroData(_nitroLevel, 500, 100,200,300);
        });
        
        public void BuyNitro(int lvl) 
        {
            Assert.IsTrue(lvl != _nitroLevel);
            Assert.IsTrue(SpendMoney(1000));
            _nitroLevel = lvl;
            GetNitroData();
        }
        
        public void GetSuspension() => DelayAction(() =>
        {
            _tuningScreen?.SetSuspensionPrice(1000);
        });
        
        public void BuySuspension(float value) 
        {
            Assert.IsTrue(SpendMoney(1000));
            GetSuspension();
        }

        public void GetVinyls()
        {
            DelayAction(() =>
            {
                _tuningScreen?.SetVinyls(new Dictionary<string, int>(_vinyls));
            });
        }
        
        public void BuyVinyls(string name) 
        {
            Assert.IsTrue(SpendMoney(_vinyls[name]));
            Debug.Log($"BuyVinyls {name}");
        }
        
        public void GetWheels() => DelayAction(() =>
        {
            _tuningScreen?.SetWheels(new Dictionary<int, int>(_wheels));
        });
        
        public void BuyWheels(int id) 
        {
            Assert.IsTrue(SpendMoney(_wheels[id]));
            Debug.Log($"BuyWheels {id}");
        }
        
        public void Close() 
        {
            Debug.Log("Close");
        }
    }
