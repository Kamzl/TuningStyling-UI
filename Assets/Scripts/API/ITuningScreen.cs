using System.Collections.Generic;

public interface ITuningScreen
{
    // general
    void Open(int balance);
    void SetBalance(int balance);
    void AddPerformanceDetail(TuningDetails type, int currentLevel, int price, int donateUpgrades);
    void PerformanceDetailComplete();
        
    // tuning body kits
    void AddBodyDetail(int id, int price);
    void BodyDetailComplete();
        
    // colors
    void SetColorPrice(int price);
        
    // wheels edit
    void SetEditWheels(Dictionary<WheelEditType,int> prices);
        
    // hydraulics
    void SetHydraulicsData(int installPrice, int removePrice, bool isInstalled);
        
    // nitro
    void SetNitroData(int currentLevel, int removePrice, int lvl1Price, int lvl2Price, int lvl3Price);
        
    // suspension
    void SetSuspensionPrice(int price);
        
    // vinyls
    void SetVinyls(Dictionary<string, int> vinyls);
        
    // wheels
    void SetWheels(Dictionary<int, int> wheels);
        
}
