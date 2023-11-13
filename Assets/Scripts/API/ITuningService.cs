public interface ITuningService
{
    // general
    void GetPerformanceData();
        
    void BuyPerformanceDetail(TuningDetails type, BuyType buyType);
        
    // tuning body kits
    void GetDetails(VehicleTuningType type);
    void BuyBodyDetail(int id, VehicleTuningType type);
    // colors
    void GetColorsPrice(ColorsType type);
    void BuyColor(ColorsType type, int red, int green, int blue, int alpha);
        
    // wheels edit
    void GetEditWheels();
    void BuyEditWheels(WheelEditType type, float value);
        
    // hydraulics
    void GetHydraulicsData();
    void BuyHydraulic(bool isRemove);

    // nitro
    void GetNitroData();
    void BuyNitro(int lvl);

    // suspension
    void GetSuspension();
    void BuySuspension(float value);
        
    // vinyls
    void GetVinyls();
    void BuyVinyls(string name);
        
    // wheels
    void GetWheels();
    void BuyWheels(int id);
        
    void Close();
}
